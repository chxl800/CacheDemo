using System.Collections.Generic;

namespace RedisCache.Redis.Interfaces
{
    public interface IRedisHash
    {

        void Set(string key, string field, string value);
        bool Remove(string key, string field);
        long Remove(string key, IList<string> fields);
        bool Exists(string key, string field);
        string Get(string key, string field);
        T Get<T>(string key, string field);
        IList<string> Get(string key, string[] fields);
        IDictionary<string, string> GetAll(string key);
        IList<string> GetKyes(string key);
        IList<string> GetValues(string key);
        void Set(string key, IDictionary<string, string> values);
        IDictionary<string, T> GetAll<T>(string key);
        void Set<T>(string key, IDictionary<string, T> values);
        IList<T> Get<T>(string key, string[] fields);
        IList<T> GetValues<T>(string key);
        void Set<T>(string key, string field, T value);
        long SetIncrement(string key, string field, long value = 1);
        long Count(string key);
    }
}