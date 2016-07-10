using AspNetElasticSearchDemo.Models.Documents;
using AspNetElasticSearchDemo.Models.DTOs;
using AspNetElasticSearchDemo.Models.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Services
{
    public class ElasticSearchIndexManager
    {

        private readonly ElasticClient client;
        private readonly string indexName;
        private static ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        public ElasticSearchIndexManager() : this(ConfigurationManager.AppSettings["ElasticSearchEndpoint"], ConfigurationManager.AppSettings["ElasticSearchIndexName"])
        {

        }
        public ElasticSearchIndexManager(string endpoint, string indexName)
        {
            var node = new Uri(endpoint);
            var connSettings = new ConnectionSettings(node);
            this.client = new ElasticClient(connSettings);
            this.indexName = indexName;

        }
        private void DeleteIndex()
        {
          var response = client.IndexExists(indexName);
            if (response.Exists)
                client.DeleteIndex(indexName);
        }

        private void CreateIndexIfNotExists()
        {


            var existsResponse = client.IndexExists(indexName);


            //if it doesn't exist, it must be created
            if (!existsResponse.Exists)
            {
                var creationResponse = client.CreateIndex(indexName, descriptor => descriptor
                    .Settings(settings => settings.NumberOfReplicas(0).NumberOfShards(1))
                    .Mappings(mappings => mappings
                    .Map<RoomDocument>(selector => selector.AutoMap())
                    .Map<SpecialPriceDocument>(selector => selector.AutoMap().Parent<RoomDocument>())
                    .Map<ReservationDocument>(selector => selector.AutoMap().Parent<RoomDocument>())
                    )
                );
                if (!creationResponse.IsValid)
                    throw new InvalidOperationException("Error creating the index: " + creationResponse.ServerError);
            }
        }

        public SuggestResult Suggest(string text)
        {
            var suggestResponse = client.Suggest<RoomDocument>(suggest => suggest.GlobalText(text).Index(indexName)
                .Completion("Rooms", descriptor => descriptor.Field(r => r.RoomNameCompletion))
                .Completion("Hotels", descriptor => descriptor.Field(r => r.HotelNameCompletion))
                );
            return new SuggestResult { Suggestions = suggestResponse.Suggestions.Select(s => SuggestionDTO.FromSuggestion(s)).Where(s => s.Terms.Any()) };
        }

        public SearchResult Search(DateTime arrival, int nights, decimal? maxPrice = null, IEnumerable<TermFilterDTO> terms = null, string text = null, int offset = 0)
        {
            var sw = new Stopwatch();
            sw.Start();
            slimLock.EnterReadLock();

            var departure = arrival.AddDays(nights);


            var searchResponse = client.Search<RoomDocument>(descriptor =>
            descriptor
                .Index(indexName)
                .From(offset)
                .Size(10)
                .Aggregations(aggregations => {


                    // We don't need aggregations when we're loading more pages of the
                    // search we did earliear, since we already got that information
                    if (offset > 0)
                        return aggregations;

                    return aggregations
                    .Terms("hotelName", aggr => aggr.Field("hotelName"))
                    .Terms("services", aggr => aggr.Field("services"))
                    .Terms("bedTypes", aggr => aggr.Field("bedTypes"))
                    //.Terms("IsDiscounted", aggr => aggr.Field("isDiscounted"))
                    .Terms("totalPeople", aggr => aggr.Field("totalPeople"))
                    //.Range("priceRange", aggr => aggr.Field("price").Ranges(range => range.To(70), range => range.From(70).To(90), range => range.From(90).To(110), range => range.From(110)))
                    ;
                    }
                    )
                .Query(query =>
                        query.FunctionScore(score =>
                            score.BoostMode(FunctionBoostMode.Sum).Functions(function => function.FieldValueFactor(value => value.Field("rating")))
                                .Query(scoreQuery => scoreQuery.Bool(boolquery =>
                                {
                                    var must = new List<Func<QueryContainerDescriptor<RoomDocument>, QueryContainer>>();
                                    var must_not = new List<Func<QueryContainerDescriptor<RoomDocument>, QueryContainer>>();
                                    var should = new List<Func<QueryContainerDescriptor<RoomDocument>, QueryContainer>>();
                                    var filter = new List<Func<QueryContainerDescriptor<RoomDocument>, QueryContainer>>();
                                    int minimunShouldMatch = 0;

                                    must_not.Add(
                                        mustNot => mustNot.HasChild<ReservationDocument>(hasChild => hasChild
                                                .Query(childQuery => childQuery.DateRange(childDateRange => childDateRange.Field(reservation => reservation.Date).GreaterThanOrEquals(arrival).LessThan(departure))
                                        ))
                                        );


                                    if (terms != null && terms.Any())
                                    {
                                        foreach (var term in terms)
                                        {
                                            filter.Add(filterQuery => filterQuery.Term(mustTerm => mustTerm.Field(term.Aggregation).Value(term.Term).Verbatim()));
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        text = text.TrimEnd('*') + '*';
                                        should.Add(shouldQuery => shouldQuery.QueryString(queryString => queryString.Boost(2.0).Query(text).Fields(fields => fields.Field("roomName", 2.0).Field("hotelNameAnalyzed", 5.0))));

                                        //If you wanted to do a fuzzy query you should do this instead
                                        //should.Add(shouldQuery => shouldQuery.Match(match => match.Boost(5.0).Field(d => d.HotelNameAnalyzed).Fuzziness(Fuzziness.EditDistance(6)).Query(text)));
                                        //should.Add(shouldQuery => shouldQuery.Match(match => match.Boost(2.0).Field(d => d.RoomName).Fuzziness(Fuzziness.EditDistance(6)).Query(text)));
                                        minimunShouldMatch++;
                                    }

                                    if (maxPrice.HasValue)
                                    {
                                        filter.Add(filterQuery => filterQuery.Range(range => range.Field(room => room.Price).LessThanOrEquals(Convert.ToDouble(maxPrice.Value))));

                                    }

                                    return boolquery.Must(must).MustNot(must_not).Should(should).Filter(filter).MinimumShouldMatch(MinimumShouldMatch.Fixed(minimunShouldMatch));
                                }
                ))))
            );
            slimLock.ExitReadLock();
            sw.Stop();
            var result = new SearchResult
            {
                Aggregations = searchResponse.Aggregations.Select(aggr => new AggregationDTO { Aggregation = aggr.Key, Terms = (aggr.Value as BucketAggregate).Items.Select(bucket => TermDTO.FromBucket(bucket)).Where(dto => dto != null && dto.DocumentCount > 0) }),
                TotalResults = searchResponse.Total,
                QueryDuration = searchResponse.Took,
                RequestDuration = sw.ElapsedMilliseconds,
                Hits = searchResponse.Hits.Select(hit => new ResultDTO { Room = hit.Source, Score = Convert.ToDecimal(hit.Score) })
            };

            return result;
        }

        public void UpdateIndex(IEnumerable<Room> rooms)
        {
            //Pessimistic approach: dropping/recreting the index will do for this demo
            //You should consider updating it instead
            slimLock.EnterWriteLock();
            DeleteIndex();
            CreateIndexIfNotExists();

            if (rooms.Any())
            {

                var bulkResponse = client.Bulk(descriptor =>
                {
                    foreach (var room in rooms)
                    {
                        var roomDocument = RoomDocument.FromRoom(room);
                        descriptor.Index<RoomDocument>(doc => doc.Index(indexName).Document(roomDocument).Id(new Id(roomDocument.RoomId)));
                        foreach (var specialPrice in room.SpecialPrices)
                        {
                            var specialPriceDocument = SpecialPriceDocument.FromSpecialPrice(specialPrice);
                            descriptor.Index<SpecialPriceDocument>(doc => doc.Index(indexName).Document(specialPriceDocument).Parent(new Id(roomDocument.RoomId)));
                        }
                        foreach (var reservation in room.Reservations)
                        {
                            var reservationDocument = ReservationDocument.FromReservation(reservation);
                            descriptor.Index<ReservationDocument>(doc => doc.Index(indexName).Document(reservationDocument).Parent(new Id(roomDocument.RoomId)));
                        }
                    }

                    return descriptor;
                });
                slimLock.ExitWriteLock();
                if (!bulkResponse.IsValid)
                    throw new InvalidOperationException("Error adding rooms to index: " + bulkResponse.ServerError.Error.Reason);

            } else
            {
                slimLock.ExitWriteLock();
            }
            
        }
    }
}
