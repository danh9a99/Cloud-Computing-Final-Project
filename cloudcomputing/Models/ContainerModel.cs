using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Models
{
    public class ContainerModel
    {
        public string ContainerName { get; set; }
        public double LimitCPU { get; set; }
        public double LimitRAM { get; set; }
    }
}
