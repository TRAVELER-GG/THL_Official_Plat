using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace League.Plat.Controllers
{
    [Authorize]
    public partial class BusinessController : Controller
    {
        private readonly IUser_Plat_Service IUser = new User_Plat_Service();
        private User_Plat MyUser() { return IUser.Get_User_Plat_By_Controller(HttpContext.User.Identity.Name); }
        private readonly IBusiness_Service IBus = new Business_Service();
    }

    public partial class BusinessController : Controller
    {
        public ActionResult Business()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Business_Filter MF = new Business_Filter
            {
                Bus_Province = Request.Query["Bus_Province"],
                Bus_City = Request.Query["Bus_City"],
                Bus_District = Request.Query["Bus_District"],
                Bus_Type = Request.Query["Bus_Type"],
                Keyword_Bus_Name = Request.Query["Keyword_Bus_Name"],
            };
            PageList<Business> PList = IBus.Get_Business_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public ActionResult Business_Add()
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Business Item = new Business();
            return View(Item);
        }

        [HttpPost]
        public void Business_Add_Post(Business Item)
        {
            try
            {
                IBus.Create_Business(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }


        public ActionResult Business_Pre(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid BID = ID;
            Business Item = IBus.Get_Business_Item(BID);
            return View(Item);
        }

        public ActionResult Business_Sub(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid BID = ID;
            Business Item = IBus.Get_Business_Item(BID);
            return View(Item);
        }

        [HttpPost]
        public void Business_Sub_Post(Guid ID, Business Item)
        {
            Guid BID = ID;
            try
            {
                IBus.Set_Business(BID, Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Business_Sub_Delete_Post(Guid ID)
        {
            Guid BID = ID;
            try
            {
                IBus.Del_Business(BID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }


        public PartialViewResult Business_Sub_Certificate(Guid ID)
        {
            Guid BID = ID;
            Business Item = IBus.Get_Business_Item(BID);
            return PartialView(Item);
        }

        //商家相册上传
        [HttpPost]
        public void Business_Sub_Certificate_Post(Guid ID, IFormFile Img_Input)
        {
            Guid BID = ID;
            try
            {
                Business_Certificate Item = new Business_Certificate
                {
                    Cert_ID = MyGUID.NewGuid(),
                    Link_BID = BID,
                    Cert_Type = Request.Form["Cert_Type"],
                };
                Item.Cert_Img = My_Upload.Image(Img_Input, My_Upload_Sub_DIR.Business_Certificate.ToString(), Item.Cert_ID);
                IBus.Upload_Business_Certificate(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Business_Sub_Certificate_Delete_Post(Guid ID)
        {
            Guid Cert_ID = ID;
            try
            {
                Business_Certificate Item = IBus.Get_Business_Certificate_Item(Cert_ID);
                IBus.Delete_Business_Certificate(Cert_ID);
                My_Upload.Delete_File(Item.Cert_Img);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }




        public PartialViewResult Business_Sub_Album(Guid ID)
        {
            Guid BID = ID;
            Business Item = IBus.Get_Business_Item(BID);
            return PartialView(Item);
        }

        //商家相册上传
        [HttpPost]
        public void Business_Sub_Album_Post(Guid ID, IFormFile Img_Input)
        {
            Guid BID = ID;
            try
            {
                Business_Album Item = new Business_Album
                {
                    Album_ID = MyGUID.NewGuid(),
                    Link_BID = BID,
                    Album_Type = Request.Form["Album_Type"],
                };
                Item.Album_Img = My_Upload.Image(Img_Input, My_Upload_Sub_DIR.Business_Album.ToString(), Item.Album_ID);
                IBus.Upload_Business_Album(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Business_Sub_Album_Delete_Post(Guid ID)
        {
            Guid Album_ID = ID;
            try
            {
                Business_Album Item = IBus.Get_Business_Album_Item(Album_ID);
                IBus.Delete_Business_Album(Album_ID);
                My_Upload.Delete_File(Item.Album_Img);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }

    //优惠卡券
    public partial class BusinessController : Controller
    {
        public ActionResult Business_Coupon(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid Link_BID = ID;
            Business_Filter MF = new Business_Filter
            {
                Link_BID = ID,
                Bus_Province = Request.Query["Bus_Province"],
                Bus_City = Request.Query["Bus_City"],
                Coupon_Status = Request.Query["Coupon_Status"],
                Keyword_Coupon_Name = Request.Query["Keyword_Coupon_Name"],
            };

            Business Bus = IBus.Get_Business_Item(Link_BID);
            ViewData["Bus"] = Bus;

            PageList<Business_Coupon> PList = IBus.Get_Business_Coupon_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Business_Coupon_Add(Guid ID)
        {
            Business_Coupon Item = new Business_Coupon();
            Item.Link_BID = ID;
            return PartialView(Item);
        }

        [HttpPost]
        public void Business_Coupon_Add_Post(Business_Coupon Item)
        {
            try
            {
                IBus.Create_Business_Coupon(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Business_Coupon_Sub(Guid ID)
        {
            Guid Coup_ID = ID;
            Business_Coupon Item = IBus.Get_Business_Coupon_Item(Coup_ID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Business_Coupon_Sub_Post(Business_Coupon Item)
        {
            try
            {
                IBus.Set_Business_Coupon(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Business_Coupon_Sub_Delete_Post(Guid ID)
        {
            Guid Coup_ID = ID;
            try
            {
                IBus.Del_Business_Coupon(Coup_ID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }


    //优惠卡券
    public partial class BusinessController : Controller
    {
        public ActionResult Business_Dues(Guid ID)
        {
            User_Plat U = this.MyUser();
            ViewData["U"] = U;
            Guid Link_BID = ID;
            Business_Filter MF = new Business_Filter
            {
                Link_BID = ID,
            };

            Business Bus = IBus.Get_Business_Item(Link_BID);
            ViewData["Bus"] = Bus;

            PageList<Business_Dues> PList = IBus.Get_Business_Dues_PageList(MF);
            ViewData["MF"] = MF;
            return View(PList);
        }

        public PartialViewResult Business_Dues_Add(Guid ID)
        {
            Business_Dues Item = new Business_Dues();
            Item.Link_BID = ID;
            return PartialView(Item);
        }

        [HttpPost]
        public void Business_Dues_Add_Post(Business_Dues Item)
        {
            try
            {
                IBus.Create_Business_Dues(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        public PartialViewResult Business_Dues_Sub(Guid ID)
        {
            Guid Coup_ID = ID;
            Business_Dues Item = IBus.Get_Business_Dues_Item(Coup_ID);
            return PartialView(Item);
        }

        [HttpPost]
        public void Business_Dues_Sub_Post(Business_Dues Item)
        {
            try
            {
                IBus.Set_Business_Dues(Item);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }

        [HttpPost]
        public void Business_Dues_Sub_Delete_Post(Guid ID)
        {
            Guid Coup_ID = ID;
            try
            {
                IBus.Del_Business_Dues(Coup_ID);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                Response.WriteAsync(Ex.Message.ToString());
            }
        }
    }
}