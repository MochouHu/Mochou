using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mochou.Core.AOP
{
    public abstract class AOPCatch: AOPInterceptor
    {
        public abstract bool Call(MethodInfo methodInfo, object[] args, Exception err);
        public override void After(MethodInfo methodInfo, object[] args, object returnVal)
        {
            
        }

        public override void Before(MethodInfo methodInfo, object[] args)
        {
        }

        public override bool Catch(MethodInfo methodInfo, object[] args, Exception err)
        {
            return Call(methodInfo, args, err);
        }

        public override void Finally(MethodInfo methodInfo, object[] args)
        {
        }
    }
}
