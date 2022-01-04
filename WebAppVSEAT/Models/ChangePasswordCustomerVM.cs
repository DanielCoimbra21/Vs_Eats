using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class ChangePasswordCustomerVM
    {

        [Required]
        public string PASSWORDCUSTOMER { get; set; }
        [Required]
        public string NEWPASSWORD { get; set; }
        [Required]
        public string CONFIRMPASSWORD { get; set; }



    }
}
