using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLCToolbox.KLCControls
{
    [DefaultEvent("_TextChanged")]
    public partial class KLCTextBox : UserControl
    {
        // Fields
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private Color borderFocusColor = Color.HotPink;
        private bool isFocused = false;

        private int borderRadius = 0;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private bool isPasswordChar = false;

        //Constructor
        public KLCTextBox()
        {
            InitializeComponent();
        }

        //Events
        public event EventHandler _TextChanged;


        //Properties 
        [Category("KLC TextBox Advance")]
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
        [Category("KLC TextBox Advance")]
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
        [Category("KLC TextBox Advance")]
        public bool KLCUnderlinedStyle
        {
            get
            {
                return underlinedStyle;
            }

            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }
        [Category("KLC TextBox Advance")]
        public bool KLCPasswordChar
        {
            get { return isPasswordChar; }
            set
            {
                isPasswordChar = value;
                textBox1.UseSystemPasswordChar = value;
            }
        }
        [Category("KLC TextBox Advance")]
        public bool KLCMultiline
        {
            get { return textBox1.Multiline; }
            set { textBox1.Multiline = value; }
        }
        [Category("KLC TextBox Advance")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {


                base.BackColor = value;
                textBox1.BackColor = value;

            }
        }
        [Category("KLC TextBox Advance")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }
        [Category("KLC TextBox Advance")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                base.Font = value;
                textBox1.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }
        [Category("KLC TextBox Advance")]
        public string KLCText
        {
            get
            {
                if (isPlaceholder) return "";
                else return textBox1.Text;
            }

            set
            {

                textBox1.Text = value;
                SetPlaceHolder();
            }
        }
        [Category("KLC TextBox Advance")]
        public Color KLCBorderFocusColor
        {
            get
            {
                return borderFocusColor;
            }

            set
            {
                borderFocusColor = value;
            }
        }


        [Category("KLC TextBox Advance")]
        public int KLCBorderRadius
        {
            get
            {
                return borderRadius;
            }

            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                  //  BackColor = this.Parent.BackColor; // User control can't paint background correctly due to radius border
                    this.Invalidate(); // Redraw control
                }
            }
        }
        [Category("KLC TextBox Advance")]
        public Color KLCPlaceholderColor
        {
            get
            {
                return placeholderColor;
            }

            set
            {
                placeholderColor = value;
                if (isPlaceholder)
                    textBox1.ForeColor = value;
            }
        }

        [Category("KLC TextBox Advance")]
        public string KLCPlaceholderText
        {
            get
            {
                return placeholderText;
            }

            set
            {
                placeholderText = value;
                textBox1.Text = "";

                SetPlaceHolder();
            }
        }

        private void SetPlaceHolder()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && placeholderText != "")
            {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = KLCPlaceholderColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = false;
            }

        }

        private void RemovePlaceHolder()
        {
            if (isPlaceholder && placeholderText != "")
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = this.ForeColor;
                if (isPasswordChar)
                    textBox1.UseSystemPasswordChar = true;
            }
        }

        public void Clear()
        {                
            textBox1.Text = string.Empty;
           
        }
        

        //Overriden methods

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            if (borderRadius > 1) // Rounded Textbox
            {
                //-Fields
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;
                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    //-Drawing
                    this.Region = new Region(pathBorderSmooth); // Set the rounded region of UserControl
                    if (borderRadius > 15) SetTextBoxRoundedRegion(); // Set the rounded region of TextBox component
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                    if (isFocused)
                        penBorder.Color = borderFocusColor;
                    if (underlinedStyle)
                    {
                        // Draw border smooting
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);

                    }
                    else
                    {
                        // Draw border smooting
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }


                }
            }
            else // Square/Normal TextBox
            {
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                    if (isFocused)
                        penBorder.Color = borderFocusColor;
                    if (underlinedStyle)
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
            }

        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathText;
            if (KLCMultiline)
            {
                pathText = GetFigurePath(textBox1.ClientRectangle, borderRadius - borderSize);
                textBox1.Region = new Region(pathText);
            }
            else
            {
                pathText = GetFigurePath(textBox1.ClientRectangle, borderSize * 2);
                textBox1.Region = new Region(pathText);
            }
        }

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
        // Private methods
        private void UpdateControlHeight()
        {
            if (textBox1.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceHolder();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceHolder();
        }
    }
}
