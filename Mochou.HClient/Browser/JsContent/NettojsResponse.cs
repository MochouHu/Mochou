using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp.Internals;

namespace Mochou.HClient.Browser.JsContent
{
    public class NettojsResponse
    {
        public const string SUCCESS = "success";
        public const string NOTFOUND_OBJECT = "notfound_object";
        public const string NOTFOUND_METHOD = "notfound_method";
        public const string NOTFOUND_PROPERTY = "notfound_property";
        public const string EXCEPTION = "net_exception";

        public string Code { get; set; } = SUCCESS;
        public string Message { get; set; } = "";
        public object Data { get; set; } = null;
    }
}
