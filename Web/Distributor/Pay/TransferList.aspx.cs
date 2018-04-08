using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Pay_TransferList : DisPageBase
{    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public string price = string.Empty;//企业钱包余额

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        price = new Hi.BLL.PAY_PrePayment().sums(this.DisID, this.CompID).ToString("0.00");
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部手动录入冲正
        // strwhere += " and PreType=3";
        //有效数据显示
        // strwhere += " and IsEnabled = 1";
        //审核状态是已审的
        //strwhere += " and AuditState = 2";
        //付款状态是成功的
        strwhere += " and Start=1";

        strwhere += " and PreType=6";
        //所属代理商
        strwhere += " and DisID=" + this.DisID;
        //int disId = Convert.ToInt32(Request.QueryString["keyId"]);
        //if (disId != 0)
        //{
        //    strwhere += " and DisID=" + disId + "";
        //}
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        if (this.txtPager.Value.Trim().ToString() != "")
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
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = pay;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length > 8)
        {
            str = str.Substring(0, 7) + "...";
        }
        return str;
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
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;

        string IDNO = Common.NoHTML(this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''"));
        string ddrAuditState = Common.NoHTML(this.ddrAuditState.Value);
        string ddrPayState = Common.NoHTML(this.ddrPayState.Value);
        string ddrPayType = Common.NoHTML(this.ddrPayType.Value);
        if (IDNO != "")
        {
            strWhere += " and ID like '%" + IDNO + "%'";
        }
        if (ddrAuditState != "-1")
        {
            strWhere += " and AuditState =" + ddrAuditState;
        }
        if (ddrPayState != "-1")
        {
            strWhere += " and Start =" + ddrPayState;
        }
        if (ddrPayType != "-1")
        {
            strWhere += " and PreType =" + ddrPayType;
        }
        if (this.txtArriveDate.Value != "")
        {
            strWhere += " and Paytime>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value != "")
        {
            strWhere += " and Paytime<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        ViewState["strwhere"] = strWhere;
        Bind();
    }

    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Getmessage(int state, int id)
    {
        string str = string.Empty;
        if (state == 0)
        {
            str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay({0})\' class=\"btnBl\">支付</a>", id);
        }
        else if (state == 2)
        {
            str = "已支付";
        }
        else if (state == 5)
        {
            str = "已退款";
        }
        //else if (state == 6)
        //{
        //    str = "已结算";
        //}

        return str;

    }
}