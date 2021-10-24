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
    public class CityManager
    {
        private ICityDB CityDb { get;}

        public CityManager(IConfiguration conf)
        {
            CityDb = new CityDB(conf);
        }

        public List<City> GetCity()
        {
            return CityDb.GetCity();
        }

    }
}
