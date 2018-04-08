using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_Rep_RepZdzfDetailsList : DisPageBase
{
    public string page = "1";//默认初始页
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal sxf = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            this.txtArriveDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 代理商列表显示
    /// </summary>
    public void Bind()
    {
        string strwhere = string.Empty;

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
        //if (this.txtArriveDate.Value.Trim() == "" && this.txtArriveDate1.Value.Trim() == "")
        //{
        //    strwhere += " and Date>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        string sql = "SELECT * FROM [dbo].[CompCollection_view] " + " where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and DisID=" + this.DisID + ") and DisID=" + this.DisID + strwhere
            + " order by Date desc";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["Price"].ToString() == "" ? "0" : ds.Rows[i]["Price"].ToString());
            sxf += Convert.ToDecimal(ds.Rows[i]["sxf"].ToString() == "" ? "0" : ds.Rows[i]["sxf"].ToString());
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
    protected void A_Seek(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string sqldate = string.Empty;
        if (this.txtArriveDate.Value.Trim() != "")
        {
            sqldate += " and Date>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            sqldate += " and Date<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        if (this.orderid.Value.Trim() != "")
        {
            sqldate += " and orderID='" + GetOrderID(Common.NoHTML(this.orderid.Value.Trim())) + "'";
        }
        if (this.ddrPayType.Value != "-1")
        {
            sqldate += " and Source='" + GetUserType(Common.NoHTML(this.ddrPayType.Value.Trim())) + "'";
        }
        return sqldate;
    }
    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length > 7)
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
        return "企业钱包支付";
    }

    //public void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
    //    {
    //        DataRowView drv = (DataRowView)e.Item.DataItem;
    //        ta += Convert.ToDecimal(drv["Price"]);
    //    }
    //    if (e.Item.ItemType == ListItemType.Footer)
    //    {
    //        if (e.Item.FindControl("total") != null)
    //        {
    //            Label tol = (Label)e.Item.FindControl("total");
    //            tol.Text = string.Format("{0}", ta.ToString("N"));
    //        }
    //    }
    //}
    public string GetOrderID(string Num)
    {
        List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", "ISNULL(dr,0)=0 and DisID=" + this.DisID + " and ReceiptNo='" + Num + "'", "");
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