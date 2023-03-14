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

namespace League.Plat.Controllers
{
    public partial class LoginController : Controller
    {
        private readonly IUser_Plat_Service IU = new User_Plat_Service();
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
            User_Plat U_Login = new User_Plat
            {
                Name = Request.Form["User_Name"],
                Password = Request.Form["Password"]
            };

            Thread.Sleep(500);
            try
            {
                string Last_Login_IP = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Client_Info = HttpContext.Request.Headers["user-agent"];

                User_Plat Item = IU.Get_User_Plat_Login(U_Login, Last_Login_IP, Client_Info);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Item.UID.ToString()),
                    new Claim(ClaimTypes.Email, Item.Email.ToString()),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                HttpContext.SignInAsync(new ClaimsPrincipal(identity), authProperties);
                return RedirectToAction("Index", "Framework");
            }
            catch (Exception Ex)
            {
                TempData["Error_Login"] = Ex.Message.ToString();
                TempData["User_Name"] = U_Login.Name;
                TempData["Password"] = U_Login.Password;
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