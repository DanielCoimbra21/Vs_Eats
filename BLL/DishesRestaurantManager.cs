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
    public class DishesRestaurantManager : IDishesRestaurantManager
    {
        private IDishesRestaurantDB DishesRestaurantDb { get; }

        public DishesRestaurantManager(IConfiguration conf)
        {
            DishesRestaurantDb = new DishesRestaurantDB(conf);
        }

        public List<DishesRestaurant> GetDishesRestaurants()
        {
            return DishesRestaurantDb.GetDishesRestaurants();
        }

        public int GetIdRestaurant(int idDish)
        {
            return DishesRestaurantDb.GetIdRestaurant(idDish);
        }

    }
}
