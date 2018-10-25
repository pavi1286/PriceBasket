using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Entities
{
    public class Products
    {
        public int ItemId { get; set; }
        public String ItemName { get; set; }
        public Double Price { get; set; }
        public String UnitofMeasure { get; set; }
    }
}
