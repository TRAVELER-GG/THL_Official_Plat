using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;

namespace League.Alu.Controllers
{
    public partial class Alumni_UnionController : Controller
    {
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
        private Alumni_Union_User MyUser() { return IAU.Get_Alumni_Union_User_By_Controller(HttpContext.User.Identity.Name); }
    }

    public partial class Alumni_UnionController : Controller
    {
        public ActionResult Search()
        {
            Guid AUID = this.MyUser().Link_AUID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return View(Item);
        }

        [HttpPost]
        public void Search_Post(Guid ID, Alumni_Union Item)
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

        public ActionResult Logo_And_Background()
        {
            Guid AUID = this.MyUser().Link_AUID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return View(Item);
        }

        public ActionResult Union_Introduce()
        {
            Guid AUID = this.MyUser().Link_AUID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return View(Item);
        }

        [HttpPost]
        public void Union_Introduce_Post(Guid ID, Alumni_Union Item)
        {
            Guid AUID = ID;
            try
            {
                IAU.Set_Alumni_Union_Introduce(AUID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Union_Introduce_Pre()
        {
            Guid AUID = this.MyUser().Link_AUID;
            Alumni_Union Item = IAU.Get_Alumni_Union_Item(AUID);
            return PartialView(Item);
        }
    }
}
