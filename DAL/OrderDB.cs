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

        public void ArchiveDelivery(Order order)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "UPDATE ORDER SET STATUS = @status" +
                        "WHERE IDORDER=@idOrder and IDSTAFF=@idStaff and IDCUSTOMER=@idCustomer and IDDISTRICT=@idDistrict";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@status", order.STATUS);
                    cmd.Parameters.AddWithValue("@idOrder", order.IDORDER);
                    cmd.Parameters.AddWithValue("@idStaff", order.IDSTAFF);
                    cmd.Parameters.AddWithValue("@idCustomer", order.IDCUSTOMER);
                    cmd.Parameters.AddWithValue("@idDistrict", order.IDDISTRICT);

                    cn.Open();

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
