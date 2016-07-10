using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class SearchResult
    {
        public IEnumerable<AggregationDTO> Aggregations { get; set; }
        public IEnumerable<ResultDTO> Hits { get; set; }
        public long TotalResults { get; set; }
        public int QueryDuration { get; set; }
        public long RequestDuration { get; set; }

    }
}
