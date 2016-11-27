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
using ACProject.Domain.Models;
using ACProject.Interfaces;
using ACProject.UIHelpers;

namespace ACProject.Forms
{
    public partial class Overview : Form, IUpdateableForm
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
                var blockOverviewControl = new BlockOverview(block, AppState.Instance.MaxBlockSize, this);
                // var tab = new TabPage("tab" +count);
                blocksPanel.Controls.Add(blockOverviewControl);
                count++;
            }

            tbBlocksCount.Text = _blocks.Sum(b => b.Count).ToString();
        }

        private void Draw()
        {
            blocksPanel.Controls.Clear();
            var count = 0;
            foreach (var block in _blocks)
            {
                var blockOverviewControl = new BlockOverview(block, AppState.Instance.MaxBlockSize, this);
                // var tab = new TabPage("tab" +count);
                blocksPanel.Controls.Add(blockOverviewControl);
                count++;
            }
        }

        private void ChooseBlockFile(object sender, EventArgs e)
        {
             OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";

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
            main.InitGrid();
        }

        private void SaveAndExit(object sender, EventArgs e)
        {
            this.Close();
        }

        public void UpdateForm()
        {
            tbBlocksCount.Text = _blocks.Sum(b => b.Count).ToString();
        }

        private void ApplyBlocksCount(object sender, EventArgs e)
        {
            try
            {
                var count = uint.Parse(tbPerBlockCount.Text);
                foreach (var block in _blocks)
                {
                    block.Count = (int) count;
                }
                Draw();
            }
            catch (Exception)
            {
                MessageBoxService.ShowError("Provide positive number!");
            }
        }
    }
}
