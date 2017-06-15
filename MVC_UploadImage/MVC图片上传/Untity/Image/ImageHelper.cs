using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAB.Utility.Image
{
    public class ImageHelper
    {
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="sFile">原图路径</param>
        /// <param name="dFile">保存路径</param>
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="dHeight">高度</param>
        /// <returns></returns>
        public static bool SavePicThumbnail(string sFile, string dFile, int flag, int dWidth = 0, int dHeight = 0)
        {

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            return SavePicThumbnail(dFile, flag, iSource, dWidth, dHeight);


        }
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="sFile">原图流</param>
        /// <param name="dFile">保存路径</param>
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="dHeight">高度</param>
        /// <returns></returns>
        public static bool SavePicThumbnail(Stream stream, string dFile, int flag, int dWidth = 0, int dHeight = 0)
        {

            System.Drawing.Image iSource = System.Drawing.Image.FromStream(stream);
            return SavePicThumbnail(dFile, flag, iSource, dWidth, dHeight);

        }



        #region GetPicThumbnail

        public static Stream GetPicThumbnail(Stream stream ,int flag,  int dWidth = 0, int dHeight = 0)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromStream(stream);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            if (dHeight == 0 && dWidth == 0)
            {
                sW = iSource.Width;
                sH = iSource.Height;
            }
            else if (dWidth != 0)
            {
                sW = dWidth;
                sH = iSource.Height * dWidth / iSource.Width;
            }
            else if (dHeight != 0)
            {
                sH = dHeight;
                sW = iSource.Width * dHeight / iSource.Height;
            }
            Bitmap ob = new Bitmap(sW, sH);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle(0, 0, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            MemoryStream ms = new MemoryStream();    
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(ms, jpegICIinfo, ep);//dFile是压缩后的新路径

                }
                else
                {
                    ob.Save(ms, tFormat);
                }
                
                return stream;
            }
            catch
            {
                return null;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }


        public static bool SavePicThumbnail(string dFile, int flag, System.Drawing.Image iSource, int dWidth = 0, int dHeight = 0)
        {

            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            if (dHeight == 0 && dWidth == 0)
            {
                sW = iSource.Width;
                sH = iSource.Height;
            }
            else if (dWidth != 0)
            {
                sW = dWidth;
                sH = iSource.Height * dWidth / iSource.Width;
            }
            else if (dHeight != 0)
            {
                sH = dHeight;
                sW = iSource.Width * dHeight / iSource.Height;
            }
            Bitmap ob = new Bitmap(sW, sH);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle(0, 0, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        #endregion
    }
}
