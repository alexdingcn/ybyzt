using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_OrderRpt : CompPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;//订单笔数总计
    public decimal tb = 0;//订单金额总计
    public decimal tc = 0;//收款金额总计
    public decimal td = 0;//退单笔数总计
    public decimal te = 0;//退货金额总计
    public decimal tf = 0;//订货代理商总计
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string CreateDate = "0";
            if (Common.GetCompService(CompID.ToString(), out CreateDate) == "0")
            {
                Response.Redirect("../SysManager/Service.aspx", true);
            }
            this.txtPager.Value = Common.PageSize;
            //默认本周
            this.txtBCreateDate.Value = DateTime.Now.AddDays(Convert.ToDouble((1 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            ViewState["strwhere2"] = Where2();
            ViewState["strwhere3"] = Where3();
            ViewState["strwhere4"] = Where4();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere2 = string.Empty;//日期搜索条件
        string strwhere3 = string.Empty;//日期搜索条件
        string strwhere4 = string.Empty;//日期搜索条件
        if (ViewState["strwhere2"] != null)
        {
            strwhere2 += ViewState["strwhere2"].ToString();
        }
        if (ViewState["strwhere3"] != null)
        {
            strwhere3 += ViewState["strwhere3"].ToString();
        }
        if (ViewState["strwhere4"] != null)
        {
            strwhere4 += ViewState["strwhere4"].ToString();
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

        string sql = string.Format(@"select ISNULL(disidnum,0) as disidnum,ISNULL(num,0) as num,ISNULL(num2,0) as num2,AuditAmount,PayedAmount,case isnull(CreateDate,'') when '' then case isnull(CreateDate2,'') when '' then CreateDate3 else CreateDate2 end else CreateDate end CreateDate,CreateDate2,CreateDate3,PayedAmount2 from (
select COUNT(*) as disidnum,SUM(AuditAmount) as AuditAmount ,SUM(num) as num,CreateDate from(
select DisID,SUM(AuditAmount) as AuditAmount,COUNT(*) as num, 
CONVERT(char(11), CreateDate,120)as CreateDate
from dis_order as x where ISNULL(dr,0)=0 and OState in(2,3,4,5,7) and Otype<>9 and CompID={0} {1}
 group by DisID, CONVERT(char(11), CreateDate,120))x 
 group by CreateDate)a
FULL   JOIN
( select SUM(PayedAmount) as payedAmount2,COUNT(*) as num2,CONVERT(char(11), x.AuditDate,120)as CreateDate2
  from DIS_OrderReturn as x,DIS_Order as b 
 where  x.OrderID=b.ID and ISNULL(x.dr,0)=0 and ISNULL(b.dr,0)=0 and Otype<>9
  and x.ReturnState=4 and x.CompID={0} and b.CompID={0} {3}
  group by  CONVERT(char(11), x.AuditDate,120))b 
on a.CreateDate=b.CreateDate2
full join
(
select CONVERT(char(11), Date,120)as CreateDate3,SUM(Price) as PayedAmount from CompCollection_view
where isnull(vedf9,0)=1 and orderid not in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype=9 or OState not in(2,3,4,5,7)) and CompID={0}) and status!=3
 and  CompID={0} {2}
group by CONVERT(char(11), Date,120))d
on a.CreateDate=d.CreateDate3
order by case isnull(CreateDate,'') when '' then case isnull(CreateDate2,'') when '' then CreateDate3 else CreateDate2 end else CreateDate end desc", this.CompID, strwhere2, strwhere3, strwhere4);

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["num"].ToString() == "" ? "0" : ds.Rows[i]["num"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString());
            tc += Convert.ToDecimal(ds.Rows[i]["PayedAmount"].ToString() == "" ? "0" : ds.Rows[i]["PayedAmount"].ToString());
            td += Convert.ToDecimal(ds.Rows[i]["num2"].ToString() == "" ? "0" : ds.Rows[i]["num2"].ToString());
            te += Convert.ToDecimal(ds.Rows[i]["PayedAmount2"].ToString() == "" ? "0" : ds.Rows[i]["PayedAmount2"].ToString());
            tf += Convert.ToDecimal(ds.Rows[i]["disidnum"].ToString() == "" ? "0" : ds.Rows[i]["disidnum"].ToString());
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

        //Pager.RecordCount = Counts;
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
        ViewState["strwhere2"] = Where2();
        ViewState["strwhere3"] = Where3();
        ViewState["strwhere4"] = Where4();
        Bind();
    }
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere2"] = Where2();
        ViewState["strwhere3"] = Where3();
        ViewState["strwhere4"] = Where4();
        Bind();
    }
    /// <summary>
    /// 日期条件
    /// </summary>
    /// <returns></returns>
    protected string Where2()
    {
        string strWhere = string.Empty;
        if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and x.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and x.CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and x.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and x.CreateDate<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }
    /// <summary>
    /// 日期条件
    /// </summary>
    /// <returns></returns>
    protected string Where3()
    {
        string strWhere = string.Empty;
        if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and [date] >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and [date]<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and [date] >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and [date]<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }

    /// <summary>
    /// 日期条件
    /// </summary>
    /// <returns></returns>
    protected string Where4()
    {
        string strWhere = string.Empty;
        if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and x.AuditDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and x.AuditDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and x.AuditDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and x.AuditDate<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }
}