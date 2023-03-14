using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using League.Api;

namespace League.Plat
{
    public class Get_And_Post
    {
        /// <summary>
        /// GET请求方式
        /// </summary>
        /// <param name="postUrl"></param>
        /// <returns></returns>
        public static string GetWebRequest(string postUrl)
        {
            var ret = string.Empty;
            try
            {
                var request = WebRequest.Create(postUrl);
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                ret = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// Post获取数据流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Stream PostMoths(string url, string data)
        {
            HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }
    }
}
