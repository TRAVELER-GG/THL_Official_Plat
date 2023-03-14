using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using League.Api;
using Microsoft.AspNetCore.Http;

namespace League.Alu.Controllers
{
    [Authorize]
    public partial class InformationController : Controller
    {
        private readonly IAlumni_Union_Service IAu = new Alumni_Union_Service();
        private Alumni_Union_User MyUser() { return IAu.Get_Alumni_Union_User_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IInformation_Service IInfor = new Information_Service();
    }

    public partial class InformationController : Controller
    {
        public ActionResult Search(Information_Filter MF)
        {
            MF.Link_AUID = this.MyUser().Link_AUID;
            PageList<Information> PList = IInfor.Get_Information_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
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
            Information Item = new Information();
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