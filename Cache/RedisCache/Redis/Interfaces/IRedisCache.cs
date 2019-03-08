using RedisCache.Cache;
using System;
using System.Collections.Generic;

namespace RedisCache.Redis.Interfaces
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    internal interface IRedisCache : ICache
    {
        IRedisHash Hash { get; }
        IRedisList List { get; }
        List<T> Get<T>(string[] keys);
        string Get(string key);
        double SetIncrement(string key, double value);
        /// <summary>
        /// 判断redis是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 设置一个Key的到期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        bool Expire(string key, DateTime time);

        /// <summary>
        /// 设置一个Key的生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        bool Expire(string key, TimeSpan time);

        /// <summary>
        /// 取消一个Key的生存时间，让它永久保存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Persist(string key);

    }
}