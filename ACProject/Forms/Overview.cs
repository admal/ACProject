using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.Controls;
using ACProject.Domain.Demo;
using ACProject.Domain.Models;

namespace ACProject.Forms
{
    public partial class Overview : Form
    {
        private IList<IBlock> _blocks;
        public Overview()
        {
            _blocks = AppState.Instance.Blocks;

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            var count = 0;

            foreach (var block in _blocks)
            {
                var blockOverviewControl = new BlockOverview(block);
                // var tab = new TabPage("tab" +count);
                panelBlocksOverview.Controls.Add(blockOverviewControl);
                count++;
            }
        }
    }
}
