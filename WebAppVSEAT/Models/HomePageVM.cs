using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class HomePageVM
    {

        public List<RestaurantVM> restaurantVMS { get; set; }
        public List<DTO.City> citiesVMS { get; set; }

    }
}
