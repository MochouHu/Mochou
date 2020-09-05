using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp.Internals;
using Mochou.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mochou.HClient.Browser.JsContent
{
    public class JavascriptContent
    {
        /// <summary>
        /// 可在js中调用的net对象
        /// </summary>
        internal Dictionary<string, Object> NettojsMap = new Dictionary<string, object>();

        public NettojsResponse Invoke(string objName, string methodName, string paras) {
            if (!NettojsMap.ContainsKey(objName))
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_OBJECT, Message = "对象不存在" };

            var obj = NettojsMap[objName];
            var objMethod = obj.GetType().GetMethod(methodName);

            if (objMethod == null)
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_METHOD, Message = "方法不存在" };

            try {
                //组装参数
                var methodParas = objMethod.GetParameters();
                object[] args = new object[methodParas.Length];

                JArray paraArr = JsonConvert.DeserializeObject<JArray>(paras);

                for (int i = 0; i < methodParas.Length; i++) {
     
                    args[i] = paraArr[i].ToObject(methodParas[i].ParameterType);
                    /*
                    var paraType = methodParas[i].ParameterType;
                    if (typeof(int).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<int>();
                    if (typeof(char).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<char>();
                    else if (typeof(long).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<long>();
                    else if (typeof(short).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<short>();
                    else if (typeof(byte).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<byte>();
                    else if (typeof(float).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<float>();
                    else if (typeof(double).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<double>();
                    else if (typeof(String).IsAssignableFrom(paraType))
                        args[i] = paraArr[i].Value<String>();
                    else
                        args[i] = paraArr[i].ToObject(paraType);
                    */
                }

                var value = objMethod.Invoke(obj, args);
                return new NettojsResponse() { Code = NettojsResponse.SUCCESS, Data = value };
            } catch (Exception e) {
                //可能会打印日志
                Console.WriteLine(e.ToString());
                return new NettojsResponse() { Code = NettojsResponse.EXCEPTION, Message = e.ToString() };
            }
        }

        public NettojsResponse Set(string objName, string propertyName, string value)
        {
            if (!NettojsMap.ContainsKey(objName))
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_OBJECT, Message = "对象不存在" };

            var obj = NettojsMap[objName];
            var objProperty = obj.GetType().GetProperty(propertyName);

            if (objProperty == null)
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_PROPERTY, Message = "属性不存在" };

            try
            {
                var val = JsonConvert.DeserializeObject(value, objProperty.PropertyType);

                objProperty.SetValue(obj, val, null);
                return new NettojsResponse() { Code = NettojsResponse.SUCCESS };
            }
            catch (Exception e)
            {
                //可能会打印日志
                Console.WriteLine(e.ToString());
                return new NettojsResponse() { Code = NettojsResponse.EXCEPTION, Message = e.ToString() };
            }
        }
        public NettojsResponse Get(string objName, string propertyName)
        {
            if (!NettojsMap.ContainsKey(objName))
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_OBJECT, Message = "对象不存在" };

            var obj = NettojsMap[objName];
            var objProperty = obj.GetType().GetProperty(propertyName);

            if (objProperty == null)
                return new NettojsResponse() { Code = NettojsResponse.NOTFOUND_PROPERTY, Message = "属性不存在" };

            try
            {
                var value = objProperty.GetValue(obj, null);
                return new NettojsResponse() { Code = NettojsResponse.SUCCESS, Data = value };
            }
            catch (Exception e)
            {
                //可能会打印日志
                Console.WriteLine(e.ToString());
                return new NettojsResponse() { Code = NettojsResponse.EXCEPTION, Message = e.ToString() };
            }
        }
    }
}
