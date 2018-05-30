using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Utilities.网络爬虫
{
    public class HttpHelper
    {
        /// <summary>   
        /// 取得HTML中所有图片的 URL。   
        /// </summary>   
        /// <param name="sHtmlText">HTML代码</param>   
        /// <returns>图片的URL列表</returns> 
        public static string HtmlCodeRequest(string Url)
        {
            if (string.IsNullOrEmpty(Url))
            {
                return "";
            }
            try
            {
                //创建一个请求
                HttpWebRequest httprequst = (HttpWebRequest)WebRequest.Create(Url);
                //不建立持久性链接
                httprequst.KeepAlive = true;
                //设置请求的方法
                httprequst.Method = "GET";
                //设置标头值
                httprequst.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                httprequst.Accept = "*/*";
                httprequst.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                httprequst.ServicePoint.Expect100Continue = false;
                httprequst.Timeout = 5000;
                httprequst.AllowAutoRedirect = true;//是否允许302
                ServicePointManager.DefaultConnectionLimit = 30;
                //获取响应
                HttpWebResponse webRes = (HttpWebResponse)httprequst.GetResponse();
                //获取响应的文本流
                string content = string.Empty;
                using (System.IO.Stream stream = webRes.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8")))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                //取消请求
                httprequst.Abort();
                //返回数据内容
                return content;
            }
            catch (Exception)
            {

                return "";
            }
        }

        /// <summary>
        /// 提取页面链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<string> GetHtmlImageUrlList(string url)
        {
            string html = HttpHelper.HtmlCodeRequest(url);
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(html);
            List<string> sUrlList = new List<string>();

            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList.Add(match.Groups["imgUrl"].Value);
            return sUrlList;
        }


        /// <summary>
        /// 提取页面链接
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<string> GetHttpLinks(string url)
        {
            //获取网址内容
            string html = HttpHelper.HtmlCodeRequest(url);
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
            //匹配http链接
            const string pattern2 = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Regex r2 = new Regex(pattern2, RegexOptions.IgnoreCase);
            //获得匹配结果
            MatchCollection m2 = r2.Matches(html);
            List<string> links = new List<string>();
            foreach (Match url2 in m2)
            {
                if (StringHelper.CheckUrlIsLegal(url2.ToString()) || links.Contains(url2.ToString()))
                    continue;
                links.Add(url2.ToString());
            }
            //匹配href里面的链接
            const string pattern = @"(?i)<a\s[^>]*?href=(['""]?)(?!javascript|__doPostBack)(?<url>[^'""\s*#<>]+)[^>]*>"; ;
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            //获得匹配结果
            MatchCollection m = r.Matches(html);
            foreach (Match url1 in m)
            {
                string href1 = url1.Groups["url"].Value;
                if (!href1.Contains("http"))
                {
                    href1 = "http://" + href1;
                }
                if (links.Contains(href1)) continue;
                links.Add(href1);
            }
            return links;
        }

        public string DownLoadimg(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    if (!url.Contains("http"))
                    {
                        url = "http://" + url;
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 2000;
                    request.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                    //是否允许302
                    request.AllowAutoRedirect = true;
                    WebResponse response = request.GetResponse();
                    Stream reader = response.GetResponseStream();
                    //文件名
                    string aFirstName = Guid.NewGuid().ToString();
                    //扩展名
                    string aLastName = url.Substring(url.LastIndexOf(".") + 1, (url.Length - url.LastIndexOf(".") - 1));
                    FileStream writer = new FileStream("/DownlowdImg/" + aFirstName + "." + aLastName, FileMode.OpenOrCreate, FileAccess.Write);
                    byte[] buff = new byte[512];
                    //实际读取的字节数
                    int c = 0;
                    while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                    {
                        writer.Write(buff, 0, c);
                    }
                    writer.Close();
                    writer.Dispose();
                    reader.Close();
                    reader.Dispose();
                    response.Close();
                    return (aFirstName + "." + aLastName);
                }
                catch (Exception)
                {
                    return "错误：地址" + url;
                }
            }
            return "错误：地址为空";
        }
    }
}
