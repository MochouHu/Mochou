using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochou.Core.AOP
{
    public class AOPAttribute : Attribute
    {
        public AOPInterceptor[] Interceptors;


        public AOPAttribute(params Type[] InterceptorTypes)
        {
            if (InterceptorTypes is null)
            {
                throw new ArgumentNullException(nameof(InterceptorTypes));
            }

            var Interceptors = new List<AOPInterceptor>();
            foreach (var InterceptorType in InterceptorTypes)
            {
                if (!typeof(AOPInterceptor).IsAssignableFrom(InterceptorType)) throw new AOPException($"{InterceptorType.FullName}is not an AOP type");

                AOPInterceptor interceptor = SingletonContainer.Get(InterceptorType.FullName,
                    () => (AOPInterceptor)Activator.CreateInstance(InterceptorType));

                Interceptors.Add(interceptor);
                //Interceptors.Add((AOPInterceptor)Activator.CreateInstance(InterceptorType));
            }

            this.Interceptors = Interceptors.ToArray();
        }


        public static bool IsAOPAttribute(MethodInfo methodInfo) {
            var type = methodInfo.DeclaringType;
            var methodName = methodInfo.Name;

            if (Attribute.IsDefined(type, typeof(AOPAttribute)))
            {
                return true;
            }

            if (Attribute.IsDefined(methodInfo, typeof(AOPAttribute)))
            {
                return true;
            }

            if (methodName.StartsWith("get_") || methodName.StartsWith("set_")) {
                var baseProperty = type.GetProperty(methodName.Substring(4));
                if (Attribute.IsDefined(baseProperty, typeof(AOPAttribute)))
                {
                    return true;
                }
            }

            return false;
        }

        public static AOPAttribute GetAOPAttribute(MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;
            var methodName = methodInfo.Name;
            AOPAttribute aopAttribute = (AOPAttribute)Attribute.GetCustomAttribute(type, typeof(AOPAttribute));
            if (aopAttribute == null)
            {
                if (methodName.StartsWith("get_") || methodName.StartsWith("set_"))
                {
                    //获取属性的注解
                    var baseProperty = type.GetProperty(methodName.Substring(4));
                    if (baseProperty != null)
                    {
                        aopAttribute = (AOPAttribute)Attribute.GetCustomAttribute(baseProperty, typeof(AOPAttribute));
                    }
                }
                else
                {
                    //获取方法的注解
                    aopAttribute = (AOPAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(AOPAttribute));
                }
            }
            return aopAttribute;
        }

        internal object Invoke(string methodName, MulticastDelegate methodDelegate, object[] args)
        {
            var baseType = methodDelegate.Method.DeclaringType.BaseType;
            var baseMethod = baseType.GetMethod(methodName);
            try
            {
                foreach (var Interceptor in Interceptors)
                    Interceptor.Before(baseMethod, args);

                var obj = methodDelegate.Method.Invoke(methodDelegate.Target, args);

                foreach (var Interceptor in Interceptors)
                    Interceptor.After(baseMethod, args, obj);

                return obj;
            }
            catch (Exception e)
            {
                bool ishand = false;
                foreach (var Interceptor in Interceptors)
                    ishand = ishand || Interceptor.Catch(baseMethod, args, e);

                if (ishand) {
                    if (baseMethod.ReturnType.Equals(typeof(void))) //无返回值时返回null
                        return null;
                    return baseMethod.ReturnType.IsValueType ? Activator.CreateInstance(baseMethod.ReturnType) : null;
                }
                throw new AOPException(e.Message, e);
            }
            finally
            {
                foreach (var Interceptor in Interceptors)
                    Interceptor.Finally(baseMethod, args);
            }
        }
    }
}
