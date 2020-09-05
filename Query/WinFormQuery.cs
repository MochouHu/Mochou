using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Mochou.Forms.Query
{
    /// <summary>
    /// 用于WinForm的查找
    /// </summary>
    public class WinFormQuery
    {
        /// <summary>
        /// 查找控件时的自定义查找
        /// </summary>
        /// <param name="control1"></param>
        /// <param name="control2"></param>
        /// <returns>是否符合查找需求</returns>
        public delegate bool QueryDelegat(Control control);

        public static List<C> ByName<C>(Control control, String name) where C : Control
        {
            List<C> ls = new List<C>();
            foreach (Control control1 in control.Controls.Find(name, true))
            {
                if (control1 is C)
                {
                    ls.Add((C)control1);
                }
                ls.AddRange(ByName<C>(control1, name));
            }
            return ls;
        }

        public static List<C> Find<C>(Control control, params QueryDelegat[] queryPara) where C : Control
        {
            return Find<C>(control, QueryMode.And, true, queryPara);
        }
        public static List<C> Find<C>(Control control, QueryMode queryMode, bool searchAllChildren, params QueryDelegat[] queryPara) where C : Control
        {
            List<C> ls = new List<C>();

            if (queryPara == null || queryPara.Length == 0) {
                return ls;
            }

            foreach (Control control1 in control.Controls)
            {
                if (!(control1 is C))
                {
                    continue;
                }
                bool res = queryPara[0](control1);
                for (int i = 1; i < queryPara.Length; i++)
                {
                    if (queryMode == QueryMode.And)
                    {
                        res &= queryPara[i](control1);
                    }
                    else {
                        res |= queryPara[i](control1);
                    }
                }
                if (res) {
                    ls.Add((C)control1);
                }
                //查找子控件
                if (searchAllChildren) {
                    ls.AddRange(Find<C>(control1, queryMode, searchAllChildren, queryPara));
                }
            }

            return ls;
        }

      
    }
    public class WFQ : WinFormQuery { }
}
