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
            var listDish = DishManager.GetDishes();

            var vm = new HomePageVM();
            vm.restaurantVMS = new List<RestaurantVM>();
            vm.dishesVMS = new List<DishesVM>();


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



            foreach (var dish in listDish)
            {
                var vmd = new DishesVM();
                var restaurant = RestaurantManager.GetRestaurant(dish.IDDISHES);
               // vmd.IDRESTAURANT = 
                vmd.NAMEDISH = dish.NAMEDISH;
                vmd.PRICEDISH = dish.PRICEDISH;
                vmd.IDDISHES = dish.IDDISHES;

                vm.dishesVMS.Add(vmd);
            }


            return View(vm);

            

        }
    }
}
