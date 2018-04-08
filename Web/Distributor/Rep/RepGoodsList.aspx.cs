

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Distributor_Rep_RepGoodsList : DisPageBase
{
    public string page = "1";//默认初始页
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public string Digits = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID);

        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");

            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());

            ViewState["strwhere"] = Where();
            Bind();
        }
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

        string sql = "select goodsID,goodsCode,goodsName,categoryName,sum(goodsNum) goodsNum,sum(SharePrice) sumAmount from(" +
        "select * from [dbo].[GoodsSaleRpt_view] where " +
        "DisID=" + this.DisID + " and CompID=" + this.ddrComp.Value+
        date + sqldate + strwhere + ")M " +
        "group by goodsID,goodsCode,goodsName,categoryName order by goodsCode,goodsName,categoryName";

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["sumAmount"].ToString() == "" ? "0" : ds.Rows[i]["sumAmount"].ToString());
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

        if (this.ddrOState.Value != "-2")
        {
            strWhere += " and OState=" + Common.NoHTML(this.ddrOState.Value);
        }
        else
        {
            strWhere += " and OState in(2,4,5,3,7)";
        }
        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and GoodsName like '%" + Common.NoHTML(this.txtGoodsName.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (this.txtCategory.Value.Trim() != "")
        {
            //商品类别 ：根据GoodsID查询
            string hideID = Common.NoHTML(this.txtCategory.Value.Trim().Replace("'", "''"));

            strWhere += " and categoryName like '%" + hideID + "%'";

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
}