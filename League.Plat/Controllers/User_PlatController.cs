using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using League.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace League.Plat.Controllers
{
    public partial class User_PlatController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class User_PlatController : Controller
    {
        public ActionResult Search(User_Plat_Filter MF)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            PageList<User_Plat> PList = IUser.Get_User_Plat_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Add()
        {
            User_Plat Item = new User_Plat();
            return PartialView(Item);
        }

        [HttpPost]
        public void Main_User_Add_Post(User_Plat Item)
        {
            try
            {
                Item.Roles_Title = Common_Lib.Convert_ToString(Request.Form["Roles_Title"]);
                IUser.Create_User(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            Guid UID = ID;
            User_Plat Item = IUser.Get_User_Plat_Item(UID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, User_Plat Item)
        {
            Guid UID = ID;
            try
            {
                Item.Roles_Title = Common_Lib.Convert_ToString(Request.Form["Roles_Title"]);
                IUser.Set_User_Item(UID, Item);
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
            Guid UID = ID;
            try
            {
                IUser.Delete_User_Item(UID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }

    public partial class User_PlatController : Controller
    {
        public ActionResult Search_Login_Record(User_Plat_Filter MF)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            PageList<User_Plat_Login_Record> PList = IUser.Get_User_Plat_Login_Record_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }
    }
}