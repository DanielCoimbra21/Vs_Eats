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
    public class CustomerController : Controller
    {

        private ICustomerManager CustomerManager { get; }
        private ICityManager CityManager { get; }
        
        public CustomerController(ICustomerManager customerManager,ICityManager cityManager)
        {
            CustomerManager = customerManager;
            CityManager = cityManager;
        }

        /// <summary>
        /// Method index to the profile of the customer
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = CustomerManager.GetCustomerID((int)HttpContext.Session.GetInt32("IdCustomer"));
            var customerVM = new CustomerVM();

            //Search the city of the customer to display after the cityname
            var city = CityManager.GetCity(customer.IDCITY);

            customerVM.CITYNAME = city.CITYNAME;
            customerVM.NAME = customer.NAME;
            customerVM.SURNAME = customer.SURNAME;
            customerVM.USERNAME = customer.USERNAME;
            customerVM.MAIL = customer.MAIL;
            customerVM.PHONE = customer.PHONE;
            customerVM.ADDRESS = customer.ADDRESS;


            return View(customerVM);
        }

        /// <summary>
        /// Method to custom the information about the customer
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Get the information from the customer and an object city that match the idCity from the customer
            var customer = CustomerManager.GetCustomerID((int)HttpContext.Session.GetInt32("IdCustomer"));
            var city = CityManager.GetCity(customer.IDCITY);

            //Creation of the customer VM with th
            CustomerVM customerVM = new CustomerVM()
            {
                IDCUSTOMER = customer.IDCUSTOMER,
                IDCITY = customer.IDCITY,
                CITYNAME = city.CITYNAME,
                NAME = customer.NAME,
                SURNAME = customer.SURNAME,
                USERNAME = customer.USERNAME,
                PHONE = customer.PHONE,
                ADDRESS = customer.ADDRESS,
                MAIL = customer.MAIL
            };

            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CustomerVM customerVM)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            DTO.Customer customer = CustomerManager.GetCustomerID(customerVM.IDCUSTOMER);

            //Find the idCity of the customer cityname
            var cities = CityManager.GetCities();
            var idCity = -1;
            foreach (var city in cities)
            {
                if(city.CITYNAME == customerVM.CITYNAME)
                {
                    idCity = city.IDCITY;
                    break;
                }
            }

            if(idCity == -1)
            {
                ModelState.AddModelError(string.Empty, "Wrong city entered, write correctly");
                return View(customerVM);
            }

            customer.IDCUSTOMER = customerVM.IDCUSTOMER;
            customer.IDCITY = idCity;
            customer.NAME = customerVM.NAME;
            customer.SURNAME = customerVM.SURNAME;
            customer.USERNAME = customerVM.USERNAME;
            customer.PHONE = customerVM.PHONE;
            customer.ADDRESS = customerVM.ADDRESS;
            customer.MAIL = customerVM.MAIL;
          
            if (ModelState.IsValid)
            {
                CustomerManager.UpdateCustomer(customer);
                return RedirectToAction(nameof(Index));
            }

            return View(customerVM);
        }

        /// <summary>
        /// Method to change the password in the profile
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePasswordCustomer()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePasswordCustomer(ChangePasswordCustomerVM changePasswordCustomerVM)
        {
            if (ModelState.IsValid)
            {
                int idCustomer = (int)HttpContext.Session.GetInt32("IdCustomer");
                var customer = CustomerManager.GetCustomerID(idCustomer);

                //check that the password entered is the same as the one saved
                if (CustomerManager.VerifyPassword(changePasswordCustomerVM.PASSWORDCUSTOMER, customer.MAIL))
                {
                    //check that the new password is equal to the confirmed password
                    if (changePasswordCustomerVM.NEWPASSWORD == changePasswordCustomerVM.CONFIRMPASSWORD)
                    {
                        customer.PASSWORD = CustomerManager.SetPassword(changePasswordCustomerVM.CONFIRMPASSWORD);
                        CustomerManager.UpdatePassword(customer);
                        return RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong password or wrong new password");
                        return View(changePasswordCustomerVM);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong password or wrong new password");
                    return View(changePasswordCustomerVM);
                }
            }
            return View(changePasswordCustomerVM);
        }

    }
}

