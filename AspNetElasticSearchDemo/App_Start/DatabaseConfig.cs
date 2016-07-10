using AspNetElasticSearchDemo.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo
{
    public static class DatabaseConfig
    {
        public static void Configure()
        {
            Database.SetInitializer(new HotelReservationInitializer());
            using (var context = new HotelReservationContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}
