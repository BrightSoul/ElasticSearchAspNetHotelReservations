using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class SpecialPrice
    {

        //For Entity Framework
        private SpecialPrice()
        {

        }

        public SpecialPrice(decimal price, Room room, DateTime date)
        {
            if (date.Date < date)
                throw new ArgumentException("The time part of the 'from' date must be zero");

            if (room == null)
                throw new ArgumentNullException(nameof(room));

            Room = room;
            Date = date;
            UpdatePrice(price);
        }

        [Key, Column(Order = 1)]
        public DateTime Date { get; private set; }
        [Key, Column(Order = 2)]
        public int RoomId { get; private set; }
        public decimal Price { get; private set; }
        public virtual Room Room { get; private set; }

        [NotMapped]
        public byte Discount {
            get {
                return Convert.ToByte(IsDiscounted ?
                    (1 - (Price / Room.BasePrice)) * 100 :
                    0);
            }
        }

        [NotMapped]
        public bool IsDiscounted
        {
            get
            {
                return Price < Room.BasePrice;
            }
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("The price must be greater than zero");

            Price = newPrice;
        }
        
    }
}
