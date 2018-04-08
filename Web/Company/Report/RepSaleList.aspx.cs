using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_RepSaleList : CompPageBase
{
    public string page = "1";//默认初始页
    public decimal td = 0;//数量总计
    public decimal te = 0;//金额总计
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
        string strwhere = string.Empty;//代理商、商品搜索条件
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

        string sql = string.Format(@"select od.GoodsinfoID,od.GoodsName,od.GoodsInfos,od.GoodsCode,gg.TypeName,od.OrderID,o.CompID,o.DisID,HtID,HospitalName HtName,sum(od.GoodsNum) GoodsNum,sum(od.GoodsPrice) GoodsPrice from 
DIS_OrderDetail od left join DIS_Order o on od.OrderID=o.ID
left join BD_GoodsInfo info on info.ID=od.GoodsinfoID left join BD_Goods g on info.GoodsID=g.ID
left join SYS_GType gg on g.CategoryID=gg.ID
left join 
(select GoodsID,HtID,ht.HospitalName from YZT_FirstCamp fc left join YZT_CMerchants cm on cm.ID=fc.CMID
left join SYS_Hospital ht on ht.ID=fc.HtID
where fc.State=2 and fc.dr=0 and cm.dr=0 and cm.IsEnabled=1) CMFC
on od.GoodsinfoID=CMFC.GoodsID
where o.OState in(2,4,5) {0} {1} {2}
group by od.GoodsinfoID,od.GoodsName,od.GoodsInfos,od.GoodsCode,gg.TypeName,HtID,HospitalName,od.OrderID,o.CompID,o.DisID order by od.GoodsinfoID ", " and o.CompID=" + this.CompID, strwhere, strwhere2);

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            td += Convert.ToDecimal(ds.Rows[i]["GoodsNum"].ToString() == "" ? "0" : ds.Rows[i]["GoodsNum"].ToString());
            te += Convert.ToDecimal(ds.Rows[i]["GoodsPrice"].ToString() == "" ? "0" : ds.Rows[i]["GoodsPrice"].ToString());
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
    /// 商品名称条件
    /// </summary>
    /// <returns></returns>
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtDisName.Value.Trim() != "")
        {
            strWhere += " and  od.GoodsName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        if (this.txtHtName.Value.Trim() != "")
        {
            strWhere += " and HospitalName like '%" + this.txtHtName.Value.Trim().ToString().Replace("'", "''") + "%'";
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
            strWhere += " and o.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and o.CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        else
        {
            if (this.txtBCreateDate.Value.Trim() != "" && this.txtECreateDate.Value.Trim() == "")
            {
                strWhere += " and o.CreateDate >= '" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'  and o.CreateDate<'" + DateTime.Now.AddDays(1) + "'";
            }
        }
        return strWhere;
    }
}