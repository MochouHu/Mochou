using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochou.Core
{
    public class TypeUtils
    {
        public static TypeObject GetInstance<TypeObject>() {
            return (TypeObject)Activator.CreateInstance(typeof(TypeObject));
        }
        public static O DicToObjct<O>(IDictionary<String, Object> dic, bool ignoreCase = true) where O:new(){
            if (ignoreCase) {
                Dictionary<String, Object> dic1 = new Dictionary<string, object>();
                foreach (var itme in dic) {
                    dic1.Add(itme.Key.ToLower(), itme.Value);
                }
                dic = dic1;
            }

            Type type = typeof(O);
            var obj = new O();
            foreach (PropertyInfo propertyInfo in type.GetProperties()) {
                String attrName = ignoreCase ? propertyInfo.Name.ToLower() : propertyInfo.Name;
                if (dic.ContainsKey(attrName)) {
                    Object val = dic[attrName];
                    //不是基本类型又不是字符串
                    if (!propertyInfo.PropertyType.IsPrimitive
                        || !typeof(String).IsAssignableFrom(propertyInfo.PropertyType))
                    {

                    }
                    TrySet(obj, propertyInfo, val);
                }
            }
            return obj;
        }
        public static bool TrySet(Object obj, String attrName, Object val)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(attrName);
            return TrySet(obj, propertyInfo, val);
        }
        public static bool TrySet(Object obj, PropertyInfo propertyInfo, Object val)
        {
            if (propertyInfo == null)
            {
                return false;
            }
            Type propertyType = propertyInfo.PropertyType;
            try
            {
                if (typeof(int).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, T.ToInt(val), null);
                if (typeof(char).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, (char)T.ToInt(val), null);
                else if (typeof(long).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, T.ToLong(val), null);
                else if (typeof(short).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, (short)T.ToInt(val), null);
                else if (typeof(byte).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, (byte)T.ToInt(val), null);
                else if (typeof(float).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, (float)T.ToDouble(val), null);
                else if (typeof(double).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, T.ToDouble(val), null);
                else if (typeof(String).IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, T.ToString(val), null);
                else if (obj.GetType().IsAssignableFrom(propertyType))
                    propertyInfo.SetValue(obj, val, null);
                else return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
