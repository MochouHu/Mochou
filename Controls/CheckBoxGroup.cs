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
    /// 专门用于放置CheckBox的GroupBox，在FormUtils.FormToObject中将特殊处理
    /// 将生成一个字符串数组
    /// </summary>
    public partial class CheckBoxGroup : GroupBox
    {
        public CheckBoxGroup()
        {
            InitializeComponent();
        }
    }
}
