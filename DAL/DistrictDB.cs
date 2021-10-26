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

        /*public List<District> GetDistrict()
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

                                District district = new District();

                                district.IDDISTRICT = (int)dr["IDDISTRICT"];

                                if(dr["NameDistrict"] != null)

                            }
                        }
                    }
                }
            }
        }*/
    }
}
