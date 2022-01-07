using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public interface IDishesOrderDB
    {
        public List<DishesOrder> GetDishesOrders();
        public List<DishesOrder> GetDishesOrders(int idOrder);
        public void InsertDishesOrder(DishesOrder dishes);
        public void UpdateDishesOrder(DishesOrder dishesOrder);
    }
}
