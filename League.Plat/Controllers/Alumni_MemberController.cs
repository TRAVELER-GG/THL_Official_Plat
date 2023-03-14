using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;

namespace League.Plat.Controllers
{
    public partial class Alumni_MemberController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
    }

    //校友
    public partial class Alumni_MemberController : Controller
    {
        public ActionResult Search()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;

            Alumni_Member_Filter MF = new Alumni_Member_Filter
            {
                Is_Frozen = Request.Query["Is_Frozen"],
                Keyword_Faculty = Request.Query["Keyword_Faculty"],
                Keyword_Year = Request.Query["Keyword_Year"],
                Keyword_Province = Request.Query["Keyword_Province"],
                Keyword_City = Request.Query["Keyword_City"],
                Keyword_District = Request.Query["Keyword_District"],
            };

            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_ALL_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            Guid AMID = ID;
            Alumni_Member Item = IAU.Get_Alumni_Member_Item(AMID);
            Item.Faculty_List = IAU.Get_Faculty_List(Guid.Empty);
            return PartialView(Item);
        }

        public PartialViewResult Search_Sub_Img(Guid ID)
        {
            Guid AMID = ID;
            Alumni_Member Item = IAU.Get_Alumni_Member_Item(AMID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, Alumni_Member Item)
        {
            Guid AMID = ID;
            try
            {
                IAU.Set_Alumni_Member_Base(AMID, Item);
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
            Guid AMID = ID;
            try
            {
                IAU.Delete_Alumni_Member_Item(AMID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Search_Pre(Guid ID)
        {
            Guid AMID = ID;
            Alumni_Member Item = IAU.Get_Alumni_Member_Item(AMID);
            return PartialView(Item);
        }

        //新增校友
        public PartialViewResult Create()
        {
            Alumni_Member Item = new Alumni_Member
            {
                Faculty_List = IAU.Get_Faculty_List(Guid.Empty)
            };
            return PartialView(Item);
        }

        [HttpPost]
        public void Create_Post(Alumni_Member Item)
        {
            try
            {
                IAU.Create_Alumni_Member(Item, Guid.Empty);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }

    //校友验证
    public partial class Alumni_MemberController : Controller
    {
        public ActionResult Verify(Alumni_Member_Filter MF)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_ALL_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        [HttpPost]
        public void Verify_Post(Guid ID)
        {
            Guid AMID = ID;
            try
            {
                IAU.Change_Alumni_Member_Status(AMID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }

    //设置审核员
    public partial class Alumni_MemberController : Controller
    {
        public ActionResult Auditor(Alumni_Member_Filter MF)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_Auditor_ALL_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Auditor_Sub(Guid ID)
        {
            Guid AMID = ID;
            Alumni_Member Item = IAU.Get_Alumni_Member_Item(AMID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Auditor_Sub_Post(Guid ID, Alumni_Member Item)
        {
            Guid AMID = ID;
            try
            {
                IAU.Set_Alumni_Member_Authority_Type(AMID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}