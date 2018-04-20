

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class Distributor_FavoriteList : DisPageBase
{
    public string page = "1";//默认初始页
    //是否启用商品库存，默认启用库存
    public int IsInve = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);
            Bind();
        }
    }

    public void Bind()
    {
        string goodsids = new Hi.BLL.BD_DisCollect().GetGoodsIDs(this.DisID);
        if (string.IsNullOrEmpty(goodsids))
        {
            goodsids = "0";
        }
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += " and isnull(dr,0)=0 and id in (" + goodsids + ") and IsEnabled=1 and ISOffline=1";


        Pager.PageSize = 12;

        //根据代理商ID获取可采购的商品ID集合
        List<Common.GoodsID> gl = Common.DisEnAreaGoodsID(this.DisID.ToString(), CompID.ToString());
        string GoodsId = string.Empty;
        if (gl != null && gl.Count > 0)
        {
            foreach (Common.GoodsID item in gl)
            {
                GoodsId += item.goodsID + ",";

            }
            strwhere += " and ID not in(" + GoodsId.Substring(0, GoodsId.Length - 1) + ")";
        }
        else
        {
            //strwhere += " and ID=0";
        }

        //List<Hi.Model.BD_Goods> orders = new Hi.BLL.BD_Goods().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        string sql = string.Format(@"select *,(select Inventory from BD_GoodsInfo where ID=g.ViewInfoID) infoInve,(select BarCode from BD_GoodsInfo where ID=g.ViewInfoID) BarCode from BD_Goods as g left join (select prod.GoodsID as proGoodsID from BD_Promotion as pro left join  BD_PromotionDetail as prod on pro.ID=prod.ProID where  pro.CompID={0} and ISNULL(pro.dr,0)=0
and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) and ISNULL(pro.IsEnabled,0)=1 group by prod.GoodsID) as b on b.proGoodsID=g.ID
where 1=1 {1} and CompID= {0} and isnull(IsEnabled,0)=1 and IsOffline=1 and isnull(dr,0)=0 order by b.proGoodsID desc,g.CreateDate desc", this.CompID, strwhere);
        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.getDataCount();
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        this.rptfavorite.DataSource = dt;
        this.rptfavorite.DataBind();
       
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    protected void A_Del(object sender, EventArgs e)
    {
        HtmlAnchor a = sender as HtmlAnchor;
        int del = int.Parse(a.Attributes["dle"]);
        if (new Hi.BLL.BD_DisCollect().delete(this.DisID, del))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Reslut", "<script>location.href=location.href</script>");
        }
        else
        {
            JScript.AlertMsgOne(this, "删除失败！", JScript.IconOption.错误);
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

    /// <summary>
    /// 商品编号
    /// </summary>
    /// <param name="infoID"></param>
    /// <returns></returns>
    public string GoodsInfoPCode(string infoID)
    {
        if (infoID.ToString() != "")
        {
            Hi.Model.BD_GoodsInfo info = new Hi.BLL.BD_GoodsInfo().GetModel(infoID.ToInt(0));
            if (info != null && info.BarCode != null)
                return info.BarCode.ToString();
        }
        return "";
    }
}
