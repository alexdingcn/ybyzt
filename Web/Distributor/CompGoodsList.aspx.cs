using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Distributor_CompGoodsList : DisPageBase
{
    public string type = string.Empty;
    public string page = "1";//默认初始页
    public int caid = 0;
    //是否启用商品库存，默认启用库存
    public int IsInve = 0;

    public List<Hi.Model.BD_GoodsCategory> goodsCategory = new List<Hi.Model.BD_GoodsCategory>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_product_class\").css(\"width\", \"140px\"); $(\".txt_product_class\").attr(\"class\",\"box txt_product_class\")</script>");

        if (!IsPostBack)
        {
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);
            this.txtPager.Value = "12";
            Bind();
        }
    }

    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        string strWhere = "and ISNULL(dr,0)=0 and ComPid=" + this.CompID;
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }

        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pagers.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "100";
                this.Pagers.PageSize = 100;
            }
        }
        //根据代理商ID获取可采购的商品ID集合
        List<Common.GoodsID> gl = Common.DisEnAreaGoodsID(this.DisID.ToString(), this.CompID.ToString());
        string GoodsId = string.Empty;
        if (gl != null && gl.Count > 0)
        {
            foreach (Common.GoodsID item in gl)
            {
                GoodsId += item.goodsID + ",";

            }
            strWhere += " and ID not in(" + GoodsId.Substring(0, GoodsId.Length - 1) + ")";
        }

        //List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList(Pagers.PageSize, Pagers.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        //goodsCategory = new Hi.BLL.BD_GoodsCategory().GetList("", " ISNULL(dr,0)=0 and ComPid=" + user.CompID, "");

        string sql = string.Format(@"select *,(select Inventory from BD_GoodsInfo where ID=g.ViewInfoID) infoInve,(select BarCode from BD_GoodsInfo where ID=g.ViewInfoID) BarCode from BD_Goods as g left join
 (select prod.GoodsID as proGoodsID from BD_Promotion as pro left join 
BD_PromotionDetail as prod on pro.ID=prod.ProID where  pro.CompID={0} and ISNULL(pro.dr,0)=0
and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) and ISNULL(pro.IsEnabled,0)=1 group by prod.GoodsID) as b on b.proGoodsID=g.ID
where 1=1 {1} and CompID= {0} and isnull(IsEnabled,0)=1 and IsOffline=1 and isnull(dr,0)=0 order by b.proGoodsID desc,g.CreateDate desc", this.CompID, strWhere);
        Pagger pagger = new Pagger(sql);
        Pagers.RecordCount = pagger.getDataCount();
        DataTable dt = pagger.getData(Pagers.PageSize, Pagers.StartRecordIndex - 1);
        this.rptProList.DataSource = dt;
        this.rptProList.DataBind();

    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pagers.CurrentPageIndex.ToString();
        Bind();
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = "";
        //赋值
        string goodsName = this.txtGoodsName.Value.Trim();//商品名称
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += string.Format(" and goodsName like '%{0}%'", goodsName.Replace("'", "''"));
        }
        string CategoryID = this.txtCategory.treeId;
        if (CategoryID != "")
        {
            string cateID = Common.CategoryId(Convert.ToInt32(CategoryID), CompID);//商品分类递归
            strWhere += "and g.categoryID in(" + cateID + ")";
        }

        return strWhere;
    }
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void A_Seek(object sender, EventArgs e)
    {
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }

    protected void A_Collect(object sender, EventArgs e)
    {
        HtmlAnchor a = sender as HtmlAnchor;
        string goodsid = a.Attributes["collect"];

        List<Hi.Model.BD_DisCollect> list = new Hi.BLL.BD_DisCollect().GetList("",
            " disID='" + this.DisID + "' and compID='" + this.CompID + "' and goodsID='" + goodsid + "'", "");
        if (list.Count > 0)
        {
            if (new Hi.BLL.BD_DisCollect().delete(this.DisID, int.Parse(goodsid)))
            {
                page = Pagers.CurrentPageIndex.ToString();
                Bind();
            }
        }
        else
        {
            Hi.Model.BD_DisCollect collect = new Hi.Model.BD_DisCollect();
            collect.CompID = this.CompID;
            collect.DisID = this.DisID;
            collect.DisUserID = this.UserID;
            collect.GoodsID = int.Parse(goodsid);
            collect.IsEnabled = 1;
            collect.CreateDate = DateTime.Now;
            collect.CreateUserID = this.UserID;
            collect.ts = DateTime.Now;
            if (new Hi.BLL.BD_DisCollect().Add(collect) > 0)
            {
                page = Pagers.CurrentPageIndex.ToString();
                Bind();
            }
        }
    }

    public string CheckGoodsCollect(string compID, string goodsID)
    {
        string str = string.Empty;
        List<Hi.Model.BD_DisCollect> list = new Hi.BLL.BD_DisCollect().GetList("",
            " disID='" + this.DisID + "' and compID='" + compID + "' and goodsID='" + goodsID + "' and IsEnabled =1", "");
        str = list.Count > 0 ? "<a class=\"CanceldcGoods\" TipGoods='" + goodsID + "' runat=\"server\" style=\"cursor: pointer;\">取消收藏</a>" : "<a class=\"dcGoods\" TipGoods='" + goodsID + "' runat=\"server\" style=\"cursor: pointer;\">收藏</a>";
        return str;
    }

    public string GetGoodsInfoID(string GoodsID)
    {
        string goodsinfoid = new Hi.BLL.BD_GoodsInfo().GetGoodsInfoID(GoodsID);
        if (goodsinfoid != "")
        {
            return goodsinfoid;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 首次加载商品价格 //by 2016-04-14
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public string TPrice(string Id, decimal SalePrice)
    {
        decimal Price = BLL.Common.GetGoodsPrice(CompID, DisID, Id.ToInt(0));
        Price = Price == 0 ? SalePrice : Price;
        string NewPrice = string.Format("{0:N2}", Convert.ToDecimal(Price).ToString("#,##0.00"));
        return NewPrice;

    }

    public string GoodsinfoModel(string Id)
    {
        Hi.Model.BD_GoodsInfo infomodel = new Hi.BLL.BD_GoodsInfo().GetModel(Id.ToInt(0));

        if (infomodel == null)
            return "";
        if (!infomodel.IsEnabled)
            return "";
        if (infomodel.IsOffline == 0)
            return "";
        return "1";
    }

    /// <summary>
    /// 商品编号
    /// </summary>
    /// <param name="infoID"></param>
    /// <returns></returns>
    public string GoodsInfoPCode(string infoID)
    {
        if (infoID.ToString() != "")
        {
            Hi.Model.BD_GoodsInfo info=new Hi.BLL.BD_GoodsInfo().GetModel(infoID.ToInt(0));
            if (info != null && info.BarCode != null)
                return info.BarCode.ToString();
        }
        return "";
    }
}