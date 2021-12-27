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
    public class OrderController : Controller
    {
        private IOrderManager OrderManager { get; }
        private ICustomerManager CustomerManager { get; }
        private ICityManager CityManager { get; }

        public OrderController(IOrderManager orderManager, ICustomerManager customerManager, ICityManager cityManager)
        {
            OrderManager = orderManager;
            CustomerManager = customerManager;
            CityManager = cityManager;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"));
            var listOrderVM = new List<OrderVM>();
            


            foreach (var o in listOrders)
            {
                var vm = new OrderVM();
                var customer = CustomerManager.GetCustomerID(o.IDCUSTOMER);
                var city = CityManager.GetCity(customer.IDCITY);

                vm.IDORDER = o.IDORDER;
                vm.NAME = customer.NAME;
                vm.SURNAME = customer.SURNAME;

                vm.ADDRESS = customer.ADDRESS;
                vm.CITY = city.CITYNAME;
                vm.NPA = city.NPA;

                vm.PHONE = customer.PHONE;

                vm.DELIVERTIME = o.DELIVERTIME;
                vm.TOTALPRICE = o.TOTALPRICE;
                vm.STATUS = o.STATUS;

                listOrderVM.Add(vm);
            }

            return View(listOrderVM);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var o = OrderManager.GetOrder(id);
            var vm = new OrderVM();
            var customer = CustomerManager.GetCustomerID(o.IDCUSTOMER);
            var city = CityManager.GetCity(customer.IDCITY);

            vm.IDORDER = o.IDORDER;
            vm.NAME = customer.NAME;
            vm.SURNAME = customer.SURNAME;

            vm.ADDRESS = customer.ADDRESS;
            vm.CITY = city.CITYNAME;
            vm.NPA = city.NPA;

            vm.PHONE = customer.PHONE;

            vm.DELIVERTIME = o.DELIVERTIME;
            vm.TOTALPRICE = o.TOTALPRICE;
            vm.STATUS = o.STATUS;

            return View(vm);
        }

        public IActionResult Archive(int id)
        {
            var order = OrderManager.GetOrder(id);
            OrderManager.ArchiveDelivery(order, "archived");
            
            
            return RedirectToAction("Index", "Order");
        }

        public IActionResult Historic()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listOrders = OrderManager.GetOrdersAll((int)HttpContext.Session.GetInt32("IdStaff"));
            var listOrderVM = new List<OrderVM>();



            foreach (var o in listOrders)
            {
                var vm = new OrderVM();
                var customer = CustomerManager.GetCustomerID(o.IDCUSTOMER);
                var city = CityManager.GetCity(customer.IDCITY);

                vm.IDORDER = o.IDORDER;
                vm.NAME = customer.NAME;
                vm.SURNAME = customer.SURNAME;

                vm.ADDRESS = customer.ADDRESS;
                vm.CITY = city.CITYNAME;
                vm.NPA = city.NPA;

                vm.PHONE = customer.PHONE;

                vm.DELIVERTIME = o.DELIVERTIME;
                vm.TOTALPRICE = o.TOTALPRICE;
                vm.STATUS = o.STATUS;

                listOrderVM.Add(vm);
            }

            return View(listOrderVM);

          
        }

    }
}
