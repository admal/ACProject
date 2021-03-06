﻿using System;
using System.Collections.Generic;
using ACProject.Domain.Models;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ACProject.Algorithm;

namespace ACProject
{
    [Serializable]
    public class AppState
    {
        private static AppState _instance;

        private IList<IBlock> _blocks;
        private uint _width = 30;

        public Solver Solver { get; set; }

        public uint Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public IList<IBlock> Blocks
        {
            get { return _blocks; }
        }

        public IList<IList<IBoardBlock>> BoardBlocks { get; set; }


        public int MaxBlockSize
        {
            get
            {
                var maxWidth = _blocks.Max(x => x.Grid.GetLength(0));
                var maxHeight = _blocks.Max(x => x.Grid.GetLength(1));
                return maxWidth > maxHeight ? maxWidth : maxHeight;
            }
        }

        protected AppState()
        {
            var rnd = new Random();
            _blocks = new List<IBlock>();
            for (int i = 0; i < 3; i++)
            {
                _blocks.Add(new Block(3,3)
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
            set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// Parse tile block and put information into configuration of the application
        /// </summary>
        /// <param name="filename"></param>
        public void LoadInitial(string filename)
        {
            this.Blocks.Clear();
            using (StreamReader sr = File.OpenText(filename))
            {
                string line = String.Empty;
                int state = 0;
                int numOfBlocks = 0;
                int blockHeight = 0;
                int currRowNo = 0;
                IBlock currentBlock = new BoardBlock(1,1);
                while ((line = sr.ReadLine()) != null)
                {
                    switch (state)
                    {
                        case 0:
                            string[] vals = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            this.Width = uint.Parse(vals[0]);
                            numOfBlocks = int.Parse(vals[1]);
                            state = 1;
                            break;
                        case 1:
                            string[] vals1 = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            currentBlock = new Block(int.Parse(vals1[0]), int.Parse(vals1[1]));
                            blockHeight = int.Parse(vals1[1]);
                            state = 2;
                            currRowNo = 0;
                            break;
                        case 2:
                            string[] vals2 = line.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < vals2.Length; i++)
                            {
                                currentBlock.Grid[i, currRowNo] = int.Parse(vals2[i]);
                                if(!(int.Parse(vals2[i]) == 0 || int.Parse(vals2[i]) ==1))
                                    throw new Exception();
                            }
                            currRowNo++;
                            if (currRowNo == blockHeight)
                            {
                                state = 1;
                                this.Blocks.Add(currentBlock);
                            }
                            break;
                    }
                }
            }
        }
    }
}