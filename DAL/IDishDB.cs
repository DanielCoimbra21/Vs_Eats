using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDishDB
    {
        List<Dish> GetDishes();

        Dish GetDish(string nameDish);

        Dish GetDish(int idDish);

        
    }
}
