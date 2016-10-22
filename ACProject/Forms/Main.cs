using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACProject.UIHelpers;

namespace ACProject.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            tbWidth.Text = AppState.Instance.Width.ToString();
        }

        

        private void showBlocksOverview(object sender, EventArgs e)
        {
            var overviewForm = new Overview();
            overviewForm.ShowDialog();
        }

        private void Apply(object sender, EventArgs e)
        {
            try
            {
                var input = tbWidth.Text;
                var width = uint.Parse(input);
                AppState.Instance.Width = width;
            }
            catch (Exception)
            {
                MessageBoxService.ShowError("Invalid operation!");
            }
        }
    }
}
