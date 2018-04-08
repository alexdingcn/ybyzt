using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_ChangePwd : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnModify_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(UserID);
        string OldLoginPwd = this.txtOldPassWord.Value.Trim().ToString();
        string NewLoginPwd = this.txtNewPassWord.Value.Trim().ToString();
        string ConfrimNewPassWord = this.txtConfrimNewPassWord.Value.Trim();
        //修改的密码，两次填写不一致
        if (ConfrimNewPassWord == NewLoginPwd)
        {
            if (User.UserPwd.ToString() == OldLoginPwd)
            {
                if (OldLoginPwd == NewLoginPwd)
                {
                    JScript.AlertMsgOne(this, "新密码不能跟老密码一样，请重新输入！", JScript.IconOption.错误);
                    return;
                }
                else if (NewLoginPwd == Util.md5("123456"))
                {
                    JScript.AlertMsgOne(this, "新密码不能设置为系统默认密码，请重新输入！", JScript.IconOption.错误);
                    return;
                }
                else
                {
                    if (new Hi.BLL.SYS_Users().UpdatePassWord(NewLoginPwd, UserID.ToString()))
                    {
                        ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>$(window.parent.leftFrame.document).find('.menuson').css('display','none');</script>");
                        JScript.AlertMethod(this, "修改成功", JScript.IconOption.正确, "function(){ window.location.href='jsc.aspx'; }");

                    }
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "原始密码错误，请重新输入！", JScript.IconOption.错误);
                return;
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "密码填写不一致！", JScript.IconOption.错误);
            return;
        }
    }
}