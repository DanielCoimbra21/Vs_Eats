using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class OrderVM
    {
        public int IDORDER { get; set; }
        public int IDDISTRICT { get; set; }
        public int IDRESTAURANT { get; set; }
        public int IDSTAFF { get; set; }
        public int IDCUSTOMER { get; set; }
        public decimal TOTALPRICE { get; set; }
        public DateTime DELIVERTIME { get; set; }
        public string STATUS { get; set; }
    }
}
