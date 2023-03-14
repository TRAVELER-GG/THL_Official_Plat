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
    public partial class Api_PersonalController : ControllerBase
    {
        private readonly IApi_Service IApi = new Api_Service();
    }

    public partial class Api_PersonalController : ControllerBase
    {
        /// <summary>
        /// 获取可修改数据
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Api_Response> App_Personal(string OpenID)
        {
            var Res_Item = new Api_Response();
            try
            {
                Res_Item.Data = IApi.Get_App_Personal(OpenID);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Mobile">手机号码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Mobile(string OpenID, string Mobile)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Mobile(OpenID, Mobile);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Gender">性别</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Gender(string OpenID, string Gender)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Gender(OpenID, Gender);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改兴趣爱好
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Interest">兴趣爱好</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Interest(string OpenID, string Interest)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Interest(OpenID, Interest);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改毕业年份
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Year">毕业年份</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Year(string OpenID, int Year)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Year(OpenID, Year);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改所获学位
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Degree">所获学位</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Degree(string OpenID, string Degree)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Degree(OpenID, Degree);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改所属院系
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Faculty">所属院系</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Faculty(string OpenID, string Faculty)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Faculty(OpenID, Faculty);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改所处行业
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Profession">所处行业</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Profession(string OpenID, string Profession)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Profession(OpenID, Profession);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改所在公司
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Company">所在公司</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Company(string OpenID, string Company)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Company(OpenID, Company);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改当前职位
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Position">当前职位</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Position(string OpenID, string Position)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Position(OpenID, Position);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }
        

        /// <summary>
        /// 修改所在城市
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Province">省</param>
        /// <param name="City">市</param>
        /// <param name="District">区</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_City(string OpenID, string Province, string City, string District)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_City(OpenID, Province, City, District);
            }
            catch (Exception Ex)
            {
                Res_Item.Error_Code = StatusCodes.Status500InternalServerError;
                Res_Item.Error_Msg = Ex.Message.ToString();
            }
            return Res_Item;
        }

        /// <summary>
        /// 修改出生籍贯
        /// </summary>
        /// <param name="OpenID">openid</param>
        /// <param name="Native_Province">省</param>
        /// <param name="Native_City">市</param>
        /// <param name="Native_District">区</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Api_Response> Update_Native_City(string OpenID, string Native_Province, string Native_City, string Native_District)
        {
            var Res_Item = new Api_Response();
            try
            {
                IApi.Update_Alumnus_Native_City(OpenID, Native_Province, Native_City, Native_District);
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