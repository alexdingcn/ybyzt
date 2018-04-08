using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgInfo : AdminPageBase
{
    public string page = "1";
    public string page1 = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request.QueryString["page1"] != null)
            {
                page1 = Request.QueryString["page1"].ToString();
                Pager1.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
            SaleManDataBind();
            UserDataBind();
        }
    }

    public void DataBinds()
    {
        Hi.Model.BD_Org Org = new Hi.BLL.BD_Org().GetModel(KeyID);
        if (Org != null)
        {
            if (UserType == 3 || UserType == 4)
            {
                if (Org.ID != OrgID)
                {
                    Response.Write("数据错误");
                    Response.End();
                }
            }
            if (Request["type"] == "3")
            {
                libtnUse.Visible = false;
                libtnNUse.Visible = false;
                libtnEdit.Visible = false;
                lblbtnback.Attributes["onclick"] = "javascript:window.location.href='OrgUserList.aspx?page=" + Request["page"] + "';";
            }
            if (Request["type"] == "2")
            {
                Atitle.InnerText = "机构查看";
                lblbtnback.Attributes["onclick"] = "javascript:window.location.href='OrgList.aspx?page=" + Request["page"] + "';";
            }

            if (Org.IsEnabled == 1)
            {
                libtnUse.Visible = false;
            }
            else
            {
                libtnNUse.Visible = false;
            }
            lblOrgName.InnerText = Org.OrgName;
            lblPrincipal.InnerText = Org.Principal;
            lblPhone.InnerText = Org.Phone;
            lblRemark.InnerText = Org.Remark;
            lblIsEnabled.InnerHtml = Org.IsEnabled == 1 ? "启用" : "<i style='color:red'>禁用</i>";
        }
        else
        {
            Response.Write("数据不存在");
            Response.End();
        }
    }

    public void SaleManDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.BD_SalesMan> LDis = new Hi.BLL.BD_SalesMan().GetList(Pager1.PageSize, Pager1.CurrentPageIndex, "id", false, " and isnull(dr,0)=0 and OrgID=" + KeyID + "", out pageCount, out Counts);
        this.Rpt_SalesMan.DataSource = LDis;
        this.Rpt_SalesMan.DataBind();
        Pager1.RecordCount = Counts;
        page1 = Pager1.CurrentPageIndex.ToString();
    }

    public void UserDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.SYS_AdminUser> LDis = new Hi.BLL.SYS_AdminUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, " and isnull(dr,0)=0  and OrgID=" + KeyID + " ", out pageCount, out Counts);
        this.Rpt_User.DataSource = LDis;
        this.Rpt_User.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }


    protected void btn_Use(object sender, EventArgs e)
    {
        Hi.Model.BD_Org Org = new Hi.BLL.BD_Org().GetModel(KeyID);
        if (Org != null)
        {
            Org.IsEnabled = 1;
            Org.ts = DateTime.Now;
            Org.modifyuser = UserID;
            if (new Hi.BLL.BD_Org().Update(Org))
            {
                List<Hi.Model.SYS_AdminUser> user = new Hi.BLL.SYS_AdminUser().GetList("", " isnull(dr,0)=0 and orgid=" + KeyID + "", "");
                foreach (Hi.Model.SYS_AdminUser model1 in user)
                {
                    model1.IsEnabled = 1;
                    model1.ts = DateTime.Now;
                    model1.modifyuser = UserID;
                    new Hi.BLL.SYS_AdminUser().Update(model1);
                }
                JScript.AlertMsgMo(this, "用户启用成功", "function(){ window.location.href='OrgList.aspx'; }");
            }
        }
    }
    protected void btn_NUse(object sender, EventArgs e)
    {
        Hi.Model.BD_Org Org = new Hi.BLL.BD_Org().GetModel(KeyID);
        if (Org != null)
        {
            Org.IsEnabled = 0;
            Org.ts = DateTime.Now;
            Org.modifyuser = UserID;
            if (new Hi.BLL.BD_Org().Update(Org))
            {
                List<Hi.Model.SYS_AdminUser> user = new Hi.BLL.SYS_AdminUser().GetList("", " isnull(dr,0)=0 and orgid=" + KeyID + "", "");
                foreach (Hi.Model.SYS_AdminUser model1 in user)
                {
                    model1.IsEnabled = 0;
                    model1.ts = DateTime.Now;
                    model1.modifyuser = UserID;
                    new Hi.BLL.SYS_AdminUser().Update(model1);
                }
                JScript.AlertMsgMo(this, "用户禁用成功", "function(){ window.location.href='OrgList.aspx'; }");
            }
        }
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        UserDataBind();
    }


    protected void Pager_PageChanged1(object sender, EventArgs e)
    {
        page1 = Pager1.CurrentPageIndex.ToString();
        SaleManDataBind();
    }
}