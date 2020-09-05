using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Mochou.HClient.Browser.JsContent
{
    public class NettojsParameter
    {
        public string ShortName { get; set; }

        public string FullName { get; set; }

        public JObject Parameters { get; set; }
    }
}
