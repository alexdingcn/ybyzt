using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_DisUserEdit : CompPageBase
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
        if (Request.QueryString["KeyID"] != null)
        {
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(KeyID);
            if (user != null)
            {
                if (user.CompID != CompID)
                {
                    Response.Write("对不起，您没有操作权限！");
                    Response.End();
                }
                txtUname.Value = user.UserName;
                txtTrueName.Value = user.UserName;
                txtOpenID.Value = user.OpenID;
                txtUpwd.Attributes.Add("value", user.UserPwd);
                if (user.IsEnabled == 0)
                {
                    rdEnabledYes.Checked = false;
                    rdEnabledNo.Checked = true;
                }
                if (txtUpwd.Text.Trim() != user.UserPwd)
                {
                    user.UserPwd = Util.md5(txtUpwd.Text.Trim());
                }
                if (user.Sex.Trim() == "女")
                {
                    rdMan.Checked = false;
                    rdWomen.Checked = true;
                }
                txtMobil.Value = user.Phone;
                txtPhone.Value = user.Tel;
                txtPId.Value = user.Identitys;
                txtEmail.Value = user.Email;
                txtAddress.Value = user.Address;
            }
        }
        else
        {
            Response.Write("用户不存在");
            Response.End();
        }
    }


    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["KeyID"] != null)
        {
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(KeyID);
            if (user != null)
            {
                user.TrueName = txtTrueName.Value.Trim();
                user.OpenID = txtOpenID.Value.Trim();
                if (rdMan.Checked)
                {
                    user.Sex = "男";
                }
                else
                {
                    user.Sex = "女";
                }
                user.Tel = txtPhone.Value.Trim();
                user.Identitys = txtPId.Value.Trim();
                if (rdEnabledYes.Checked)
                {
                    user.IsEnabled = 1;
                }
                else
                {
                    user.IsEnabled = 0;
                }
                user.Email = txtEmail.Value.Trim();
                user.Address = txtAddress.Value.Trim();
                if (new Hi.BLL.SYS_Users().Update(user))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>window.location.href='DisUserInfo.aspx?KeyID=" + KeyID + "';</script>");
                }
            }
        }
    }

}