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
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable ht = new Hi.BLL.YZT_Library().getHtDrop(DisID.ToString());
            this.HtDrop.DataSource = ht;
            this.HtDrop.DataTextField = "hospital";
            this.HtDrop.DataValueField = "hospital";
            this.HtDrop.DataBind();
            this.index.Value = Request.QueryString["index"];
            this.hidhtId.Value = Request["id"] + "";
            this.HtDrop.SelectedValue = Request["id"] + "";
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
        string strwhere = Where();
        DataTable dt = new Hi.BLL.YZT_LibraryDetail().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts, Pager.RecordCount);
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
        string strWhere = " and l.DisID=" + DisID + " and l.dr=0 and d.OutNum>0 and l.IState=1";
        if (!string.IsNullOrWhiteSpace(HtDrop.SelectedValue))
        {
            strWhere += " and l.hospital = '" + this.HtDrop.SelectedValue + "'";
        }

        //if (this.LibraryNO.Value.Trim() != "")
        //{
        //    strWhere += " and l.LibraryNO like '%" + this.LibraryNO.Value.Trim() + "%'";
        //}
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and l.LibraryDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and l.LibraryDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }


}