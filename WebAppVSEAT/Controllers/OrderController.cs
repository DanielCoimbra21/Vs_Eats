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
            //Vérifier que l'utilisateur est bien connecté
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"),"ongoing");
            var listOrderVM = new List<OrderVM>();
            
            if(listOrders == null)
            {
                return View("~/Views/Order/ErrorNoCommand.cshtml");
            }

            //vérifier que si il n'y a pas de commande pas d'erreur????
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

        public IActionResult CancelOrder()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(CancelOrderVM cancelOrderVM)
        {
            //Order sera utilisé plus tard pour l'archivage
            DTO.Order order = OrderManager.GetOrder(cancelOrderVM.IDORDER);
            //long msTN = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //DateTime timeNow = DateTime.Now;
            //long msTN = timeNow.Millisecond;
            //DateTime myDeliveryTime = order.DELIVERTIME;
            //long msOD = myDeliveryTime.ToUnixTimeMilliseconds();

            DateTime timeNow = DateTime.Now;
            TimeSpan diff = order.DELIVERTIME - timeNow;

            long threeHours = OrderManager.ConvertHoursToMiliseconds(3);


            //Condition vérification si l'heure de l'ordre et la l'heure d'aujourd'hui
            //est plus grande que 3 heures
            if (diff.TotalMilliseconds > threeHours)
            {
                int idCustomer = OrderManager.GetOrder(cancelOrderVM.IDORDER).IDCUSTOMER;
                DTO.Customer customer = CustomerManager.GetCustomerID(idCustomer);
                string codeToValidate = String.Concat(cancelOrderVM.IDORDER, cancelOrderVM.NAME, cancelOrderVM.SURNAME);


                OrderManager.CancelOrder(customer, codeToValidate, cancelOrderVM.IDORDER);
                return RedirectToAction("Orders", "Order");
            }

            ModelState.AddModelError(string.Empty, "To late to cancel");


            return View(cancelOrderVM);
        }

        

        public IActionResult Orders()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var idCustomer = (int)(HttpContext.Session.GetInt32("IdCustomer"));
            var listCustomerOrders = OrderManager.GetCustomerOrders(idCustomer);
            var listCustomerOrderVM = new List<CustomerOrderVM>();

            foreach (var o in listCustomerOrders)
            {
                var vm = new CustomerOrderVM();
                vm.IDORDER = o.IDORDER;
                vm.DELIVERTIME = o.DELIVERTIME;

                listCustomerOrderVM.Add(vm);
            }
            return View(listCustomerOrderVM);
        }

        public IActionResult Historic()
        {
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"));
            var listOrderVM = new List<OrderVM>();
             
            /*
             *  si le staff est nouveau, il n'y aura pas de commande -> pas d'historique
             *  page pour annoncer aucun historique
             */
            if (listOrders == null)
            {
                return View("~/Views/Order/ErrorNoHistoric.cshtml");
            }

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
