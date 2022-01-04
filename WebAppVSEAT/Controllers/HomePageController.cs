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

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rest = RestaurantManager.GetRestaurants();
            var restaurants_vm = new List<RestaurantVM>();


            foreach (var r in rest)
            {
                var vm = new RestaurantVM();
                var city = CityManager.GetCity(r.IDCITY);
                vm.CITYNAME = city.CITYNAME;
                vm.IDRESTAURANT = r.IDRESTAURANT;
                vm.NAMERESTAURANT = r.NAMERESTAURANT;
                vm.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                restaurants_vm.Add(vm);
            }
            return View(restaurants_vm);
        }
    }
}
