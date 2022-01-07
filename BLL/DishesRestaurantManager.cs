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

        public List<int> GetListDishes(int idRestaurant)
        {
            return DishesRestaurantDb.GetListDishes(idRestaurant);
        }
    }
}
