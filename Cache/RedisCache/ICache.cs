using System;

namespace RedisCache.Cache
{
    public interface ICache
    {
        /// <summary>
        /// 获取一个缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        void Set<T>(string key, T value, int? second = null);

        /// <summary>
        /// 移除一个缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);


        /// <summary>
        /// 获取一个缓存对象，如果为Null时，执行生成对象的方法，并写入缓存中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func">对象不存在时，生成对象的方法</param>
        /// <param name="second">秒，默认为永久</param>
        T GetOrSet<T>(string key, Func<T> func, int? second = null);

        /// <summary>
        /// 设置一个缓存，
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second">秒，默认为永久</param>
        void Set(string key, string value, int? second = null);
    }
}