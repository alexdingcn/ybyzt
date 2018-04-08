using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_Systems_DisList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            page = Request.QueryString["page"].ToString();
            Pager.CurrentPageIndex = Convert.ToInt32(page);
        }

        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            //Common.BindOrgSale(Org, SaleMan, "全部");
            DataBinds();
        }
    }
    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
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

        //List<Hi.Model.BD_Distributor> LUser = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);

        const string joinTableStr = " BD_Distributor A left join SYS_CompUser cu on A.ID=cu.DisID";
        DataTable LUser = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "A.Createdate", true,string.Join(",", typeof(Hi.Model.BD_Distributor).GetProperties().Select(p => "A." + p.Name)), joinTableStr, SearchWhere(), out pageCount, out Counts);



        this.Rpt_Distribute.DataSource = LUser;
        
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        //this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
    public void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and isnull(A.dr,0)=0 ";
        //if (SalesManID >0 || OrgID > 0)
        //{
        //    string whereIn = string.Empty;
        //    if (OrgID > 0)
        //    {
        //        whereIn += "  and OrgID=" + OrgID + "";
        //    }
        //    if (SalesManID > 0)
        //    {
        //        whereIn += " and SalesManID=" + SalesManID + "";
        //    }
        //    where += " and CompID in (select ID from [dbo].[BD_Company] where  isnull(dr,0)=0 " + whereIn + ") ";
        //}

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and A.DisName like '%" + Common.NoHTML(txtDisName.Value.Trim().Replace("'", "''")) + "%'";
        }
        //if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        //{
        //    where += " and exists(select 1 from BD_Company  where id=CompID and CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        //}
        if (ddlAUState.SelectedValue != "-1")
        {
            where += " and cu.ISState='" + Common.NoHTML(ddlAUState.SelectedValue) + "'";
        }
        if (ddlState.SelectedValue != "-1")
        {
            where += " and cu.IsEnabled='" + Common.NoHTML(ddlState.SelectedValue)+ "'";
        }
        if (txtCreateDate.Value.Trim() != "")
        {
            where += " and A.createdate>='" + Convert.ToDateTime(txtCreateDate.Value.Trim()) + "' ";
        }
        if (txtEndCreateDate.Value.Trim() != "")
        {
            where += " and A.createdate<='" + Convert.ToDateTime(txtEndCreateDate.Value.Trim()).AddDays(1) + "' ";
        }
        //if (Org.SelectedValue != "-1")
        //{
        //    string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
        //    where += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + org + "')";
        //}
        //if (salemanid.Value != "-1"&&salemanid.Value!="")
        //{
        //    where += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + salemanid.Value + "')";
        //}
        return where;
    }
}