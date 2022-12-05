using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CustomerVM
    {

        public int IDCUSTOMER { get; set; }
        public int IDCITY { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string SURNAME { get; set; }
        [Required]
        public string USERNAME { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PHONE { get; set; }
        [Required]
        public string ADDRESS { get; set; }
        [Required]
        public string CITYNAME { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string MAIL { get; set; }

    }
}
