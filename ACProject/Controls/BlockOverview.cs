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

        private void OnPaint(object sender, PaintEventArgs e)
        {
            
            Graphics graphics = e.Graphics;

            var width = blockView.Width;
            var height = blockView.Height;

            var blockWidth = _block.Grid.GetLength(0);
            var blockHeight = _block.Grid.GetLength(1);

            int cellWidth = width / blockWidth;
            int cellHeight = height / blockHeight;

            using (SolidBrush brush = new SolidBrush(Color.Gray))
            {
                for (int i = 0; i < blockHeight; i++)
                {
                    for (int j = 0; j < blockWidth; j++)
                    {
                        if (_block.Grid[j, i] == 1)
                        {
                            var rect = new Rectangle(cellWidth * i, cellHeight * j, cellWidth, cellHeight);
                            graphics.FillRectangle(brush, rect);

                        }
                    }
                }
            }
        }
    }
}
