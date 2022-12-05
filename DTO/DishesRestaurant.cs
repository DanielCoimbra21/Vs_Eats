using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DishesRestaurant
    {
        public int IDDISHES { get; set; }
        public int IDRESTAURANT { get; set; }

        public override string ToString()
        {
            return "IdDishes : " + IDDISHES + "\n"
                + "IdRestaurant : " + IDRESTAURANT + "\n";
        }

    }
}
