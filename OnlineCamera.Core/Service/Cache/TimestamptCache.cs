using OnlineCamera.Core;
using OnlineCamera.Core.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OnlineCamera.Core
{
    public class TimestamptCache<TKey, TValue>
    {
        public class TimestamptCacheKey
        {
            public TKey Key { get; set; }
            public DateTime ServiceTimestamp { get; set; }
            public DateTime SourceTimestamp { get; set; }

            public override bool Equals(object obj)
            {
                return obj is TimestamptCacheKey key &&
                    EqualityComparer<TKey>.Default.Equals(Key, key.Key);
            }

            public override int GetHashCode()
            {
                var hashCode = -2016576744;
                hashCode = hashCode * -1521134295 + EqualityComparer<TKey>.Default.GetHashCode(Key);
                return hashCode;
            }

            public static bool operator ==(TimestamptCacheKey x, TimestamptCacheKey y)
                => x.Key.Equals(y.Key);

            public static bool operator !=(TimestamptCacheKey x, TimestamptCacheKey y)
                => !x.Key.Equals(y.Key);
            
        }

        protected readonly IDateService dateService;

        ConcurrentDictionary<TimestamptCacheKey, TValue> Cache { get; set; } 
            = new ConcurrentDictionary<TimestamptCacheKey, TValue>();

        public void SetCache(Dictionary<TimestamptCacheKey, TValue> cache)
        {
            Cache = new ConcurrentDictionary<TimestamptCacheKey, TValue>(cache);
        }

        public TimestamptCacheKey[] Keys => Cache.Keys.ToArray();

        public void RemoveAll(IEnumerable<TimestamptCacheKey> keys)
        {
            Cache.RemoveAll(keys);
        }

        public KeyValuePair<TimestamptCacheKey, TValue>[] ToArray() => Cache.ToArray();
        

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

            Cache.AddOrUpdate(keyT, k => value, (k, oldV) => {
                k.ServiceTimestamp = now;
                k.SourceTimestamp = sourceTimestamp;
                return value;
            });
        }

        public TValue Get(TKey key) => Cache.GetValue(new TimestamptCacheKey { Key = key });
    }
}
