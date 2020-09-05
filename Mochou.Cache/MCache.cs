using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mochou.Core;

namespace Mochou.Cache
{
    public static class MCache
    {
        private static MochouCache cache = SingletonContainer.Get<MochouCache>();
    }
}
