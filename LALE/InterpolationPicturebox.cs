using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LALE
{
    public partial class InterpolationPicturebox : PictureBox
    {
        public InterpolationPicturebox()
        {
            InitializeComponent();
        }

        InterpolationMode m = InterpolationMode.Default;
        public InterpolationMode InterpolationMode
        {
            get { return m; }
            set { m = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = m;
            try
            {
                base.OnPaint(pe);
            }
            catch (Exception) { }
        }

        private void InterpolationPicturebox_Load(object sender, EventArgs e)
        {

        }

        private void InterpolationPicturebox_Load_1(object sender, EventArgs e)
        {

        }
    }
}
