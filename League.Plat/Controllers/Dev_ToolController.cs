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
    public partial class Dev_ToolController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}