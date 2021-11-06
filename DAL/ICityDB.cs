using DTO;
using System.Collections.Generic;

namespace DAL
{
    public interface ICityDB
    {
        public List<City> GetCities();

        public int GetCity(int idCity);

    }
}