using Mochou.Forms.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mochou.Forms
{
    public class RadioUtils
    {
        /// <summary>
        /// 清除单选框选择
        /// </summary>
        /// <param name="control"></param>
        public static void ClearRadio(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton r = (RadioButton)c;
                    r.Text = null;
                    r.Tag = null;
                    r.Visible = false;
                }
            }
        }

        /// <summary>
        /// 获取选择的单选框
        /// </summary>
        /// <param name="control"></param>
        public static RadioButton GetSelectRadio(Control control)
        {
            List<RadioButton> ls = WFQ.Find<RadioButton>(control, (con)=>((RadioButton)control).Checked);
            if (ls == null || ls.Count == 0) {
                return null;
            }
            return ls[0];
        }

        public static void SelectNextRadioButton(Control control) {
            Control parent = control;
            if (control is RadioButton) {
                parent = control.Parent;
            }

            List<RadioButton> ls = WFQ.Find<RadioButton>(control, (con) => true);
            if (ls == null || ls.Count == 0)
            {
                return;
            }
            if (ls.Count == 1)
            {
                return;
            }
            //第一个是选择的  如果第一个没有选择，就直接选择第一个
            ls.Sort((obj1, obj2) => obj1.TabIndex.CompareTo(obj2.TabIndex));

        
            for (var i = 0; i < ls.Count; i++)
            {
                if (ls[i].Checked) 
                {
                    ls[i].Checked = false;
                    if (i == ls.Count - 1)
                    {
                        ls[0].Checked = true;
                    }
                    else {
                        ls[i + 1].Checked = true;
                    }
                    return;
                }
            }
        }
    }
}
