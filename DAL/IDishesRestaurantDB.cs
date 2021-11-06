using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDishesRestaurantDB
    {

        List<DishesRestaurant> GetDishesRestaurants();

        public int GetIdRestaurant(int idDish);

    }
}
