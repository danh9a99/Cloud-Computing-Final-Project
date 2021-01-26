using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Efs.Entities
{
    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public ICollection<Container> Containers { get; set; }
    }
}
