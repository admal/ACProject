using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.UIHelpers;
using System.IO;
using System.Threading;
using ACProject.Algorithm;
using ACProject.CustomThreads;
using ACProject.Domain.Models;
using ACProject.Interfaces;

namespace ACProject.Forms
{
    enum SimulationState
    {
        NotStarted,
        Started,
        Paused
    }

    public partial class Main : Form, IGridForm
    {
        private Control panelCanvas;
        private SimulationState _simulationState = SimulationState.NotStarted;
        private int _cellSize;
        private uint _width;
        private IList<IList<IBoardBlock> >_shownBlocks;
        private int _k;
        private bool _computing = false;
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            tbWidth.Text = AppState.Instance.Width.ToString();
            _width = AppState.Instance.Width;
            _shownBlocks = new List<IList<IBoardBlock>>();
            _shownBlocks.Add(new List<IBoardBlock>());
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

        public void InitGrid()
        {
            if (_simulationState == SimulationState.NotStarted)
            {
                GenerateBoards();

                _shownBlocks.Clear();
                for (int i = 0; i < _k; i++)
                {
                    _shownBlocks.Add(new List<IBoardBlock>());
                }
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
            int onPageGridCount = 2;

            tabBoards.TabPages.Clear();
            tabBoards.Controls.Clear();

            TabPage currentTab = null;
            for (int i = 0; i < _k; i++)
            {
                _shownBlocks.Add(new List<IBoardBlock>());

                if (i % onPageGridCount == 0)
                {
                    var grid = new TableLayoutPanel
                    {
                        RowCount = 1,
                        ColumnCount = 2,
                        ColumnStyles =
                        {
                            new ColumnStyle(SizeType.Percent, 50),
                            new ColumnStyle(SizeType.Percent, 50),
                            //new ColumnStyle(SizeType.Percent, 25),
                            //new ColumnStyle(SizeType.Percent, 25)
                        },
                        RowStyles = {new RowStyle(SizeType.Percent, 100)},
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
                newBoard.Tag = i;
                newBoard.Paint += OnPaint;
                
                currGrid.Controls.Add(newBoard);
            }

        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            panelCanvas = tabBoards.Controls[0].Controls[0];
            var control = sender as Control;
            _cellSize = 15 < (int)(control.Width / _width) ? 15 : (int)(control.Width / _width);

            var graphics = e.Graphics;

            var cellSize = _cellSize;

            int viewHeight = panelCanvas.Height/cellSize;

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
            using (var brush = new SolidBrush(Color.Aqua))
            {
                var blocks = _shownBlocks[tabIndex];
                foreach (var block in blocks)
                {
                    block.Draw(graphics, brush, cellSize);
                }
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

        private int counter = 0;

        private void MoveOneStepSimulation(object sender, EventArgs e)
        {
            panelCanvas.Invalidate();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        //public IList<IBoardBlock> Blocks
        //{
        //    get { return _shownBlocks; }
        //    set { _shownBlocks = value; }
        //}

        public void UpdateGrid()
        {
            panelCanvas.Invalidate();
        }

        private void DoBackgroundWork(object sender, DoWorkEventArgs e)
        {
            while (_computing)
            {
                Debug.WriteLine("Starting tasks...");
                this.UseWaitCursor = true;
                //var taskList = new List<Task>();
                //for (int i = 0; i < _k; i++)
                //{
                //    var thread = new ComputingThread(_k, AppState.Instance.Blocks, (int)_width, this);
                //    var task = Task.Factory.StartNew(thread.StartComputations);
                //    taskList.Add(task);
                //}
                //Task.WaitAll(taskList.ToArray());

                var solver = new Solver(AppState.Instance.Blocks.Select(b => new MultipleBlock()
                {
                    Count = b.Count,
                    Block = b
                }).ToList(), (int)_width);
                var nextBlock = AppState.Instance.Blocks.FirstOrDefault(x => x.Count != 0);
                if (nextBlock == null)
                {
                    MessageBoxService.ShowInfo("Computation ended!");
                    _computing = false;
                    return;
                }
                nextBlock.Count--;
                var ret = solver.GetNextMoves(new BoardBlock(nextBlock, new Point()));
                foreach (var move in ret)
                {
                    _shownBlocks[move.Board].Add(move.Block);
                }

                UpdateGrid();
                this.UseWaitCursor = false;
                //FIND MINIMUM FROM PIOTR'S DATA
                Debug.WriteLine("All finished");
                Thread.Sleep(1000);
            }

        }
    }
}
