using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mochou.Core.Http
{
    /// <summary>
    /// Http的设置
    /// </summary>
    public class HttpSetting
    {
        //全局Http默认编码格式
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;
    }
}
