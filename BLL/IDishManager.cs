using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IDishManager
    {
               
        
        Dish GetDish(string dishName);


        Dish GetDish(int idDish);


        List<Dish> GetDishes();
        
    }
}
