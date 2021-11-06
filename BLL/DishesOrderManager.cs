using DAL;
using DTO;
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

        private IOrderDB orderDb { get; }

        public DishesOrderManager(IConfiguration configuration)
        {
            DishesOrderDb = new DishesOrderDB(configuration);

            DishesOrderDb.GetDishesOrders();

            orderDb = new OrderDB(configuration);
        }

        public DishesOrder InsertDishesOrder(DishesOrder dishesOrder)
        { 
            List<DishesOrder> listDO = new List<DishesOrder>();
            Order order = orderDb.GetOrder(dishesOrder.IDORDER);
        
            return DishesOrderDb.InsertDishesOrder(dishesOrder);
        }
    }
}
