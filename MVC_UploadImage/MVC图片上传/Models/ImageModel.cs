using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models
{
    public class ImageModel:BaseEntity
    {
       /// <summary>
        /// 用户Id
       /// </summary>
        public int ID { get; set; }
        /// <summary>
        ///身份证正面相对路径
        /// </summary>
        public string IDProofFront { get; set; }
        /// <summary>
        ///身份证背面相对路径
        /// </summary>
        public string IDProofBack { get; set; }
    }
}