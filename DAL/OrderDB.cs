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
                    string query = "Select * from [dbo].[ORDER]";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Order>();

                            Order order = new Order();

                            order.IDORDER = (int)dr["IDORDER"];
                            order.IDDISTRICT = (int)dr["IDDISTRICT"];
                            order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                            order.IDSTAFF = (int)dr["IDSTAFF"];
                            order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                            order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                            order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                            order.STATUS = (string)dr["STATUS"];
                            
                            results.Add(order);
                        }  
                    }
                }
            }catch (Exception e)
            {
                throw e;
            }
            return results;
        }

        public List<Order> GetOrders(int idStaff)
        {
            List<Order> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from [dbo].[ORDER] WHERE IDSTAFF = @idStaff AND STATUS='ongoing'";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", idStaff);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Order>();

                            Order order = new Order();

                            order.IDORDER = (int)dr["IDORDER"];
                            order.IDDISTRICT = (int)dr["IDDISTRICT"];
                            order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                            order.IDSTAFF = (int)dr["IDSTAFF"];
                            order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                            order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                            order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                            order.STATUS = (string)dr["STATUS"];

                            results.Add(order);
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

        public List<Order> GetOrdersAll(int idStaff)
        {
            List<Order> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from [dbo].[ORDER] WHERE IDSTAFF = @idStaff AND STATUS!='free'";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idStaff", idStaff);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Order>();

                            Order order = new Order();

                            order.IDORDER = (int)dr["IDORDER"];
                            order.IDDISTRICT = (int)dr["IDDISTRICT"];
                            order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                            order.IDSTAFF = (int)dr["IDSTAFF"];
                            order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                            order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                            order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                            order.STATUS = (string)dr["STATUS"];

                            results.Add(order);
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

        public Order GetOrder(int orderId)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            Order order = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from [dbo].[ORDER] where [IDORDER] = @orderId";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        if (dr.Read())
                        {
                            order = new Order();

                            order.IDORDER = (int)dr["IDORDER"];
                            order.IDDISTRICT = (int)dr["IDDISTRICT"];
                            order.IDRESTAURANT = (int)dr["IDRESTAURANT"];
                            if (dr["IDSTAFF"] != null)
                            {
                                order.IDSTAFF = (int)dr["IDSTAFF"];
                               
                            }     
                            order.IDCUSTOMER = (int)dr["IDCUSTOMER"];
                            order.TOTALPRICE = (decimal)dr["TOTALPRICE"];
                            order.DELIVERTIME = (DateTime)dr["DELIVERTIME"];
                            order.STATUS = (string)dr["STATUS"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return order;
        }

        public void ArchiveDelivery(Order order, string status)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "UPDATE [dbo].[ORDER] SET STATUS = @status WHERE IDORDER=@idOrder";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@idOrder", order.IDORDER);
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Order InsertOrder(Order order)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using(SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Insert into [dbo].[ORDER](IDDISTRICT, IDRESTAURANT, IDCUSTOMER, TOTALPRICE, DELIVERTIME, STATUS) " +
                        "values(@idDistrict, @idRestaurant, @idCustomer, @totalPrice, @deliverTime, @status) SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@idDistrict", order.IDDISTRICT);
                    cmd.Parameters.AddWithValue("@idRestaurant", order.IDRESTAURANT);
                    cmd.Parameters.AddWithValue("@idCustomer", order.IDCUSTOMER);
                    cmd.Parameters.AddWithValue("@totalPrice", order.TOTALPRICE);
                    cmd.Parameters.AddWithValue("@deliverTime", order.DELIVERTIME);
                    cmd.Parameters.AddWithValue("@status", order.STATUS);
                    

                    cn.Open();

                    order.IDORDER = Convert.ToInt32(cmd.ExecuteScalar());
                    
                }
            }
            catch(Exception e)
            {             
                throw e;
            }
            return order;
        }

        public void AddTime(Order order)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE [dbo].[ORDER] SET DELIVERYTIME = @deliveryTime WHERE IDORDER=@idOrder";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    //cmd.Parameters.AddWithValue("@idDistrict", order.IDDISTRICT);
                    //cmd.Parameters.AddWithValue("@idRestaurant", order.IDRESTAURANT);
                    //cmd.Parameters.AddWithValue("@idCustomer", order.IDCUSTOMER);
                    //cmd.Parameters.AddWithValue("@totalPrice", order.TOTALPRICE);
                    cmd.Parameters.AddWithValue("@deliverTime", order.DELIVERTIME);
                    //cmd.Parameters.AddWithValue("@status", order.STATUS);


                    cn.Open();

                    order.IDORDER = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
