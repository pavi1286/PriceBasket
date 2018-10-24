using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket
{
    class Program
    {
        static List<Goods> lGoods = new List<Goods>();
        static List<String> lCartItems = new List<String>();
        static Receipt FinalReceipt = new Receipt();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the items");
            lCartItems = Console.ReadLine().Split(' ').ToList();


            
            lGoods.Add(new Goods { ItemName = "Soup", Price = .65, UnitofMeasure = "tin" });
            lGoods.Add(new Goods { ItemName = "Bread", Price = .80, UnitofMeasure = "loaf" });
            lGoods.Add(new Goods { ItemName = "Milk", Price = 1.3, UnitofMeasure = "bottle" });
            lGoods.Add(new Goods { ItemName = "Apples", Price = 1, UnitofMeasure = "bag" });


            CalculateTotal();
            CalculateDiscount();
        }

        private static void CalculateTotal()
        {
            Double SubTotal = 0;
            Double UnitPrice = 0;

            foreach (String Item in lCartItems)
            {
                UnitPrice = lGoods.Where(x => x.ItemName.ToUpper() == Item.ToUpper()).FirstOrDefault().Price;
                SubTotal = SubTotal + UnitPrice;
            }

            FinalReceipt.SubTotalAmount = SubTotal;

        }

        private static void CalculateDiscount()
        {
            int CountApples = lCartItems.Where(x => x.ToString().ToUpper() == "APPLES").Count();
            int ApplesDiscount = 0;
            if (CountApples > 0)
            {

            }

            int CountSoup = lCartItems.Where(x => x.ToString().ToUpper() == "SOUP").Count();
        }

        private static void FormatDisplay()
        { }
    }

    class Goods
    {
        public String ItemName { get; set; }
        public Double Price { get; set; }
        public String UnitofMeasure { get; set; }
    }

    class Receipt
    {
        public Double SubTotalAmount { get; set; }
        public String DiscountText { get; set; }
        public String DiscountAmount { get; set; }
        public Double TotalAmount { get; set; }
    }
}
