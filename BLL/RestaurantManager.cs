using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using Microsoft.Extensions.Configuration;

namespace BLL
{
    public class RestaurantManager : IRestaurantManager
    {
        private IRestaurantDB RestaurantDb { get; }

        public RestaurantManager(IConfiguration configuration)
        {
            RestaurantDb = new RestaurantDB(configuration);
        }

        public List<Restaurant> GetRestaurants()
        {
            return RestaurantDb.GetRestaurants();
        }

        public Restaurant GetRestaurant(int idRestaurant)
        {
            return RestaurantDb.GetRestaurant(idRestaurant);
        }

    }
}
