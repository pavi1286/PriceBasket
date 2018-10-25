using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Entities
{
    public class Receipt
    {
        public Double SubTotalAmount { get; set; }
        public List<FinalDiscounts> FinalDiscounts { get; set; }
        public Double TotalAmount { get; set; }
    }

    public class FinalDiscounts
    {
        public Double DiscountAmout { get; set; }
        public String DiscountText { get; set; }
    }
}
