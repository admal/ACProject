using ACProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Algorithm
{
    [Serializable]
    public class MultipleBlock
    {
        public BoardBlock Block { get; set; }
        public int Count { get; set; }

        public MultipleBlock()
        {

        }
        public MultipleBlock(MultipleBlock m)
        {
            Block = new BoardBlock(m.Block);
            Count = m.Count;
        }
    }
}
