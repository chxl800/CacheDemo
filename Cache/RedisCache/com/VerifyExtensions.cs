using System.Text.RegularExpressions;

namespace RedisCache.Extensions
{
    public static class ValiadateExtensions
    {
        /// <summary>
        /// 判断对象是否为null
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsNull(this object item)
        {
            return item == null;
        }

        /// <summary>
        /// 判断是否不为null
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object item)
        {
            return item != null;
        }

        /// <summary>
        /// 判断指定的字符串否空字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        /// <summary>
        /// 判断指定的字符串否空字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断指定的字符串否null或者空白字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="ro">枚举值的一个按位组合，默认值为指定不区分大小写的匹配</param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions ro = RegexOptions.IgnoreCase)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return Regex.IsMatch(input, pattern, ro);
        }

        /// <summary>
        /// 判断参数是否为Email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            return input.IsMatch("^[a-z_0-9.-]{1,64}@([a-z0-9-]{1,200}.){1,5}[a-z]{1,6}$");
        }

        /// <summary>
        /// 判断参数是否为电话号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobile(this string input)
        {
            return
                input.IsMatch(
                    @"^(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$");
        }

        /// <summary>
        /// 判断参数是否为一个合格的SKU
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSku(this string input)
        {
            if (input.IsNullOrEmpty())
            { 
                return false;
            }
            bool res = true;
            if (input.Length == 12)
            {
                res = input.IsMatch(@"^3\d{11}$");
            }
            return res;
        }

        /// <summary>
        /// 判断参数是否为一个合格的Guid值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(this string input)
        {
            //Guid guid = Guid.Empty;
            //Guid.TryParse(input, out guid);
            return input.IsNullOrEmpty() ? false : Regex.IsMatch(input, @"^[0-9a-f]{32}$", RegexOptions.IgnoreCase);
        }
    }
}