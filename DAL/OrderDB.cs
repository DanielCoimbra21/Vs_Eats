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
    public class OrderDB : IOrderDB
    {
        private IConfiguration Configuration { get; }

        public OrderDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Order> GetOrders()
        {
            List<Order> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Order";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (results == null)
                            results = new List<Order>();

                        Order order = new Order();

                        order.IDORDER = (int)dr["IDORDER"];
                        order.IDDISTRICT= (int)dr["IDDISTRICT"];
                        order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                        order.IDSTAFF = (int)dr["IDSTAFF"];
                        order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                        order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                        order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                        order.STATUS = (string)dr["STATUS"];
                        
                        results.Add(order);

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
