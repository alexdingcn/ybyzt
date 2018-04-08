using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_TransferMoneyRpt : CompPageBase
{
    public string page = "1";//默认初始页
    public decimal ta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
            //this.txtBCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            //this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewState["sqldate"] = Where();
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
        strwhere += " and Start=1 and PreType=6 and CompID=" + this.CompID;
        //if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        //{
        //    //默认当前年
        //    strwhere += " and CreatDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}

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
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);
        List<Hi.Model.PAY_PrePayment> dis = new Hi.BLL.PAY_PrePayment().GetList("", "isnull(dr,0)=0" + strwhere, "");
        for (int i = 0; i < dis.Count; i++)
        {
            ta += dis[i].price;
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sqldate"] = Where();
        Bind();
    }
    protected string Where()
    {
        string sqldate = string.Empty;
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            sqldate += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" + txtDisName.Value.Trim().Replace("'", "") + "%')";
            //strWhere += " and disid=" + this.DisListID.Disid;
        }
        //if (this.DisListID.Disid != "")
        //{
            //string id = "0";
            //string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and DisName like '%" + this.txtDisName.Value.Trim().ToString() + "%'";
            //DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        id += "," + dr["ID"].ToString();
            //    }
            //}

            //sqldate += " and DisID in (" + id + ") ";
            //sqldate += " and DisID ="+this.DisListID.Disid;
        //}
        if (this.txtBCreateDate.Value.Trim() != "")
        {
            sqldate += " and CreatDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            sqldate += " and CreatDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (this.txtReceiptNo.Value.Trim() != "")
        {
            sqldate += "and ID like '%" + this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        return sqldate;       
    }

    public string GetStr(string str)
    {
        if (str.Length > 7)
        {
            str = str.Substring(0, 7) + "...";
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