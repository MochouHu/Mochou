using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;

namespace Mochou.Core.Http
{
    /// <summary>
    /// 虚拟客户端：每一个客户端的访问都将记录下Cookie，保持状态
    /// </summary>
    public class VirtualClient
    {
        private CookieContainer cookies = new CookieContainer();

        private Object OperateLock = new Object();

        private HttpRequestHeader requestHeader;
        public HttpRequestHeader RequestHeader {
            get { return requestHeader; }
            set {
                lock (OperateLock) {
                    requestHeader = value;
                }
            }
        }

      
        public void ClearCookie() {
            cookies = new CookieContainer();
        }
        /// <summary>
        /// 还未实现 后续实现
        /// </summary>
        /// <returns></returns>
        public List<Cookie> GetAllCookies(Uri uri, String path, String cookieName)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cookies.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cookies, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }

    }
}
