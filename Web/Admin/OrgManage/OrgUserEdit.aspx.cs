using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgUserEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        Hi.Model.SYS_AdminUser user = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
        if (user != null)
        {
            if (user.UserType != 3 && user.UserType != 4)
            {
                Response.Write("该用户不是机构用户");
            }
            txtLoginName.Value = user.LoginName;
            txtTrueName.Value = user.TrueName;
            txtPhone.Value = user.Phone;
            if (user.IsEnabled == 0)
            {
                rdEbleYes.Checked = false;
                rdEbleNo.Checked = true;
            }
            txtUpwd.Attributes.Add("value",user.LoginPwd);
        }
        else
        {
            Response.Write("数据不存在");
            Response.End();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_AdminUser user = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
        if (user != null)
        {
            if (txtUpwd.Text.Trim() != user.LoginPwd)
            {
                user.LoginPwd = Util.md5(txtUpwd.Text.Trim());
            }
            user.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
            user.IsEnabled = rdEbleYes.Checked ? 1 : 0;
            user.ts = DateTime.Now;
            user.modifyuser = UserID;
            if (new Hi.BLL.SYS_AdminUser().Update(user))
            {
                Response.Redirect("OrgUserInfo.aspx?KeyID=" + KeyID);
            }
        }
    }
}