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
    /// <summary>
    /// Default Block which is a non planar tile representation
    /// </summary>
    [Serializable]
    public class Block : IBlock
    {
        /// <summary>
        /// random number generator for color creation
        /// </summary>
        private static readonly Random rnd = new Random();
        /// <summary>
        /// Tile grid 
        /// two dimensional array of 1's and 0's
        /// 1 - there is a cell in a tile
        /// 0 - there is no cell in a tile
        /// </summary>
        private int[,] _grid;
        /// <summary>
        /// Block copy constructor
        /// </summary>
        /// <param name="block"> Block to be copied </param>
        public Block(IBlock block)
        {
            this.Color = Color.FromArgb(block.Color.ToArgb());
            this.Grid = (int[,]) block.Grid.Clone();
        }
        /// <summary>
        /// Creates Block with empty grid
        /// </summary>
        /// <param name="width"> width of a tile </param>
        /// <param name="height"> height of a tile </param>
        public Block(int width, int height)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            _grid =  new int[width, height];
        }
        /// <summary>
        /// Creates a Block from a tile grid
        /// </summary>
        /// <param name="grid"> Grid of a tile </param>
        public Block(int[,] grid)
        {
            Color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            _grid = grid;
            RemoveUselessRowsAndCols();
        }
        /// <summary>
        /// Removes useless rows and columns from a tile grid and resizes the block
        /// When a block dimensions exceed the grid of a tile they are reduced to match it perfectly
        /// </summary>
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
        /// <summary>
        /// Count of a type of a tile
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Tile grid of a block
        /// </summary>
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
        /// <summary>
        /// Color of a Block
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Draws a tile in a given context
        /// </summary>
        /// <param name="graphics">Context for drawing</param>
        /// <param name="cellSize">Size of each square cell of a tile in pixels</param>
        public virtual void Draw(Graphics graphics, int cellSize)
        {
            using (var brush = new SolidBrush(Color))
            {
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
        /// <summary>
        /// Draws a tile in a given context in container
        /// </summary>
        /// <param name="graphics">Context for drawing</param>
        /// <param name="cellSize">Size of each square cell of a tile in pixels</param>
        /// <param name="containerHeight">Height of a container we draw in pixels</param>
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
