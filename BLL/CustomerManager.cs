using DTO;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CustomerManager
    {

        private ICustomerDB CustomerDb { get; set; }

        public CustomerManager(IConfiguration conf)
        {
            CustomerDb = new CustomerDB(conf);
        }

        public Customer GetCustomer(string email, string password)
        {
            return CustomerDb.GetCustomer(email, password);
        }

        public List<Customer> GetCustomers()
        {
            return CustomerDb.GetCustomers();
        }

        public Customer InsertCustomer(Customer customer)//Customer customer
        {
            var mail = customer.MAIL;
            var username = customer.USERNAME;

            var list = CustomerDb.GetCustomers();

            foreach(Customer customer1 in list)
            {
                if(customer1.MAIL.Equals(mail))
                {
                    Console.WriteLine("This Email Exists already");
                    customer = null;
                    return customer;
                }

                if(customer1.USERNAME.Equals(username))
                {
                    Console.WriteLine("This Username Exists already");
                    customer = null;
                    return customer;
                }
            }
            return CustomerDb.InsertCustomer(customer);


        }
       
    }
}
