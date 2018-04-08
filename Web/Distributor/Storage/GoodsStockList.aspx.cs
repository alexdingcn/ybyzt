using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Distributor_Storage_GoodsStockList : DisPageBase
{
    Hi.BLL.YZT_CMerchants CMerchantsBll = new Hi.BLL.YZT_CMerchants();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            if (Request["type"] + "" == "1")
            {
                //当前时间
                DateTime now = DateTime.Now;
                //快到期时间
                DateTime today = now.AddDays(30);

                string goods_sql = string.Format(@"and s.validDate<='" + today + "'");
                ViewState["goods_sql"] = goods_sql;
            }
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

        if (ViewState["goods_sql"] != null) {
            strwhere += ViewState["goods_sql"].ToString();
        }

        DataTable dt=new Hi.BLL.YZT_GoodsStock().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts, Pager.RecordCount);

        //dt.DefaultView.Sort = "validDate ASC";

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

    public string isvalidDate(string validDate)
    {
        DateTime now = DateTime.Now;

        DateTime day=validDate.ToDateTime();

        TimeSpan ts = day - now;

        if (ts.TotalDays <= 30) {
            return "1";
        }
        return "";
    }
}