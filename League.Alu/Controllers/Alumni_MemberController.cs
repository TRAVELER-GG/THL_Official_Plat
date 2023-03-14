using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace League.Alu.Controllers
{
    [Authorize]
    public partial class Alumni_MemberController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
    }

    //校友账号
    public partial class Alumni_MemberController : Controller
    {
        public ActionResult Search(Alumni_Member_Filter MF)
        {
            Guid Link_AUID = Guid.Empty;
            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            Guid AMID = ID;
            Guid Link_AUID = Guid.Empty;
            Alumni_Member Item = IAU.Get_Alumni_Member_Item(AMID);
            Item.Faculty_List = IAU.Get_Faculty_List(Link_AUID);
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

        //新增社交校友
        public PartialViewResult Create()
        {
            Guid Link_AUID = Guid.Empty;
            Alumni_Member Item = new Alumni_Member
            {
                Faculty_List = IAU.Get_Faculty_List(Link_AUID)
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
            MF.Faculty_List = IAU.Get_Faculty_List(MF.Link_AUID);
            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_PageList(MF);
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

    //设置副会长
    public partial class Alumni_MemberController : Controller
    {
        public ActionResult Auditor(Alumni_Member_Filter MF)
        {
            MF.Faculty_List = IAU.Get_Faculty_List(MF.Link_AUID);
            PageList<Alumni_Member> PList = IAU.Get_Alumni_Member_Auditor_PageList(MF);
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