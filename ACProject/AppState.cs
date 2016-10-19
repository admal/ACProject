using System.Collections.Generic;
using ACProject.Domain.Demo;
using ACProject.Domain.Models;

namespace ACProject
{
    public class AppState
    {
        private static AppState _instance;

        private IList<IBlock> _blocks;

        public IList<IBlock> Blocks
        {
            get { return _blocks; }
        }

        protected AppState()
        {
            _blocks = new List<IBlock>();
            for (int i = 0; i < 5; i++)
            {
                _blocks.Add(new DummyBlock
                {
                    Count = i
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