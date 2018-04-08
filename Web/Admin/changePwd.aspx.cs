using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_changePwd : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnModify_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_AdminUser User = new Hi.BLL.SYS_AdminUser().GetModel(UserID);
        string OldLoginPwd = this.txtOldPassWord.Value.Trim().ToString();
        string NewLoginPwd = this.txtNewPassWord.Value.Trim().ToString();
        string ConfrimNewPassWord = this.txtConfrimNewPassWord.Value.Trim();
        //修改的密码，两次填写不一致
        if (ConfrimNewPassWord == NewLoginPwd)
        {
            if (User.LoginPwd.ToString() == OldLoginPwd)
            {
                if (new Hi.BLL.SYS_AdminUser().UpdatePassWord(NewLoginPwd, UserID.ToString()))
                {
                    JScript.AlertMsgMo(this, "修改成功", "function(){ window.location.href='changePwd.aspx'; }");
                }
            }
            else
            {
                JScript.AlertMsg(this, "原始密码错误，请重新输入！");
                return;
            }
        }
        else
        {
            JScript.AlertMsg(this, "密码填写不一致！");
            return;
        }
    }
}