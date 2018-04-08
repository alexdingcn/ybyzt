using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Distributor_CMerchants_FirstCampList : DisPageBase
{
    Hi.BLL.YZT_CMerchants CMerchantsBll = new Hi.BLL.YZT_CMerchants();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
            Common.ListFMComps(this.ddrComp, this.UserID.ToString(), this.DisID.ToString());

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
        string strwhere = string.Empty;

        if (ViewState["strwhere"] !=null) {
            strwhere = ViewState["strwhere"].ToString();
        }

        string sql = @"select fc.ID,fc.State,fc.CompID,fc.ForceDate,fc.InvalidDate,cm.CMCode,cm.CMName,
g.CategoryID,info.BarCode GoodsCode,g.GoodsName,cm.ProvideData,info.ValueInfo,fc.HtID,h.HospitalName from YZT_FirstCamp
 fc left join YZT_CMerchants cm on fc.CMID=cm.ID 
 left join BD_GoodsInfo info on info.ID=cm.GoodsID left join BD_Goods g on info.GoodsID=g.ID left join SYS_Hospital h on fc.HtID=h.ID where fc.dr=0 and fc.DisID=" + this.DisID + " and (ISNULL(cm.ForceDate,0)=0 or cm.ForceDate <= getdate() ) and (ISNULL(cm.InvalidDate,0)=0 or cm.InvalidDate>=getdate()) and 1=1 ";

        if (strwhere != "") {
            sql += strwhere;
        }
        if (this.ddrComp.Value != "")
        {
            sql += " and fc.CompID=" + this.ddrComp.Value;
        }

        sql += " order by fc.ID desc";

        MyPagination mypag = new MyPagination();
        DataTable dt = mypag.GetDt(Pager.CurrentPageIndex, Pager.PageSize, sql, sql, out Counts);

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

    public string Where()
    {
        string strWhere = string.Empty;

        if (this.txtCMName.Value.Trim() != "") {
            strWhere += " and cm.CMName like '%" + this.txtCMName.Value.Trim() + "%'";
        }
        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and cm.GoodsName like '%" + this.txtGoodsName.Value.Trim() + "%'";
        }
        if (this.ddrState.Value != "") {
            strWhere += " and fc.State =" + this.ddrState.Value.Trim();
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and fc.ForceDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and fc.ForceDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

}