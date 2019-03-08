using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ServiceStack;
using ServiceStack.Text;
 

namespace RedisCache.Extensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
      
        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isToLower">是否将键的首字母转为小写</param>
        /// <param name="isFormat">是否格式化，默认不格式化</param>
        /// <param name="useValuesOfEnums">枚举是否使用数字</param>
        /// <param name="includeTypeInfo">是否包含类型信息</param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool isToLower = true, bool isFormat = false,
            bool useValuesOfEnums = true, bool includeTypeInfo = false)
        {
            JsConfig.IncludeTypeInfo = includeTypeInfo; //排除类型信息
            JsConfig.ExcludeTypeInfo = !includeTypeInfo; //排除类型信息
            JsConfig.EmitCamelCaseNames = isToLower;
            JsConfig.TreatEnumAsInteger = useValuesOfEnums;

            var json = JsonSerializer.SerializeToString(obj);

            return isFormat ? json.IndentJson() : json;
        }

        /// <summary>
        /// 将json文件序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return json.FromJson<T>();
        }

        /// <summary>
        /// 将json文件序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(this string json, Type type)
        {
            return JsonSerializer.DeserializeFromString(json, type);
        }

        /// <summary>
        ///获取指定字符串md5值 ,使用32小写加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str, Encoding ec = null)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = ec == null ? Encoding.Default.GetBytes(str) : ec.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            StringBuilder strMd5 = new StringBuilder();
            for (int i = 0; i < targetData.Length; i++)
            {
                strMd5.AppendFormat("{0:x2}", targetData[i]);
            }
            return strMd5.ToString();
        }

        /// <summary>
        /// 字符串MD5加密后小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLowerMD5(this string str)
        {
            return str.ToMD52().ToLower();
        }

        public static string ToMD52(this string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //string md5Str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)));
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string pwd = "";
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;// md5Str.Replace("-", "");
        }
        /// <summary>
        /// 获取指定字符窜加密为密码使用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPassword(this string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string md5Str = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)));
            string md5Join = string.Join("", md5Str.Split('-').OrderByDescending(c => c).ToList());
            SHA256 sha256 = new SHA256Managed();
            string password = BitConverter.ToString(sha256.ComputeHash(UTF8Encoding.Default.GetBytes(md5Join)));
            return password.Replace("-", "");
        }

        /// <summary>
        /// 字符串SHA256编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA256(this string str)
        {
            SHA256 sha256 = new SHA256Managed();
            string shaPwd = BitConverter.ToString(sha256.ComputeHash(UTF8Encoding.Default.GetBytes(str)));
            return shaPwd.Replace("-", "");
        }

        /// <summary>
        /// 字符串转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="def">默认值为0，可选</param>
        /// <returns></returns>
        public static int ToInt(this string str, int def = 0)
        {
            int result = 0;
            if (int.TryParse(str, out result)) return result;
            else return def;
        }


        /// <summary>
        /// object转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <param name="def">默认值为0，可选</param>
        /// <returns></returns>
        public static int ToInt(this object obj, int def = 0)
        {
            if (obj == null) return def;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                int result = 0;
                if (int.TryParse(obj.ToString(), out result)) return result;
                else return def;
            }
        }

        public static int ToInt(this decimal obj)
        {
            return (int)(obj);
        }
        public static int ToInt(this float obj)
        {
            return (int)(obj);
        }
        public static int ToInt(this double obj)
        {
            return (int)(obj);
        }
        public static int ToInt(this Hashtable ht, string key, int def = 0)
        {
            if (ht == null || string.IsNullOrWhiteSpace(key)) return def;
            if (!ht.ContainsKey(key)) return def;
            return ht[key].ToInt(def);
        }

        /// <summary>
        /// 枚举转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">枚举对象</param>
        /// <returns></returns>
        public static int ToInt(this Enum obj)
        {
            return obj.ToInt(0);
        }


        /// <summary>
        /// 枚举转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">枚举对象</param>
        /// <param name="def">默认值为0，可选</param>
        /// <returns></returns>
        public static int ToInt(this Enum obj, int def = 0)
        {
            if (obj == null) return def;
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return def;
            }
        }

        public static decimal ToDecimal(this string str, decimal def = 0)
        {
            decimal result = 0;
            if (decimal.TryParse(str, out result)) return result;
            else return def;
        }

        public static decimal ToDecimal(this object obj, decimal def = 0)
        {
            if (obj == null) return def;
            decimal result = 0;
            if (decimal.TryParse(obj.ToString(), out result)) return result;
            else return def;
        }

        /// <summary>
        /// 四舍五入保留两位小数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal2(this decimal obj)
        {
            return Math.Round(obj, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 四舍五入保留两位小数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal4(this decimal obj)
        {
            return Math.Round(obj, 4, MidpointRounding.AwayFromZero);
        }

        public static double ToDouble(this object obj, double def = 0)
        {
            if (obj == null) return def;
            double result = 0;
            if (double.TryParse(obj.ToString(), out result)) return result;
            else return def;
        }

        /// <summary>
        /// 逗号分隔数字每三位一个逗号不保留小数点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToCommaSeparateString(this object obj)
        {
            return string.Format("{0:N0}", obj);
        }
        /// <summary>
        /// 逗号分隔数字每三位一个逗号保留小数点2位
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToCommaSeparateString2(this object obj)
        {
            return string.Format("{0:N2}", obj);
        }
        /// <summary>
        /// 逗号分隔数字每三位一个逗号保留小数点4位
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToCommaSeparateString4(this object obj)
        {
            return string.Format("{0:N4}", obj);
        }

        /// <summary>
        /// object转换ToStr()，转换失败返回默认值   
        /// </summary>
        public static string ToStr(this object obj, string def = "")
        {
            if (obj == null) return def;
            else return obj.ToString().Trim();

            //if (obj is String)
            //    return obj.ToString();
            //else
            //    return def;
        }

        /// <summary>
        /// object转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <param name="def">默认值为DateTime.Now，可选</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj, DateTime? def = null)
        {
            if (def == null)
                def = DateTime.Now;
            if (obj == null)
                return def.Value;
            DateTime result = DateTime.Now;
            if (DateTime.TryParse(obj.ToString(), out result)) return result;
            else return def.Value;
        }


        /// <summary>
        /// 整型转换为布尔型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <returns></returns>
        public static bool IntToBool(this int value)
        {
            return value == 1 ? true : false;
        }

        /// <summary>
        /// 布尔型转换为整型
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true为1，false为0</returns>
        public static int BoolToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        /// 检查是否为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string input)
        {
            double result = 0;
            return double.TryParse(input, NumberStyles.Any, null, out result);
        }

        /// <summary>
        /// 检查是否为日期型的字符串
        /// </summary>
        /// <param name="input">要检查的字符串</param>
        /// <returns>如果是，则返回true，如果不是，则返回false</returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dt;
            return DateTime.TryParse(input, out dt);
        }

        /// <summary>
        /// 逗号分隔转list<int>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<int> ToIntList(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<int>();
            }
            return input.Split(',').Select(c => c.IsNumeric() ? int.Parse(c) : 0).ToList();
        }

        /// <summary>
        /// 匹配单项
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="ro">枚举值的一个按位组合，默认值为指定不区分大小写的匹配</param>
        /// <returns></returns>
        public static RegexDictionary Match(this string input, string pattern,
            RegexOptions ro = RegexOptions.IgnoreCase)
        {
            RegexDictionary dic = new RegexDictionary();
            Match m = Regex.Match(input, pattern, ro);
            if (!m.Success) return dic;
            for (int i = 0; i < m.Groups.Count; i++)
            {
                dic[i] = m.Groups[i].Value;
            }
            return dic;
        }

        /// <summary>
        /// 匹配多项
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="ro">枚举值的一个按位组合，默认值为指定不区分大小写的匹配</param>
        /// <returns></returns>
        public static ListDictionary Matches(this string input, string pattern,
            RegexOptions ro = RegexOptions.IgnoreCase)
        {
            ListDictionary list = new ListDictionary();
            MatchCollection mc = Regex.Matches(input, pattern, ro);
            if (mc.Count == 0) return list;
            for (int j = 0; j < mc.Count; j++)
            {
                RegexDictionary dic = new RegexDictionary();
                Match m = mc[j];
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    dic[i] = m.Groups[i].Value;
                }
                list[j] = dic;
            }
            return list;
        }


        /// <summary>
        /// Guid转成Mysql的无横线字符串
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToShortString(this Guid guid)
        {
            return guid.ToString("N");
        }

        /// <summary>
        /// 将一个字符串集合合并称一个字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> list, string separator)
        {
            var values = list as string[] ?? list.ToArray();
            return (list.IsNotNull() && values.Any()) ? string.Join(separator, values) : string.Empty;
        }
        
    }


    public class RegexDictionary
    {
        private Dictionary<int, string> dic = new Dictionary<int, string>();

        public string this[int key]
        {
            get { return dic.ContainsKey(key) ? dic[key] : string.Empty; }
            set { dic[key] = value; }
        }

        public int Count
        {
            get { return dic.Keys.Count; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return dic.Values.GetEnumerator();
        }
    }

    public class ListDictionary
    {
        private Dictionary<int, RegexDictionary> dic = new Dictionary<int, RegexDictionary>();

        public RegexDictionary this[int key]
        {
            get { return dic.ContainsKey(key) ? dic[key] : new RegexDictionary(); }
            set { dic[key] = value; }
        }

        public int Count
        {
            get { return dic.Keys.Count; }
        }

        public IEnumerator<RegexDictionary> GetEnumerator()
        {
            return dic.Values.GetEnumerator();
        }
    }
}