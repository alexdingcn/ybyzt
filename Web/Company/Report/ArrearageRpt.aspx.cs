using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_ArrearageRpt : CompPageBase
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
            //add by 0512
            if (Request["type"] != null)
            {
                if (Request["type"].ToString() == "1")
                {
                    this.txtCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                    this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

                    btnSearch_Click(null, null);
                }
            }
            else
            {
                this.txtPager.Value = Common.PageSize;
                Bind();
            }
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
        string sql = "select compID,DisID,sum(AuditAmount-PayedAmount) AuditAmount,sum( CASE WHEN DiffYear='1' THEN AuditAmount-PayedAmount ELSE 0 END) year1," +
        "sum( CASE WHEN DiffYear=2 THEN AuditAmount-PayedAmount ELSE 0 END) year2,sum( CASE WHEN DiffYear<>2 and DiffYear<>1 THEN AuditAmount-PayedAmount ELSE 0 END) year3 ,DisAccount from(" +
         "select * from [dbo].[ArrearageRpt_view] where " +
         " CompID=" + this.CompID +" and Otype!=9 " +
         strwhere +")M " +
         " where compID=" + this.CompID +
         " group by compID,DisID,DisAccount order by compID,DisID";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString());
            tb += Convert.ToDecimal(((Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(ds.Rows[i]["DisID"].ToString().ToInt(0), CompID))) > 0 ? (Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(ds.Rows[i]["DisID"].ToString().ToInt(0), CompID))).ToString() : "0").ToString());
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

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

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" + txtDisName.Value.Trim().Replace("'", "") + "%')";
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }

        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        return strWhere;        
    }
}