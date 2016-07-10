using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetElasticSearchDemo.Models.Entities
{
    public class BedAllocation
    {
        //For entity framework
        private BedAllocation()
        {

        }

        public BedAllocation(BedKind kind, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            Kind = kind;
            Quantity = quantity;
        }

        public int BedAllocationId { get; private set; }
        public int Quantity { get; private set; }
        public BedKind Kind { get; private set; }

        private int? totalPeople;

        [NotMapped]
        public int TotalPeople
        {
            get {
                if (!totalPeople.HasValue)
                {
                    var type = typeof(BedKind);
                    var memInfo = type.GetMember(Kind.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(ForPeopleAttribute), false);
                    totalPeople = ((ForPeopleAttribute)attributes.First()).People * Quantity;
                }
                return totalPeople.Value;
            }
        }
    }
}
