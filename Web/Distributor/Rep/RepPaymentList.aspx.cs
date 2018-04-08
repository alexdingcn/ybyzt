

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_Rep_RepPaymentList : DisPageBase
{
    public string page = "1";//默认初始页
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal tb = 0;
    public decimal tc = 0;


    public string Digits = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");


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
        string sqldate = string.Empty;

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

        string sql = string.Format(@"select l.hospital,sum(ld.sumAmount) sumAmount,sum(p.sumAmount) PaymentAmount
,(isnull(sum(ld.sumAmount),0)-isnull(sum(p.sumAmount),0)) Amount,l.MoneyDate from YZT_LibraryDetail ld 
left join YZT_Library l on ld.LibraryID=l.ID 
left join (select Sum(sumAmount) sumAmount,p.hospital,p.DisID,pd.LibraryID  from YZT_Payment p 
left join YZT_PaymentDetail pd on pd.PaymentID=p.ID  where ISNULL(pd.dr,0)=0 and ISNULL(p.dr,0)=0
 and ISNULL(p.IState,0)=1
group by p.hospital,p.DisID,pd.LibraryID) p on p.hospital=l.hospital and ld.ID=p.LibraryID
where l.DisID={0} {1}
group by l.hospital,l.MoneyDate order by l.hospital", this.DisID, strwhere);

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["Amount"].ToString() == "" ? "0" : ds.Rows[i]["Amount"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["sumAmount"].ToString() == "" ? "0" : ds.Rows[i]["sumAmount"].ToString());
            tc += Convert.ToDecimal(ds.Rows[i]["PaymentAmount"].ToString() == "" ? "0" : ds.Rows[i]["PaymentAmount"].ToString());
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
    protected void A_Seek(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txthospital.Value.Trim() != "")
        {
            strWhere += " and l.hospital like '%" + Common.NoHTML(this.txthospital.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (this.txtArriveDate.Value.Trim() != "")
        {
            strWhere += " and l.LibraryDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            strWhere += " and l.LibraryDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        return strWhere;
        
    }
}