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

        public int GetIdRestaurant(int idDish)
        {
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

                            // DishesRestaurant dt = new DishesRestaurant();
                            //dt.IDDISHES = (int)dr["IDDISHES"];
                            // results.Add(dt);
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
