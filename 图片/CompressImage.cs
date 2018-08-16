using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Utils
{
    public class ImageUtils
    {

        /// <summary>
        /// 在模板图片上描绘文字
        /// </summary>
        /// <param name="image">图片模板</param>
        /// <param name="value">描绘文字</param>
        /// <param name="xPoint">X轴</param>
        /// <param name="yPoint">Y轴</param>
        /// <param name="_SolidBrushConfig">画笔配置</param>
        /// <param name="_FontConfig">字体配置</param>
        public static void DrawString(Image image, string value, int xPoint, int yPoint, SolidBrushConfig _SolidBrushConfig = null, FontConfig _FontConfig = null)
        {
            //准备画板
            using (Graphics graphics = Graphics.FromImage(image))
            {
                _SolidBrushConfig = _SolidBrushConfig ?? new SolidBrushConfig() { Color = Color.Black };
                //设置画笔
                using (SolidBrush drawBrush = new SolidBrush(_SolidBrushConfig.Color))
                {
                    _FontConfig = _FontConfig ?? new FontConfig()
                    {
                        Family = new FontFamily("Microsoft YaHei"),
                        Size = 50,
                        Style = FontStyle.Bold
                    };
                    //设置字体颜色
                    using (Font drawFont = new Font(_FontConfig.Family, _FontConfig.Size, _FontConfig.Style))
                    {
                        //绘画
                        graphics.DrawString(value, drawFont, drawBrush, xPoint, yPoint);
                        graphics.Save();
                    }
                }
            }
        }

        /// <summary>
        /// 图片描线
        /// </summary>
        /// <param name="image"></param>
        public static void DrawLine(Image image)
        {
            Graphics graphics = Graphics.FromImage(image);
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF rect = image.GetBounds(ref unit);
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            for (int x = 100; x < rect.Width; x += 100)
            {
                graphics.DrawLine(Pens.Red, new Point(x, 0), new Point(x, (int)rect.Height));
                graphics.DrawString(x.ToString(), drawFont, drawBrush, x, 10);
            }
            for (int y = 100; y < rect.Height; y += 100)
            {
                graphics.DrawLine(Pens.Red, new Point(0, y), new Point((int)rect.Width, y));
                graphics.DrawString(y.ToString(), drawFont, drawBrush, 10, y);
            }
            graphics.Save();
            graphics.Dispose();
        }

        public static Image CombinImage(List<CombinImg> imgs)
        {
            try
            {
                var width = imgs.Max(t => t.Image.Width);
                var height = imgs.Max(t => t.Image.Height);
                Bitmap bg = new Bitmap(width, height);
                using (Graphics graphic = Graphics.FromImage(bg))
                {
                    //清除画布,背景透明
                    graphic.Clear(Color.Transparent);
                    foreach (var img in imgs)
                    {
                        graphic.DrawImage(img.Image, img.XPoint, img.YPoint);
                    }
                    graphic.Save();
                    return bg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (var img in imgs)
                {
                    img.Image.Dispose();
                }
            }
        }

        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="img">原始图片</param>
        /// <param name="targetWidth">目标图片宽度</param>
        /// <param name="targetHeight">目标图片高度</param>
        public static Image ImageEqualProportionScale(Image img,
            double targetWidth,
            double targetHeight)
        {
            //原图宽高均小于模版，不作处理，直接保存
            if (img.Width <= targetWidth && img.Height <= targetHeight)
            {
                //originalImage.Dispose();
                return img;
            }
            else
            {
                #region 尺寸算法
                //缩略图宽、高计算
                double newWidth = img.Width;
                double newHeight = img.Height;
                //宽大于高或宽等于高（横图或正方）
                if (img.Width > img.Height || img.Width == img.Height)
                {
                    //如果宽大于模版
                    if (img.Width > targetWidth)
                    {
                        //宽按模版，高按比例缩放
                        newWidth = targetWidth;
                        newHeight = img.Height * (targetWidth / img.Width);
                    }
                }
                //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (img.Height > targetHeight)
                    {
                        //高按模版，宽按比例缩放
                        newHeight = targetHeight;
                        newWidth = img.Width * (targetHeight / img.Height);
                    }
                }
                #endregion

                try
                {
                    //新建一个bmp图片
                    //using (Bitmap newImage = new Bitmap((int)newWidth, (int)newHeight))
                    //{
                    Bitmap newImage = new Bitmap((int)newWidth, (int)newHeight);
                    //新建一个画板
                    using (Graphics graphic = Graphics.FromImage(newImage))
                    {
                        //设置质量
                        graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //置背景色
                        graphic.Clear(Color.Transparent);
                        //画图
                        graphic.DrawImage(img,
                            new Rectangle(0, 0, newImage.Width, newImage.Height),
                            new Rectangle(0, 0, img.Width, img.Height),
                            GraphicsUnit.Pixel);
                        graphic.Save();

                        return newImage;
                        //保存缩略图
                        //newImage.Save(newPhysicalImagePath, rawFormat);
                        //释放资源
                        //graphic.Dispose();
                        //newImage.Dispose();
                    }
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放原图
                    img.Dispose();
                }
            }
        }


        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图或质量压缩
        /// </summary>
        /// <param name="sourcePath">源图路径（物理路径）</param>
        /// <param name="targetPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度，如果宽度为0则不缩略</param>
        /// <param name="height">缩略图高度，如果高度为0则不缩略</param>
        /// <param name="mode">生成缩略图的方式,默认为空,为空则不缩略高宽[HW 指定高宽缩放（不变形）；W 指定宽，高按比例；H 指定高，宽按比例；CUT 指定高宽裁减（不变形）]</param>  
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小,0为不限制大小</param>
        public static void MakeThumbnail(string sourcePath, string targetPath, int width = 0, int height = 0, string mode = "", int flag = 100, int size = 0)
        {
            Image sourceImage = null;
            Image bitmap = null;
            Graphics g = null;
            EncoderParameters ep = null;
            EncoderParameter eParam = null;
            try
            {
                sourceImage = Image.FromFile(sourcePath);

                int toWidth = 0;
                if (width > 0)
                {
                    toWidth = width;
                }
                else
                {
                    toWidth = sourceImage.Width;
                }

                int toHeight = 0;
                if (height > 0)
                {
                    toHeight = height;
                }
                else
                {
                    toHeight = sourceImage.Height;
                }

                int x = 0;
                int y = 0;
                int ow = sourceImage.Width;
                int oh = sourceImage.Height;

                if (width > 0 && height > 0 && !string.IsNullOrWhiteSpace(mode))
                {
                    switch (mode.ToUpper())
                    {
                        case "HW"://指定高宽缩放（不变形）
                            int tempheight = sourceImage.Height * width / sourceImage.Width;
                            if (tempheight > height)
                            {
                                toWidth = sourceImage.Width * height / sourceImage.Height;
                            }
                            else
                            {
                                toHeight = sourceImage.Height * width / sourceImage.Width;
                            }
                            break;
                        case "W"://指定宽，高按比例                    
                            toHeight = sourceImage.Height * width / sourceImage.Width;
                            break;
                        case "H"://指定高，宽按比例
                            toWidth = sourceImage.Width * height / sourceImage.Height;
                            break;
                        case "CUT"://指定高宽裁减（不变形）                
                            if ((double)sourceImage.Width / (double)sourceImage.Height > (double)toWidth / (double)toHeight)
                            {
                                oh = sourceImage.Height;
                                ow = sourceImage.Height * toWidth / toHeight;
                                y = 0;
                                x = (sourceImage.Width - ow) / 2;
                            }
                            else
                            {
                                ow = sourceImage.Width;
                                oh = sourceImage.Width * height / toWidth;
                                x = 0;
                                y = (sourceImage.Height - oh) / 2;
                            }
                            break;
                    }
                }

                //新建一个bmp图片
                bitmap = new Bitmap(toWidth, toHeight);

                //新建一个画板
                g = Graphics.FromImage(bitmap);

                g.CompositingQuality = CompositingQuality.HighQuality;

                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(sourceImage, new Rectangle(0, 0, toWidth, toHeight),
                    new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);

                //以下代码为保存图片时，设置压缩质量
                ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = flag;//设置压缩的比例1-100
                eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();//获取图像编码器的信息
                ImageCodecInfo jpegICIinfo = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[i];
                        break;
                    }
                }

                if (jpegICIinfo != null)
                {
                    bitmap.Save(targetPath, jpegICIinfo, ep);
                    FileInfo fiTarget = new FileInfo(targetPath);
                    if (size > 0 && fiTarget.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        MakeThumbnail(sourcePath, targetPath, width, height, mode, flag, size);
                    }
                }
                else
                {
                    //以jpg格式保存缩略图
                    bitmap.Save(targetPath, ImageFormat.Jpeg);
                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sourceImage != null)
                {
                    sourceImage.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }
                if (ep != null)
                {
                    ep.Dispose();
                }
                if (eParam != null)
                {
                    eParam.Dispose();
                }
            }
        }

    }

    /// <summary>
    /// 画笔配置
    /// </summary>
    public class SolidBrushConfig
    {
        /// <summary>
        /// 画笔颜色
        /// </summary>
        public Color Color { get; set; }
    }

    /// <summary>
    /// 字体配置
    /// </summary>
    public class FontConfig
    {
        /// <summary>
        /// 字体
        /// </summary>
        public FontFamily Family { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// 字体风格
        /// </summary>
        public FontStyle Style { get; set; }
    }

    /// <summary>
    /// 合并图片
    /// </summary>
    public class CombinImg
    {
        /// <summary>
        /// 图片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// X轴
        /// </summary>
        public int XPoint { get; set; }

        /// <summary>
        /// Y轴
        /// </summary>
        public int YPoint { get; set; }
    }

    public class SS
    {



    }
}
