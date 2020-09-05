using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Mochou.Core
{
    public class StringUtils
    {
        public static readonly String LINE = Environment.NewLine;
        /// <summary>
        /// 获取str中s和p之间的子串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static String InTheMiddle(String str, String s, String p)
        {

            int index = str.IndexOf(s);
            if (index == -1)
            {
                return null;
            }
            index += s.Length;
            int endIndex = String.IsNullOrWhiteSpace(p)?str.Length : str.IndexOf(p, index);
            String callUrl = str.Substring(index, endIndex - index);

            return callUrl;
        }
        /// <summary>
        /// 获取str中s和p之间的子串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="beg">开始查找的下标</param>
        /// <returns></returns>
        public static String InTheMiddle(String str, String s, String p, int beg)
        {
            int index = str.IndexOf(s, beg);
            if (index == -1)
            {
                return null;
            }
            index += s.Length;
            int endIndex = str.IndexOf(p, index);
            String callUrl = str.Substring(index, endIndex - index);

            return callUrl;
        }


        /// <summary>
        /// 以前后标志判断变量，替换变量值
        /// </summary>
        /// <param name="BeginLogo">变量开始表标识</param>
        /// <param name="EndLogo">变量结束标识</param>
        /// <param name="text">原字符串</param>
        /// <param name="vals">变量值的字典</param>
        /// <param name="nullVal">如果字典中没有此值用nullVal替换如果nullVal为空则保持不变</param>
        /// <param name="logFile">如果有值在此文件下输出本次替换的日志</param>
        /// <returns></returns>
        public static string getKVResult(string BeginLogo, string EndLogo, string text, object data, string nullVal = "")
        {

            if (data == null) return null;
            if (data is string) return data as string;

            IDictionary<string, object> vals = T.ToDictionary(data);

            StringBuilder result = new StringBuilder();

            int keyBi = -1;
            int keyEi = -1;

            do
            {
                keyBi = text.IndexOf(BeginLogo);
                if (keyBi >= 0)
                {
                    keyEi = text.IndexOf(EndLogo);
                    if (keyEi >= 0)
                    {
                        result.Append(text.Substring(0, keyBi));
                        string key = text.Substring(keyBi + BeginLogo.Count(), keyEi - keyBi - BeginLogo.Count());
                        if (vals.ContainsKey(key))
                        {
                            result.Append(vals[key]);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(nullVal))
                            {
                                result.Append(nullVal);
                            }
                            else
                            {
                                result.Append(BeginLogo).Append(key).Append(EndLogo);
                            }
                        }
                        text = text.Substring(keyEi + EndLogo.Count());
                        continue;
                    }
                }
                result.Append(text);
                break;
            } while (text.Count() > 0);

            return result.ToString();
        }

    }

    public class SU : StringUtils { }
}
