using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Internals;
using Mochou.Core;
using Newtonsoft.Json;

namespace Mochou.HClient.Browser.JsContent
{
    /// <summary>
    /// 注册.net对象到js中
    /// </summary>
    public class NettojsController
    {

        /// <summary>
        /// 查找电脑信息
        /// </summary>
        /// <param name="javascriptCallback"></param>
        public NettojsResponse RegisterNettojs(string jsonNettojsParameters)
        {
            List<NettojsParameter> nettojsParameters = JsonConvert.DeserializeObject<List<NettojsParameter>>(jsonNettojsParameters);

            try {
                nettojsParameters.ForEach(nettojsParameter =>
                {
                    if (nettojsParameter.Parameters != null && nettojsParameter.Parameters.Count > 0)
                        SingletonContainer.Get<JavascriptContent>().NettojsMap.Add(nettojsParameter.ShortName,
                            nettojsParameter.Parameters.ToObject(Type.GetType(nettojsParameter.FullName)));
                    else
                        SingletonContainer.Get<JavascriptContent>().NettojsMap.Add(nettojsParameter.ShortName,
                            Activator.CreateInstance(Type.GetType(nettojsParameter.FullName)));
                });
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return new NettojsResponse() { Code = NettojsResponse.EXCEPTION, Message = e.ToString() };
            }

            return new NettojsResponse();
        }


        public NettojsResponse InvokeNettojs(string objName, string methodName, string json)
        {
            return SingletonContainer.Get<JavascriptContent>().Invoke(objName, methodName, json);
        }

        public NettojsResponse GetProperty(string objName, string propertyName)
        {
            return SingletonContainer.Get<JavascriptContent>().Get(objName, propertyName);
        }
        public NettojsResponse SetProperty(string objName, string propertyName, string json)
        {
            return SingletonContainer.Get<JavascriptContent>().Set(objName, propertyName, json);
        }
    }
}
