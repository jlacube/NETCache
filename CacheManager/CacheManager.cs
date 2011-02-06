using System;
using System.Collections.Generic;
using System.Threading;

namespace CacheManager
{
	public interface IDataProvider<V,L>
	{
		V LoadObject(L _loadKey);
	}
	
	public class CacheManager<K,V,L>
	{
		private IDataProvider<V,L> dataProvider;
		private Dictionary<K,CacheEntry<K,V>> internalCache;
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
		
		public CacheManager (IDataProvider<V,L> _dataProvider)
		{
			internalCache = new Dictionary<K, CacheEntry<K, V>>();
			dataProvider = _dataProvider;
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
		
		private void UpdateObjectWhenLoaded(K _key, L _loadKey)
		{
			internalCache[_key].Value = dataProvider.LoadObject(_loadKey);
			internalCache[_key].DataAvalaible.Set();
		}
		
		public V GetEntry(K _key, L _loadKey)
		{
			cacheLock.EnterUpgradeableReadLock();
			
			try {
				V entry = GetEntry(_key);
			
				if (entry.Equals(default(V)))
				{
					AddEntry(_key, default(V), false);
					UpdateObjectWhenLoaded(_key, _loadKey);
					
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

