using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class LoginVM
    {
        [Required]
        public string mail { get; set; }

        [Required]
        public string password { get; set; }

    }
}
