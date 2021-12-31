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

        public string GetPassword(string password)
        {
            //methode pour hasher les mots de passe, pas réussi
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

        public string HashPassword(string password)
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

        public Customer LoginCustomer(string email, string password )
        {
            List<Customer> customerList = CustomerDb.GetCustomers();
            Customer customer = null;
            
            //parcourir tous les clients
            foreach (var cs in customerList)
            {
                //si le mot de pass et le username se trouve dans la base de données
                if (cs.MAIL.Equals(email) && cs.PASSWORD.Equals(password))
                {
                    Console.WriteLine("LOGIN CORRECT");
                    //return CustomerDb.LoginCustomer(email, password);
                    return customer = cs;
                    
                }

            }

            //sinon message d'erreur s'affiche
            Console.WriteLine("Login or Password Incorrect");

            return customer;
            
        }
       
    }
}
