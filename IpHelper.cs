
using System;
using System.Text.RegularExpressions;

namespace DotNet.Utilities
{
    /// <summary>
    /// 共用工具类
    /// </summary>
    public static class IpHelper
    {
        #region 获得用户IP
        /// <summary>
        /// 获得用户IP
        /// </summary>
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }
        #endregion

        #region 访客IP获取

        public static string GetVisitorsIpAddress()
        {
            string result = string.Empty;
            result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //如果使用代理，获取真实IP
            if (result != null && result.IndexOf(".") == -1)
            {
                result = null;
            }else if (result != null)
            {
                if (result.IndexOf(",") != -1)
                {
                    //有“,”，可能多个代理，取第一个不是内网的IP
                    result = result.Replace(" ", "").Replace("'", "");
                    string[] temparyip = result.Split(",;".ToCharArray());
                    for (int i = 0; i < temparyip.Length; i++)
                    {
                        if (IsIpAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." &&
                            temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                        {
                            return temparyip[i];//找到不是内网的地址
                        }
                    }
                }else if (IsIpAddress(result)) //代理即是IP格式
                    return result;
                else
                {
                    result = null;//代理中的内容 非IP，取IP
                }
            }
            if (null == result || result == string.Empty)
                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == string.Empty)
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }

        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="ipstr"></param>
        /// <returns></returns>
        private static bool IsIpAddress(string ipstr)
        {
            if(string.IsNullOrEmpty(ipstr)||ipstr.Length<7||ipstr.Length>15)
                return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat,RegexOptions.IgnoreCase);
            return regex.IsMatch(ipstr);
        }
        #endregion

    }
}
