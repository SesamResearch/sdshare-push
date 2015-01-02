using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

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
            CacheMethod = cacheMethod;
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

        public CacheMethod CacheMethod { get; private set; }

        public TimeSpan Expiration
        {
            get { return _slidingCachExpiration; }
        }

        public void Receive(IEnumerable<string> resources, string graphUri, string payload)
        {
            var key = GetKey(resources, graphUri, payload);
            if (!IsInCache(key))
            {
                _wrappedReceiver.Receive(resources, graphUri, payload);
            }
        }

        public IEnumerable<string> Labels { get; set; }

        public IFragmentReceiver WrappedReceiver
        {
            get { return _wrappedReceiver; }
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

        private string GetKey(IEnumerable<string> resources, string graphUri, string payload)
        {
            var rsrc = resources == null
                ? string.Empty
                : resources.OrderBy(r => r).Aggregate(
                    new StringBuilder(),
                    (sb, r) =>
                    {
                        sb.Append(r);
                        return sb;
                    }).ToString();

            var s = string.Format("{0}{1}{2}",
                payload ?? string.Empty,
                graphUri ?? string.Empty,
                rsrc);

            return s.GetHashCode().ToString();
        }
    }
}
