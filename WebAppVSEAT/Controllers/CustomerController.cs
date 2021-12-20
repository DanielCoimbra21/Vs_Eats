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
        

        public CustomerController(ICustomerManager customerManager)
        {
            CustomerManager = customerManager;
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = CustomerManager.GetCustomerID((int)HttpContext.Session.GetInt32("IdCustomer"));
            var customerList = new CustomerVM();

            customerList.NAME = customer.NAME;
            customerList.SURNAME = customer.SURNAME;
            customerList.MAIL = customer.MAIL;
            customerList.PHONE = customer.PHONE;


            return View(customerList);
        }
    }
}
