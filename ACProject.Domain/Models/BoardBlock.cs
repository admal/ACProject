﻿using System;
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

        public void RotateClockwise()
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
            Grid = newMatrix;
        }

        public void RotateCounterClockwise()
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
            Grid = newMatrix;
        }

        public void Draw(Graphics graphics, Brush brush, int cellSize)
        {

            for (int i = 0; i < Grid.GetLength(1); i++)
            {
                for (int j = 0; j < Grid.GetLength(0); j++)
                {
                    if (this.Grid[j, i] == 1)
                    {
                        var rect = new Rectangle(Position.X + (cellSize * i) + 1, Position.Y + (cellSize * j) + 1, cellSize - 1, cellSize - 1);
                        graphics.FillRectangle(brush, rect);
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