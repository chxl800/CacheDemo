

using RedisCache.Cache;

namespace RedisCache.Local
{
    /// <summary>
    /// 本地缓存，用.Net自带的MemoryCache实现
    /// </summary>
    public interface ILocalCache : ICache
    {
    }
}