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
        private ICityManager CityManager { get; }

        public LoginController(ICityManager cityManager,ICustomerManager customerManager, IStaffManager staffManager)
        {
            CustomerManager = customerManager;
            StaffManager = staffManager;
            CityManager = cityManager;
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
                    return RedirectToAction("OrdersStaff", "Order");
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

        /// <summary>
        /// Méthode pour afficher la vue pour créer un staff
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                var listCustomer = CustomerManager.GetCustomers();
                foreach(var c in listCustomer)
                {
                    if(c.MAIL == customerVM.MAIL)
                    {
                        ModelState.AddModelError(string.Empty, "An account already exists with this mail");
                        return View(customerVM);
                    }
                }
                //Trouver l'idCity en fonction de la ville
                var cities = CityManager.GetCities();
                var idCity = -1;
                foreach (var city in cities)
                {
                    if (city.CITYNAME == customerVM.CITYNAME)
                    {
                        idCity = city.IDCITY;
                    }
                }

                if (idCity == -1)
                {
                    ModelState.AddModelError(string.Empty, "Wrong city entered, write correctly");
                    return View(customerVM);
                }

                var customer = new DTO.Customer();
                customer.IDCITY = idCity;
                customer.NAME = customerVM.NAME;
                customer.SURNAME = customerVM.SURNAME;
                customer.USERNAME = customerVM.USERNAME;
                customer.PHONE = customerVM.PHONE;
                customer.ADDRESS = customerVM.ADDRESS;
                customer.MAIL = customerVM.MAIL;
                customer.PASSWORD = customerVM.PASSWORD;
                //customer.PASSWORD = customerVM.confirmerPassword;

                CustomerManager.InsertCustomer(customer);
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
