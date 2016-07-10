using AspNetElasticSearchDemo.Models.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Documents
{
    [ElasticsearchType(IdProperty = nameof(RoomId))]
    public class RoomDocument
    {

        private RoomDocument()
        {

        }


        //Prepare the document by denormalizing the entity tree
        public static RoomDocument FromRoom(Room room)
        {
            return new RoomDocument
            {
                RoomId = "Room" + room.RoomId.ToString(),
                BedTypes = room.Beds.SelectMany(bed => Enumerable.Repeat(bed.Kind.ToString(), bed.Quantity)).ToArray(),
                Price = room.BasePrice,
                Rating = room.Rating,
                Services = room.Services.Select(service => service.Name).ToArray(),
                RoomName = room.Name,
                RoomNameCompletion = room.Name,
                HotelName = room.Hotel.Name,
                HotelNameAnalyzed = room.Hotel.Name,
                HotelNameCompletion = room.Hotel.Name,
                TotalPeople = room.Beds.Sum(bed => bed.TotalPeople),
                Image = room.Image
            };
        }

        [Date(Store = true, NumericResolution = NumericResolutionUnit.Seconds, Similarity = SimilarityOption.Default)]
        public DateTime Date { get; set; }
        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string RoomId { get; set; }
        [String(Analyzer = "simple", Index = FieldIndexOption.Analyzed, Store = true, Similarity = SimilarityOption.BM25)]
        public string RoomName { get; set; }

        [Completion]
        public string RoomNameCompletion { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Image { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed, Store = true, Similarity = SimilarityOption.BM25)]
        public string HotelName { get; set; }
        [String(Analyzer = "simple", Index = FieldIndexOption.Analyzed, IncludeInAll = false, Store = true, Similarity = SimilarityOption.BM25)]
        public string HotelNameAnalyzed { get; set; }
        [Completion]
        public string HotelNameCompletion { get; set; }
        public bool IsDiscounted { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }

        [Number(Store = false, Index = NonStringIndexOption.NotAnalyzed)]
        public decimal Rating { get; set; }

        [Number(Store = false, Index = NonStringIndexOption.NotAnalyzed)]
        public int TotalPeople { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed, Store = true, IncludeInAll = false, TermVector = TermVectorOption.Yes)]
        public string[] BedTypes { get; set; }
        [String(Index = FieldIndexOption.NotAnalyzed, Store = true, IncludeInAll = false, TermVector = TermVectorOption.Yes)]
        public string[] Services { get; set; }
        
    }
}
