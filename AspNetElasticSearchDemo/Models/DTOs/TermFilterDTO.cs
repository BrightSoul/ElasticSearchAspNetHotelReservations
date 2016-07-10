using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class TermFilterDTO
    {
        private TermFilterDTO()
        {

        }
        public static TermFilterDTO FromString(string term)
        {
            var parts = term.Split('_');
            if (parts.Length != 2)
                return null;
            return new TermFilterDTO { Aggregation = parts[0], Term = parts[1] };
        }
        public string Aggregation { get; private set; }
        public string Term { get; private set; }

    }
}
