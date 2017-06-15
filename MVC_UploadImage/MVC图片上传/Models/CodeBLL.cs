using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models
{
    public class CodeBLL
    {
        private readonly IResourcesImage _resourcesImage;
        public CodeBLL()
        {
            this._resourcesImage = new ResourcesImage();
        }
        /// <summary>
        /// 根据字段更新用户的文件资料信息
        /// </summary>
        /// <param name="fileNameField">字段</param>
        /// <param name="fileNameValue">字段值</param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public bool UpdateFileName(string IDProofFront, string IDProofBack, int pId)
        {
            bool flag = false;
            flag = _resourcesImage.UpdateIDProof(IDProofFront, IDProofBack, pId);
            return flag;
        }
        public List<ImageModel> FindTo(int id)
        {
            return _resourcesImage.FindTo(id);
        }
    }
}