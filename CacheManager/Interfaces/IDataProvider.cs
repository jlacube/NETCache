using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCache.CacheManager.Interfaces
{
    public interface IDataProvider<V, L>
    {
        V LoadObject(L _loadKey);
    }
}
