using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SSO_jlc_indexold : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpCookie cookie = Request.Cookies["loginmodel"];
            if (cookie != null)
            {
                this.txtuid.Value = System.Web.HttpUtility.UrlDecode(cookie.Value.ToString());
            }
            IsLogin();
        }

    }

    public void IsLogin()
    {
        if (LoginModel.IsLoginAll())
        {
            LoginModel logUser = Session["UserModel"] as LoginModel;
            string username = logUser.UserName;
            string usertypeid = logUser.TypeID.ToString();
            if (usertypeid == "1" || usertypeid == "5")//代理商
            {
                Response.Redirect("/Distributor/UserIndex.aspx");
            }
            else if (usertypeid == "3" || usertypeid == "4")//厂商
            {
                Response.Redirect("/Company/jsc.aspx");
            }
        }
    }


    protected void QuitLogin_Click(object sender, EventArgs e)
    {
        if (Context.Request.Cookies["loginmodel"] != null)
        {
            HttpCookie cookie = new HttpCookie("loginmodel");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
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
        Session.Remove("UserModel");
        Response.Redirect("/login.aspx");
    }

}