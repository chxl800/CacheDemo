using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using RedisCache.Redis.Interfaces;
using RedisCache.Extensions;

namespace RedisCache.Redis.Abstractions
{
    public class RedisHash : IRedisHash
    {
        internal Func<IDatabase> GetRedisClient { get; set; }

        public void Set(string key, string field, string value)
        {
            IDatabase redis = GetRedisClient();
            redis.HashSet(key, field, value);
        }

        public long Count(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashLength(key);
        }

        public void Set<T>(string key, string field, T value)
        {
            Set(key, field, value.ToJson(isFormat: false));
        }

        public void Set<T>(string key, IDictionary<string, T> values)
        {
            IDatabase redis = GetRedisClient();
            redis.HashSet(key, values.Select(it => new HashEntry(it.Key, it.Value.ToJson())).ToArray());
        }

        public void Set(string key, IDictionary<string, string> values)
        {
            IDatabase redis = GetRedisClient();
            redis.HashSet(key, values.Select(it => new HashEntry(it.Key, it.Value)).ToArray());
        }

        public string Get(string key, string field)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashGet(key, field);
        }

        public long SetIncrement(string key, string field, long value = 1)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashIncrement(key, field, value);
        }

        public T Get<T>(string key, string field)
        {
            IDatabase redis = GetRedisClient();
            var value = redis.HashGet(key, field);
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return value.ToString().ToObject<T>();
        }

        public IList<string> Get(string key, string[] fields)
        {
            IDatabase redis = GetRedisClient();
            var arr = fields.Select(it => (RedisValue) it).ToArray();
            return redis.HashGet(key, arr).Select(it => it.ToString()).ToList();
        }

        public IList<T> Get<T>(string key, string[] fields)
        {
            IDatabase redis = GetRedisClient();
            var arr = fields.Select(it => (RedisValue) it).ToArray();
            var list = redis.HashGet(key, arr).Where(it => it.HasValue);
            return list.Select(it => it.ToString().ToObject<T>()).ToList();
        }

        public IDictionary<string, string> GetAll(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashGetAll(key).ToDictionary(it => it.Name.ToString(), it => it.Value.ToString());
        }

        public IDictionary<string, T> GetAll<T>(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashGetAll(key).ToDictionary(it => it.Name.ToString(), it => it.Value.ToString().ToObject<T>());
        }

        public IList<string> GetKyes(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashKeys(key).Select(it => it.ToString()).ToList();
        }

        public IList<string> GetValues(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashValues(key)
                .Select(it => it.ToString())
                .ToList();
        }

        public IList<T> GetValues<T>(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashValues(key)
                .Select(it => it.IsNull ? default(T) : it.ToString().ToObject<T>())
                .ToList();
        }

        public bool Exists(string key, string field)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashExists(key, field);
        }

        public bool Remove(string key, string field)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashDelete(key, field);
        }

        public long Remove(string key, IList<string> fields)
        {
            IDatabase redis = GetRedisClient();
            return redis.HashDelete(key, fields.Select(it => (RedisValue) it).ToArray());
        }
    }
}