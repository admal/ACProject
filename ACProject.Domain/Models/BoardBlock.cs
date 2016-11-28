using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Domain.Models
{
    /// <summary>
    /// Planar representation of a block 
    /// Block with position in the well and 2d transformations
    /// </summary>
    [Serializable]
    public class BoardBlock : Block , IBoardBlock
    {
        /// <summary>
        /// Position of a block in a well
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// BoardBlock copy constructor
        /// </summary>
        /// <param name="block"> BoardBlock to be copied </param>
        public BoardBlock(IBoardBlock block) : base(block)
        {
            this.Position = new Point(block.Position.X, block.Position.Y);
        }
        /// <summary>
        /// Creating a Board Block from a Block
        /// </summary>
        /// <param name="block"> Block we want to put on a board </param>
        /// <param name="position"> Postion on the board we want to put the block in </param>
        public BoardBlock(IBlock block, Point position) : base(block.Grid)
        {
            this.Color = block.Color;
            this.Position = position;
        }
        /// <summary>
        /// Create an empty Board Block with holder for tile grid
        /// </summary>
        /// <param name="width"> Width of tile grid </param>
        /// <param name="height"> Height of tile grid </param>
        public BoardBlock(int width, int height) : base(width, height){}
        /// <summary>
        /// Creates a BoardBlock from tile grid on a given position on the board
        /// </summary>
        /// <param name="grid"> Tile grid </param>
        /// <param name="position"> Postion on a board </param>
        public BoardBlock(int[,] grid, Point position) : base(grid)
        {
            Position = position;
        }
        /// <summary>
        /// Creates a Block
        /// </summary>
        /// <param name="block"></param>
        /// <param name="position"></param>
        public BoardBlock(Block block, Point position) : base(block.Grid)
        {
            this.Position = position;
        }
        /// <summary>
        /// count the area of a Block
        /// </summary>
        /// <returns> area of a block </returns>
        public int GetArea()
        {
            return Grid.Cast<int>().Sum();
        }
        /// <summary>
        /// Rotates the tile counter clockwise
        /// </summary>
        /// <returns>Rotated tile</returns>
        public BoardBlock RotateClockwise()
        {
            
            int[,] newMatrix = new int[Grid.GetLength(1), Grid.GetLength(0)];
            int newColumn, newRow = 0;
            for (int oldColumn = 0; oldColumn <= Grid.GetLength(1) - 1; oldColumn++)
            {
                newColumn = 0;
                for (int oldRow = Grid.GetLength(0) - 1; oldRow >= 0 ; oldRow--)
                {
                    newMatrix[newRow, newColumn] = Grid[oldRow, oldColumn];
                    newColumn++;
                }
                newRow++;
            }
            BoardBlock block = new BoardBlock(newMatrix, new Point(Position.X, Position.Y));
            return block;
        }
        /// <summary>
        /// Rotates the tile counter clockwise
        /// </summary>
        /// <returns>Rotated tile</returns>
        public BoardBlock RotateCounterClockwise()
        {
            int[,] newMatrix = new int[Grid.GetLength(1), this.Grid.GetLength(0)];
            int newColumn, newRow = 0;
            for (int oldColumn = Grid.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < Grid.GetLength(0); oldRow++)
                {
                    newMatrix[newRow, newColumn] = Grid[oldRow, oldColumn];
                    newColumn++;
                }
                newRow++;
            }
            BoardBlock block = new BoardBlock(newMatrix, Position);
            return block;
        }
        /// <summary>
        /// Draws a block on it's position in a well
        /// </summary>
        /// <param name="graphics"> context for drawing </param>
        /// <param name="brush"> brush for drawing </param>
        /// <param name="cellSize"> Size of each square cell of a tile in pixels </param>
        public void Draw(Graphics graphics, Brush brush, int cellSize)
        {

            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (this.Grid[i, j] == 1)
                    {
                        var rect = new Rectangle((Position.X + i) * cellSize + 1, (Position.Y + j) * cellSize + 1, cellSize - 1, cellSize - 1);
                        graphics.FillRectangle(brush, rect);
                    }
                }
            }
        }
        /// <summary>
        /// Draws a block on it's position in a well
        /// </summary>
        /// <param name="graphics"> context for drawing </param>
        /// <param name="cellSize"> Size of each square cell of a tile in pixels </param>
        /// <param name="containerHeight"> Height of a container we draw in pixels </param>
        public override void Draw(Graphics graphics, int cellSize, int containerHeight)
        {
            using (var brush = new SolidBrush(Color))
            {
                var width = Grid.GetLength(0);
                var height = Grid.GetLength(1);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (this.Grid[i, j] == 1)
                        {
                            var rect = new Rectangle((Position.X + width - i - 1) * cellSize + 1,
                                containerHeight - cellSize * (Position.Y + height - j) + 1, 
                                cellSize - 1,
                                cellSize - 1);

                            graphics.FillRectangle(brush, rect);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws a Block on it's position
        /// </summary>
        /// <param name="graphics"> context for drawing </param>
        /// <param name="cellSize"> Size of each square cell of a tile in pixels </param>
        public override void Draw(Graphics graphics, int cellSize)
        {
            using (var brush = new SolidBrush(Color))
            {
                this.Draw(graphics, brush, cellSize);
            }
        }
    }
}
