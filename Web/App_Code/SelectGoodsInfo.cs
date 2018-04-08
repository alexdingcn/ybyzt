using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Text;

/// <summary>
///SelectGoodsInfo 的摘要说明
/// </summary>
public class SelectGoodsInfo
{
    //代理商Id
    public static int DisId;
    public static string ProID = "0";
    public static string ProPrice = "";
    public static string ProIDD = "0";
    public static string ProType = "";
    public int IsInve = 0;
    static Hi.BLL.DIS_Order OrderBLL = new Hi.BLL.DIS_Order();  //订单表
    static Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();  //订单明细表
    static Hi.BLL.BD_GoodsInfo GoodsInfoBll = new Hi.BLL.BD_GoodsInfo();  //商品信息表
    static Hi.BLL.BD_Goods GoodsBll = new Hi.BLL.BD_Goods();  //商品基本表
    public SelectGoodsInfo()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 选择商品sql
    /// </summary>
    /// <param name="Compid">厂商ID</param>
    /// <param name="DisId">代理商ID</param>
    /// <param name="where">查询条件</param>
    /// <param name="Utype">1、代理商  2、厂商</param>
    /// <returns></returns>
    public static string Returnsql(string Compid, string DisId, string where, string Utype)
    {
        //string ss = Utype == "1" ? "(select cd.GoodsID from  YZT_Contract con left join  YZT_ContractDetail cd on con.ID=cd.ContID where con.DisID=" + DisId + " and con.CState=1 union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID=fc.CMID left join YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.CompID in (" + Compid + ") and fc.DisID=" + DisId + ") cd left join" : "";
        //string ss1 = Utype == "1" ? "on cd.GoodsID=info.ID" : "";

        string ss = Utype == "1" ? "(select i.GoodsID,I.ID from  (select cd.GoodsID from  YZT_Contract con left join  YZT_ContractDetail cd on con.ID=cd.ContID where con.DisID=" + DisId + " and con.CState=1 union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID=fc.CMID left join YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.CompID in (" + Compid + ") and fc.DisID=" + DisId + ") cd left join BD_GoodsInfo i on i.ID=cd.GoodsID) cd on cd.ID=info.ID left join" : "";

        string ss1 = Utype == "1" ? "left join" : "left join";

        StringBuilder sql = new StringBuilder();

        sql.AppendFormat(@"select  info.ID,info.GoodsID,info.CompID,info.ValueInfo,info.Inventory,info.TinkerPrice,info.BarCode
,g.GoodsName,g.CategoryID,g.Unit,g.Pic,g.Pic2
,coll.GoodsID collGoodsID
,pro.ProID ,pro.ID prodID,pro.GoodInfoID,pro.GoodsPrice proGoodsPrice,pro.Discount proDiscount,pro.CreateDate proCreateDate
,pro.Type proTypes,pro.ProType
,info.TinkerPrice typeTinkerPrice 
,info.TinkerPrice disTinkerPrice
,case when isnull(ProType,0) not in (3,5,6) then pro.GoodsPrice else null end disProPr
,info.TinkerPrice pr
from BD_GoodsInfo info {4} {3} BD_Goods g on info.GoodsID=g.ID
left join BD_DisCollect coll on coll.GoodsID=info.GoodsID and isnull(coll.dr,0)=0 and coll.CompID={0} and coll.DisID={1}
left join (select * from (select  ROW_NUMBER() over(PARTITION BY GoodInfoID order by p.createdate desc) rowid, 
pd.ProID,pd.ID,pd.GoodInfoID,GoodsPrice,p.Discount,p.CreateDate,p.ProType,p.Type
,p.IsEnabled,p.dr,p.ProStartTime,p.ProEndTime,p.CompID
from BD_PromotionDetail pd  join BD_Promotion p on  p.ID=pd.ProID and 
isnull(p.Type,0) in (0,1) and isnull(p.ProType,0)in(1,2,3,4)
and isnull(p.dr,0)=0  and isnull(p.IsEnabled,0)=1 and getdate() between  p.ProStartTime and dateadd(D,1, p.ProEndTime)
and p.CompID={0})b where b.rowid=1) as pro on pro.GoodInfoID=info.ID
where not exists (select GoodsID from BD_GoodsAreas ga where info.GoodsID=ga.GoodsID and ga.CompID={0} and ga.DisID={1} and IsNull(ga.dr,0)=0) and info.CompID={0}  and isnull(info.IsOffline,0)=1 and isnull(info.dr,0)=0 and isnull(info.IsEnabled,0)=1 and isnull(g.IsEnabled,0)=1 and isnull(g.IsOffline,0)=1 and isnull(g.dr,0)=0 {2}
order by pro.CreateDate desc,coll.GoodsID desc,g.CreateDate desc", Compid, DisId, where, ss, ss1);

        return sql.ToString();
    }

    public static string Returnsql2(string Compid, string DisId, string where, string rowid = "", string Utype="")
    {

        string ss = Utype == "1" ? "(select i.GoodsID,I.ID  from  (select cd.GoodsID from  YZT_Contract con left join  YZT_ContractDetail cd on con.ID=cd.ContID where con.DisID=" + DisId + " and con.CState=1 union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID=fc.CMID left join YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.CompID in (" + Compid + ") and fc.DisID=" + DisId + ") cd left join BD_GoodsInfo i on i.ID=cd.GoodsID) cd on cd.ID=info.ID left join" : "";

        string ss1 = Utype == "1" ? "left join" : "left join";

        StringBuilder sql = new StringBuilder();

        sql.AppendFormat(@"select * from ( select ROW_NUMBER() over(PARTITION BY info.GoodsID order by  pro.CreateDate desc,coll.GoodsID desc,g.CreateDate desc) rowid, info.ID,info.GoodsID,info.CompID,info.ValueInfo,info.Inventory,info.TinkerPrice,info.BarCode
,g.GoodsName,g.CategoryID,g.Unit,g.Pic,g.Pic2,pro.CreateDate as CreateDate1,g.CreateDate as CreateDate2
,coll.GoodsID collGoodsID
,pro.ProID ,pro.ID prodID,pro.GoodInfoID,pro.GoodsPrice proGoodsPrice,pro.Discount proDiscount,pro.CreateDate proCreateDate
,pro.Type proTypes,pro.ProType
,info.TinkerPrice typeTinkerPrice 
,info.TinkerPrice disTinkerPrice
,case when isnull(ProType,0) not in (3,5,6) then pro.GoodsPrice else null end disProPr
,info.TinkerPrice pr
from  BD_GoodsInfo info {5} {4} BD_Goods g on info.GoodsID=g.ID
left join BD_DisCollect coll on coll.GoodsID=info.GoodsID and isnull(coll.dr,0)=0 and coll.CompID={0} and coll.DisID={1}
left join (select * from (select  ROW_NUMBER() over(PARTITION BY GoodInfoID order by p.createdate desc) rowid, 
pd.ProID,pd.ID,pd.GoodInfoID,GoodsPrice,p.Discount,p.CreateDate,p.ProType,p.Type
,p.IsEnabled,p.dr,p.ProStartTime,p.ProEndTime,p.CompID
from BD_PromotionDetail pd  join BD_Promotion p on  p.ID=pd.ProID and 
isnull(p.Type,0) in (0,1) and isnull(p.ProType,0)in(1,2,3,4)
and isnull(p.dr,0)=0  and isnull(p.IsEnabled,0)=1 and getdate() between  p.ProStartTime and dateadd(D,1, p.ProEndTime)
and p.CompID={0})b where b.rowid=1) as pro on pro.GoodInfoID=info.ID
where not exists (select GoodsID from BD_GoodsAreas ga where info.GoodsID=ga.GoodsID and ga.CompID={0} and ga.DisID={1} and IsNull(ga.dr,0)=0) and info.CompID={0}  and isnull(info.dr,0)=0 and isnull(info.IsEnabled,0)=1 and isnull(g.IsEnabled,0)=1 and isnull(g.IsOffline,0)=1 and isnull(g.dr,0)=0 {2} )yy {3}
order by CreateDate1 desc,GoodsID desc,CreateDate2 desc", Compid, DisId, where, rowid, ss, ss1);

        return sql.ToString();
    }
    /// <summary>
    /// 修改商品信息
    /// </summary>
    /// <param name="GoodsinfoID">商品ID</param>
    /// <param name="DisId">代理商Id</param>
    /// <param name="CompId">企业Id</param>
    /// <param name="Price">单价</param>
    /// <param name="GoodsNum">数量</param>
    /// <param name="Remark">备注</param>
    public static void UpDateGoods(int GoodsinfoID, int DisId, int CompId, decimal Price, decimal GoodsNum, string Remark, string type)
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {
            DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

            DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}' and GoodsinfoID='{2}'", DisId, CompId, GoodsinfoID));

            decimal TinkerPrice = 0;

            if (dr.Length > 0)
            {
                //获取商品信息
                Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodsinfoID);
                decimal SalePrice = goodsinfoModel.TinkerPrice;
                if (goodsinfoModel != null)
                {
                    if (goodsinfoModel.IsOffline == 1)
                    {
                        //判断是否存在促销活动 优先取促销活动价
                        if (Common.GetPro(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString()) == 0)
                        {
                            //判断是否启用代理商价格维护
                            if (Common.IsDisPrice != "0")
                            {
                                //获取商品价格
                                List<Hi.Model.BD_GoodsPrice> pl = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", "DisID=" + DisId + " and CompID=" + CompId + " and GoodsInfoID=" + GoodsinfoID + " and isnull(IsEnabled,0)=1 and ISNULL(dr,0)=0", "");

                                if (pl != null && pl.Count > 0)
                                {

                                    TinkerPrice = pl[0].TinkerPrice.ToString() == "" ? goodsinfoModel.TinkerPrice : pl[0].TinkerPrice;
                                    SalePrice = pl[0].TinkerPrice.ToString() == "" ? goodsinfoModel.TinkerPrice : pl[0].TinkerPrice;
                                }
                                else
                                {
                                    TinkerPrice = goodsinfoModel.TinkerPrice;
                                    SalePrice = goodsinfoModel.TinkerPrice;
                                }
                            }
                            else
                            {
                                TinkerPrice = goodsinfoModel.TinkerPrice;
                                SalePrice = goodsinfoModel.TinkerPrice;
                            }
                        }
                        else
                        {
                            TinkerPrice = Common.GetProPrice(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString());
                        }
                    }
                    else
                    {
                        TinkerPrice = 0;
                        SalePrice = 0;
                    }
                    //判断是不是赠品
                    //if (GoodsName(goodsinfoModel.GoodsID, "IsSale") == "1")
                    //{
                    //    TinkerPrice = 0;
                    //}

                    dr[0]["GoodsNum"] = GoodsNum;
                    //if (!Price.ToString().Equals("0"))
                    //{

                    //}
                    //else
                    //{
                    if (type == "1")
                    {
                        //修改商品价格  价格预审
                        //dr[0]["Price"] = Price;
                        dr[0]["Total"] = Price * GoodsNum;
                        dr[0]["AuditAmount"] = Price;
                    }
                    else if (type == "2")
                    {
                        //修改商品价格
                        dr[0]["Price"] = Price;
                        dr[0]["Price2"] = SalePrice;
                        dr[0]["Total"] = Price * GoodsNum;
                        dr[0]["AuditAmount"] = Price;
                    }
                    else if (type == "3")
                    {
                        //新增商品
                        dr[0]["Price"] = TinkerPrice;
                        dr[0]["Price2"] = SalePrice;
                        dr[0]["Total"] = TinkerPrice * GoodsNum;
                        dr[0]["AuditAmount"] = TinkerPrice;
                    }
                    //}
                    if (Remark != "")
                        dr[0]["Remark"] = Remark;
                }
            }
        }
    }
    /// <summary>
    /// 商品总价
    /// </summary>
    /// <returns></returns>
    public static decimal SumTotal(int DisId, int CompId)
    {
        try
        {
            decimal sum = 0;
            if (HttpContext.Current.Session["GoodsInfo"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

                DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}'", DisId, CompId));

                if (dr.Length > 0)
                {
                    foreach (var item in dr)
                    {
                        sum += item["Total"].ToString().ToDecimal(0);
                    }
                }
                HttpContext.Current.Session["GoodsInfo"] = dt;
            }
            return sum;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 商品总价
    /// </summary>
    /// <returns></returns>
    public static decimal SumTotal(int DisId, int CompId, string ProPrice)
    {
        try
        {
            decimal sum = 0;
            if (HttpContext.Current.Session["GoodsInfo"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

                DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}'", DisId, CompId));

                if (dr.Length > 0)
                {
                    foreach (var item in dr)
                    {
                        sum += item["Total"].ToString().ToDecimal(0);
                    }
                }
                HttpContext.Current.Session["GoodsInfo"] = dt;
            }
            return sum - ProPrice.ToDecimal(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 商品总数量
    /// </summary>
    /// <returns></returns>
    public static decimal SumNum(int DisId, int CompId)
    {
        try
        {
            decimal sum = 0;
            if (HttpContext.Current.Session["GoodsInfo"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

                DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}'", DisId, CompId));

                if (dr.Length > 0)
                {
                    foreach (var item in dr)
                    {
                        sum += item["GoodsNum"].ToString().ToDecimal(0);
                    }
                }
                HttpContext.Current.Session["GoodsInfo"] = dt;
            }
            return sum;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 选择商品
    /// </summary>
    /// <param name="Id">商品Id</param>
    public static void Goods(int Id, int DisId, int CompId)
    {
        DataTable dt = null;
        int ProID = 0;
        //获取商品信息
        Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(Id);
        string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompId);

        if (goodsinfoModel != null)
        {
            int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
            if (IsInve == 0)
            {
                if (goodsinfoModel.Inventory <= 0)
                    return;
            }

            decimal TinkerPrice = 0;
            decimal Price = goodsinfoModel.TinkerPrice;
            decimal Price2 = goodsinfoModel.TinkerPrice;
            if (goodsinfoModel.IsOffline == 1)
            {
                //判断是否存在促销活动 优先取促销活动价
                ProID = Common.GetPro(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString());
                if (ProID == 0)
                {
                    //判断是否启用代理商价格维护
                    if (Common.IsDisPrice != "0")
                    {
                        //获取商品价格
                        List<Hi.Model.BD_GoodsPrice> pl = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", "DisID=" + DisId + " and CompID=" + CompId + " and GoodsInfoID=" + Id + " and isnull(IsEnabled,0)=1 and ISNULL(dr,0)=0", "");

                        if (pl != null && pl.Count > 0)
                        {
                            TinkerPrice = pl[0].TinkerPrice.ToString() == "" ? goodsinfoModel.TinkerPrice : pl[0].TinkerPrice;
                            Price = pl[0].TinkerPrice.ToString() == "" ? goodsinfoModel.TinkerPrice : pl[0].TinkerPrice;
                        }
                        else
                        {
                            TinkerPrice = goodsinfoModel.TinkerPrice.ToString() == "" ? 0 : goodsinfoModel.TinkerPrice;
                            Price = goodsinfoModel.TinkerPrice.ToString() == "" ? 0 : goodsinfoModel.TinkerPrice;
                        }
                    }
                    else
                    {
                        TinkerPrice = goodsinfoModel.TinkerPrice.ToString() == "" ? 0 : goodsinfoModel.TinkerPrice;
                        Price = goodsinfoModel.TinkerPrice.ToString() == "" ? 0 : goodsinfoModel.TinkerPrice;
                    }
                }
                else
                {
                    TinkerPrice = Common.GetProPrice(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString());
                }
            }
            else
            {
                Price = 0;
                TinkerPrice = 0;
            }

            //判断是不是赠品
            //if (GoodsName(goodsinfoModel.GoodsID, "IsSale") == "1")
            //{
            //    TinkerPrice = 0;
            //}

            if (HttpContext.Current.Session["GoodsInfo"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Id", typeof(string));     //订单明细Id
                dt.Columns.Add("DisId", typeof(string));     //代理商Id
                dt.Columns.Add("CompId", typeof(string));     //企业Id
                dt.Columns.Add("GoodsID", typeof(Int64)); //商品基本档案ID
                dt.Columns.Add("GoodsinfoID", typeof(Int64)); //商品ID
                dt.Columns.Add("BarCode", typeof(string));   //商品编码
                dt.Columns.Add("GoodsName", typeof(string));    //商品名称
                dt.Columns.Add("GoodsInfos", typeof(string));   //商品属性信息
                dt.Columns.Add("Inventory", typeof(decimal));   //商品库存
                dt.Columns.Add("Price", typeof(decimal)); //商品价格
                dt.Columns.Add("Price2", typeof(decimal)); //商品价格
                dt.Columns.Add("AuditAmount", typeof(decimal)); //审核后价格
                dt.Columns.Add("Unit", typeof(string)); //商品计量单位
                dt.Columns.Add("Pic", typeof(string)); //商品图片
                //dt.Columns.Add("GoodsCode", typeof(string)); //商品编码
                dt.Columns.Add("GoodsNum", typeof(decimal)); //商品数量
                dt.Columns.Add("Remark", typeof(string)); //备注
                dt.Columns.Add("vdef1", typeof(string)); //是否促销商品
                dt.Columns.Add("vdef2", typeof(string)); //促销商品数量
                dt.Columns.Add("vdef3", typeof(string)); //促销Protype
                dt.Columns.Add("Total", typeof(decimal)); //小计

                DataRow dr1 = dt.NewRow();

                dr1["Id"] = 0;
                dr1["DisId"] = DisId;
                dr1["CompId"] = CompId;
                dr1["GoodsID"] = goodsinfoModel.GoodsID;
                dr1["GoodsinfoID"] = Id;
                dr1["BarCode"] = goodsinfoModel.BarCode;
                dr1["GoodsName"] = GoodsName(goodsinfoModel.GoodsID, "GoodsName");
                dr1["GoodsInfos"] = goodsinfoModel.ValueInfo;// == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : goodsinfoModel.ValueInfo;
                dr1["Price"] = TinkerPrice;
                dr1["Price2"] = Price;
                dr1["Inventory"] = goodsinfoModel.Inventory;
                dr1["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                dr1["AuditAmount"] = TinkerPrice;
                dr1["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                dr1["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", (1).ToString("#,####" + Digits)));
                dr1["Remark"] = "";
                dr1["vdef1"] = ProID; //Common.GetPro(goodsinfoModel.GoodsID, Id.ToString(), CompId.ToString());
                dr1["vdef2"] = "0";
                dr1["vdef3"] = "";
                dr1["Total"] = TinkerPrice * 1;

                dt.Rows.Add(dr1);
            }
            else
            {
                dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;
                DataRow dr2 = dt.NewRow();

                dr2["Id"] = 0;
                dr2["DisId"] = DisId;
                dr2["CompId"] = CompId;
                dr2["GoodsID"] = goodsinfoModel.GoodsID;
                dr2["GoodsinfoID"] = Id;
                dr2["BarCode"] = goodsinfoModel.BarCode;
                dr2["GoodsName"] = GoodsName(goodsinfoModel.GoodsID, "GoodsName");
                dr2["GoodsInfos"] = goodsinfoModel.ValueInfo;// == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : goodsinfoModel.ValueInfo;
                dr2["Inventory"] = goodsinfoModel.Inventory;
                dr2["Price"] = TinkerPrice;
                dr2["Price2"] = Price;
                dr2["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                dr2["AuditAmount"] = TinkerPrice;
                dr2["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                dr2["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", (1).ToString("#,####" + Digits)));
                dr2["Remark"] = "";
                dr2["vdef1"] = ProID;// Common.GetPro(goodsinfoModel.GoodsID, Id.ToString(), CompId.ToString());
                dr2["vdef2"] = "0";
                dr2["vdef3"] = "";
                dr2["Total"] = TinkerPrice * 1;
                dt.Rows.Add(dr2);
            }
            dt.DefaultView.Sort = "id desc";
            HttpContext.Current.Session["GoodsInfo"] = dt;
        }

    }
    /// <summary>
    /// 订单商品明细
    /// </summary>
    /// <param name="Id">订单Id</param>
    public static void OrderDetail(int Id, int DisId, int CompId, string type)
    {
        HttpContext.Current.Session["GoodsInfo"] = null;

        List<Hi.Model.DIS_OrderDetail> l = OrderDetailBll.GetList("", " isnull(dr,0)=0 and OrderId=" + Id, "Id desc");

        string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompId);

        if (l != null && l.Count > 0)
        {
            DataTable dt = null;

            foreach (Hi.Model.DIS_OrderDetail item in l)
            {
                //获取商品信息
                Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(item.GoodsinfoID);

                if (goodsinfoModel != null)
                {
                    if (goodsinfoModel.dr == 1 || goodsinfoModel.IsOffline == 0 || goodsinfoModel.IsEnabled == false)
                        continue;
                    if (type.Trim() == "2")
                    {
                        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompId).ToInt(0);
                        if (IsInve == 0)
                        {
                            if (goodsinfoModel.Inventory <= 0)
                                continue;
                        }
                    }
                    //获取商品价格
                    if (HttpContext.Current.Session["GoodsInfo"] == null)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Id", typeof(string));     //订单明细Id
                        dt.Columns.Add("DisId", typeof(string));     //代理商Id
                        dt.Columns.Add("CompId", typeof(string));     //企业Id
                        dt.Columns.Add("GoodsID", typeof(Int64)); //商品基本档案ID
                        dt.Columns.Add("GoodsinfoID", typeof(Int64)); //商品ID
                        dt.Columns.Add("BarCode", typeof(string));   //商品编码
                        dt.Columns.Add("GoodsName", typeof(string));    //商品名称
                        dt.Columns.Add("GoodsInfos", typeof(string));   //商品属性信息
                        dt.Columns.Add("Price", typeof(decimal)); //商品价格
                        dt.Columns.Add("Price2", typeof(decimal)); //商品价格
                        dt.Columns.Add("Inventory", typeof(decimal));   //商品库存
                        dt.Columns.Add("AuditAmount", typeof(decimal)); //审核后价格
                        dt.Columns.Add("Unit", typeof(string)); //商品计量单位
                        dt.Columns.Add("Pic", typeof(string)); //商品图片
                        //dt.Columns.Add("StockNum", typeof(int)); //数据类型为 文本
                        dt.Columns.Add("GoodsNum", typeof(decimal)); //商品数量
                        dt.Columns.Add("Remark", typeof(string)); //备注
                        dt.Columns.Add("vdef1", typeof(string)); //是否促销商品
                        dt.Columns.Add("vdef2", typeof(string)); //促销商品数量
                        dt.Columns.Add("vdef3", typeof(string)); //促销Protype
                        dt.Columns.Add("Total", typeof(decimal)); //小计

                        DataRow dr1 = dt.NewRow();

                        dr1["Id"] = item.ID;
                        dr1["DisId"] = DisId;
                        dr1["CompId"] = CompId;
                        dr1["GoodsID"] = goodsinfoModel.GoodsID;
                        dr1["GoodsinfoID"] = goodsinfoModel.ID;
                        dr1["BarCode"] = goodsinfoModel.BarCode;
                        dr1["GoodsName"] = GoodsName(goodsinfoModel.GoodsID, "GoodsName");
                        dr1["GoodsInfos"] = item.GoodsInfos;// == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : item.GoodsInfos;
                        dr1["Price"] = item.Price;
                        dr1["Price2"] = item.GoodsPrice;
                        dr1["Inventory"] = goodsinfoModel.Inventory + item.GoodsNum;
                        dr1["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                        dr1["AuditAmount"] = item.AuditAmount;
                        dr1["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                        dr1["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item.GoodsNum.ToString("#,####" + Digits)));
                        dr1["Remark"] = item.Remark;
                        if (item.ProID != null)
                            dr1["vdef1"] = item.ProID.ToString() == "" ? "0" : item.ProID.ToString();
                        else
                            dr1["vdef1"] = "0";
                        if (item.vdef5 != "")
                        {
                            dr1["vdef2"] = decimal.Parse(string.Format("{0:N4}", Convert.ToDecimal(item.vdef5).ToString("#,####" + Digits)));
                        }
                        else
                            dr1["vdef2"] = "0";
                        //dr1["vdef3"] = item.vdef6;

                        dr1["Total"] = item.sumAmount;

                        dt.Rows.Add(dr1);
                    }
                    else
                    {
                        dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;
                        DataRow dr2 = dt.NewRow();

                        dr2["Id"] = item.ID;
                        dr2["DisId"] = DisId;
                        dr2["CompId"] = CompId;
                        dr2["GoodsID"] = goodsinfoModel.GoodsID;
                        dr2["GoodsinfoID"] = goodsinfoModel.ID;
                        dr2["BarCode"] = goodsinfoModel.BarCode;
                        dr2["GoodsName"] = GoodsName(goodsinfoModel.GoodsID, "GoodsName");
                        dr2["GoodsInfos"] = item.GoodsInfos;// == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : item.GoodsInfos;
                        dr2["Price"] = item.Price;
                        dr2["Price2"] = item.GoodsPrice;
                        dr2["Inventory"] = goodsinfoModel.Inventory + item.GoodsNum;
                        dr2["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                        dr2["AuditAmount"] = item.AuditAmount;
                        dr2["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                        dr2["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item.GoodsNum.ToString("#,####" + Digits)));
                        dr2["Remark"] = item.Remark;
                        if (item.ProID != null)
                            dr2["vdef1"] = item.ProID.ToString() == "" ? "0" : item.ProID.ToString();
                        else
                            dr2["vdef1"] = "0";
                        if (item.vdef5 != "")
                        {
                            dr2["vdef2"] = decimal.Parse(string.Format("{0:N4}", Convert.ToDecimal(item.vdef5).ToString("#,####" + Digits)));
                        }
                        else
                            dr2["vdef2"] = "0";
                        //dr2["vdef3"] = item.vdef6;
                        dr2["Total"] = item.sumAmount;
                        dt.Rows.Add(dr2);
                    }
                    dt.DefaultView.Sort = "id desc";
                    HttpContext.Current.Session["GoodsInfo"] = dt;
                }
            }
        }
    }

    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public static string Clear()
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {

            HttpContext.Current.Session["GoodsInfo"] = null;
            HttpContext.Current.Session.Remove("GoodsInfo");
            //HttpContext.Current.Session.Abandon(); //清除全部Session
        }
        return "";
    }
    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public static void Clear(int CompId)
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {
            DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

            DataRow[] dr = dt.Select(string.Format("CompId='{0}'", CompId));

            if (dr.Length > 0)
            {
                foreach (var item in dr)
                {
                    dt.Rows.Remove(item);
                }
            }
            HttpContext.Current.Session["GoodsInfo"] = dt;
        }
    }

    /// <summary>
    /// 获取商品名称
    /// </summary>
    /// <param name="Id">商品信息Id</param>
    /// <returns></returns>
    public static string GoodsName(int Id, string col)
    {
        List<Hi.Model.BD_Goods> l = GoodsBll.GetList("", " isnull(dr,0)=0 and Id=" + Id, "");
        if (l != null && l.Count > 0)
        {
            foreach (Hi.Model.BD_Goods item in l)
            {
                if (col == "GoodsName")
                {
                    return item.GoodsName;
                }
                else if (col == "GoodsCode")
                {
                    return item.GoodsCode;
                }
                else if (col == "Unit")
                {
                    return item.Unit;
                }
                else if (col == "Pic")
                {
                    return item.Pic;
                }
                else if (col == "IsSale")
                {
                    return item.IsSale.ToString();
                }
            }
        }
        return "";
    }
    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="DisId"></param>
    /// <param name="CompId"></param>
    public static string DelGoods(int GoodsinfoID, int DisId, int CompId)
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {
            DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

            DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}' and GoodsinfoID='{2}'", DisId, CompId, GoodsinfoID));
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
            }
            HttpContext.Current.Session["GoodsInfo"] = dt;
        }
        decimal TotalAmount = SelectGoodsInfo.SumTotal(DisId, CompId);
        ProPrice = Common.GetProPrice(TotalAmount, out ProID, out ProIDD, out ProType, CompId).ToString();
        //商品删除后总价格
        string monery = SelectGoods.SumTotal(DisId, CompId).ToString("N");
        string Json = "{\"ds\":\"" + true + "\",\"SumTotal\":\"" + monery + "\",\"proPrice\":\"" + ProPrice + "\"}";
        return Json;
    }

    /// <summary>
    /// 促销提示
    /// </summary>
    /// <param name="ProID">促销主表ID</param>
    /// <param name="Protype">促销详细</param>
    /// <param name="proGoodsPrice">促销价格</param>
    /// <param name="unit">商品单位</param>
    /// <returns></returns>
    public static string protitle(string ProID, string Protype, string unit)
    {
        string str = "";
        if (ProID != "")
        {
            if (!string.IsNullOrEmpty(Protype))
            {
                string[] type = Protype.Split(new char[] { ',' });
                str = ConvertJson.IsCx(type[0], type[1], type[2], type[3], unit);
            }
        }
        return str;
    }

    /// <summary>
    /// 促销提示
    /// </summary>
    /// <param name="ProID">促销主表ID</param>
    /// <param name="Protype">促销详细</param>
    /// <param name="proGoodsPrice">促销价格</param>
    /// <param name="unit">商品单位</param>
    /// <returns></returns>
    public static string protitle(string ProID, string Protype, string unit, int Compid)
    {
        string str = "";
        if (ProID != "")
        {
            if (!string.IsNullOrEmpty(Protype))
            {
                string[] type = Protype.Split(new char[] { ',' });
                str = ConvertJson.IsCxComp(type[0], type[1], type[2], type[3], unit, Compid);
            }
        }
        return str;
    }

    /// <summary>
    /// 购物车促销提示
    /// </summary>
    /// <param name="ProID">促销主表ID</param>
    /// <param name="unit">商品单位</param>
    /// <returns></returns>
    public static string protitle(string ProID, string unit)
    {
        string str = "";
        if (ProID != "")
        {
            DataTable dt = new Hi.BLL.BD_Promotion().ProType(ProID.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                str = ConvertJson.IsCx(dt.Rows[0]["Type"].ToString(), dt.Rows[0]["Protype"].ToString(), dt.Rows[0]["GoodsPrice"].ToString(), dt.Rows[0]["DisCount"].ToString(), unit);
            }
        }
        return str;
    }

    /// <summary>
    /// 格式化商品属性
    /// </summary>
    /// <param name="Infos"></param>
    /// <returns></returns>
    public static string GetGoodsInfos(string Infos)
    {
        if (Infos == "")
            return "";
        return Infos.Replace(":", "：").Substring(0, Infos.Length - 1);
    }

    /// <summary>
    /// 返回商品图片
    /// </summary>
    /// <param name="GoodsInfoID"></param>
    /// <returns></returns>
    public static string GetGoodsPic(string GoodsPic)
    {
        string ViewPath = Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/";
        string pic = "";

        if (!string.IsNullOrEmpty(GoodsPic))
            pic += GoodsPic;

        if (pic != "")
        {
            ViewPath = ViewPath + pic;
            return ViewPath;
        }
        return ViewPath;
    }

    /// <summary>
    /// 下单数量显示保留小数位数
    /// </summary>
    /// <param name="num"></param>
    /// <param name="Digits"></param>
    /// <returns></returns>
    public static string GetNum(string num, string Digits)
    {
        return string.Format("{0:N4}", num.ToDecimal(0).ToString("#,####" + Digits));
    }

    /// <summary>
    /// DataRow转换为DataTable
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="strWhere">筛选的条件</param>
    /// <returns></returns>
    public static DataTable SreeenDataTable(DataTable dt, string strWhere)
    {
        if (dt.Rows.Count <= 0) return dt;        //当数据为空时返回
        DataTable dtNew = dt.Clone();         //复制数据源的表结构
        DataRow[] dr = dt.Select(strWhere);  //strWhere条件筛选出需要的数据！
        for (int i = 0; i < dr.Length; i++)
        {
            dtNew.Rows.Add(dr[i].ItemArray);  // 将DataRow添加到DataTable中
        }
        return dtNew;
    }

    /// <summary>
    /// 加载发货单选项
    /// </summary>
    /// <param name="lo">发货单表</param>
    /// <param name="l">订单明细表</param>
    /// <param name="lod">订单发货明细表</param>
    /// <param name="type">1 、厂商 0、代理商</param>
    public static string outbind(DataTable lo, DataTable l, List<Hi.Model.DIS_OrderOutDetail> lod, string Digits, int type)
    {
        bool isSign = false;
        StringBuilder str = new StringBuilder();
        //发货主表信息
        foreach (DataRow item in lo.Rows)
        {
            //Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", item["CompID"].ToString().ToInt(0));
            //判断发货单是否作废
            string cancel = "";
            if (item["IsAudit"].ToString().ToInt(0) == 3)
                cancel = " <div class=\"cancel\"></div>";

            str.AppendFormat("<div class=\"tabLine\">{0}<div class=\"deli-if\">", cancel);
            str.AppendFormat("<i class=\"t\">发货单号：<b class=\"gray\">{0}</b></i>", item["ReceiptNo"]);
            str.AppendFormat("<i class=\"t\">发货日期：<b class=\"gray\">{0}</b></i>", item["SendDate"].ToString() != "" ? item["SendDate"].ToString().ToDateTime().ToString("yyyy-MM-dd") : "");

            if (type == 1)
            {
                //厂商
                //发货物流信息
                string strlog = "";
                if (item["ComPName"].ToString() != "" && item["LogisticsNo"].ToString() != "")
                {
                    strlog = string.Format("<b class=\"gray\" tiplog=\"{2}\" tipl=\"{3}\">物流公司：{0}，物流单号：{1}<a href=\"javascript:;\" tip=\"{2}\" class=\"bule upLogistics\">修改物流</a></b>", item["ComPName"], item["LogisticsNo"], Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ID"]);
                }
                else if (item["CarUser"].ToString() != "" || item["CarNo"].ToString() != "")
                {
                    strlog = string.Format("<b class=\"gray\" tiplog=\"{3}\" tipl=\"{4}\">姓名：{0} {1}，车牌号：{2}<a href=\"javascript:;\" tip=\"{3}\" class=\"bule upLogistics\">修改物流</a></b>", item["CarUser"], item["CarNo"], item["Car"], Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ID"]);
                }
                else
                {
                    if (item["IsAudit"].ToString().ToInt(0) != 3)
                        strlog = string.Format("<b class=\"gray\" tiplog=\"{0}\" tipl=\"{1}\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule upLogistics\">修改物流</a></b>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ID"]);
                }

                str.AppendFormat("<i class=\"t\">物流信息：{0}<a href=\"javascript:;\" tip=\"{1}\" class=\"bule Logistics\">查看物流</a></i>", strlog, Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey));

                if (item["IsAudit"].ToString().ToInt(0) == 3)
                    str.Append("<div class=\"btn\"></div>");
                else
                {
                    //判断是否已签收
                    if (item["IsSign"].ToString().ToInt(0) == 1)
                        //str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a><i>|</i><a href=\"javascript:;\" tip=\"{0}\" class=\"bule btnorderoutdel\">作废</a></div>", item["ID"]);
                        str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a></div>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey));
                    else
                        //str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a><i>|</i><a href=\"javascript:;\" tip=\"{0}\" dts=\"{1}\" class=\"bule btnupout\">修改</a><i>|</i><a href=\"javascript:;\" tip=\"{0}\" dts=\"{1}\" class=\"bule btnorderoutdel\">作废</a></div>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ts"]);
                        str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a><i>|</i><a href=\"javascript:;\" tip=\"{0}\" dts=\"{1}\" class=\"bule btnorderoutdel\">作废</a></div>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ts"]);
                }
            }
            else if (type == 0)
            {
                //代理商
                //发货物流信息
                string strlog = "";
                if (item["ComPName"].ToString() != "" && item["LogisticsNo"].ToString() != "")
                {
                    strlog = string.Format("<b class=\"gray\" tiplog=\"{2}\" tipl=\"{3}\">物流公司：{0}，物流单号：{1}</b>", item["ComPName"], item["LogisticsNo"], Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ID"]);
                }
                else if (item["CarUser"].ToString() != "" || item["CarNo"].ToString() != "")
                {
                    strlog = string.Format("<b class=\"gray\" tiplog=\"{3}\" tipl=\"{4}\">姓名：{0} {1}，车牌号：{2}</b>", item["CarUser"], item["CarNo"], item["Car"], Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ID"]);
                }

                str.AppendFormat("<i class=\"t\">物流信息：{0}<a href=\"javascript:;\" tip=\"{1}\" class=\"bule Logistics\">查看物流</a></i>", strlog, Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey));

                //判断是否已签收
                if (item["IsSign"].ToString().ToInt(0) == 1)
                    //str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a><i>|</i><a href=\"javascript:;\" tip=\"{0}\" class=\"bule btnorderoutdel\">作废</a></div>", item["ID"]);
                    str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a></div>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey));
                else
                {
                    if (item["IsAudit"].ToString().ToInt(0) == 3)
                        str.Append("<div class=\"btn\"></div>");
                    else
                    {
                        str.AppendFormat("<div class=\"btn\"><a href=\"javascript:;\" tip=\"{0}\" class=\"bule print\">打印</a><i class=\"btnsigni\">|</i><a href=\"javascript:;\" tip=\"{0}\" dts=\"{1}\" class=\"bule btnsign\">确认收货</a></div>", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey), item["ts"]);
                        isSign = true;
                    }
                }
            }

            str.Append("</div>");

            str.AppendFormat("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" tip=\"tab_{0}\">", Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey));
            str.Append("<thead><tr><th class=\"\">商品名称</th><th class=\"t1\">规格属性</th><th class=\"t5\">单位</th><th class=\"t3\">订购数量</th><th class=\"t3\">已发货数量</th><th class=\"t3\">本次发货</th><th class=\"t3\">批次号</th><th class=\"t3\">有效期</th></tr></thead><tbody>");


            //发货明细表
            List<Hi.Model.DIS_OrderOutDetail> lood = lod.FindAll(o => o.OrderOutID == item["ID"].ToString().ToInt(0));

            if (lood != null && lood.Count > 0)
            {
                foreach (Hi.Model.DIS_OrderOutDetail oouitem in lood)
                {
                    //发货明细对应的订单明细(商品信息)
                    DataRow[] od = l.Select(string.Format("GoodsinfoID ={0} and OrderID ={1}", oouitem.GoodsinfoID, oouitem.OrderID));
                    if (od.Length > 0)
                    {
                        str.AppendFormat("<tr ttrd=\"{0}\">", oouitem.ID);

                        str.AppendFormat("<td><div class=\"sPic\"><span><a target=\"_blank\" href=\"../../e" + od[0]["GoodsInfoID"] + "_" + item["CompID"] + "_.html\"><img src=\"{0}\" width=\"60\" height=\"60\"></a></span> <a target=\"_blank\" href=\"../../e" + od[0]["GoodsInfoID"] + "_" + item["CompID"] + "_.html\" class=\"code\">商品编码：{1}</a> <a  target=\"_blank\" href=\"../../e" + od[0]["GoodsInfoID"] + "_" + item["CompID"] + "_.html\" class=\"name\">{2}<i>{3}</i></a></div></td>", SelectGoodsInfo.GetGoodsPic(Convert.ToString(od[0]["Pic"])), od[0]["GoodsCode"], Common.MySubstring(od[0]["GoodsName"].ToString(), 20, "..."), od[0]["GoodsName"]);

                        str.AppendFormat("<td><div class=\"tc\">{0}</div></td>", SelectGoodsInfo.GetGoodsInfos(Convert.ToString(od[0]["GoodsInfos"])));
                        str.AppendFormat("<td><div class=\"tc\">{0}</div></td>", od[0]["Unit"]);

                        string pro = od[0]["ProNum"].ToString().ToDecimal(0) == 0 ? "" : " 赠(" + SelectGoodsInfo.GetNum(od[0]["ProNum"].ToString(), Digits) + ")";

                        str.AppendFormat("<td><div class=\"tc\">{0}</div></td>", SelectGoodsInfo.GetNum(od[0]["GoodsNum"].ToString(), Digits) + pro);
                        str.AppendFormat("<td><div class=\"tc\">{0}</div></td>", SelectGoodsInfo.GetNum(od[0]["OutNum"].ToString(), Digits));
                        str.AppendFormat("<td style=\" width:15%;\"><div class=\"tc\">{0}</div><div class=\"sl\" style=\"display:none;\"><input type=\"hidden\" id=\"Notshipnum\" class=\"Notshipnum\" value=\"{1}\" /><input type=\"hidden\" id=\"outnum\" class=\"outnum\" value=\"{0}\" /><a href=\"javascript:;\" class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum\" onchange=\"outOrderNum(this,1);\" onkeyup='KeyInt2(this)' value=\"{0}\"><a href=\"javascript:;\" class=\"add\">+</a></div></td>", SelectGoodsInfo.GetNum(oouitem.OutNum.ToString(), Digits), ((od[0]["GoodsNum"].ToString().ToDecimal(0) + od[0]["ProNum"].ToString().ToDecimal(0)) - od[0]["OutNum"].ToString().ToDecimal(0) + oouitem.OutNum));

                        if (isSign)
                        {
                            str.AppendFormat("<td><div class=\"tc\"><label  style=\"display:none;\">{0}</label><input type=\"text\" class=\"box BatchNO\" readonly=\"readonly\" value=\"{0}\"/></div></td>", oouitem.Batchno);
                            str.AppendFormat("<td><div class=\"tc\"><label style=\"display:none;\">{0}</label><input type=\"text\" class=\"Wdate validDate\" readonly=\"readonly\" onclick=\"WdatePicker()\" value=\"{0}\" /></div></td>", oouitem.Validdate == DateTime.MinValue ? "" : oouitem.Validdate.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            str.AppendFormat("<td><div class=\"tc\"><label>{0}</label><input type=\"text\" style=\"display:none;\" class=\"box BatchNO\" readonly=\"readonly\" value=\"{0}\"/></div></td>", oouitem.Batchno);
                            str.AppendFormat("<td><div class=\"tc\"><label>{0}</label><input type=\"text\" style=\"display:none;\" class=\"Wdate validDate\" readonly=\"readonly\" onclick=\"WdatePicker()\" value=\"{0}\" /></div></td>", oouitem.Validdate == DateTime.MinValue ? "" : oouitem.Validdate.ToString("yyyy-MM-dd"));
                        }
                        str.Append("</tr>");
                    }
                }
            }
            str.Append("</tbody></table>");
            str.Append("</div><div class=\"blank20\"></div>");
        }
        return str.ToString();
    }

    /// <summary>
    /// 获取商品批次号、有效期
    /// </summary>
    /// <param name="compID"></param>
    /// <param name="goodsinfoid"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static string GetBv(string CompID, string goodsinfoid, string col)
    {
        //List<Hi.Model.BD_GoodsInfo> infolist = new Hi.BLL.BD_GoodsInfo().GetList("", " isnull(IsEnabled,0)=1 and isnull(IsOffline,0)=1 and isnull(dr,0)=0 and CompID=" + CompID + " and id=" + goodsinfoid, "");

        List<Hi.Model.DIS_GoodsStock> stocklist = new Hi.BLL.DIS_GoodsStock().GetList("", " CompID=" + CompID + " and GoodsInfo=" + goodsinfoid, "CreateDate asc");

        if (stocklist != null && stocklist.Count > 0)
        {
            if (col == "BatchNO")
            {
                return stocklist[0].BatchNO;
            }
            else if (col == "validDate")
            {
                return stocklist[0].validDate == DateTime.MinValue ? "" : stocklist[0].validDate.ToString("yyyy-MM-dd");
            }
        }
        return "";
    }

    /// <summary>
    /// 代理商调价选商品
    /// </summary>
    /// <returns></returns>
    public static string sql(string compid, string where)
    {
        string sql = string.Format(@"select b.ID as goodsinfoid,GoodsName, BarCode,ValueInfo,Unit,TinkerPrice,b.SalePrice,a.Pic 
from BD_Goods as a ,BD_GoodsInfo as b 
where a.ID=b.GoodsID and ISNULL(a.dr,0)=0
 and a.CompID={0} and b.CompID={0}
 and b.IsOffline=1 {1} order by b.id desc", compid, where);
        return sql;
    }

    /// <summary>
    /// 发货批次号库存判断
    /// </summary>
    /// <param name="GoodsInfoID">商品ID</param>
    /// <param name="BatchNO">批次号</param>
    /// <param name="outnum">发货数量</param>
    /// <returns> false 可以发货 true  商品库存不足，不能发货 </returns>
    public static bool GetIsStock(string GoodsInfoID, string BatchNO, decimal outnum, out decimal StockNum)
    {
        bool falg = false;
        StockNum = 0;
        List<Hi.Model.DIS_GoodsStock> stocklistAll = new Hi.BLL.DIS_GoodsStock().GetList("", " GoodsInfo=" + GoodsInfoID, "");

        if (stocklistAll != null && stocklistAll.Count > 0)
        {
            if (!string.IsNullOrWhiteSpace(BatchNO))
            {
                //批次号不为空时,该批次商品库存数量
                List<Hi.Model.DIS_GoodsStock> stocklist = stocklistAll.FindAll(p => p.BatchNO == BatchNO);
                if (stocklist != null && stocklist.Count > 0)
                {
                    //该商品批次库存数量小于发货数量
                    if (stocklist[0].StockNum < outnum)
                    {
                        StockNum = stocklist[0].StockNum;
                        falg = true;
                    }
                }
                else
                {
                    //该批次没的库存数量，发货不能成功
                    StockNum = 0;
                    falg = true;
                }
            }
            else
            {
                //批次号为空
                List<Hi.Model.DIS_GoodsStock> stocklist = stocklistAll.FindAll(p => p.BatchNO == "");
                if (stocklist != null && stocklist.Count > 0)
                {
                    if (stocklist[0].StockNum < outnum)
                    {
                        StockNum = stocklist[0].StockNum;
                        falg = true;
                    }
                }
                else
                {
                    //该批次没的库存数量，发货不能成功
                    StockNum = 0;
                    falg = true;
                }
            }
        }
        return falg;
    }

}