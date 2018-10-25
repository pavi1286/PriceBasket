using PriceBasket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PriceBasket.TotalReceipt
{
    public class CalculateReceipt
    {
        List<Products> _productsList = new List<Products>();
        List<Discount> _discountsList = new List<Discount>();
        List<Products> _cartItemsList = new List<Products>();
        Receipt _finalReceipt = new Receipt();

        public void CalculateSubTotal()
        {
            Double subTotal = _cartItemsList.Sum(x => x.Price);

            _finalReceipt.SubTotalAmount = subTotal;
        }

        public void CalculateTotal()
        {
            Double totalDiscount = _finalReceipt.FinalDiscounts.Sum(x => x.DiscountAmout);

            _finalReceipt.TotalAmount = _finalReceipt.SubTotalAmount - totalDiscount;
        }
    }
}
