using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_loginout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string SysFoder = Common.AdminFoder;
        if (Session["UserModel"] is LoginModel)
        {
            //Hi.Model.SYS_Users model = (Hi.Model.SYS_Users)Session["UserModel"];
            //记录退出日志
            //Utils.EditLog("安全日志", "用户" + model.Name + "退出管理系统", "系统安全模块", "logout.aspx", 0);
        }
        if (Context.Request.Cookies["token"] != null)
        {
            HttpCookie cookie = new HttpCookie("token");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_type"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_type");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_state"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_state");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        Session["UserModel"] = null;
        Session.Clear();
        Session.Abandon();
        Response.Clear();
        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
        //酒隆仓
        if (Request["type"] == "1")
        {
            //Response.Redirect("http://jlc.my1818.com", true);
        }
        else
        {
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
        
    }
}