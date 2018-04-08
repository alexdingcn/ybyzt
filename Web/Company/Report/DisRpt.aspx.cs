using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_DisRpt : CompPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;//订单笔数总计
    public decimal tb = 0;//订单金额总计
    public decimal tc = 0;//收款金额总计
    public decimal td = 0;//退单笔数总计
    public decimal te = 0;//退货金额总计
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
            ViewState["strwhere"] = Where();
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
        string strwhere = string.Empty;//代理商搜索条件
        string strwhere2 = string.Empty;//日期搜索条件
        string strwhere3 = string.Empty;//收款日期搜索条件
        string strwhere4 = string.Empty;//收款日期搜索条件
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
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

        string sql = string.Format(@"select c.ID,DisName,isnull(num,0) as num,AuditAmount,PayedAmount,isnull(num2,0) as num2,PayedAmount2 from (
select DisID,SUM(AuditAmount) as AuditAmount,COUNT(*) as num
from dis_order as a where ISNULL(dr,0)=0 and OState in(2,3,4,5,7) and Otype<>9 and CompID={0} {2} group by DisID)a
FULL   JOIN
(select a.DisID as DisID2,COUNT(*) as num2,SUM(PayedAmount) as PayedAmount2  from DIS_OrderReturn as a
left join dis_order as b on a.OrderID=b.ID
 where ISNULL(a.dr,0)=0 and ISNULL(b.dr,0)=0 and OState =7 and a.CompID={0} and Otype<>9
 and b.CompID={0} {4} and a.ReturnState=4
 group by a.DisID)b 
on a.DisID=b.DisID2
full join
(select DisID as DisID3,SUM(PayedAmount)as PayedAmount from(
select DisID,CONVERT(char(11), Date,120)as CreateDate2,SUM(Price) as PayedAmount from CompCollection_view where isnull(vedf9,0)=1 and CompID={0} and status!=3
and orderID not in(
select ID from DIS_Order where (OState not in(2,3,4,5,7) or Otype=9)and CompID={0})
 {3}
group by DisID,CONVERT(char(11), Date,120))ax
group by DisID)d
on a.DisID=d.DisID3
,(select dis.DisName,cu.CompID,dis.ID,dis.dr
 from SYS_CompUser cu left join BD_Distributor dis on cu.DisID=dis.ID
 where cu.CType=2 and cu.UType=5 and cu.IsAudit=2 and cu.IsEnabled=1) as c
where (DisID=c.ID or DisID2=c.ID) and ISNULL(c.dr,0)=0 and c.CompID={0} {1} order by c.id", this.CompID, strwhere, strwhere2, strwhere3, strwhere4);

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
        ViewState["strwhere"] = Where();
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
        ViewState["strwhere"] = Where();
        ViewState["strwhere2"] = Where2();
        ViewState["strwhere3"] = Where3();
        ViewState["strwhere4"] = Where4();
        Bind();
    }
    /// <summary>
    /// 代理商名称条件
    /// </summary>
    /// <returns></returns>
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtDisName.Value.Trim() != "")
        {
            strWhere += " and  DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        
        return strWhere;
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
            strWhere += " and a.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and a.CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and a.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and a.CreateDate<'" + DateTime.Now.AddDays(1) + "'";
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
            strWhere += " and a.AuditDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and a.AuditDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and a.AuditDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and a.AuditDate<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }
}