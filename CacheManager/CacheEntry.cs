using System;
using System.Threading;

namespace CacheManager
{
	public class CacheEntry<K,V>
	{
		private K key;
		private V value;
		private ManualResetEvent dataAvailable;
		
		public K Key
		{
			get {
				return key;
			}
		}
		
		public V Value
		{
			get {
				return value;
			}
			set {
				this.value = value;
			}
		}
		
		public ManualResetEvent DataAvalaible
		{
			get {
				return dataAvailable;
			}
		}
		
		public CacheEntry(K _key, V _value)
			: this(_key, _value, true)
		{
		}
		
		public CacheEntry (K _key, V _value, bool _dataAvailable)
		{
			this.key = _key;
			this.value = _value;
			this.dataAvailable = new ManualResetEvent(_dataAvailable);
		}
	}
}

