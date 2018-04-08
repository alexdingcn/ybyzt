using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        if (this.txtPageSize.Value.ToString() != "" && int.TryParse(txtPageSize.Value.Trim(), out pageCount))
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_Org> Lorg = new Hi.BLL.BD_Org().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Org.DataSource = Lorg;
        this.Rpt_Org.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }

    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(txtOrgName.Value.Trim()))
        {
            where += " and ( OrgName like '%" + Common.NoHTML(txtOrgName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.ddrOtype.Value != "-1")
        {
            where += " and IsEnabled=" + this.ddrOtype.Value;
        }
        if (!string.IsNullOrEmpty(txtPrincipal.Value.Trim()))
        {

            where += " and Principal like '%" + Common.NoHTML(txtPrincipal.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and Phone like '%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (UserType == 3)
        {
            where += " and id='" + OrgID + "'";
        }
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
}