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
    public class ErrorController : Controller
    {

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PageNotFound404()
        {
            return View();
        }

        public IActionResult Error500()
        {
            return View();
        }

        [Route("Error/{StatusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch(statusCode)
            {
                case 404: ViewBag.errorMessage = "Page not found";
                    return View("PageNotFound404");
                    break;
                case 500:
                    ViewBag.errorMessage = "Error during production";
                    return View("Error500");
                    break;
            }

            return View("");
        }

    }
}
