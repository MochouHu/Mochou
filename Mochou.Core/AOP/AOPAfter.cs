using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochou.Core.AOP
{
    public abstract class AOPAfter : AOPInterceptor
    {
        public abstract void Call(MethodInfo methodInfo, object[] args, object returnVal);
        public override void After(MethodInfo methodInfo, object[] args, object returnVal)
        {
            Call(methodInfo, args, returnVal);
        }

        public override void Before(MethodInfo methodInfo, object[] args)
        {
        }

        public override bool Catch(MethodInfo methodInfo, object[] args, Exception err)
        {
            return false;
        }

        public override void Finally(MethodInfo methodInfo, object[] args)
        {
        }
    }
}
