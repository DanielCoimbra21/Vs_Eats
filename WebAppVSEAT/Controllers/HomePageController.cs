using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppVSEAT.Models;

namespace WebAppVSEAT.Controllers
{
    public class HomePageController : Controller
    {
        private IRestaurantManager RestaurantManager { get; }
        private IDishesRestaurantManager DishesRestaurantManager { get; }
        private IDishManager DishManager { get; }
        private ICityManager CityManager { get; }
        private IOrderManager OrderManager { get; }
        private IDishesOrderManager DishesOrderManager { get; }

        public HomePageController(IDishesOrderManager dishesOrderManager, IOrderManager orderManager, ICityManager cityManager, IRestaurantManager restaurantManager, IDishManager dishManager, IDishesRestaurantManager dishesRestaurantManager)
        {
            RestaurantManager = restaurantManager;
            DishesRestaurantManager = dishesRestaurantManager;
            DishManager = dishManager;
            CityManager = cityManager;
            OrderManager = orderManager;
            DishesOrderManager = dishesOrderManager;
        }

        /// <summary>
        /// Method for the home page, display the restaurant and the cities
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rest = RestaurantManager.GetRestaurants();               
            var listDish = DishManager.GetDishes();
            var listCities = CityManager.GetCities();

            var vm = new HomePageVM();
            vm.restaurantVMS = new List<RestaurantVM>();
            vm.citiesVMS = new List<DTO.City>();

            //Get all restaurants and create a view model
            foreach (var r in rest)
                {
                    var vmr = new RestaurantVM();
                    var city = CityManager.GetCity(r.IDCITY);
                    vmr.CITYNAME = city.CITYNAME;
                    vmr.IDRESTAURANT = r.IDRESTAURANT;
                    vmr.NAMERESTAURANT = r.NAMERESTAURANT;
                    vmr.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                    vm.restaurantVMS.Add(vmr);
                }
            //Get all the cites and create a view model
            foreach(var c in listCities)
            {
                var vmc = new DTO.City();
                vmc.IDCITY = c.IDCITY;
                vmc.IDDISTRICT = c.IDDISTRICT;
                vmc.CITYNAME = c.CITYNAME;
                vmc.NPA = c.NPA;

                vm.citiesVMS.Add(vmc);
            }

            return View(vm);
        }
    }
}
