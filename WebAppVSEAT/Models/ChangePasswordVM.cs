using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string PASSWORDSTAFF { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NEWPASSWORD { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NEWPASSWORD), ErrorMessage = "This Password doesn't correspond to the previous password")]
        public string CONFIRMPASSWORD { get; set; }


    }
}
