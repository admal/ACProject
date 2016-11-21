using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Domain.Models
{
    [Serializable]
    public class Block : IBlock
    {
        private static readonly Random rnd = new Random();

        public Block(int width, int height)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            this.Grid =  new int[width, height];
        }

        public Block(int[,] grid)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            this.Grid = grid;
        }

        public int Count { get; set; }
        public int[,] Grid { get; set; }
        public Color Color { get; set; }

        public virtual void Draw(Graphics graphics, int cellSize)
        {
            using (var brush = new SolidBrush(Color))
            {
                //for (int i = 0; i < Grid.GetLength(1); i++)
                //{
                //    for (int j = 0; j < Grid.GetLength(0); j++)
                //    {
                //        if (this.Grid[j, i] == 1)
                //        {
                //            var rect = new Rectangle((cellSize * i) + 1, (cellSize * j) + 1, cellSize - 1, cellSize - 1);
                //            graphics.FillRectangle(brush, rect);
                //        }
                //    }
                //}
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {
                        if (this.Grid[i, j] == 1)
                        {
                            var rect = new Rectangle((cellSize * i) + 1, (cellSize * j) + 1, cellSize - 1, cellSize - 1);
                            graphics.FillRectangle(brush, rect);
                        }
                    }
                }
            }
        }
    }
}
