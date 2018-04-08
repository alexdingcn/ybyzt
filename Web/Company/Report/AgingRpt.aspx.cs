using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_AgingRpt : CompPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal zta = 0;//未支付金额总计
    public decimal ztb = 0;//1-30总计
    public decimal ztc = 0;//31-60总计
    public decimal ztd = 0;//61-90总计
    public decimal zte = 0;//91-180总计
    public decimal ztf = 0;//180-总计
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
            ViewState["strwhere2"] = Where2();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere2 = string.Empty;//日期搜索条件
        if (ViewState["strwhere2"] != null)
        {
            strwhere2 += ViewState["strwhere2"].ToString();
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

        string sql = string.Format(@"select DisID,SUM(AUDITAMOUNT) as AUDITAMOUNT,SUM(AUDITAMOUNT2) as AUDITAMOUNT2,SUM(AUDITAMOUNT3) as AUDITAMOUNT3,SUM(AUDITAMOUNT4) as AUDITAMOUNT4,
SUM(AUDITAMOUNT5) as AUDITAMOUNT5
 from(
select DISID,
 (SELECT SUM(AUDITAMOUNT) WHERE DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())>=0 AND DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())<=30)AS AUDITAMOUNT,
 (SELECT SUM(AUDITAMOUNT) WHERE DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())>=31 AND DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())<=60)AS AUDITAMOUNT2,
 (SELECT SUM(AUDITAMOUNT) WHERE DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())>=61 AND DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())<=90)AS AUDITAMOUNT3,
 (SELECT SUM(AUDITAMOUNT) WHERE DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())>=91 AND DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())<=180)AS AUDITAMOUNT4,
 (SELECT SUM(AUDITAMOUNT) WHERE DATEDIFF(day,CONVERT(char(11), CreateDate,120),getdate())>=181)AS AUDITAMOUNT5
 from (
select * from (
select disid,SUM(AuditAmount) AuditAmount,CONVERT(char(11), CreateDate,120)as CreateDate

 from DIS_Order 
where OState in(2,3,4,5,7) and PayState=0 and ISNULL(dr,0)=0 and Otype<>9 and CompID={0}
group by DisID,CONVERT(char(11), CreateDate,120)

union all

select disid,(SUM(AuditAmount)-SUM(PayedAmount)) AuditAmount,CONVERT(char(11), CreateDate,120)as CreateDate

 from DIS_Order 
where OState in(2,3,4,5,7) and PayState in(1,5) and ISNULL(dr,0)=0 and Otype<>9 and CompID={0}
group by DisID,CONVERT(char(11), CreateDate,120))xy
)yx  group by DisID,CreateDate
)xxy where 1=1 {1} group by DisID 
order by DisID ", this.CompID, strwhere2);

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ztb += Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString());
            ztc += Convert.ToDecimal(ds.Rows[i]["AuditAmount2"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount2"].ToString());
            ztd += Convert.ToDecimal(ds.Rows[i]["AuditAmount3"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount3"].ToString());
            zte += Convert.ToDecimal(ds.Rows[i]["AuditAmount4"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount4"].ToString());
            ztf += Convert.ToDecimal(ds.Rows[i]["AuditAmount5"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount5"].ToString());
        }
        zta = ztb + ztc + ztd + zte + ztf;
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
        Bind();
    }
    /// <summary>
    /// 日期条件
    /// </summary>
    /// <returns></returns>
    protected string Where2()
    {
        string strWhere = string.Empty;
        if (this.txtDisName.Value.Trim() != "")
        {
            string id = "0";
            string sql = " select id from BD_Distributor where ISNULL(dr,0)=0 and compid=" + this.CompID + " and DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    id += "," + dr["ID"].ToString();
                }
            }
            strWhere += " and DisID in (" + id + ") ";
        }
        return strWhere;
    }

    /// <summary>
    /// 未支付总计
    /// </summary>
    /// <param name="pr1"></param>
    /// <param name="pr2"></param>
    /// <param name="pr3"></param>
    /// <param name="pr4"></param>
    /// <param name="pr5"></param>
    /// <returns></returns>
    public decimal getSum(string pr1, string pr2, string pr3, string pr4, string pr5)
    {
        return pr1.ToDecimal(0) + pr2.ToDecimal(0) + pr3.ToDecimal(0) + pr4.ToDecimal(0) + pr5.ToDecimal(0);
    }
}