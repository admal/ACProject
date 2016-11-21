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
        public Solver(IList<MultipleBlock> blocks, int width, int k)
        {
            K = k;
            Width = width;
            Blocks = blocks;
            BoardStates = new List<BoardState>();
            BoardStates.Add(new BoardState(Width, 0));
        }

        public IList<Move> GetNextMoves(BoardBlock block)
        {
            ConcurrentQueue<Move> q = new ConcurrentQueue<Move>();
            List<Task<List<Move>>> tasks = new List<Task<List<Move>>>();
            foreach(var state in BoardStates)
            {
                Task<List<Move>> t = Task<List<Move>>.Factory.StartNew(() =>state.CalculateTask(block));
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            List<Move> ret = new List<Move>();
            foreach(var t in tasks)
            {
                ret.AddRange(t.Result);
            }
            ret = ret.OrderBy(x => x.Cost).Skip(ret.Count - K).ToList();
            UpdateBoardStates(ret);
            return ret;
        }

        private  void UpdateBoardStates(List<Move> moves)
        {
            BoardStates.Clear();
            int i = 0;
            foreach (var move in moves)
            {
                BoardStates.Add(new BoardState(move.NewHeights, i));
            }
        }
    }
}
