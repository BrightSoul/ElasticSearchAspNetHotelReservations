using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class Hotel
    {
        private Hotel()
        {
            Rooms = new HashSet<Room>();

        }

        public Hotel(string name) : this()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("This hotel must have a name");

            Name = name;
        }

        public int HotelId { get; private set; }
        public string Name { get; private set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public Hotel WithRooms(params Room[] rooms)
        {
            foreach (var room in rooms)
                Rooms.Add(room);

            return this;
        }
    }
}
