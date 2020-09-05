namespace Mochou.Forms.Controls
{
    partial class TitleTextBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lalText = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lalText
            // 
            this.lalText.Dock = System.Windows.Forms.DockStyle.Left;
            this.lalText.Location = new System.Drawing.Point(0, 0);
            this.lalText.Name = "lalText";
            this.lalText.Size = new System.Drawing.Size(62, 28);
            this.lalText.TabIndex = 0;
            this.lalText.Text = "label1";
            this.lalText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lalText.SizeChanged += new System.EventHandler(this.label1_SizeChanged);
            this.lalText.TextChanged += new System.EventHandler(this.label1_TextChanged);
            this.lalText.Click += new System.EventHandler(this.label1_Click);
            this.lalText.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(57, 2);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(243, 25);
            this.txtValue.TabIndex = 1;
            this.txtValue.Click += new System.EventHandler(this.textBox1_Click);
            this.txtValue.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            this.txtValue.SizeChanged += new System.EventHandler(this.textBox1_SizeChanged);
            this.txtValue.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtValue.Enter += new System.EventHandler(this.textBox1_Enter);
            this.txtValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.txtValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.txtValue.Layout += new System.Windows.Forms.LayoutEventHandler(this.textBox1_Layout);
            this.txtValue.Leave += new System.EventHandler(this.textBox1_Leave);
            this.txtValue.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDoubleClick);
            this.txtValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.txtValue.MouseEnter += new System.EventHandler(this.textBox1_MouseEnter);
            this.txtValue.MouseLeave += new System.EventHandler(this.textBox1_MouseLeave);
            this.txtValue.MouseHover += new System.EventHandler(this.textBox1_MouseHover);
            this.txtValue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseMove);
            this.txtValue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // TitleTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lalText);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.Name = "TitleTextBox";
            this.Size = new System.Drawing.Size(300, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lalText;
        private System.Windows.Forms.TextBox txtValue;
    }
}
