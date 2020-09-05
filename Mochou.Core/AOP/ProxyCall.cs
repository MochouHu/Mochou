using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mochou.Core.AOP
{
    public class ProxyCall
    {
        public Object Call(String methodName, MulticastDelegate methodDelegate, params Object[] args)
        {
            var baseType = methodDelegate.Method.DeclaringType.BaseType;
            var baseMethod = baseType.GetMethod(methodName);
            //获取类上的注解
            AOPAttribute aopAttribute = AOPAttribute.GetAOPAttribute(baseMethod);

            if (aopAttribute != null) {
                return aopAttribute.Invoke(methodName, methodDelegate, args);
            }
            
            return methodDelegate.Method.Invoke(methodDelegate.Target, args);
        }
    }
}
