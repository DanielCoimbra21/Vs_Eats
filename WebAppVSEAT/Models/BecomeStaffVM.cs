using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class BecomeStaffVM
    {
     
        [Required]
        public string mailFrom { get; set;}
        [Required]
        public string subjectMail { get; set; }
        [Required]
        public string bodyMail { get; set; }
    }
}
