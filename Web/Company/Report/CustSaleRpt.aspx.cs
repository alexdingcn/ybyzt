using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_CustSaleRpt : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public decimal tb = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
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

        string sql = "SELECT DisID,Sum([AuditAmount]) as [AuditAmount] ,Sum(PayedAmount) as PayedAmount FROM  [dbo].[DIS_Order] " +
                     "where isnull(dr,0)=0 and Otype<>9 and compID=" + this.CompID +
                     strwhere + " group by disid order by disid";

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

        //Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
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
            string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and  DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
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
        if (this.ddrOState.Value != "-2")
        {
            strWhere += " and OState=" + this.ddrOState.Value;
        }
        else
        {
            strWhere += " and OState in(2,4,5)";
        }

        if (this.txtBCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        return strWhere;
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetPayState(string id)
    {
        string payState = Common.GetDis(id.ToInt(0), "PayState");
        string state = string.Empty;
        switch (payState)
        {
            case "1":
                state = "部分支付";
                break;
            case "2":
                state = "已支付";
                break;
            case "0":
                state = "未支付";
                break;
        }
        return state;
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetOState(string id)
    {
        string payState = Common.GetDis(id.ToInt(0), "OState");
        string state = string.Empty;
        switch (payState)
        {
            case "0":
                state = "";
                break;
            case "1":
                state = "";
                break;
            case "2":
                state = "";
                break;
        }
        return state;
    }
}