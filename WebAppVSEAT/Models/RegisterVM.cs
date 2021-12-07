using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(password), ErrorMessage = "This Password doesn't correspond to the your password")]
        public string confirmerPassword { get; set; }
    }
}
