using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public int DiscountTypeId { get; set; }
        public String DiscountType { get; set; }
        public String DiscountItemName { get; set; }
        public int NoOfItemsEligibleForDiscount { get; set; }
        public String DiscountText { get; set; }
        public double DiscountValue { get; set; }
        public String DiscountUnit { get; set; }
        public bool DiscountOnSameProduct { get; set; }
        public String OtherDiscountedProductName { get; set; }
        public Duration DiscountValidTime { get; set; }
    }

    public class Duration
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
