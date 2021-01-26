using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Efs.Entities
{
    public partial class Container
    {
        public int ContainerID { get; set; }
        public string ContainerName { get; set; }
        public double LimitCPU { get; set; }
        public double LimitRAM { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserID { get; set; }
    }
}
