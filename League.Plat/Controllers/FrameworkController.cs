using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using League.Api;

namespace League.Plat.Controllers
{
    [Authorize]
    public partial class FrameworkController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class FrameworkController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Top_Head()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            return View();
        }

        public ActionResult _Left_Menu()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Function_Tree Item = FunTree.Get_Tree_List(U);
            return View(Item);
        }

        public ActionResult Welcome()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            return View(new User_Plat());
        }
    }
}