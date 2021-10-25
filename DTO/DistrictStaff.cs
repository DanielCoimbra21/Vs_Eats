using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DistrictStaff
    {
        public int IDSTAFF { get; }
        public int IDDISTRICT { get; }

        public override string ToString()
        {
            return "IDSTAFF : " + IDSTAFF + "\n" +
            "IDDISTRICT : " + IDDISTRICT + "\n";
        }
    }
}
