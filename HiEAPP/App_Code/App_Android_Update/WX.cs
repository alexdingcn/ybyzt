using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using LitJson;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
///WX 的摘要说明
/// </summary>
public class WX
{
	public WX()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    //微信公众号配置信息
    //微信公众号openID
    public string AppID = ConfigurationManager.AppSettings["WXAppID"];
    //微信公众号Key
    public string AppSecret = ConfigurationManager.AppSettings["WXSecret"];


    //获取微信config接口所需参数
    public jsapi_ticket getWXconfig(string JSon,string version)
    {
        string appId = string.Empty;
        string timestamp = string.Empty;
        string nonceStr = string.Empty;
        string signature = string.Empty;
        string CompID = string.Empty;
        string url = string.Empty;
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["CompID"].ToString() != "" && JInfo["url"].ToString() != "")
            {
                CompID = JInfo["CompID"].ToString().Trim();
                url = JInfo["url"].ToString().Trim();
            }
            //获取核心企业维护的appid和secret
            //List<Hi.Model.Pay_PayWxandAli> list_paywx = new Hi.BLL.Pay_PayWxandAli().GetList("", "CompID = " + CompID + " and wx_Isno = 1 ", "");
            //if (list_paywx == null || list_paywx.Count <= 0 || ClsSystem.gnvl(list_paywx[0].wx_appid, "") == "" || ClsSystem.gnvl(list_paywx[0].wx_appsechet, "") == "")
            //    return new jsapi_ticket() { Result = "F", Description = "公众号配置信息不足" };

            appId = AppID;
            string secret = AppSecret;
            string token = getWXtoken(appId, secret);
            string ticket = getJSTicketTicket(token);
            int num = url.IndexOf("#");
            if (num > 0)
                url = url.Substring(0,num);
            //url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            nonceStr = createNonceStr();
            timestamp = ConvertDateTimeInt(DateTime.Now).ToString();

            string[] ArrayList = { "jsapi_ticket=" + ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + url };
            Array.Sort(ArrayList);
            signature = string.Join("&", ArrayList);
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1").ToLower();
            jsapi_ticket result = new jsapi_ticket();
            result.Result = "T";
            result.Description = "获取成功";
            result.appId = appId;
            result.timestamp = timestamp;
            result.nonceStr = nonceStr;
            result.signature = signature;
            return result;

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "getWXconfig:");
            return new jsapi_ticket() { Result = "F", Description = "参数异常" };
        }
    }

    //获取token
    public string getWXtoken(string id,string secret)
    {
        string content = "";
        string access_token = "";
        #region 获取token
        //string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + id + "&secret=" + secret;

        string strUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", id, secret);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);

        req.Method = "GET";

        using (WebResponse wr = req.GetResponse())
        {
            HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            content = reader.ReadToEnd();
        }
        JsonData JInfo = JsonMapper.ToObject(content);
        if (((IDictionary)JInfo).Contains("access_token"))
            access_token = JInfo["access_token"].ToString();
        else
            throw new Exception("获取token失败" + JInfo["errmsg"].ToString());
        #endregion
        return access_token;
    }

    /// 获取jsapi_ticket
    /// <summary>
    /// 获取jsapi_ticket
    /// </summary>
    /// <param name="access"></param>
    /// <returns></returns>
    private string getJSTicketTicket(string access)
    {
        string content = "";
        string ticket = "";
        #region 获取JSTicket
        string strUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access + "&type=jsapi";

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);

        req.Method = "GET";

        using (WebResponse wr = req.GetResponse())
        {
            HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            content = reader.ReadToEnd();
        }
        JsonData JInfo = JsonMapper.ToObject(content);
        if (((IDictionary)JInfo).Contains("ticket"))
            ticket = JInfo["ticket"].ToString();
        else
            throw new Exception("获取JSTicket失败" + JInfo["errmsg"].ToString());
        #endregion
        return ticket;

    }

    /// <summary>
    /// 获取wx临时图片
    /// </summary>
    /// <param name="mediaid"></param>
    /// <returns></returns>
    public string getJSImage(string mediaid)
    {
        string access = getWXtoken(AppID, AppSecret);
        #region 获取图片
        //string strUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", access, mediaid);
        string strUrl = string.Format(@"http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}",access,mediaid);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
        req.ServicePoint.Expect100Continue = false;
        req.Method = "GET";
        req.KeepAlive = true;

        using (WebResponse wr = req.GetResponse())
        {
            HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
            Stream stream = myResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            string path = ConfigurationManager.AppSettings["OrderPayAttchPath"].ToString().Trim();
            string viewpath = ConfigurationManager.AppSettings["OrderPayAttchViewPath"].ToString().Trim();
            //判断文件夹是否存在
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
            //保存在服务器的文件名
            string FileName = GenerateTimeStamp() + ".jpg";
            string saveFile = path + FileName;

            //MemoryStream ms = null;
            //Byte[] buffer = new Byte[1024];
            //int offset = 0, actuallyRead = 0;
            //do
            //{
            //    actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
            //    offset += actuallyRead;
            //}
            //while (actuallyRead > 0);
            //ms = new MemoryStream(buffer);

            //byte[] buffurPic = ms.ToArray();
            //System.IO.File.WriteAllBytes(saveFile, buffurPic);

            System.Drawing.Bitmap sourcebm = new System.Drawing.Bitmap(reader.BaseStream);
            reader.Close();
            sourcebm.Save(saveFile);//filename 保存地址

            return FileName;
        }
        #endregion
    }

    public string GenerateTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds).ToString();
    }  

    /// <summary>
    /// 创建随机字符串
    /// </summary>
    /// <returns></returns>
    private static string createNonceStr()
    {
        int length = 16;
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string str = "";
        Random rad = new Random();
        for (int i = 0; i < length; i++)
        {
            str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
        }
        return str;
    }

    /// 将c# DateTime时间格式转换为Unix时间戳格式  
    /// <summary>  
    /// 将c# DateTime时间格式转换为Unix时间戳格式  
    /// </summary>  
    /// <param name="time">时间</param>  
    /// <returns>double</returns>  
    public static int ConvertDateTimeInt(System.DateTime time)
    {
        int intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = Convert.ToInt32((time - startTime).TotalSeconds);
        return intResult;
    }

    public class jsapi_ticket
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public String signature { get; set; }//是否维护快捷支付账号
    }
}