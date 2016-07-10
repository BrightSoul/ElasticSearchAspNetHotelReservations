using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class AggregationDTO
    {

        
        public string Aggregation { get; set; }
        public IEnumerable<TermDTO> Terms { get; set; }
    }
}
