using BLL;
using WebAppVSEAT.Models;
using Microsoft.AspNetCore.Mvc;
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
        

        public RestaurantController(IRestaurantManager restaurantManager, IDishManager dishManager, IDishesRestaurantManager dishesRestaurantManager)
        {
            RestaurantManager = restaurantManager;
            DishesRestaurantManager = dishesRestaurantManager;
            DishManager = dishManager;
        }

        public IActionResult Index()
        {
            var rest = RestaurantManager.GetRestaurants();
            return View(rest);
        }

        public ActionResult Details(int id)
        
        {
            var listDish = new List<int>();
            listDish = DishesRestaurantManager.GetListDishes(id);
            List<DTO.Dish> results = null;


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


    }
}
