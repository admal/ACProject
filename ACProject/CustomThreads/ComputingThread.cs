using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ACProject.Domain.Models;
using ACProject.Extensions;
using ACProject.Interfaces;

namespace ACProject.CustomThreads
{
    public class ComputingThread
    {
        private int _k;
        private IList<IBoardBlock> _blocks;
        private int _width;
        private IUpdateableForm _form;

        //public ComputingThread(int k, IList<IBoardBlock> blocks, int width, IGridForm form)
        //{
        //    _k = k;
        //    _blocks = blocks;
        //    _width = width;
        //    _form = form;
        //}

        public void StartComputations()
        {
            Debug.WriteLine("Thread started...");
            Thread.Sleep(3000);
            Debug.WriteLine("Clearing blocks...");
            //_blocks.Clear();
            Debug.WriteLine("Invoke...");
            _form.InvokeEx(f => f.UpdateForm());
        }

        public void PauseComputations()
        {
            
        }

        public void StopComputations()
        {
            
        }
    }
}