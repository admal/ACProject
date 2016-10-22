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
using ACProject.UIHelpers;

namespace ACProject.Controls
{
    public partial class BlockOverview : UserControl
    {
        private IBlock _block;
        public BlockOverview(IBlock block)
        {
            InitializeComponent();
            _block = block;
            tbBlocksCount.Text = block.Count.ToString();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            //this.Dock = DockStyle.Fill;
        }

        private void OnInputChange(object sender, EventArgs e)
        {
            var input = tbBlocksCount.Text;

            try
            {
                var count = int.Parse(input);
                _block.Count = count;
            }
            catch (FormatException)
            {
                MessageBoxService.ShowError("Invalid operation!");
            }
        }
    }
}
