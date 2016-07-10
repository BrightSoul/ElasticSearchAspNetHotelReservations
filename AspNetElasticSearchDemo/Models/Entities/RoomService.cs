using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class RoomService
    {
        private RoomService()
        {

        }
        public RoomService(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("This room service must have a name");

            Name = name;
        }

        public int RoomServiceId { get; private set; }
        public string Name { get; private set; }
    }
}
