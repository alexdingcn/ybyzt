using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class jlc_login : System.Web.UI.Page
{
    public string user = string.Empty;
    private string sign = "";
    public string inKey = "haiyusoft";
    private string validity = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpCookie cookie = Request.Cookies["loginmodel"];
            if (cookie != null)
            {
                this.txtuid.Value = System.Web.HttpUtility.UrlDecode(cookie.Value.ToString());
            }
            //IsLogin();
        }

    }

    //public void IsLogin()
    //{
    //    if (LoginModel.IsLoginAll())
    //    {
    //        LoginModel logUser = Session["UserModel"] as LoginModel;
    //        string username = logUser.UserName;
    //        string usertypeid = logUser.TypeID.ToString();
    //        if (usertypeid == "1" || usertypeid == "5")//代理商
    //        {
    //            Response.Redirect("/Distributor/UserIndex.aspx");
    //        }
    //        else if (usertypeid == "3" || usertypeid == "4")//厂商
    //        {
    //            Response.Redirect("/Company/jsc.aspx");
    //        }
    //    }
    //}


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

    protected void btnlogin_click(object sender, EventArgs e)
    {
        var uid = this.txtuid.Value.Trim();
        var password = this.txtpwd.Value.Trim();

        if (uid == "" || uid == "帐号" || password == "" || password == "密码")
        {
            JScript.AlertMethod(this, "用户名或密码不能为空！！！", JScript.IconOption.错误, "function(){location.href='index.aspx';}");
            return;            
        }
        else
        {
            string jsonUser = "{\"type\":\"1\",\"username\":\"" + this.txtuid.Value.Trim() + "\",\"password\":\"" + this.txtpwd.Value.Trim() + "\",\"logo\":\"\"}";

            user = new AESHelper(inKey).KeyEncrypt(jsonUser, out sign, out validity);

            string url = System.Configuration.ConfigurationManager.AppSettings["WebDomainName"].ToString() + "/SSOLogin.aspx?user=" + user + "&sign=" + sign + "&valid=" + validity;

            ClientScript.RegisterStartupScript(GetType(), "ssologin", "<script>window.location.href='" + url + "'</script>");
        }
    }
}
