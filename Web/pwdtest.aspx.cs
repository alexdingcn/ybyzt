using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Hi.BLL;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class pwdtest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 后台帐号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {

        List<Hi.Model.SYS_AdminUser> ListUsers = new Hi.BLL.SYS_AdminUser().GetList("", "", "");
        for (int i = 0; i < ListUsers.Count; i++)
        {
            Hi.Model.SYS_AdminUser user = new Hi.BLL.SYS_AdminUser().GetModel(ListUsers[i].ID);
            user.LoginPwd = SHA1Encrypt(SHA1Encrypt(ListUsers[i].LoginPwd));
            bool result = new Hi.BLL.SYS_AdminUser().Update(user);
        }
    }

    /// <summary>
    /// 前台帐号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_Users> ListUsers1 = new Hi.BLL.SYS_Users().GetList("", "", "");
        for (int i = 0; i < ListUsers1.Count; i++)
        {
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(ListUsers1[i].ID);
            user.UserPwd = SHA1Encrypt(SHA1Encrypt(ListUsers1[i].UserPwd));
            bool result = new Hi.BLL.SYS_Users().Update(user);
        }
    }

    /// <summary>
    /// sha1加密方法
    /// </summary>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public static string SHA1Encrypt(string pwd)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "SHA1").ToLower();
    }
    
}