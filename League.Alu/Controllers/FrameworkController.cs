using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using League.Api;

namespace League.Alu.Controllers
{
    [Authorize]
    public partial class FrameworkController : Controller
    {
        private readonly IAlumni_Union_Service IAu = new Alumni_Union_Service();
        private Alumni_Union_User MyUser() { return IAu.Get_Alumni_Union_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class FrameworkController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Top_Head()
        {
            Alumni_Union_User U = IAu.Get_Alumni_Union_User_Item(this.MyUser().UID);
            ViewData["U"] = U;
            return View();
        }

        public ActionResult _Left_Menu()
        {
            Alumni_Union_User U = this.MyUser();
            Function_Tree Item = FunTree.Get_Tree_List(U);
            return View(Item);
        }

        public ActionResult Welcome()
        {
            Alumni_Union_User U = IAu.Get_Alumni_Union_User_Item(this.MyUser().UID);
            ViewData["U"] = U;
            return View(new Alumni_Union_User());
        }
    }
}