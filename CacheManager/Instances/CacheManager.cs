using System;
using System.Collections.Generic;
using System.Threading;

using NETCache.CacheManager.Interfaces;
using NETCache.CacheTools;

namespace NETCache.CacheManager
{
    public class CacheManager<K, V> : ICacheManager<K, V>
    {
        private Dictionary<K, CacheEntry<K, V>> internalCache;
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public CacheManager()
        {
            internalCache = new Dictionary<K, CacheEntry<K, V>>();
        }

        public bool ContainsKey(K key)
        {
            cacheLock.EnterReadLock();

            try
            {
                return internalCache.ContainsKey(key);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public void AddEntry(K _key, V _value)
        {
            cacheLock.EnterUpgradeableReadLock();

            try
            {
                if (!internalCache.ContainsKey(_key))
                {
                    cacheLock.EnterWriteLock();

                    try
                    {
                        internalCache.Add(_key, new CacheEntry<K, V>(_key, _value, true));
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        public V GetEntry(K _key)
        {
            cacheLock.EnterReadLock();

            try
            {
                if (internalCache.ContainsKey(_key))
                {
                    internalCache[_key].DataAvalaible.WaitOne();
                    return internalCache[_key].Value;
                }
                else
                    return default(V);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }
    }

	public class CacheManager<K,V,L> : ICacheManager<K,V,L>
	{
		private IDataProvider<V,L> dataProvider;
		private Dictionary<K,CacheEntry<K,V>> internalCache;
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public void SetDataProdiver(IDataProvider<V, L> newDataProvider)
        {
            dataProvider = newDataProvider;
        }

        public CacheManager()
            :this(null)
        {
        }

		public CacheManager(IDataProvider<V,L> _dataProvider)
		{
			internalCache = new Dictionary<K, CacheEntry<K, V>>();
			dataProvider = _dataProvider;
		}

        public bool ContainsKey(K key)
        {
            cacheLock.EnterReadLock();

            try
            {
                return internalCache.ContainsKey(key);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

		public void AddEntry(K _key, V _value)
		{
			AddEntry(_key, _value, true);
		}
		
		private void AddEntry(K _key, V _value, bool _dataAvailable)
		{
			cacheLock.EnterUpgradeableReadLock();
			
			try
			{
				if (!internalCache.ContainsKey(_key))
				{
					cacheLock.EnterWriteLock();
					
					try
					{
						internalCache.Add(_key, new CacheEntry<K,V>(_key, _value, _dataAvailable));
					} finally {
						cacheLock.ExitWriteLock();
					}
				}
			}
			finally
			{
				cacheLock.ExitUpgradeableReadLock();
			}
		}
		
		public V GetEntry(K _key)
		{
			cacheLock.EnterReadLock();
			
			try {
				if (internalCache.ContainsKey(_key))
				{
					internalCache[_key].DataAvalaible.WaitOne();
					return internalCache[_key].Value;
				} else
					return default(V);
			} finally {
				cacheLock.ExitReadLock();
			}
		}
		
		public V GetEntry(K _key, L _loadKey)
		{
            if (dataProvider == null)
                return GetEntry(_key);

			cacheLock.EnterUpgradeableReadLock();
			
			try {
				V entry = GetEntry(_key);
			
				if (entry.Equals(default(V)))
				{
					AddEntry(_key, default(V), false);
                    internalCache[_key].Value = dataProvider.LoadObject(_loadKey);
                    internalCache[_key].DataAvalaible.Set();
					
					return GetEntry(_key);
				}
				else
				{
					return entry;	
				}
				
			} finally {
				cacheLock.ExitUpgradeableReadLock();
			}
		}
	}
}

