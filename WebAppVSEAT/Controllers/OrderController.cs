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
        /// Display the orders of the Staff
        /// </summary>
        /// <returns></returns>
        public IActionResult OrdersStaff()
        {
            //Verify that the user is connected
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"), "ongoing");
            var listOrderVM = new List<OrderVM>();

            //The order can be null, so we display an error page with a message "No command"
            if (listOrders == null)
            {
                return View("~/Views/Order/ErrorNoDelivery.cshtml");
            }

            //Create view model
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
        /// Method to archive the commands from the staff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Archive(int id)
        {
            //Control if the staff is connected
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = OrderManager.GetOrder(id);
            OrderManager.ArchiveDelivery(order, "archived");

            return RedirectToAction("OrdersStaff", "Order");
        }

        /// <summary>
        /// Method to cancel an order by the customer
        /// </summary>
        /// <returns></returns>
        public IActionResult CancelOrder()
        {
            //Verifiy is the customer is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        /// <summary>
        /// Method to cancel an order by the customer by selecting the right order id
        /// </summary>
        /// <returns></returns>
        public IActionResult CancelOrderID(int id)
        {
            //Verifiy is the customer is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            CancelOrderVM vm = new CancelOrderVM();
            vm.IDORDER = id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(CancelOrderVM cancelOrderVM)
        {
            DTO.Order order = OrderManager.GetOrder(cancelOrderVM.IDORDER);

            //Verify if the order exist
            if (order == null)
            {
                ModelState.AddModelError(string.Empty, "This order doesn't exist");
                return View(cancelOrderVM);
            }

            //Verify the status of the order, cannot cancel an order that is not created or has been already canceld
            if (!order.STATUS.Equals("ongoing"))
            {
                ModelState.AddModelError(string.Empty, "Order has been dealt with");
                return View(cancelOrderVM);
            }

            //Verify if the customer who is logged have order this
            if (order.IDCUSTOMER != HttpContext.Session.GetInt32("IdCustomer"))
            {
                ModelState.AddModelError(string.Empty, "Cannot canceled an order you didn't make");
                return View(cancelOrderVM);
            }

            DateTime timeNow = DateTime.Now;
            TimeSpan diff = order.DELIVERTIME - timeNow;

            long threeHours = OrderManager.ConvertHoursToMiliseconds(3);
            
            //Verify if the order can be canceld, condition : 3 hours before        
            if (diff.TotalMilliseconds > threeHours)
            {
                int idCustomer = OrderManager.GetOrder(cancelOrderVM.IDORDER).IDCUSTOMER;
                DTO.Customer customer = CustomerManager.GetCustomerID(idCustomer);
                string codeToValidate = String.Concat(cancelOrderVM.IDORDER, cancelOrderVM.NAME, cancelOrderVM.SURNAME);

                bool canceled = OrderManager.CancelOrder(customer, codeToValidate, cancelOrderVM.IDORDER);

                if (canceled != false)
                {
                    new MailController().SendCancelOrderMail(customer.MAIL, cancelOrderVM.IDORDER);
                    return RedirectToAction("Orders", "Order");
                }
                ModelState.AddModelError(string.Empty, "Wrong Name or Surname");
                return View(cancelOrderVM);
            }

            ModelState.AddModelError(string.Empty, "Order can't be cancelled less than 3h before the delivery");
            return View(cancelOrderVM);
        }


        /// <summary>
        /// Method to display all the command from a customer
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders()
        {
            //Verifiy is the customer is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var idCustomer = (int)(HttpContext.Session.GetInt32("IdCustomer"));
            var listCustomerOrders = OrderManager.GetCustomerOrders(idCustomer);
            var listCustomerOrderVM = new List<CustomerOrderVM>();

            //verify if the list order is null, then show an error command
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
        /// Method to display all the command from a staff
        /// </summary>
        /// <returns></returns>
        public IActionResult Historic()
        {
            //Verifiy is the staff is connected
            if (HttpContext.Session.GetInt32("IdStaff") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listOrders = OrderManager.GetOrders((int)HttpContext.Session.GetInt32("IdStaff"));
            var listOrderVM = new List<OrderVM>();

            /*
             * If the staff is new, there will be no command, so no historic
             *  display the an error page
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
        /// Method to display the command made by the customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DetailsOrder(int id)
        {
            //Verifiy is the customer is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Get order, restaurant and city to create a new view model
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
        /// Method to modify the command
        /// </summary>
        /// <returns></returns>
        public IActionResult EditOrder(int id)
        {
            //Verifiy is the customer is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Search the information for the order
            var order = OrderManager.GetOrder(id);
            var dishesOrder = DishesOrderManager.GetDishesOrders(id);
            var restaurant = RestaurantManager.GetRestaurant(order.IDRESTAURANT);
            DateTime deliveryTime = order.DELIVERTIME; 

            //Creation of the view
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
                    //Get the command from the view model
                    var order = OrderManager.GetOrder(editOrderVM.IDORDER);

                    //Compare the hour of the command with the actual hour and verify if we can canceld
                    var myDeliveryTime = order.DELIVERTIME;

                    if (DateTime.Compare(DateTime.Now, myDeliveryTime) > 0)
                    {
                        ModelState.AddModelError(String.Empty, "Cannot modify your order because the date passed");
                        return View(editOrderVM);
                    }

                    //if the deliverytime is 30 minutes before the choosen deliverytime
                    var diff = myDeliveryTime.Subtract(DateTime.Now);

                    if (diff.TotalMilliseconds < 1800000)
                    {
                        ModelState.AddModelError(String.Empty, "Cannot modify your order 30 minutes before delivery");
                        return View(editOrderVM);
                    }

                    //Calculate the sum of the order
                    double somme = 0;

                    foreach (var d in editOrderVM.orderDishes)
                    {
                        somme += d.PRICEDISH * d.QUANTITY;
                    }
 
                    //Only update the datable dishesorder if the sum is over zero
                    if (somme > 0)
                    {
                        order.TOTALPRICE = (decimal)somme;

                        OrderManager.UpdateOrder(order);


                        //Add dishes in the table DISHESORDER
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
