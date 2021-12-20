using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ICustomerManager
    {


        public Customer GetCustomer(string email, string password);


        public List<Customer> GetCustomers();


        public Customer InsertCustomer(Customer customer);



        public string GetPassword(string password);


        public string HashPassword(string password);


        public Customer LoginCustomer(string email, string password);

        public Customer GetCustomerID(int id);
        

    }

}

