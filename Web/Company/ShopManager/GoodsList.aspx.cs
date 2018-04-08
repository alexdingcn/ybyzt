using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_ShopManager_GoodsList : CompPageBase
{
    public string IdList = string.Empty;
    public string page="0";
    public int index = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_product_class\").css(\"width\", \"150px\");</script>");
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {

        int pageCount = 0;
        int Counts = 0;
        string strWhere = " and ISNULL(dr,0)=0 and ComPid=" + CompID + " and GoodsName!='' and isoffline=1";
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        //每页显示的数据设置
        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                this.Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        this.rptGoods.DataSource = l;
        this.rptGoods.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private void Where()
    {

        string strWhere = string.Empty;
        //赋值
        string goodsName = this.txtGoodsName.Value.Trim();//商品名称
        string hideID = this.txtCategory.treeId;//类别id
        string idlist = string.Empty;
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += " and goodsname like '%" + goodsName + "%'";
        }
        string idlist2 = string.Empty;
        if (!Util.IsEmpty(hideID))
        {
            string cateID = Common.CategoryId(Convert.ToInt32(hideID), CompID);//商品分类递归
            strWhere += " and categoryID in(" + cateID + ") and isnull(dr,0)=0 and isenabled=1";
        }
        ViewState["strWhere"] = strWhere;
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
            Where();
            Bind();
    }
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Where();
        Bind();
    }
}