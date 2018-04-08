using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;

public partial class Admin_Systems_UserInfo : AdminPageBase
{

    public int RoleID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        RoleID = Convert.ToInt32(Request["RoleID"]);
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_AdminUser adminuser = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
            lblusername.InnerText = adminuser.LoginName;
            lbltruename.InnerText = adminuser.TrueName;
            lblphone.InnerText = adminuser.Phone;
            lblRemark.InnerText = adminuser.Remark;
            lblstate.InnerText = adminuser.IsEnabled == 1 ? "启用" : "禁用";
        }
        else
        {
            Response.Redirect("../Role/RoleInfo.aspx?KeyID=" + RoleID);
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_AdminUser AdminUser = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
        if (AdminUser != null)
        {
            AdminUser.dr = 1;
            AdminUser.ts = DateTime.Now;
            AdminUser.modifyuser = UserID;
            if (new Hi.BLL.SYS_AdminUser().Delete(KeyID))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='UserList.aspx'; }");
                Response.Redirect("../Role/RoleInfo.aspx?KeyID=" + RoleID);
            }
        }
    }
}