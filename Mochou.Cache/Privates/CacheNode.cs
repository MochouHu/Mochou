using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mochou.Cache.Privates
{
    /// <summary>
    /// 缓存节点
    /// </summary>
    class CacheNode
    {
        public string Key;
        public Object Value;
        public DateTime CreateTime;
    }
}
