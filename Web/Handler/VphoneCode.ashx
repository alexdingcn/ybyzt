<%@ WebHandler Language="C#" Class="VphoneCode" %>

using System;
using System.Web;
using System.Collections.Generic;

using CFCA.Payment.Api;
using System.Web.Configuration;

using System.Web.SessionState;

public class VphoneCode : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        Hi.Model.PAY_FastPayMent fastpayModel = new Hi.Model.PAY_FastPayMent();
        string txtphone = Convert.ToString(request["phone"]);//手机号码
        string txtphonecode = Convert.ToString(request["phonecode"]);//手机验证码
       // string username = Convert.ToString(request["username"]);//用户名

        Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel("收款帐号绑定", txtphone, txtphonecode);
        string Json = string.Empty;
       // Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        
        if (logUser == null)
        {
            Json = "{\"ID\":\"0\",\"msg\":\"请先登录!\"}";
            context.Response.Write(Json);
            context.Response.End();
        }
        
        if (phonecode != null)
        {
            phonecode.IsPast = 1;
            phonecode.ts = DateTime.Now;
            phonecode.modifyuser = logUser.UserID;
            if (new Hi.BLL.SYS_PhoneCode().Update(phonecode))
            {
                Json = "{\"ID\":\"2000\",\"msg\":\"验证成功\"}";

            }
        }
        else
        {
            Json = "{\"ID\":\"2002\",\"msg\":\"手机验证码错误!\"}";
        }
        context.Response.Write(Json);
        context.Response.End();
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}