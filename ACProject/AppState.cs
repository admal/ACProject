using System;
using System.Collections.Generic;
using ACProject.Domain.Demo;
using ACProject.Domain.Models;

namespace ACProject
{
    public class AppState
    {
        private static AppState _instance;

        private IList<IBlock> _blocks;
        private uint _width = 30;

        public uint Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public IList<IBlock> Blocks
        {
            get { return _blocks; }
        }

        protected AppState()
        {
            var rnd = new Random();
            _blocks = new List<IBlock>();
            for (int i = 0; i < 3; i++)
            {
                _blocks.Add(new DummyBlock
                {
                    Count = rnd.Next(1, 12)
                });
            }
            _blocks[0].Grid = new int[3, 3] {{1, 0, 0}, {1, 1, 0}, {1, 0, 0}};
            _blocks[1].Grid = new int[3, 3] {{0, 0, 0}, {1, 1, 1}, {0, 0, 0}};
            _blocks[2].Grid = new int[3, 3] {{1, 1, 1}, {1, 1, 1}, {1, 1, 1}};
        }

        public static AppState Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new AppState();
                return _instance;
            }
        }
    }
}