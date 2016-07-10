using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public enum BedKind
    {
        [ForPeople(1)]
        Single,
        [ForPeople(2)]
        Full,
        [ForPeople(2)]
        King
    }
}
