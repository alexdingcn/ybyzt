using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Report_GodRet : CompPageBase
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

            Bind();
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere = string.Empty;//代理商搜索条件
        strwhere = Where();
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

        string sql = string.Format(@"select d.ID,o.ReceiptNo,b.DisName,b.DisCode,o.CreateDate,
        d.GoodsCode,d.GoodsName,d.GoodsNum,d.AuditAmount 
        from DIS_OrderDetail d,DIS_Order o,BD_Distributor b where o.ID=d.OrderID 
        and b.ID=d.DisID and ISNULL(o.dr,0)=0 and o.OState in(2,3,4,5,7) and o.Otype<>9  
        and o.CompID=" + this.CompID+ " "+ strwhere + "  order by o.CreateDate desc");
        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
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
        Bind();
    }
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind();
    }
    /// <summary>
    /// 代理商名称条件
    /// </summary>
    /// <returns></returns>
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and  d.GoodsName like '%" + this.txtGoodsName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }

        if (this.txtDisName.Value.Trim() != "")
        {
            strWhere += " and  b.DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        if (this.txtBCreateDate.Value.Trim() != "" )
        {
            strWhere += " and o.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        
        if ( this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and o.CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        
        return strWhere;
    }

}