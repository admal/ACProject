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
    [Serializable]
    public class BoardBlock : Block , IBoardBlock
    {
        public Point Position { get; set; }

        public BoardBlock(IBoardBlock block) : base(block)
        {
            this.Position = block.Position;
        }

        public BoardBlock(IBlock block, Point position) : base(block.Grid)
        {
            this.Color = block.Color;
            this.Position = position;
        }

        public BoardBlock(int width, int height) : base(width, height){}

        public BoardBlock(int[,] grid, Point position) : base(grid)
        {
            Position = position;
        }

        public BoardBlock(Block block, Point position) : base(block.Grid)
        {
            this.Position = position;
        }

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
            BoardBlock block = new BoardBlock(newMatrix, Position);
            return block;
        }

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
                                containerHeight - cellSize * (Position.Y + height - j) - 1, 
                                cellSize - 1,
                                cellSize - 1);

                            graphics.FillRectangle(brush, rect);
                        }
                    }
                }
            }
        }

        public override void Draw(Graphics graphics, int cellSize)
        {
            using (var brush = new SolidBrush(Color))
            {
                this.Draw(graphics, brush, cellSize);
            }
        }
    }
}
