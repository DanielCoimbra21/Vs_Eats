using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
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
            return CustomerDb.InsertCustomer(customer);
        }

       
    }
}
