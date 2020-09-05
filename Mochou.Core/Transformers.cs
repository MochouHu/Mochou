using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mochou.Core
{
    /// <summary>
    /// 用于各种类型转换
    /// </summary>
    public class Transformers
    {
        public static String ToString(Object o, String def = default(string)) {
            if (o == null) {
                return def;
            }
            if (o is String) {
                return o as String;
            }
            return o.ToString();
        }
        public static bool ToBoolean(object o, bool def = default(bool)) {
            if (o == null) {
                return def;
            }
            if (o is bool)
            {
                return (bool)o;
            }
            if (o is Boolean)
            {
                return (Boolean)o;
            }

            var strO = ToString(o);
            bool res = false;
            if (bool.TryParse(strO, out res)) {
                return res;
            }
            return 0 != ToInt(o);
        }

        public static int ToInt(object o, int def = default(int))
        {
            if (o == null)
            {
                return def;
            }
            if (IsBaseType(o.GetType(), false))
            {
                if (o is bool)
                {
                    return ((bool)o) ? 1 : 0;
                }
                else
                {
                    return (int)o;
                }
            }
            int io = 0;
            if (int.TryParse(ToString(o), out io))
            {
                return io;
            }
            return def;
        }
        public static long ToLong(object o, long def = default(long))
        {
            if (o == null)
            {
                return def;
            }
            if (IsBaseType(o.GetType(), false))
            {
                if (o is bool)
                {
                    return ((bool)o) ? 1 : 0;
                }
                else
                {
                    return (long)o;
                }
            }
            long io = 0;
            if (long.TryParse(ToString(o), out io))
            {
                return io;
            }
            return def;
        }
    
        public static double ToDouble(object o, double def = default(double))
        {
            if (o == null)
            {
                return def;
            }
            if (IsBaseType(o.GetType(), false))
            {
                if (o is bool)
                {
                    return ((bool)o) ? 1 : 0;
                }
                else
                {
                    return (double)o;
                }
            }
            double io = 0;
            if (double.TryParse(ToString(o), out io))
            {
                return io;
            }
            return def;
        }
        public static bool IsBaseType(Type className, bool incString = false)
        {
            if (incString && className.Equals(typeof(String))) {
                return true;
            }

            if (className.IsPrimitive) {
                return true;
            }
            return className.Equals(typeof(Int16)) ||
                    className.Equals(typeof(Int32)) ||
                    className.Equals(typeof(Int64)) ||
                    className.Equals(typeof(UInt16)) ||
                    className.Equals(typeof(UInt32)) ||
                    className.Equals(typeof(UInt64)) ||
                    className.Equals(typeof(int)) ||
                    className.Equals(typeof(Byte)) ||
                    className.Equals(typeof(byte)) ||
                    className.Equals(typeof(long)) ||
                    className.Equals(typeof(long)) ||
                    className.Equals(typeof(Double)) ||
                    className.Equals(typeof(double)) ||
                    className.Equals(typeof(float)) ||
                    className.Equals(typeof(float)) ||
                    className.Equals(typeof(char)) ||
                    className.Equals(typeof(short)) ||
                    className.Equals(typeof(short)) ||
                    className.Equals(typeof(Boolean)) ||
                    className.Equals(typeof(bool));
        }


        public static IDictionary<string, object> ToDictionary(object o)
        {
            if (o is IDictionary<string, object>) return o as IDictionary<string, object>;
            IDictionary<string, object> res = null;
            if (o is IDictionary<string, string>)
            {
                res = new Dictionary<string, object>();
                foreach (var it in (o as IDictionary<string, string>))
                    res.Add(it.Key, it.Value);
            }
            else
            {
                res = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(o));
            }
            return res;
        }

        /// <summary>
        /// 转为全小写Key的Dictionary
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToLowerKeyDictionary(object o)
        {
            IDictionary<string, object> res = null;
            if (o is IDictionary<string, object>) {
                res = new Dictionary<string, object>();
                foreach (var it in (o as IDictionary<string, object>))
                    res.Add(it.Key.ToLower(), it.Value);
            } 
            
            if (o is IDictionary<string, string>)
            {
                res = new Dictionary<string, object>();
                foreach (var it in (o as IDictionary<string, string>))
                    res.Add(it.Key.ToLower(), it.Value);
            }
            else
            {
                IDictionary<string, object> jsonRes = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(o));
                res = new Dictionary<string, object>();
                foreach (var it in jsonRes)
                    res.Add(it.Key.ToLower(), it.Value);
            }
            return res;
        }
    }
    /// <summary>
    /// 别名，用于快速应用
    /// </summary>
    public class T : Transformers
    {
    }
}
