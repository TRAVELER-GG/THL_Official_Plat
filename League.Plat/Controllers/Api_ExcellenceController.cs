using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using League.Api;

namespace League.Plat.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class Api_ExcellenceController : ControllerBase
    {
        private readonly IDiscovery_Service IDis = new Discovery_Service();
    }

    public partial class Api_ExcellenceController : ControllerBase
    {
        /// <summary>
        /// 微服坊搜索
        /// </summary>
        /// <param name="Keyword">服务类型关键词</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get(string Keyword)
        {
            List<Service> List = IDis.Get_Service(Keyword);
            var Res_Item = new Api_Response { Data = List };
            return Res_Item;
        }

        /// <summary>
        /// 微服坊列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Mini_Service()
        {
            Service_List Item = IDis.Get_Service_List();
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }

        /// <summary>
        /// 微服坊详情
        /// </summary>
        /// <param name="MSID">展位ID</param>
        /// <param name="UID">当前校友ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Service_Item(string MSID, string UID)
        {
            Service Item = IDis.Get_Service_Item(MSID, UID);
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }

        /// <summary>
        /// 获取已关注的微服坊列表
        /// </summary>
        /// <param name="ID">当前校友ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Service_Follow(Guid ID)
        {
            List<Service> List = IDis.Get_Follow_Service_List(ID);
            var Res_Item = new Api_Response { Data = List };
            return Res_Item;
        }

        /// <summary>
        /// 关注/取关微服坊
        /// </summary>
        /// <param name="Follow_MSID">对方企业ID</param>
        /// <param name="Fans_AMID">当前校友ID(*required)</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Service_Post(Guid Follow_MSID, Guid Fans_AMID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IDis.Follow_Service_Or_Cancel(Follow_MSID, Fans_AMID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }
    }
}