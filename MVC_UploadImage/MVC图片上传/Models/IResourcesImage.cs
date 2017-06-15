using MVC图片上传.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC图片上传.Models
{
    public interface IResourcesImage : IRepository<ImageModel>
    {
        bool UpdateIDProof(string IDProofFront, string IDProofBack, int pId);
        List<ImageModel> FindTo(int id);
    }
}