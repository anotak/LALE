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
    public partial class ExportMap : Form
    {

        bool setClose = false;

        public ExportMap()
        {
            InitializeComponent();
        }

        private void ExportMap_Load(object sender, EventArgs e)
        {
            setValue(0, pBar.Maximum);
        }

        public void setValue(int map, int max)
        {
            setPBarValue(map);
            setLabelText("Map " + map + "/" + max + " - " + (int)((decimal)((decimal)map / (decimal)max) * (decimal)100) + "%");
            if (map == max)
            {
                setClose = true;
                close();
            }
        }

        private void setPBarValue(int value)
        {
            if (pBar.InvokeRequired)
            {
                pBar.BeginInvoke(new MethodInvoker(delegate() { setPBarValue(value); }));
            }
            else
            {
                pBar.Value = value;
            }
        }

        private void setLabelText(string text)
        {
            if (pBar.InvokeRequired)
            {
                pBar.BeginInvoke(new MethodInvoker(delegate() { setLabelText(text); }));
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        private void close()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate() { close(); }));
            }
            else
            {
                this.Close();
            }
        }

        private void frmExportMaps_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!setClose)
                e.Cancel = true;
        }
    }
}
