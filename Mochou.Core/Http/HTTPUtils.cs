using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Mochou.Core.Http.Interceptor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mochou.Core.Http
{
    public class HTTPUtils
    {
        public static string Post(String url, object data, HttpPara para = null)
        {
            para = para == null ? new HttpPara() : para;

            if (!HttpInterceptor.Invoke.Post(url, data, para)) return "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = Create(url, data, "POST");
                para.HandRequest?.Invoke(request);
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader srReader = new StreamReader(response.GetResponseStream(), para.Encoding))
                {
                    return srReader.ReadToEnd();
                }
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Close();
            }
        }
        public static O Post<O>(String url, object data, HttpPara para = null) where O : new()
        {
            var httpRes = Post(url, data, para);
            if (string.IsNullOrWhiteSpace(httpRes)) return default(O);
            return JsonConvert.DeserializeObject<O>(httpRes);
        }
        public static string Get(String url, object data, HttpPara para = null)
        {
            para = para == null ? new HttpPara() : para;

            if (!HttpInterceptor.Invoke.Get(url, data, para)) return "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                Create(url, data, "GET");
                para.HandRequest?.Invoke(request);
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader srReader = new StreamReader(response.GetResponseStream(), para.Encoding))
                {
                    return srReader.ReadToEnd();
                }
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Close();
            }
        }
        public static O Get<O>(String url, object data, HttpPara para = null) where O : new()
        {
            var httpRes = Get(url, data, para);
            if (string.IsNullOrWhiteSpace(httpRes)) return default(O);
            return JsonConvert.DeserializeObject<O>(httpRes);
        }

        public static Cookie GetCookie(CookieContainer cc, Uri uri, String path, String cookieName)
        {

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (var pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies)
                        if (c.Domain.Equals(uri.Host) && path.Equals(c.Path) && cookieName.Equals(c.Name))
                        {
                            return c;
                        }
            }
            return null;
        }
        public static String GetResponseStreamData(HttpWebResponse httpWebResponse)
        {
            return GetResponseStreamData(httpWebResponse, HttpSetting.DefaultEncoding);
        }
        public static String GetResponseStreamData(HttpWebResponse httpWebResponse, Encoding encoding)
        {
            using (Stream stream = httpWebResponse.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(stream, encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static HttpWebRequest Create(String url, Object data, String Method = "GET")
        {

            if (!HttpInterceptor.Invoke.Create(url, data, Method)) return null;

            if (Method.ToLower().Equals("get") && data != null)
            {
                url = url + "?" + GetURLPara(data);
            }
            HttpWebRequest req = null;
            req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = Method;
            if (!Method.ToLower().Equals("get") && data != null)
            {
                SetRequestData(req, data);
            }
            return req;
        }
        public static void SetRequestData(HttpWebRequest request, Object data)
        {
            if (!HttpInterceptor.Invoke.SetRequestData(request, data)) return;

            byte[] oneData = HttpSetting.DefaultEncoding.GetBytes(GetURLPara(data));
            using (Stream newMyStream = request.GetRequestStream())
            {
                newMyStream.Write(oneData, 0, oneData.Length);
            }
        }

        public static string GetURLPara(object o)
        {
            if (o == null) return null;
            if (o is string) return o as string;

            IDictionary<string, object> data = null;
            if (o is IDictionary<string, object>)
            {
                data = o as IDictionary<string, object>;
            }
         
            data = JsonConvert.DeserializeObject<Dictionary<String, Object>>(JsonConvert.SerializeObject(o));
            
            if (!HttpInterceptor.Invoke.GetURLPara(data)) return "";

            StringBuilder post = new StringBuilder();
            foreach (String key in data.Keys)
            {
                Object val = data[key];
                if (val == null)
                {
                    val = "";
                }
                post.Append(key).Append("=").Append(HttpUtility.UrlEncode(T.ToString(val))).Append("&");
            }
            if (post.Length > 0)
            {
                post.Remove(post.Length - 1, 1);
            }
            return post.ToString();
        }
    }
    public class HU : HTTPUtils { }
}