<%@ WebHandler Language="C#" Class="SubmitUserMsg" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

public class SubmitUserMsg : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Regex Phonereg = new Regex("^0?1[0-9]{10}$");
        context.Response.ContentType = "text/plain";
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
        context.Response.Expires = 0;
        context.Response.CacheControl = "no-cache";
        context.Response.AddHeader("Pragma", "No-Cache");
        string Username = context.Request["name"]+ "";
        string UserPhone = context.Request["Phone"] + "";
        string UserMailQQ = context.Request["MailQQ"] + "";
        string UserMessge = context.Request["Msg"] + "";
        string Code = context.Request["Code"] + "";
        JavaScriptSerializer Js = new JavaScriptSerializer();
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        if (string.IsNullOrEmpty(Username))
        {
            Msg.Msg = "请输入您的姓名";
        }
        else if (string.IsNullOrEmpty(UserPhone)) {
            Msg.Msg = "请输入您的手机号码";
        }
        else if (!Phonereg.IsMatch(UserPhone)) {
            Msg.Msg = "手机号码格式错误";
        }
        else if (string.IsNullOrEmpty(UserMessge))
        {
            Msg.Msg = "请输入留言内容";
        }
        else if (!Code.Equals(context.Session["CheckCode"].ToString()))
        {
            Msg.Msg = "验证码错误";
        }
        else
        {
            Hi.Model.SYS_UserMessage Umsg = new Hi.Model.SYS_UserMessage();
            Umsg.UserName = Common.NoHTML( Username);
            Umsg.UserPhone = Common.NoHTML(UserPhone);
            Umsg.UserMailQQ =Common.NoHTML( UserMailQQ);
            Umsg.UserMessge =Common.NoHTML( UserMessge);
            Umsg.CreateDate = DateTime.Now;
            Umsg.ModifyDate = DateTime.Now;
            Umsg.State = 0;
            Umsg.ModifyUser = 0;
            if (new Hi.BLL.SYS_UserMessage().Add(Umsg) > 0)
            {
                Msg.Code = "留言成功";
                Msg.Result = true;
            }
        }

        context.Response.Write(Js.Serialize(Msg));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}