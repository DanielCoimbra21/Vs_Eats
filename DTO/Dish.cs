using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Dish
    {
        public int IDDISHES { get; set; }
        public string NAMEDISH { get; set; }

        public double PRICEDISH { get; set; }

        public override string ToString()
        {
            return "IdDish " + IDDISHES + "\n"
                + "NameDish " + NAMEDISH + "\n"
                + "PriceDish " + PRICEDISH + "\n";
        }


    }
}
