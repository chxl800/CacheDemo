using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using RedisCache.Redis.Interfaces;

namespace RedisCache.Redis.Abstractions
{
    public  class RedisList : IRedisList
    {
        internal Func<IDatabase> GetRedisClient { get; set; }

        public long Length(string key)
        {
            IDatabase redis = GetRedisClient();

            return redis.ListLength(key);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(string key, long index, string value)
        {
            IDatabase redis = GetRedisClient();
            redis.ListSetByIndex(key, index, value);
        }

        /// <summary>
        /// 列表尾部追加内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long Append(string key, string value)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListRightPush(key, value);
        }

        /// <summary>
        /// 列表头部插入内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long Prepend(string key, string value)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListLeftPush(key, value);
        }


        public void Remove(string key, string value)
        {
            IDatabase redis = GetRedisClient();
            redis.ListRemove(key, value);

        }

        /// <summary>
        /// 返回顶部对象，并移除他
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Pop(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListLeftPop(key);
        }

        /// <summary>
        /// 返回尾部对象，并移除它
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string RightPop(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListRightPop(key);
        }

        public IList<string> GetAll(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListRange(key).Cast<string>().ToList();
        }

        public IList<string> GetRange(string key, long start, long end)
        {
            IDatabase redis = GetRedisClient();
            return redis.ListRange(key, start, end).Cast<string>().ToList();
        }
    }
}