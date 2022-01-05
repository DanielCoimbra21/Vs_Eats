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

        public void InsertDishesOrder(DishesOrder dishes);

        public List<DishesOrder> GetDishesOrders(int idOrder);

        public void UpdateDishesOrder(DishesOrder dishesOrder);
    }
}
