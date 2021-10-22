using Microsoft.Ajax.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CustomerDB : ICustomerDB
    {
        private IConfiguration Configuration { get; }

        public CustomerDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Customer";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Customer>();

                            Customer customer = new Customer();

                            customer.IdCustomer = (int)dr["idCustomer"];

                            if (dr["Name"] != null)
                                customer.Name = (string)dr["Name"];

                            if (dr["Surname"] != null)
                                customer.Surname = (string)dr["Surname"];

                            customer.Mail = (string)dr["Email"];

                            customer.Password = (string)dr["Password"];

                            results.Add(customer);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }

        public Customer GetCustomer(string email, string password)
        {
            Customer customer = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Customer where email = @mail AND password = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    cn.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            customer = new Customer();

                            customer.IdCustomer = (int)dr["idCustomer"];

                            if (dr["Name"] != null)
                                customer.Name = (string)dr["Name"];

                            if (dr["Surname"] != null)
                                customer.Surname = (string)dr["Surname"];

                            if (dr["Email"] != null)
                                customer.Mail = (string)dr["Email"];
                            if (dr["Password"] != null)
                                customer.Password = (string)dr["Email"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return customer;
        }
    }
}
