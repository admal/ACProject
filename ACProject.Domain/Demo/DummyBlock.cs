using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACProject.Domain.Models;

namespace ACProject.Domain.Demo
{
    public class DummyBlock : IBlock
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public int[,] Grid { get; set; }
        public void Draw(Graphics graphics, Brush brush, Point position, int cellSize)
        {
            for (int i = 0; i < Grid.GetLength(1); i++)
            {
                for (int j = 0; j < Grid.GetLength(0); j++)
                {
                    if (this.Grid[j, i] == 1)
                    {
                        var rect = new Rectangle( position.X +(cellSize * i), position.Y + (cellSize * j), cellSize, cellSize);
                        graphics.FillRectangle(brush, rect);

                    }
                }
            }
        }

        public DummyBlock(int width, int height)
        {
            this.Count = 1;
            this.Grid = new int[width, height];
        }
    }
}
