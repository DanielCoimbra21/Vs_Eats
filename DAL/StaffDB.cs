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
                        if (results == null)
                            results = new List<Staff>();

                        Staff staff = new Staff();

                        staff.IDSTAFF = (int)dr["IDSTAFF"];
                        staff.IDCITY = (int)dr["IDCITY"];
                        staff.NAMESTAFF = (string)dr["NAMESTAFF"];
                        staff.SURNAMESTAFF = (string)dr["SURNAMESTAFF"];
                        staff.PHONENUMBER = (int)dr["PHONENUMBER"];
                        staff.ADDRESSSTAFF = (string)dr["ADDRESSSTAFF"];
                        staff.MAILSTAFF = (string)dr["MAILSTAFF"];
                        staff.USERNAMESTAFF = (string)dr["USERNAMESTAFF"];
                        staff.PASSWORDSSTAFF = (string)dr["PASSWORDSSTAFF"];
                        staff.ORDERCURRENTTOTAL = (int)dr["ORDERCURRENTTOTAL"];

                        results.Add(staff);

                    }
                }
            }catch (Exception e)
            {
                throw e;
            }
            return results;
        }
    }
}
