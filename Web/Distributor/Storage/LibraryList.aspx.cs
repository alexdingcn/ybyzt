using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Storage_LibraryList : DisPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
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
        DataTable list = new Hi.BLL.YZT_Library().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, strwhere, out pageCount, out Counts, Pager.RecordCount);
        this.rptOrder.DataSource = list;
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
        string strWhere = " and l.DisID ="+ DisID + " and l.dr=0";

        if (this.txtLibraryNO.Value.Trim() != "")
        {
            strWhere += " and l.LibraryNO like '%" + this.txtLibraryNO.Value.Trim() + "%'";
        }
        
        if (this.ddrState.Value != "")
        {
            strWhere += " and l.IState =" + this.ddrState.Value.Trim();
        }
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