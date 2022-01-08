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

        //public void SetPassword(int idCustomer, string password)
        //{
        //   CustomerDb.SetPassword(idCustomer,  password);
        //}


        private string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public string HashPassword(string password)
        {

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            //System.Security.Cryptography.SHA256Managed sha256hashString = new System.Security.Cryptography.SHA256Managed();
            //byte[] hash = sha256hashString.ComputeHash(bytes);

            //string bitString = BitConverter.ToString(hash);

            //return bitString;

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);

            return Convert.ToBase64String(encrypted_bytes);
        }

        public string SetPassword(string password)
        {
            //methode pour hasher les mots de passe, pas réussi
            string salt = CreateSalt(10);
            string hashedPassword = HashPassword(password);
            string fullHashPassword = string.Concat("$" + salt + "$" + hashedPassword);
            //string fullHashPassword = hashedPassword;

            return CustomerDb.SetPassword(fullHashPassword);
        }

        public Boolean VerifyPassword(string passwordLogin, string mail)
        {
            string passwordCustomer = CustomerDb.GetPassword(mail);
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
