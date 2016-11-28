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
        public double Cost { get; set; } //cost of the move
        public int[] Heights { get; set; } //resulting heights of the toip of the well
        public Point Location { get; set; } // cell in which the top left corner of the block should be placed
    }
}
