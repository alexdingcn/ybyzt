<%@ WebHandler Language="C#" Class="GoodsInfoPrice" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data;

public class GoodsInfoPrice : IHttpHandler {

    public string action = string.Empty;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        if (context.Request["action"] != null)
        {
            action = context.Request["action"].ToString();
        }
        
        if (action == "attr")
        {
            int Id = context.Request["Id"].ToString().ToInt(0);
            string attr = context.Request["attrvalue"].ToString();
            string attr1 = context.Request["attrvalue1"].ToString();
            int DisID = context.Request["DisID"].ToString().ToInt(0);
            int CompID = context.Request["CompID"].ToString().ToInt(0);

            context.Response.Write(Attrval(Id, attr, attr1, DisID, CompID));
            context.Response.End();
        }
        if (action == "infoattr")
        {
            string infoid = context.Request["infoid"] + "";
            int DisID = context.Request["DisID"].ToInt(0);
            int CompID = context.Request["CompID"].ToInt(0);

            context.Response.Write(infoVal(infoid, DisID, CompID));
            context.Response.End();
        }

        if (action == "pro") {
            string proid = context.Request["proID"] + "";
            string pro_type = context.Request["pro_type"] + "";
            context.Response.Write(ProType(proid, pro_type));
            context.Response.End();
        }

        if (action == "Goodspro")
        {
            string GoodsID = context.Request["GoodsID"] + "";
            string CompId = context.Request["CompId"] + "";
            
            context.Response.Write(ProTypeGoodsID(GoodsID, CompId));
            context.Response.End();
        }
        //if (action == "Goodsprodis")
        //{
        //    string GoodsID = context.Request["GoodsID"] + "";
        //    string CompId = context.Request["CompId"] + "";

        //    context.Response.Write(disProTypeGoodsID(GoodsID, CompId));
        //    context.Response.End();
        //}
    }

    /// <summary>
    /// 根据商品属性 获取商品信息及价格
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public string Attrval(int Id, string attr, string attr1, int DisID, int CompID)
    {
        string Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + 0 + "\",\"BarCode\":\"" + 0 + "\",\"TinkerPrice\":\"" + 0 + "\"}";

        string strWhere = " GoodsID=" + Id + "and isnull(ValueInfo,'')='" + attr + "'and IsOffline=1 and CompID=" + CompID + "and IsEnabled=1 and isnull(dr,0)=0";
        string strWhere1 = " GoodsID=" + Id + "and isnull(ValueInfo,'')='" + attr1 + "'and IsOffline=1 and CompID=" + CompID + "and IsEnabled=1 and isnull(dr,0)=0";
        
        List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", strWhere, "");
        
        if (l.Count == 0)
            l = new Hi.BLL.BD_GoodsInfo().GetList("", strWhere1, "");
        
        if (l != null && l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsInfo item in l)
            {

                //判断是否存在促销活动 优先取促销活动价
                //if (Common.GetPro(item.GoodsID.ToString(), item.ID.ToString(), CompID.ToString()) == 0)
                //{
                //}
                decimal Price = BLL.Common.GetGoodsPrice(CompID, DisID, item.ID);
                string Pr = Convert.ToDecimal(Price.ToString()).ToString("#,##0.00");
                if (item.IsOffline == 0)
                    Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + 0 + "\",\"Inventory\":\"" + item.Inventory + "\",\"BarCode\":\"" + item.BarCode + "\",\"TinkerPrice\":\"" + Pr + "\"}";
                else if (!item.IsEnabled)
                    Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + 0 + "\",\"Inventory\":\"" + item.Inventory + "\",\"BarCode\":\"" + item.BarCode + "\",\"TinkerPrice\":\"" + Pr + "\"}";
                    else
                    Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + item.ID + "\",\"Inventory\":\"" + item.Inventory + "\",\"BarCode\":\"" + item.BarCode + "\",\"TinkerPrice\":\"" + Pr + "\"}";
            }
        }
        else
        {
            //获取商品基础价格
            Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(Id);
            
            Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(goodsModel.ViewInfoID.ToString().ToInt(0));
            if (goodsinfoModel.IsOffline == 0)
                Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + 0 + "\",\"Inventory\":\"" + goodsinfoModel.Inventory + "\",\"BarCode\":\"" + goodsinfoModel.BarCode + "\",\"TinkerPrice\":\"" + goodsinfoModel.TinkerPrice.ToString("0.00") + "\"}";
            else if (!goodsinfoModel.IsEnabled)
                Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + 0 + "\",\"Inventory\":\"" + goodsinfoModel.Inventory + "\",\"BarCode\":\"" + goodsinfoModel.BarCode + "\",\"TinkerPrice\":\"" + goodsinfoModel.TinkerPrice.ToString("0.00") + "\"}";
            else
            Josn = "{\"falg\":\"" + false + "\",\"goodsinfoid\":\"" + goodsinfoModel.ID + "\",\"Inventory\":\"" + goodsinfoModel.BarCode + "\",\"BarCode\":\"" + goodsinfoModel.BarCode + "\",\"TinkerPrice\":\"" + goodsinfoModel.TinkerPrice.ToString("0.00") + "\"}";
        }
        
        return Josn;
    }

    
    /// <summary>
    /// 根据GoodsInfo的Id查询商品信息
    /// </summary>
    /// <param name="infoid"></param>
    /// <param name="DisID"></param>
    /// <param name="CompID"></param>
    /// <returns></returns>
    public string infoVal(string infoid, int DisID, int CompID)
    {
        List<Hi.Model.BD_GoodsInfo> l = new List<Hi.Model.BD_GoodsInfo>();

        if (infoid != "")
        {
            string infostr = " ID in (" + infoid.Substring(0, infoid.Length - 1) + ")and IsOffline=1 and CompID=" + CompID + "and IsEnabled=1 and isnull(dr,0)=0";

            l = new Hi.BLL.BD_GoodsInfo().GetList("", infostr, "");
            
            //判断是否启用代理商价格维护
            if (Common.IsDisPrice != "0")
            {
                if (l != null && l.Count > 0)
                {

                    foreach (var item in l)
                    {
                        //判断是否存在促销活动 优先取促销活动价
                        if (Common.GetPro(item.GoodsID.ToString(), item.ID.ToString(), CompID.ToString()) == 0)
                        {
                            string strinfo = " DisID=" + DisID + " and CompID=" + CompID + " and GoodsInfoID=" + item.ID + " and IsEnabled=1 and ISNULL(dr,0)=0";

                            //获取代理商 商品价格
                            List<Hi.Model.BD_GoodsPrice> pricel = new Hi.BLL.BD_GoodsPrice().GetList("top 1 *", strinfo, "CreateDate desc");

                            if (pricel != null)
                            {
                                if (pricel.Count > 0)
                                {
                                    item.TinkerPrice = pricel[0].TinkerPrice;
                                }
                            }
                        }
                        else
                        {
                            //促销价
                            item.TinkerPrice = Common.GetProPrice(item.GoodsID.ToString(), item.ID.ToString(), CompID.ToString());
                        }
                    }
                }
            }
            else
            {
                if (l != null && l.Count > 0)
                {
                    foreach (var item in l)
                    {
                        //判断是否存在促销活动 优先取促销活动价
                        if (Common.GetPro(item.GoodsID.ToString(), item.ID.ToString(), CompID.ToString()) != 0)
                        {
                            //促销价
                            item.TinkerPrice = Common.GetProPrice(item.GoodsID.ToString(), item.ID.ToString(), CompID.ToString());
                        }
                    }
                }
            }
        }
       
        return Newtonsoft.Json.JsonConvert.SerializeObject(l);
    }

    /// <summary>
    /// 返回促销提示
    /// </summary>
    /// <param name="infoid"></param>
    /// <param name="DisID"></param>
    /// <param name="CompID"></param>
    /// <returns></returns>
    public string ProType(string ProID, string pro_type)
    {
        string Josn = "{\"falg\":\"" + false + "\",\"TheLabel\":\"\"}";
        string TheLabel = "";
        
        if (pro_type != "")
        {
            string[] pro = pro_type.Split(new char[] { ',' });
            if (pro.Length == 4)
            {
                if (pro[0].ToString() == "0")
                {
                    TheLabel = "<i class='gray'>特价商品</i>";
                }
                else if (pro[0].ToString() == "1")
                {
                    if (pro[1].ToString() == "3")
                    {
                        TheLabel = "<i class='gray'>订购数量每满（" + pro[3].ToString().ToDecimal(0).ToString("0") + "），获赠商品（" + pro[2].ToString().ToDecimal(0).ToString("0") + "）个</i>";
                    }
                    else
                    {
                        TheLabel = "<i class='gray'>在原订货价基础上已打折（" + pro[3].ToString().ToDecimal(0).ToString("0") + "）%</i>";
                    }
                }
                else if (pro[1].ToString() == "2")
                {
                    if (pro[1].ToString() == "5")
                    {
                        TheLabel = "<i class='gray'>订单金额满（" + pro[2].ToString().ToDecimal(0).ToString("0") + "），立减（" + pro[3].ToString().ToDecimal(0).ToString("0") + "）</i>";
                    }
                    else
                    {
                        TheLabel = "<i class='gray'>订单金额满（" + pro[2].ToString().ToDecimal(0).ToString("0") + "），打折（" + pro[3].ToString().ToDecimal(0).ToString("0") + "）%</i>";
                    }
                }
                Josn = "{\"falg\":\"" + true + "\",\"TheLabel\":\"" + TheLabel + "\"}";
            }
        }
        else if (ProID != "" && pro_type == "")
        {
            string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice,prod2.OrderPrice,prod2.Discount as ProDiscount from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID left join BD_PromotionDetail2 as prod2 on pro.ID=prod2.ProID  where pro.ID={0} order by pro.CreateDate desc", ProID);

            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Type"].ToString() == "0")
                {
                    TheLabel = "<i class='gray' style='text-align:left;'>特价商品</i>";
                }
                else if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    if (dt.Rows[0]["Protype"].ToString() == "3")
                    {
                        TheLabel = "<i class='gray'>订购数量每满（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "），获赠商品（" + Convert.ToInt32(dt.Rows[0]["GoodsPrice"]) + "）个</i>";
                    }
                    else
                    {
                        TheLabel = "<i class='gray'>在原订货价基础上已打折（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "）%</i>";
                    }
                }
                else if (dt.Rows[0]["Type"].ToString() == "2")
                {
                    if (dt.Rows[0]["Protype"].ToString() == "5")
                    {
                        TheLabel = "<i class='gray'>订单金额满（" + Convert.ToInt32(dt.Rows[0]["OrderPrice"]) + "），立减（" + Convert.ToInt32(dt.Rows[0]["ProDiscount"]) + "）</i>";
                    }
                    else
                    {
                        TheLabel = "<i class='gray'>订单金额满（" + Convert.ToInt32(dt.Rows[0]["OrderPrice"]) + "），打折（" + Convert.ToInt32(dt.Rows[0]["ProDiscount"]) + "）%</i>";
                    }
                }
                Josn = "{\"falg\":\"" + true + "\",\"TheLabel\":\"" + TheLabel + "\"}";
            }
        }

        return Josn;
    }


    /// <summary>
    /// 返回促销提示
    /// </summary>
    /// <param name="infoid"></param>
    /// <param name="DisID"></param>
    /// <param name="CompID"></param>
    /// <returns></returns>
    public string ProTypeGoodsID(string GoodsID, string CompId)
    {
        string Josn = "{\"falg\":\"" + false + "\",\"TheLabel\":\"\"}";
        if (GoodsID != "")
        {
            string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.[GoodsPrice],prod.GoodsID,prod.GoodInfoID from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} order by pro.CreateDate desc", GoodsID.ToString(), CompId);

            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                string TheLabel = "";
                if (dt.Rows[0]["Type"].ToString() == "0")
                {
                    TheLabel = "<i class='gray'>特价商品</i>";
                }
                else if (dt.Rows[0]["Type"].ToString() == "1")
                {
                    if (dt.Rows[0]["Protype"].ToString() == "3")
                    {
                        TheLabel = "<i class='gray'>订购数量每满（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "），获赠商品（" + Convert.ToInt32(dt.Rows[0]["GoodsPrice"]) + "）个</i>";
                    }
                    else
                    {
                        TheLabel = "<i class='gray'>在原订货价基础上已打折（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "）%</i>";
                    }
                }
                Josn = "{\"falg\":\"" + true + "\",\"TheLabel\":\"" + TheLabel + "\"}";
            }
        }

        return Josn;
    }


    /// <summary>
    /// 返回促销提示
    /// </summary>
    /// <param name="infoid"></param>
    /// <param name="DisID"></param>
    /// <param name="CompID"></param>
    /// <returns></returns>
    //public string disProTypeGoodsID(string GoodsID, string CompId)
    //{
    //    string Josn = "{\"falg\":\"" + false + "\",\"TheLabel\":\"\"}";
    //    if (GoodsID != "")
    //    {
    //        string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.[GoodsPrice],prod.GoodsID,prod.GoodInfoID from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} order by pro.CreateDate desc", GoodsID.ToString(), CompId);

    //        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            string TheLabel = "";
    //            if (dt.Rows[0]["Type"].ToString() == "0")
    //            {
    //                TheLabel = "<i class='gray' stlye='text-align:left;'>特价商品</i>";
    //            }
    //            else if (dt.Rows[0]["Type"].ToString() == "1")
    //            {
    //                if (dt.Rows[0]["Protype"].ToString() == "3")
    //                {
    //                    TheLabel = "<i class='gray'>订购数量每满（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "），获赠商品（" + Convert.ToInt32(dt.Rows[0]["GoodsPrice"]) + "）个</i>";
    //                }
    //                else
    //                {
    //                    TheLabel = "<i class='gray'>在原订货价基础上已打折（" + Convert.ToInt32(dt.Rows[0]["DisCount"]) + "）%</i>";
    //                }
    //            }
    //            Josn = "{\"falg\":\"" + true + "\",\"TheLabel\":\"" + TheLabel + "\"}";
    //        }
    //    }

    //    return Josn;
    //}
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}