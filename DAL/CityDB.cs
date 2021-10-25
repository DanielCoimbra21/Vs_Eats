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
    public class CityDB : ICityDB
    {
        private IConfiguration Configuration { get;}

        public CityDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<City> GetCity()
        {
            List<City> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from City";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<City>();

                            City city = new City();

                            city.IDCITY = (int)dr["IDCITY"];

                            if (dr["CITYNAME"] != null)
                                city.CITYNAME = (string)dr["CITYNAME"];

                            if(dr["NPA"] != null)
                                    city.NPA = (int)dr["NPA"];

                            results.Add(city);
                        }
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
