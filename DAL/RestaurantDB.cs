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
    public class RestaurantDB : IRestaurantDB
    {
        private IConfiguration Configuration { get; }

        public RestaurantDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Restaurant> GetRestaurants()
        {
            List<Restaurant> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from RESTAURANT";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Restaurant>();

                            Restaurant restaurant = new Restaurant();

                            restaurant.IDRESTAURANT = (int)dr["IDRESTAURANT"];

                            restaurant.IDCITY = (int)dr["IDCITY"];

                            restaurant.NAMERESTAURANT = (string)dr["NAMERESTAURANT"];

                            restaurant.ADDRESSRESTAURANT = (string)dr["ADDRESSRESTAURANT"];

                            results.Add(restaurant);

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

        public Restaurant GetRestaurant(int idResaurant)
        {
            Restaurant restaurant = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from RESTAURANT " +
                        "where IDRESTAURANT = @idRestaurant";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idRestaurant", idResaurant);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            
                            restaurant = new Restaurant();

                            restaurant.IDRESTAURANT = (int)dr["IDRESTAURANT"];

                            restaurant.IDCITY = (int)dr["IDCITY"];

                            restaurant.NAMERESTAURANT = (string)dr["NAMERESTAURANT"];

                            restaurant.ADDRESSRESTAURANT = (string)dr["ADDRESSRESTAURANT"];

                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return restaurant;
        }
    }
}
