using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using League.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading;

namespace League.Alu.Controllers
{
    public partial class LoginController : Controller
    {
        private readonly IAlumni_Union_Service IAU = new Alumni_Union_Service();
    }

    public partial class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Login_Post()
        {
            Alumni_Union_User A_Login = new Alumni_Union_User
            {
                User_Name = Request.Form["User_Name"],
                Password = Request.Form["Password"]
            };
            string PinCode = Request.Form["PinCode"];
            Thread.Sleep(500);
            try
            {
                string Last_Login_IP = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Client_Info = HttpContext.Request.Headers["user-agent"];

                Alumni_Union_User Item = IAU.Alumni_Union_User_Login(A_Login, PinCode, Last_Login_IP, Client_Info);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Item.UID.ToString()),
                    new Claim(ClaimTypes.Email, Item.Email.ToString()),
                    new Claim(ClaimTypes.IsPersistent, true.ToString())
                };
                var identity = new ClaimsIdentity(claims, "User");
                HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Framework");
            }
            catch (Exception Ex)
            {
                TempData["Error_Login"] = Ex.Message.ToString();
                TempData["User_Name"] = A_Login.User_Name;
                TempData["Password"] = A_Login.Password;
                TempData["PinCode"] = PinCode;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public RedirectToActionResult Login_Out()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}