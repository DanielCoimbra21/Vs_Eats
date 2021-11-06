using DAL;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DishManager
    {
        private IDishDB DishDb { get; set; }

        public DishManager(IConfiguration conf)
        {
            DishDb = new DishDB(conf);
        }

        public Dish GetDish(string dishName)
        {
            return DishDb.GetDish(dishName);
        }

        public Dish GetDish(int idDish)
        {
            return DishDb.GetDish(idDish);
        }

        public List<Dish> GetDishes()
        {
            return DishDb.GetDishes();
        }
    }
}
