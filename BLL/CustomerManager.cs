using DTO;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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

        public string GetPassword(string password)
        {
            //string salt = CreateSalt(10);
            string hashedPassword = HashPassword(password);
            

            return CustomerDb.GetPassword(hashedPassword);
        }

       /* private string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }*/

        private string HashPassword(string password)
        {

           /* byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            System.Security.Cryptography.SHA256Managed sha256hashString = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashString.ComputeHash(bytes);

            string bitString = BitConverter.ToString(hash);*/

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);

            return Convert.ToBase64String(encrypted_bytes);
        }

        public List<Customer> LoginCustomer(string email, string password )
        {
           
            //if the email and password are correct they will be place in this list, so its size will be 2
            if(LoginCustomer(email, password).Count < 2)
            {
                Console.WriteLine("Email or Password incorrect");
                return null;
            }
            else
            {
                return CustomerDb.LoginCustomer(email, password);
                Console.WriteLine("LOGIN CORRECT");
            }
            
            
        }
       
    }
}
