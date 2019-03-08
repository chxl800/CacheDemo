using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using RedisCache.Redis.Interfaces;
using RedisCache.Extensions;
using RedisCache.Cache;

namespace RedisCache.Redis.Abstractions
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisBase : CacheBase,IRedisCache
    {
        #region 数据类型操作类

        public IRedisHash Hash { get; }
        public IRedisList List { get; }

        #endregion

        #region 
        protected Lazy<ConnectionMultiplexer> ClientsManager;
        #endregion

        protected IDatabase GetRedisClient()
        {
            return ClientsManager.Value.GetDatabase();
        }

        public RedisBase(RedisConfigItem redisConfig)
        {
            ClientsManager = new Lazy<ConnectionMultiplexer>(() =>
            {
                ConfigurationOptions config = new ConfigurationOptions()
                {
                    Password = redisConfig.Password
                };
                foreach (var readWriteHost in redisConfig.Hosts)
                {
                    config.EndPoints.Add(readWriteHost);
                }
                return ConnectionMultiplexer.Connect(config);
            });

            RedisHash hash = new RedisHash();
            RedisList list = new RedisList();
            hash.GetRedisClient = GetRedisClient;
            list.GetRedisClient = GetRedisClient;
            this.Hash = hash;
            this.List = list;
        }

        public string Get(string key)
        {
            IDatabase redis = GetRedisClient();
            var value = redis.StringGet(key);

            return value;
        }

        /// <summary>
        /// 取消一个Key的生存时间，让它永久保存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Persist(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.KeyPersist(key);
        }

        /// <summary>
        /// 设置一个Key的生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Expire(string key,TimeSpan time)
        {
            IDatabase redis = GetRedisClient();
             return     redis.KeyExpire(key, time);
        }

        /// <summary>
        /// 设置一个Key的到期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Expire(string key, DateTime time)
        {
            IDatabase redis = GetRedisClient();
            return redis.KeyExpire(key, time);
        }



        public override T Get<T>(string key)
        {
            var value = Get(key);
            if (value.IsNullOrEmpty())
            {
                return default(T);
            }
            return value.ToString().ToObject<T>();
        }


        public  double SetIncrement(string key,double value)
        {
            IDatabase redis = GetRedisClient();
          return  redis.StringIncrement(key, value);
        }




        public List<T> Get<T>(string[] keys)
        {
            IDatabase redis = GetRedisClient();
            var arr = keys.Select(it => (RedisKey) it).ToArray();

            return redis.StringGet(arr)
                .Where(it => it.HasValue)
                .Select(it => it.ToString().ToObject<T>()).ToList();
        }

        public override void Set(string key, string value, int? second = null)
        {
            IDatabase redis = GetRedisClient();

            if (value == null)
            {
                Remove(key);
            }
            else
            {
                redis.StringSet(key, value.ToJson(),
                    second.HasValue ? TimeSpan.FromSeconds(second.Value) : TimeSpan.MaxValue);
            }
        }

        public override void Set<T>(string key, T value, int? second = null)
        {
            IDatabase redis = GetRedisClient();
    
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                redis.StringSet(key, value.ToJson(),
                    second.HasValue ? TimeSpan.FromSeconds(second.Value) : TimeSpan.MaxValue);
            }
        }



        public override void Remove(string key)
        {
            IDatabase redis = GetRedisClient();
            redis.KeyDelete(key);
        }


        public virtual bool Exists(string key)
        {
            IDatabase redis = GetRedisClient();
            return redis.KeyExists(key);
        }
    }
}