using System.Collections.Generic;
using System.ComponentModel;
using ACProject.Domain.Models;

namespace ACProject.Interfaces
{
    public interface IUpdateableForm : ISynchronizeInvoke
    {
        void UpdateForm();
    }
}