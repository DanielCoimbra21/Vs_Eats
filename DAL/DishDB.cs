﻿using DTO;
using Microsoft.Ajax.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DishDB : IDishDB
    {
        private IConfiguration Configuration { get; }

        public DishDB(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Dish> GetDishes()
        {
            List<Dish> results = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");


            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from DISHES";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            if (results == null)
                                results = new List<Dish>();

                            Dish dish = new Dish();

                            dish.IDDISHES = (int)dr["IDDISHES"];

                            if (dr["NAMEDISH"] != null)
                                dish.NAMEDISH = (string)dr["NAMEDISH"];

                            if (dr["PRICEDISH"] != null)
                                dish.PRICEDISH = (decimal)dr["PRICEDISH"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return results;
        }

        public Dish GetDish(string dishName, int dishPrice)
        {
            Dish dish = null;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            try
            {
                using(SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from DISHES where dishName = @dishName and dishPrice = @dishPrice";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@dishName", dishName);
                    cmd.Parameters.AddWithValue("@dishPrice", dishPrice);

                    cn.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            dish = new Dish();

                            dish.IDDISHES = (int)dr["IDDISHES"];

                            if (dr["NAMEDISH"] != null)
                                dish.NAMEDISH = (string)dr["NAMEDISH"];

                            if (dr["PRICEDISH"] != null)
                                dish.PRICEDISH = (int)dr["PRICEDISH"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return dish;
        }
    }
}
