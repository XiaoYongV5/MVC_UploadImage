using KAB.Utility.Image;
using MVC图片上传.Untity.FTP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC图片上传.Controllers
{
    public class FileUploadController : Controller
    {
        private static string _fileExtension = ConfigurationManager.AppSettings["FileType"];
        private readonly string _filePath = ConfigurationManager.AppSettings["UploadPath"];
        private readonly string _fileSize = ConfigurationManager.AppSettings["FileSizem"];

        /// <summary>
        /// 通过ftp上传指定服务器
        /// </summary>
        /// <returns></returns>
        public ActionResult FileUpLoad()
        {

            bool flag = false;
            string msg = string.Empty;
            int size = Convert.ToInt16(_fileSize) * 1024 * 1024;

            try
            {
                Dictionary<string, string> fileDict = new Dictionary<string, string>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    string extension = Path.GetExtension(file.FileName);
                    string[] fileExtensions = _fileExtension.Split(';');
                    if (fileExtensions.Any(o => o.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (file.ContentLength <= size)
                        {
                            string fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetFileName(file.FileName));
                            if (file.ContentLength <= 10 * 1024 * 1024)
                            {
                                byte[] buffer = new byte[file.ContentLength];
                                file.InputStream.Read(buffer, 0, file.ContentLength);
                                flag = FileUpLoad(buffer, file.FileName, out fileName, out msg);
                            }
                            else//图片压缩有问题>>
                            {
                                var stream = ImageHelper.GetPicThumbnail(file.InputStream, 40);
                                byte[] buffer = new byte[stream.Length];
                                stream.Read(buffer, 0, (int)stream.Length);
                                flag = FileUpLoad(buffer, file.FileName, out fileName, out msg);
                            }
                            fileDict.Add(Request.Files.AllKeys[i], fileName);
                        }
                        else
                        {
                            msg = string.Format("上传文件不能大于{0}M", _fileSize);
                        }
                    }
                    else
                    {
                        msg = string.Format("上传的文件类型不正确");
                    }
                }
                return Json(new { Result = "0", MSG = "" + msg, Data = fileDict });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "0", MSG = "网络异常，请稍后再试" });
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="originalName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected bool FileUpLoad(byte[] fileBytes, string originalName, out string newFileName, out string msg)
        {
            msg = "";
            newFileName = "";
            try
            {

                FTPUpFile ftp = new FTPUpFile();
                newFileName = ftp.UpFile(fileBytes, originalName);
                if (string.IsNullOrEmpty(newFileName))
                {
                    msg = "上传文件时出错！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
    }
}
