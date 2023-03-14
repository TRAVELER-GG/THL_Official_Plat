using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;

namespace League.Plat.Controllers
{
    public partial class Alumni_UnionController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
    }

    public partial class Alumni_UnionController : Controller
    {
        public ActionResult Search(Alumni_Union_Filter MF)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            PageList<Alumni_Union> PList = IAU.Get_Alumni_Union_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid AUID = ID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return PartialView(Item);
        }

        public PartialViewResult Search_Sub_Pre(Guid ID)
        {
            Guid AUID = ID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, Alumni_Union Item)
        {
            Guid AUID = ID;
            try
            {
                IAU.Set_Alumni_Union_Base(AUID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Reset_PW_Post(Guid ID)
        {
            Guid AUID = ID;
            try
            {
                string Default_PW = Common_Lib.Convert_ToString(Request.Form["Default_Password"]);
                IAU.Reset_Alumni_Union_Password(AUID, Default_PW);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Delete(Guid ID)
        {
            try
            {
                IAU.Delete_Alumni_Union_Item(ID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Logo_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                string OLD_Logo = IAU.Get_Alumni_Union_Item(AUID).Logo;
                string Logo = My_Upload.Image(File_Input, "Logo", AUID);
                IAU.Set_Alumni_Union_Logo(AUID, Logo);
                My_Upload.Delete_File(OLD_Logo);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Logo_College_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                string OLD_Logo_College = IAU.Get_Alumni_Union_Item(AUID).Logo_College;
                string Logo_College = My_Upload.Image(File_Input, "Logo_College", AUID);
                IAU.Set_Alumni_Union_Logo_College(AUID, Logo_College);
                My_Upload.Delete_File(OLD_Logo_College);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Background_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                string OLD_Background = IAU.Get_Alumni_Union_Item(AUID).Background;
                string Background =  My_Upload.Image_Thumb(File_Input, "Background", AUID);
                IAU.Set_Alumni_Union_Background(AUID, Background);
                My_Upload.Delete_File(OLD_Background);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Background_Delete(Guid ID)
        {
            Guid AUID = ID;
            try
            {
                Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
                IAU.Set_Alumni_Union_Background(AUID, string.Empty);
                My_Upload.Delete_File(Item.Background);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }

    //新增校友分会
    public partial class Alumni_UnionController : Controller
    {
        public PartialViewResult Create()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Alumni_Union Item = new Alumni_Union();
            return PartialView(Item);
        }

        [HttpPost]
        public void Create_Post(Alumni_Union Item)
        {
            try
            {
                IAU.Create_Alumni_Union_And_Alumni(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}