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
        private IDishManager DishManager { get; }


        public OrderController(IDishManager dishManager, IRestaurantManager restaurantManager, IDishesOrderManager dishesOrderManager, IOrderManager orderManager, ICustomerManager customerManager, ICityManager cityManager)
        {
            OrderManager = orderManager;
            CustomerManager = customerManager;
            CityManager = cityManager;
            RestaurantManager = restaurantManager;
            DishesOrderManager = dishesOrderManager;
            DishManager = dishManager;
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


            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"), "ongoing");
            var listOrderVM = new List<OrderVM>();

            //Si liste des commandes est null, afficher une vue erreur
            if (listOrders == null)
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
        /// Méthode pour l'archivage des livraisons du staff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Archive(int id)
        {
            //Vérification si le staff est bien connecté
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = OrderManager.GetOrder(id);
            OrderManager.ArchiveDelivery(order, "archived");

            return RedirectToAction("OrdersStaff", "Order");
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

            //vérification si le numéro d'ordre existe dans la base de données
            if (order == null)
            {
                ModelState.AddModelError(string.Empty, "This order doesn't exist");
                return View(cancelOrderVM);
            }

            //vérification du status de l'order id
            if (!order.STATUS.Equals("ongoing"))
            {
                ModelState.AddModelError(string.Empty, "Order has been dealt with");
                return View(cancelOrderVM);
            }

            //vérification si le customer logger a bien fait cette commande
            if (order.IDCUSTOMER != HttpContext.Session.GetInt32("IdCustomer"))
            {
                ModelState.AddModelError(string.Empty, "Cannot canceled an order you didn't make");
                return View(cancelOrderVM);
            }



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

            if (listCustomerOrders == null)
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
        /// <summary>
        /// Methode pour l'affichage détaillé de la commande faite par le customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DetailsOrder(int id)
        {
            //Vérifier si le customer est bien connecté
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = OrderManager.GetOrder(id);
            var restaurant = RestaurantManager.GetRestaurant(order.IDRESTAURANT);
            var city = CityManager.GetCity(restaurant.IDCITY);

            var vm = new CustomerDetailOrderVM();
            vm.IDORDER = id;
            vm.CITYNAME = city.CITYNAME;
            vm.NAMERESTAURANT = restaurant.NAMERESTAURANT;
            vm.totalPrice = order.TOTALPRICE;
            vm.DELIVERTIME = order.DELIVERTIME;

            var dishesOrder = DishesOrderManager.GetDishesOrders(id);
            vm.orderDishes = new List<DishesOrderVM>();

            foreach (var idDish in dishesOrder)
            {
                var vmDish = new DishesOrderVM();
                vmDish.IDDISHES = idDish.IDDISHES;
                vmDish.IDRESTAURANT = restaurant.IDRESTAURANT;
                var dish = DishManager.GetDish(idDish.IDDISHES);
                vmDish.NAMEDISH = dish.NAMEDISH;
                vmDish.PRICEDISH = dish.PRICEDISH;
                vmDish.QUANTITY = idDish.QUANTITY;

                vm.orderDishes.Add(vmDish);
            }

            return View(vm);
        }

        /// <summary>
        /// Methode pour la modification de la commande
        /// </summary>
        /// <returns></returns>
        public IActionResult EditOrder(int id)
        {
            //Vérifier si le customer est bien connecté
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //rechercher les informations pour order
            var order = OrderManager.GetOrder(id);
            var dishesOrder = DishesOrderManager.GetDishesOrders(id);
            var restaurant = RestaurantManager.GetRestaurant(order.IDRESTAURANT);
            DateTime deliveryTime = order.DELIVERTIME; 

            //créer la view
            var vm = new EditOrderVM();
            vm.IDRESTAURANT = restaurant.IDRESTAURANT;
            vm.NAMERESTAURANT = restaurant.NAMERESTAURANT;
            vm.IDORDER = id;
            vm.orderDishes = new List<DishesOrderVM>();

            foreach (var dish in dishesOrder)
            {
                var vmDish = new DishesOrderVM();
                var d = DishManager.GetDish(dish.IDDISHES);
                vmDish.IDRESTAURANT = restaurant.IDRESTAURANT;
                vmDish.IDDISHES = d.IDDISHES;
                vmDish.NAMEDISH = d.NAMEDISH;
                vmDish.PRICEDISH = d.PRICEDISH;
                vmDish.QUANTITY = dish.QUANTITY;
                vmDish.IDRESTAURANT = restaurant.IDRESTAURANT;
                vm.orderDishes.Add(vmDish);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrder(EditOrderVM editOrderVM)
        {
            if (ModelState.IsValid)
            {
                if (editOrderVM != null)
                {
                    //Récupération de la commande
                    var order = OrderManager.GetOrder(editOrderVM.IDORDER);


                    //si l'heure de livraison est 30 minutes avant, pas possible de modifier
                    var myDeliveryTime = order.DELIVERTIME;
                    var diff = myDeliveryTime.Subtract(DateTime.Now);

                    if (diff.TotalMilliseconds < 1800000)
                    {
                        ModelState.AddModelError(String.Empty, "Cannot modify your order 30 minutes before delivery");
                        return View(editOrderVM);
                    }

                    //Calculer la somme de la livraison
                    double somme = 0;

                    foreach (var d in editOrderVM.orderDishes)
                    {
                        somme += d.PRICEDISH * d.QUANTITY;
                    }

                    //Vérifier si l'heure n'est pas avant l'heure actuel
                    if (somme > 0)
                    {
                        //mettre à jour l'order avec les nouvelles valeurs
                        order.TOTALPRICE = (decimal)somme;

                        OrderManager.UpdateOrder(order);


                        //Ajouter les plats dans DISHESORDER
                        //si la quantité est supérieur à 0
                        var idOrder = editOrderVM.IDORDER;
                        foreach (var o in editOrderVM.orderDishes)
                        {
                            if (o.QUANTITY > 0)
                            {
                                DTO.DishesOrder dishesOrder = new DTO.DishesOrder
                                {
                                    IDDISHES = o.IDDISHES,
                                    IDORDER = idOrder,
                                    QUANTITY = o.QUANTITY
                                };
                                DishesOrderManager.UpdateDishesOrder(dishesOrder);
                            }
                        }
                    }
                    return RedirectToAction("Orders", "Order");
                }
                ModelState.AddModelError(string.Empty, "Cannot command with zero quantity");
            }
            return View(editOrderVM);
        }
    }
}
