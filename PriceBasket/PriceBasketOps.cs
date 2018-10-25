using PriceBasket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket
{
    sealed class PriceBasketOps : IPriceBasketOps
    {
        List<Products> _productsList = new List<Products>();
        List<Discount> _discountsList = new List<Discount>();
        List<Products> _cartItemsList = new List<Products>();
        Receipt _finalReceipt = new Receipt();

        public PriceBasketOps()
        {
            _productsList.Add(new Products { ItemId = 1, ItemName = "SOUP", Price = .65, UnitofMeasure = "tin" });
            _productsList.Add(new Products { ItemId = 2, ItemName = "BREAD", Price = .80, UnitofMeasure = "loaf" });
            _productsList.Add(new Products { ItemId = 3, ItemName = "MILK", Price = 1.3, UnitofMeasure = "bottle" });
            _productsList.Add(new Products { ItemId = 4, ItemName = "APPLES", Price = 1, UnitofMeasure = "bag" });

            _discountsList.Add(new Discount
            {
                DiscountId = 1,
                DiscountTypeId = 1,
                DiscountType = "% off",
                DiscountItemName = "APPLES",
                NoOfItemsEligibleForDiscount = 1,
                DiscountText = "Apples 10 % off: ",
                DiscountValue = 10,
                DiscountUnit = " %",
                DiscountOnSameProduct = true,
                DiscountValidTime = new Duration { StartDate = new DateTime(2018, 10, 21), EndDate = new DateTime(2018, 11, 27) }
            });

            _discountsList.Add(new Discount
            {
                DiscountId = 2,
                DiscountTypeId = 2,
                DiscountType = "Buy 2 get other product half price",
                DiscountItemName = "SOUP",
                NoOfItemsEligibleForDiscount = 2,
                DiscountText = "Bread 50% off: ",
                DiscountValue = 50,
                DiscountUnit = "%",
                DiscountOnSameProduct = false,
                OtherDiscountedProductName = "BREAD",
                DiscountValidTime = new Duration { StartDate = new DateTime(2018, 10, 21), EndDate = new DateTime(2018, 11, 27) }
            });
        }

        public void CalculateForFinalReceipt()
        {
            CalculateSubTotal();
            CalculateDiscount();

            CalculateTotal();
        }

        public void AddToCart(List<String> itemsList)
        {
            foreach (String item in itemsList)
            {
                Products selectedItem = _productsList.Where(x => x.ItemName == item.ToString()).FirstOrDefault();
                _cartItemsList.Add(selectedItem);
            }
        }


        private void CalculateSubTotal()
        {
            Double subTotal = _cartItemsList.Sum(x => x.Price);

            _finalReceipt.SubTotalAmount = subTotal;
        }

        private void CalculateTotal()
        {
            Double totalDiscount = _finalReceipt.FinalDiscounts.Sum(x => x.DiscountAmout);

            _finalReceipt.TotalAmount = _finalReceipt.SubTotalAmount - totalDiscount;
        }


        private void CalculateDiscount()
        {
            List<FinalDiscounts> finalDiscountsList = new List<FinalDiscounts>();

            DateTime dateToday = DateTime.Today;

            var discountedItems = _cartItemsList
                                    .Where(x => _discountsList
                                                    .Any(y => (y.DiscountItemName == x.ItemName)
                                                                && (y.DiscountValidTime.StartDate <= dateToday)
                                                                && (y.DiscountValidTime.EndDate >= dateToday))).GroupBy(x => x.ItemId).ToList();

            if (discountedItems.Count() > 0)
            {
                foreach (var item in discountedItems)
                {
                    Double calculatedDiscount = 0;
                    Double unitPrice = 0;

                    String discountedItem = item.FirstOrDefault().ItemName;

                    Discount discountDetailsForItem = _discountsList.Where(x => x.DiscountItemName == discountedItem).FirstOrDefault();

                    if (item.Count() >= discountDetailsForItem.NoOfItemsEligibleForDiscount)
                    {

                        if (discountDetailsForItem.DiscountOnSameProduct == true)
                        {
                            unitPrice = item.FirstOrDefault().Price;
                            calculatedDiscount = item.Count() * unitPrice * (discountDetailsForItem.DiscountValue / 100);
                        }
                        else
                        {
                            int countDiscountedProduct = _cartItemsList.Where(x => x.ItemName.Contains(discountDetailsForItem.OtherDiscountedProductName)).Count();
                            int noOfProductDiscountApplied = 0;
                            if (countDiscountedProduct > 0)
                            {
                                int noOfProductForDiscount = item.Count() / discountDetailsForItem.NoOfItemsEligibleForDiscount;

                                noOfProductDiscountApplied = countDiscountedProduct <= noOfProductForDiscount ? countDiscountedProduct : noOfProductForDiscount;

                                unitPrice = _productsList.Where(x => x.ItemName == discountDetailsForItem.OtherDiscountedProductName).FirstOrDefault().Price;
                                calculatedDiscount = noOfProductDiscountApplied * unitPrice * (discountDetailsForItem.DiscountValue / 100);

                            }
                        }

                        if (calculatedDiscount != 0)
                            finalDiscountsList.Add(new FinalDiscounts { DiscountAmout = calculatedDiscount, DiscountText = discountDetailsForItem.DiscountText });
                    }
                }
            }
            if (finalDiscountsList.Count == 0)
            {
                String noOffersText = "(no offers available)";
                finalDiscountsList.Add(new FinalDiscounts { DiscountAmout = 0.0, DiscountText = noOffersText });
            }

            _finalReceipt.FinalDiscounts = finalDiscountsList;
        }

        public void FormatOutputAndDisplay()
        {
            Console.WriteLine("SubTotal: £" + _finalReceipt.SubTotalAmount);
            foreach (var discounts in _finalReceipt.FinalDiscounts)
                Console.WriteLine(discounts.DiscountText + discounts.DiscountAmout);
            Console.WriteLine("Total: £" + _finalReceipt.TotalAmount);

            Console.ReadLine();
        }
    }
}
