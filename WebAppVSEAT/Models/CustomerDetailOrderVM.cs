using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CustomerDetailOrderVM
    {
        public int IDORDER { get; set; }
        public string CITYNAME { get; set; }
        public string NAMERESTAURANT { get; set; }
        public List<DishesOrderVM> orderDishes { get; set; }
        public DateTime DELIVERTIME { get; set; }
        public decimal totalPrice { get; set; }
    }
}
