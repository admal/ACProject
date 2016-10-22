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
            for (int i = 0; i < 5; i++)
            {
                _blocks.Add(new DummyBlock
                {
                    Count = rnd.Next(1, 12)
                });
            }
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