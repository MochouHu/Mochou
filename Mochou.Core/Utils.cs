using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mochou.Core
{
    public class Utils
    {
        public static dynamic ToDynamic(IDictionary<string, object> dict)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            foreach (var entry in dict)
            {
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
            }

            return result;
        }


        public static string MD5(string txt)
        {
            using (MD5 mi = System.Security.Cryptography.MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public static String[] MD5StringSet(HashSet<String> newWfk)
        {
            String[] md5s = new String[newWfk.Count];
            int index = 0;
            foreach (String str in newWfk)
            {
                md5s[index++] = Utils.MD5(str);
            }
            return md5s;
        }

        public static String UUID() { 
            return System.Guid.NewGuid().ToString(); 
        }
    }
    public class U : Utils { }

}
