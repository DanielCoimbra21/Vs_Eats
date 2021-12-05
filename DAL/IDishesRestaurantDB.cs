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

        int GetIdRestaurant(int idDish);

        List<int> GetListDishes(int idRestaurant);

    }
}
