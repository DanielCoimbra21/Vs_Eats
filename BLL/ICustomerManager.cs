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
        public Customer GetCustomer(string email);
        //public Customer GetCustomer(string email, string password);

        public List<Customer> GetCustomers();
        public void UpdatePassword(Customer customer);

        public void InsertCustomer(Customer customer);


        public string HashPassword(string password);

        public Customer GetCustomerID(int id);

        public void UpdateCustomer(Customer customer);

        //public string HashPassword(string password, string salt);

        public string SetPassword(string password);

        public string GetPassword(string mail);


        public Boolean VerifyPassword(string passwordLogin, string passwordStored);


    }

}

