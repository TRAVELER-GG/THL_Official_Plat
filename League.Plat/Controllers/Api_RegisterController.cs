using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using League.Api;
using Newtonsoft.Json;

namespace League.Plat.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class Api_RegisterController : ControllerBase
    {
        #region 2020.12.3
        /// <summary>
        /// 获取OpenID
        /// </summary>
        /// <param name="Code">前端传code，后台发送请求</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_App_User_OpenID_By_Code(string Code)
        {
            var Res_Item = new Api_Response();

            var appid = "wxae7a42770879e22d";
            var appsecret = "72e375daf0983c03d15e183fca33dfcc";
            string wx_url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&grant_type=authorization_code&js_code={2}";
            wx_url = string.Format(wx_url, appid, appsecret, Code);

            string Result = Get_And_Post.GetWebRequest(wx_url);
            Res_Item.Data = Result;
            return Res_Item;
        }
        #endregion
    }
}