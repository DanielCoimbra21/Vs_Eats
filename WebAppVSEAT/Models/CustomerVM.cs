using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CustomerVM
    {

        public int IDCUSTOMER { get; set; }
        public int IDCITY { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string USERNAME { get; set; }
        public int PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string MAIL { get; set; }
        public string PASSWORD { get; set; }

    }
}
