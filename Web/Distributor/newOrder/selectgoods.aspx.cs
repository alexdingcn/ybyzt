using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

public partial class Distributor_newOrder_selectgoods : System.Web.UI.Page
{
    public string page;
    public string action;
    //代理商ID
    public int DisID = 0;
    //企业ID
    public int CompId = 0;
    //是否启用商品库存，默认启用库存
    public int IsInve = 0;
    //订单下单数量是否取整
    public string Digits = "0";
    public string goodsInfoId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request["DisId"] != null)
                DisID = Request["DisId"].ToString().ToInt(0);

            if (Request["CompId"] != null)
                CompId = Request["CompId"].ToString().ToInt(0);
            if (Request["index"] != null)
                this.hidIndex.Value = Request["index"].ToString();
            if (Request["goodsInfoId"] != null)
                this.hidgoodsInfoId.Value = Request["goodsInfoId"].ToString();
            if (Request["goodsInfoIdList"] != null)
                this.hidgoodsInfoIdList.Value = Request["goodsInfoIdList"].ToString();
            if (Request["type"] != null)
            {
                if (Request["type"].ToString()=="1")
                {
                    this.hidtype.Value = Request["type"].ToString();
                }
            }
            if (Request["Utype"] != null)
                this.hidUtype.Value = Request["Utype"].ToString();
            if (Request["stock"] != null)
                this.hidStock.Value = Request["stock"] + "";

            Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompId);
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
            tdIsInve.Visible = IsInve == 0;
            hidsDigits.Value = Digits;
            hidIsInve.Value = IsInve.ToString();
            hidCompId.Value = CompId.ToString();
            hidDisId.Value = DisID.ToString();
            hidImgViewPath.Value = Common.GetPicBaseUrl();
            //databind();

            menu2.InnerHtml = GoodsCategory();
        }
    }

    //    public void databind()
    //    {
    //        StringBuilder strwhere = new StringBuilder();
    //        string sql = Returnsql(CompId.ToString(), DisID.ToString(), strwhere.ToString());
    //        //查询商品信息
    //        Pagger pagger = new Pagger(sql);
    //        Pager.RecordCount = pagger.GetDataCount(sql);
    //        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

    //        this.rpGoodsInfo.DataSource = dt;
    //        this.rpGoodsInfo.DataBind();
    //    }

    //    public string Returnsql(string Compid, string DisId, string where)
    //    {
    //        StringBuilder sql = new StringBuilder();

    //        sql.AppendFormat(@"select info.ID,info.GoodsID,info.CompID,info.ValueInfo,info.Inventory,info.SalePrice,info.BarCode
    //,g.GoodsName,g.CategoryID,g.Unit,g.Pic
    //,coll.GoodsID collGoodsID
    //,dis.TinkerPrice disTinkerPrice
    //,pro.ProID ,pro.ID prodID,pro.GoodInfoID,pro.GoodsPrice proGoodsPrice,pro.Discount proDiscount,pro.CreateDate proCreateDate
    //,pro.Type proTypes,pro.ProType
    //,case when isnull(ProType,0) not in (3,5,6) then (case when isnull(ProID,0)<>0 then pro.GoodsPrice
    // when isnull(dis.TinkerPrice,0)<>0 then dis.TinkerPrice else info.SalePrice end)
    // when isnull(dis.TinkerPrice,0)<>0 then dis.TinkerPrice else info.SalePrice end pr
    //from BD_GoodsInfo info left join BD_Goods g on info.GoodsID=g.ID
    //left join BD_DisCollect coll on coll.GoodsID=info.GoodsID and coll.CompID={0} and coll.DisID={1}
    //left join BD_GoodsPrice dis on dis.GoodsInfoID=info.ID and dis.CompID={0} and dis.DisID={1} and Isnull(dis.IsEnabled,0)=1
    //left join (select a.ProID,a.ID,a.GoodInfoID,a.GoodsPrice,a.Discount,a.CreateDate,a.ProType,a.Type from 
    //(select prod.ProID,prod.ID,prod.GoodInfoID,GoodsPrice,pro.Discount,pro.CreateDate,pro.ProType,pro.Type
    //,pro.IsEnabled,pro.dr,pro.ProStartTime,pro.ProEndTime,pro.CompID
    //from BD_Promotion pro left join  BD_PromotionDetail prod on pro.ID=prod.ProID ) a
    //inner join (select  max(prod.ID) ID,max(ProID) proID,prod.GoodInfoID from BD_Promotion as pro 
    //left join BD_PromotionDetail as prod on pro.ID=prod.ProID group by prod.GoodInfoID) b 
    //on a.ID=b.ID and a.proID=b.proID and a.goodInfoID=b.GoodInfoID
    //where Isnull(a.Type,0) in (0,1) and isnull(a.ProType,0)in(1,2,3,4)
    //and isnull(a.dr,0)=0  and isnull(a.IsEnabled,0)=1 and getdate() between  a.ProStartTime and a.ProEndTime
    //and a.CompID={0}) as pro on pro.GoodInfoID=info.ID
    //where isnull(info.IsOffline,0)=1 and isnull(info.dr,0)=0 and isnull(info.IsEnabled,0)=1 and info.CompID={0} 
    //and isnull(g.IsEnabled,0)=1 and isnull(g.IsOffline,0)=1 and isnull(g.dr,0)=0 {2}
    //order by pro.CreateDate desc,coll.GoodsID desc,g.CreateDate desc", Compid, DisId, where);

    //        return sql.ToString();
    //    }

    //    public void Pager_PageChanged(object sender, EventArgs e)
    //    {
    //        databind();
    //    }

    /// <summary>
    /// 绑定商品分类
    /// </summary>
    /// <param name="CompId"></param>
    /// <returns></returns>
    public string GoodsCategory()
    {
        StringBuilder sb = new StringBuilder();

        List<Hi.Model.SYS_GType> gTypes = new Hi.BLL.SYS_GType().GetList("Deep,TypeName,ID,ParentID", " Isnull(dr,0)=0 and Isnull(IsEnabled,0)=1", "");

        if (gTypes != null && gTypes.Count > 0)
        {
            //商品分类一级
            List<Hi.Model.SYS_GType> cl1 = gTypes.FindAll(p => p.Deep == 1 && p.ParentId == 0);
            if (cl1 != null && cl1.Count > 0)
            {
                sb.Append("<div class=\"sorts\">");

                sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"\">{0}</a></div>", "全部");
                sb.Append("</div>");
                foreach (var item in cl1)
                {
                    sb.Append("<div class=\"sorts\">");

                    sb.AppendFormat("<div class=\"sorts1\"><i class=\"arrow2\"></i><a href=\"javascript:;\" class=\"a1\" tip=\"{1}\">{0}</a></div>", item.TypeName, item.ID);

                    //商品分类二级
                    List<Hi.Model.SYS_GType> cl2 = gTypes.FindAll(p => p.Deep == 2 && p.ParentId == item.ID);
                    if (cl2 != null && cl2.Count > 0)
                    {
                        sb.Append("<ul class=\"sorts2\" tipdis=\"no\" style=\"display:none;\">");
                        foreach (var item2 in cl2)
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<i class=\"arrow3\"></i><a href=\"javascript:;\" tip=\"{1}\">{0}</a>", item2.TypeName, item2.ID);
                            //商品分类三级
                            List<Hi.Model.SYS_GType> cl3 = gTypes.FindAll(p => p.Deep == 3 && p.ParentId == item2.ID);

                            if (cl3 != null && cl3.Count > 0)
                            {
                                sb.Append("<ul class=\"sorts3\" tipdis=\"no\" style=\"display:none;\">");
                                foreach (var item3 in cl3)
                                {
                                    sb.AppendFormat("<li tip=\"{1}\"><a href=\"javascript:;\" tip=\"{1}\">{0}</a></li>", item3.TypeName, item3.ID);
                                }
                                sb.Append("</ul>");
                            }
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</div>");
                }
            }
        }
        return sb.ToString();
    }
}