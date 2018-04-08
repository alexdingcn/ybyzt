using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_Rep_RepMonthList : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal tb = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        //dis = new Hi.BLL.BD_Distributor().GetModel(user.DisID);
        //if (!IsPostBack)
        //    user = LoginModel.IsLogined(this);
        //if (user != null)
        //{
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            if (Request.QueryString["type"] == "1")
            {
                this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");

            }
            else if (Request.QueryString["type"] + "" == "2")
            {

                this.txtArriveDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            ViewState["strwhere"] = Where();
            Bind();
        }
        //}
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string date = string.Empty;
        string strwhere = string.Empty;
        string sqldate = string.Empty;

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
        if (this.txtArriveDate.Value.Trim() == "" && this.txtArriveDate1.Value.Trim() == "")
        {
            this.txtArriveDate.Value = Convert.ToDateTime(DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1").ToString("yyyy-MM-dd");
            this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            date = " and CreateDate>='" + DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1 0:0:0' ";
        }
        
        //string sql = @"select DisID,YEAR([CreateDate]) Years,MONTH([CreateDate]) as Months,SUM([sumAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM [dbo].[MonthSaleRpt_view] where DisID=" + this.DisID + date + sqldate + strwhere + ") M " + "where DisID=" + this.DisID +" group by YEAR([CreateDate]), MONTH([CreateDate]),disID order by YEAR([CreateDate]),MONTH([CreateDate])";

        string sql = @"select DisID,YEAR([CreateDate]) Years,MONTH([CreateDate]) as Months,SUM(CASE WHEN Otype<>9 THEN AuditAmount ELSE 0 END) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount] from DIS_Order where DisID=" + this.DisID +" and CompID=" + this.ddrComp.Value +" " + strwhere + " group by YEAR([CreateDate]), MONTH([CreateDate]),disID order by YEAR([CreateDate]),MONTH([CreateDate])";

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for(int i=0;i<ds.Rows.Count;i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["TotalAmount"].ToString() == "" ? "0" : ds.Rows[i]["TotalAmount"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["zdAmount"].ToString() == "" ? "0" : ds.Rows[i]["zdAmount"].ToString());
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
        string strWhere = string.Empty;
        string sqldate = string.Empty;

        //if (this.txtGoodsName.Value.Trim() != "")
        //{
        //    strWhere += " and  GoodsName like '%" + this.txtGoodsName.Value.Trim().ToString() + "%'";
        //}
        ////商品类别 ：根据GoodsID查询
        //string hideID = this.hid_product_class.Value.Trim();
        //if (this.txt_product_class.Value.Trim() != "")
        //{
        //    string idlist = string.Empty;
        //    if (!Util.IsEmpty(hideID))
        //    {
        //        string cateID = Common.CategoryId(Convert.ToInt32(hideID), user.CompID);//商品分类递归
        //        strWhere += " and categoryID in(" + cateID + ") ";
        //    }
        //}
        if (this.ddrOState.Value != "-2")
        {
            strWhere += " and OState=" + Common.NoHTML(this.ddrOState.Value);
        }
        else
        {
            strWhere += " and OState in(2,4,5,3,7)";
        }

        if (this.txtArriveDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        return strWhere;
        
    }
    //public void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Footer)
    //    {
    //        Repeater rpt = (Repeater)sender;
    //        DataTable ds = (DataTable)rpt.DataSource;
    //        int a = ds.Rows.Count;
    //        if (e.Item.FindControl("total1") != null)
    //        {
    //            Label tol1 = (Label)e.Item.FindControl("total1");
    //            tol1.Text = string.Format("{0}", ta.ToString("N"));
    //        }
    //        //if (e.Item.FindControl("total2") != null)
    //        //{
    //        //    Label tol2 = (Label)e.Item.FindControl("total2");
    //        //    tol2.Text = string.Format("{0}", tb.ToString("N"));
    //        //}
    //    }
    //}
}