using System.Collections.Generic;
using System.Drawing;

namespace ACProject.Domain.Models
{
    public interface IBlock
    {
        int[,] Grid { get; set; }
        Color Color { get; set; }
        int Count { get; set; }
        void Draw(Graphics graphics, int cellSize);
        void Draw(Graphics graphics, int cellSize, int containerHeight);
    }
}