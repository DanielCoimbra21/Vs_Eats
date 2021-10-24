using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class City
    {
        public int IDCITY { get; set; }
        public string CITYNAME { get; set; }
        public int NPA { get; set; }

        public override string ToString()
        {
            return "IdCity : " + IDCITY + "\n" +
            "CityName : " + CITYNAME + "\n" +
            "NPA : " + NPA + "\n";
        }



    }
}
