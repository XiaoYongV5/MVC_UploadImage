using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Drawing;
using System.Configuration;
using MVC图片上传.Untity.FTP;
namespace KAB.Utility.Image
{
    public class ImageMerge
    {

        private MergeModel _merge;

        /// <summary>
        /// 身份证名称
        /// </summary>
        public string SignUrl { get; set; }
        /// <summary>
        /// 身份证背面名称
        /// </summary>
        public string SignBackUrl { get; set; }
        /// <summary>
        /// 个人签名图片名称
        /// </summary>
        public string PersignNamesUrl { get; set; }

        public ImageMerge(MergeModel model, string signUrl, string signBackUrl, string persignNamesUrl)
        {
            _merge = model;
            SignUrl = signUrl;
            SignBackUrl = signBackUrl;
            PersignNamesUrl = persignNamesUrl;
        }
        ImagesDIY imgdiy = new ImagesDIY();
        FTPUpFile ftp = new FTPUpFile();
        public string CombinImage_Mobile()
        {

            //string signnames = signname.Text.Trim();//身份证名称
            //string signnames_back = signnamefan.Text.Trim();//身份证背面名称
            //string persignnames = persignname.Text.Trim();//个人签名图片名称

            //物理路径
            //string signpath = Server.MapPath("~\\File\\") + signnames;
            //string signpath_back = Server.MapPath("~\\File\\") + signnames_back;
            //string persignpath = Server.MapPath("~\\File\\") + persignnames;


            //DownLoadFile(asign.HRef, signpath);
            //DownLoadFile(asignfan.HRef, signpath_back);
            //DownLoadFile(apersign.HRef, persignpath);

            //byte[] signData = RequestUtility.DownloadData(SignUrl);
            //byte[] signBackData = RequestUtility.DownloadData(SignBackUrl);
            //byte[] persignNamesData = RequestUtility.DownloadData(PersignNamesUrl);

            //MemoryStream signStream = new MemoryStream(signData);
            //MemoryStream signBackStream = new MemoryStream(signBackData);
            //MemoryStream persignNameStream = new MemoryStream(persignNamesData);

            string signName = string.Format("~/file/{0}_身份证正面{1}", DateTime.Now.ToString("yyyyMMddHHmm"), Path.GetExtension(SignUrl));
            string signBackName = string.Format("~/file/{0}_身份证背面{1}", DateTime.Now.ToString("yyyyMMddHHmm"), Path.GetExtension(SignBackUrl));
            string persignName = string.Format("~/file/{0}_身份证签名{1}", DateTime.Now.ToString("yyyyMMddHHmm"), Path.GetExtension(PersignNamesUrl));
            string signPath = HttpContext.Current.Server.MapPath(signName);
            string signBackPath = HttpContext.Current.Server.MapPath(signBackName);
            string persignPath = HttpContext.Current.Server.MapPath(persignName);
            var signDown = ftp.FileDown(signPath, SignUrl);
            var signBackDown = ftp.FileDown(signBackPath, SignBackUrl);
            var persignDown = ftp.FileDown(persignPath, PersignNamesUrl);

            //DownLoadFile(ReplaceUrlAuthority(asign.HRef, DownloadAuthority), signpath);
            //DownLoadFile(ReplaceUrlAuthority(asignfan.HRef, DownloadAuthority), signpath_back);
            //DownLoadFile(ReplaceUrlAuthority(apersign.HRef, DownloadAuthority), persignpath);

            int index1 = SignUrl.LastIndexOf('.');
            int index2 = SignBackUrl.LastIndexOf('.');
            int index3 = PersignNamesUrl.LastIndexOf('.');

            string signimgtype = SignUrl.Substring(index1, SignUrl.Length - index1);
            string signimgtype_back = SignBackUrl.Substring(index2, SignBackUrl.Length - index2);
            string persignimgtype = PersignNamesUrl.Substring(index3, PersignNamesUrl.Length - index3);

            string imgID_template = "~/File/KSLSignatureCard01072015.png";//背景图片模板
            string imgnewpath = "~/File/newCutID" + signimgtype;//新的压缩截图旋转后的身份证保存地址
            string imgnewpath_back = "~/File/newCutIDBack" + signimgtype_back;//新的压缩截图旋转后的身份证背面保存地址
            string imgnewperpath = "~/File/newpersign" + persignimgtype;//压缩旋转保存后的个人签名图片保存地址


            System.Drawing.Image imgIDold = System.Drawing.Image.FromFile(signPath);//原身份证图片  
            System.Drawing.Image imgIDBackold = System.Drawing.Image.FromFile(signBackPath);//原身份证背面图片  
            System.Drawing.Image imgold = System.Drawing.Image.FromFile(persignPath);//原个人签名图片

            double width1 = Convert.ToDouble(_merge.img1w);
            double heigh1 = Convert.ToDouble(_merge.img1h);
            int cutwidth = (int)(_merge.imgcutw);
            int cutheigh = (int)(_merge.imgcuth);
            int cutx = Math.Abs((int)_merge.imgcutx);
            int cuty = Math.Abs((int)_merge.imgcuty);
            Int32 angle1 = 360 - Convert.ToInt32(_merge.img1deg);
            imgdiy.CutImg(imgIDold, width1, heigh1, cutx, cuty, cutwidth, cutheigh, angle1, imgnewpath);//新的压缩截图旋转后的身份证保存


            double widthback = Convert.ToDouble(_merge.imgw_back);
            double heighback = Convert.ToDouble(_merge.imgh_back);
            int cutwidthback = (int)_merge.imgcutw_back;
            int cutheighback = (int)_merge.imgcuth_back;
            int cutxback = Math.Abs((int)_merge.imgcutx_back);
            int cutyback = Math.Abs((int)_merge.imgcuty_back);
            Int32 angleback = 360 - Convert.ToInt32(_merge.imgdeg_back);
            imgdiy.CutImg(imgIDBackold, widthback, heighback, cutxback, cutyback, cutwidthback, cutheighback, angleback, imgnewpath_back);//新的压缩截图旋转后的身份证背面保存


            Decimal w2 = _merge.img2w;
            Decimal h2 = _merge.img2h;
            Int32 angle2 = 360 - Convert.ToInt32(_merge.img2deg);
            imgdiy.KiResizeImage(imgold, Convert.ToDouble(w2), Convert.ToDouble(h2), angle2, imgnewperpath);//压缩旋转个人签名并保存


            Decimal x1 = _merge.img1x;//获取的图1x
            Decimal y1 = _merge.img1y;//获取的图1y
            Decimal py = _merge.imgpy;
            Decimal px = _merge.imgpx;
            x1 = px + x1;
            y1 = py + y1;

            Decimal xback = _merge.imgx_back;//获取的图back
            Decimal yback = _merge.imgy_back;//获取的图back
            Decimal pyback = _merge.imgpy_back;
            Decimal pxback = _merge.imgpx_back;
            xback = pxback + xback;
            yback = pyback + yback;

            Decimal x2 = _merge.img2x;//获取的图2x
            Decimal y2 = _merge.img2y;//获取的图2y
            Decimal py2 = _merge.img2py;
            Decimal px2 = _merge.img2px;
            x2 = px2 + x2;
            y2 = py2 + y2;

            string lastfilename = string.Empty;
            using (System.Drawing.Image imgonce = imgdiy.GetDrawing(
                System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgID_template)), System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgnewpath)),
                System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgnewpath_back)), x1, y1, xback, yback, _merge.pname, _merge.IdcardNo, _merge.CountryName))//新生成的背景图和身份证图
            {

                using (System.Drawing.Bitmap imgtwo1 = new Bitmap(imgdiy.GetDrowTwo(imgonce, System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgnewperpath)), x2, y2)))//新生成的身份证和签名图
                {

                    // imgtwo1.Save(HttpContext.Current.Server.MapPath("~/File/zuizhong.png"));//把文件上传到服务器的绝对路径上

                    lastfilename = ftp.SaveUploadImg(imgtwo1);

                }

            }
            ////虚拟路径

            imgIDold.Dispose();
            imgIDBackold.Dispose();
            imgold.Dispose();
            File.Delete(signPath);
            File.Delete(signBackPath);
            File.Delete(persignPath);

            return lastfilename;
        }
        public string CombinImage_PC()
        {



            //物理路径
            //byte[] signData = RequestUtility.DownloadData(SignUrl);
            //byte[] signBackData = RequestUtility.DownloadData(SignBackUrl);


            //MemoryStream signStream = new MemoryStream(signData);
            //MemoryStream signBackStream = new MemoryStream(signBackData);

            string signName = string.Format("~/file/{0}_身份证正面{1}", DateTime.Now.ToString("yyyyMMddHHmm"), Path.GetExtension(SignUrl));
            string signBackName = string.Format("~/file/{0}_身份证背面{1}", DateTime.Now.ToString("yyyyMMddHHmm"), Path.GetExtension(SignBackUrl));
            string signPath = HttpContext.Current.Server.MapPath(signName);
            string signBackPath = HttpContext.Current.Server.MapPath(signBackName);
            var signDown = ftp.FileDown(signPath, SignUrl);
            var signBackDown = ftp.FileDown(signBackPath, SignBackUrl);

            int index = SignBackUrl.LastIndexOf('.');
            string signimgtype_back = SignBackUrl.Substring(index, SignBackUrl.Length - index);
            string imgnewpath_back = "~/File/newCutIDBack" + signimgtype_back;//新的压缩截图旋转后的身份证背面保存地址


            System.Drawing.Image imgIDBackold = System.Drawing.Image.FromFile(signBackPath);//原身份证背面图片  

            double widthback = Convert.ToDouble(_merge.imgw_back);
            double heighback = Convert.ToDouble(_merge.imgh_back);
            int cutwidthback = (int)_merge.imgcutw_back;
            int cutheighback = (int)_merge.imgcuth_back;
            int cutxback = Math.Abs((int)_merge.imgcutx_back);
            int cutyback = Math.Abs((int)_merge.imgcuty_back);
            Int32 angleback = 360 - (int)_merge.imgdeg_back;

            imgdiy.CutImg(imgIDBackold, widthback, heighback, cutxback, cutyback, cutwidthback, cutheighback, angleback, imgnewpath_back);//新的压缩截图旋转后的身份证背面保存


            Decimal xback = _merge.imgx_back;//获取的图back
            Decimal yback = _merge.imgy_back;//获取的图back
            Decimal pyback = _merge.imgpy_back;
            Decimal pxback = _merge.imgpx_back;
            xback = pxback + xback;
            yback = pyback + yback;

            string lastfilename = string.Empty;
            using (System.Drawing.Image imgonce = imgdiy.GetDrawing_PC(System.Drawing.Image.FromFile(signPath), System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgnewpath_back)), xback, yback, _merge.pname, _merge.IdcardNo, _merge.CountryName))//新生成的背景图和身份证图
            {

                using (System.Drawing.Bitmap imgtwo1 = new Bitmap(imgonce))//新生成的身份证和签名图
                {

                    // imgtwo1.Save(HttpContext.Current.Server.MapPath("~/File/zuizhong.png"));//把文件上传到服务器的绝对路径上
                    lastfilename = ftp.SaveUploadImg(imgtwo1);

                }

            }
            ////虚拟路径

            imgIDBackold.Dispose();
            File.Delete(signPath);
            File.Delete(signBackPath);
            return lastfilename;
        }
    }

    public class MergeModel
    {
        public decimal img1x { get; set; }
        public decimal img1y { get; set; }
        public decimal img1deg { get; set; }
        public decimal img1w { get; set; }
        public decimal img1h { get; set; }
        public decimal imgcutw { get; set; }
        public decimal imgcuth { get; set; }
        public decimal imgcutx { get; set; }
        public decimal imgcuty { get; set; }
        public decimal imgpy { get; set; }
        public decimal imgpx { get; set; }
        public decimal imgx_back { get; set; }
        public decimal imgy_back { get; set; }
        public decimal imgdeg_back { get; set; }
        public decimal imgw_back { get; set; }
        public decimal imgh_back { get; set; }
        public decimal imgcutw_back { get; set; }
        public decimal imgcuth_back { get; set; }
        public decimal imgcutx_back { get; set; }
        public decimal imgcuty_back { get; set; }
        public decimal imgpy_back { get; set; }
        public decimal imgpx_back { get; set; }
        public decimal img2x { get; set; }
        public decimal img2y { get; set; }
        public decimal img2deg { get; set; }
        public decimal img2w { get; set; }
        public decimal img2h { get; set; }
        public decimal img2py { get; set; }
        public decimal img2px { get; set; }


        public string pname { get; set; }
        public string IdcardNo { get; set; }
        public string CountryName { get; set; }
    }
}