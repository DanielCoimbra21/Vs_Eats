using BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Controllers
{
    public class DishesController : Controller
    {

        private IDishManager DishManager { get; }

        public DishesController(IDishManager dishManager)
        {
            DishManager = dishManager;
        }

        public IActionResult Index()
        {
            var dishes = DishManager.GetDishes();
            return View(dishes);
            
        }

    }
}
