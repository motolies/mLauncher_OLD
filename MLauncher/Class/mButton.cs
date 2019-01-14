using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLauncher.Class
{
    class mButton : Button
    {
        private Label _label = new Label();
        public string Title { get { return _label.Text; } set { _label.Text = value; } }

        public new event EventHandler Click
        {
            add
            {
                base.Click += value;
                foreach (Control control in Controls)
                {
                    control.Click += value;
                }
            }
            remove
            {
                base.Click -= value;
                foreach (Control control in Controls)
                {
                    control.Click -= value;
                }
            }
        }
        public event EventHandler LabelClick;

       

        public mButton()
        {
            _label.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
            _label.Dock = DockStyle.Bottom;
            _label.Height = 14;
            _label.Click += new EventHandler(Label_Click);
            this.Controls.Add(this._label);
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (LabelClick != null)
                LabelClick(this, e);
        }

    

      
    }

}
