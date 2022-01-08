using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BLL;
using FluentEmail.Core;
using FluentEmail.Smtp;
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

        /// <summary>
        /// Method that display the page where you can log as a customer
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method that display the page where you can log as a staff
        /// </summary>
        /// <returns></returns>
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
                //Verify the password with the one from the database
                if (CustomerManager.VerifyPassword(loginVM.password, loginVM.mail))
                {
                    var customer = CustomerManager.GetCustomer(loginVM.mail);

                    if (customer != null)
                    {
                        HttpContext.Session.SetInt32("IdCustomer", customer.IDCUSTOMER);
                        return RedirectToAction("Index", "HomePage");
                    }
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
                if (StaffManager.VerifyPassword(loginStaffVM.PASSWORDSTAFF, loginStaffVM.MAILSTAFF))
                {
                    var staff = StaffManager.GetStaff(loginStaffVM.MAILSTAFF);

                    if (staff != null)
                    {
                        HttpContext.Session.SetInt32("IdStaff", staff.IDSTAFF);
                        return RedirectToAction("OrdersStaff", "Order");
                    }
                }


                ModelState.AddModelError(string.Empty, "Invalid email or password");
            }
            return View(loginStaffVM);
        }

        /// <summary>
        /// Method to logout from the customer or the staff part
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        public IActionResult Logout(LoginVM loginVM)
        {
            //allow you to disconnect  
            HttpContext.Session.Clear();

            //return back to the login page
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Method to display the page where you can sign in
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CreateCustomerVM createCustomerVM)
        {
            if (ModelState.IsValid)
            {
                var listCustomer = CustomerManager.GetCustomers();
                foreach (var c in listCustomer)
                {
                    if (c.MAIL == createCustomerVM.MAIL)
                    {
                        ModelState.AddModelError(string.Empty, "An account already exists with this mail");
                        return View(createCustomerVM);
                    }
                }

                var cities = CityManager.GetCities();
                var idCity = -1;
                foreach (var city in cities)
                {
                    if (city.CITYNAME == createCustomerVM.CITYNAME)
                    {
                        idCity = city.IDCITY;
                    }
                }

                //If the idCity is not found, return an error
                if (idCity == -1)
                {
                    ModelState.AddModelError(string.Empty, "Wrong city entered, write correctly");
                    return View(createCustomerVM);
                }

                //verify if the data entered by the user will match the size in the database
                if (VerifyDataCustomer(createCustomerVM))
                {
                    var customer = new DTO.Customer();
                    customer.IDCITY = idCity;

                    customer.NAME = createCustomerVM.NAME;
                    customer.SURNAME = createCustomerVM.SURNAME;
                    customer.USERNAME = createCustomerVM.USERNAME;
                    customer.PHONE = createCustomerVM.PHONE;
                    customer.ADDRESS = createCustomerVM.ADDRESS;
                    customer.MAIL = createCustomerVM.MAIL;
                    customer.PASSWORD = CustomerManager.SetPassword(createCustomerVM.PASSWORD);

                    //Insert the new customer in the db
                    CustomerManager.InsertCustomer(customer);

                    //Send a registration mail to the new customer
                    new MailController().SendRegisterMail(createCustomerVM.MAIL, createCustomerVM.NAME);

                    return RedirectToAction("Index", "Login");
                }
                return View(createCustomerVM);
            }
            return RedirectToAction("Index", "Login");
        }

        private bool VerifyDataCustomer(CreateCustomerVM createCustomerVM)
        {
            //Control the length of the data before inserted in the db 
            if (createCustomerVM.NAME.Length > 25)
            {
                ModelState.AddModelError(string.Empty, "Name entered is too long");
                return false;
            }
            if (createCustomerVM.SURNAME.Length > 25)
            {
                ModelState.AddModelError(string.Empty, "Surname entered is too long");
                return false;
            }
            if (createCustomerVM.USERNAME.Length > 25)
            {
                ModelState.AddModelError(string.Empty, "Username entered is too long");
                return false;
            }
            if (createCustomerVM.PHONE.Length > 25)
            {
                ModelState.AddModelError(string.Empty, "Phone entered is too long");
                return false;
            }
            if (createCustomerVM.ADDRESS.Length > 25)
            {
                ModelState.AddModelError(string.Empty, "Address entered is too long");
                return false;
            }
            if (createCustomerVM.MAIL.Length > 50)
            {
                ModelState.AddModelError(string.Empty, "MAIL entered is too long");
                return false;
            }
            return true;
        }
    }
}

