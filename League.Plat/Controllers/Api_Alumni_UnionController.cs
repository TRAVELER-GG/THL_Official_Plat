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
        /// 获取分会列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_App_Association()
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_Association();
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取当前分会详情
        /// </summary>
        /// <param name="AUID">分会ID</param>
        /// <param name="OpenID">用户OpenID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Association_Info(string AUID, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Association_Item(AUID, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 加入校友分会
        /// </summary>
        /// <param name="AUID"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Join_Association(string AUID, string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Create_Alumni_Union_Member_Link(AUID, OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 浏览量+1
        /// </summary>
        /// <param name="PV_Type">浏览页类型(Information、Activity、Business、Alumni_Union)</param>
        /// <param name="ID">传入不同页面的ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Add_PV(string PV_Type, string ID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Set_Page_View(PV_Type, ID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 搜索标签
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Mark">Member(校友)、Member_Company(校企)、Mini(微服坊)、Business(商家)</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Label(string OpenID, string Mark)
        {
            Lable_List Item = IApi.Get_Lable_List(OpenID, Mark);
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }


        /// <summary>
        /// 搜索校友
        /// </summary>
        /// <param name="OpenID">OpenID</param>
        /// <param name="Keyword">校友姓名/院系/行业/籍贯/兴趣关键词</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Alumnus_List(string OpenID, string Keyword)
        {
            List<Alumnus> List = IApi.Get_Alumnus_Search(OpenID, Keyword);
            var Res_Item = new Api_Response { Data = List };
            return Res_Item;
        }

        /// <summary>
        /// 查看其他校友资料
        /// </summary>
        /// <param name="AMID">查询校友ID</param>
        /// <param name="UID">当前用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Member(string AMID, string UID)
        {
            Member Item = IApi.Get_Member_Other(AMID, UID);
            var Res_Item = new Api_Response { Data = Item };
            return Res_Item;
        }

        /// <summary>
        /// 关注/取关校友
        /// </summary>
        /// <param name="Follow_AMID">对方校友ID</param>
        /// <param name="Fans_AMID">当前校友ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Follow_Member(string Follow_AMID, string Fans_AMID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Follow_Member_Or_Cancel(Follow_AMID, Fans_AMID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }


        /// <summary>
        /// 获取新鲜事列表
        /// </summary>
        /// <param name="ID">关联校友会ID</param>
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
        /// 获取新鲜事详情
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

        /// <summary>
        /// 一键报名
        /// </summary>
        /// <param name="ID">活动ID</param>
        /// <param name="AMID">校友ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Activity_Post(string ID, string AMID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Create_Activity_Apply(ID, AMID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取首页信息
        /// </summary>
        /// <param name="openid">用户的openid</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_App_Home(string openid, int pageindex)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_Home(openid, pageindex);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取校友企业列表
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="keyword">企业名称/行业关键词</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Company_List(string openid, string keyword, int pageindex)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Enterprise_List(openid, keyword, pageindex);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取企业资料详情
        /// </summary>
        /// <param name="MEID">企业ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Company(string MEID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Member_Enterprise_Item(MEID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="openid">openid有值可获取用户资料，无值可用作用户注册界面</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Member_Info(string openid)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_User_By_OpenID(openid);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取找校友列表
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Alumnus_Wrap_Mine(string openid)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Alumnus_Wrap_Mine(openid);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取用户简略信息
        /// </summary>
        /// <param name="openid">校友ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Member_Info_Brief(string openid)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_User_Brief(openid);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取我参与的活动列表
        /// </summary>
        /// <param name="AMID">校友ID</param>
        /// <param name="PageIndex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Event_List(string AMID, int PageIndex)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Event_List(AMID, PageIndex);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取待认证校友列表
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Verifty_List(string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Alumnus_List_Verifty(OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 认证通过
        /// </summary>
        /// <param name="AMID">校友ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Pass_Verifty(string AMID)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Change_Alumni_Verifty(AMID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Attention(string openid)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Attention(openid);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取注册所需高校列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Signin_Union_Name()
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Alumni_Union_Name_List();
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取高校下所有院系
        /// </summary>
        /// <param name="Union_Name">高校名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Signin_Faculty_Name(string Union_Name)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Faculty_Name_List();
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取毕业年份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Signin_Years_List()
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Years_List();
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 获取所获学历
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> Get_Degree_List()
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_Degree_List();
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 注册提交
        /// </summary>
        /// <param name="Member"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Submit_Signin([FromBody] Alumni_Member Member, int Year)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Create_Alumnus_App(Member, Year);
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