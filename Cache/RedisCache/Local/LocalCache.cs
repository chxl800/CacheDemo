using RedisCache.Cache;
using System;
using System.Runtime.Caching;

namespace RedisCache.Local
{
    /// <summary>
    /// 本地缓存，用.Net自带的MemoryCache实现
    /// </summary>
    public class LocalCache : CacheBase, ILocalCache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        /// <summary>
        /// 获取一个缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public override T  Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        public override void Set(string key, string value, int? second = null)
        {
            Set<string>(key, value, second);

        }

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        public override void Set<T>(string key, T value, int? second)
        {
            if (value == null)
                Remove(key);

            if (second.HasValue)
            {
                Cache.Set(key, value, DateTimeOffset.Now.AddSeconds(second.Value));
            }
            else
            {
                Cache.Set(key, value,
                    new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.MaxValue,
                        Priority = CacheItemPriority.NotRemovable
                    });
            }
        }

        /// <summary>
        /// 移除一个缓存
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            Cache.Remove(key);
        }

    }
}