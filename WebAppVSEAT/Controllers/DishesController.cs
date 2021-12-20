using BLL;
using Microsoft.AspNetCore.Http;
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
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dishes = DishManager.GetDishes();
            return View(dishes);
            
        }

       

    }
}
