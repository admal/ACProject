using System.Collections.Generic;
using System.Drawing;

namespace ACProject.Domain.Models
{
    public interface IBlock
    {
        int Count { get; set; }
        int[,] Grid { get; set; }
        void Draw(Graphics graphics, Brush brush, Point position, int cellSize);
        void Draw(Graphics graphics, Point position, int cellSize);
    }
}