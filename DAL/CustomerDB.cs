using DTO;
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
                    string query = "Select * from CUSTOMER";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Customer>();

                            Customer customer = new Customer();

                            customer.IDCUSTOMER = (int)dr["IDCUSTOMER"];

                            if (dr["NAME"] != null)
                                customer.NAME = (string)dr["NAME"];

                            if (dr["SURNAME"] != null)
                                customer.SURNAME = (string)dr["SURNAME"];

                            customer.USERNAME = (string)dr["USERNAME"];

                            customer.PHONE = (int)dr["PHONE"];

                            customer.ADDRESS = (string)dr["ADDRESS"];

                            customer.MAIL = (string)dr["MAIL"];

                            customer.PASSWORD = (string)dr["PASSWORD"];

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
                    string query = "Select * from CUSTOMER where email = @mail AND password = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    cn.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            customer = new Customer();

                            customer.IDCUSTOMER = (int)dr["IDCUSTOMER"];

                            if (dr["NAME"] != null)
                                customer.NAME = (string)dr["NAME"];

                            if (dr["SURNAME"] != null)
                                customer.SURNAME = (string)dr["SURNAME"];

                            if (dr["MAIL"] != null)
                                customer.MAIL = (string)dr["MAIL"];

                            if (dr["PASSWORD"] != null)
                                customer.PASSWORD = (string)dr["EMAIL"];

                            if (dr["USERNAME"] != null)
                                customer.USERNAME = (string)dr["USERNAME"];

                            customer.PHONE = (int)dr["PHONE"];

                            if (dr["ADDRESS"] != null)
                                customer.ADDRESS = (string)dr["ADDRESS"];

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
