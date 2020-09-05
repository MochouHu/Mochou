using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Mochou.Core.Http
{
    /// <summary>
    /// 请求时用到的参数
    /// </summary>
    public class HttpPara
    {
        public Encoding Encoding { get; set; } = HttpSetting.DefaultEncoding;
        public Action<HttpWebRequest> HandRequest { get; set; } = request =>
        {
            request.ContentType = "application/x-www-form-urlencoded";
        };
    }
}
