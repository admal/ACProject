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
        private int _maxBlockWidth;
        public BlockOverview(IBlock block, int maxBlockWidth)
        {
            InitializeComponent();
            _block = block;
            _maxBlockWidth = maxBlockWidth;
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

        private void OnPaint(object sender, PaintEventArgs e)
        {
            
            Graphics graphics = e.Graphics;

            var width = blockView.Width;

            var blockWidth = _block.Grid.GetLength(0);

            int cellSize = width / _maxBlockWidth;

            using (var pen = new Pen(Color.Black))
            {

                for (int i = 0; i < _maxBlockWidth; i++)
                {
                    for (int j = 0; j < _maxBlockWidth; j++)
                    {
                        graphics.DrawRectangle(pen, i * cellSize, j * cellSize, cellSize, cellSize);
                    }
                }
            }
            _block.Draw(graphics, new Point(0,0), cellSize);
            
            
        }
    }
}
