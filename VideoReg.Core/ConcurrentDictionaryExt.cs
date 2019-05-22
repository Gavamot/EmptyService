using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VideoReg.Core
{
    public static class CollectionExt
    {
        public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] res = new T[self.Count];
            self.CopyTo(res, 0);
            return res;
        }
    }

    public static class ConcurrentDictionaryExt
    {
        /// <summary>
        /// Добавляет элемент если его нет
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns> false - элемент с таким ключом уже в коллекции, true - элемент успешно добавлен </returns>
        public static bool AddIfNotExist<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            bool isExist = false;
            while (!self.ContainsKey(key))
            {
                self.TryAdd(key, value);
                isExist = true;
            }
            return isExist;
        }
        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            while (self.ContainsKey(key))
            {
                self.TryRemove(key, out var v);
            }
        }

        public static void RemoveAll<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, IEnumerable<TKey> keys)
        {
            foreach(var key in keys)
            {
                self.Remove(key);
            }
        }

        public static TValue GetValueIfNotExistDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        { 
            while (self.ContainsKey(key))
            {
                if (self.TryGetValue(key, out var res))
                {
                    return res;
                }
            }
            return default;
        }
        public static TValue GetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            while (self.ContainsKey(key))
            {
                if (self.TryGetValue(key, out var res))
                {
                    return res;
                }
            }
            throw new KeyNotFoundException();
        }

        public static TValue AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            return self.AddOrUpdate(key, k => value, (k, oldV) => value);
        }
    }
}