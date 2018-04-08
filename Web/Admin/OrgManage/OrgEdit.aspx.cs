using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgEdit : AdminPageBase
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
        if (KeyID > 0)
        {
            Hi.Model.BD_Org Org = new Hi.BLL.BD_Org().GetModel(KeyID);
            if (Org != null)
            {
                if (UserType == 3 || UserType == 4) {
                    if (Org.ID != OrgID)
                    {
                        Response.Write("数据错误");
                        Response.End();
                    }
                }
                Atitle.InnerText = "机构编辑";
                txtOrgName.Value = Org.OrgName;
                txtPrincipal.Value = Org.Principal;
                txtPhone.Value = Org.Phone;
                if (Org.IsEnabled == 0)
                {
                    rdEbleYes.Checked = false;
                    rdEbleNo.Checked = true;
                }
                txtSortIndex.Value = Org.SortIndex;
                txtRemark.Value = Org.Remark;
                List<Hi.Model.SYS_AdminUser> user = new Hi.BLL.SYS_AdminUser().GetList("", "  isnull(dr,0)=0 and  orgid='" + KeyID + "' and usertype in(3)", "");
                if (user.Count > 0)
                {
                    txtUsername.Disabled = true;
                    txtUserPhone.Disabled = true;
                    txtUsername.Value = user[0].LoginName;
                    txtUserPhone.Value = user[0].Phone;
                    txtUserTrueName.Value = user[0].TrueName;
                    txtUpwd.Attributes.Add("value", user[0].LoginPwd);
                    txtUpwds.Attributes.Add("value", user[0].LoginPwd);
                }
            }
            else
            {
                Response.Write("数据错误");
                Response.End();
            }
        }
        else
        {
            txtUpwd.Attributes.Add("value", "123456");
            txtUpwds.Attributes.Add("value", "123456");
        }
    }



    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (KeyID > 0)
        {
            Hi.Model.BD_Org org = new Hi.BLL.BD_Org().GetModel(KeyID);
            if (org != null)
            {
                if (Common.OrgExistsAttribute("OrgName", txtOrgName.Value.Trim(), KeyID.ToString()))
                {
                    JScript.AlertMsg(this, "该机构名称已存在。");
                    return;
                }
                org.OrgName = Common.NoHTML(txtOrgName.Value.Trim());
                org.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
                org.Phone = Common.NoHTML(txtPhone.Value.Trim());
                org.IsEnabled = rdEbleYes.Checked ? 1 : 0;
                org.SortIndex = Common.NoHTML(txtSortIndex.Value.Trim());
                org.Remark = Common.NoHTML(txtRemark.Value.Trim());
                org.ts = DateTime.Now;
                org.modifyuser = UserID;
                if (new Hi.BLL.BD_Org().Update(org))
                {
                    List<Hi.Model.SYS_AdminUser> user = new Hi.BLL.SYS_AdminUser().GetList("", "  isnull(dr,0)=0 and  orgid='" + KeyID + "' and usertype in(3)", "");
                    if (user.Count > 0)
                    {
                        if (user[0].LoginPwd != txtUpwd.Text.Trim())
                        {
                            user[0].LoginPwd = Util.md5(txtUpwd.Text.Trim());
                        }
                        user[0].TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                        user[0].IsEnabled = rdEbleYes.Checked ? 1 : 0;
                        user[0].ts = DateTime.Now;
                        user[0].modifyuser = UserID;
                        new Hi.BLL.SYS_AdminUser().Update(user[0]);
                        Response.Redirect("OrgInfo.aspx?KeyID=" + KeyID+ "&type=2&page=1");
                    }
                    else
                    {
                        Hi.Model.SYS_AdminUser  userModel = new Hi.Model.SYS_AdminUser();
                        userModel.OrgID = KeyID;
                        userModel.UserType = 3;
                        userModel.IsEnabled = rdEbleYes.Checked ? 1 : 0;
                        userModel.LoginName = Common.NoHTML(txtUsername.Value.Trim());
                        userModel.LoginPwd = Util.md5(txtUpwd.Text.Trim());
                        userModel.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                        userModel.TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                        userModel.CreateDate = DateTime.Now;
                        userModel.CreateUserID = UserID;
                        userModel.ts = DateTime.Now;
                        userModel.modifyuser = UserID;
                        if (new Hi.BLL.SYS_AdminUser().Add(userModel) > 0)
                        {
                            Response.Redirect("OrgInfo.aspx?KeyID=" + KeyID + "&type=2&page=1");
                        }
                        else
                        {
                            new Hi.BLL.BD_Org().Delete(KeyID);
                        }
                    }
                }
            }
        }
        else
        {
            if (Common.OrgExistsAttribute("OrgName", txtOrgName.Value.Trim()))
            {
                JScript.AlertMsg(this, "该机构名称已存在。");
                return;
            }
            if (Common.SysUserExistsAttribute("LoginName", txtUsername.Value.Trim()))
            {
                JScript.AlertMsg(this, "该登录帐号已存在。");
                return;
            }
            Hi.Model.BD_Org org = new Hi.Model.BD_Org();
            org.OrgName = Common.NoHTML(txtOrgName.Value.Trim());
            org.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
            org.Phone = Common.NoHTML(txtPhone.Value.Trim());
            org.IsEnabled = rdEbleYes.Checked ? 1 : 0;
            org.SortIndex = Common.NoHTML(txtSortIndex.Value.Trim());
            org.Remark = Common.NoHTML(txtRemark.Value.Trim());
            org.ts = DateTime.Now;
            org.modifyuser = UserID;
            int Orgid = 0;
            if ((Orgid = new Hi.BLL.BD_Org().Add(org)) > 0)
            {
                Hi.Model.SYS_AdminUser user = new Hi.Model.SYS_AdminUser();
                user.OrgID = Orgid;
                user.UserType = 3;
                user.IsEnabled = rdEbleYes.Checked ? 1 : 0;
                user.LoginName = Common.NoHTML(txtUsername.Value.Trim());
                user.LoginPwd = Util.md5(txtUpwd.Text.Trim());
                user.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                user.TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                user.CreateDate = DateTime.Now;
                user.CreateUserID = UserID;
                user.ts = DateTime.Now;
                user.modifyuser = UserID;
                if (new Hi.BLL.SYS_AdminUser().Add(user) > 0)
                {
                    Response.Redirect("OrgInfo.aspx?KeyID=" + Orgid + "&type=2&page=1");
                }
                else
                {
                    new Hi.BLL.BD_Org().Delete(Orgid);
                }
            }
        }
    }
}