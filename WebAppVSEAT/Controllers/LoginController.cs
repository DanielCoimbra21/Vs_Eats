using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppVSEAT.Models;

namespace WebAppVSEAT.Controllers
{
    public class LoginController : Controller
    {

        private ICustomerManager CustomerManager { get; }
        private IStaffManager StaffManager { get; }

        public LoginController(ICustomerManager customerManager, IStaffManager staffManager)
        {
            CustomerManager = customerManager;
            StaffManager = staffManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginVM loginvm)
        {
            if (ModelState.IsValid)
            {
                var customer = CustomerManager.GetCustomer(loginvm.mail, loginvm.password);

                if (customer != null)
                {
                    HttpContext.Session.SetInt32("IdCustomer", customer.IDCUSTOMER);
                    return RedirectToAction("Index", "Customer");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            return View(loginvm);
        }

    }
}
