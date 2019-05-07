using OnlineCamera.Core;
using OnlineCamera.Core.Config;
using OnlineCamera.Value;
using System;
using System.Collections.Concurrent;

namespace OnlineCamera.Service
{
    public class TimestamptCache<TKey, TValue>
    {
        public class TimestamptCacheKey
        {
            public TKey Key { get; set; }
            public DateTime ServiceTimestamp { get; set; }
            public DateTime SourceTimestamp { get; set; }
        }

        protected readonly IDateService dateService;
        public readonly  ConcurrentDictionary<TKey, TValue> cache = new ConcurrentDictionary<TKey, TValue>();

        public TimestamptCache(IDateService dateService)
        {
            this.dateService = dateService;
        }

        public void Set(TKey key, TValue value, DateTime sourceTimestamp)
        {
            var now = dateService.GetCurrentDateTime();
            var keyT = new TimestamptCacheKey() {
                Key = key ,
                ServiceTimestamp = now,
                SourceTimestamp = sourceTimestamp };

            cache.AddOrUpdate(key, k => value, (k, oldV) => {
                keyT.ServiceTimestamp = now;
                keyT.SourceTimestamp = sourceTimestamp;
                return value;
            });
        }

        public TValue Get(TKey key) => cache.GetValue(key);


    }
}
