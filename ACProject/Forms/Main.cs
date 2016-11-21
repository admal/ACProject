using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.UIHelpers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ACProject.Algorithm;
using ACProject.CustomThreads;
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

    public partial class Main : Form, IGridForm
    {
        private Control panelCanvas;
        private SimulationState _simulationState = SimulationState.NotStarted;
        private int _cellSize;
        private uint _width;
        private int _k;
        private bool _computing = false;
        private IFormatter formatter;
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            tbWidth.Text = AppState.Instance.Width.ToString();
            _width = AppState.Instance.Width;
            AppState.Instance.BoardBlocks = new List<IList<IBoardBlock>>();
            AppState.Instance.BoardBlocks.Add(new List<IBoardBlock>());
            _k = 1;
            formatter = new BinaryFormatter();
            tbK.Text = _k.ToString();
            InitGrid();
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
                panelCanvas = tabBoards.Controls[0].Controls[0];

                AppState.Instance.Solver = new Solver(AppState.Instance.Blocks.Select(b => new MultipleBlock()
                {
                    Count = b.Count,
                    Block = b
                }).ToList(), (int)_width, _k);
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
                AppState.Instance.Solver = new Solver(AppState.Instance.Blocks.Select(b => new MultipleBlock()
                {
                    Count = b.Count,
                    Block = b
                }).ToList(), (int)_width, _k);

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
                AppState.Instance.BoardBlocks.Add(new List<IBoardBlock>());

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
                var blocks = AppState.Instance.BoardBlocks[tabIndex];
                foreach (var block in blocks)
                {
                    block.Draw(graphics, cellSize, board.Height);
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

        private void MoveOneStepSimulation(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            SimulationStep();
            this.UseWaitCursor = false;
            _simulationState = SimulationState.Paused;
            EnableButtons();
        }
        
        public void UpdateGrid()
        {
            panelCanvas.Invalidate(true);
        }

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
                for (int i = 0; i < steps; i++)
                {
                    SimulationStep();
                }
                this.UseWaitCursor = false;
                _simulationState = SimulationState.Paused;
                EnableButtons();                
            }
            catch (Exception)
            {
                MessageBoxService.ShowError("Provide positive number!");
            }
            
        }

        private void SimulationStep()
        {
            var solver = AppState.Instance.Solver;
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
                AppState.Instance.BoardBlocks[move.Board].Add(move.Block);
            }

            this.InvokeEx(f => f.UpdateGrid());
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = new SaveFileDialog();
            file.Filter = "AC project (*.bin) | *.bin";
            if (file.ShowDialog() != DialogResult.OK) return;
            Stream stream = new FileStream(file.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                formatter.Serialize(stream, AppState.Instance);
            }
            catch (Exception exception)
            {
                UIHelpers.MessageBoxService.ShowError(exception.Message);
            }
            stream.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = new OpenFileDialog();
            file.Filter = "AC project (*.bin) | *.bin";
            if (file.ShowDialog() != DialogResult.OK) return;
            Stream stream = new FileStream(file.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                AppState.Instance = (AppState)formatter.Deserialize(stream);
            }
            catch (Exception exception)
            {
                UIHelpers.MessageBoxService.ShowError(exception.Message);
            }
            stream.Close();
        }
    }
}
