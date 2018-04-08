<%@ WebHandler Language="C#" Class="ShopMessage" %>

using System;
using System.Web;

public class ShopMessage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        System.Text.RegularExpressions.Regex Phonereg = new System.Text.RegularExpressions.Regex("^0?1[0-9]{10}$");
        context.Response.ContentType = "text/plain";
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
        context.Response.Expires = 0;
        context.Response.CacheControl = "no-cache";
        context.Response.AddHeader("Pragma", "No-Cache");
        string Username = context.Request["name"] + "";
        string UserPhone = context.Request["Phone"] + "";
        string Compid = context.Request["Compid"] + "";
        string UserMessge = context.Request["Msg"] + "";
        //string Code = context.Request["Code"] + "";
        System.Web.Script.Serialization.JavaScriptSerializer Js = new System.Web.Script.Serialization.JavaScriptSerializer();
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        if (string.IsNullOrEmpty(Username))
        {
            Msg.Msg = "请输入您的姓名";
        }
        else if (string.IsNullOrEmpty(UserPhone))
        {
            Msg.Msg = "请输入您的手机号码";
        }
        else if (!Phonereg.IsMatch(UserPhone))
        {
            Msg.Msg = "手机号码格式错误";
        }
        else if (string.IsNullOrEmpty(UserMessge))
        {
            Msg.Msg = "请输入留言内容";
        }
        //else if (!Code.Equals(context.Session["CheckCode"].ToString()))
        //{
        //    Msg.Msg = "验证码错误";
        //}
        else
        {
            Hi.Model.BD_ShopMessage Umsg = new Hi.Model.BD_ShopMessage();
            Umsg.CompID = Convert.ToInt32(Compid);
            Umsg.Name =Common.NoHTML(Username);
            Umsg.Phone = Common.NoHTML(UserPhone);
            Umsg.Remark = Common.NoHTML(UserMessge);
            Umsg.CreateDate = DateTime.Now;
            Umsg.dr = 0;
            Umsg.IsRead = 0;
            if (new Hi.BLL.BD_ShopMessage().Add(Umsg) > 0)
            {
                Msg.Code = "留言成功";
                Msg.Result = true;
            }
        }

        context.Response.Write(Js.Serialize(Msg));
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