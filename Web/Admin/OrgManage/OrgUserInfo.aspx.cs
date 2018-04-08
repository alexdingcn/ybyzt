using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgUserInfo : AdminPageBase
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
        if (user != null) {
            if (user.UserType != 3 && user.UserType != 4) {
                Response.Write("该用户不是机构用户");
            }
            lblLoginName.InnerText = user.LoginName;
            lblTrueName.InnerText = user.TrueName;
            lblOrgName.InnerText = Common.GetOrgValue(user.OrgID, "OrgName").ToString();
            lblUtype.InnerText = Common.GetUTypeName(user.UserType.ToString());
            lblIsEnabled.InnerHtml = user.IsEnabled == 1 ? "启用" : "<i style='color:red'>禁用</i>";
            lblPhone.InnerText = user.Phone;
            lblRemark.InnerText = user.Remark;
        }
        else
        {
            Response.Write("数据不存在");
            Response.End();
        }
    }
}