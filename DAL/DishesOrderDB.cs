﻿using DTO;
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

        public List<DishesOrder> GetDishesOrders(int idOrder)
        {
            List<DishesOrder> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from DISHESORDER where @idOrder = IDORDER";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@idOrder", idOrder);

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

        public void InsertDishesOrder(DishesOrder dishesOrder)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Insert into DISHESORDER(IDDISHES,IDORDER, QUANTITY)" +
                        " values(@idDishes,@idOrder, @quantity)";

                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@idDishes", dishesOrder.IDDISHES);
                    cmd.Parameters.AddWithValue("@idOrder", dishesOrder.IDORDER);
                    cmd.Parameters.AddWithValue("@quantity", dishesOrder.QUANTITY);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateDishesOrder(DishesOrder dishesOrder)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    var query = "UPDATE [dbo].[DISHESORDER] " +
                        "SET QUANTITY=@quantity " +
                        "WHERE IDDISHES=@iddishes AND IDORDER=@idorder";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@quantity", dishesOrder.QUANTITY);
                    cmd.Parameters.AddWithValue("@iddishes", dishesOrder.IDDISHES);
                    cmd.Parameters.AddWithValue("@idorder", dishesOrder.IDORDER);

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
