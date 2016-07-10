using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class SuggestResult
    {
        public IEnumerable<SuggestionDTO> Suggestions { get; set; }
    }
}
