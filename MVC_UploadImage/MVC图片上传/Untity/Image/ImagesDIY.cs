using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAB.Utility.Image
{
    using System.Drawing;
    using System.Web;
    public class ImagesDIY
    {
        private double _tWidth;   //设置缩略图初始宽度
        private double _tHeight;  //设置缩略图初始高度

        public ImagesDIY()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }

        //计算缩略图的宽度和高度_tWidth，_tHeight
        //参数：oWidth为图片原始宽度，oHeight为图片原始高度
        public void bili(double oWidth, double oHeight, double newW, double newH)
        {
            //按比例计算出缩略图的宽度和高度
            if (oWidth >= oHeight)
            {
                _tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(newW) / Convert.ToDouble(oWidth)));
                _tWidth = newW;
            }
            else
            {
                _tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(newH) / Convert.ToDouble(oHeight)));
                _tHeight = newH;
            }
            //System.Web.HttpContext.Current.Response.Write("宽度：" + _tWidth + "  高度：" + _tHeight + "<br>");
        }

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public Bitmap KiResizeImages(Bitmap bmp, double newW, double newH)
        {
            try
            {
                Bitmap b = new Bitmap(Convert.ToInt32(newW), Convert.ToInt32(newH));
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 高质量
                g.SmoothingMode = SmoothingMode.HighQuality;

                g.DrawImage(bmp, new Rectangle(0, 0, Convert.ToInt32(newW), Convert.ToInt32(newH)), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        ImageRotate imgr = new ImageRotate();
        /// <summary>
        ///  缩放 摆放角度
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="savaPath"></param>
        /// <param name="newW"></param>
        /// <param name="newH"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public void KiResizeImage(Image imageUrl, double newW, double newH, int angle, string imgbackpath)
        {
            System.Drawing.Image image = imageUrl;
            bili(image.Width, image.Height, newW, newH);//比例缩放

            Bitmap NBM = new System.Drawing.Bitmap(image, image.Width, image.Height);

            using (image = KiResizeImages(NBM, _tWidth, _tHeight))
            {
                Bitmap bitrotate = new Bitmap(image);
                using (Bitmap afterrotate = imgr.Rotate(bitrotate, angle))
                {

                    afterrotate.Save(HttpContext.Current.Server.MapPath(imgbackpath));
                    NBM.Dispose();
                    bitrotate.Dispose();
                }
            }

        }
        /// <summary>
        /// 从指定路径中获取图片，按照指定位置及大小截取相应的图片内容，并保存到指定路径下
        /// </summary>
        /// <param name="filepath">图片来源路径及文件名(已使用Server.MapPath)</param>
        /// <param name="cutX">裁减的起始X轴坐标</param>
        /// <param name="cutY">裁减的起始Y坐标</param>
        /// <param name="cutwidth">裁减的宽度</param>
        /// <param name="cutheight">裁减的高度</param>
        /// <param name="savepath">裁减后的图片名称，路径为上一级的images文件夹中</param>
        /// <param name="context">所有http特定的信息对象</param>
        public void CutImg(Image imageUrl, double pageWith, double pageHeight, int cutX, int cutY, int cutwidth, int cutheight, int angle, string savepath)
        {
            /*
             * 思路整理（暂时未对图片文件进行MIME类型判断）：
             *   1.判断要操作的文件是否为指定类型图片
             *   2.根据指定图片的实际大小创建图像对象
             *   3.根据原图对象创建画布
             *   4.创建矩形对象，在原始图像上标识截取部分
             *   5.创建新图像对象，大小为指定裁减的宽、高
             *   6.根据新图像对象创建新画布
             *   7.清空新画布背景，可选
             *   8.创建一个矩形对象，作用是控制按照上个矩形裁减出来的图像在新画布上显示的位置及宽高
             *   9.使用画布的DrawImage方法，执行裁减操作
             *   10.设置新图的文件名，并保存到指定位置
            */

            //TODO 判断文件类型暂时未做


            //创建图像对象，由于web中有个image控件，会导致这个图像的类重复，需要带上使用命名空间
            System.Drawing.Image oldImage = imageUrl;
            bili(oldImage.Width, oldImage.Height, pageWith, pageHeight);//比例缩放

            Bitmap NBM = new System.Drawing.Bitmap(oldImage, oldImage.Width, oldImage.Height);

            using (oldImage = KiResizeImages(NBM, _tWidth, _tHeight))
            {
                //创建一个指定宽高的图像对象
                System.Drawing.Image newImage = new Bitmap(cutwidth, cutheight);
                Graphics newGraphics = Graphics.FromImage(newImage);
                //创建矩形对象，裁减是就是照着这个矩形来裁减的
                Rectangle CutReatangle = new Rectangle(cutX, cutY, cutwidth, cutheight);
                //创建矩形对象，用于下面的指定裁减出来的图像在新画布上的显示情况
                Rectangle showRectangle = new Rectangle(0, 0, cutwidth, cutheight);
                //执行裁减操作
                newGraphics.DrawImage(oldImage, showRectangle, CutReatangle, GraphicsUnit.Pixel);

                Bitmap bitrotate = new Bitmap(newImage);
                using (Bitmap afterrotate = imgr.Rotate(bitrotate, angle))
                {
                    try
                    {
                        //保存新图到指定路径
                        afterrotate.Save(HttpContext.Current.Server.MapPath(savepath), System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        afterrotate.Dispose();
                        //释放新图像的资源，如果在保存前释放，会造成程序出错
                        newImage.Dispose();
                        //释放资源（除图像对象的资源）
                        NBM.Dispose();
                        newGraphics.Dispose();
                        bitrotate.Dispose();
                    }
                }


            }


            #region 使用内存流测试显示图片
            //MemoryStream ms = new MemoryStream();
            //newImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //context.Response.Clear();
            //context.Response.ContentType = "image/jpg";
            //context.Response.BinaryWrite(ms.ToArray());
            //ms.Dispose();
            #endregion
        }
        /// <summary>
        /// 有背景图
        /// </summary>
        /// <param name="background"></param>
        /// <param name="aa"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public System.Drawing.Image GetDrawing(System.Drawing.Image background, System.Drawing.Image aa, System.Drawing.Image aback, decimal x1, decimal y1, decimal xback, decimal yback, string pname, string cardid, string country)
        {
            //在模板背景上加姓名身份证号

            System.Drawing.Image imgbackground = new Bitmap(background, 1240, 1754);

            using (System.Drawing.Bitmap imgBack = new System.Drawing.Bitmap(aa))//引用切图后的身份证图片  
            {
                using (System.Drawing.Bitmap imgaaback = new System.Drawing.Bitmap(aback))
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgbackground))
                    {

                        g.DrawString(pname, new Font("Microsoft YaHei", 25), new SolidBrush(Color.Black), new PointF(375, 248));
                        g.DrawString(cardid, new Font("Microsoft YaHei", 25), new SolidBrush(Color.Black), new PointF(375, 300));
                        g.DrawString(country, new Font("Microsoft YaHei", 25), new SolidBrush(Color.Black), new PointF(375, 348));


                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        g.DrawImage(aa, Convert.ToInt32(x1), Convert.ToInt32(y1), aa.Width, aa.Height);
                        g.DrawImage(aback, Convert.ToInt32(xback), Convert.ToInt32(yback), aback.Width, aback.Height);

                        aa.Dispose();
                        background.Dispose();
                        aback.Dispose();
                    }
                }
            }

            return imgbackground;
        }

        /// <summary>
        /// PC
        /// </summary>
        /// <param name="background"></param>
        /// <param name="aa"></param>
        /// <param name="aback"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="xback"></param>
        /// <param name="yback"></param>
        /// <param name="pname"></param>
        /// <param name="cardid"></param>
        /// <param name="country"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public System.Drawing.Image GetDrawing_PC(System.Drawing.Image background, System.Drawing.Image aback, decimal xback, decimal yback, string pname, string cardid, string country)
        {
            //在模板背景上加姓名身份证号

            System.Drawing.Image imgbackground = new Bitmap(background, 1240, 1754);

            //using (System.Drawing.Bitmap imgBack = new System.Drawing.Bitmap(aa))//引用切图后的身份证图片  
            //{
            using (System.Drawing.Bitmap imgaaback = new System.Drawing.Bitmap(aback))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgbackground))
                {

                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    //g.DrawImage(aa, Convert.ToInt32(x1), Convert.ToInt32(y1), aa.Width, aa.Height);//身份证正面
                    g.DrawImage(aback, Convert.ToInt32(xback), Convert.ToInt32(yback), aback.Width, aback.Height);//方面
                    aback.Dispose();
                    //aa.Dispose();
                    background.Dispose();
                }
            }
            // }

            return imgbackground;
        }

        /// <summary>
        /// 透明背景图
        /// </summary>
        /// <param name="aa"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="angle1"></param>
        /// <returns></returns>
        public System.Drawing.Image GetDrewOnce(System.Drawing.Image aa, decimal x1, decimal y1, System.Drawing.Image bb, decimal xback, decimal yback, int angle1)
        {
            System.Drawing.Image imgblank = new Bitmap(1200, 800);

            System.Drawing.Bitmap imgBack = new System.Drawing.Bitmap(aa);//引用压缩后身份证图片  
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(bb);//引用压缩后身份证背面图片  
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(imgblank);

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(aa, Convert.ToInt32(x1), Convert.ToInt32(y1), aa.Width, aa.Height);
            g.DrawImage(bb, Convert.ToInt32(xback), Convert.ToInt32(yback), bb.Width, bb.Height);
            g.Dispose();
            imgBack.Dispose();
            img.Dispose();
            return imgblank;
        }

        public System.Drawing.Image GetDrowTwo(System.Drawing.Image imgblank, System.Drawing.Image bb, decimal x2, decimal y2)
        {
            System.Drawing.Image imgblank1 = new System.Drawing.Bitmap(imgblank);
            using (System.Drawing.Image bb2 = new System.Drawing.Bitmap(bb))
            {
                using (System.Drawing.Graphics gg = System.Drawing.Graphics.FromImage(imgblank1))
                {
                    gg.CompositingQuality = CompositingQuality.HighQuality;
                    gg.SmoothingMode = SmoothingMode.HighQuality;
                    gg.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Rectangle srcRect = new Rectangle(0, 0, Convert.ToInt32(bb.Width), Convert.ToInt32(bb.Height));
                    // GraphicsUnit units = GraphicsUnit.Pixel;

                    int newx2 = Convert.ToInt32(Math.Round(x2, 0));
                    int newy2 = Convert.ToInt32(Math.Round(y2, 0));
                    gg.DrawImage(bb2, newx2, newy2, Convert.ToInt32(bb.Width), Convert.ToInt32(bb.Height));
                    imgblank.Dispose();
                    bb.Dispose();
                }
            }

            return imgblank1;
        }

    }
}
