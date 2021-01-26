using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Models
{
    public class Command
    {
        [Required]
        [MaxLength(140)]
        public string ip { get; set; }
        public string serverName { get; set; }
        public string passWord { get; set; }
    }
}
