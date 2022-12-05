using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CommandVM
    {
        public int IDORDER { get; set; }
        public int IDRESTAURANT { get; set; }
        public int QUANTITY { get; set; }
        public string CITYNAME { get; set; }
        public string NAMERESTAURANT { get; set; }
        public List<CommandVM> orderDishes { get; set; }
        public DTO.Dish dish { get; set; }
        public DateTime DELIVERTIME { get; set; }
        public string hour { get; set; }
        public decimal totalPrice { get; set; }
    }
}
