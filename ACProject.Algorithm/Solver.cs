using ACProject.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    public class Solver
    {
        private int Width { get; set; }
        private IList<MultipleBlock> Blocks { get; set; }
        private int K { get; set; }
        private IList<BoardState> BoardStates { get; set; }
        public Solver(IList<MultipleBlock> blocks, int width)
        {
            Width = width;
            Blocks = blocks;
            BoardStates = new List<BoardState>();
            BoardStates.Add(new BoardState(Width, K));
        }

        public IList<Move> GetNextMoves(BoardBlock block)
        {
            ConcurrentQueue<Move> q = new ConcurrentQueue<Move>();
            List<Task> tasks = new List<Task>();
            foreach(var state in BoardStates)
            {
                Task t = Task.Factory.StartNew(() =>state.GetMoves(q, block));
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            List<Move> ret = q.ToList();
            ret = ret.OrderBy(x => x.Cost).Skip(ret.Count - K).ToList();
            return ret;
        }
    }
}
