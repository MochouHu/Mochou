using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mochou.Core.AOP;

namespace Mochou.Core
{
    /// <summary>
    /// 单例容器
    /// 解耦单例造成的耦合
    /// </summary>
    public class SingletonContainer
    {
        private static ConcurrentDictionary<string, Object> container = new ConcurrentDictionary<string, object>();

        public static void Put(object val)
        {
            lock (container) container[val.GetType().FullName] = val;
        }
        public static void Put(string name, object val)
        {
            lock (container) container[name] = val;
        }
        public static void Put<Type>(object val)
        {
            if (typeof(Type).IsAssignableFrom(val.GetType()))
                lock (container) container[typeof(Type).FullName] = val;
            else
                throw new FormatException("类型错误！");
        }
        public static SingletonType Get<SingletonType>() where SingletonType : new()
        {
            Type type = typeof(SingletonType);
            var key = type.FullName;
            if (!container.ContainsKey(key))
            {
                lock (container)
                    if (!container.ContainsKey(key))
                        //获取得对象经过了AOP的代理
                        container[key] = AOPBuider.Buider<SingletonType>();
            }
            return (SingletonType)container[key];
        }
        public static object Get(Type type)
        {
            var key = type.FullName;
            if (!container.ContainsKey(key))
            {
                lock (container)
                    if (!container.ContainsKey(key))
                        //获取得对象经过了AOP的代理
                        container[key] = AOPBuider.Buider(type);
            }
            return container[key];
        }
        public static object Get(string key, Type type)
        {
            if (!container.ContainsKey(key))
            {
                lock (container)
                    if (!container.ContainsKey(key))
                        //获取得对象经过了AOP的代理
                        container[key] = AOPBuider.Buider(type);
            }
            return container[key];
        }

        public static SingletonType Get<SingletonType>(Func<SingletonType> getFunc = default)
        {
            Type type = typeof(SingletonType);
            var key = type.FullName;
            if (!container.ContainsKey(key) || container[key] == null)
            {
                lock (container)
                    if (!container.ContainsKey(key) || container[key] == null)
                        container[key] = getFunc != default ? getFunc() : TypeUtils.GetInstance<SingletonType>();
            }
            return (SingletonType)container[key];
        }

        public static SingletonType Get<SingletonType>(string key, Func<SingletonType> getFunc = default)
        {
            if (!container.ContainsKey(key) || container[key] == null)
            {
                lock (container)
                    if (!container.ContainsKey(key) || container[key] == null)
                        container[key] = getFunc != default ? getFunc() : TypeUtils.GetInstance<SingletonType>();
            }
            return (SingletonType)container[key];
        }
    }
}
