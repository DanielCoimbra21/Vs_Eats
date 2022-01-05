using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IDishesOrderManager
    {
        public void InsertDishesOrder(DishesOrder dishesOrder);

        public List<DishesOrder> GetDishesOrders(int idOrder);

        public void UpdateDishesOrder(DishesOrder dishesOrder);
    }

    
}
