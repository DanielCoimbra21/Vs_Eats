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

        public Staff GetStaff(string mailStaff, string passwordStaff)
        {
            Staff staff = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * From STAFF where MAILSTAFF = @mailStaff AND PASSWORDSTAFF = @password";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@mailStaff", mailStaff);
                    cmd.Parameters.AddWithValue("@password", passwordStaff);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
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
                            if (dr["ORDERCURRENTTOTAL"] != null)
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
                            if (dr["ORDERCURRENTTOTAL"] != null)
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
                        "SET IDCITY=@idCity,NAMESTAFF = @nameStaff, SURNAMESTAFF=@surnameStaff, PHONENUMBERSTAFF=@phoneNumber, " +
                            "ADDRESSSTAFF=@addressStaff, MAILSTAFF=@mailStaff, USERNAMESTAFF=@usernameStaff " +
                        "WHERE IDSTAFF=@idStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", staff.IDSTAFF);
                    cmd.Parameters.AddWithValue("@idCity", staff.IDCITY);
                    cmd.Parameters.AddWithValue("@nameStaff", staff.NAMESTAFF);
                    cmd.Parameters.AddWithValue("@surnameStaff", staff.SURNAMESTAFF);
                    cmd.Parameters.AddWithValue("@phoneNumber", staff.PHONENUMBERSTAFF);
                    cmd.Parameters.AddWithValue("@addressStaff", staff.ADDRESSSTAFF);
                    cmd.Parameters.AddWithValue("@mailStaff", staff.MAILSTAFF);
                    cmd.Parameters.AddWithValue("@usernameStaff", staff.USERNAMESTAFF);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                }
            }catch(Exception e)
            {
                throw e;
            }
        }

        public void UpdatePassword(Staff staff)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "UPDATE STAFF " +
                        "SET PASSWORDSTAFF = @passwordStaff WHERE IDSTAFF=@idStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", staff.IDSTAFF);
                    cmd.Parameters.AddWithValue("@passwordStaff", staff.PASSWORDSTAFF);
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateCurrentTotal(int idStaff)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "UPDATE STAFF " +
                        "SET ORDERCURRENTTOTAL += 1" + 
                        "WHERE IDSTAFF=@idStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    
                    cmd.Parameters.AddWithValue("@idStaff",idStaff);
                   
                    cn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
