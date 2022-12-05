using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DistrictStaff
    {
        public int IDSTAFF { get; set; }
        public int IDDISTRICT { get; set; }

        public override string ToString()
        {
            return "IDSTAFF : " + IDSTAFF + "\n" +
            "IDDISTRICT : " + IDDISTRICT + "\n";
        }
    }
}
