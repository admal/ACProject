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
    public class Solver
    {
        private int Width { get; set; }
        private IList<MultipleBlock> Blocks { get; set; }
        private int K { get; set; }
        private IList<BoardState> BoardStates { get; set; }
        public Solver(IList<MultipleBlock> blocks, int width, int k)
        {
            K = k;
            Width = width;
            Blocks = blocks;
            BoardStates = new List<BoardState>();
            BoardStates.Add(new BoardState(Width, 0, blocks));
        }

        public IList<Move> GetNextMoves(BoardBlock block)
        {
            List<Task<List<Move>>> tasks = new List<Task<List<Move>>>();
            foreach (var state in BoardStates)
            {
                for (int i=0; i< state.Blocks.Count; i++)
                {
                    int idx = i;
                    Task<List<Move>> t = Task<List<Move>>.Factory.StartNew(() => state.CalculateTask(state.Blocks[idx].Block , idx));
                    tasks.Add(t);
                }
            }
            Task.WaitAll(tasks.ToArray());
            List<Move> ret = new List<Move>();
            foreach(var t in tasks)
            {
                ret.AddRange(t.Result);
            }
            ret = ret.OrderBy(x => x.Cost).Take(K).ToList();
            UpdateBoardStates(ret);
            return ret;
        }

        private  void UpdateBoardStates(List<Move> moves)
        {
            BoardStates.Clear();
            int i = 0;
            foreach (var move in moves)
            {
                var list = new List<MultipleBlock>();
                foreach (var block in move.BoardState.Blocks)
                    list.Add(new MultipleBlock(block));
                var state = new BoardState(move.NewHeights, i, list);
                state.PlacedArea = move.PlacedArea;
                list[move.ListIndex].Count--;
                if (list[move.ListIndex].Count <= 0)
                    list.RemoveAt(move.ListIndex);
                BoardStates.Add(state);
                i++;
            }
        }
    }
}
