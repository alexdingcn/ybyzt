using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_DisUserList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan, "全部");
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
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 inner join BD_Company on SYS_CompUser.CompID=BD_Company.ID and BD_Company.dr=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", true, "SYS_CompUser.CompID, SYS_CompUser.id,UserName,SYS_CompUser.Disid,trueName,utype Type,Users.Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate ", JoinTableStr, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Disuser.DataSource = LUser;
        this.Rpt_Disuser.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
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
        string where = " and utype in(1,5) and ctype=2  and exists(select 1 from BD_distributor bd where isnull(bd.dr,0)=0 and bd.id= SYS_CompUser.disid)  ";
        //if (SalesManID != null && SalesManID != 0)
        //{
        //    where += " and SYS_CompUser.CompID in (select ID from [dbo].[BD_Company] where orgid=" + OrgID + " and SalesManID=" + SalesManID + " and isnull(dr,0)=0)";
        //}
        if (SalesManID > 0 || OrgID > 0)
        {
            string whereIn = string.Empty;
            if (OrgID > 0)
            {
                whereIn += "  and OrgID=" + OrgID + "";
            }
            if (SalesManID > 0)
            {
                whereIn += " and SalesManID=" + SalesManID + "";
            }
            where += " and SYS_CompUser.CompID in (select ID from [dbo].[BD_Company] where  isnull(dr,0)=0 "+ whereIn + ")";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and exists(select 1 from BD_Distributor where dr=0 and  id=SYS_CompUser.DisID and Disname like '%" + Common.NoHTML(txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtuname.Value.Trim()))
        {
            where += " and username like '%" + Common.NoHTML(txtuname.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and SYS_CompUser.IsEnabled='" + Common.NoHTML(sltIsAllow.SelectedValue) + "'";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            where += " and exists(select 1 from BD_Company where dr=0 and  id=SYS_CompUser.CompID and CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (txtCreateDate.Value.Trim() != "")
        {
            where += " and SYS_CompUser.createdate>='" + Convert.ToDateTime(txtCreateDate.Value.Trim()) + "' ";
        }
        if (txtEndCreateDate.Value.Trim() != "")
        {
            where += " and SYS_CompUser.createdate<='" + Convert.ToDateTime(txtEndCreateDate.Value.Trim()).AddDays(1) + "' ";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and OrgID='" + Common.NoHTML(org) + "')";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        return where;
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
}