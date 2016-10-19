using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACProject.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void showBlocksOverview(object sender, EventArgs e)
        {
            var overviewForm = new Overview();
            overviewForm.ShowDialog();
        }
    }
}
