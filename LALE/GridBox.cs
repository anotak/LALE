using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LALE
{
    public partial class GridBox : InterpolationPicturebox
    {

        private Color hoverColor = Color.White;
        private Color selectedColor = Color.Red;
        private Size selectionSize = new Size(12, 10);
        private int selectedMap = 0;
        private bool canSelect = true;
        private bool hoverBox = true;
        private bool allowMultiSelection = false;
        private Rectangle selectionRectangle = new Rectangle(0, 0, 1, 1);
        private int hoverIndex = -1;
        private int lastHoverIndex = -1;
        private int startSelection = -1;
        private Size canvas = new Size(160, 128);
        private Pen selectionPen = new Pen(Color.Red, 2);

        public GridBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void GridBox_Load(object sender, EventArgs e)
        {

        }

        [Description("The hover color."), Browsable(true)]
        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; this.Invalidate(); }
        }

        [Description("The selection color."), Browsable(true)]
        public Color SelectionColor
        {
            get { return selectedColor; }
            set { selectedColor = value; selectionPen = new Pen(selectedColor, 2); this.Invalidate(); }
        }

        [Description("The box size."), Browsable(true)]
        public Size BoxSize
        {
            get { return selectionSize; }
            set { selectionSize = value; this.Invalidate(); }
        }

        [Description("The selected index."), Browsable(true)]
        public int SelectedIndex
        {
            get { return selectedMap; }
            set { selectedMap = value; selectionRectangle = new Rectangle((value % (canvas.Width / selectionSize.Width)), (value / (canvas.Height / selectionSize.Height)), 1, 1); this.Invalidate(); }
        }

        [Description("Determines whether or not items can be selected."), Browsable(true)]
        public bool Selectable
        {
            get { return canSelect; }
            set { canSelect = value; this.Invalidate(); }
        }

        [Description("Determines whether or not the hoverbox will be shown."), Browsable(true)]
        public bool HoverBox
        {
            get { return hoverBox; }
            set { hoverBox = value; this.Invalidate(); }
        }

        [Description("The canvas size."), Browsable(true)]
        public Size CanvasSize
        {
            get { return canvas; }
            set { canvas = value; }
        }

        [Browsable(false)]
        public int HoverIndex
        {
            get { return hoverIndex; }
        }

        [Browsable(false)]
        public Rectangle SelectionRectangle
        {
            get { return selectionRectangle; }
            set { selectionRectangle = value; this.Invalidate(); }
        }

        [Description("Determines whether or not more than one items can be selected."), Browsable(true)]
        public bool AllowMultiSelection
        {
            get { return allowMultiSelection; }
            set { allowMultiSelection = value; }
        }

        private void GridBox_Paint(object sender, PaintEventArgs e)
        {
            if (canSelect)
            {
                if (!allowMultiSelection)
                {
                    if (selectedMap != -1)
                    {
                        Point p = getIndexPoint(selectedMap);
                        e.Graphics.DrawRectangle(selectionPen, p.X, p.Y, selectionSize.Width, selectionSize.Height);
                    }
                }
                else
                {
                    Point p = getIndexPoint(selectedMap);
                    e.Graphics.DrawRectangle(selectionPen, p.X, p.Y, selectionSize.Width * selectionRectangle.Width, selectionSize.Height * selectionRectangle.Height);
                }
            }

            if (hoverBox)
            {
                if (hoverIndex != -1)
                {
                    Point p = getIndexPoint(hoverIndex);
                    e.Graphics.DrawRectangle(new Pen(hoverColor), p.X, p.Y, selectionSize.Width - 1, selectionSize.Height - 1);
                }
            }
        }

        private Point getIndexPoint(int i)
        {
            int width = (canvas.Width / selectionSize.Width);
            int height = (canvas.Height / selectionSize.Height);
            int x = i % width;
            int y = i / width;
            return new Point(x * selectionSize.Width, y * selectionSize.Height);
        }

        private void GridBox_MouseMove(object sender, MouseEventArgs e)
        {
            int x;
            int y;
            int width;
            int height;
            if (allowMultiSelection && startSelection != -1)
            {
                x = e.X / selectionSize.Width;
                y = e.Y / selectionSize.Height;
                width = x - selectionRectangle.X + 1;
                height = y - selectionRectangle.Y + 1;
                if (width > 0)
                    selectionRectangle.Width = width;
                else
                    selectionRectangle.Width = 1;
                if (height > 0)
                    selectionRectangle.Height = height;
                else
                    selectionRectangle.Height = 1;
                if (selectionRectangle.X + selectionRectangle.Width > canvas.Width / selectionSize.Width)
                    selectionRectangle.Width = (canvas.Width / selectionSize.Width) - selectionRectangle.X;
                if (selectionRectangle.Y + selectionRectangle.Height > canvas.Height / selectionSize.Height)
                    selectionRectangle.Height = (canvas.Height / selectionSize.Height) - selectionRectangle.Y;
                this.Invalidate();
                return;
            }

            if (e.X < 0 || e.Y < 0 || e.X >= canvas.Width || e.Y >= canvas.Height)
            {
                if (hoverIndex != -1)
                {
                    hoverIndex = -1;
                    lastHoverIndex = -1;
                    this.Invalidate();
                }
                return;
            }

            width = (canvas.Width / selectionSize.Width);
            height = (canvas.Height / selectionSize.Height);
            x = e.X / selectionSize.Width;
            y = e.Y / selectionSize.Height;
            hoverIndex = x + y * width;

            if (hoverBox)
            {
                if (lastHoverIndex != hoverIndex)
                {
                    lastHoverIndex = hoverIndex;
                    this.Invalidate();
                }
            }
        }

        private void GridBox_MouseLeave(object sender, EventArgs e)
        {
            if (hoverBox)
            {
                hoverIndex = -1;
                lastHoverIndex = -1;
                this.Invalidate();
            }
        }

        private void GridBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (canSelect && hoverIndex != -1)
            {
                startSelection = hoverIndex;
                selectionRectangle = new Rectangle((e.X / selectionSize.Width), (e.Y / selectionSize.Height), 1, 1);
                selectedMap = hoverIndex;
                this.Invalidate();
            }
        }

        private void GridBox_MouseUp(object sender, MouseEventArgs e)
        {
            startSelection = -1;
        }
    }  
}
