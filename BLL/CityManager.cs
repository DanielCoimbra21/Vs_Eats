using BLL;
using DAL;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CityManager : ICityManager
    {
        private ICityDB CityDb { get;}

        public CityManager(IConfiguration conf)
        {
            CityDb = new CityDB(conf);
        }

        public List<City> GetCities()
        {
            return CityDb.GetCities();
        }

        public City GetCity(int idCity)
        {
            return GetCity(idCity);
        }

        

    }
}
