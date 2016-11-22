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
    class BoardState
    {
        public int Width { get; set; }
        private int[] Heights { get; set; }

        private const double A = 0.6d;
        private const double B = 0.3d;
        private const double C = 0.9d;
        private const double D = 3d;
        public int K { get; set; }
        public IList<MultipleBlock> Blocks { get; set; }
        public BoardState(int width, int k)
        {
            K = k;
            Width = width;
            Heights = new int[Width];
        }

        public BoardState(int[] widths, int k)
        {
            K = k;
            Width = widths.Length;
            Heights = widths;
        }

        public List<Move> CalculateTask(BoardBlock block)
        {
            List<Move> ret = new List<Move>();
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i <= Width - block.Grid.GetLength(0); i++)
                {
                    var result = CheckMove(i, block);
                    if(result != null)
                    {
                        block.Position = result.Location;
                        ret.Add(new Move(block, i, result.Cost, K, result.Heights));
                    }
                }
                block = block.RotateClockwise();
            }
            return ret;
        }

        public CheckMoveResult CheckMove(int i, BoardBlock block)
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
            int min = botEmpty.Min();
            int[] positions = new int[w];
            for (int idx = 0; idx < w; idx++)
            {
                //botEmpty[idx] = botEmpty[idx] - min;
                positions[idx] = this.Heights[i + idx] +  block.Grid.GetLength(1) - botEmpty[idx];
            }
            int blockPos = Array.IndexOf(positions, positions.Max());
            int blockHeight = this.Heights[i + blockPos] + block.Grid.GetLength(1) - botEmpty[blockPos];
            bool ok = true;
            for(int idx = 0; idx < w; idx++)
            {
                if (blockHeight - block.Grid.GetLength(1) + botEmpty[idx] - this.Heights[i + idx] < 1)
                    ok = false;
            }
            ok = true;
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
                return new CheckMoveResult()
                {
                    Cost = (A * aggHeight + B * bumps + C * holes + D * heightIncrease),
                    Heights = tempHeights,
                    Location = new System.Drawing.Point(i, blockHeight -block.Grid.GetLength(1))
                };
            }
            else
                return null;
        }
    }
}
