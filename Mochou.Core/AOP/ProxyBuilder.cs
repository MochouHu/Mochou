using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Mochou.Core.AOP
{
    
    public class ProxyBuilder
    {
        internal ProxyBuilder() { }
        //后续采用配置文件
        public static readonly string DllName = "DynamicProxy.dll";

        //缓存下生成的类型
        public static Dictionary<String, Type> ProxyTypeDic = new Dictionary<string, Type>();


        internal static object Buider(Type type)
        {
            if (!ProxyTypeDic.ContainsKey(type.FullName))
            {
                lock (ProxyTypeDic)
                {
                    if (!ProxyTypeDic.ContainsKey(type.FullName))
                    {
                        Type newType = null;
                        try
                        {
                            Type m_Type = type;
                            AppDomain domain = AppDomain.CurrentDomain;
                            AssemblyBuilder m_Assembly = domain.DefineDynamicAssembly(new AssemblyName("DynamicModule"), AssemblyBuilderAccess.RunAndSave);
                            ModuleBuilder m_Module = m_Assembly.DefineDynamicModule("Module", DllName);
                            TypeBuilder m_TypeBuilder = m_Module.DefineType(m_Type.Name + "_proxy_" + m_Type.GetHashCode().ToString(), TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, m_Type);
                            MethodInfo[] methodInfos = getALLMethod(m_Type);
                            TypeBuilder[] m_NestedTypeBuilders = new TypeBuilder[methodInfos.Length];
                            ConstructorBuilder[] m_NestedTypeConstructors = new ConstructorBuilder[methodInfos.Length];

                            FieldBuilder m_Interceptor = m_TypeBuilder.DefineField("__Interceptor", typeof(ProxyCall), FieldAttributes.Private);

                            FieldBuilder[] m_MultiCastDelegates = new FieldBuilder[methodInfos.Length];
                            MethodBuilder[] m_CallBackMethods = new MethodBuilder[methodInfos.Length];
                            ConstructorBuilder m_ConstructorBuilder = m_TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(ProxyCall) });

                            for (Int32 i = 0; i < m_NestedTypeBuilders.Length; i++)
                            {
                                m_NestedTypeBuilders[i] = m_TypeBuilder.DefineNestedType("__" + methodInfos[i].Name + "__delegate", TypeAttributes.NestedPrivate | TypeAttributes.Sealed, typeof(MulticastDelegate));
                                m_NestedTypeConstructors[i] = m_NestedTypeBuilders[i].DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(Object), typeof(IntPtr) });
                                m_NestedTypeConstructors[i].SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                                Type[] argsType = GetParameterTypes(methodInfos[i]);
                                MethodBuilder mb = m_NestedTypeBuilders[i].DefineMethod("Invoke", MethodAttributes.Public, CallingConventions.Standard, methodInfos[i].ReturnType, argsType);
                                mb.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                            }

                            for (Int32 i = 0; i < methodInfos.Length; i++)
                            {
                                m_MultiCastDelegates[i] = m_TypeBuilder.DefineField(methodInfos[i].Name + "_field", m_NestedTypeBuilders[i], FieldAttributes.Private);
                            }

                            for (Int32 i = 0; i < methodInfos.Length; i++)
                            {
                                Type[] argTypes = GetParameterTypes(methodInfos[i]);
                                m_CallBackMethods[i] = m_TypeBuilder.DefineMethod("callback_" + methodInfos[i].Name, MethodAttributes.Private, CallingConventions.Standard, methodInfos[i].ReturnType, argTypes);
                                ILGenerator ilGenerator = m_CallBackMethods[i].GetILGenerator();
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                for (Int32 j = 0; j < argTypes.Length; j++)
                                {
                                    ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                                }
                                ilGenerator.Emit(OpCodes.Call, methodInfos[i]);
                                ilGenerator.Emit(OpCodes.Ret);
                            }

                            for (Int32 i = 0; i < methodInfos.Length; i++)
                            {
                                Type[] argTypes = GetParameterTypes(methodInfos[i]);
                                MethodBuilder mb = m_TypeBuilder.DefineMethod(methodInfos[i].Name, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, methodInfos[i].ReturnType, argTypes);
                                ILGenerator ilGenerator = mb.GetILGenerator();
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, m_Interceptor);
                                ilGenerator.Emit(OpCodes.Ldstr, methodInfos[i].Name);
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, m_MultiCastDelegates[i]);
                                LocalBuilder local = ilGenerator.DeclareLocal(typeof(Object[]));
                                ilGenerator.Emit(OpCodes.Ldc_I4, argTypes.Length);
                                ilGenerator.Emit(OpCodes.Newarr, typeof(Object));
                                ilGenerator.Emit(OpCodes.Stloc, local);
                                ilGenerator.Emit(OpCodes.Ldloc, local);
                                for (Int32 j = 0; j < argTypes.Length; j++)
                                {
                                    ilGenerator.Emit(OpCodes.Ldc_I4, j);
                                    ilGenerator.Emit(OpCodes.Ldarg, j + 1);
                                    ilGenerator.Emit(OpCodes.Box, argTypes[j]);
                                    ilGenerator.Emit(OpCodes.Stelem_Ref);
                                    ilGenerator.Emit(OpCodes.Ldloc, local);
                                }
                                ilGenerator.Emit(OpCodes.Call, typeof(ProxyCall).GetMethod("Call", new Type[] { typeof(String), typeof(MulticastDelegate), typeof(Object[]) }));
                                if (methodInfos[i].ReturnType.Equals(typeof(void)))
                                {
                                    ilGenerator.Emit(OpCodes.Pop);
                                }
                                else
                                {
                                    ilGenerator.Emit(OpCodes.Unbox_Any, methodInfos[i].ReturnType);
                                }
                                ilGenerator.Emit(OpCodes.Ret);
                            }

                            ILGenerator ilGenerator2 = m_ConstructorBuilder.GetILGenerator();
                            ilGenerator2.Emit(OpCodes.Ldarg_0);
                            ilGenerator2.Emit(OpCodes.Call, m_Type.GetConstructor(new Type[] { }));
                            ilGenerator2.Emit(OpCodes.Ldarg_0);
                            ilGenerator2.Emit(OpCodes.Ldarg_1);
                            ilGenerator2.Emit(OpCodes.Stfld, m_Interceptor);
                            for (Int32 i = 0; i < m_MultiCastDelegates.Length; i++)
                            {
                                ilGenerator2.Emit(OpCodes.Ldarg_0);
                                ilGenerator2.Emit(OpCodes.Ldarg_0);
                                ilGenerator2.Emit(OpCodes.Ldftn, m_CallBackMethods[i]);
                                ilGenerator2.Emit(OpCodes.Newobj, m_NestedTypeConstructors[i]);
                                ilGenerator2.Emit(OpCodes.Stfld, m_MultiCastDelegates[i]);
                            }
                            ilGenerator2.Emit(OpCodes.Ret);

                            newType = m_TypeBuilder.CreateType();

                            foreach (TypeBuilder tb in m_NestedTypeBuilders)
                            {
                                tb.CreateType();
                            }

                            m_Assembly.Save(DllName);

                            ProxyTypeDic.Add(type.FullName, newType);
                        }
                        catch (Exception err)
                        {
                            throw new AOPException(err.Message, err);
                        }
                    }
                }
            }

            return Activator.CreateInstance(ProxyTypeDic[type.FullName], new ProxyCall());
        }

        public static T Buider<T>() where T : new()
        {
            return (T)Buider(typeof(T));
        }

        internal static Type[] GetParameterTypes(MethodInfo methodInfo)
        {
            ParameterInfo[] args = methodInfo.GetParameters();
            Type[] argsType = new Type[args.Length];
            for (Int32 j = 0; j < args.Length; j++)
            {
                argsType[j] = args[j].ParameterType;
            }
            return argsType;
        }

        public static MethodInfo[] getALLMethod(Type t)
        {
            var methods = new List<MethodInfo>();
            foreach (var method in t.GetMethods())
            {
                if (AOPAttribute.IsAOPAttribute(method)) {
                    if (!method.IsVirtual)
                    {
                        throw new AOPException($"{method.Name} is not a virtual method");
                    }
                    methods.Add(method);
                }
                //AOPAttribute aOPAttribute = AOPAttribute.GetAOPAttribute(method);
                //if (aOPAttribute != null)
                //{
                //    if (!method.IsVirtual)
                //    {
                //        throw new AOPException($"{method.Name} is not a virtual method");
                //    }
                //    methods.Add(method);
                //}
            }

            return methods.ToArray();
        }
    }

    public class AOPBuider : ProxyBuilder { private AOPBuider() { }

    }
}
