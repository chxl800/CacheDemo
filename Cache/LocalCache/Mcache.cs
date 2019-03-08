using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace LocalCache
{
    /// <summary>
    /// 本地缓存，用.Net自带的MemoryCache实现
    /// </summary>
    public class Mcache
    {

        private static readonly MemoryCache Cache = MemoryCache.Default;

        /// <summary>
        /// 获取一个缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        public void Set(string key, string value, int? second = null)
        {
            Set<string>(key, value, second);

        }

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        public void Set<T>(string key, T value, int? second)
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
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 获取一个缓存对象，如果为Null时，执行生成对象的方法，并写入缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func">对象不存在时，生成对象的方法</param>
        /// <param name="second">秒，默认为永久</param>
        public T GetOrSet<T>(string key, Func<T> func, int? second = null)
        {
            var item = Get<T>(key);
            if (item == null)
            {
                item = func();
                Set(key, item, second);
            }

            return item;
        }
    }
}
