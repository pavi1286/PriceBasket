using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket
{
    class Program
    {
        static List<Goods> _goodsList = new List<Goods>();
        static List<Discounts> _discountsList = new List<Discounts>();
        static List<Goods> _cartItemsList = new List<Goods>();
        static Receipt _finalReceipt = new Receipt();
        

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the list of items");

            List<String> itemsList = Console.ReadLine().ToUpper().Split(' ').ToList();


            InitialiseValues();
            AddToCart(itemsList);

            CalculateSubTotal();
            CalculateDiscount();

            CalculateTotal();
            FormatOutputForDisplay();
        }

        private static void AddToCart(List<String> itemsList)
        {
            foreach (String item in itemsList)
            {
                Goods selectedItem = _goodsList.Where(x => x.ItemName == item.ToString()).FirstOrDefault();
                _cartItemsList.Add(selectedItem);
            }
        }

        private static void InitialiseValues()
        {
            _goodsList.Add(new Goods { ItemId = 1, ItemName = "SOUP", Price = .65, UnitofMeasure = "tin", HasDiscount = true });
            _goodsList.Add(new Goods { ItemId = 2, ItemName = "BREAD", Price = .80, UnitofMeasure = "loaf", HasDiscount = false });
            _goodsList.Add(new Goods { ItemId = 3, ItemName = "MILK", Price = 1.3, UnitofMeasure = "bottle", HasDiscount = false });
            _goodsList.Add(new Goods { ItemId = 4, ItemName = "APPLES", Price = 1, UnitofMeasure = "bag", HasDiscount = true });

            
            _discountsList.Add(new Discounts
            {
                DiscountId = 1,
                DiscountItemName = "APPLES",
                NoOfItemsEligibleForDiscount = 1,
                DiscountText = "Apples 10 % off: ",
                DiscountValue = 10,
                DiscountUnit = " %",
                DiscountOnSameProduct = true,
                DiscountValidTime = new Duration { StartDate = new DateTime(21/10/2018), EndDate = new DateTime(27/10/2018) }
            });
            _discountsList.Add(new Discounts
            {
                DiscountId = 2,
                DiscountItemName = "SOUP",
                NoOfItemsEligibleForDiscount = 2,
                DiscountText = "Bread for half price for 2 tins of soup: ",
                DiscountValue = 50,
                DiscountUnit = "%",
                DiscountOnSameProduct = false,
                OtherDiscountedProductName = "BREAD",
                DiscountValidTime = new Duration { StartDate = new DateTime(21/10/2018), EndDate = new DateTime(27/10/2018) }
            });
        }

        private static void CalculateSubTotal()
        {
            Double subTotal =  _cartItemsList.Sum(x => x.Price);

            _finalReceipt.SubTotalAmount = subTotal;
        }

        private static void CalculateTotal()
        {
            Double totalDiscount = _finalReceipt.FinalDiscounts.Sum(x => x.DiscountAmout);

            _finalReceipt.TotalAmount = _finalReceipt.SubTotalAmount - totalDiscount;
        }


        private static void CalculateDiscount()
        {
            List<FinalDiscounts> finalDiscountsList = new List<FinalDiscounts>();

            if (_cartItemsList.Where(x => x.HasDiscount == true).Count() > 0)
            {
                var discountedItems = _cartItemsList.Where(x => x.HasDiscount == true).GroupBy(x => x.ItemId).ToList();
                

                foreach (var item in discountedItems)
                {

                    Console.WriteLine(item.Key);

                    Double calculatedDiscount = 0;
                    Double unitPrice = 0;

                    String discountedItem = item.FirstOrDefault().ItemName;

                    Discounts discountForItem = _discountsList.Where(x => x.DiscountItemName == discountedItem).FirstOrDefault();

                    if (item.Count() >= discountForItem.NoOfItemsEligibleForDiscount)
                    {

                        if (discountForItem.DiscountOnSameProduct == true)
                        {
                            unitPrice = item.FirstOrDefault().Price;
                            calculatedDiscount = item.Count() * unitPrice * (discountForItem.DiscountValue / 100);
                        }
                        else
                        {
                            if (_cartItemsList.Where(x => x.ItemName.Contains(discountForItem.OtherDiscountedProductName)).Count() > 0)
                            {
                                int noofSoupForDiscount = item.Count() / discountForItem.NoOfItemsEligibleForDiscount;
                                if (_cartItemsList.Where(x => x.ItemName.Contains(discountForItem.OtherDiscountedProductName)).Count() <= noofSoupForDiscount)
                                {
                                    unitPrice = _goodsList.Where(x => x.ItemName == discountForItem.OtherDiscountedProductName).FirstOrDefault().Price;
                                    calculatedDiscount = _cartItemsList.Where(x => x.ItemName.Contains(discountForItem.OtherDiscountedProductName)).Count() * unitPrice * (discountForItem.DiscountValue / 100);
                                }
                            }
                        }
                        

                        finalDiscountsList.Add(new FinalDiscounts { DiscountAmout = calculatedDiscount, DiscountText = discountForItem.DiscountText });
                    }
                }
            }
            else
            {
                String noOffersText = "(no offers available)";
                finalDiscountsList.Add(new FinalDiscounts { DiscountAmout = 0.0, DiscountText = noOffersText });                
            }

            _finalReceipt.FinalDiscounts = finalDiscountsList;
        }

        private static Discounts GetDiscountForItem(IEnumerable<object> item)
        {
            
            
            
            return new Discounts();
        }

        private static void FormatOutputForDisplay()
        {
            Console.WriteLine("SubTotal: £" + _finalReceipt.SubTotalAmount);
            foreach (var discounts in _finalReceipt.FinalDiscounts)
                Console.WriteLine(discounts.DiscountText + discounts.DiscountAmout);
            Console.WriteLine("Total: £" + _finalReceipt.TotalAmount);

            Console.ReadLine();
        }
    }

    class Goods
    {
        public int ItemId { get; set; }
        public String ItemName { get; set; }
        public Double Price { get; set; }
        public String UnitofMeasure { get; set; }
        public bool HasDiscount { get; set; }
    }

    class Receipt
    {
        public Double SubTotalAmount { get; set; }
        public List<FinalDiscounts> FinalDiscounts { get; set; }
        public Double TotalAmount { get; set; }
    }

    class FinalDiscounts
    {
        public Double DiscountAmout { get; set; }
        public String DiscountText { get; set; }
    }

    class Discounts
    {
        public int DiscountId { get; set; }
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


//private static void CalculateDiscountOnSameProduct(Goods cartItem, Discounts discountForItem)
//{


//    //int countApples = _cartItems.Where(x => x.ToString().ToUpper() == "APPLES").Count();
//    //Double discountApples = 0;
//    //List<String> discountsList = new List<string>();

//    //if (countApples > 0)
//    //{
//    //    discountApples = countApples * 0.10 *
//    //                     _goodsList.Where(x => x.ItemName.ToUpper() == "APPLES").FirstOrDefault().Price;

//    //    _finalReceipt.DiscountAmount = _finalReceipt.DiscountAmount + discountApples;
//    //    _finalReceipt.DiscountText.Add(_discountsList[0].DiscountText);
//    //    //";
//    //}
//}

//private static void CalculateDiscountOnDifferentProduct(Goods cartItem, Discounts discountForItem)
//{
//    //_cartItems.GroupBy(x=>x.)





//    //int countSoup = _cartItems.Where(x => x.ToString().ToUpper() == "SOUP").Count();
//    //int countBread = _cartItems.Where(x => x.ToString().ToUpper() == "BREAD").Count();
//    //double discountBread = 0;

//    //if ((countSoup > 1) && (countBread > 0))
//    //{
//    //    int noofSoupForDiscount = countSoup / 2;

//    //    if (countBread <= noofSoupForDiscount)
//    //    {
//    //        discountBread = countBread * 0.50 *
//    //                        _goodsList.Where(x => x.ItemName.ToUpper() == "BREAD").FirstOrDefault().Price;

//    //    }

//    //    _finalReceipt.DiscountAmount = _finalReceipt.DiscountAmount + discountBread;
//    //    _finalReceipt.DiscountText = "Bread for half price for 2 tins of soup: ";
//    //}

//}
