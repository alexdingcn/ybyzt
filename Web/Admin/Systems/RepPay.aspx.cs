using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_RepPay : AdminPageBase
{
    public string page = "1";//默认初始页
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
        int pageCount = 0;
        int Counts = 0;
        string strwhere = " and CompID in (select ID from [dbo].[BD_Company] where orgid="+OrgID+ " and SalesManID=" + SalesManID + " and isnull(dr,0)=0)";
        strwhere += " and OState in (2,4,5)  and PayState in(0,1)  and DisID='" + KeyID + "' and isnull(dr,0)=0 and Otype!=9 ";
        if (Request["Sdate"].ToString() != "")
        {
            strwhere += " and CreateDate>='" + Convert.ToDateTime(Request["Sdate"].ToString()) + "'";
        }
        if (Request["Edate"].ToString() != "")
        {
            strwhere += " and CreateDate<'" + Convert.ToDateTime(Request["Edate"].ToString()).AddDays(1) + "'";
        }
        List<Hi.Model.DIS_Order> l = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", false, strwhere, out pageCount, out Counts);

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
        Bind();
    }
}