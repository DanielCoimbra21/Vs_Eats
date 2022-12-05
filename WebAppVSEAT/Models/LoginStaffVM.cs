using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAppVSEAT.Models
{
    public class LoginStaffVM
    {
        [Required]
        public string MAILSTAFF { get; set; }
        [Required]
        public string PASSWORDSTAFF { get; set; }
    }
}
