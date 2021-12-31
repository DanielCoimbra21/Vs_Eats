using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class StaffVM
    {
        public int IDSTAFF { get; set; }
        public int IDCITY { get; set; }
        public string CITYNAME { get; set; }
        public string NAMESTAFF { get; set; }
        public string SURNAMESTAFF { get; set; }
        public int PHONENUMBERSTAFF { get; set; }
        public string ADDRESSSTAFF { get; set; }
        public string MAILSTAFF { get; set; }
        public string USERNAMESTAFF { get; set; }
        public string PASSWORDSTAFF { get; set; }
    }
}
