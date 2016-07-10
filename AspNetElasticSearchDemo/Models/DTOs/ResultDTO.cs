using AspNetElasticSearchDemo.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.DTOs
{
    public class ResultDTO
    {
        public decimal Score { get; set; }
        public RoomDocument Room { get; set; }
    }
}
