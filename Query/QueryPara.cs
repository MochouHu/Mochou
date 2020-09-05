using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mochou.Forms.Query
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryPara
    {
        /// <summary>
        /// 查找控件时的自定义查找
        /// </summary>
        /// <param name="control1"></param>
        /// <param name="control2"></param>
        /// <returns>是否符合查找需求</returns>
        public delegate bool QueryDelegat(Control control1);
    }
}
