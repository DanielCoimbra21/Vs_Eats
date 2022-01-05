using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class StaffVM
    {
        public int IDSTAFF { get; set; }
        public int IDCITY { get; set; }
        [Required]
        public string CITYNAME { get; set; }
        [Required]
        public string NAMESTAFF { get; set; }
        [Required]
        public string SURNAMESTAFF { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public int PHONENUMBERSTAFF { get; set; }
        [Required]
        public string ADDRESSSTAFF { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string MAILSTAFF { get; set; }
        public string USERNAMESTAFF { get; set; }
        
        [DataType(DataType.Password)]
        public string PASSWORDSTAFF { get; set; }
    }
}
