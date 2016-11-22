using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Domain.Models
{
    public interface IBoardBlock : IBlock
    {
        Point Position { get; set; }
        BoardBlock RotateClockwise();
        BoardBlock RotateCounterClockwise();
        void Draw(Graphics graphics, Brush brush, int cellSize);
    }
}
