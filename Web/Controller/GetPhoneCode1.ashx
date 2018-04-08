<%@ WebHandler Language="C#" Class="GetPhoneCode1" %>

using System;
using System.Web;
using DBUtility;
using System.Collections.Generic;
using System.Linq;

public class GetPhoneCode1 : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string str = "";
            Hi.Model.SYS_PhoneCode phonecode = null;
            if (!string.IsNullOrEmpty(context.Request["phone"].ToString()))
            {
                string phone = context.Request["phone"];
                if (string.IsNullOrEmpty(phone))
                {
                    context.Response.Write("{\"type\":false,\"str\":\"手机号码格式错误！\"}");
                    return;
                }
                long outin = 0;
                if (!long.TryParse(phone, out outin))
                {
                    phone = AESHelper.Decrypt_php(phone);
                }
                string sendCode = CreateRandomCode(6);
                List<Hi.Model.SYS_PhoneCode> Lphones = new Hi.BLL.SYS_PhoneCode().GetList("CreateDate", " dr=0 and Ispast=1 and  Module='" + context.Request["Module"] + "'  and isPast=0 and  DateDiff(dd,createdate,getdate())=0 and phone='" + phone + "' ", " CreateDate desc");
                int QuickPaycard = Convert.ToInt32(ConfigCommon.GetNodeValue("Version.xml", "quickcardnum"));
                if (Lphones.Count > QuickPaycard)
                {
                    context.Response.Write("{\"type\":false,\"str\":\"该手机号今天获取验证码超过" + QuickPaycard + "次！\"}");
                    return;
                }
                List<Hi.Model.SYS_PhoneCode> Lphone = new Hi.BLL.SYS_PhoneCode().GetList("top 1 CreateDate", " dr=0 and isPast=0 and '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' between  createdate and DATEADD(MI,30,CreateDate) and Module='" + context.Request["Module"] + "' and phone='" + phone + "' ", " CreateDate desc");
                if (Lphone.Count > 0)
                {
                    if (Lphone[0].CreateDate.AddMinutes(2) >= DateTime.Now)
                    {
                        str = "\"type\":false,\"str\":\"请" + ((int)((Lphone[0].CreateDate.AddMinutes(2) - DateTime.Now).TotalSeconds)) + "s后重新获取\"";
                        str = PoseStr(str);
                        context.Response.Write(str);
                        return;
                    }
                }
                int UserId = 0;
                if (!Common.GetUserExistsWhere("Phone", phone, "", ref UserId) && context.Request["Module"] == "手机登录")
                {
                    str = "\"type\":false,\"str\":\"该手机未绑定用户，请先注册！\"";
                    str = PoseStr(str);
                    context.Response.Write(str);
                    return;
                }
                string Module = Common.NoHTML(context.Request["Module"].ToString().Trim());
                List<Hi.Model.SYS_Users> Users = new List<Hi.Model.SYS_Users>();
                List<Hi.Model.SYS_CompUser> ListCompUser = new List<Hi.Model.SYS_CompUser>();
                if (!string.IsNullOrEmpty(context.Request["type"].ToString()))
                {
                    string UserName = context.Request["username"];
                    if (Module == "修改登录密码")
                    {
                        Users = new Hi.BLL.SYS_Users().GetListUser("username,id,AuditState,IsEnabled", "Phone", phone, "");
                        if (Users.Count > 0)
                        {
                            Users = Users.Where(T => T.IsEnabled == 1).ToList();
                            if (Users.Count == 0)
                            {
                                context.Response.Write("{\"type\":false,\"str\":\"该手机绑定的用户已被禁用\"}");
                                return;
                            }
                            string UsersId = string.Join(",", Users.Select(T => T.ID));
                            ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,IsEnabled,IsAudit,utype,DisID,Compid", " dr=0 and  Userid in(" + UsersId + ")", " createdate");
                            if (ListCompUser.Count == 0)
                            {
                                context.Response.Write("{\"type\":false,\"str\":\"该手机未绑定用户\"}");
                                return;
                            }
                            ListCompUser = ListCompUser.Where(T => T.IsAudit == 2).ToList();
                            if (ListCompUser.Count == 0)
                            {
                                context.Response.Write("{\"type\":false,\"str\":\"该手机绑定的用户未通过审核\"}");
                                return;
                            }
                            ListCompUser = ListCompUser.Where(T => T.IsEnabled == 1).ToList();
                            if (ListCompUser.Count == 0)
                            {
                                context.Response.Write("{\"type\":false,\"str\":\"该手机绑定的用户已被禁用\"}");
                                return;
                            }
                            if (Users.Where(T => T.UserName == UserName).ToList().Count == 0)
                            {
                                context.Response.Write("{\"type\":false,\"str\":\"验证的账号不正确。\"}");
                                return;
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"type\":false,\"str\":\"该手机未绑定用户\"}");
                            return;
                        }
                    }
                    int type = context.Request["type"].ToInt(0);
                    phonecode = new Hi.Model.SYS_PhoneCode();
                    phonecode.Type = type;
                    phonecode.Module = Module;
                    phonecode.UserName = UserName;
                    phonecode.Phone = phone;
                    phonecode.PhoneCode = sendCode;
                    phonecode.IsPast = 0;
                    phonecode.UserID = context.Request["userid"].ToInt(0);
                    phonecode.CreateDate = DateTime.Now;
                    phonecode.ts = DateTime.Now;
                    phonecode.modifyuser = context.Request["userid"].ToInt(0);
                }
                else
                {
                    str = "\"type\":false,\"str\":\"参数错误\"";
                    str = PoseStr(str);
                    context.Response.Write(str);
                    return;
                }
                GetPhoneCode getphonecode = new GetPhoneCode();
                getphonecode.GetUser(System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(), System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
                string rstr = string.Empty;
                string compName = string.Empty;
                switch (Module)
                {
                    case "修改登录密码":
                    case "手机登录":
                    case "修改地址":
                    case "修改绑定手机":
                    case "支付密码找回":
                    case "绑定快捷支付获取验证码":
                        rstr = getphonecode.ReturnSTR(phone, sendCode); break;
                    default: str = "\"type\":false,\"str\":\"提交参数错误\"";
                        str = PoseStr(str);
                        context.Response.Write(str);
                        return;
                }
                if (rstr == "Success")
                {
                    if (new Hi.BLL.SYS_PhoneCode().Add(phonecode) > 0)
                    {
                        str = "\"type\":true";
                        str = PoseStr(str);
                        context.Response.Write(str);
                    }
                    else
                    {
                        str = "\"type\":false,\"str\":\"服务器异常\"";
                        str = PoseStr(str);
                        context.Response.Write(str);
                    }
                }
                else
                {
                    str = "\"type\":false,\"str\":\"短信发送失败，请重试。\"";
                    str = PoseStr(str);
                    context.Response.Write(str);
                }
            }
            else
            {
                str = "\"type\":false,\"str\":\"手机号码不能为空\"";
                str = PoseStr(str);
                context.Response.Write(str);
            }
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
        }
    }

    public string PoseStr(string str)
    {
        str = "{" + str + "}";
        return str;
    }

    public string CreateRandomCode(int n)
    {
        string code = "";
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            if (i == 0)
            {
                code += rand.Next(1, 9).ToString();
            }
            else
            {
                code += rand.Next(0, 9).ToString();
            }
        }
        return code;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}