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
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string USERNAME { get; set; }
        public int PHONE { get; set; }
        public string ADDRESS { get; set; }
        [Required]
        public string CITYNAME { get; set; }
        [Required]
        public string MAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }

    }
}
