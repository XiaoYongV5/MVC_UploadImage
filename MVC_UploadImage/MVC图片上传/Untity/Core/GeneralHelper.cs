using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC图片上传.Untity.Core
{
    public class GeneralHelper
    {
        //private static string[] _extensionList = { ".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff" };
        public GeneralHelper() { }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMixPwd(int num)//生成混合随机数
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成随机数不包含0跟O
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMT4pwd(int num)
        {
            string a = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetMixPWD(int num)//生成混合随机数
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_!@#$%&";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }
        //随机字符串生成器的主要功能如下： 
        //1、支持自定义字符串长度
        //2、支持自定义是否包含数字
        //3、支持自定义是否包含小写字母
        //4、支持自定义是否包含大写字母
        //5、支持自定义是否包含特殊符号
        //6、支持自定义字符集
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，1=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string GetRndstr(int length, string custom = "", bool useNum = true, bool useLow = true, bool useUpp = false, bool useSpe = false)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "23456789"; }
            if (useLow == true) { str += "abcdefghijkmnpqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        /// <summary>
        /// 检查上传文件的后缀名
        /// 允许上传的有：".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff"
        /// </summary>
        /// <param name="fileName">待检查的文件完整名称</param>
        /// <returns></returns>
        public static bool CheckFileExtension(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                fileName = fileName.ToLower();
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                    return false;
                string[] _extensionList = { ".jpg", ".gif", ".bmp", ".doc", ".docx", ".jpeg", ".png", ".tif", ".tiff" };
                if (!_extensionList.Contains(extension))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查上传文件的后缀名
        /// 允许上传的有：“.jpg|.gif|.bmp|.jpeg|.png”
        /// </summary>
        /// <param name="fileName">待检查的文件完整名称</param>
        /// <returns></returns>
        public static bool CheckIsImgExtension(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                fileName = fileName.ToLower();
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                    return false;
                string[] _extensionList = { ".jpg", ".gif", ".bmp", ".jpeg", ".png" };
                if (!_extensionList.Contains(extension))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}