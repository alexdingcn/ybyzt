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
        string cid = Request.QueryString["cid"];
        if (!string.IsNullOrWhiteSpace(cid)) {
            string strwhere = Where(cid);
            DataTable l =new Hi.BLL.DIS_OrderOut().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts);
            this.rptOrder.DataSource = l;
            this.rptOrder.DataBind();
            Pager.RecordCount = Counts;
            page = Pager.CurrentPageIndex.ToString();
        }
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

    public string Where(string cid)
    {
        string strWhere = " and o.compid="+ cid + " and o.DisID="+DisID+" ";

        if (this.ReceiptNo.Value.Trim() != "") {
            strWhere += " and o.ReceiptNo like '%" + this.ReceiptNo.Value.Trim() + "%'";
        }
       
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and o.SendDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and o.SendDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

}