using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CreateCustomerVM
    {

        public int IDCUSTOMER { get; set; }
        public int IDCITY { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string USERNAME { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        [Required]
        public string CITYNAME { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string MAIL { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(PASSWORD), ErrorMessage = "This Password doesn't correspond to the previous r password")]
        public string confirmerPassword { get; set; }

    }
}
