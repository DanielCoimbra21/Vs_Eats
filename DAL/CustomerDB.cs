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

                            customer.IDCITY = (int)dr["IDCITY"];

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

        public Customer InsertCustomer(Customer customer)//Customer customer
        {
       
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using(SqlConnection cn = new SqlConnection(connectionString))
                {
                    //string query = "Insert into CUSTOMER(IDCITY, NAME, SURNAME, USERNAME, PHONE, ADDRESS, MAIL, PASSWORD) values(@IDCITY, @NAME, @SURNAME, @USERNAME, @PHONE, @ADDRESS, @MAIL, @PASSWORD)";
                    string query = "Insert into CUSTOMER(IDCITY, NAME, SURNAME, USERNAME, PHONE, ADDRESS, MAIL, PASSWORD) values(@idCity, @name, @surname, @username, @phone, @address, @mail, @password) SELECT SCOPE_IDENTITY()";
                    //string query = "Insert into CUSTOMER(IDCUSTOMER, IDCITY, NAME, SURNAME, USERNAME, PHONE, ADDRESS, MAIL, PASSWORD) values(@IDCUSTOMER, @IDCITY, @NAME, @SURNAME, @USERNAME, @PHONE, @ADDRESS, @MAIL, @PASSWORD) ";
                    SqlCommand cmd = new SqlCommand(query, cn);
                   
                    //cmd.Parameters.AddWithValue("@IDCUSTOMER", customer.IDCUSTOMER);
                    cmd.Parameters.AddWithValue("@idCity", customer.IDCITY);
                    cmd.Parameters.AddWithValue("@name", customer.NAME);
                    cmd.Parameters.AddWithValue("@surname", customer.SURNAME);
                    cmd.Parameters.AddWithValue("@username", customer.USERNAME);
                    cmd.Parameters.AddWithValue("@phone", customer.PHONE);
                    cmd.Parameters.AddWithValue("@address",customer. ADDRESS);
                    cmd.Parameters.AddWithValue("@mail", customer.MAIL);
                    cmd.Parameters.AddWithValue("@password", GetPassword(customer.PASSWORD));

                    cn.Open();

                    customer.IDCUSTOMER = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return customer;
        }


        public Customer GetCustomer(string email, string password)
        {
            Customer customer = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from CUSTOMER where MAIL = @email AND password = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", GetPassword(password));

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
                                customer.PASSWORD = (string)dr["PASSWORD"];

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


        public string GetPassword(string password)
        {

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using(SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * FROM CUSTOMER where PASSWORD = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@password", password);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if(dr.Read())
                        {
                            password = (string)dr["PASSWORD"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return password;

        }

        public Customer LoginCustomer(string email, string password)
        {
            Customer customer = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from CUSTOMER where MAIL = @email AND password = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
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
                                customer.PASSWORD = (string)dr["PASSWORD"];

                            if (dr["USERNAME"] != null)
                                customer.USERNAME = (string)dr["USERNAME"];

                            customer.PHONE = (int)dr["PHONE"];

                            if (dr["ADDRESS"] != null)
                                customer.ADDRESS = (string)dr["ADDRESS"];
                          
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return customer;
        }

    }
}
