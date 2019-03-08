using System.Collections.Generic;
namespace RedisCache.Redis.Interfaces
{
    public interface IRedisList
    {


        IList<string> GetAll(string key);
        long Length(string key);
        IList<string> GetRange(string key, long start, long end);
        void Remove(string key, string value);

        /// <summary>
        /// 列表尾部追加内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        long Append(string key, string value);

        /// <summary>
        /// 列表头部插入内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        long Prepend(string key, string value);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void Insert(string key, long index, string value);

        /// <summary>
        /// 返回顶部对象，并移除他
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Pop(string key);

        /// <summary>
        /// 返回尾部对象，并移除它
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string RightPop(string key);
    }
}