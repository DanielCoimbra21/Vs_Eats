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

        private IRestaurantManager RestaurantManager { get; }
 
        private IDishesOrderManager DishesOrderManager { get; }


        public OrderController(IRestaurantManager restaurantManager, IDishesOrderManager dishesOrderManager ,IOrderManager orderManager, ICustomerManager customerManager, ICityManager cityManager)
        {
            OrderManager = orderManager;
            CustomerManager = customerManager;
            CityManager = cityManager;
            RestaurantManager = restaurantManager;
            DishesOrderManager = dishesOrderManager;
        }

        /// <summary>
        /// Affiche directement les commandes en cours du staff
        /// </summary>
        /// <returns></returns>
        public IActionResult OrdersStaff()
        {
            //Vérifier que l'utilisateur est bien connecté
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"),"ongoing");
            var listOrderVM = new List<OrderVM>();
            
            //Si liste des commandes est null, afficher une vue erreur
            if(listOrders == null)
            {
                return View("~/Views/Order/ErrorNoDelivery.cshtml");
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

        /// <summary>
        /// Méthode Edit pour apercevoir la commande et l'archiver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            //Vérification si le staff est bien connecté
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var o = OrderManager.GetOrder(id);

            //création de la nouvelle view model
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

        /// <summary>
        /// Méthode pour l'archivage des livraisons du staff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Archive(int id)
        {
            var order = OrderManager.GetOrder(id);
            OrderManager.ArchiveDelivery(order, "archived");
            
            return RedirectToAction("Index", "Order");
        }

        /// <summary>
        /// Méthode pour l'annulation de commande par le customer
        /// redirige vers la view CancelOrder pour l'annulation
        /// </summary>
        /// <returns></returns>
        public IActionResult CancelOrder()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public IActionResult CancelOrderID(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            CancelOrderVM vm = new CancelOrderVM();
            vm.IDORDER = id;
            
            return View(vm);
        }


        /// <summary>
        /// Méthode pour enregistrer l'annulation de la commande
        /// </summary>
        /// <param name="cancelOrderVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(CancelOrderVM cancelOrderVM)
        {
            //Order sera utilisé plus tard pour l'archivage
            DTO.Order order = OrderManager.GetOrder(cancelOrderVM.IDORDER);

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


                bool canceled = OrderManager.CancelOrder(customer, codeToValidate, cancelOrderVM.IDORDER);

                if (canceled != false)
                {
                    return RedirectToAction("Orders", "Order");
                }
                ModelState.AddModelError(string.Empty, "Wrong Name or Surname");
                return View(cancelOrderVM);

            }


            ModelState.AddModelError(string.Empty, "Order can't be cancelled less than 3h before the delivery");
            return View(cancelOrderVM);
        }

        
        /// <summary>
        /// Méthode pour afficher toutes les commandes d'un customer
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders()
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var idCustomer = (int)(HttpContext.Session.GetInt32("IdCustomer"));
            var listCustomerOrders = OrderManager.GetCustomerOrders(idCustomer);
            var listCustomerOrderVM = new List<CustomerOrderVM>();

            if(listCustomerOrders == null)
            {
                return View("~/Views/Order/ErrorNoCommand.cshtml");
            }

            foreach (var o in listCustomerOrders)
            {
                var vm = new CustomerOrderVM();
                vm.IDORDER = o.IDORDER;
                vm.DELIVERTIME = o.DELIVERTIME;

                listCustomerOrderVM.Add(vm);
            }

            return View(listCustomerOrderVM);
        }

        /// <summary>
        /// Méthode pour l'affichage de l'historique des commandes d'un staff
        /// </summary>
        /// <returns></returns>
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

        public IActionResult DetailsOrder(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var order = OrderManager.GetOrder(id);
            var restaurant = RestaurantManager.GetRestaurant(order.IDRESTAURANT);
            var dishesOrder = DishesOrderManager.GetDishesOrders(id);
            var dishes = new List<DTO.Dish>();
            var vms = new List<CommandVM>();


            foreach (var idDish in dishesOrder)
            {
                var vm = new CommandVM();
                vm.DELIVERTIME = order.DELIVERTIME;
                vm.QUANTITY = idDish.QUANTITY;
                vm.NAMERESTAURANT = restaurant.NAMERESTAURANT;
                vm.totalPrice = order.TOTALPRICE;

                vms.Add(vm);
            }

            return View(vms);



        }



    }
}
