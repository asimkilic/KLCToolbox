﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace KLCToolbox.KLCControls
{
    public class KLCButton : Button
    {
        // Fields
        private int borderSize = 0;
        private int borderRadius = 40;
        private Color borderColor = Color.PaleVioletRed;
        public KLCButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;
            this.Resize += new EventHandler(Button_Resize);
        }


        // Properties
        [Category("KLC Button Advance")]
        public int KLCBorderSize
        {
            get => borderSize;

            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("KLC Button Advance")]
        public int KLCBorderRadius
        {
            get => borderRadius;
            set
            {
                if (value <= this.Height)
                    borderRadius = value;
                else borderRadius = this.Height;
                this.Invalidate();
            }
        }
        [Category("KLC Button Advance")]
        public Color KLCBorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("KLC Button Advance")]
        public Color KLCBackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("KLC Button Advance")]
        public Color KLCTextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }


        // Methods
        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(1, 1, this.Width - 0.8F, this.Height - 1);
            if (borderRadius > 2) // Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - 1F))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    // button surface
                    this.Region = new Region(pathSurface);
                    // Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    //button border
                    if (borderSize >= 1)
                        // draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else // Normal button
            {
                //button surface
                this.Region = new Region(rectSurface);
                //button border
                if (borderSize >= 1)
                {
                    using (Pen penborder = new Pen(borderColor, borderSize))
                    {
                        penborder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penborder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }

        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
                this.Invalidate();
        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                KLCBorderRadius = this.Height;
        }

    }
}
