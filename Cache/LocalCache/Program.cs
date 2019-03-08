using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalCache
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("请输入缓存值: ");
            var str = Console.ReadLine().ToString();
            Mcache cdb = new Mcache();
            cdb.Set("cde", str);

            var gstr = cdb.Get<string>("cde");
            Console.WriteLine($"获取的缓存值：{gstr}");
            Console.ReadKey();

        }
    }
}
