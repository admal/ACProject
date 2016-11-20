using System.Collections.Generic;
using System.ComponentModel;
using ACProject.Domain.Models;

namespace ACProject.Interfaces
{
    public interface IGridForm : ISynchronizeInvoke
    {
        IList<IBoardBlock> Blocks { get; set; }
        void UpdateGrid();
    }
}