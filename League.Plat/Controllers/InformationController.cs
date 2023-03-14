using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace League.Plat.Controllers
{
    [Authorize]
    public partial class InformationController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IInformation_Service IInfor = new Information_Service();
    }

    public partial class InformationController : Controller
    {
        public ActionResult Search()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;

            Information_Filter MF = new Information_Filter
            {
                Link_AUID = Common_Lib.Convert_GUID(Request.Query["Link_AUID"]),
                Classify = Request.Query["Classify"],
                Keyword_Title = Request.Query["Keyword_Title"],
                Infor_Status = Request.Query["Infor_Status"],
            };

            if (string.IsNullOrEmpty(Request.Query["Link_AUID"]))
            {
                Guid ALL_Info = new Guid("00000000-0000-0000-0000-000000000001");
                MF.Link_AUID = ALL_Info;
            }

            PageList<Information> PList = IInfor.Get_Information_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid IMID = ID;
            Information Item = IInfor.Get_Information_Item(IMID);
            return PartialView(Item);
        }

        public PartialViewResult Search_Sub_Cover_Img(Guid ID)
        {
            Guid IMID = ID;
            Information Item = IInfor.Get_Information_Item(IMID);
            return PartialView(Item);
        }

        public PartialViewResult Search_Pre(Guid ID)
        {
            Guid IMID = ID;
            Information Item = IInfor.Get_Information_Item(IMID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, Information Item)
        {
            Guid IMID = ID;
            try
            {
                IInfor.Set_Information(IMID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Add_Post(Guid ID, Information Item)
        {
            Guid IMID = ID;
            try
            {
                IInfor.Set_Information(IMID, Item);
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
            Guid IMID = ID;
            try
            {
                IInfor.Del_Information(IMID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public ActionResult Publish()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Information Item = IInfor.Get_Information_Empty();
            return View(Item);
        }

        [HttpPost]
        public void Publish_Post(Information Item)
        {
            Guid UID = this.MyUser().UID;
            try
            {
                IInfor.Create_Information(UID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Publish_Save_Post(Information Item)
        {
            Guid UID = this.MyUser().UID;
            try
            {
                IInfor.Save_Information(UID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}