using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using DTO;
using DAL;
using BLL;

namespace Vs_Eats
{
    public class Program
    {

        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();


        public static void Main(string[] args)
        {
           

            //var customerManager = new CustomerManager(Configuration);

            //Console.WriteLine("Exercise to insert Customers");
            //var newCustomer = customerManager.InsertCustomer(new Customer { IDCITY = 1, ADDRESS = "Le coin des BGS", MAIL = "Hash9@hash.ch", NAME = "HashTest", PASSWORD = "1234", PHONE = 1234, SURNAME = "HashTest", USERNAME = "HashTest9" });

            //Console.WriteLine("Exercice Insert Order");
            //var orderManager = new OrderManager(Configuration);

            //var src = DateTime.Now;
            //var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0);

            //var newOrder = orderManager.InsertOrder(new Order { IDDISTRICT = 1, IDRESTAURANT = 1, IDCUSTOMER = 1, TOTALPRICE = 20, DELIVERTIME = hm, STATUS="Free"});

            //Console.WriteLine("Check if login is correct");
            //var newCustomer = customerManager.LoginCustomer("thomas@bg.ch", "WESHHALORS");
            //Console.WriteLine("ON EST Là");

            //var customer = new CustomerManager(Configuration);
            //Console.WriteLine("Exercise List of all customers");

            //var customers =  customer.GetCustomers();

            //foreach(var c in customers)
            //{
            //    Console.WriteLine(c.ToString());
            //}
            
            var city = new CityManager(Configuration);          
            var cityN = city.GetCity(0);
            var cityName = cityN.CITYNAME;
            Console.WriteLine(cityName);

            //Exercise List of all cities
            Console.WriteLine("Exercise List of all cities");
            var cities = city.GetCities();

            foreach (var m in cities)
            {
                Console.WriteLine(m.ToString());
            }

            //Exercise List of all Districts
            //var district = new DistrictManager(Configuration);
            //Console.WriteLine("Exercise list of all districts");

            //var districts = district.GetDistricts();

            //foreach(var d in districts)
            //{
            //    Console.WriteLine(d.ToString());
            //}


            //Exercise List of all Dishes
            //var dish = new DishManager(Configuration);
            //Console.WriteLine("Exercise list of all Dishes");

            //var dishes = dish.GetDishes();

            //foreach(var dh in dishes)
            //{
            //   Console.WriteLine(dh.ToString());
            //}

            //Update orderStatus
            //Console.WriteLine("Update orderStatus");
            //var order = new OrderManager(Configuration);
            //var orderSelected = order.GetOrder(0);
            //Console.WriteLine(orderSelected.ToString());
            //order.ArchiveDelivery(orderSelected, "terminé");
            //var orderSelected1 = order.GetOrder(0);
            //Console.WriteLine(orderSelected1.ToString());

            //Cancel an order
            //Console.WriteLine("Cancel an order");
            //var orderC = new OrderManager(Configuration);
            //var orderToCancel = orderC.GetOrder(0);
            //var customerC = new CustomerManager(Configuration);
            //var customerCurrent = customerC.GetCustomer("dsaf", "1234");
            //orderC.CancelOrder(customerCurrent, orderToCancel.IDORDER, "0CoimbraDaniel");
            //Console.WriteLine(orderToCancel.ToString());


            //Assign order
            //Console.WriteLine("Assign order");
            //var od = new OrderManager(Configuration);
            //var idStaffChosen = od.AssignStaff(0);

            //Console.WriteLine("Staffs choisis");
            //Console.WriteLine("IdStaff :" + idStaffChosen);
            


        }
    }
}
