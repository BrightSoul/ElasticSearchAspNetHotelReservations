using AspNetElasticSearchDemo.Models;
using AspNetElasticSearchDemo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Services
{
    public class RandomDiscountEngine
    {
        public void ApplyRules(IEnumerable<Room> rooms)
        {
            // As a simple business rule, web apply a random discount to dates that have a special price
            var random = new Random();
            foreach (var room in rooms)
            {
                foreach (var specialPrice in room.SpecialPrices)
                {
                    // Set a random discount between 5 and 30 euros
                    var discount = random.Next(5, 26);
                    specialPrice.UpdatePrice(room.BasePrice - discount);
                }
            }
        }
    }
}
