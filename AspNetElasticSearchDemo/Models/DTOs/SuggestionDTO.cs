using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class SuggestionDTO
    {
        private SuggestionDTO()
        {

        }
        public string Suggestion { get; private set; }
        public IEnumerable<string> Terms { get; private set; }

        public static SuggestionDTO FromSuggestion (KeyValuePair<string, Suggest[]> suggestion)
        {
            return new SuggestionDTO
            {
                Suggestion = suggestion.Key,
                Terms = suggestion.Value.SelectMany(v => v.Options).Select(o => o.Text)
            };
        }
    }
}
