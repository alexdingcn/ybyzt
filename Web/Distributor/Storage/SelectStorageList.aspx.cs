using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Distributor_Storage_SelectOrderOut : DisPageBase
{
    Hi.BLL.YZT_CMerchants CMerchantsBll = new Hi.BLL.YZT_CMerchants();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.index.Value = Request.QueryString["index"];
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = "and s.StockNum-s.StockUseNum>0" + Where();
        DataTable dt = new Hi.BLL.YZT_GoodsStock().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts, Pager.RecordCount);
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
        string strWhere = " and DisID=" + DisID + " and s.dr=0  and StockNum>0";

        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and s.GoodsName like '%" + this.txtGoodsName.Value.Trim() + "%'";
        }
        return strWhere;
    }


}