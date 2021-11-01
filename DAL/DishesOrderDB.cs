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
   public class DishesOrderDB : IDishesOrderDB
    {

        private IConfiguration Configuration { get; }

        public DishesOrderDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<DishesOrder> GetDishesOrders()
        {
            List<DishesOrder> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from DISHESORDER";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<DishesOrder>();

                            DishesOrder dishesOrder = new DishesOrder();

                            dishesOrder.IDDISHES = (int)dr["IDDISHES"];

                            dishesOrder.IDORDER = (int)dr["IDORDER"];

                            dishesOrder.QUANTITY = (int)dr["QUANTITY"];

                            results.Add(dishesOrder);

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
