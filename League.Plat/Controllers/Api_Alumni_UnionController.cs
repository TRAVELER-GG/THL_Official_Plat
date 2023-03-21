using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using League.Api;

namespace League.Plat.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class Api_Alumni_UnionController : ControllerBase
    {
        private readonly IApi_Service IApi = new Api_Service();
    }

    public partial class Api_Alumni_UnionController : ControllerBase
    {
        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> News_List(int pageindex)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_News_List(pageindex);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取新闻详情
        /// </summary>
        /// <param name="ID">新鲜事ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> News(Guid ID)
        {
            News Item = IApi.Get_News_Item(ID);
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="OpenID">OpenID</param>
        /// <param name="PageIndex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Activity_List(string OpenID, int PageIndex)
        {
            List<Alu_Activity> List = IApi.Get_Activity_List(OpenID, PageIndex).ToList();
            var Res_Item = new Api_Response { Data = List };
            return Res_Item;
        }

        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="AID">活动ID</param>
        /// <param name="AMID">校友ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Activity(string AID, string AMID)
        {
            Alu_Activity Item = IApi.Get_Activity_Item(AID, AMID);
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }
    }
}