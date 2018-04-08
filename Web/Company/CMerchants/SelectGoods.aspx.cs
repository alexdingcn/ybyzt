using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using DBUtility;

/**
 * 应该是Goods表的数据
 * **/
public partial class Company_CMerchants_SelectGoods : CompPageBase
{
    public int num = 0;
    public string IdList = string.Empty;
    public string page;

    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_product_class\").css(\"width\", \"150px\");</script>");
        if (!IsPostBack)
        {
            this.txtPager.Value = "10";
            Bind();
            num = 1;
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 10;
                this.txtPager.Value = "10";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        string strWhere = string.Format("and a.compid={0} and b.compid={0}", this.CompID);
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }

        Pagger pagger = new Pagger(Returnsql(strWhere.ToString()));
        Pager.RecordCount = pagger.getDataCount();
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

        this.rptGoodsInfo.DataSource = dt;
        this.rptGoodsInfo.DataBind();

        //ClientScript.RegisterStartupScript(this.GetType(), "", "<script>$(function(){     $(\"#CB_SelAll\").trigger(\"click\");})</script>");
    }

    public string Returnsql(string where)
    {
        string sql = string.Format(@"select a.*,b.GoodsName,b.memo,b.CategoryID from BD_GoodsInfo as a ,BD_Goods as b where a.GoodsID=b.ID and b.IsEnabled=1 and ISNULL(a.IsOffline,0)=1 and ISNULL(a.dr,0)=0 and ISNULL(b.dr,0)=0 and a.IsEnabled=1  {0} order by a.CreateDate desc", where);
        return sql;
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = string.Empty;
        //赋值
        string goodsName = this.txtGoodsName.Value.Trim().Replace("'", "''");//商品名称
        string hideID = this.txtCategory.treeId;//类别id
        string idlist = string.Empty;
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += string.Format(" and (goodsname like '%{0}%' or a.barcode like '%{0}%')", goodsName);
        }
        string idlist2 = string.Empty;
        if (!Util.IsEmpty(hideID))
        {
            string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
            strWhere += " and categoryID in(" + cateID + ")";
        }
        return strWhere;
    }

    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        
        Bind();
        num = 1;
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        
        Bind();
        num = 1;
    }

   
}