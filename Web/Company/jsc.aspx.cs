using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_jsc : CompPageBase
{
    public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            user = this.CompUser;

            if (user.UserPwd == Util.md5("123456"))
            {
                  JScript.AlertMethod(this, "检测到您的登录密码为系统默认密码，请先修改您的登录密码！", JScript.IconOption.错误, "function(){location.href='ChangePwd.aspx?IsDpwd=1'}");
                return;
            }
        }
    }
}