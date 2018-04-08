using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_CompCollection_ZD : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
            //this.txtCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            //this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere = string.Empty;
        string IDlist = string.Empty;//销售经理下属 员工ID集合
        if (DisSalesManID != 0)
        {
            if (Common.GetDisSalesManType(DisSalesManID.ToString(), out IDlist))
            {
                //销售经理
                strwhere = "and DisID in(select ID from BD_Distributor where smid in(" + IDlist + "))";
            }
            else
            {
                strwhere = "and DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
            }
        }

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

        string sql = "SELECT * FROM [dbo].[CompCollection_view] " + " where OrderID  in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and CompID=" + this.CompID + ") and CompID=" + this.CompID + strwhere 
            + " order by Date desc";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["Price"].ToString() == "" ? "0" : ds.Rows[i]["Price"].ToString())-Convert.ToDecimal(ds.Rows[i]["sxf"].ToString() == "" ? "0" : ds.Rows[i]["sxf"].ToString());
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

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
    protected string Where()
    {
        string strWhere = string.Empty;
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML(txtDisName.Value.Trim().Replace("'", "")) + "%')";
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and Date>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and Date<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (this.ddrPayType.Value != "-1")
        {
            strWhere += " and Source='" + GetUserType(this.ddrPayType.Value.Trim()) + "'";
        }
        if (this.orderid.Value.Trim() != "")
        {
            strWhere += " and orderID='" + GetOrderID(this.orderid.Value.Trim().Replace("'", "")) + "'";
        }
        return strWhere;        
    }

    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length >7)
        {
            str = str.Substring(0, 7) + "...";
        }
        return str;
    }

    public string GetUserType(string str)
    {
        switch (str)
        {
            case "1":
                str = "快捷支付";
                break;
            case "2":
                str = "网银支付";
                break;
            case "3":
                str = "企业钱包支付";
                break;
            case "4":
                str = "转账汇款";
                break;
            case "5":
                str = "现金";
                break;
            case "6":
                str = "票据";
                break;
            case "7":
                str = "其它";
                break;
        }
        return str;
    }

    public string GetBankName(string BankID)
    {
        if (BankID != "")
            return new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID);
        return "预收款收款";
    }
    public string GetOrderID(string Num)
    {
        List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", "ISNULL(dr,0)=0 and compid=" + CompID + " and ReceiptNo='" +Common.NoHTML( Num) + "'", "");
        if (order.Count > 0)
        {
            return order[0].ID.ToString();
        }
        else
        {
            return "";
        }
    }
}