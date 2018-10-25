using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discounts
{
    public class CalculateDiscounts : IDiscount
    {
        public void AddDiscounts() { }

        /// <summary>
        /// This menthod is used to calculate the total discounted amount
        /// </summary>
        /// <returns></returns>
        public double GetDiscountedAmount()
        {
            throw new NotImplementedException();
        }

        private void VerifyMultipleDiscounts() { }

    }
}
