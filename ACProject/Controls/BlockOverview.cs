using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.Domain.Demo;
using ACProject.Domain.Models;

namespace ACProject.Controls
{
    public partial class BlockOverview : UserControl
    {
        public BlockOverview(IBlock block)
        {
            InitializeComponent();
            
            tbBlocksCount.Text = block.Count.ToString();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            //this.Dock = DockStyle.Fill;
        }
    }
}
