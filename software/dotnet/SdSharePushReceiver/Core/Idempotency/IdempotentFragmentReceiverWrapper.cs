using System;
using System.Runtime.Caching;

namespace SdShare.Idempotency
{
    public class IdempotentFragmentReceiverWrapper : IFragmentReceiver
    {
        private readonly object _syncLock = new object();
        private readonly ObjectCache _cache;
        private readonly IFragmentReceiver _wrappedReceiver;
        private readonly TimeSpan _slidingCachExpiration;

        public IdempotentFragmentReceiverWrapper(IFragmentReceiver wrappedReceiver, TimeSpan slidingCachExpiration, CacheMethod cacheMethod)
        {
            _wrappedReceiver = wrappedReceiver;
            _slidingCachExpiration = slidingCachExpiration;
            if (CacheMethod.File == cacheMethod)
            {
                _cache = new FileCache();
            }
            else
            {
                _cache = new MemoryCache("MemoryCache");
            }
        }

        public void Receive(string resourceUri, string graphUri, string payload)
        {
            var key = GetKey(resourceUri, graphUri, payload);
            if (!IsInCache(key))
            {
                _wrappedReceiver.Receive(resourceUri, graphUri, payload);
            }
        }

        private bool IsInCache(string key)
        {
            if (_cache.Contains(key))
            {
                return true;
            }

            lock (_syncLock)
            {
                if (_cache.Contains(key))
                {
                    return true;
                }

                _cache.Add(key, true, new CacheItemPolicy { SlidingExpiration = _slidingCachExpiration });

                return false;
            }
        }

        private string GetKey(string resourceUri, string graphUri, string payload)
        {
            var s = string.Format("{0}{1}{2}",
                payload ?? string.Empty,
                graphUri ?? string.Empty,
                resourceUri ?? string.Empty);

            return s.GetHashCode().ToString();
        }
    }
}
