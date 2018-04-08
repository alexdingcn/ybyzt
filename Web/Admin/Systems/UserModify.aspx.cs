using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_UserModify : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Action = Request["Action"] + "";
            string OrgID = Request["OrgID"] + "";
            if (Action == "Action")
            {
                Response.Write(Common.getsaleman(OrgID, true));
                Response.End();
            }
            Common.BindOrgSale(Org, SaleMan, "全部");
            Databind();
        }
    }

    public void Databind()
    {
        if (KeyID != 0)
        {
            txtusername.Style["display"] = "none";
            Hi.Model.SYS_AdminUser adminuser = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
            try
            {
                lblusername.InnerText = adminuser.LoginName;
                txtusername.Value = adminuser.LoginName;
                txtpwd.Attributes.Add("value", adminuser.LoginPwd);
                txtpwd2.Attributes.Add("value", adminuser.LoginPwd);

                txtturename.Value = adminuser.TrueName;
                txttel.Value = adminuser.Phone;
                txtRemark.Value = adminuser.Remark;
                int status = adminuser.IsEnabled;
                this.rdoStatus1.Checked = (status != 1);
                this.rdoStatus0.Checked = (status == 1);
            }
            catch (Exception ex)
            {

            }
        }


    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_AdminUser Adminuser = null;

        if (txtpwd.Text.Trim() != txtpwd2.Text.Trim())
        {
            JScript.AlertMsg(this, "两次密码不一致，请确认!");
            return;
        }
        if (KeyID != 0)
        {
            Adminuser = new Hi.BLL.SYS_AdminUser().GetModel(KeyID);
            if (Adminuser.LoginPwd !=Common.NoHTML( txtpwd.Text.Trim()))
            {
                Adminuser.LoginPwd = Util.md5(Common.NoHTML(txtpwd.Text.Trim()));
            }
            Adminuser.TrueName =Common.NoHTML( txtturename.Value.Trim());
            Adminuser.Phone = Common.NoHTML( txttel.Value.Trim());
            Adminuser.Remark =Common.NoHTML( txtRemark.Value.Trim());
            if (this.rdoStatus1.Checked)
                Adminuser.IsEnabled = 0;
            else
                Adminuser.IsEnabled = 1;
            if (this.rdotype2.Checked)
            {
                Adminuser.UserType = 2;//系统用户
            }
            if (salemanid.Value != "0")
            {
                Adminuser.UserType = 4;//系统用户
            }
            Adminuser.ts = DateTime.Now;
            Adminuser.modifyuser = UserID;
            Adminuser.SalesManID = Convert.ToInt32(salemanid.Value);
            Adminuser.OrgID = Convert.ToInt32(orgids.Value);
            if (new Hi.BLL.SYS_AdminUser().Update(Adminuser))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='UserInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("UserInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {
            Adminuser = new Hi.Model.SYS_AdminUser();
            Adminuser.LoginName = Common.NoHTML(txtusername.Value.Trim());
            if (DisExistsAttribute("LoginName", Adminuser.LoginName))
            {
                JScript.AlertMsg(this, "该登录帐号已存在。");
                return;
            }
            Adminuser.LoginPwd = Util.md5(txtpwd.Text.Trim());
            Adminuser.TrueName = Common.NoHTML(txtturename.Value.Trim());
            Adminuser.Phone = Common.NoHTML(txttel.Value.Trim());
            if (this.rdotype2.Checked)
            {
                Adminuser.UserType = 2;//系统用户
            }
            if (salemanid.Value != "0")
            {
                Adminuser.UserType = 4;//系统用户
            }
            Adminuser.Remark = Common.NoHTML(txtRemark.Value.Trim());
            Adminuser.RoleID = Convert.ToInt32(Request["RoleID"]);
            if (this.rdoStatus1.Checked)
                Adminuser.IsEnabled = 0;
            else
                Adminuser.IsEnabled = 1;

            //标准参数
            Adminuser.CreateDate = DateTime.Now;
            Adminuser.CreateUserID = UserID;
            Adminuser.ts = DateTime.Now;
            Adminuser.modifyuser = UserID;
            Adminuser.SalesManID =Convert.ToInt32(salemanid.Value);
            Adminuser.OrgID=Convert.ToInt32(orgids.Value);
            int newuserid = 0;
            newuserid = new Hi.BLL.SYS_AdminUser().Add(Adminuser);
            if (newuserid > 0)
            {
                Response.Redirect("../Role/RoleInfo.aspx?KeyID=" + Request["RoleID"]);
            }
        }
    }

    public bool DisExistsAttribute(string name, string value, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.SYS_AdminUser> Adminuser = new Hi.BLL.SYS_AdminUser().GetList("", " " + name + "='" + value + "' and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Adminuser.Count > 0)
            {
                exists = true;
            }
        }
        else
        {
            List<Hi.Model.SYS_AdminUser> Adminuser = new Hi.BLL.SYS_AdminUser().GetList("", " " + name + "='" + value + "' and isnull(dr,0)=0 ", "");
            if (Adminuser.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }


}