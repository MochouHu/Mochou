using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mochou.Cache.Privates;
using Mochou.Core;

namespace Mochou.Cache
{
    public class MochouCache
    {
        private CacheConfig config;

        /// <summary>
        /// 主要用于处理过期缓存
        /// </summary>
        private LinkedList<CacheNode> orderList = new LinkedList<CacheNode>();
        /// <summary>
        /// 缓存的键值对
        /// </summary>
        private ConcurrentDictionary<string, CacheNode> datas = new ConcurrentDictionary<string, CacheNode>();

        public MochouCache() {
            config = SingletonContainer.Get<CacheConfig>();
        }

        public Value Get<Value>(string cacheKey)
        {
            return (Value)datas[cacheKey].Value;
        }
        public Value Get<Value>(string cacheKey, Func<Value> getValFonc)
        {
            if (!datas.ContainsKey(cacheKey))
                lock (datas)
                    if (!datas.ContainsKey(cacheKey))
                        datas.TryAdd(cacheKey, new CacheNode() { Key = cacheKey, Value = getValFonc(), CreateTime = DateTime.Now });

            return (Value)datas[cacheKey].Value;
        }

        public void Add(string cacheKey, object val)
        {
            CacheNode oldnode = null;

            if (datas.ContainsKey(cacheKey)) {
                    datas.TryRemove(cacheKey, out oldnode);
            }
        }

            private void add(string cacheKey, object val) {
            CacheNode node = new CacheNode() { CreateTime = DateTime.Now, Value = val, Key = cacheKey };

            if (orderList.Count > config.MaxSize) {
                //删除最老的缓存
                CacheNode cacheNode = null;
                lock (datas) datas.TryRemove(orderList.First.Value.Key, out cacheNode);
                orderList.RemoveFirst();
            }
        }



        private object getData(string cacheKey)
        {
            lock (datas) return datas[cacheKey].Value;
        }
        private bool containsKey(string cacheKey)
        {
            lock (datas) return datas.ContainsKey(cacheKey);
        }
    }
}
