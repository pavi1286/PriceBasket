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
        static List<String> _cartItems = new List<String>();
        static Receipt _finalReceipt = new Receipt();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the list of items");

            _cartItems = Console.ReadLine().Split(' ').ToList();



            _goodsList.Add(new Goods { ItemId = 1, ItemName = "Soup", Price = .65, UnitofMeasure = "tin", HasDiscount = true });
            _goodsList.Add(new Goods { ItemId = 2, ItemName = "Bread", Price = .80, UnitofMeasure = "loaf", HasDiscount = false });
            _goodsList.Add(new Goods { ItemId = 3, ItemName = "Milk", Price = 1.3, UnitofMeasure = "bottle", HasDiscount = false });
            _goodsList.Add(new Goods { ItemId = 4, ItemName = "Apples", Price = 1, UnitofMeasure = "bag", HasDiscount = true });


            CalculateTotal();
            CalculateDiscount();

            _finalReceipt.TotalAmount = _finalReceipt.SubTotalAmount - _finalReceipt.DiscountAmount;

            Console.WriteLine("Final Amount: " + _finalReceipt.TotalAmount);
        }

        private static void CalculateTotal()
        {
            Double SubTotal = 0;
            Double UnitPrice = 0;

            foreach (String Item in _cartItems)
            {
                UnitPrice = _goodsList.Where(x => x.ItemName.ToUpper() == Item.ToUpper()).FirstOrDefault().Price;
                SubTotal = SubTotal + UnitPrice;
            }

            _finalReceipt.SubTotalAmount = SubTotal;
        }

        private static void CalculateDiscount()
        {
            int countApples = _cartItems.Where(x => x.ToString().ToUpper() == "APPLES").Count();
            Double discountApples = 0;

            if (countApples > 0)
            {
                discountApples = countApples * 0.10 *
                                 _goodsList.Where(x => x.ItemName.ToUpper() == "APPLES").FirstOrDefault().Price;

            }

            _finalReceipt.DiscountAmount = _finalReceipt.DiscountAmount + discountApples;


            int countSoup = _cartItems.Where(x => x.ToString().ToUpper() == "SOUP").Count();
            int countBread = _cartItems.Where(x => x.ToString().ToUpper() == "BREAD").Count();
            double discountBread = 0;

            if ((countSoup > 1) && (countBread > 0))
            {
                int noofSoupForDiscount = countSoup / 2;

                if (countBread <= noofSoupForDiscount)
                {
                    discountBread = countBread * 0.50 *
                                    _goodsList.Where(x => x.ItemName.ToUpper() == "BREAD").FirstOrDefault().Price;

                }

            }

            _finalReceipt.DiscountAmount = _finalReceipt.DiscountAmount + discountBread;


        }

        private static void FormatOutputForDisplay()
        { }
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
        public String DiscountText { get; set; }
        public Double DiscountAmount { get; set; }
        public Double TotalAmount { get; set; }
    }
}
