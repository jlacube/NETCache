using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NETCache.CacheManager.Interfaces
{
    public interface ICacheEntry<K,V>
    {
        K Key { get; }
        V Value { get; }
        ManualResetEvent DataAvalaible { get; }
    }
}
