using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DishesOrderManager
    {
        private IDishesOrderDB DishesOrderDb { get; set; }

        public DishesOrderManager(IConfiguration configuration)
        {
            DishesOrderDb = new DishesOrderDB(configuration);

            DishesOrderDb.GetDishesOrders();
        }
    }
}
