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
        private static readonly Random rnd = new Random();
        private int _count;
        private Color _color;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

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
                        var rect = new Rectangle( position.X +(cellSize * i) + 1, position.Y + (cellSize * j) + 1, cellSize - 1, cellSize - 1);
                        graphics.FillRectangle(brush, rect);

                    }
                }
            }
        }

        public void Draw(Graphics graphics, Point position, int cellSize)
        {
            using (var brush = new SolidBrush(_color))
            {
                this.Draw(graphics, brush, position, cellSize);
            }
        }

        public DummyBlock(int width, int height)
        {
            _color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));

            this.Count = 1;
            this.Grid = new int[width, height];
        }
    }
}
