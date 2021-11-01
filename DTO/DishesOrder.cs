using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DishesOrder
    {

        public int IDDISHES { get; set; }
        public int IDORDER { get; set; }

        public int QUANTITY { get; set; }

        public override string ToString()
        {
            return "IdDishes : " + IDDISHES + "\n"
                + "IdOrder : " + IDORDER + "\n"
                + "Quantity : " + QUANTITY + "\n";
        }
    }
}
