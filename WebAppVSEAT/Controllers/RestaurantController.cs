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


        public RestaurantController(IDishesOrderManager dishesOrderManager,IOrderManager orderManager, ICityManager cityManager, IRestaurantManager restaurantManager, IDishManager dishManager, IDishesRestaurantManager dishesRestaurantManager)
        {
            RestaurantManager = restaurantManager;
            DishesRestaurantManager = dishesRestaurantManager;
            DishManager = dishManager;
            CityManager = cityManager;
            OrderManager = orderManager;
            DishesOrderManager = dishesOrderManager;
        }



        public IActionResult Index(string searchBy, string search)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rest = RestaurantManager.GetRestaurants();
            var restaurants_vm = new List<RestaurantVM>();


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


            if (searchBy == "Restaurant")
            {
                var restVM = new List<RestaurantVM>();
                foreach (var r in rest)
                {
                    if (r.NAMERESTAURANT.StartsWith(search))
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

            return View(restaurants_vm);
        }




        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var listDish = new List<int>();
            listDish = DishesRestaurantManager.GetListDishes(id);
            var dishesRestaurantVM = new List<DishesRestaurantVM>();


            if (listDish == null)
            {
                var error = new ErrorViewModel();
                return View(error);
            }

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

        public IActionResult TakeAnOrder(int id)
        {
            if (HttpContext.Session.GetInt32("IdCustomer") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //liste des id des plats dans le restaurant
            var listIDDishes = DishesRestaurantManager.GetListDishes(id);
            var dishes = new List<DTO.Dish>();

            //Ajout dans le tableau tous les plats liés au restaurant
            foreach (var idDish in listIDDishes)
            {
                dishes.Add(DishManager.GetDish(idDish));
            }

            var myModel = new CommandVM();
            myModel.IDRESTAURANT = id;
            myModel.orderDishes = new List<CommandVM>();


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TakeAnOrder(CommandVM commandVM)
        {
            int idRestaurant = -1; 

            if (ModelState.IsValid)
            {
                if(commandVM != null)
                {
                    //Calculer la somme de la livraison
                    double somme = 0;

                    foreach (var d in commandVM.orderDishes)
                    {
                        somme += d.dish.PRICEDISH * d.QUANTITY;
                        idRestaurant = d.IDRESTAURANT;
                    }

                    //Calculer la nouvelle date
                    DateTime dateTimeNow = DateTime.Now;
                    var hour = int.Parse(commandVM.hour.Split(":")[0]);
                    var minutes = int.Parse(commandVM.hour.Split(":")[1]);
                    DateTime myDeliveryTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, hour, minutes, 0);

                    //si l'heure de livraison est 30 minutes avant, pas possible
                    var diff = myDeliveryTime.Subtract(dateTimeNow);
                    
                    if (diff.TotalMilliseconds < 1800000)
                    {
                        ModelState.AddModelError(String.Empty, "Choose an another time, it is too short to deliver");
                        return View(commandVM);
                    }

                    //Définir le status
                    string status = "ongoing";

                    //Chercher l'id du customer
                    int idCustomer = (int)HttpContext.Session.GetInt32("IdCustomer");

                    //Trouver l'id du district
                    var city = CityManager.GetCity(RestaurantManager.GetRestaurant(idRestaurant).IDCITY);
                    int idDistrict = city.IDDISTRICT;


                    //Créer une nouvelle commande sans l'id du Staff
                    DTO.Order order = new DTO.Order();
                    order.IDDISTRICT = idDistrict;
                    order.IDRESTAURANT = idRestaurant;
                    order.IDCUSTOMER = idCustomer;
                    order.TOTALPRICE = (decimal)somme;
                    order.DELIVERTIME = myDeliveryTime;
                    order.STATUS = status;

                    //Trouver l'id du Staff
                    int idStaff = OrderManager.AssignStaff(order);

                    //Vérifier l'id du Staff
                    if (idStaff == -1)
                    {
                        ModelState.AddModelError(String.Empty, "No staff available. please choose an other delivery time");
                        return View(commandVM);
                    }

                    //Vérifier si l'heure n'est pas avant l'heure actuel
                    if (somme > 0)
                    {
                        if (myDeliveryTime < dateTimeNow)
                        {
                            ModelState.AddModelError(String.Empty, "Choose an another time, it passed");
                            return View(commandVM);
                        }

                        //Créer l'ordre avec l'id du Staff
                        var preOrder = OrderManager.InsertOrder(order, idStaff);


                        //Ajouter les plats dans DISHESORDER
                        //si la quantité est supérieur à 0
                        var idOrder = preOrder.IDORDER;
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

                        return View("~/Views/Restaurant/TakeAnOrderConfirmation.cshtml", commandVM);
                    }
                }
                ModelState.AddModelError(string.Empty, "Please, choose at least one dish");
            }
            return View(commandVM);
        }
    }
}
    
