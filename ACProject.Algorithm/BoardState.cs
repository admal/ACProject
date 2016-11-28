using ACProject.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    [Serializable]
    public class BoardState
    {
        public int Width { get; set; }
        private int[] Heights { get; set; }

        private const double A = 0.1d; //aggHeight
        private const double B = 0.1d; //bumpts
        private const double C = 0.1d; //holes
        private const double D = 2d; //height increase
        public int PlacedArea { get; set; } //area of already placed blocks
        public int K { get; set; }
        public IList<MultipleBlock> Blocks { get; set; }
        public BoardState(int width, int k)
        {
            PlacedArea = 0;
            K = k;
            Width = width;
            Heights = new int[Width];
        }

        public BoardState(int[] widths, int k)
        {
            PlacedArea = 0;
            K = k;
            Width = widths.Length;
            Heights = widths;
        }

        public BoardState(int[] heights, int k, IList<MultipleBlock> blocks) :this(heights,k)
        {
            PlacedArea = 0;
            Blocks = blocks;
        }

        public BoardState( int width, int k, IList<MultipleBlock> blocks) : this(width, k)
        {
            PlacedArea = 0;
            Blocks = blocks;
        }

        public List<Move> CalculateTask(BoardBlock block, int idx)
        //step of the algorithm for one board, calculate cost for every block and every rotation when placed in each column
        {
            List<Move> ret = new List<Move>();
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i <= Width - block.Grid.GetLength(0); i++)
                {
                    var result = CheckMove(i, block);
                    if(result != null)
                    {
                        var newBlock = new BoardBlock(block);
                        newBlock.Position = result.Location;
                        ret.Add(new Move(newBlock, i, result.Cost, K, result.Heights, idx, this, PlacedArea+block.GetArea(), (PlacedArea + block.GetArea())/(double)(result.Heights.Max()*Width)));
                    }
                }
                block = block.RotateClockwise();
            }
            return ret;
        }

        public CheckMoveResult CheckMove(int i, BoardBlock block)
         // i - column of the well in which the block should be placed
        {
            int w = block.Grid.GetLength(0);
            int[] botEmpty = new int[w];
            for (int idx = 0; idx < w; idx++)
            {
                int h = 0;
                int hpos = block.Grid.GetLength(1) - 1;
                while (hpos >= 0 && block.Grid[idx, hpos] == 0)
                {
                    h++;
                    hpos--;
                }
                botEmpty[idx] = h;
            }
            //calculate the number of empty cells in each column of the blopck
            int min = botEmpty.Min();
            int[] positions = new int[w];
            for (int idx = 0; idx < w; idx++)
            {
                //botEmpty[idx] = botEmpty[idx] - min;
                positions[idx] = this.Heights[i + idx] +  block.Grid.GetLength(1) - botEmpty[idx];
            }
            int blockPos = Array.IndexOf(positions, positions.Max());
            int blockHeight = this.Heights[i + blockPos] + block.Grid.GetLength(1) - botEmpty[blockPos];
            //find the position of the block if placed on top of the stack in given columns
            bool ok = true;
            for(int idx = 0; idx < w; idx++)
            {
                if (blockHeight - block.Grid.GetLength(1) + botEmpty[idx] - this.Heights[i + idx] < 1)
                    ok = false;
            }
            ok = true;
            //check if this placing is valid
            if (ok)
            {
                int[] topEmpty = new int[w];
                int[] newHeights = new int[w];
                for (int idx = 0; idx < w; idx++)
                {
                    int h = 0;
                    int hpos = 0;
                    while (hpos < block.Grid.GetLength(1) && block.Grid[idx, hpos] == 0)
                    {
                        h++;
                        hpos++;
                    }
                    topEmpty[idx] = h;
                    newHeights[idx] = blockHeight - topEmpty[idx];
                }
                //find the number of empty cells on top of the block in each column
                //and the heights of the well after placing the block 
                int[] tempHeights = new int[Heights.Length];
                Heights.CopyTo(tempHeights, 0);
                newHeights.CopyTo(tempHeights, i);
                int aggHeight = tempHeights.Sum();
                int heightIncrease = tempHeights.Max() - Heights.Max();
                int bumps = 0;
                for (int idx = 0; idx < tempHeights.Length - 1; idx++)
                    bumps = bumps + (Math.Abs(tempHeights[idx] - tempHeights[idx + 1]));
                int holes = 0;
                for (int idx = 0; idx < w; idx++)
                {
                    holes = holes + (blockHeight - block.Grid.GetLength(1) + botEmpty[idx] - Heights[i + idx]);
                }
                //find variables needed to compute the cost
                return new CheckMoveResult()
                {
                    Cost = (A * aggHeight + B * bumps + C * holes + D * heightIncrease),
                    Heights = tempHeights,
                    Location = new System.Drawing.Point(i, blockHeight -block.Grid.GetLength(1)),
                };
            }
            else
                return null;
        }
    }
}
