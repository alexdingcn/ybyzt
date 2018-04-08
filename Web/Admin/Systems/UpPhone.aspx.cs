using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_UpPhone : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DataBind();
        }
    }

    public void DataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", false, " Users.id,Users.Phone ", JoinTableStr, "  and SYS_CompUser.Compid=" + KeyID + " and Ctype=1 and Utype=4 ", out pageCount, out Counts);
        if (LUser.Rows.Count > 0)
        {
            txt_OldPhone.Value = LUser.Rows[0]["Phone"].ToString();
            ViewState["Userid"] = LUser.Rows[0]["id"].ToString();
        }
        else
        {
            Response.Write("用户异常");
            Response.End();
        }
    }

    protected void btnSubMit_Click(object sender, EventArgs e)
    {
        if (ViewState["Userid"] != null)
        {
            if (string.IsNullOrWhiteSpace(txt_NewPhone.Value.Trim()))
            {
                JScript.AlertMsg(this, "新手机号码不能为空！。");
                return;
            }
            int Userid = ViewState["Userid"].ToString().ToInt(0);
            Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(Userid);
            if (User != null)
            {
                User.Phone = Common.NoHTML(txt_NewPhone.Value.Trim());
                new Hi.BLL.SYS_Users().Update(User);
                ClientScript.RegisterStartupScript(this.GetType(), "MSG", "<script>window.parent.location.href=window.parent.location.href; </script>");
            }
        }
    }
}