using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_Systems_RepMoneyList : AdminPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    decimal ta=0;
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
            this.txtBCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["sqldate"] = Where();
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
        strwhere += " and Start=1 and PreType=1";
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
        //if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        //{
        //    strwhere += " and CreatDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

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
        if (this.txtDisName.Value.Trim() != "")
        {
            string id = "0";
            string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and DisName like '%" + Common.NoHTML(this.txtDisName.Value.Trim().ToString().Replace("'", "''")) + "%'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    id += "," + dr["ID"].ToString();
                }
            }
            sqldate += " and DisID in (" + id + ") ";
        }
        if (this.txtBCreateDate.Value.Trim() != "")
        {
            sqldate += " and CreatDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            sqldate += " and CreatDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            sqldate += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        return sqldate;
    }
    public string GetStr(string str)
    {
        if (str.Length > 5)
        {
            str = str.Substring(0, 4) + "...";
        }
        return str;
    }
    public void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Hi.Model.PAY_PrePayment drv = (Hi.Model.PAY_PrePayment)e.Item.DataItem;
            ta += Convert.ToDecimal(drv.price);
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            if (e.Item.FindControl("total") != null)
            {
                Label tol = (Label)e.Item.FindControl("total");
                tol.Text = string.Format("{0}", ta.ToString("N"));
            }
        }
    }
}