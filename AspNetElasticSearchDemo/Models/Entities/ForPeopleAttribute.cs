using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class ForPeopleAttribute : Attribute
    {
        public int People { get; }
        public ForPeopleAttribute(int people)
        {
            if (people < 0 && people > 2)
                throw new ArgumentException("A bed can be for one or two people only");

            People = people;
        }
    }
}
