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

        public List<DishesRestaurant> GetDishesRestaurants()
        {
            List<DishesRestaurant> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from DISHESRESTAURANT";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (results == null)
                            results = new List<DishesRestaurant>();

                        DishesRestaurant dishesRestaurant = new DishesRestaurant();

                        dishesRestaurant.IDDISHES = (int)dr["IDDISHES"];
                        dishesRestaurant.IDDISHES = (int)dr["IDRESTAURANT"];

                        results.Add(dishesRestaurant);
                    }
                }
            }catch (Exception e)
            {
                throw e;
            }
            return results;

        }

        public int GetIdRestaurant(int idDish)
        {
            List<DishesRestaurant> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            int idRestaurant;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select IDRESTAURANT from DISHESRESTAURANT" +
                        " where @idDish = IDDISHES";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idDish", idDish);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        idRestaurant = (int)dr["IDRESTAURANT"];
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return idRestaurant;

        }
    }
}
