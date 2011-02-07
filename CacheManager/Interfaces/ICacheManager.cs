using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCache.CacheManager.Interfaces
{
    public interface ICacheManager<K,V>
    {
        void AddEntry(K key, V value);
        bool ContainsKey(K key);
        V GetEntry(K key);
    }

    public interface ICacheManager<K, V, L> : ICacheManager<K, V>
    {
        void SetDataProdiver(IDataProvider<V, L> dataProvider);
        V GetEntry(K key, L loadKey);
    }
}
