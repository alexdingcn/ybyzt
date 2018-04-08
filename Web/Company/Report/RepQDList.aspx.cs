using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_RepQDList : CompPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
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
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
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

        string sql = string.Format(@"select cd.ID,cd.ContID,cd.GoodsID,cd.GoodsName,cd.GoodsCode,cd.ValueInfo,cd.HtID,ht.HospitalName,
cd.SalePrice,cd.discount,cd.TinkerPrice,cd.target,c.DisID,dis.DisName,(select sum(sumAmount) from  DIS_OrderDetail od left join DIS_Order o 
on od.OrderID=o.ID where o.OState in(2,4,5) and o.dr=0 and o.DisID=c.DisID and  o.CompID=c.CompID and od.GoodsinfoID=cd.GoodsID) 
sumAmount 
from YZT_ContractDetail cd join YZT_Contract c on cd.ContID=c.ID and c.CState<>2 
left join SYS_Hospital ht on cd.HtID=ht.ID
left join BD_Distributor dis on dis.ID=c.DisID
where c.CompID={0} {1} order by c.CreateDate", this.CompID, strwhere2);

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
        ViewState["strwhere"] = Where();
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
        ViewState["strwhere"] = Where();
        ViewState["strwhere2"] = Where2();
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
            strWhere += " and  dis.DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
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
            strWhere += " and c.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and c.CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and c.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and c.CreateDate<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }
}