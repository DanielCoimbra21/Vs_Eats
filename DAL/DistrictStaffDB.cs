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
                    string query = "Select * from DistrictStaff";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DistrictStaff districtStaff = new DistrictStaff();

                        districtStaff.IDDISTRICT = (int)dr["IDDISTRICT"];
                        districtStaff.IDSTAFF = (int)dr["IDSTAFF"];

                        results.Add(districtStaff);
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
