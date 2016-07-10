using AspNetElasticSearchDemo.Models.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Documents
{
    [ElasticsearchType]
    public class SpecialPriceDocument
    {

        private SpecialPriceDocument()
        {

        }


        //Prepare the document by denormalizing the entity tree
        public static SpecialPriceDocument FromSpecialPrice(SpecialPrice specialPrice)
        {
            return new SpecialPriceDocument
            {
                Date = specialPrice.Date,
                Price = specialPrice.Price,
                IsDiscounted = specialPrice.IsDiscounted,
                Discount = specialPrice.Discount
            };
        }

        [Date(Store = true, NumericResolution = NumericResolutionUnit.Seconds, Similarity = SimilarityOption.Default)]
        public DateTime Date { get; set; }
        public bool IsDiscounted { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        
    }
}
