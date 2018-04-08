using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Distributor_CMerchants_CMerchantsList : DisPageBase
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
        string strwhere = "and dr=0 and isnull(IsEnabled,0)=1 and (ISNULL(ForceDate,0)=0 or ForceDate <= getdate() ) and (ISNULL(InvalidDate,0)=0 or InvalidDate>=getdate())";

        if (ViewState["strwhere"] !=null) {
            strwhere += ViewState["strwhere"].ToString();
        }

        string compstr = this.ddrComp.Value != "" ? " and fca.CompID=" + this.ddrComp.Value : "";
        string compstr1 = this.ddrComp.Value != "" ? " and CompID=" + this.ddrComp.Value : "";

        strwhere += " and id in ( select fca.CMID from YZT_FCArea fca left join BD_Distributor dis on (fca.Province+fca.City+fca.Area=dis.Province+dis.City+dis.Area or fca.Province+fca.City=dis.Province+dis.City or fca.Province=dis.Province) and dis.IsEnabled=1 where 1=1 and  dis.ID= " + this.DisID + compstr + " union select fcd.CMID from YZT_FCDis fcd where fcd.DisID=" + this.DisID + compstr1 + " union select ID from YZT_CMerchants where type=1 and dr=0" + compstr1 + ")";

        if (this.ddrComp.Value != "")
        {
            strwhere += " and CompID=" + this.ddrComp.Value;
        }


        List<Hi.Model.YZT_CMerchants> l = CMerchantsBll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;
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
            strWhere += " and CMName like '%" + this.txtCMName.Value.Trim() + "%'";
        }
        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and GoodsName like '%" + this.txtGoodsName.Value.Trim() + "%'";
        }
        //if (this.ddrState.Value != "") {
        //    strWhere += " and IsEnabled =" + this.ddrState.Value.Trim();
        //}
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and ForceDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and ForceDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

}