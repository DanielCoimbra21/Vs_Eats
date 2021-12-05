using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{

    public class RestaurantVM
    {
        public string NAMERESTAURANT { get; set; }
        public string ADDRESSRESTAURANT { get; set; }
        public IEnumerable<DTO.Restaurant> Restaurants { get; set; }
    }
}
