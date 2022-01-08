using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ICustomerDB
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(string mail);
        void InsertCustomer(Customer customer);

        string SetPassword(string password);
        string GetPassword(string mail);
        Customer GetCustomerID(int id);
        public void UpdatePassword(Customer customer);
        void UpdateCustomer(Customer customer);      
    }
}
