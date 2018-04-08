using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Company_CMerchants_FirstCampList : CompPageBase
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
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int Counts=0;
        string strwhere = " and cm.CompID=" + this.CompID + "";

        if (ViewState["strwhere"] !=null) {
            strwhere += ViewState["strwhere"].ToString();
        }
        string str = string.Empty;
        if (Request["type"] + "" == "1")
        {
            strwhere += " and fcnum>0";
            str += " and State<>2";
        }

        string sql = string.Format(@"select * from (
select cm.ID,cm.CMCode,cm.CompID,cm.CMName,g.CategoryID,cm.GoodsID,info.BarCode GoodsCode,g.GoodsName,cm.ProvideData,info.ValueInfo,cm.InvalidDate,cm.ForceDate,(select COUNT(*) from YZT_FirstCamp fc where fc.CMID=cm.ID {1}) fcnum
from YZT_CMerchants cm left join BD_GoodsInfo info  on info.ID=cm.GoodsID
left join BD_Goods g on info.GoodsID=g.ID
 where cm.dr=0 ) cm where 1=1 {0}", strwhere, str);

        if (strwhere != "")
        {
            sql += strwhere;
        }

        sql += " order by cm.ID desc";

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
            strWhere += " and cm.GoodsName like '%" + this.txtCMName.Value.Trim() + "%'";
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and cm.ForceDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and cm.ForceDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

}