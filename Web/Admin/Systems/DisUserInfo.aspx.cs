using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_DisUserInfo : AdminPageBase
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
        if (KeyID!=0)
        {
            int pageCount = 0;
            int Counts = 0;
            string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
            DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", false, " users.Tel,Identitys,Email,Address,SYS_CompUser.id,UserName,SYS_CompUser.compid,trueName,utype Type,Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate,SYS_CompUser.DisID ,OpenID,Sex ", JoinTableStr, "  and SYS_CompUser.id=" + KeyID + " and exists(select 1 from BD_distributor bd where isnull(bd.dr,0)=0 and bd.id= SYS_CompUser.disid) ", out pageCount, out Counts);
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
                JScript.AlertMsgMo(this, "该用户不存在！", " function (){ location.href='DisUserList.aspx' }");
                return;
            }
        }
        else
        {
            JScript.AlertMsgMo(this, "数据错误！", " function (){ location.href='DisUserList.aspx' }");
            return;
        }
    }
}