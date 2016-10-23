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
using ACProject.UIHelpers;

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
                var blockOverviewControl = new BlockOverview(block, AppState.Instance.MaxBlockWidth);
                // var tab = new TabPage("tab" +count);
                panelBlocksOverview.Controls.Add(blockOverviewControl);
                count++;
            }
        }

        private void Draw()
        {
            panelBlocksOverview.Controls.Clear();
            var count = 0;
            foreach (var block in _blocks)
            {
                var blockOverviewControl = new BlockOverview(block, AppState.Instance.MaxBlockWidth);
                // var tab = new TabPage("tab" +count);
                panelBlocksOverview.Controls.Add(blockOverviewControl);
                count++;
            }
        }

        private void ChooseBlockFile(object sender, EventArgs e)
        {
             OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "block files (*.blk)|*.blk|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filename = dialog.FileName;
                tbFilePath.Text = filename;
                try
                {
                    AppState.Instance.LoadInitial(filename);
                    Draw();
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            var main = this.Owner as Main;
            main.UpdateGrid();
        }
    }
}
