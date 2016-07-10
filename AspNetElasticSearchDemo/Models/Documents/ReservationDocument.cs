using AspNetElasticSearchDemo.Models.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Documents
{
    [ElasticsearchType]
    public class ReservationDocument
    {

        private ReservationDocument()
        {

        }


        //Prepare the document by denormalizing the entity tree
        public static ReservationDocument FromReservation(Reservation reservation)
        {
            //I don't need to index the name of the person who reserved this room
            //I woulnd't use that information in the search function anyway

            //I don't need to store the room id either, since that's used during indexing,
            //as the parent index of this document
            return new ReservationDocument
            {
                Date = reservation.Date
            };
        }

        [Date(Store = true, NumericResolution = NumericResolutionUnit.Seconds, Similarity = SimilarityOption.Default)]
        public DateTime Date { get; set; }

    }
}
