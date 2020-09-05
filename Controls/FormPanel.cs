using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mochou.Forms.Controls
{
    /// <summary>
    /// 表单面板
    /// </summary>
    public partial class FormPanel : Panel
    {
        public FormPanel()
        {
            InitializeComponent();
        }



        public O Parse<O>() where O : new()
        {
            return FormUtils.FormToObject<O>(this);
        }
        public dynamic Parse() 
        {
            return FormUtils.FormToObject(this);
        }
    }
}
