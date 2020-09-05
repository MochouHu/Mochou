using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochou.Core.AOP
{
    public abstract class  AOPInterceptor
    {
        public abstract void Before(MethodInfo methodInfo, Object[] args);
        public abstract void After(MethodInfo methodInfo, Object[] args, Object returnVal);
        /// <summary>
        /// 拦截异常，如果出现异常，该方法返回值为返回类型的默认值
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="args"></param>
        /// <param name="err"></param>
        /// <returns>异常是否已处理，如果已经处理异常将返回返回类型的默认值,如果没有处理异常，则继续抛出异常</returns>
        public abstract bool Catch(MethodInfo methodInfo, Object[] args, Exception err);
        public abstract void Finally(MethodInfo methodInfo, Object[] args);
    }
}
