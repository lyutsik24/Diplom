using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom
{
    class TextBoxWithPlaceholder : TextBox
    {
        private string PlaceholderTextDefault = string.Empty;
        private Color PlaceholderColorDefault = Color.Gray;

        public TextBoxWithPlaceholder()
        {
            this.BackColor = Color.FromArgb(74, 79, 99);
            this.BorderStyle = BorderStyle.None;
            this.Font = new Font("Comic Sans MS", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        [Category("Appearance")]
        public string PlaceholderText { get => PlaceholderTextDefault; set => PlaceholderTextDefault = value; }
        [Category("Appearance")]
        public Color PlaceholderColor { get => PlaceholderColorDefault; set => PlaceholderColorDefault = value; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!DesignMode)
            {
                this.Text = PlaceholderText;
                this.ForeColor = PlaceholderColor;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (string.IsNullOrWhiteSpace(Text))
            {
                this.Text = PlaceholderText;
                this.ForeColor = PlaceholderColor;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (Text.Equals(PlaceholderText))
            {
                this.Text = string.Empty;
                this.ForeColor = Color.FromArgb(200, 200, 200);
            }
        }
    }
}