using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace League.Plat.Controllers
{
    public class Editor_UploadController : Controller
    {
        public JsonResult Image(IFormFileCollection formFiles)
        {
            Editor_Result Item = new Editor_Result();
            if (formFiles == null)
            {
                throw new Exception("formFiles is null");
            }

            foreach (var formFile in formFiles)
            {
                Item.data.Add(My_Upload.Image_Thumb(formFile, "Images", MyGUID.NewGuid()));
            }
            Item.errno = 0;

            //解决富文本编辑跨域报错问题
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return Json(Item);
        }
    }

    public class Editor_Result
    {
        public Editor_Result()
        {
            errno = 0;
            data = new List<string>();
        }
        public int errno { get; set; }
        public List<string> data { get; set; }
    }
}