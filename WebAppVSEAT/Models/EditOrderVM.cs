using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class EditOrderVM
    {
        public int IDORDER { get; set; }
        public int IDRESTAURANT { get; set; }
        public string NAMERESTAURANT { get; set; }
        public int QUANTITY { get; set; }
        public List<DishesOrderVM> orderDishes { get; set; }
    }
}
