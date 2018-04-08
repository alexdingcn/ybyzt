using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Web.UI.WebControls;

/// <summary>
///OrderGoods 订单明细，选择商品信息
/// </summary>
public class SelectGoods
{
    static Hi.BLL.DIS_Order OrderBLL = new Hi.BLL.DIS_Order();  //订单表
    static Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();  //订单明细表
    static Hi.BLL.BD_GoodsInfo GoodsInfoBll = new Hi.BLL.BD_GoodsInfo();  //商品信息表
    static Hi.BLL.BD_Goods GoodsBll = new Hi.BLL.BD_Goods();  //商品基本表

	public SelectGoods()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获取商品名称
    /// </summary>
    /// <param name="Id">商品信息Id</param>
    /// <returns></returns>
    public static string GoodsName(int Id,string col)
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

        //object value = null;
        //object model = new Hi.BLL.BD_Goods().GetModel(ID);
        //if (model != null)
        //{
        //    foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
        //    {
        //        if (info.Name.ToLower() == col.ToLower())
        //        {
        //            return info.GetValue(model, null);
        //        }
        //    }
        //}
        //return value;
    }

    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public static void Clear(int DisId,int CompId)
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {
            DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

            DataRow [] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}'", DisId, CompId));

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
    /// 清除DataTable数据
    /// </summary>
    public static void Clear()
    {
        if (HttpContext.Current.Session["GoodsInfo"] != null)
        {

            HttpContext.Current.Session["GoodsInfo"] = null;
            HttpContext.Current.Session.Remove("GoodsInfo");
            //HttpContext.Current.Session.Abandon(); //清除全部Session
        }
    }
  

    /// <summary>
    /// 选择商品
    /// </summary>
    /// <param name="Id">商品Id</param>
    public static void Goods(int Id,int DisId,int CompId)
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
            if (goodsinfoModel.IsOffline == 1)
            {
                //判断是否存在促销活动 优先取促销活动价
                ProID=Common.GetPro(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString());
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

                dr1["Id"] = 0;
                dr1["DisId"] = DisId;
                dr1["CompId"] = CompId;
                dr1["GoodsID"] = goodsinfoModel.GoodsID;
                dr1["GoodsinfoID"] = Id;
                dr1["BarCode"] = goodsinfoModel.BarCode;
                dr1["GoodsName"] = GoodsName(goodsinfoModel.GoodsID, "GoodsName");
                dr1["GoodsInfos"] = goodsinfoModel.ValueInfo == "" ?  Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID),30) : goodsinfoModel.ValueInfo;
                dr1["Price"] = Price;
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
                dr2["GoodsInfos"] = goodsinfoModel.ValueInfo == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : goodsinfoModel.ValueInfo;
                dr2["Inventory"] = goodsinfoModel.Inventory;
                dr2["Price"] = Price;
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
    public static void OrderDetail(int Id, int DisId,int CompId)
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
                    //获取商品价格
                    //List<Hi.Model.BD_GoodsPrice> pl = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", "DisID=" + DisId + " and CompID=" + CompId + " and GoodsInfoID=" + item.GoodsinfoID + " and IsEnabled=1 and ISNULL(dr,0)=0", "");
                    //if (pl != null)
                    //{
                    //    if (pl.Count > 0)
                    //    {  //}
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
                        dt.Columns.Add("Price", typeof(decimal)); //商品价格
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
                        dr1["GoodsInfos"] = item.GoodsInfos == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : item.GoodsInfos;
                        dr1["Price"] = item.Price;
                        dr1["Inventory"] = goodsinfoModel.Inventory + item.GoodsNum;
                        dr1["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                        dr1["AuditAmount"] = item.AuditAmount;
                        dr1["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                        dr1["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item.GoodsNum.ToString("#,####" + Digits)));
                        dr1["Remark"] = item.Remark;
                        if (item.vdef1 != null)
                            dr1["vdef1"] = item.vdef1.ToString() == "" ? "0" : item.vdef1.ToString();
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
                        dr2["GoodsInfos"] = item.GoodsInfos == "" ? Util.GetSubString(Common.GetGoodsMemo(goodsinfoModel.GoodsID), 30) : item.GoodsInfos;
                        dr2["Price"] = item.Price;
                        dr2["Inventory"] = goodsinfoModel.Inventory + item.GoodsNum;
                        dr2["Pic"] = GoodsName(goodsinfoModel.GoodsID, "Pic");
                        dr2["AuditAmount"] = item.AuditAmount;
                        dr2["Unit"] = GoodsName(goodsinfoModel.GoodsID, "Unit");
                        dr2["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item.GoodsNum.ToString("#,####" + Digits)));
                        dr2["Remark"] = item.Remark;
                        if (item.vdef1 != null)
                            dr2["vdef1"] = item.vdef1.ToString() == "" ? "0" : item.vdef1.ToString();
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
    public static decimal SumTotal(int DisId, int CompId,string ProPrice)
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
    /// 商品基本总价
    /// </summary>
    /// <returns></returns>
    public static decimal SumPriceTotal(int DisId, int CompId)
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
                        sum += item["Price"].ToString().ToDecimal(0) * item["GoodsNum"].ToString().ToInt(0);
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
    /// 单个商品小计
    /// </summary>
    /// <param name="GoodsinfoID"></param>
    /// <param name="DisId"></param>
    /// <param name="CompId"></param>
    /// <returns></returns>
    public static decimal GoodsPrice(int GoodsinfoID, int DisId, int CompId)
    {
        try
        {
            decimal sum = 0;

            if (HttpContext.Current.Session["GoodsInfo"] != null)
            {
                DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;

                DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}' and GoodsinfoID='{2}'", DisId, CompId, GoodsinfoID));
                if (dr.Length > 0)
                {
                    foreach (var item in dr)
                    {
                        //sum = item["Price"].ToString().ToDecimal(0) * item["GoodsNum"].ToString().ToInt(0);
                        sum = item["Total"].ToString().ToDecimal(0);
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
    /// 获取商品最新价格
    /// </summary>
    /// <param name="GoodsinfoID"></param>
    /// <param name="DisId"></param>
    /// <param name="CompId"></param>
    /// <returns></returns>
    public static decimal GoodsNewPrice(int GoodsinfoID, int DisId, int CompId)
    {
        decimal TinkerPrice = 0;
        //获取商品信息
        Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodsinfoID);

        if (goodsinfoModel != null)
        {
            //判断商品是否上架
            if (goodsinfoModel.IsOffline != 1)
                return 0;

            //判断是否存在促销活动 优先取促销活动价
            if (Common.GetPro(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString()) == 0)
            {
                //判断是否启用代理商价格维护
                if (Common.IsDisPrice != "0")
                {
                    List<Hi.Model.BD_GoodsPrice> pl = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", "DisID=" + DisId + " and CompID=" + CompId + " and GoodsInfoID=" + GoodsinfoID + " and IsEnabled=1 and ISNULL(dr,0)=0", "");

                    if (pl != null && pl.Count > 0)
                    {

                        TinkerPrice = pl[0].TinkerPrice.ToString() == "" ? goodsinfoModel.TinkerPrice : pl[0].TinkerPrice; ;

                    }
                    else
                    {
                        TinkerPrice = goodsinfoModel.TinkerPrice;
                    }
                }
                else
                {
                    TinkerPrice = goodsinfoModel.TinkerPrice;
                }
            }
            else
            {
                TinkerPrice = Common.GetProPrice(goodsinfoModel.GoodsID.ToString(), goodsinfoModel.ID.ToString(), CompId.ToString());
            }
            //判断是不是赠品
            //if (GoodsName(goodsinfoModel.GoodsID, "IsSale") == "1")
            //{
            //    TinkerPrice = 0;
            //}
        }
        return TinkerPrice;
    }


    /// <summary>
    /// 查询商品使用的促销方式 by szj 2016-04-13
    /// </summary>
    /// <param name="proID">促销ID</param>
    public static string ProType(object proID)
    {
        string TheLabel = string.Empty;
        if (proID.ToString() != "")
        {
            DataTable dt = new Hi.BLL.BD_Promotion().ProType(proID.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                TheLabel = "<div class=\"sale-box\"><i class=\"sale\">促销</i><div class=\"sale-txt\"><i class=\"arrow\"></i>";
                if (dt.Rows[0]["Type"].ToString() == "0")
                {
                    TheLabel += "特价商品";
                }
                else if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    if (dt.Rows[0]["Protype"].ToString() == "3")
                        TheLabel += "满" + OrderInfoType.GetNum(dt.Rows[0]["DisCount"].ToString().ToDecimal(0)) + "件" + "，获赠商品（" + OrderInfoType.GetNum(dt.Rows[0]["GoodsPrice"].ToString().ToDecimal(0)) + "）个";
                    else
                        TheLabel += "在原订货价基础上已打（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "）%折";
                }
                TheLabel += " </div></div>";
            }
        }

        return TheLabel;
    }

    /// <summary>
    /// 商品信息 by szj 2016-04-13
    /// </summary>
    /// <param name="viewinfo"></param>
    /// <returns></returns>
    public static string Viewinfos(object viewinfo)
    {
        string TheLabel = "";
        if (!viewinfo.ToString().Equals(""))
        {
            string[] view = viewinfo.ToString().Split(new char[] { '；' });
            if (view.Length > 0)
            {
                for (int i = 0; i < view.Length; i++)
                {
                    TheLabel += " <li>" + view[i].Replace(":", "：") + "</li>";
                }
            }

            return TheLabel;
        }
        return "";
    }

    /// <summary>
    /// 显示商品价格 by szj 2016-04-15
    /// </summary>
    /// <param name="proID"></param>
    /// <param name="proNum"></param>
    /// <param name="Price"></param>
    /// <param name="AuditAmount"></param>
    /// <returns></returns>
    public static string GetPrice(object proID, object ProType, object Price, object AuditAmount)
    {
        string TheLabel = string.Empty;
        if (proID.ToString() == "0" && ProType.ToString().ToDecimal(0) == 0)
        {
            TheLabel = "￥" + AuditAmount.ToString().ToDecimal(0).ToString("N");
        }
        else if (proID.ToString() != "" && ProType.ToString().ToDecimal(0) != 3)
        {
            TheLabel = "<i class=\"del\">￥" + Price.ToString().ToDecimal().ToString("N") + "</i><i class=\"red\">￥" + AuditAmount.ToString().ToDecimal(0).ToString("N") + "</i>";
        }
        else
        {
            TheLabel = "￥" + AuditAmount.ToString().ToDecimal(0).ToString("N");
        }

        return TheLabel;
    }
}