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
    public class DistrictDB : IDistrictDB
    {
        private IConfiguration Configuration { get; }

        public DistrictDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<District> GetDistricts()
        {
            List<District> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from District";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cn.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                            {
                                results = new List<District>();
                            }

                              District district = new District();

                              district.IDDISTRICT = (int)dr["IDDISTRICT"];

                            if (dr["NameDistrict"] != null)
                            {
                                district.NAMEDISTRICT = (string)dr["NAMEDISTRICT"];
                            }

                                results.Add(district);

                            }
                        }
                    }
                }
            catch(Exception e)
            {
                throw e;
            }
            return results;
        }

        public District GetDistrict(int idDistrict)
        {
            District district = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from [dbo].DISTRICT where IDDISTRICT = @idDistrict";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idDistrict", idDistrict);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            district = new District();

                            district.IDDISTRICT = (int)dr["IDCITY"];

                            district.NAMEDISTRICT = (string)dr["NAMEDISTRICT"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return district;
        }
    }
}
