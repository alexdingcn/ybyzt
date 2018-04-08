

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_UserPWDEdit : DisPageBase
{
    public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        if (!IsPostBack)
        {
            txtpwd1.Value = "";
        }
    }

    protected void Btn_Update(object sender, EventArgs e)
    {
        if (user != null)
        {
            if (txtpwd1.Value == user.UserPwd)
            {
                
                if (txtpwd2.Value == user.UserPwd)
                {
                    spanpwd1.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                    spanpwd1.InnerText = "新密码不能与原密码相同";
                    return;
                }
                spanpwd1.Attributes.CssStyle.Value = "display:none;";
                user.UserPwd = txtpwd3.Value;
                user.ts = DateTime.Now;
                user.modifyuser = user.ID;
                
                if (user.IsFirst == 0 || user.IsFirst == 2)
                {
                    if (user.IsFirst == 0)
                    {
                        user.IsFirst = 1;
                        new Hi.BLL.SYS_Users().Update(user); 
                        JScript.AlertMethod(this, "恭喜您，您登录密码已经修改成功，下面将带您去修改支付密码！", JScript.IconOption.笑脸, "function (){ location.href = 'PayPWDEdit.aspx'; }");
                        return;
                            
                    }
                    else
                    {
                        user.IsFirst = 3;
                        new Hi.BLL.SYS_Users().Update(user); 
                        JScript.AlertMethod(this, "恭喜您，您已经完成了首次登录验证！", JScript.IconOption.笑脸, "function (){ location.href = 'PayPWDEdit.aspx'; }");
                        return;
                    }
                }
                if (user.IsFirst == 1)
                {
                    new Hi.BLL.SYS_Users().Update(user);
                    JScript.AlertMethod(this, "经核查，目前您的支付密码还未修改，下面将带您去修改支付密码！", JScript.IconOption.笑脸, "function (){ location.href = 'PayPWDEdit.aspx'; }");
                    return;
                }
                if (user.IsFirst == 3)
                {
                    new Hi.BLL.SYS_Users().Update(user);
                    JScript.AlertMethod(this, "您的登录密码已经修改成功！", JScript.IconOption.笑脸, "function (){ location.href = 'UserIndex.aspx'; }");
                    return;
                }
                if (user.IsFirst == 4)
                {
                    new Hi.BLL.SYS_Users().Update(user);
                    JScript.AlertMethod(this, "您的登录密码已经修改成功！", JScript.IconOption.笑脸, "function (){ location.href = 'UserIndex.aspx'; }");
                    return;
                }
                
            }
            else
            {
                spanpwd1.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                spanpwd1.InnerText = "原密码错误";
                return;
            }
        }
    }
}