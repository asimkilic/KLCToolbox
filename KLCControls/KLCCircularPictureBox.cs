using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using KLCToolbox.Properties;

namespace KLCToolbox.KLCControls
{
    public class KLCCircularPictureBox : PictureBox
    {
        //Fields
        private int borderSize = 2;
        private Color borderColor = Color.RoyalBlue;
        private Color borderColor2 = Color.HotPink;
        private DashStyle borderLineStyle = DashStyle.Solid;
        private DashCap borderCapStyle = DashCap.Flat;
        private float gradientAngle = 50F;

        // Constructor
        public KLCCircularPictureBox()
        {
            this.Size = new Size(100, 100);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
          
        }
        // Properties
        [Category("KLC Picture Box Advance")]
        public int KLCBorderSize
        {
            get
            {
                return borderSize;
            }

            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("KLC Picture Box Advance")]
        public Color KLCBorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("KLC Picture Box Advance")]
        public Color KLCBorderColor2
        {
            get
            {
                return borderColor2;
            }

            set
            {
                borderColor2 = value;
                this.Invalidate();
            }
        }
        [Category("KLC Picture Box Advance")]
        public DashStyle KLCBorderLineStyle
        {
            get
            {
                return borderLineStyle;
            }

            set
            {
                borderLineStyle = value;
                this.Invalidate();
            }
        }
        [Category("KLC Picture Box Advance")]
        public DashCap KLCBorderCapStyle
        {
            get
            {
                return borderCapStyle;
            }

            set
            {
                borderCapStyle = value;
                this.Invalidate();
            }
        }
        [Category("KLC Picture Box Advance")]
        public float KLCGradientAngle
        {
            get
            {
                return gradientAngle;
            }

            set
            {
                gradientAngle = value;
                this.Invalidate();
            }
        }

        // Overriden methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = new Size(this.Width, this.Width);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //Fields
            var graph = pe.Graphics;
            var rectContourSmooth = Rectangle.Inflate(this.ClientRectangle, -1, -1);
            var rectBorder = Rectangle.Inflate(rectContourSmooth, -borderSize, -borderSize);
            var smoothSize = borderSize > 0 ? borderSize * 3 : 1;
            using (var borderGColor = new LinearGradientBrush(rectBorder, borderColor, borderColor2, gradientAngle))
            using (var pathRegion = new GraphicsPath())
            using (var penSmooth = new Pen(this.Parent.BackColor, smoothSize))
            using (var penBorder = new Pen(borderGColor, borderSize))
            {
                penBorder.DashStyle = borderLineStyle;
                penBorder.DashCap = borderCapStyle;
                pathRegion.AddEllipse(rectContourSmooth);
                this.Region = new Region(pathRegion);
                graph.SmoothingMode = SmoothingMode.AntiAlias;

                // Drawing
                graph.DrawEllipse(penSmooth, rectContourSmooth);
                if (borderSize > 0)
                    graph.DrawEllipse(penBorder, rectBorder);

            }
        }
    }
}
