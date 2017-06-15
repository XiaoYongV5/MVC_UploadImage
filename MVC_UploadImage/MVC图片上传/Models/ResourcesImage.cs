using MVC图片上传.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models
{
    public class ResourcesImage : BaseRepository<ImageModel>, IResourcesImage
    {
        public ResourcesImage() { }
        /// <summary>
        /// 上传身份信息采用此种方式
        /// </summary>
        /// <param name="IDProofBack"></param>
        /// <param name="IDProofBack"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public bool UpdateIDProof(string IDProofFront, string IDProofBack, int pId)
        {
            int flag = 0;

            if (IDProofFront != "" && IDProofFront != null)
            {
                flag = this.Update(m => m.ID == pId, u => new ImageModel { IDProofFront = IDProofFront });
                if (flag == 1)
                {
                    if (IDProofBack != "" && IDProofBack != null)
                        flag = this.Update(m => m.ID == pId, u => new ImageModel { IDProofBack = IDProofBack });
                }
            }
            else
            {
                if (IDProofBack != "" && IDProofBack != null)
                    flag = this.Update(m => m.ID == pId, u => new ImageModel { IDProofBack = IDProofBack });
            }
            return flag == 0 ? false : true;
        }

        public List<ImageModel> FindTo(int id)
        {
            try
            {
                var query =
             from m in Table
             where m.ID == id
             select m;
                return query.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}