using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using DTO;
using DAL;

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

            var customer = new CustomerManager(Configuration);
            Console.WriteLine("Exercise List of all customers");

            var customers =  customer.GetCustomers();

            foreach(var c in customers)
            {
                Console.WriteLine(c.ToString());
            }
            
            var city = new CityManager(Configuration);
            //Exercise List of all cities
            Console.WriteLine("Exercise List of all cities");

            var cities = city.GetCity();

            foreach (var m in cities)
            {
                Console.WriteLine(m.ToString());
            }
        }
    }
}
