using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_loginout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string SysFoder = Common.AdminFoder;
        if (Session["AdminUser"] != null)
        {
            Hi.Model.SYS_AdminUser model = (Hi.Model.SYS_AdminUser)Session["AdminUser"];
            //记录退出日志
            //Utils.EditLog("安全日志", "用户" + model.Name + "退出管理系统", "系统安全模块", "logout.aspx", 0);
        }

        Session["AdminUser"] = null;
        Session.Clear();
        Session.Abandon();
        Response.Clear();

        base.Response.Redirect("login.aspx");//新的登录页面
        
    }
}