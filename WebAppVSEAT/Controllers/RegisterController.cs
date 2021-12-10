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
    public class RegisterController : Controller
    {

        private ICustomerManager CustomerManager { get; }
        private IStaffManager StaffManager { get; }

        public RegisterController(ICustomerManager customerManager, IStaffManager staffManager)
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
        public IActionResult Index(RegisterVM registervm)
        {
            if (ModelState.IsValid)
            {
                var customer = CustomerManager.GetCustomer(registervm.mail, registervm.password);

                if (customer != null)
                {
                    HttpContext.Session.SetInt32("IdCustomer", customer.IDCUSTOMER);
                    return RedirectToAction("Index", "Customer");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            return View(registervm);
        }


    }
}
