using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DotNet.Utilities.图片
{
    /// <summary>
    /// 根据Url链接保存Image图片到本地磁盘
    /// </summary>
    public class DownloadImgByUrl
    {
        public static byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myrep = myReq.GetResponse();

            Stream stream = myrep.GetResponseStream();

            //int i;
            using(BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            myrep.Close();
            return b;
        }

        public static void WriteBytesToFile(string fileName,byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }
        }
    }
}
