using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_GoodsRpt : CompPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;//订单笔数总计
    public decimal tb = 0;//订单金额总计
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
            ViewState["strwhere2"] = Where2();
            ViewState["strwhere3"] = Where3();
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
        if (ViewState["strwhere2"] != null)
        {
            strwhere2 += ViewState["strwhere2"].ToString();
        }
        if (ViewState["strwhere3"] != null)
        {
            strwhere3 += ViewState["strwhere3"].ToString();
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

        string sql = string.Format(@"select x.GoodsName,x.GoodsinfoID,y.GoodsinfoID as GoodsinfoID2,AuditAmount,AuditAmount2,isnull(num,0) as num,isnull(num2,0) as num2 from (
select GoodsName, GoodsinfoID,SUM(SharePrice) as AuditAmount,COUNT(*) as num
 from DIS_Order as a 
left join DIS_OrderDetail as b
on a.id=b.OrderID
where ISNULL(a.dr,0)=0 and ISNULL(b.dr,0)=0 and a.CompID={0} and GoodsinfoID is not null
and OState in(2,3,4,5,7) and Otype<>9 {1}
group by GoodsName,GoodsinfoID)x
full join
(select GoodsName, GoodsinfoID,cast(SUM((sumAmount/c.TotalAmount)*c.PayedAmount) as decimal(18,   2)) as AuditAmount2,COUNT(*) as num2
from DIS_OrderReturn as a
left join
DIS_Order as c  on a.OrderID=c.ID
left join DIS_OrderDetail as b
on c.id=b.OrderID
where ISNULL(a.dr,0)=0 and ISNULL(b.dr,0)=0 and ISNULL(c.dr,0)=0
 and a.CompID={0} and c.CompID={0} and GoodsinfoID is not null
and OState =7 and Otype<>9 and a.ReturnState=4 {2}
group by GoodsName,GoodsinfoID)y on x.GoodsinfoID=y.GoodsinfoID order by x.GoodsinfoID", this.CompID, strwhere2, strwhere3);

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["num"].ToString() == "" ? "0" : ds.Rows[i]["num"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString());
            td += Convert.ToDecimal(ds.Rows[i]["num2"].ToString() == "" ? "0" : ds.Rows[i]["num2"].ToString());
            te += Convert.ToDecimal(ds.Rows[i]["AuditAmount2"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount2"].ToString());
        }
        this.rptGoods.DataSource = dt;
        this.rptGoods.DataBind();

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
    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <returns></returns>
    public string getGoodsStr(string goodsinfoid)
    {
        string str = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(goodsinfoid));
        if (model != null)
        {
            str = model.BarCode;
            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            if (model2 != null)
            {
                str += "&nbsp;" + model2.GoodsName;
            }
        }
        return str;
    }
}