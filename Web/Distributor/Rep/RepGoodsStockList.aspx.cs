using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;



public partial class Distributor_Rep_RepGoodsStockList : DisPageBase
{
    Hi.BLL.YZT_CMerchants CMerchantsBll = new Hi.BLL.YZT_CMerchants();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount=0;
        int Counts=0;
        string strwhere = Where();
        DataTable dt=new Hi.BLL.YZT_GoodsStock().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts, Pager.RecordCount);
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind();
    }

    public string Where()
    {
        string strWhere = " and DisID=" + DisID + " and s.dr=0 ";

        if (this.txtGoodsName.Value.Trim() != "") {
            strWhere += " and s.GoodsName like '%" + this.txtGoodsName.Value.Trim() + "%'";
        }
        return strWhere;
    }


    public string Ld(string stockID) {
        string msg = "0";

        DateTime now = DateTime.Now;
        DateTime sanday = now.AddDays(-30);

        string sql = string.Format(@"select isnull(Avg(ld.OutNum),0)  OutNum from YZT_LibraryDetail ld left join  YZT_Library l on ld.LibraryID=l.ID where ISNULL(l.dr,0)=0 and l.IState=1 and ld.DisID={0} and ld.StockID={1} and ISNULL(ld.dr,0)=0 and ld.CreateDate>='{2}'", this.DisID, stockID, sanday);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dt != null && dt.Rows.Count > 0) {
            msg = dt.Rows[0]["OutNum"].ToString().ToDecimal(0).ToString("0");
        }

        return msg;
    }
}