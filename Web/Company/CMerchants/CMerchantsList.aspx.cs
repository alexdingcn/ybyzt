using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Company_CMerchants_CMerchantsList : CompPageBase
{
    Hi.BLL.YZT_CMerchants CMerchantsBll = new Hi.BLL.YZT_CMerchants();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            
            this.hidCompId.Value = this.CompID.ToString();
            this.txtPager.Value = Common.PageSize;
          
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
        string strwhere = " and dr=0" + " and CompID=" + this.CompID;

        if (ViewState["strwhere"] !=null) {
            strwhere += ViewState["strwhere"].ToString();
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
            strWhere += " and GoodsName like '%" + this.txtCMName.Value.Trim() + "%'";
        }
        if (this.ddrState.Value != "") {
            strWhere += " and IsEnabled =" + this.ddrState.Value.Trim();
        }
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