using ACProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    public class Move
    {
        public BoardBlock Block { get; set; } // block is alerady rotated to correct position
        public int Board { get; set; } // 0 <= number of board on which to place the block < k 

        public int Column { get; set; }
        public double Cost { get; set; }
        public int[] NewHeights { get; set; }
        public int ListIndex { get; set; }
        public BoardState BoardState { get; set; }
        public  int PlacedArea {get; set;}
        public double Density { get; set; }
        public Move(BoardBlock block, int column, double cost, int board, int[] heights, int lidx, BoardState bs, int placedArea, double density)
        {

            Block = new BoardBlock(block.Grid.Clone() as int[,], new Point(block.Position.X, block.Position.Y));
            Board = board;
            Column = column;
            Cost = cost;
            NewHeights = heights.Clone() as int[];
            ListIndex = lidx;
            BoardState = bs;
            Density = density;
            PlacedArea = placedArea;
        }
    }
}
