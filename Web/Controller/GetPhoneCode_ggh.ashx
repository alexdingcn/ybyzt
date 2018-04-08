<%@ WebHandler Language="C#" Class="GetPhoneCode_ggh" %>

using System;
using System.Web;
using DBUtility;

public class GetPhoneCode_ggh : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string str = "";
            int i = 0;
            if (!string.IsNullOrEmpty(context.Request["phone"].ToString()))
            {
                string phone = context.Request["phone"].ToString();
                string content = CreateRandomCode(6);
                Hi.Model.SYS_PhoneCode phonecodes = new Hi.BLL.SYS_PhoneCode().GetModel(phone);
                if (phonecodes != null)
                {
                    if (phonecodes.CreateDate.AddMinutes(1) >= DateTime.Now)
                    {
                        str = "\"type\":false,\"str\":\"请勿重复发送\"";
                        str = PoseStr(str);
                        context.Response.Write(str);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(context.Request["type"].ToString()))
                {
                    int type = int.Parse(context.Request["type"].ToString());
                    Hi.Model.SYS_PhoneCode phonecode = new Hi.Model.SYS_PhoneCode();
                    phonecode.Type = type;
                    phonecode.Module = context.Request["Module"].ToString();
                    phonecode.UserName = context.Request["username"].ToString();
                    phonecode.Phone = phone;
                    phonecode.PhoneCode = content;
                    phonecode.IsPast = 2;
                    phonecode.UserID = int.Parse(context.Request["userid"].ToString());
                    phonecode.CreateDate = DateTime.Now;
                    phonecode.ts = DateTime.Now;
                    phonecode.modifyuser = int.Parse(context.Request["userid"].ToString());
                    i = new Hi.BLL.SYS_PhoneCode().Add(phonecode);
                }
                else
                {
                    str = "\"type\":false,\"str\":\"手机验证码储存加模块字段,请写上他属于哪个模块!\"";
                    str = PoseStr(str);
                    context.Response.Write(str);
                    return;
                }
                GetPhoneCode getphonecode = new GetPhoneCode();
                getphonecode.GetUser(System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(),
                        System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
                string rstr = getphonecode.ReturnSTR(phone, content);
                if (rstr == "Success")
                {
                    if (i > 0)
                    {
                        Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel(i);
                        phonecode.IsPast = 0;
                        if (new Hi.BLL.SYS_PhoneCode().Update(phonecode))
                        {
                            str = "\"type\":true";
                            str = PoseStr(str);
                            context.Response.Write(str);
                        }
                        else
                        {
                            str = "\"type\":false,\"str\":\"程序异常\"";
                            str = PoseStr(str);
                            context.Response.Write(str);
                        }
                    }
                    else
                    {
                        str = "\"type\":false,\"str\":\"数据插入异常\"";
                        str = PoseStr(str);
                        context.Response.Write(str);
                    }
                }
                else
                {
                    str = "\"type\":false,\"str\":\"发送失败\"";
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