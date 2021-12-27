using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class OrderVM
    {
        public int IDORDER { get; set; }
        public decimal TOTALPRICE { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public int NPA { get; set; }
        public int PHONE { get; set; }
        public DateTime DELIVERTIME { get; set; }
        public string STATUS { get; set; }
    }
}
