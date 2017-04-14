using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LALE
{
    public partial class Credits : Form
    {
        public int fatories = 0;

        public Credits()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fatories == 0)
            {
                label2.Text = "Fatories Fatories Fatories Fatories Fatories Fatories";
                label3.Text = label2.Text;
                button1.Text = "Close";
                fatories = 1;
            }
            else if (fatories == 1)
                this.Close();
        }
    }
}
