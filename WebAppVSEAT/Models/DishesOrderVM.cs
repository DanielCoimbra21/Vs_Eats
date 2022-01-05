using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class DishesOrderVM
    {
        public int IDDISHES { get; set; }
        public string NAMEDISH { get; set; }
        public double PRICEDISH { get; set; }
        public int IDRESTAURANT { get; set; }
        public int QUANTITY { get; set; }
    }
}
