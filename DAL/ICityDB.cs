using DTO;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ICityDB
    {
        List<City> GetCities();

        DTO.City GetCity(int idCity);

       

    }
}