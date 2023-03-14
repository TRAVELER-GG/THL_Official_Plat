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
    public class CommonController : Controller
    {
        public PartialViewResult Regional_Panel()
        {
            List<Regional> List = My_Regional.Get_Regional_By_XML();
            return PartialView(List);
        }

        public PartialViewResult Native_Place()
        {
            List<Regional> List = My_Regional.Get_Regional_By_XML();
            return PartialView(List);
        }
    }
}