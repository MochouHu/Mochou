using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Mochou.Core;

namespace Mochou.Cache
{
    public class CacheConfig
    {
        /// <summary>
        /// 系统默认Config
        /// </summary>
        public static CacheConfig Instance { get => SingletonContainer.Get<CacheConfig>(); }
        public static CacheConfig GetCache(string cacheName) => SingletonContainer.Get<CacheConfig>(() =>
        {
            //先不实现  根据cacheName获取Config
            return Instance;
        });

        /// <summary>
        /// 最大缓存数(不是占用内存而是缓存条数)---将有10%的溢出防止频繁释放资源（当前采用Link方式不必担心这个问题）---
        /// 默认1024条记录
        /// </summary>
        public int MaxSize = 1024;

        /// <summary>
        /// 缓存过期时间，单位分钟
        /// 默认一小时
        /// </summary>
        public int Overtime = 60;

        ///// <summary>
        ///// 是否保存到硬盘
        ///// 暂时不提供
        ///// </summary>
        //public bool SaveToDrive = false;
    }
}
