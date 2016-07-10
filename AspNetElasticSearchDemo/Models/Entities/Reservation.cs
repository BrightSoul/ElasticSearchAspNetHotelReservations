using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class Reservation
    {

        //For Entity Framework
        private Reservation()
        {

        }

        public Reservation(DateTime date, Room room, string personName)
        {
            if (date.Date < date)
                throw new ArgumentException("The time part of the date must be zero");

            if (room == null)
                throw new ArgumentNullException(nameof(room));

            if (string.IsNullOrEmpty(personName))
                throw new ArgumentNullException(nameof(personName));

            Date = date;
            Room = room;
            PersonName = personName;
        }

        [Key, Column(Order = 1)]
        public DateTime Date { get; private set; }
        [Key, Column(Order = 2)]
        public int RoomId { get; private set; }
        public virtual Room Room { get; private set; }

      
        [Required]
        public string PersonName { get; private set; }
    }
}
