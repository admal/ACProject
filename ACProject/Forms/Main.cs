﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ACProject.UIHelpers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ACProject.Algorithm;
using ACProject.Domain.Models;
using ACProject.Extensions;
using ACProject.Interfaces;

namespace ACProject.Forms
{
    enum SimulationState
    {
        NotStarted,
        Started,
        Paused
    }

    public partial class Main : Form, IUpdateableForm
    {
        private Control _panelCanvas;
        private SimulationState _simulationState = SimulationState.NotStarted;
        private int _cellSize;
        private uint _width;
        private int _k;
        private bool _computing = false;
        private readonly IFormatter _formatter;
        private double _bestDensity = -1d;
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            tbWidth.Text = AppState.Instance.Width.ToString();
            _width = AppState.Instance.Width;
            AppState.Instance.BoardBlocks = new List<IList<IBoardBlock>>();
            AppState.Instance.BoardBlocks.Add(new List<IBoardBlock>());
            _k = 1;
            _formatter = new BinaryFormatter();
            tbK.Text = _k.ToString();
            InitGrid();
            EnableButtons();
        }

        private void OnWindowLoad(object sender, EventArgs e)
        {
            GenerateBoards();
        }

        /// <summary>
        /// Enables and disables buttons due to state of the application.
        /// </summary>
        private void EnableButtons()
        {
            switch (_simulationState)
            {
                case SimulationState.NotStarted:
                    btnPause.Enabled = false;
                    btnNextStep.Enabled = true;
                    btnJump.Enabled = true;
                    tbJump.Enabled = true;
                    btnReset.Enabled = true;
                    btnApply.Enabled = true;
                    btnStart.Enabled = true;
                    break;
                case SimulationState.Started:
                    btnPause.Enabled = true;
                    btnNextStep.Enabled = false;
                    btnJump.Enabled = false;
                    tbJump.Enabled = false;
                    btnReset.Enabled = false;
                    btnApply.Enabled = false;
                    btnStart.Enabled = false;
                    break;
                case SimulationState.Paused:
                    btnPause.Enabled = false;
                    btnNextStep.Enabled = true;
                    btnJump.Enabled = true;
                    tbJump.Enabled = true;
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

        public void InitGrid()
        {
            if (_simulationState == SimulationState.NotStarted)
            {
                GenerateBoards();

                AppState.Instance.BoardBlocks.Clear();
                for (int i = 0; i < _k; i++)
                {
                    AppState.Instance.BoardBlocks.Add(new List<IBoardBlock>());
                }
                tbWidth.Text = AppState.Instance.Width.ToString();
                _width = AppState.Instance.Width;
                _panelCanvas = tabBoards.Controls[0].Controls[0];

                AppState.Instance.Solver = new Solver(AppState.Instance.Blocks.Select(b => new MultipleBlock()
                {
                    Count = b.Count,
                    Block = new BoardBlock(b, new Point())
                }).ToList(), (int)_width, _k);
               _panelCanvas.Invalidate();
            }

        }

        private void Apply(object sender, EventArgs e)
        {
            try
            {
                var input = tbWidth.Text;
                var width = uint.Parse(input);
                _k = int.Parse(tbK.Text);
                _width = width;

                GenerateBoards();

                AppState.Instance.Width = width;
                AppState.Instance.Solver = new Solver(AppState.Instance.Blocks.Select(b => new MultipleBlock()
                {
                    Count = b.Count,
                    Block = new BoardBlock(b, new Point())
                }).ToList(), (int)_width, _k);

                _width = width;
                _cellSize = (int)(_panelCanvas.Width / _width);
                _panelCanvas.Invalidate();
            }
            catch (FormatException)
            {
                MessageBoxService.ShowError("Invalid operation!");
            }
        }

        private void GenerateBoards()
        {
            int onPageGridCount = 1;

            tabBoards.TabPages.Clear();
            tabBoards.Controls.Clear();

            TabPage currentTab = null;
            for (int i = 0; i < _k; i++)
            {
                AppState.Instance.BoardBlocks.Add(new List<IBoardBlock>());

                if (i%onPageGridCount == 0)
                {
                    var grid = new TableLayoutPanel
                    {
                        RowCount = 1,
                        ColumnCount = onPageGridCount,
                        ColumnStyles =
                        {
                            new ColumnStyle(SizeType.Percent, 100)
                        },
                        RowStyles = {new RowStyle(SizeType.Percent, 100)},
                        Dock = DockStyle.Fill
                    };

                    currentTab = new TabPage("tab");

                    currentTab.Controls.Add(grid);
                    tabBoards.TabPages.Add(currentTab);
                }
                var currGrid = currentTab.Controls[0] as TableLayoutPanel;
                currGrid.AutoScroll = true;
                var panelWrapper = new FlowLayoutPanel();
                //panelWrapper.AutoSize = true;
                panelWrapper.AutoScroll = false;
                panelWrapper.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                panelWrapper.VerticalScroll.Visible = true;
                panelWrapper.Dock = DockStyle.Fill;
                

                var newBoard = new Panel
                {
                    BackColor = Color.AntiqueWhite,
                    BorderStyle = BorderStyle.FixedSingle,
                    Height = 4000
                };
                newBoard.Tag = i;
                newBoard.Paint += OnPaint;

                panelWrapper.Controls.Add(newBoard);

                currGrid.Controls.Add(panelWrapper);
                panelWrapper.AutoScroll = true; //need to reset it
            }
            
        }
        
        /// <summary>
        /// Event handler to repaint tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            _panelCanvas = tabBoards.Controls[0].Controls[0].Controls[0].Controls[0]; //trzbea coś z tym zrobić
            var control = sender as Control;
            _cellSize = 10;// < (int)(control.Width / _width) ? 15 : (int)(control.Width / _width);
            control.Width = (int)( _cellSize*_width);
            var graphics = e.Graphics;

            var cellSize = _cellSize;

            int viewHeight = _panelCanvas.Height/cellSize;
            

            using (var pen = new Pen(Color.Black))
            {
                for (int y = 0; y < viewHeight; ++y)
                {
                    graphics.DrawLine(pen, 0, y * cellSize, _width * cellSize, y * cellSize);
                }

                for (int x = 0; x < _width + 1; ++x)
                {
                    graphics.DrawLine(pen, x * cellSize, 0, x * cellSize, viewHeight * cellSize);
                }
            }
           
            //drawing blocks
            var board = sender as Control;
            int tabIndex = (int) board.Tag;
            var blocks = AppState.Instance.BoardBlocks[tabIndex];
            foreach (var block in blocks)
            {
                block.Draw(graphics, cellSize, board.Height);
            }
        }

        private void StartSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.Started;
            EnableButtons();
            _computing = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void PauseSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.Paused;
            _computing = false;
            EnableButtons();
        }

        private void ResetSimulation(object sender, EventArgs e)
        {
            _simulationState = SimulationState.NotStarted;
            _computing = false;
            InitGrid();
            EnableButtons();
        }

        private void MoveOneStepSimulation(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            _computing = true;
            SimulationStep();
            this.UseWaitCursor = false;
            _simulationState = SimulationState.Paused;
            EnableButtons();
        }
        /// <summary>
        /// Forces repainting all tabs and sets the density textbox
        /// </summary>
        public void UpdateForm()
        {
            _panelCanvas.Invalidate(true);
            tbDensity.Text = _bestDensity.ToString();
        }
        /// <summary>
        /// Method started in the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoBackgroundWork(object sender, DoWorkEventArgs e)
        {
            while (_computing)
            {
                Debug.WriteLine("Start simulation step");
                this.UseWaitCursor = true;
                SimulationStep();
                this.UseWaitCursor = false;
                Debug.WriteLine("Finished simulation step");
                Thread.Sleep(1000);
            }

        }

        private void JumpSimulation(object sender, EventArgs e)
        {
            try
            {
                var steps = uint.Parse(tbJump.Text);
                this.UseWaitCursor = true;
                _computing = true;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < steps; i++)
                {
                    SimulationStep();
                }
                stopwatch.Stop();
                MessageBoxService.ShowInfo("Elapsed: " + stopwatch.Elapsed);
                this.UseWaitCursor = false;
                _simulationState = SimulationState.Paused;
                _computing = false;
                EnableButtons();                
            }
            catch (Exception)
            {
                MessageBoxService.ShowError("Provide positive number!");
            }
            
        }
        /// <summary>
        /// Method does one step of the simulation.
        /// </summary>
        private void SimulationStep()
        {
            var solver = AppState.Instance.Solver;
            var ret = solver.GetNextMoves();

            if (ret.Count == 0)
            {
                if (_computing)
                {
                    MessageBoxService.ShowInfo("Computation ended!");
                    _computing = false;
                }
                return;
            }
                 
            for (int i = 0; i < ret.Count; i++)
            {
                var move = ret[i];
                if (move.Board == i)
                {
                    AppState.Instance.BoardBlocks[i].Add(move.Block);
                }
                else //draw using other board
                {
                    AppState.Instance.BoardBlocks[i] = new List<IBoardBlock>(AppState.Instance.BoardBlocks[move.Board]);
                    if (move.Board < i) //it means that the move on that board was already done, so remove it and redo with new move
                    {
                        AppState.Instance.BoardBlocks[i].RemoveAt(AppState.Instance.BoardBlocks[i].Count - 1);
                    }
                    AppState.Instance.BoardBlocks[i].Add(move.Block);
                }
                
            }
            _bestDensity = ret.Max(r => r.Density);
            this.InvokeEx(f => f.UpdateForm());
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = new SaveFileDialog {Filter = "AC project (*.bin) | *.bin"};
            if (file.ShowDialog() != DialogResult.OK) return;
            Stream stream = new FileStream(file.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                _formatter.Serialize(stream, AppState.Instance);
            }
            catch (Exception exception)
            {
                UIHelpers.MessageBoxService.ShowError(exception.Message);
            }
            stream.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = new OpenFileDialog {Filter = "AC project (*.bin) | *.bin"};
            if (file.ShowDialog() != DialogResult.OK) return;
            Stream stream = new FileStream(file.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                AppState.Instance = (AppState)_formatter.Deserialize(stream);
                _k = AppState.Instance.BoardBlocks.Count;
                GenerateBoards();
            }
            catch (Exception exception)
            {
                UIHelpers.MessageBoxService.ShowError(exception.Message);
            }
            stream.Close();
            UpdateForm();
        }
    }
}
