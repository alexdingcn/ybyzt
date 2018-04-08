using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_SysManager_DisUserInfo : CompPageBase
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
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", false, " users.Tel,Identitys,Email,Address,SYS_CompUser.id,UserName,SYS_CompUser.compid,trueName,utype Type,Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate,SYS_CompUser.DisID ,OpenID,Sex ", JoinTableStr, "  and SYS_CompUser.id=" + KeyID + "", out pageCount, out Counts);
        if (LUser.Rows.Count > 0)
        {
            lblDisName.InnerText = Common.GetDisValue(LUser.Rows[0]["DisID"].ToString().ToInt(0), "disname").ToString();
            lblUname.InnerText = LUser.Rows[0]["UserName"].ToString();
            lblOpenID.InnerText = LUser.Rows[0]["OpenID"].ToString();
            lblSex.InnerText = LUser.Rows[0]["Sex"].ToString();
            lblPhone.InnerText = LUser.Rows[0]["Phone"].ToString();
            lblTel.InnerText = LUser.Rows[0]["Tel"].ToString();
            lblIdentitys.InnerText = LUser.Rows[0]["Identitys"].ToString();
            lblTrueName.InnerText = LUser.Rows[0]["TrueName"].ToString();
            lblIsEnabled.InnerHtml = LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1 ? "启用" : "<i style='color:red;'>禁用</i>";
            lblEmail.InnerText = LUser.Rows[0]["Email"].ToString();
            //lblAddress.InnerText = user.Address;
        }
        else
        {
            JScript.AlertMethod(this, "该用户不存在！", JScript.IconOption.错误, " function (){ location.href='DisUserList.aspx' }");
            return;
        }
    }

    protected void btn_Enabled(object sender, EventArgs e)
    {
        Hi.Model.SYS_CompUser user = new Hi.BLL.SYS_CompUser().GetModel(KeyID);
        if (user != null)
        {
            if (user.IsEnabled==1)
            {
                JScript.AlertMethod(this, "用户已是启用状态！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
            }
            else
            {
                user.IsEnabled = 1;
                user.modifyuser = UserID;
                user.ts = DateTime.Now;
                if (new Hi.BLL.SYS_CompUser().Update(user))
                {
                    JScript.AlertMethod(this, "用户启用成功", JScript.IconOption.正确, "function(){ window.location.href=window.location.href; }");
                }
            }
            
        }
    }

    protected void btn_NEnabled(object sender, EventArgs e)
    {
        Hi.Model.SYS_CompUser user = new Hi.BLL.SYS_CompUser().GetModel(KeyID);
        if (user != null)
        {
            if (user.IsEnabled == 1)
            {
                user.IsEnabled = 0;
                user.modifyuser = UserID;
                user.ts = DateTime.Now;
                if (new Hi.BLL.SYS_CompUser().Update(user))
                {
                    JScript.AlertMethod(this, "用户禁用成功", JScript.IconOption.正确, "function(){ window.location.href=window.location.href; }");
                }
            }
            else
            {
                JScript.AlertMethod(this, "用户已是禁用状态！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
            }
            
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_CompUser user = new Hi.BLL.SYS_CompUser().GetModel(KeyID);
        if (user != null)
        {
            user.dr = 1;
            user.modifyuser = UserID;
            user.ts = DateTime.Now;
            if (new Hi.BLL.SYS_CompUser().Update(user))
            {
                JScript.AlertMethod(this, "用户删除成功！", JScript.IconOption.正确, "function(){ window.location.href='DisUserList.aspx'; }");
            }
        }
    }
}