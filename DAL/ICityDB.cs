using DTO;
using System.Collections.Generic;

namespace DAL
{
    public interface ICityDB
    {
        List<City> GetCity();
    }
}