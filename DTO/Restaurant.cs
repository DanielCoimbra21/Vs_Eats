using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vs_Eats
{
    public class Restaurant
    {
        public int IDRESTAURANT { get; set; }
        public int IDCITY { get; set; }
        public string NAMERESTAURANT { get; set; }
        public string ADDRESSRESTAURANT { get; set; }


        public override string ToString()
        {
            return "IdRestaurant : " + IDRESTAURANT + "\n" +
            "CityName : " + IDCITY + "\n" +
            "NameRestaurant : " + NAMERESTAURANT + "\n" +
            "AddressRestaurant : " + ADDRESSRESTAURANT + "\n";
        }

    }
}
