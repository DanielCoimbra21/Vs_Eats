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
    public class DistrictStaffDB : IDistrictStaffDB
    {
        private IConfiguration Configuration { get; }
        public DistrictStaffDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<DistrictStaff> GetDistrictStaffs()
        {
            List<DistrictStaff> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from [dbo].DISTRICTSTAFF";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<DistrictStaff>();

                            DistrictStaff districtStaff = new DistrictStaff();

                            districtStaff.IDSTAFF = (int)dr["IDSTAFF"];
                            districtStaff.IDDISTRICT = (int)dr["IDDISTRICT"];

                            results.Add(districtStaff);
                        }                 
                    }
                }
            }catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
            return results;
        }
    }
}
