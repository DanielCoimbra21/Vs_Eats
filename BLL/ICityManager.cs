using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ICityManager
    {
        List<City> GetCities();

        DTO.City GetCity(int idCity);

        DTO.City GetCity(string cityname);


    }
}
