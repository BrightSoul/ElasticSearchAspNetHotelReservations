using AspNetElasticSearchDemo.Models;
using AspNetElasticSearchDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AspNetElasticSearchDemo.Models.Entities;

namespace AspNetElasticSearchDemo
{
    public static class ElasticSearchIndexingJob
    {
        public static void Execute()
        {

            // Get all the reservables dates (just a demo, eh?)
            List<Room> rooms;
            using (var context = new HotelReservationContext())
            {
                rooms = context.Rooms
                    .Include(room => room.Services)
                    .Include(room => room.Reservations)
                    .Include(room => room.SpecialPrices)
                    .Include(room => room.Beds)
                    .Include(room => room.Hotel)
                    .ToList();
            }

            //Apply business rules
            var engine = new RandomDiscountEngine();
            engine.ApplyRules(rooms);

            //Reindex. I drop and recreate the index, beware the downtime
            var indexManager = new ElasticSearchIndexManager();
            //Prepare documents (ReservableDateDocument here also acts a mapper/denormalizer)
            indexManager.UpdateIndex(rooms);

        }
    }
}
