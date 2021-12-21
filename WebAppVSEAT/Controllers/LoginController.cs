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

        public IActionResult IndexStaff()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var customer = CustomerManager.GetCustomer(loginVM.mail, loginVM.password);

                if (customer != null)
                {
                    HttpContext.Session.SetInt32("IdCustomer", customer.IDCUSTOMER);
                    return RedirectToAction("Index", "Customer");
                }
                                
                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            return View(loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexStaff(LoginStaffVM loginStaffVM)
        {
            if (ModelState.IsValid)
            {
                var staff = StaffManager.GetStaff(loginStaffVM.MAILSTAFF, loginStaffVM.PASSWORDSTAFF);

                if (staff != null)
                {
                    HttpContext.Session.SetInt32("IdStaff", staff.IDSTAFF);
                    return RedirectToAction("Index", "Order");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            return View(loginStaffVM);
        }

        public IActionResult Logout(LoginVM loginVM)
        {
            //permet de se déconnecter de la session 
            HttpContext.Session.Clear();

            //retour sur la page login
            return RedirectToAction("Index", "Login");
        }
    }
}
