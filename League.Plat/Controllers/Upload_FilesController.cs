using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using League.Api;

namespace League.Plat.Controllers
{
    public partial class Upload_FilesController : Controller
    {
        private readonly IInformation_Service IInfor = new Information_Service();
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
        private readonly IActivity_Service IAct = new Activity_Service();
    }

    public partial class Upload_FilesController : Controller
    {
        public void Information_Cover_Img_Post(Guid ID, IFormFile File_Input)
        {
            Guid IMID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Cover_Img = IInfor.Get_Information_Item(IMID).Cover_Img;
                string Cover_Img = My_Upload.Image_Thumb(File_Input, "Cover_Img", IMID);

                IInfor.Set_Information_Cover_Img(IMID, Cover_Img);
                if (OLD_Cover_Img != Cover_Img) { My_Upload.Delete_File(OLD_Cover_Img); }
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public void Member_Img_Post(Guid ID, IFormFile File_Input)
        {
            Guid AMID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Img = IAU.Get_Alumni_Member_Item(AMID).Img;
                string Img = My_Upload.Image_Thumb(File_Input, "Head_Photo", AMID);
                IAU.Set_Alumni_Member_Img(AMID, Img);
                if (OLD_Img != Img) { My_Upload.Delete_File(OLD_Img); }
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public void Enterprise_Logo_Post(Guid ID, IFormFile File_Input)
        {
            Guid MEID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Logo = IAU.Get_Alumni_Member_Enterprise_Item(MEID).Logo;
                string Logo = My_Upload.Image(File_Input, "Enterprise_Logo", MEID);
                IAU.Set_Alumni_Member_Enterprise_Logo(MEID, Logo);
                if (OLD_Logo != Logo) { My_Upload.Delete_File(OLD_Logo); }
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public void Activity_Poster_Post(Guid ID, IFormFile File_Input)
        {
            Guid AID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Poster = IAct.Get_Activity_Item(AID).Poster;
                string Poster = My_Upload.Image_Thumb(File_Input, "Cover_Img", AID);
                IAct.Set_Activity_Poster(AID, Poster);
                if (OLD_Poster != Poster) { My_Upload.Delete_File(OLD_Poster); }
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Logo_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
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
        public void Logo_College_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
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
        public void Background_Post(Guid ID, IFormFile File_Input)
        {
            Guid AUID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                string OLD_Background = IAU.Get_Alumni_Union_Item(AUID).Background;
                string Background = My_Upload.Image_Thumb(File_Input, "Background", AUID);
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
        public void Background_Delete(Guid ID)
        {
            Guid AUID = ID;
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
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
}