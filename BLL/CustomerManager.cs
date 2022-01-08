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
    public class CustomerManager : ICustomerManager
    {

        private ICustomerDB CustomerDb { get; set; }

        public void UpdatePassword(Customer customer)
        {
            CustomerDb.UpdatePassword(customer);
        }
 

        public CustomerManager(IConfiguration conf)
        {
            CustomerDb = new CustomerDB(conf);
        }

        public Customer GetCustomer(string email)
        {
            return CustomerDb.GetCustomer(email);
        }

        public List<Customer> GetCustomers()
        {
            return CustomerDb.GetCustomers();
        }

        public Customer GetCustomerID(int id)
        {
            return CustomerDb.GetCustomerID(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            CustomerDb.UpdateCustomer(customer);
        }

        public void InsertCustomer(Customer customer)//Customer customer
        { 
            CustomerDb.InsertCustomer(customer);
        }

        public string GetPassword(string mail)
        {
            return CustomerDb.GetPassword(mail);
        }

        /// <summary>
        /// Method to create the salt we are using to encrypt the password
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Method to hash the password with the entered password from the customer 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            //Change the password in an array of byte
            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            //Hash the password byte[] with the sha1
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);

            /*
             * Converts the value of an array of 8-bit 
             * unsigned integers to its equivalent string representation that is encoded with base-64 digits.
             */
            return Convert.ToBase64String(encrypted_bytes);
        }

        /// <summary>
        /// Method to set the password in the db
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string SetPassword(string password)
        {
            string salt = CreateSalt(10);
            string hashedPassword = HashPassword(password);
            string fullHashPassword = string.Concat("$" + salt + "$" + hashedPassword);    
            return fullHashPassword;
        }

        /// <summary>
        /// Method to verify the password entered by the user
        /// </summary>
        /// <param name="passwordLogin"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public Boolean VerifyPassword(string passwordLogin, string mail)
        {
            //Get the password from the customer
            string passwordCustomer = CustomerDb.GetPassword(mail);
            //Find the salt in the password
            string salt = passwordCustomer.Substring(1, passwordCustomer.LastIndexOf("$"));
            string passwordSansSalt = passwordCustomer.Remove(0, passwordCustomer.LastIndexOf('$') + 1);

            string passwordGiven = HashPassword(passwordLogin);

            if (passwordGiven == passwordSansSalt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
