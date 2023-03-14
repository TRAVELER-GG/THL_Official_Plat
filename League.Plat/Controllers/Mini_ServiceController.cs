using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace League.Plat.Controllers
{
    [Authorize]
    public partial class Mini_ServiceController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IDiscovery_Service IDis = new Discovery_Service();
    }

    public partial class Mini_ServiceController : Controller
    {
        public ActionResult Search(Mini_Service_Filter MF)
        {
            PageList<Mini_Service> PList = IDis.Get_Mini_Service_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            Guid MSID = ID;
            Mini_Service Item = IDis.Get_Mini_Service_Item(MSID);
            List<string> List = IDis.Type_List();
            ViewData["List"] = List;
            return PartialView(Item);
        }

        public PartialViewResult Search_Sub_Cover_Img(Guid ID)
        {
            Guid MSID = ID;
            Mini_Service Item = IDis.Get_Mini_Service_Item(MSID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Cover_Img_Post(Guid ID, IFormFile File_Input)
        {
            Guid MSID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Cover_Img = IDis.Get_Mini_Service_Item(MSID).Cover_Img;
                string Cover_Img = My_Upload.Image_Thumb(File_Input, "Cover_Img", MSID);
                IDis.Set_Mini_Service_Cover_Img(MSID, Cover_Img);
                if (OLD_Cover_Img != Cover_Img) { My_Upload.Delete_File(OLD_Cover_Img); }
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Search_Pre(Guid ID)
        {
            Guid MSID = ID;
            Mini_Service Item = IDis.Get_Mini_Service_Item(MSID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, Mini_Service Item)
        {
            Guid MSID = ID;
            try
            {
                IDis.Set_Mini_Service(MSID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Add_Post(Guid ID, Mini_Service Item)
        {
            Guid MSID = ID;
            try
            {
                IDis.Set_Mini_Service(MSID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Delete_Post(Guid ID)
        {
            Guid MSID = ID;
            try
            {
                IDis.Del_Mini_Service(MSID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
        
        public ActionResult Publish()
        {
            Mini_Service Item = new Mini_Service();
            List<string> List = IDis.Type_List();
            ViewData["List"] = List;
            return View(Item);
        }

        [HttpPost]
        public void Publish_Post(Mini_Service Item)
        {
            try
            {
                IDis.Create_Mini_Service(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Publish_Save_Post(Mini_Service Item)
        {
            try
            {
                IDis.Save_Mini_Service(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}