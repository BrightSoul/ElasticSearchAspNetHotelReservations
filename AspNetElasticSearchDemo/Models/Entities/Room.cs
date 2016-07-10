using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class Room
    {
        
        private Room()
        {
            Beds = new HashSet<BedAllocation>();
            Reservations = new HashSet<Reservation>();
            SpecialPrices = new HashSet<SpecialPrice>();
            Services = new HashSet<RoomService>();
        }
        public Room(string name, string image = null) : this()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("This room must have a name");

            Name = name;
            Image = image;
        }
        [Key]
        public int RoomId { get; private set; }

        public int HotelId { get; private set; }
        [StringLength(100)]
        public string Name { get; private set; }
        [StringLength(100)]
        public string Image { get; private set; }
        public virtual Hotel Hotel { get; private set; }
        public decimal Rating { get; private set; }
        public decimal BasePrice { get; private set; }
        public virtual ICollection<BedAllocation> Beds { get; private set; }
        public virtual ICollection<Reservation> Reservations { get; private set; }
        public virtual ICollection<SpecialPrice> SpecialPrices { get; private set; }
        public virtual ICollection<RoomService> Services { get; private set; }

        public Room WithBeds(BedKind kind, int quantity) {
            Beds.Add(new BedAllocation(kind, quantity));
            return this;
        }
        public Room WithServices(params RoomService[] services)
        {
            foreach (var service in services)
                Services.Add(service);
            return this;
        }

        public Room WithReservation(string personName, DateTime arrival, int nights = 1)
        {
            if (nights < 1)
                throw new ArgumentException("A person must stay at least 1 night");

            if (string.IsNullOrEmpty(personName))
                throw new ArgumentNullException(nameof(personName));

            for (var i = 1; i<=nights; i++)
            {
                Reservations.Add(new Reservation(arrival.AddDays(i - 1), this, personName));
            }

            return this;
        }
        
        //Of course you don't set the rating by hand, but for this demo this implementation will do
        public Room HasRating(decimal averageRating)
        {
            if (averageRating > 5.0m || averageRating <= 0m)
                throw new ArgumentException("Rating is out of range. Allowed values are between 0 and 5 (inclusive)");

            Rating = averageRating;
            return this;
        }

        public Room HasBasePrice(decimal basePrice)
        {
            if (basePrice <= 0m)
                throw new ArgumentException("This room can't be free");

            BasePrice = basePrice;
            return this;
        }

        public Room HasSpecialPrice(decimal specialPrice, DateTime from, int nights = 1)
        {

            if (specialPrice <= 0m)
                throw new ArgumentException("This room can't be free");

            if (nights < 1)
                throw new ArgumentException("This special price must be valid for at least one night");

            for (var i = 0; i<=nights; i++)
            {
                SpecialPrices.Add(new SpecialPrice(specialPrice, this, from.AddDays(i-1)));
            }

            return this;
        }



    }
}
