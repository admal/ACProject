using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    public class CheckMoveResult
    {
        public double Cost { get; set; }
        public int[] Heights { get; set; }
        public Point Location { get; set; }
    }
}
