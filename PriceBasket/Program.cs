using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceBasket.Entities;

namespace PriceBasket
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the list of items");
            List<String> itemsList = Console.ReadLine().ToUpper().Split(' ').ToList();

            IPriceBasketOps pb = new PriceBasketOps();
            pb.AddToCart(itemsList);

            pb.CalculateForFinalReceipt();
            pb.FormatOutputAndDisplay();
        }
    }
}
