

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_Rep_RepMoneyList : DisPageBase
{
    public string page = "1";//默认初始页
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["sqldate"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 代理商列表显示
    /// </summary>
    public void Bind()
    {
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        strwhere += " and Start=1 and PreType=1";

        if (this.DisID != 0)
        {
            strwhere += " and DisID=" + this.DisID + "";
        }
        if (ViewState["sqldate"] != null)
        {
            strwhere += ViewState["sqldate"].ToString();
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
        //    strwhere += " and CreatDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);
        List<Hi.Model.PAY_PrePayment> p = new Hi.BLL.PAY_PrePayment().GetList("", " Start=1 and PreType=1 and disid=" + this.DisID + Where(), "");
        for (int i = 0; i < p.Count; i++)
        {
            ta += p[i].price;
        }
        this.rptOrder.DataSource = pay;
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
        ViewState["sqldate"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void A_Seek(object sender, EventArgs e)
    {
        ViewState["sqldate"] = Where();
        Bind();
    }
    protected string Where()
    {
        string sqldate = string.Empty;
        if (this.txtArriveDate.Value.Trim() != "")
        {
            sqldate += " and CreatDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            sqldate += " and CreatDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        return sqldate;
        
    }
    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length > 5)
        {
            str = str.Substring(0, 4) + "...";
        }
        return str;
    }
    //public void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
    //    {
    //        Hi.Model.PAY_PrePayment drv = (Hi.Model.PAY_PrePayment)e.Item.DataItem;
    //        ta += Convert.ToDecimal(drv.price);
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
}