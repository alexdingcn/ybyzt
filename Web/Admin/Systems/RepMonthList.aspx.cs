using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_Systems_RepMonthList : AdminPageBase
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
            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string date = string.Empty;
        string strwhere = string.Empty;

        if ( ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
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

        if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        {
            this.txtBCreateDate.Value = Convert.ToDateTime(DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1").ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            date = " and CreateDate>='" + DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1 0:0:0' ";
        }
        string sql = "select CompID,DisID,YEAR([CreateDate]) Years,MONTH([CreateDate]) as Months,SUM([sumAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount] " +
              "from ( SELECT * FROM [dbo].[MonthSaleRpt_view] where OState in(2,4,5) " + strwhere + date +
              ")M " +
              " group by YEAR([CreateDate]), MONTH([CreateDate]),CompID,disID order by YEAR([CreateDate]),MONTH([CreateDate])";

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["TotalAmount"].ToString() == "" ? "0" : ds.Rows[i]["TotalAmount"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["zdAmount"].ToString() == "" ? "0" : ds.Rows[i]["zdAmount"].ToString());
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
        string sqldate = string.Empty;
        string CompName = string.Empty;

        string DisName = string.Empty;
        if (this.txtDisName.Value.Trim() != "")
        {
            DisName = " and  DisName like '%" + Common.NoHTML(this.txtDisName.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }

        if (this.txtBCreateDate.Value.Trim() != "")
        {
            sqldate += " and CreateDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim())+"'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            sqldate += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
            
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            CompName += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            CompName += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + Common.NoHTML(org) + "')";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            CompName += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        return CompName + DisName + sqldate;

    }
}