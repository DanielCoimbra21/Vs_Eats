using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppVSEAT.Models
{
    public class CancelOrderVM
    {
        [Required]
        public int IDORDER { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string SURNAME { get; set; }
    }
}
