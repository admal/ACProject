﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.UIHelpers;
using System.IO;
using ACProject.Domain.Models;

namespace ACProject.Forms
{
    enum SimulationState
    {
        NotStarted,
        Started,
        Paused
    }

    public partial class Main : Form
    {
        private Control panelCanvas;
        private SimulationState _simulationState = SimulationState.NotStarted;
        private int _cellSize;
        private uint _width;
        private IList<IBlock> _shownBlocks;
        private int _k;
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            tbWidth.Text = AppState.Instance.Width.ToString();
            _width = AppState.Instance.Width;
            _shownBlocks = new List<IBlock>();
            _k = 1;
            tbK.Text = _k.ToString();
            EnableButtons();
        }

        private void OnWindowLoad(object sender, EventArgs e)
        {
            GenerateBoards();
        }

        private void EnableButtons()
        {
            switch (_simulationState)
            {
                case SimulationState.NotStarted:
                    btnPause.Enabled = false;
                    btnNextStep.Enabled = false;
                    btnReset.Enabled = true;
                    btnApply.Enabled = true;
                    btnStart.Enabled = true;
                    break;
                case SimulationState.Started:
                    btnPause.Enabled = true;
                    btnNextStep.Enabled = false;
                    btnReset.Enabled = false;
                    btnApply.Enabled = false;
                    btnStart.Enabled = false;
                    break;
                case SimulationState.Paused:
                    btnPause.Enabled = false;
                    btnNextStep.Enabled = true;
                    btnReset.Enabled = true;
                    btnApply.Enabled = false;
                    btnStart.Enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void showBlocksOverview(object sender, EventArgs e)
        {
            var overviewForm = new Overview();
            overviewForm.ShowDialog(this);
        }

        public void UpdateGrid()
        {
            if (_simulationState == SimulationState.NotStarted)
            {
                GenerateBoards();

                _shownBlocks.Clear();
                tbWidth.Text = AppState.Instance.Width.ToString();
                _width = AppState.Instance.Width;
                panelCanvas = tabBoards.Controls[0].Controls[0];

                //_cellSize = (int) (panelCanvas.Width/_width);

               panelCanvas.Invalidate();
            }
            else
            {
                MessageBoxService.ShowError("Configuration not saved! You must stop simulation first!");
            }

        }

        private void Apply(object sender, EventArgs e)
        {
            try
            {
                var input = tbWidth.Text;
                var width = uint.Parse(input);
                _k = int.Parse(tbK.Text);

                GenerateBoards();

                AppState.Instance.Width = width;


                _width = width;
                _cellSize = (int)(panelCanvas.Width / _width);
                panelCanvas.Invalidate();
            }
            catch (Exception)
            {
                MessageBoxService.ShowError("Invalid operation!");
            }
        }

        private void GenerateBoards()
        {
            tabBoards.TabPages.Clear();
            tabBoards.Controls.Clear();

            TabPage currentTab = null;
            for (int i = 0; i < _k; i++)
            {
                
                if (i%4 == 0)
                {
                    var grid = new TableLayoutPanel
                    {
                        RowCount = 2,
                        ColumnCount = 2,
                        ColumnStyles = {new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50)},
                        RowStyles = {new RowStyle(SizeType.Percent, 50), new RowStyle(SizeType.Percent, 50)},
                        GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
                        Dock = DockStyle.Fill
                    };

                    currentTab = new TabPage("tab");
                    currentTab.Dock = DockStyle.Fill;
                    
                    currentTab.Controls.Add(grid);
                    tabBoards.TabPages.Add(currentTab);
                }

                var currGrid = currentTab.Controls[0];
                var newBoard = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.AntiqueWhite,
                    BorderStyle = BorderStyle.FixedSingle
                };
                newBoard.Paint += OnPaint;
                
                currGrid.Controls.Add(newBoard);
            }

        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            panelCanvas = tabBoards.Controls[0].Controls[0];
            _cellSize = 15;// (int)(control.Width / _width);

            var graphics = e.Graphics;

            var cellSize = _cellSize;

            int viewHeight = panelCanvas.Height/cellSize;

            using (var pen = new Pen(Color.Black))
            {
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < viewHeight; j++)
                    {
                        graphics.DrawRectangle(pen, new Rectangle(i * cellSize, j * cellSize, cellSize, cellSize));
                    }
                }
            }
                var rnd = new Random();
                foreach (var block in _shownBlocks)
                {
                    int posX = rnd.Next(0, (int) _width - AppState.Instance.MaxBlockSize);
                    int posY = rnd.Next(0, viewHeight - AppState.Instance.MaxBlockSize);

                    block.Draw(graphics, new Point(posX * cellSize, posY * cellSize), cellSize );
                }
        }

        private void StartSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.Started;
            EnableButtons();
        }

        private void PauseSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.Paused;
            EnableButtons();
        }

        private void ResetSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.NotStarted;
            EnableButtons();
        }

        private int counter = 0;

        private void MoveOneStepSimulation(object sender, EventArgs e)
        {
            _shownBlocks.Add(AppState.Instance.Blocks[counter]);
            counter =(++counter) % AppState.Instance.Blocks.Count;

            panelCanvas.Invalidate();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

    }
}
