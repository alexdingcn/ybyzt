using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_SaleManList : AdminPageBase
{
    public string page = "1";//默认初始页
    public DropDownList allSaleMan = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.BindOrgSale(allorg, allSaleMan,"全部");
            Bind();
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;

        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
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
        List<Hi.Model.BD_SalesMan> LAdminuser = new Hi.BLL.BD_SalesMan().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = LAdminuser;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.allorg.SelectedValue = this.OrgID == 0 ? allorg.SelectedValue : this.OrgID.ToString();
    }
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
    public void btn_SearchClick(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(salename.Value.Trim()))
        {
            where += " and (SalesName like '%" + Common.NoHTML(salename.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(salecode.Value.Trim()))
        {
            where += " and (SalesCode like '%" + Common.NoHTML(salecode.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and IsEnabled='" + sltIsAllow.SelectedValue + "'";
        }
        if (allorg.SelectedValue != "" && allorg.SelectedValue!="-1")
        {
            string org = this.OrgID == 0 ? allorg.SelectedValue : this.OrgID.ToString();
            where += " and OrgID='" + org + "'";
        }
        if (UserType == 3)
        {
            where += " and orgid='" + OrgID + "'";
        }
        return where;
    }
}