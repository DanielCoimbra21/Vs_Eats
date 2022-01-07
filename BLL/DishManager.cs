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
    public class DishManager : IDishManager
    {
        private IDishDB DishDb { get; set; }

        public DishManager(IDishDB dishDB)
        {
            DishDb = dishDB;
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
