using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StaffDB : IStaffDB
    {
        private IConfiguration Configuration { get; }
        public StaffDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Staff> GetStaffs()
        {
            List<Staff> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Staff";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Staff>();

                            Staff staff = new Staff();

                            staff.IDSTAFF = (int)dr["IDSTAFF"];
                            staff.IDCITY = (int)dr["IDCITY"];
                            staff.NAMESTAFF = (string)dr["NAMESTAFF"];
                            staff.SURNAMESTAFF = (string)dr["SURNAMESTAFF"];
                            staff.PHONENUMBERSTAFF = (int)dr["PHONENUMBERSTAFF"];
                            staff.ADDRESSSTAFF = (string)dr["ADDRESSSTAFF"];
                            staff.MAILSTAFF = (string)dr["MAILSTAFF"];
                            staff.USERNAMESTAFF = (string)dr["USERNAMESTAFF"];
                            staff.PASSWORDSTAFF = (string)dr["PASSWORDSTAFF"];
                            staff.ORDERCURRENTTOTAL = (int)dr["ORDERCURRENTTOTAL"];

                            results.Add(staff);
                        }
                            

                    }
                }
            }catch (Exception e)
            {
                throw e;
            }
            return results;
        }

        public Staff GetStaff(string usernameStaff, string passwordStaff)
        {
            Staff staff = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "Select * From STAFF where USERNAMESTAFF=@username and PASSWORDSTAFF=@password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@username", usernameStaff);
                    cmd.Parameters.AddWithValue("@password", passwordStaff);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            staff = new Staff();

                            staff.IDSTAFF = (int)dr["IDSTAFF"];
                            staff.IDCITY = (int)dr["IDCITY"];
                            staff.NAMESTAFF = (string)dr["NAMESTAFF"];
                            staff.SURNAMESTAFF = (string)dr["SURNAMESTAFF"];
                            staff.PHONENUMBERSTAFF = (int)dr["PHONENUMBERSTAFF"];
                            staff.ADDRESSSTAFF = (string)dr["ADDRESSSTAFF"];
                            staff.MAILSTAFF = (string)dr["MAILSTAFF"];
                            staff.USERNAMESTAFF = (string)dr["USERNAMESTAFF"];
                            staff.PASSWORDSTAFF = (string)dr["PASSWORDSTAFF"];
                            staff.ORDERCURRENTTOTAL = (int)dr["ORDERCURRENTTOTAL"];
    
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Username or Password incorrect");
                throw e;
            }
            return staff;
        }

        public Staff GetStaff(int idStaff)
        {
            Staff staff = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "Select * From STAFF where IDSTAFF=@idStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", idStaff);
                

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            staff = new Staff();

                            staff.IDSTAFF = (int)dr["IDSTAFF"];
                            staff.IDCITY = (int)dr["IDCITY"];
                            staff.NAMESTAFF = (string)dr["NAMESTAFF"];
                            staff.SURNAMESTAFF = (string)dr["SURNAMESTAFF"];
                            staff.PHONENUMBERSTAFF = (int)dr["PHONENUMBERSTAFF"];
                            staff.ADDRESSSTAFF = (string)dr["ADDRESSSTAFF"];
                            staff.MAILSTAFF = (string)dr["MAILSTAFF"];
                            staff.USERNAMESTAFF = (string)dr["USERNAMESTAFF"];
                            staff.PASSWORDSTAFF = (string)dr["PASSWORDSTAFF"];
                            staff.ORDERCURRENTTOTAL = (int)dr["ORDERCURRENTTOTAL"];

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Username or Password incorrect");
                throw e;
            }
            return staff;
        }

        public void UpdateStaff(Staff staff)
        {
           
            string connectionString = Configuration.GetConnectionString("DefaultConnection");  
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    

                    var query = "UPDATE STAFF " +
                        "SET NAMESTAFF = @nameStaff, SURNAMESTAFF=@surnameStaff, PHONENUMBERSTAFF=@phoneNumber, " +
                            "ADDRESSSTAFF=@addressStaff, MAILSTAFF=@mailStaff " +
                        "WHERE USERNAMESTAFF=@usernameStaff and PASSWORDSTAFF=@passwordStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@nameStaff", staff.NAMESTAFF);
                    cmd.Parameters.AddWithValue("@surnameStaff", staff.SURNAMESTAFF);
                    cmd.Parameters.AddWithValue("@phoneNumber", staff.PHONENUMBERSTAFF);
                    cmd.Parameters.AddWithValue("@addressStaff", staff.ADDRESSSTAFF);
                    cmd.Parameters.AddWithValue("@mailStaff", staff.MAILSTAFF);
                    cmd.Parameters.AddWithValue("@usernameStaff", staff.USERNAMESTAFF);
                    cmd.Parameters.AddWithValue("@passwordStaff", staff.PASSWORDSTAFF);
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();

                }
            }catch(Exception e)
            {
                throw e;
            }
        }

        public List<Order> GetUpComingOrder(int idStaff, string status)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            List <Order> results = null;
            
            try
            {
                using(SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "Select * FROM Order WHERE IDSTAFF=@idStaff and STATUS=@status";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", idStaff);
                    cmd.Parameters.AddWithValue("@status", status);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Order>();

                            Order order = new Order();

                            order.IDORDER = (int)dr["IDORDER"];
                            order.IDDISTRICT = (int)dr["IDDISTRICT"];
                            order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                            order.IDSTAFF = (int)dr["IDSTAFF"];
                            order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                            order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                            order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                            order.STATUS = (string)dr["STATUS"];

                            results.Add(order);
                        }
                    }   
                }
            }catch(Exception e)
            {
                throw e;
            }
            return results;
        }

        
        
    }
}
