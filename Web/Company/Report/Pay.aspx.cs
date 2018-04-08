using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Report_Pay : CompPageBase
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
        string strwhere = string.Empty;
        if (Request["type"] == "1")
        {
            strwhere += " and OState in (2,4,5)  and PayState in(0,1)  and DisID='" + KeyID + "' and isnull(dr,0)=0 and Otype!=9 and compid=" + CompID;
        }
        if (Request["type"] == "2")
        {
            strwhere += " and OState in (2,4,5)  and PayState in(0,1)  and DisID='" + KeyID + "' and isnull(dr,0)=0 and Otype=9 and compid=" + CompID;
            num.InnerText = "账单编号";
            date.InnerText = "账单日期";
            amount.InnerText = "账单金额";
            state.InnerText = "账单状态";
        }
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