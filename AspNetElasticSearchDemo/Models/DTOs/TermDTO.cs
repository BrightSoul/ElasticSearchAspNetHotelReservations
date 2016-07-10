using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class TermDTO
    {
        private TermDTO()
        {

        }

        public static TermDTO FromBucket(IBucket bucket)
        {

            if (bucket is RangeBucket)
            {
                var rangeBucket = bucket as RangeBucket;
                return new TermDTO { Term = rangeBucket.Key, DocumentCount = rangeBucket.DocCount };
                
            } else if (bucket is KeyedBucket)
            {
                var keyedBucket = bucket as KeyedBucket;
                return new TermDTO { Term = keyedBucket.Key, DocumentCount = keyedBucket.DocCount.HasValue ? keyedBucket.DocCount.Value : 0 };
            }
            return null;

            
        }

        public string Term { get; set; }
        public long DocumentCount { get; set; }
    }
}
