using System.Text.RegularExpressions;

namespace DotNet.Utilities
{
    /// <summary>
    /// 操作正则表达式的公共类
    /// </summary>    
    public class RegexHelper
    {
        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

        /// <summary>
        ///    验证助手类
        /// </summary>
        public class ValidHelper
        {
            #region 验证只能是字母或数字
            /// <summary>
            /// 验证只能是字母或数字
            /// </summary>
            /// <param name="str">验证的字符串</param>
            /// <returns>判断结果</returns>
            public static bool IsLetterOrnumber(string str)
            {
                Regex r = new Regex("^[0-9a-zA-Z]*$");

                return !r.IsMatch(str);
            }
            #endregion

            #region 验证是否为正整数
            /// <summary>
            /// 验证是否为正整数
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsInt(string str)
            {

                return Regex.IsMatch(str, @"^[0-9]*$");
            }
            #endregion

            #region 验证是否符合email格式
            /// <summary>
            /// 验证是否符合email格式
            /// </summary>
            /// <param name="strEmail">要判断的email字符串</param>
            /// <returns>判断结果</returns>
            public static bool IsValidEmail(string strEmail)
            {
                //return Regex.IsMatch(strEmail, @"^[A-Za-z0-9-_]+@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
                return Regex.IsMatch(strEmail, @"^[\w\.]+@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
            }

            public static bool IsValidDoEmail(string strEmail)
            {
                return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            }
            #endregion

            #region 验证是否是正确的Url
            /// <summary>
            /// 验证是否是正确的Url
            /// </summary>
            /// <param name="strUrl">要验证的Url</param>
            /// <returns>判断结果</returns>
            public static bool IsURL(string strUrl)
            {
                return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
            }
            #endregion

            #region 验证是否为IP
            /// <summary>
            /// 验证是否为IP
            /// </summary>
            /// <param name="ip"></param>
            /// <returns></returns>
            public static bool IsIP(string ip)
            {
                return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            }

            public static bool IsIPSect(string ip)
            {
                return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
            }
            #endregion

            #region 验证是否为手机号码
            /// <summary>
            /// 验证是否为手机号码
            /// </summary>
            /// <param name="handset"></param>
            /// <returns></returns>
            public static bool IsHandset(string handset)
            {
                return System.Text.RegularExpressions.Regex.IsMatch(handset, @"^[1]+[3,5,8]+\d{9}");
            }
            #endregion

            #region 验证是否为身份证号码
            /// <summary>
            /// 验证是否为身份证号码
            /// </summary>
            /// <param name="idcard"></param>
            /// <returns></returns>
            public static bool IsIDcard(string idcard)
            {
                return System.Text.RegularExpressions.Regex.IsMatch(idcard, @"(^\d{18}$)|(^\d{15}$)");
            }
            #endregion

            #region 验证是否是时间格式
            /// <summary>
            /// 验证是否是时间格式
            /// </summary>
            /// <returns></returns>
            public static bool IsTime(string timeval)
            {
                return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
            }
            #endregion

            #region 检测是否有Sql危险字符
            /// <summary>
            /// 检测是否有Sql危险字符
            /// </summary>
            /// <param name="str">要判断字符串</param>
            /// <returns>判断结果</returns>
            public static bool IsSafeSqlString(string str)
            {

                return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
            }
            #endregion

            #region 判断是否为base64字符串
            /// <summary>
            /// 判断是否为base64字符串
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsBase64String(string str)
            {
                //A-Z, a-z, 0-9, +, /, =
                return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
            }
            #endregion

            #region 判断字符串是否是yy-mm-dd字符串
            /// <summary>
            /// 判断字符串是否是yy-mm-dd字符串
            /// </summary>
            /// <param name="str">待判断字符串</param>
            /// <returns>判断结果</returns>
            public static bool IsDateString(string str)
            {
                return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
            }
            #endregion

            #region 验证是否符合扩展名要求
            /// <summary>
            /// 验证是否符合扩展名要求
            /// </summary>
            /// <param name="extensions">扩展名字符串</param>
            /// <param name="fileName">文件名</param>
            /// <returns>判断结果</returns>
            public static bool IsValidFileExtensions(string extensions, string fileName)
            {
                string strPattern = string.Format(@"^.+\.(?:{0})$", extensions.Replace(',', '|'));
                Regex reg = new Regex(strPattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

                return reg.IsMatch(fileName);
            }
            #endregion
        }
    }
}
