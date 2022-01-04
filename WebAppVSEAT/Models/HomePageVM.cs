using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class HomePageVM
    {
        public int IDRESTAURANT { get; set; }
        public string CITYNAME { get; set; }
        public string NAMERESTAURANT { get; set; }
        public string ADDRESSRESTAURANT { get; set; }

        public int IDDISHES { get; set; }
        public string NAMEDISH { get; set; }
        public double PRICEDISH { get; set; }

        public IEnumerable<DishesRestaurantVM> DishesR { get; set; }
    }
}
