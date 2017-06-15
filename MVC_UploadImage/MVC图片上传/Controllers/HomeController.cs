using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC图片上传.Models;

namespace MVC图片上传.Controllers
{
    public class HomeController : Controller
    {
        public string ipaddress = ConfigurationManager.AppSettings["IPaddress"].ToString();
        public string _pathSrc = ConfigurationManager.AppSettings["PathSrc"].ToString();
        public string ImageType = ConfigurationManager.AppSettings["FileType"].ToString();
        CodeBLL ms = new CodeBLL();
        public ActionResult Index()
        {
            var model = ms.FindTo(1);
            ImageModel m = new ImageModel();
            foreach (var item in model)
            {
                m.ID = item.ID;
                m.IDProofFront = item.IDProofFront;
                m.IDProofBack = item.IDProofBack;
            }
            ViewBag.pathSrc = _pathSrc;
            return View(m);
        }

        [HttpPost]
        public ActionResult UpLoadImage(ImageModel entity)
        {
            try
            {
                     var falg = ms.UpdateFileName(entity.IDProofFront, entity.IDProofBack, entity.ID);
                    if (falg)
                    {
                       // return Json(new { Result = "1", MSG = "成功上传一张身份信息" });
                        return  Content("<script>alert('上传成功');location.href='/Home/Index'</script>"); 
                    }
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "0", MSG = "操作失败," + ex.Message });
                return Content("<script>alert('操作失败');location.href='/Home/Index'</script>"); 
            }
           // return Json(new { Result = "0", MSG = "上传失败" });
            return Content("<script>alert('操作失败');location.href='/Home/Index'</script>"); 
        }

    }
}
