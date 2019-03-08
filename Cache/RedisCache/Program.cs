using RedisCache.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisCache.Redis;
using RedisCache.Redis.Abstractions;

namespace RedisCache.Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("请输入本地缓存值：");
            //LocalCache lc = new LocalCache();
            //lc.Set("lc", Console.ReadLine());

            //Console.WriteLine($"获取到本地缓存值：{lc.Get<string>("lc")}");
            //Console.ReadKey();



            Console.Write("请输入Redis缓存值：");
            RedisConfigItem config = new RedisConfigItem();
            config.Password = "";
            //config.Hosts = new string[] { "119.147.171.113:16379" };
            config.Hosts = new string[] { "127.0.0.1:6379" };
            RedisBase rc = new RedisBase(config);
            rc.Set("rc", Console.ReadLine());


  
            Console.WriteLine($"获取到Redis缓存值：{rc.Get<string>("rc")}");

            rc.Hash.Set("hash", "hash1", "你好1");
            rc.Hash.Set("hash", "hash2", "你好2");

            Console.WriteLine($"获取到Redis_hash缓存值：{rc.Hash.Get("hash", "hash1")}");
            Console.WriteLine($"获取到Redis_hash缓存值：{rc.Hash.Get("hash", "hash2")}");

            var hashkeys = rc.Hash.GetKyes("hash");
            Console.WriteLine($"获取到Redis_hashkeys缓存值：{string.Join(",", hashkeys)}");

            //移出
            rc.Hash.Remove("hash", hashkeys);
            Console.WriteLine($"移出后获取到Redis_hash缓存值：{rc.Hash.Get("hash", "hash1")}");
            Console.WriteLine($"移出后获取到Redis_hash缓存值：{rc.Hash.Get("hash", "hash2")}");

            Console.ReadKey();
        }
    }
}
