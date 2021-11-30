using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IRestaurantManager
    {
        List<Restaurant> GetRestaurants();

        Restaurant GetRestaurant(int idRestaurant);

    }
}
