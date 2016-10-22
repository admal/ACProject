using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACProject.Domain.Models;

namespace ACProject.Domain.Demo
{
    public class DummyBlock : IBlock
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public int[,] Grid { get; set; }
    }
}
