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
    public partial class Api_BusinessController : ControllerBase
    {
        private readonly IApi_Service IApi = new Api_Service();
    }

    public partial class Api_BusinessController : ControllerBase
    {
        /// <summary>
        /// 获取吃住行数据
        /// </summary>
        /// <param name="Keyword">搜索关键词</param>
        /// <param name="Keyword_Area">商家区域</param>
        /// <param name="Keyword_Type">商家类型</param>
        /// <param name="PageIndex">当前页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Business_List(string Keyword, string Keyword_Area, string Keyword_Type, int PageIndex)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_Business(Keyword, Keyword_Area, Keyword_Type, PageIndex);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }
     
        /// <summary>
        /// 获取商家详情
        /// </summary>
        /// <param name="BID">商户ID</param>
        /// <param name="OpenID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Business_Info(string BID, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Business_Info(BID, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 领取卡券
        /// </summary>
        /// <param name="Coupon_ID">卡券ID</param>
        /// <param name="OpenID">用户OpenID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Join_Association(string Coupon_ID, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Create_Business_Coupon_Member(Coupon_ID, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 我的优惠券
        /// </summary>
        /// <param name="Keyword_Area">市区关键词</param>
        /// <param name="Keyword_Name">商家关键词</param>
        /// <param name="OpenID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Business_Coupon_Mine(string Keyword_Area, string Keyword_Name, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_Business_Coupon(Keyword_Area, Keyword_Name, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取优惠券详情
        /// </summary>
        /// <param name="Coupon_ID">优惠券ID</param>
        /// <param name="OpenID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Business_Coupon_Mine_Info(string Coupon_ID, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Business_Coupon_Member_Item(Coupon_ID, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 核销使用
        /// </summary>
        /// <param name="Cou_Mem_ID">使用券ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Cancel_Business_Coupon(string Cou_Mem_ID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Cancel_Business_Coupon_Member(Cou_Mem_ID);
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