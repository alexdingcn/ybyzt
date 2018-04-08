using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace DBUtility
{
    /// <summary>
    /// HttpHelp 的摘要说明
    /// </summary>
    public class HttpHelp
    {
        public HttpHelp()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 通过Http的Get方式访问
        /// </summary>
        /// <param name="Url">访问的Url</param>
        /// <param name="Coding">规定返回的编码格式（可不填）</param>
        /// <returns></returns>
        public static string HttpGet(string Url, Encoding Coding = null)
        {
            StreamReader StReder = null;
            try
            {
                if (string.IsNullOrEmpty(Url.Trim()))
                {
                    throw new Exception("URL地址不能为空");
                }
                HttpWebRequest Requst = (HttpWebRequest)HttpWebRequest.Create(Url.Trim());
                Requst.Method = "GET";
                Requst.Timeout = 2000;
                Requst.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse Respons = (HttpWebResponse)Requst.GetResponse();
                if (Coding != null)
                    StReder = new StreamReader(Respons.GetResponseStream(), Coding);
                else
                    StReder = new StreamReader(Respons.GetResponseStream(), Encoding.UTF8);
                string result = StReder.ReadToEnd();
                StReder.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 通过Http的Post方式访问
        /// </summary>
        /// <param name="Url">访问的Url</param>
        /// <param name="Coding">规定返回的编码格式（可不填）</param>
        /// <returns></returns>
        public static string HttpPost(string Url, Dictionary<string, string> valuePair, Encoding Coding = null)
        {
            StreamReader StReder = null;
            try
            {
                if (string.IsNullOrEmpty(Url.Trim()))
                {
                    throw new Exception("URL地址不能为空");
                }
                HttpWebRequest Requst = (HttpWebRequest)HttpWebRequest.Create(Url.Trim());
                Requst.Method = "POST";
                Requst.ContentType = "application/x-www-form-urlencoded";
                if (valuePair.Count > 0)
                {
                    #region 添加Post 参数  
                    StringBuilder builder = new StringBuilder("&");
                    foreach (var item in valuePair)
                    {
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    }
                    //post数据转换成byte
                    byte[] Postdata = Encoding.UTF8.GetBytes(builder.ToString());
                    Requst.ContentLength = Postdata.Length;
                    using (Stream reqStream = Requst.GetRequestStream())
                    {
                        reqStream.Write(Postdata, 0, Postdata.Length);
                        reqStream.Close();
                    }
                    #endregion
                }
                HttpWebResponse Respons = (HttpWebResponse)Requst.GetResponse();
                if (Coding != null)
                    StReder = new StreamReader(Respons.GetResponseStream(), Coding);
                else
                    StReder = new StreamReader(Respons.GetResponseStream(), Encoding.UTF8);
                string result = StReder.ReadToEnd();
                StReder.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// 通过Http的Post方式访问
        /// </summary>
        /// <param name="Url">访问的Url</param>
        /// <param name="Coding">规定返回的编码格式（可不填）</param>
        /// <returns></returns>
        public static string HttpPost(string Url, string content, Encoding Coding = null)
        {
            StreamReader StReder = null;
            try
            {
                if (string.IsNullOrEmpty(Url.Trim()))
                {
                    throw new Exception("URL地址不能为空");
                }
                HttpWebRequest Requst = (HttpWebRequest)HttpWebRequest.Create(Url.Trim());
                Requst.Method = "POST";
                Requst.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrEmpty(content.Trim()))
                {
                    #region 添加Post 参数  
                    byte[] data = Encoding.UTF8.GetBytes(content);
                    Requst.ContentLength = data.Length;
                    using (Stream reqStream = Requst.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                        reqStream.Close();
                    }
                    #endregion
                }
                HttpWebResponse Respons = (HttpWebResponse)Requst.GetResponse();
                if (Coding != null)
                    StReder = new StreamReader(Respons.GetResponseStream(), Coding);
                else
                    StReder = new StreamReader(Respons.GetResponseStream(), Encoding.UTF8);
                string result = StReder.ReadToEnd();
                StReder.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }

    }
}