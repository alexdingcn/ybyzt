<%@ WebHandler Language="C#" Class="FindPwd" %>


using System;
using System.Web;
using DBUtility;
using System.Web.SessionState;
using System.Collections.Generic;

public class FindPwd : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string UserName = context.Request["username"].ToString();
        string Code = context.Request["code"].ToString();
        if (Code != "0")
        {
            if (Code.ToString() != context.Session["CheckCode"].ToString())
            {
                context.Response.Write("{\"type\":false,\"usertel\":0,\"typeid\":0,\"userid\":0,\"str\":\"验证码错误！\"}");
                return;
            }
        }
        if (!DBHelper.IsOpen())
        {
            context.Response.Write("{\"type\":false,\"usertel\":0,\"typeid\":0,\"userid\":0,\"str\":\"系统无法连接数据库服务器，请联系管理员！\"}");
            return;
        }
        List<Hi.Model.SYS_Users> ListUser = new Hi.BLL.SYS_Users().GetListUser("IsEnabled,AuditState,Phone,Type", "UserName", UserName, "");
        if (ListUser.Count == 0)
        {
            context.Response.Write("{\"type\":false,\"usertel\":0,\"typeid\":0,\"userid\":0,\"str\":\"登陆帐号不存在！\"}");
            return;
        }
        else
        {
            if (ListUser[0].IsEnabled == 1 && ListUser[0].AuditState == 2)// AuditState:0：未审 2:已审 
            {
                // context.Response.Write("{\"type\":true,\"usertel\":" + user.Phone + ",\"typeid\":" + user.Type + ",\"userid\":"+user.ID+",\"username\":"+user.UserName+",\"str\":\"-该用户为禁用状态！\"}");

                context.Response.Write("{\"type\":true,\"usertel\":" + ListUser[0].Phone + ",\"typeid\":" + ListUser[0].Type + ",\"userid\":" + 0 + ",\"str\":\"0\"}");
            }
            else
            {
                context.Response.Write("{\"type\":false,\"usertel\":0,\"typeid\":0,\"userid\":0,\"str\":\"该用户为禁用状态！\"}");
                return;
            }

        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}