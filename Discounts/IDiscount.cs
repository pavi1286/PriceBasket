using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discounts
{
    public interface IDiscount
    {
        void AddDiscounts();
        int CalculateDiscounts();
        void VerifyMultipleDiscounts(); // implement logic if there are multiple discounts on same product
    }
}
