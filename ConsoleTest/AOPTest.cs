using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mochou.Core.AOP;

namespace ConsoleTest
{
    class AOPTest
    {
        public static void Test(string[] args)
        {
            RealClass proxy = AOPBuider.Buider<RealClass>();
            proxy.Test();
            proxy.Test2();

            proxy.AAA = "aaa";
            Console.WriteLine("调用AAA:" + proxy.AAA);
            proxy.BBB = "bbb";
            Console.WriteLine("调用BBB:" + proxy.BBB);

            Console.ReadKey();

            RealClass proxy2 = AOPBuider.Buider<RealClass>();
            proxy2.Test();

            RealClass2 proxy21 = AOPBuider.Buider<RealClass2>();
            proxy2.Test();
            Console.ReadKey();
        }

    }

    [AOP(typeof(CustAOP))]
    public class RealClass2
    {
        public virtual bool Test()
        {
            return false;
        }
    }

    [AOP(typeof(CustAOP))]
    public class RealClass
    {
        public virtual String AAA { get; set; }

        public String BBB;

        public RealClass() { }

        //必须是虚方法

        public virtual bool Test()
        {
            return false;
        }
        public virtual void Test2()
        {
            Console.WriteLine("调用Test2");
            throw new Exception();
        }
    }
    class CustAOP : AOPInterceptor
    {
        public static int count = 0;
        public CustAOP() {
            Console.WriteLine($"CustAOP创建：{++count}次！");
        }

        public override void After(MethodInfo methodInfo, object[] args, object returnVal)
        {
            Console.WriteLine("结束CustAOP拦截器方法：" + methodInfo.Name + "返回值：" + returnVal);
        }
        public override void Before(MethodInfo methodInfo, object[] args)
        {
            Console.WriteLine("进入CustAOP拦截器方法：" + methodInfo.Name);
        }
        public override bool Catch(MethodInfo methodInfo, object[] args, Exception err)
        {
            Console.WriteLine("CustAOP出现异常方法：" + methodInfo.Name);
            return true;
        }
        public override void Finally(MethodInfo methodInfo, object[] args)
        {
            Console.WriteLine("CustAOP Finally：" + methodInfo.Name);
        }
    }
}
