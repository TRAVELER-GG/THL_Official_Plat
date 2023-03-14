using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using League.Api;
using Microsoft.AspNetCore.Http;

namespace League.Alu.Controllers
{
    [Authorize]
    public partial class ActivityController : Controller
    {
        private readonly IAlumni_Union_Service IAu = new Alumni_Union_Service();
        private Alumni_Union_User MyUser() { return IAu.Get_Alumni_Union_User_By_Controller(HttpContext.User.Identity.Name); }

        private readonly IActivity_Service IAct = new Activity_Service();
    }

    public partial class ActivityController : Controller
    {
        public ActionResult Search(Activity_Filter MF)
        {
            MF.Link_AUID = this.MyUser().Link_AUID;
            PageList<Activity> PList = IAct.Get_Activity_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Search_Sub(Guid ID)
        {
            Guid AID = ID;
            Activity Item = IAct.Get_Activity_Item(AID);
            return PartialView(Item);
        }

        public PartialViewResult Search_Sub_Poster(Guid ID)
        {
            Guid AID = ID;
            Activity Item = IAct.Get_Activity_Item(AID);
            return PartialView(Item);
        }

        public PartialViewResult Search_Pre(Guid ID)
        {
            Guid AID = ID;
            Activity Item = IAct.Get_Activity_Item(AID);
            return PartialView(Item);
        }

        public PartialViewResult Search_People(Guid ID, Activity_Filter MF)
        {
            Guid AID = ID;
            MF.Link_AID = AID;
            PageList<Activity_Apply> PList = IAct.Get_Activity_Apply_PageList(MF);
            ViewData["MF"] = MF;
            return PartialView(PList);
        }

        [HttpPost]
        public void Search_Sub_Post(Guid ID, Activity Item)
        {
            Guid AID = ID;
            try
            {
                IAct.Set_Activity(AID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Search_Sub_Add_Post(Guid ID, Activity Item)
        {
            Guid AID = ID;
            try
            {
                IAct.Set_Activity(AID, Item);
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
            Guid AID = ID;
            try
            {
                IAct.Del_Activity(AID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public ActionResult Publish()
        {
            Activity Item = new Activity();
            return View(Item);
        }

        [HttpPost]
        public void Publish_Post(Activity Item)
        {
            Guid Link_UID = this.MyUser().UID;
            try
            {
                IAct.Create_Activity(Link_UID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Publish_Save_Post(Activity Item)
        {
            Guid Link_UID = this.MyUser().UID;
            try
            {
                IAct.Save_Activity(Link_UID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}