using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data.SqlClient;

namespace DAL
{
    public class DishesRestaurantDB : IDishesRestaurantDB
    {
        private IConfiguration Configuration { get; }

        public DishesRestaurantDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public List<int> GetListDishes(int idRestaurant)
        {
            List<int> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
           
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select IDDISHES from DISHESRESTAURANT" +
                        " where @idRestaurant = IDRESTAURANT";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idRestaurant", idRestaurant);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<int>();

                            results.Add((int)dr["IDDISHES"]);
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
    }
}
