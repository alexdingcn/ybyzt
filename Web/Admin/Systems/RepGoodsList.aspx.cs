using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_Systems_RepGoodsList : AdminPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan, "全部");
            this.txtPager.Value = Common.PageSize;
            this.txtBCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
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
        string strwhere = " ";

        if (SalesManID > 0 || OrgID > 0)
        {
            string whereIn = string.Empty;
            if (OrgID > 0)
            {
                whereIn += "  and OrgID=" + OrgID + "";
            }
            if (SalesManID > 0)
            {
                whereIn += " and SalesManID=" + SalesManID + "";
            }
            strwhere = " and CompID in (select ID from [dbo].[BD_Company] where isnull(dr,0)=0 "+ whereIn + ")";
        }
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

        //if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        //{
        //    date = " and CreateDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        string sql = "select CompID,goodsID,goodsCode,goodsName,categoryName,sum(goodsNum) goodsNum,sum(sumAmount) sumAmount from(" +
        "select * from [dbo].[GoodsSaleRpt_view] where OState in(2,4,5) " +
        date + strwhere + ")M " +
        "group by CompID,goodsID,goodsCode,goodsName,categoryName order by CompID";

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
        GetAllCompID();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void GetAllCompID()
    {
        string Hcomp = string.Empty;
        string sql = "select CompID,goodsID,goodsCode,goodsName,categoryName,sum(goodsNum) goodsNum,sum(sumAmount) sumAmount from(" +
        "select * from [dbo].[GoodsSaleRpt_view] where 1=1 )M "  +
        " group by CompID,goodsID,goodsCode,goodsName,categoryName order by goodsCode,goodsName,categoryName";
        Pagger pagger = new Pagger(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hcomp += dt.Rows[i]["CompID"].ToString() + ",";
            }
            this.hidCompId.Value = Hcomp.Substring(0, Hcomp.Length - 1);
        }
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and  GoodsName like '%" + Common.NoHTML(this.txtGoodsName.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }

        //string hideID = this.hid_product_class.Value.Trim();
        //if (this.txt_product_class.Value.Trim() != "")
        //{
        //    if (!Util.IsEmpty(hideID))
        //    {
        //        string cateID = Common.AdminCategoryId(Convert.ToInt32(hideID));//商品分类递归
        //        strWhere += " and categoryID in(" + cateID + ") ";
        //    }
        //}

        if (this.txtBCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + Common.NoHTML(org) + "' and SalesManID=" + SalesManID + ")";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        return strWhere;
    }
   
}