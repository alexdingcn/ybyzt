using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Systems_CompUserEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID > 0)
        {
            int pageCount = 0;
            int Counts = 0;
            string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
            DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", false, "UserPwd, users.Tel,Identitys,Email,Address,SYS_CompUser.id,UserName,SYS_CompUser.compid,trueName,utype Type,Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate,SYS_CompUser.CompID ,OpenID,Sex ", JoinTableStr, "  and SYS_CompUser.id=" + KeyID + "", out pageCount, out Counts);
            if (LUser.Rows.Count > 0)
            {
                txtUname.Value = LUser.Rows[0]["UserName"].ToString();
                txtTrueName.Value = LUser.Rows[0]["UserName"].ToString();
                txtOpenID.Value = LUser.Rows[0]["OpenID"].ToString();
                txtUpwd.Attributes.Add("value",LUser.Rows[0]["UserPwd"].ToString());
                if (LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) == 0)
                {
                    rdEnabledYes.Checked = false;
                    rdEnabledNo.Checked = true;
                }
                if (LUser.Rows[0]["Sex"].ToString() == "女")
                {
                    rdMan.Checked = false;
                    rdWomen.Checked = true;
                }
                txtMobil.Value = LUser.Rows[0]["Phone"].ToString();
                txtPhone.Value = LUser.Rows[0]["Tel"].ToString();
                txtPId.Value = LUser.Rows[0]["Identitys"].ToString();
                txtEmail.Value = LUser.Rows[0]["Email"].ToString();
                LUser.Rows[0]["Address"].ToString();
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (KeyID>0)
        {
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and id=" + KeyID + "", "");
            if (ListCompUser.Count > 0)
            {
                SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(ListCompUser[0].UserID);
                user.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
                user.OpenID = Common.NoHTML(txtOpenID.Value.Trim());
                if (rdMan.Checked)
                {
                    user.Sex = "男";
                }
                else
                {
                    user.Sex = "女";
                }
                user.Tel = Common.NoHTML(txtPhone.Value.Trim());
                if (txtUpwd.Text.Trim() != user.UserPwd)
                {
                    user.UserPwd = Util.md5(txtUpwd.Text.Trim());
                }
                user.Identitys = Common.NoHTML(txtPId.Value.Trim());
                ListCompUser[0].IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                user.Email = Common.NoHTML(txtEmail.Value.Trim());
                user.Address = Common.NoHTML(txtAddress.Value.Trim());
                new Hi.BLL.SYS_Users().Update(user,Tran);
                new Hi.BLL.SYS_CompUser().Update(ListCompUser[0], Tran);
                Tran.Commit();
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>window.location.href='CompUserInfo.aspx?KeyID=" + KeyID + "';</script>");

            }
        }
    }
}