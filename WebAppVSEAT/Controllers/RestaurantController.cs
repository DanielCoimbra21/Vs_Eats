using BLL;
using WebAppVSEAT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAppVSEAT.Controllers
{
    public class RestaurantController : Controller
    {
        private IRestaurantManager RestaurantManager { get; }
        private IDishesRestaurantManager DishesRestaurantManager { get; }
        private IDishManager DishManager { get; }
        private ICityManager CityManager { get; }

        private List<DTO.Dish> ld = new List<DTO.Dish>();


        public RestaurantController(ICityManager cityManager,IRestaurantManager restaurantManager, IDishManager dishManager, IDishesRestaurantManager dishesRestaurantManager)
        {
            RestaurantManager = restaurantManager;
            DishesRestaurantManager = dishesRestaurantManager;
            DishManager = dishManager;
            CityManager = cityManager;
        }

        
        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rest = RestaurantManager.GetRestaurants();
            var restaurants_vm = new List<RestaurantVM>();
           

            foreach(var r in rest)
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


        public ActionResult Details(int id)
        {

            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listDish = new List<int>();
            listDish = DishesRestaurantManager.GetListDishes(id);
            List<DTO.Dish> results = null;


            if (listDish == null)
            {
                var error = new ErrorViewModel();
                return View(error);
            }


            foreach (var idDish in listDish)
            {
                if (results == null)
                {
                    results = new List<DTO.Dish>();
                }
                results.Add(DishManager.GetDish(idDish));
            }

            

            var vm = new DishesVM()
            {
                Dishes = results
            };
            

             return View(vm);
        }

        public ActionResult Add(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dish = DishManager.GetDish(id);
            return null;
        }

       
       


    }
}
