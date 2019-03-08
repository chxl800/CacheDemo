using StackExchange.Redis;
using RedisCache.Redis.Abstractions;

namespace RedisCache.Redis
{
    /// <summary>
    /// 通用数据缓存操作类
    /// </summary>
    public class RedisCache : RedisBase
    {

        public RedisCache(RedisConfigItem config) : base(config)
        {

        }

    }
}