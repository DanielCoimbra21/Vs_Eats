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
    public class RestaurantController : Controller
    {
        private IRestaurantManager RestaurantManager { get; }
        private IDishesRestaurantManager DishesRestaurantManager { get; }
        private IDishManager DishManager { get; }
        private ICityManager CityManager { get; }
        private IOrderManager OrderManager { get; }
        private IDishesOrderManager DishesOrderManager { get; }
        private ICustomerManager CustomerManager;
        private IStaffManager StaffManager;

        public RestaurantController(ICustomerManager customerManager, IStaffManager staffManager,IDishesOrderManager dishesOrderManager,IOrderManager orderManager, ICityManager cityManager, IRestaurantManager restaurantManager, IDishManager dishManager, IDishesRestaurantManager dishesRestaurantManager)
        {
            RestaurantManager = restaurantManager;
            DishesRestaurantManager = dishesRestaurantManager;
            DishManager = dishManager;
            CityManager = cityManager;
            OrderManager = orderManager;
            DishesOrderManager = dishesOrderManager;
            CustomerManager = customerManager;
            StaffManager = staffManager;
        }


        /// <summary>
        /// Method for the main page when we clicked on the button restaurant
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IActionResult Index(string searchBy, string search)
        {
            //Check if the user is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rest = RestaurantManager.GetRestaurants();
            var restaurants_vm = new List<RestaurantVM>();

            //Create the view model with all restaurants
            foreach (var r in rest)
            {
                var vm = new RestaurantVM();
                var city = CityManager.GetCity(r.IDCITY);
                vm.CITYNAME = city.CITYNAME;
                vm.IDRESTAURANT = r.IDRESTAURANT;
                vm.NAMERESTAURANT = r.NAMERESTAURANT;
                vm.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                restaurants_vm.Add(vm);
            }

            //Control if the search bar is null, display all restaurants
            if (search == null)
            {
                var rest_vm = new List<RestaurantVM>();
                foreach (var r in rest)
                {
                    var vm = new RestaurantVM();
                    var city = CityManager.GetCity(r.IDCITY);
                    vm.CITYNAME = city.CITYNAME;
                    vm.IDRESTAURANT = r.IDRESTAURANT;
                    vm.NAMERESTAURANT = r.NAMERESTAURANT;
                    vm.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                    rest_vm.Add(vm);
                }
                return View(rest_vm);
            }

            //Check if we display the restaurant by restaurant name
            if (searchBy == "Restaurant")
            {
                var restVM = new List<RestaurantVM>();
                foreach (var r in rest)
                {
                    if (r.NAMERESTAURANT.ToLower().StartsWith(search.ToLower()))
                    {

                        var vm = new RestaurantVM();
                        var city = CityManager.GetCity(r.IDCITY);

                        vm.CITYNAME = city.CITYNAME;
                        vm.IDRESTAURANT = r.IDRESTAURANT;
                        vm.NAMERESTAURANT = r.NAMERESTAURANT;
                        vm.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                        restVM.Add(vm);
                    }
                }

                return View(restVM);
            }

            //Check if we display by cityname
            if (searchBy == "City")
            {
                var restVM = new List<RestaurantVM>();
                var city = CityManager.GetCity(search);

                if(city == null)
                {
                    ModelState.AddModelError(String.Empty, "Wrong city entered");
                    return View(restVM);
                }

                foreach (var r in rest)
                {
                    if (r.IDCITY == city.IDCITY)
                    {
                        var vm = new RestaurantVM();
                        var cityR = CityManager.GetCity(r.IDCITY);
                        
                        vm.CITYNAME = cityR.CITYNAME;
                        vm.IDRESTAURANT = r.IDRESTAURANT;
                        vm.NAMERESTAURANT = r.NAMERESTAURANT;
                        vm.ADDRESSRESTAURANT = r.ADDRESSRESTAURANT;

                        restVM.Add(vm);
                    }
                }

                return View(restVM);
            }

            return View(restaurants_vm);
        }


        /// <summary>
        /// Method to list all dishes from a choosen restaurant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            //Check if the user is connected
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listDish = new List<int>();
            listDish = DishesRestaurantManager.GetListDishes(id);
            var dishesRestaurantVM = new List<DishesRestaurantVM>();

            //Verify is the listDish is null, then display an error model
            if (listDish == null)
            {
                var error = new ErrorViewModel();
                return View(error);
            }

            //Create the list of dishes from a restaurant
            foreach (var idDish in listDish)
            {
                var vm = new DishesRestaurantVM();
                var dish = DishManager.GetDish(idDish);
                vm.IDRESTAURANT = id;
                vm.NAMEDISH = dish.NAMEDISH;
                vm.PRICEDISH = dish.PRICEDISH;
                vm.IDDISHES = idDish;

                dishesRestaurantVM.Add(vm);
            }
            return View(dishesRestaurantVM);
        }


        /// <summary>
        /// Method when we want to make an order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult TakeAnOrder(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //List of id from a choosen restaurant
            var listIDDishes = DishesRestaurantManager.GetListDishes(id);
            var dishes = new List<DTO.Dish>();

            //Add to list dishes all dish which are related
            foreach (var idDish in listIDDishes)
            {
                dishes.Add(DishManager.GetDish(idDish));
            }

            var myModel = new CommandVM();
            myModel.IDRESTAURANT = id;

            //Initialize the new orderDishes in CommandVM
            myModel.orderDishes = new List<CommandVM>();

            //Control if the list of dishes is null, if yes return the view model
            if (dishes != null)
            {
                foreach (var dish in dishes)
                {
                    var myCityID = RestaurantManager.GetRestaurant(id).IDCITY;
                    CommandVM myDishVM = new CommandVM()
                    {
                        IDRESTAURANT = id,
                        QUANTITY = 0,
                        CITYNAME = CityManager.GetCity(myCityID).CITYNAME,
                        NAMERESTAURANT = RestaurantManager.GetRestaurant(id).NAMERESTAURANT,
                        dish = dish,
                        DELIVERTIME = DateTime.Now,
                        IDORDER = -1,
                    };
                    myModel.orderDishes.Add(myDishVM);
                }
            }
            return View(myModel);
        }


        /// <summary>
        /// Method to confirm the new order we make
        /// </summary>
        /// <param name="commandVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TakeAnOrder(CommandVM commandVM)
        {
            int idRestaurant = -1; 

            if (ModelState.IsValid)
            {
                if(commandVM != null)
                {
                    //Calculate the sum of the new order
                    double somme = 0;

                    foreach (var d in commandVM.orderDishes)
                    {
                        somme += d.dish.PRICEDISH * d.QUANTITY;
                        idRestaurant = d.IDRESTAURANT;
                    }

                    //Create the new datetime to insert after in the order
                    DateTime dateTimeNow = DateTime.Now;
                    var hour = int.Parse(commandVM.hour.Split(":")[0]);
                    var minutes = int.Parse(commandVM.hour.Split(":")[1]);
                    DateTime myDeliveryTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, hour, minutes, 0);

                    //If the order is made 30 minutes before the delivery, cannot do it
                    var diff = myDeliveryTime.Subtract(dateTimeNow);
                    
                    if (diff.TotalMilliseconds < 1800000)
                    {
                        ModelState.AddModelError(String.Empty, "Choose an another time, it is too short to deliver");
                        return View(commandVM);
                    }

                    //Initialize the status
                    string status = "ongoing";

                    //Get the idCustomer of the customer
                    int idCustomer = (int)HttpContext.Session.GetInt32("IdCustomer");

                    //Find the district of the restaurant to add to the order
                    var city = CityManager.GetCity(RestaurantManager.GetRestaurant(idRestaurant).IDCITY);
                    int idDistrict = city.IDDISTRICT;

                    /*
                     * Create an order without the id staff 
                     * to match the method AssignStaff who need an order
                    */
                    DTO.Order order = new DTO.Order();
                    order.IDDISTRICT = idDistrict;
                    order.IDRESTAURANT = idRestaurant;
                    order.IDCUSTOMER = idCustomer;
                    order.TOTALPRICE = (decimal)somme;
                    order.DELIVERTIME = myDeliveryTime;
                    order.STATUS = status;

                    //Find the id of the staff
                    int idStaff = OrderManager.AssignStaff(order);

                    //Verifiy the id of the staff
                    if (idStaff == -1)
                    {
                        ModelState.AddModelError(String.Empty, "No staff available. please choose an other delivery time");
                        return View(commandVM);
                    }

                    //Verify the time that is not before the actual time
                    if (myDeliveryTime < dateTimeNow)
                    {
                        ModelState.AddModelError(String.Empty, "Choose an another time, it passed");
                        return View(commandVM);
                    }

                    //Insert the order only if the sum is upper than 0
                    if (somme > 0)
                    {
                        
                        //Insert the new order in the database
                        var myOrder = OrderManager.InsertOrder(order, idStaff);


                        //Add the dishes in the table DISHESORDER
                        var idOrder = myOrder.IDORDER;
                        foreach (var o in commandVM.orderDishes)
                        {
                            if (o.QUANTITY > 0)
                            {
                                DTO.DishesOrder dishesOrder = new DTO.DishesOrder
                                {
                                    IDDISHES = o.dish.IDDISHES,
                                    IDORDER = idOrder,
                                    QUANTITY = o.QUANTITY
                                };
                                DishesOrderManager.InsertDishesOrder(dishesOrder);
                            }
                        }
                        commandVM.IDORDER = idOrder;

                        bool sent = new MailController(CustomerManager,StaffManager).SendOrderConfirmationMail(idCustomer,idStaff,idOrder,myDeliveryTime,somme);

                        if (!sent)
                        {
                            ModelState.AddModelError(string.Empty, "Order has been made, but the confirmation mail has not been sent");
                        }

                        return View("~/Views/Restaurant/TakeAnOrderConfirmation.cshtml", commandVM);
                    }
                }
                ModelState.AddModelError(string.Empty, "Please, choose at least one dish");
            }
            return View(commandVM);
        }
    }
}
    
