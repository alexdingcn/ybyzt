using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_Systems_RepCusList : AdminPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public decimal tb = 0;

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
            this.txtPager.Value = Common.PageSize;
            this.txtBCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere = " and isnull(dr,0)=0";

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
            strwhere = " and CompID in (select ID from [dbo].[BD_Company] where  isnull(dr,0)=0 "+ whereIn + ")";
        }
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        //if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        //{
        //    strwhere += " and CreateDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        string sql = "SELECT CompID,DisID,Sum([AuditAmount]) as [AuditAmount] ,Sum(PayedAmount) as PayedAmount FROM [dbo].[DIS_Order] " +
                     "where isnull(dr,0)=0 and Otype!=9 and  OState in(2,4,5) " + 
                     strwhere + " group by compid,disid order by compid,disid";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["PayedAmount"].ToString() == "" ? "0" : ds.Rows[i]["PayedAmount"].ToString());
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtDisName.Value.Trim() != "")
        {
            string id = "0";
            string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and DisName like '%" + Common.NoHTML(this.txtDisName.Value.Trim().ToString().Replace("'", "''")) + "%'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    id += "," + dr["ID"].ToString();
                }
            }
            strWhere += " and DisID in (" + id + ") ";
        }

        if (this.txtBCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + Common.NoHTML(txtCompName.Value.Trim()) + "%')";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + Common.NoHTML(org)+ "' and SalesManID=" + SalesManID + ")";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        return strWhere;       
    }
}