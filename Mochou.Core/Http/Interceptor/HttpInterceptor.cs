using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Mochou.Core.Http.Interceptor
{
    public static class HttpInterceptor
    {
        private static Dictionary<Type, PostInterceptor> Posts = new Dictionary<Type, PostInterceptor>();
        private static Dictionary<Type, GetInterceptor> Gets = new Dictionary<Type, GetInterceptor>();
        private static Dictionary<Type, CreateInterceptor> Creates = new Dictionary<Type, CreateInterceptor>();
        private static Dictionary<Type, SetRequestDataInterceptor> SetRequestDatas = new Dictionary<Type, SetRequestDataInterceptor>();
        private static Dictionary<Type, GetURLParaInterceptor> GetURLParas = new Dictionary<Type, GetURLParaInterceptor>();

        public static class Invoke{
            public static bool Post(String url, object data, HttpPara para)
            {
                foreach (var interceptor in Posts) 
                    if (!T.ToBoolean(interceptor.Value.Invoke(url, data, para)))
                        return false;
                return true;
            }
            public static bool Get(String url, object data, HttpPara para)
            {
                foreach (var interceptor in Gets)
                    if (!T.ToBoolean(interceptor.Value.Invoke(url, data, para)))
                        return false;
                return true;
            }
            //String url, Object data, String Method

            // HttpWebRequest request, Object data

            // object o
            public static bool Create(String url, Object data, String Method)
            {
                foreach (var interceptor in Creates)
                    if (!T.ToBoolean(interceptor.Value.Invoke(url, data, Method)))
                        return false;
                return true;
            }
            public static bool SetRequestData(HttpWebRequest request, Object data)
            {
                foreach (var interceptor in SetRequestDatas)
                    if (!T.ToBoolean(interceptor.Value.Invoke(request, data)))
                        return false;
                return true;
            }
            public static bool GetURLPara(IDictionary<string, object> o)
            {
                foreach (var interceptor in GetURLParas)
                    if (!T.ToBoolean(interceptor.Value.Invoke(o)))
                        return false;
                return true;
            }
        }

        public static class Add {
            public static void Post(PostInterceptor interceptor)
            {
                Type type = interceptor.GetType();
                if (Posts.ContainsKey(type))
                    return;
                Posts.Add(type, interceptor);
            }
            public static void Get(GetInterceptor interceptor)
            {
                Type type = interceptor.GetType();
                if (Gets.ContainsKey(type))
                    return;
                Gets.Add(type, interceptor);
            }
            public static void Create(CreateInterceptor interceptor)
            {
                Type type = interceptor.GetType();
                if (Creates.ContainsKey(type))
                    return;
                Creates.Add(type, interceptor);
            }
            public static void SetRequestData(SetRequestDataInterceptor interceptor)
            {
                Type type = interceptor.GetType();
                if (SetRequestDatas.ContainsKey(type))
                    return;
                SetRequestDatas.Add(type, interceptor);
            }
            public static void GetURLPara(GetURLParaInterceptor interceptor)
            {
                Type type = interceptor.GetType();
                if (GetURLParas.ContainsKey(type))
                    return;
                GetURLParas.Add(type, interceptor);
            }
        }

        public delegate bool PostInterceptor(String url, object data, HttpPara para);
        public delegate bool GetInterceptor(String url, object data, HttpPara para);
        public delegate bool CreateInterceptor(String url, Object data, String Method);
        public delegate bool SetRequestDataInterceptor(HttpWebRequest request, Object data);
        public delegate bool GetURLParaInterceptor(IDictionary<string, object> o);
    }
}
