using PriceBasket.Entities;
using System.Collections.Generic;

namespace PriceBasket
{
    public interface IPriceBasketOps
    {
        void AddToCart(List<string> itemsList);
        void CalculateForFinalReceipt();
        void FormatOutputAndDisplay();
    }
}