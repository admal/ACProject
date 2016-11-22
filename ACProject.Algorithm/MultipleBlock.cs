using ACProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    [Serializable]
    public class MultipleBlock
    {
        public IBoardBlock Block { get; set; }
        public int Count { get; set; }
    }
}
