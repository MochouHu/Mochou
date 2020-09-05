using Mochou.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mochou.Forms.Extension
{
    public static class RadioButtonExtension
    {
        /// <summary>
        /// 选择下一个单选框
        /// </summary>
        /// <param name="radio"></param>
        public static void SelectNext(this RadioButton radio) => RadioUtils.SelectNextRadioButton(radio);
    }
}
