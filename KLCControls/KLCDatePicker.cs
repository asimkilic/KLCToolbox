using System;
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
    public class KLCDatePicker : DateTimePicker
    {

        //Fields
        //-> Appearance
        private Color skinColor = Color.MediumSlateBlue;
        private Color textColor = Color.White;
        private Color borderColor = Color.PaleVioletRed;
        private int borderSize = 0;

        //-> Other Values
        private bool droppedDown = false;
        private Image calenderIcon = Properties.Resources.calendarWhite;
        private RectangleF iconButtonArea;
        private const int calenderIconWidth = 34;
        private const int arrowIconWidth = 17;

        // Properties
        [Category("KLC DatePicker Advance")]
        public Color KLCSkinColor
        {
            get
            {
                return skinColor;
            }
            set
            {
                skinColor = value;
                if (skinColor.GetBrightness() >= 0.8F)
                    calenderIcon = Properties.Resources.calendarDark;
                else
                    calenderIcon = Properties.Resources.calendarWhite;
                this.Invalidate();
            }
        }
        [Category("KLC DatePicker Advance")]
        public Color KLCTextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                this.Invalidate();
            }
        }
        [Category("KLC DatePicker Advance")]
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
        [Category("KLC DatePicker Advance")]
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



        // Constructor
        public KLCDatePicker()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(0, 35);
            this.Font = new Font(this.Font.Name, 9.5F);
        }

        // Overriden methods
        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            droppedDown = true;
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            droppedDown = false;
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Graphics graphics = this.CreateGraphics())
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (SolidBrush skinBrush = new SolidBrush(skinColor))
            using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (StringFormat textFormat = new StringFormat())
            {
                RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
                RectangleF iconArea = new RectangleF(clientArea.Width - calenderIconWidth, 0, calenderIconWidth, clientArea.Height);
                penBorder.Alignment = PenAlignment.Inset;
                textFormat.LineAlignment = StringAlignment.Center;

                // Draw surface
                graphics.FillRectangle(skinBrush, clientArea);
                //Draw text
                graphics.DrawString("    " + this.Text, this.Font, textBrush, clientArea, textFormat);
                //Draw open calender icon highlight
                if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);
                //Draw border
                if (borderSize >= 1) graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                //Draw icon
                graphics.DrawImage(calenderIcon, this.Width - calenderIcon.Width - 9, (this.Height - calenderIcon.Height) / 2);

            }

        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            int iconWidth = GetIconButtonWidth();
            iconButtonArea = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (iconButtonArea.Contains(e.Location))
                this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }
        // Private methods
        private int GetIconButtonWidth()
        {
            int textWidth = TextRenderer.MeasureText(this.Text, this.Font).Width;
            if (textWidth <= this.Width - (calenderIconWidth + 20))
                return calenderIconWidth;
            else return arrowIconWidth;
        }

    }
}
