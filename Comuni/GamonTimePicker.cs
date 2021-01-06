using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Comuni
{
    public class GamonTimePicker : System.Windows.Forms.DateTimePicker
    {
        private Color _disabled_back_color;
        private Image _image;
        private Color _text_color = Color.Black;

        public GamonTimePicker() : base()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            _disabled_back_color = Color.FromKnownColor(KnownColor.Control);
        }

        /// <summary>
        ///     Gets or sets the background color of the control
        /// </summary>
        [Browsable(true)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        ///     Gets or sets the background color of the control when disabled
        /// </summary>
        [Category("Appearance")]
        [Description("The background color of the component when disabled")]
        [Browsable(true)]
        public Color BackDisabledColor
        {
            get
            {
                return _disabled_back_color;
            }
            set
            {
                _disabled_back_color = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Image next to the dropdownbutton
        /// </summary>
        [Category("Appearance")]
        [Description("Get or Set the small Image next to the dropdownbutton")]
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text color when calendar is not visible
        /// </summary>
        [Category("Appearance")]
        public Color TextColor
        {
            get
            {
                return _text_color;
            }
            set
            {
                _text_color = value;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Dropdownbutton rectangle
            Rectangle ddb_rect = new Rectangle(ClientRectangle.Width - 17, 0, 17, ClientRectangle.Height);
            // Background brush
            Brush bb;

            ComboBoxState visual_state;

            // When enabled the brush is set to Backcolor, 
            // otherwise to color stored in _disabled_back_Color
            if (this.Enabled)
            {
                bb = new SolidBrush(this.BackColor);
                visual_state = ComboBoxState.Normal;
            }
            else
            {
                bb = new SolidBrush(this._disabled_back_color);
                visual_state = ComboBoxState.Disabled;
            }

            // Filling the background
            g.FillRectangle(bb, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            // Drawing the datetime text
            g.DrawString(this.Text, this.Font, new SolidBrush(TextColor), 5, 2);

            // Drawing icon
            if (_image != null)
            {
                Rectangle im_rect = new Rectangle(ClientRectangle.Width - 40, 4, ClientRectangle.Height - 8, ClientRectangle.Height - 8);
                g.DrawImage(_image, im_rect);
            }

            // Drawing the dropdownbutton using ComboBoxRenderer
            ComboBoxRenderer.DrawDropDownButton(g, ddb_rect, visual_state);

            g.Dispose();
            bb.Dispose();
        }
    }
}
