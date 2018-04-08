

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_OrderOutList : CompPageBase
{
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderOut OrderOutBll = new Hi.BLL.DIS_OrderOut();
    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        //page = Request["page"] + "";
        //this.Pager.CurrentPageIndex = page.ToInt(0);

        if (!IsPostBack)
        {
            this.hidCompId.Value = this.CompID.ToString();
            this.txtPager.Value = Common.PageSize;
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {

        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        //strwhere += "and OState in (" + (int)Enums.OrderState.已提交 + "," + (int)Enums.OrderState.已审 + ") and IsAudit=0 and isnull(dr,0)=0"; //isnull(dr,0)=1  订单已删除

        strwhere += "and CompID=" + this.CompID + " and isnull(dr,0)=0";

        List<Hi.Model.DIS_OrderOut> l = OrderOutBll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "SendDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }

    /// <summary>
    /// 分页
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

    public string Where() {
        string strWhere = string.Empty;

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.txtReceiptNo1.Value != "")
        {
            strWhere += " and OrderID in (select ID from DIS_Order where ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo1.Value.Trim().ToString().Replace("'", "''")) + "%')";
        }
        if (this.txtReceiptNo.Value != "")
        {
            strWhere += "and ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and SendDate>'" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and SendDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        return strWhere;
    }
}