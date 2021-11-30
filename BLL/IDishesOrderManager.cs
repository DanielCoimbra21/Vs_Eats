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
        DishesOrder InsertDishesOrder(DishesOrder dishesOrder);
    }
}
