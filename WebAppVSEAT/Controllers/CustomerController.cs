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

        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = CustomerManager.GetCustomerID((int)HttpContext.Session.GetInt32("IdCustomer"));
            var customerVM = new CustomerVM();

            //Rechercher le nom de la ville
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

        public IActionResult Edit()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = CustomerManager.GetCustomerID((int)HttpContext.Session.GetInt32("IdCustomer"));
            var city = CityManager.GetCity(customer.IDCITY);

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

            //Trouver l'idCity en fonction de la ville
            var cities = CityManager.GetCities();
            var idCity = -1;
            foreach (var city in cities)
            {
                if(city.CITYNAME == customerVM.CITYNAME)
                {
                    idCity = city.IDCITY;
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
        /// Méthode pour le changement de mot de passe dans la page profile
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


        /// <summary>
        /// Méthode pour changer le mot de passe
        /// </summary>
        /// <param name="changePasswordVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePasswordCustomer(ChangePasswordCustomerVM changePasswordCustomerVM)
        {
            if (ModelState.IsValid)
            {
                int idCustomer = (int)HttpContext.Session.GetInt32("IdCustomer");
                var customer = CustomerManager.GetCustomerID(idCustomer);
                
                //vérification que le mot de passe entrer est bien le même que celui sauvegarder
                if (changePasswordCustomerVM.PASSWORDCUSTOMER == customer.PASSWORD)
                {
                    //vérification que le nouveau mot de passe est égal au mot de passe confirmé
                    if (changePasswordCustomerVM.NEWPASSWORD == changePasswordCustomerVM.CONFIRMPASSWORD)
                    {
                        customer.PASSWORD = changePasswordCustomerVM.CONFIRMPASSWORD;
                        CustomerManager.UpdatePassword(customer);
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

            return RedirectToAction("Index", "Customer");
        }

    }
}

