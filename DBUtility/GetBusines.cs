using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/**
 * 工商四元素
 * **/
namespace DBUtility
{
    public class GetBusines
    {
        private const string strurl = "https://icreditapi.jd.com/api/ent/fourElementVerify";
        //商户号
        private const string MerchantNo = "110875625";
        //密钥
        private const string Key = "9204A9EB56ADDCF704FE3CDA1D92A31D";

        public GetBusines()
        {

        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// md5 加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string md5(string str)
        {
            byte[] sor = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }

        /// <summary>
        /// 工商四元素认证
        /// </summary>
        /// <param name="entName">企业名称</param>
        /// <param name="legalCard">法人身份证</param>
        /// <param name="creditCode">统一社会信用代码</param>
        /// <param name="legalName">法人姓名</param>
        /// <returns></returns>
        public string GetBus(string entName, string legalCard, string creditCode,string legalName)
        {
            try
            {
                //加密前数据设置信息
                string jsondata = "{\"entName\":\"" + entName + "\",\"legalCard\":\"" + legalCard + "\",\"creditCode\": \"" + creditCode + "\",\"legalName\":\"" + legalName + "\"}";
                //用3eds加密
                string basejsondata = Convert.ToBase64String(Encoding.UTF8.GetBytes(Des3.EncodeCBC(Key, jsondata)));

                //订单编号
                var uuid = Guid.NewGuid().ToString("N");
                string orderNo = uuid;

                //版本号
                string version = "1.0";
                //字符编码
                string charset = "UTF-8";
                //时间戳
                string time = GetTimeStamp();
                //数据签名
                string checkSign = md5(version + charset + time + orderNo + basejsondata);

                string JSONData = "{\"orderNo\": \"" + orderNo + "\",\"version\": \"" + version + "\",\"charset\": \"" + charset + "\",\"data\": \"" + basejsondata + "\",\"checkSign\": \"" + checkSign.ToUpper() + "\",\"time\": \"" + time + "\"}";

                string result = GetResponseData(JSONData);

                //string qq="{\"orderNo\":\"a0874164e85b4a7db36dc561c0c695b8\",\"version\":\"1.0\",\"charset\":\"UTF-8\",\"data\":\"Q1Q5NXcxc01tQnhxeDJDQ0pSMFNIMFd5WWhrUSs3RFlyKzBRV1N1dHZ1bUJER2VVWUppaWh1UlJrNkhQcisvZQ==\",\"checkSign\":\"568AF001FCC574E0C697DEF9A4D1A0D0\",\"time\":\"1520413500234\",\"flag\":null,\"success\":true,\"resultCode\":\"SUCCESS\",\"resultMsg\":\"成功\",\"tradeNo\":\"TRADE201803070505001899101970932\"}";

                JObject jo = (JObject)JsonConvert.DeserializeObject(result);

                if (jo["success"].ToString().ToLower() == "true")
                {
                    //请求成功
                    string data = jo["data"].ToString();
                    //解密
                    string Decodedata = Des3.DecodeCBC(Key, data);

                    JObject jodata = (JObject)JsonConvert.DeserializeObject(Decodedata.Replace("\0", ""));
                    if (jodata["authResult"].ToString().ToUpper() == "VALID")
                        return "SUCCESS";
                    else
                        return jodata["authMsg"].ToString();
                }
                else
                {
                    //请求不成功
                    return jo["resultMsg"].ToString();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public string GetBus1(string entName, string legalCard, string creditCode, string legalName)
        {
            //加密前数据设置信息
            string jsondata = "{\"entName\":\"" + entName + "\",\"legalCard\":\"" + legalCard + "\",\"creditCode\": \"" + creditCode + "\",\"legalName\":\"" + legalName + "\"}";
            //用3eds加密
            string basejsondata = Convert.ToBase64String(Encoding.UTF8.GetBytes(Des3.EncodeCBC(Key, jsondata)));

            //订单编号
            var uuid = Guid.NewGuid().ToString("N");
            string orderNo = uuid;

            //版本号
            string version = "1.0";
            //字符编码
            string charset = "UTF-8";
            //时间戳
            string time = GetTimeStamp();
            //数据签名
            string checkSign = md5(version + charset + time + orderNo + basejsondata);

            string JSONData = "{\"orderNo\": \"" + orderNo + "\",\"version\": \"" + version + "\",\"charset\": \"" + charset + "\",\"data\": \"" + basejsondata + "\",\"checkSign\": \"" + checkSign.ToUpper() + "\",\"time\": \"" + time + "\"}";

            //return GetResponseData(JSONData);
            return "";
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <returns>返回的JSON处理字符串</returns>
        public string GetResponseData(string JSONData)
        {
            WebHeaderCollection hc = new WebHeaderCollection();
            hc.Add("merchantNo", MerchantNo);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strurl);
            //request.ServicePoint.Expect100Continue = false;
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
            //request.ContentLength = bytes.Length;
            //request.ContentType = "merchantNo=" + MerchantNo + ";Content-Type=application/json; charset=utf-8;";
            request.Method = "POST";
            request.Headers.Add(hc);
            request.ContentType = "Content-Type=application/json; charset=utf-8";
            //声明一个HttpWebRequest请求
            //request.Timeout = 20000;
            //设置连接超时时间
            //request.Headers.Set("Pragma", "no-cache");
            Stream reqstream = request.GetRequestStream();
            //reqstream.Write(bytes, 0, bytes.Length);
            StreamWriter myStreamWriter = new StreamWriter(reqstream, Encoding.GetEncoding("UTF-8"));
            myStreamWriter.Write(JSONData);
            myStreamWriter.Close();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;
                using (StreamReader streamReader = new StreamReader(streamReceive, encoding))
                {
                    string strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    streamReader.Dispose();
                    return strResult;
                }
            }
        }
    }
}
