using ACProject.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    class BoardState
    {
        public int Width { get; set; }
        private int[] Heights { get; set; }

        private const double A = 0.6d;
        private const double B = 0.3d;
        private const double C = 0.9d;
        public int K { get; set; }
        public BoardState(int width, int k)
        {
            K = k;
            Width = width;
            Heights = new int[Width];
        }

        public async void GetMoves(ConcurrentQueue<Move> q, BoardBlock block)
        {
            IList<Move> result = await Task.Run(() => CalculateTask(block));
            foreach (var r in result)
                q.Enqueue(r); 
        }

        public List<Move> CalculateTask(BoardBlock block)
        {
            List<Move> ret = new List<Move>();
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < Width - block.Grid.GetLength(0); i++)
                {
                    var cost = CheckMove(i, block);
                    if(!double.IsNaN(cost))
                    {
                        ret.Add(new Move(block, i, cost, K));
                    }
                }
                block.RotateClockwise();
            }
            return ret;
        }

        public double CheckMove(int i, BoardBlock block)
        {
            int w = block.Grid.GetLength(0);
            int[] heights = new int[w];
            for (int idx = 0; idx < w; idx++)
            {
                int h = 0;
                int hpos = block.Grid.GetLength(1) - 1;
                while (hpos >= 0 && block.Grid[idx, hpos] == 0)
                {
                    h++;
                    hpos--;
                }
                heights[idx] = h;
            }
            int min = heights.Min();
            int[] positions = new int[w];
            for (int idx = 0; idx < w; idx++)
            {
                heights[idx] = heights[idx] - min;
                positions[idx] = this.Heights[i + idx] + heights[idx];
            }
            int blockPos = Array.IndexOf(positions, positions.Max());
            int blockHeight = this.Heights[i + blockPos] + block.Grid.GetLength(1) - heights[blockPos];
            bool ok = true;
            for(int idx = 0; idx < w; idx++)
            {
                if (blockHeight - block.Grid.GetLength(1) + heights[idx] - this.Heights[i + idx] < 0)
                    ok = false;
            }
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
                    newHeights[idx] = Heights[i + idx] + blockHeight - topEmpty[idx];
                }
                int[] tempHeights = new int[Heights.Length];
                Heights.CopyTo(tempHeights, 0);
                newHeights.CopyTo(tempHeights, i);
                int aggHeight = tempHeights.Sum();
                int bumps = 0;
                for (int idx = 0; idx < tempHeights.Length - 1; idx++)
                    bumps = bumps + (Math.Abs(tempHeights[idx] - tempHeights[idx + 1]));
                int holes = 0;
                for (int idx = 0; idx < w; idx++)
                {
                    holes = holes + (blockHeight - block.Grid.GetLength(1) + heights[idx] - Heights[i + idx]);
                }
                return A * aggHeight + B * bumps + C * holes;

            }
            else
                return double.NaN;
        }
    }
}
