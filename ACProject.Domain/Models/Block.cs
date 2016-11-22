using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Domain.Models
{
    [Serializable]
    public class Block : IBlock
    {
        private static readonly Random rnd = new Random();
        private int[,] _grid;

        public Block(int width, int height)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            _grid =  new int[width, height];
        }

        public Block(int[,] grid)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            _grid = grid;
            RemoveUselessRowsAndCols();
        }
        private void RemoveUselessRowsAndCols()
        {
            int[] necessaryRows = new int[_grid.GetLength(1)]; // necessary grid rows
            int[] necessaryCols = new int[_grid.GetLength(0)]; // necessary grid cols
            for (int i = 0; i < necessaryCols.Length; i++) necessaryCols[i] = 0;
            for (int i = 0; i < necessaryRows.Length; i++) necessaryRows[i] = 0;

            for (int i = 0; i < Grid.GetLength(0); i++) // po szerokosci
            {
                for (int j = 0; j < Grid.GetLength(1); j++) // po wysokosci
                {
                    if (this.Grid[i, j] == 1)
                    {
                        necessaryCols[i] = 1;
                        necessaryRows[j] = 1;
                    }
                }
            }

            int countCols = 0;
            int countRows = 0;
            for (int i = 0; i < necessaryCols.Length; i++) // count sizes of a new grid
            {
                if(necessaryCols[i] == 1)
                    countCols++;
            }
            for (int i = 0; i < necessaryRows.Length; i++)
            {
                if (necessaryRows[i] == 1)
                    countRows++;
            }
            int[,] newGrid = new int[countCols, countRows];

            int x = 0, y = 0;
            for (int i = 0; i < necessaryRows.Length; i++)
            {
                if (necessaryRows[i] == 1)
                {
                    x = 0;
                    for (int j = 0; j < necessaryCols.Length; j++)
                    {
                        if (necessaryCols[j] == 1 )
                        {
                            newGrid[x, y] = _grid[j, i];
                            x++;
                        }
                    }
                    y++;
                }
            }
            _grid = newGrid;
        }

        public int Count { get; set; }
        public int[,] Grid {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;
                RemoveUselessRowsAndCols();
            } 
        }
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
        public virtual void Draw(Graphics graphics, int cellSize, int containerHeight)
        {
            using (var brush = new SolidBrush(Color))
            {
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {
                        if (this.Grid[i, j] == 1)
                        {
                            var rect = new Rectangle((cellSize * i) + 1, containerHeight - (cellSize * j) -1, cellSize - 1, cellSize - 1);
                            graphics.FillRectangle(brush, rect);
                        }
                    }
                }
            }
        }
    }
}
