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
    public class DishesOrderManager : IDishesOrderManager
    {
        private IDishesOrderDB DishesOrderDb { get; set; }

        public List<DishesOrder> GetDishesOrders(int idOrder)
        {
            return DishesOrderDb.GetDishesOrders(idOrder);
        }

        private IOrderDB orderDb { get; }

        public DishesOrderManager(IConfiguration configuration)
        {
            DishesOrderDb = new DishesOrderDB(configuration);

            DishesOrderDb.GetDishesOrders();

            orderDb = new OrderDB(configuration);
        }

        public void InsertDishesOrder(DishesOrder dishesOrder)
        { 
            DishesOrderDb.InsertDishesOrder(dishesOrder);
        }

        
    }
}
