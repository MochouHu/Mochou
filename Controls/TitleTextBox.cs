using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mochou.Forms.Controls
{
    public partial class TitleTextBox : UserControl
    {
        public TitleTextBox()
        {
            InitializeComponent();
        }

        private string title;
        public string Title { set { title = value; lalText.Text = value + "  "; } get { return title; } }

        public override string Text { set { base.Text = value; txtValue.Text = value; } get { return base.Text; } }

        public event EventHandler TitleTextChanged;
        private void label1_TextChanged(object sender, EventArgs e)
        {
            Label l = sender as Label;
            SizeF sizeF = CreateGraphics().MeasureString(l.Text, l.Font);
            l.Size = new Size((int)sizeF.Width + 20, (int)l.Size.Height);
            if (TitleTextChanged != null)
            {
                TitleTextChanged(null,null);   
            }
        }

        private void label1_SizeChanged(object sender, EventArgs e)
        {
            txtValue.Location = new Point(lalText.Size.Width - 15, this.Size.Height / 2 - txtValue.Size.Height / 2);//确定新的位置
            txtValue.Size = new Size(this.Size.Width - lalText.Size.Width  + 7, txtValue.Size.Height);//确定新的大小
        }

        public new event EventHandler TextChanged;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = txtValue.Text;
            if(TextChanged != null){
                TextChanged(sender,e);
            }
        }

        public new event EventHandler SizeChanged;

        private void textBox1_SizeChanged(object sender, EventArgs e)
        {
            if (SizeChanged != null)
            {
                SizeChanged(sender, e);
            }
        }

        public new event MouseEventHandler MouseUp;

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(sender, e);
            }
        }
        public new event EventHandler MouseLeave;
        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(sender, e);
            }
        }
        public new event MouseEventHandler MouseMove;
        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(sender, e);
            }
        }
        public new event EventHandler MouseHover;
        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            if (MouseHover != null)
            {
                MouseHover(sender, e);
            }
        }
        public new event EventHandler MouseEnter;
        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(sender, e);
            }
        }
        public new event MouseEventHandler MouseDown;
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(sender, e);
            }
        }
        public new event MouseEventHandler MouseDoubleClick;
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseDoubleClick != null)
            {
                MouseDoubleClick(sender, e);
            }
        }
        public new event MouseEventHandler MouseClick;
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseClick != null)
            {
                MouseClick(sender, e);
            }
        }
        public new event EventHandler Leave;
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (Leave != null)
            {
                Leave(sender, e);
            }
        }

        public new event LayoutEventHandler Layout;
        
        private void textBox1_Layout(object sender, LayoutEventArgs e)
        {
            if (Layout != null)
            {
                Layout(sender, e);
            }
        }

        public new event KeyEventHandler KeyUp;
       
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyUp != null)
            {
                KeyUp(sender, e);
            }
        }

        public new event KeyPressEventHandler KeyPress;
       
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPress != null)
            {
                KeyPress(sender, e);
            }
        }

        public new event KeyEventHandler KeyDown;
       
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(sender, e);
            }
        }

        public new event EventHandler Enter;
       
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (Enter != null)
            {
                Enter(sender, e);
            }
        }

        public new event EventHandler Click;
       
        private void textBox1_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }
        public new event EventHandler TitleClick;
        private void label1_Click(object sender, EventArgs e)
        {
            if (TitleClick != null)
            {
                TitleClick(sender, e);
            }
        }
        public new event EventHandler TitleDoubleClick;
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            if (TitleDoubleClick != null)
            {
                TitleDoubleClick(sender, e);
            }
        }

    }
}
