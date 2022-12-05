using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class District
    {
        public int IDDISTRICT { get; set; }
        public string NAMEDISTRICT { get; set; }

        public override string ToString()
        {
            return "IdDistrict : " + IDDISTRICT + "\n" +
            "NameDistrict : " + NAMEDISTRICT + "\n";
        }
    }
}
