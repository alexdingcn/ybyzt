using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Configuration;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json;

namespace DBUtility
{
    public class GetPhoneCode
    {
        public GetPhoneCode()
        {
            
        }

        private String product = "Dysmsapi"; //短信API产品名称（短信产品名固定，无需修改）
        private String domain = "dysmsapi.aliyuncs.com"; //短信API产品域名（接口地址固定，无需修改）
        private String accessKeyId = "LTAIb3uxdYYBP3yP"; //你的accessKeyId，参考本文档步骤2
        private String accessKeySecret = "H4rE8qJzGec9CbrCPgIbeRUH4wMba9"; //你的accessKeySecret，参考本文档步骤2

        //private const string strurl = "http://sh2.ipyy.com/sms.aspx?action=send&userid=&account={0}&password={1}&mobile={2}&content={3}&sendTime=&extno=";
        //private string corpid { get; set; }
        //private string Pwd { get; set; }

        //短信模板变量替换JSON串,
        string TParam = string.Empty;
        string TCode = string.Empty;
        string SName = string.Empty;
        string PhoneSendTel = ConfigurationManager.AppSettings["PhoneSendTel"];

        private string VerifyTemplate = "\"code\":\"{0}\"";
        private string VerifyTemplate1 = "\"company\":\"{0}\",\"account\":\"{1}\"";// 1、5参数相同
        //private string VerifyTemplate5 = "\"company\":\"{0}\",\"account\":\"{1}\"";

        private string VerifyTemplate3 = "\"company\":\"{0}\"";// 3、4参数相同
        //private string VerifyTemplate4 = "\"company\":\"{0}\"";

        private string VerifyTemplate6 = "\"apply\":\"{0}\",\"person\":\"{1}\",\"action\":\"{2}\"";

        private string VerifyTemplate7 = "\"userName\":\"{0}\",\"initPass\":\"{1}\"";

        private string VerifyTemplate8 = "\"company\":\"{0}\",\"reason\":\"{1}\"";

        //private string VerifyTemplate = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】您的验证码为：${code} \n(验证码请勿泄露，需要对您进行身份校验。(如非本人操作,请致电" + ConfigurationManager.AppSettings["PhoneSendTel"] + ")";

        //private string VerifyTemplate1 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】您的企业 ${company} 已经成功入驻" + ConfigurationManager.AppSettings["PhoneSendName"] + "平台，欢迎使用：${account} 账号进行登录。\n(如非本人操作,请致电" + ConfigurationManager.AppSettings["PhoneSendTel"] + ")";

        private string VerifyTemplate2 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】{0}\n(如非本人操作,请致电" + ConfigurationManager.AppSettings["PhoneSendTel"] + ")";

        //private string VerifyTemplate3 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】代理商：${company} 已经申请入驻您的企业，请尽快处理。\n(如非本人操作,请致电" + ConfigurationManager.AppSettings["PhoneSendTel"] + ")";

        //private string VerifyTemplate4 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】企业：${company} 申请入驻" + ConfigurationManager.AppSettings["PhoneSendName"] + "，请尽快审批。";

        //private string VerifyTemplate5 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】您好，您已经成功加盟 ${company}，欢迎使用：${account} 账号进行登录。(如非本人操作,请致电" + ConfigurationManager.AppSettings["PhoneSendTel"] + ")";

       //private string VerifyTemplate6 = "【" + ConfigurationManager.AppSettings["PhoneSendName"] + "】尊敬的客户您好，您的 ${apply} 申请已提交，请等待 ${person} 审核，审核成功后，我们将以短信的方式通知您。如有疑问，请 ${action}，感谢您的配合。";

        /// <summary>
        /// 账号：zgfcs6  密码：a123456 
        /// 最新账号:jksc068   密码:jksc06833
        /// http://sh2.ipyy.com:8080/Home/Index
        /// http://sh2.cshxsp.com/sms.aspx
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        public void GetUser(string uid, string pwd)
        {
            //corpid = uid;
            //Pwd = pwd;
        }

        public string ReturnSTR(string phone, string content)
        {
            try
            {
                TParam = string.Format(VerifyTemplate, content);
                //SName = "yzt_verify";
                TCode = "SMS_123672350";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReturnSTRS(string phone, string CompName,string Username)
        {
            try
            {
                //string content = string.Empty;
                //content = string.Format(VerifyTemplate1, CompName, Username);
                //return send(phone, content);

                TParam = string.Format(VerifyTemplate1, CompName, Username);
                //SName = "yzt_register";
                TCode = "SMS_123797162";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReturnDisRe(string phone, string CompName, string Username)
        {
            try
            {
                //string content = string.Empty;
                //content = string.Format(VerifyTemplate5, CompName, Username);
                //return send(phone, content);
                
                TParam = string.Format(VerifyTemplate1, CompName, Username);
                //SName = "yzt_enroll_success";
                TCode = "SMS_123672338";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReturnSTRS1(string phone, string DisName)
        {
            try
            {
                //string content = string.Format(VerifyTemplate3, DisName);
                //return send(phone, content);
                
                TParam = string.Format(VerifyTemplate3, DisName);
                //SName = "yzt_apply";
                TCode = "SMS_123797163";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReturnST(string phone, string content)
        {
            try
            {
                //content = string.Format(VerifyTemplate2, content);
                //return send(phone, content);

                //TParam = string.Format("{\"code\":\"{0}\"}", content);
                //SName = "yzt_verify";
                //TCode = "SMS_123672350";
                //return send(phone, SName, TCode, TParam);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ReturnComp(string phone, string content)
        {
            try
            {
                //content = string.Format(VerifyTemplate4, content);
                //return send(phone, content);

                TParam = string.Format(VerifyTemplate3, content);
                //SName = "yzt_enroll";
                TCode = "SMS_123667321";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendComp(string phone, string content = "拨打客服热线")
        {
            try
            {
                //content = string.Format(VerifyTemplate6, content, "入驻", "平台");
                //return send(phone, content);

                TParam = string.Format(VerifyTemplate6, "入驻", "平台", content + PhoneSendTel);
                //SName = "yzt_applywait";
                TCode = "SMS_123672344";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendDis(string phone, string content)
        {
            try
            {
                //content = string.Format(VerifyTemplate6, content, "加盟", "企业");
                //return send(phone, content);
                //TParam = "{\"apply\":\"{0}\",\"person\":\"{1}\",\"action\":\"{2}\"}";
                TParam = string.Format(VerifyTemplate6, "加盟", "企业", content);
                //SName = "yzt_applywait";
                TCode = "SMS_123672344";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       
        public string SendUser(string phone, string userName, string initPass)
        {
            try
            {
                TParam = string.Format(VerifyTemplate7, userName, initPass);
                //SName = "employee_enroll";
                TCode = "SMS_125024035";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendConfirm(string phone, string CompName, string Username)
        {
            try
            {
                TParam = string.Format(VerifyTemplate1, CompName, Username);
                //SName = "yzt_enroll_success";
                TCode = "SMS_123672338";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SendReject(string phone, string CompName, string reason)
        {
            try
            {
                TParam = string.Format(VerifyTemplate8, CompName, reason);
                //SName = "yzt_enroll_reject";
                TCode = "SMS_125118848";
                return send(phone, TCode, TParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string send(string phone, string content)
        {
            string url = string.Empty;
            string result = string.Empty;
            try
            {
                url = string.Format("", ConfigurationManager.AppSettings["PhoneCodeAccount"], ConfigurationManager.AppSettings["PhoneCodePwd"], phone, content);
                HttpWebRequest http = WebRequest.Create(url) as HttpWebRequest;
                http.ServicePoint.Expect100Continue = false;
                http.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
                http.Method = "GET";
                http.Timeout = 2000;

                //
                //http.ContentType = "merchantNo=商户号;Content-Type=application/json; charset=utf-8;";

                using (WebResponse response = http.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            result = reader.ReadToEnd();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(result);
                            XmlNode returnstatus = doc.DocumentElement.SelectSingleNode("returnstatus");
                            if (returnstatus != null)
                            {
                                result = returnstatus.InnerText.ToString();
                                if (result != "Success")
                                {
                                    result = doc.DocumentElement.SelectSingleNode("message").InnerText.ToString();
                                }
                            }
                            return result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            reader.Close();
                            response.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="SignName">短信签名</param>
        /// <param name="TemplateCode">短信模板</param>
        /// <param name="TemplateParam">模板中的变量替换JSON串</param>
        /// <returns></returns>
        public string send(string phone, string TemplateCode, string TemplateParam)
        {
            try
            {
                IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
                //IAcsClient client = new DefaultAcsClient(profile);
                // SingleSendSmsRequest request = new SingleSendSmsRequest();
                //初始化ascClient,暂时不支持多region（请勿修改）
                DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
                IAcsClient acsClient = new DefaultAcsClient(profile);
                SendSmsRequest request = new SendSmsRequest();

                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phone;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "医伴金服";  //SignName; //"xxxxxxxx";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = TemplateCode;  //"SMS_00000001";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为

                string json = "{" + TemplateParam + "}";

                request.TemplateParam = json; //"{\"name\":\"Tom\"， \"code\":\"123\"}"; "{\"code\":\"772251\"}"
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (!"OK".Equals(sendSmsResponse.Code))
                {
                    return sendSmsResponse.Message;
                }
                return "Success";

            }
            catch (ServerException e)
            {
                return e.Message;
            }
            catch (ClientException e)
            {
                return e.Message;
            }
        }

        public string md5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }
    }
}
