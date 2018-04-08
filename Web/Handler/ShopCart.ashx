<%@ WebHandler Language="C#" Class="ShopCart" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Collections.Generic;

public class ShopCart : IRequiresSessionState, IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

         string ActionType = context.Request["ActionType"];
         switch (ActionType)
         {
             case "CartToOrder"://购物车转订单明细 --del
                 context.Response.Write(CartTopOrder(context));
                 context.Response.End();
                 break;
             case "AddShopCart"://添加商品到购物车
                 context.Response.Write(Cart(context));
                 context.Response.End();
                 break;
             case "CartNote"://修改单个商品备注 --del
                 context.Response.Write(AddNote(context));
                 context.Response.End();
                 break;
             case "AddOrderNote"://修改订单备注 --del
                 context.Response.Write(AddOrderNote(context));
                 context.Response.End();
                 break;
             case "AddAddr"://新增收货地址
                 context.Response.Write(AddAddr(context));
                 context.Response.End();
                 break;
             case "UpAddr"://修改收货地址
                 context.Response.Write(UpAddr(context));
                 context.Response.End();
                 break;
             case "DelShopCart"://删除订单结算页商品
                 context.Response.Write(DelShopCart(context));
                 context.Response.End();
                 break;
             case "AddNoteOrderCart"://购物车结算页面添加商品备注
                 context.Response.Write(AddNoteOrderCart(context));
                 context.Response.End();
                 break;
             case "AddDcGoods"://添加购物车商品到收藏
                 context.Response.Write(AddDcGoods(context));
                 context.Response.End();
                 break;
             case "CalDcGoods"://添加购物车商品到收藏
                 context.Response.Write(CalDcGoods(context));
                 context.Response.End();
                 break;
             case "deladdr": //删除地址
                 context.Response.Write(delAddr(context));
                 context.Response.End();
                 break;
             case "isdef": //设置默认收货信息
                 context.Response.Write(isdefAddr(context));
                 context.Response.End();
                 break;
         }
    }

    /************************************* 购物车 region ******************************************************/
    /// <summary>
    /// 添加商品到购物车
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Cart(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        try
        {
            Hi.BLL.DIS_ShopCart shopbll = new Hi.BLL.DIS_ShopCart();
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                //是否添加商品 1、添加到购物车，2、减商品数量 3、加商品数量  4、删除购物车商品
                string CartType = context.Request["CartType"] + "";

                if (CartType == "4")
                {
                    // 0、删除单个商品  1、清空购物车
                    string delType = context.Request["deltype"] + "";

                    if (delType == "0")
                    {
                        //删除购物车商品信息
                        string cartID = context.Request["cartID"] + "";
                        //删除单个商品
                        if (shopbll.Delete(cartID.ToInt(0)))
                        {
                            //查询购物车商品数量、总价
                            DataTable dt = shopbll.SumCartNum(logUser.CompID.ToString(), logUser.DisID.ToString());
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                ////删除单个商品成功
                                if (context.Session["GoodsCart"] != null)
                                {
                                    DataTable OrderDt = context.Session["GoodsCart"] as DataTable;
                                    if (OrderDt.Rows.Count > 0)
                                    {
                                        DataRow[] dr = OrderDt.Select(" CartID=" + cartID);
                                        OrderDt.Rows.Remove(dr[0]);
                                    }
                                    context.Session["GoodsCart"] = OrderDt;
                                }

                                msg.SumCart = dt.Rows[0]["cart"].ToString().ToDecimal(0).ToString("0");
                                msg.SumAmount = dt.Rows[0]["SumAmount"].ToString().ToDecimal(0);
                            }
                            else
                            {
                                //清除DataTable数据
                                Clear(context);
                                if (HttpContext.Current.Session["GoodsCart"] != null)
                                {
                                    HttpContext.Current.Session["GoodsCart"] = null;
                                    HttpContext.Current.Session.Remove("GoodsCart");
                                }
                            }
                            msg.Result = true;
                            msg.Msg = "删除购物车商品成功！";
                        }
                        else
                        {
                            //删除单个商品失败
                            msg.Result = false;
                            msg.Msg = "删除购物车商品失败！";
                        }
                    }
                    else
                    {
                        //清空购物车
                        string str = " CompID=" + logUser.CompID + " and DisID=" + logUser.DisID;
                        if (shopbll.CartEmpty(str))
                        {
                            //清除DataTable数据
                            Clear(context);
                            msg.Result = true;
                            msg.Msg = "清空购物车商品成功！";
                        }
                        else
                        {
                            msg.Result = false;
                            msg.Msg = "清空购物车商品失败！";
                        }
                    }
                }
                else
                {
                    //商品信息ID
                    msg.IsRegi = true;
                    string goodsinfoid = context.Request["GoodsInfoID"] + "";
                    string Goodsid = context.Request["Goodsid"] + "";
                    List<Hi.Model.BD_Goods> LGoods = new Hi.BLL.BD_Goods().GetList(" top 1 *", "  id='" + Goodsid + "' and isnull(dr,0)=0  ", "");
                    if (LGoods.Count > 0)
                    {
                        //if (logUser.DisID == 0 || (logUser.TypeID != 1 && logUser.TypeID != 5) || (LGoods[0].CompID != logUser.CompID))
                        //{

                        //    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,IsAudit", " isnull(dr,0)=0 and ctype=2 and Compid=" + LGoods[0].CompID + " and Userid=" + logUser.UserID + " ", "");
                        //    msg.Result = false;
                        //    if (ListCompUser.Count > 0)
                        //    {
                        //        if (ListCompUser[0].IsAudit == 0)
                        //        {
                        //            msg.Msg = "请等待审核通过后，切换至该企业的代理商后操作。";
                        //        }
                        //        else
                        //        {
                        //            msg.Msg = "请切换至该企业代理商的后操作。";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        msg.Msg = "当前用户不是该企业代理商，请加盟后操作！";
                        //    }
                        //    msg.Result = false;
                        //    return (new JavaScriptSerializer().Serialize(msg));
                        //}
                    }


                    //促销ID
                    string ProID = context.Request["ProID"] + "";
                    //商品数量
                    string Num =Common.NoHTML( context.Request["Num"]) + "";

                    Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(goodsinfoid.ToInt(0));
                    if (goodsinfoModel == null)
                    {
                        msg.Result = false;
                        msg.Msg = "该属性商品已下架，无法加入购物车，<br/>如果该商品有其它属性，请重新选择。";
                        return (new JavaScriptSerializer().Serialize(msg));
                    }
                    int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", logUser.CompID).ToInt(0);
                    if (IsInve == 0)
                    {
                        if (Num.ToDecimal() > goodsinfoModel.Inventory)
                        {
                            msg.Result = false;
                            msg.Msg = "订货数量不能大于商品库存数量！";
                            msg.Num = goodsinfoModel.Inventory;
                            return (new JavaScriptSerializer().Serialize(msg));
                        }
                    }
                    else
                    {
                        if (Num.ToDecimal() >= 1000000)
                        {
                            msg.Result = false;
                            msg.Msg = "加入购物车的商品数量不能大于100万！";
                            return (new JavaScriptSerializer().Serialize(msg));
                        }
                    }


                    //商品基本价格
                    string Price = "0";
                    //商品最终销售价格
                    string TPrice = "0";
                    //if (CartType == "1")
                    //{
                    Price = context.Request["Price"] + "";
                    TPrice = context.Request["TPrice"] + "";
                    //////////////////////////////////验证商品，商品价格 region//////////////////////////////////
                    if (goodsinfoModel != null)
                    {
                        int count = 0;
                        List<Common.GoodsID> gl = Common.DisEnAreaGoodsID(logUser.DisID.ToString(), logUser.CompID.ToString());
                        if (gl != null && gl.Count > 0)
                            count = gl.FindAll(p => p.goodsID == goodsinfoModel.GoodsID.ToString()).Count;
                        if (count > 0)
                        {
                            msg.Result = false;
                            msg.Msg = "你不在该商品可售区域，不能购买！";
                            return (new JavaScriptSerializer().Serialize(msg));
                        }

                        if (goodsinfoModel.CompID != logUser.CompID)
                        {
                            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,IsAudit", " isnull(dr,0)=0 and ctype=2 and Compid=" + goodsinfoModel.CompID + " and Userid=" + logUser.UserID + " ", "");
                            msg.Result = false;
                            if (ListCompUser.Count > 0)
                            {
                                if (ListCompUser[0].IsAudit == 0)
                                {
                                    msg.Msg = "请等待审核通过后，切换至该企业的代理商后操作。";
                                }
                                else
                                {
                                    msg.Msg = "请切换至该企业的代理商后操作。";
                                }
                            }
                            else
                            {
                                msg.Msg = "未加盟该企业，无法加入购物车！";
                            }
                            return (new JavaScriptSerializer().Serialize(msg));
                        }
                        else if (goodsinfoModel.IsOffline == 0)
                        {
                            msg.Result = false;
                            msg.Msg = "商品已下架！";
                            return (new JavaScriptSerializer().Serialize(msg));
                        }
                        else if (goodsinfoModel.dr == 1)
                        {
                            msg.Result = false;
                            msg.Msg = "商品已删除！";
                            return (new JavaScriptSerializer().Serialize(msg));
                        }

                        Price = goodsinfoModel.TinkerPrice.ToString();
                        TPrice = BLL.Common.GetGoodsPrice(logUser.CompID, logUser.DisID, goodsinfoModel.ID).ToString();
                        msg.TPrice = TPrice.ToDecimal(0);
                    }
                    else
                    {
                        msg.Result = false;
                        msg.IsRegi = false;
                        msg.Msg = "商品不存在,无法加入购物车！";
                        return (new JavaScriptSerializer().Serialize(msg));
                    }
                    //////////////////////////////////验证商品，商品价格 end//////////////////////////////////
                    //}

                    if (shopbll.Cart(logUser.UserID, logUser.CompID, logUser.DisID, goodsinfoid, ProID, Num, Price, TPrice, CartType) > 0)
                    {
                        //查询购物车商品数量、总价
                        DataTable dt = shopbll.SumCartNum(logUser.CompID.ToString(), logUser.DisID.ToString());
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            msg.SumCart = dt.Rows[0]["cart"].ToString().ToDecimal(0).ToString("0");
                            msg.SumAmount = dt.Rows[0]["SumAmount"].ToString().ToDecimal(0);
                            msg.AuditAmount = shopbll.GetAuditAmount(logUser.CompID.ToString(), logUser.DisID.ToString(), goodsinfoid.ToString());
                        }
                        msg.Result = true;
                        msg.Code = "";
                        msg.Msg = "添加到购物车成功！";
                    }
                    else
                    {
                        msg.Msg = "添加到购物车失败！";
                    }
                }
            }
            else
            {
                msg.Msg = "用户未登陆,请登录后操作。";
                msg.Code = "Login";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    
    /// <summary>
    /// 删除订单结算页商品
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string DelShopCart(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        if (context.Session["UserModel"] is LoginModel)
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            try
            {
                Hi.BLL.DIS_ShopCart shopbll = new Hi.BLL.DIS_ShopCart();
                //删除购物车商品信息
                string cartID = context.Request["cartID"] + "";

                //删除单个商品
                if (shopbll.Delete(cartID.ToInt(0)))
                {
                    //删除单个商品成功

                    //删除下单的商品
                    if (context.Session["GoodsCart"] != null)
                    {
                        DataTable OrderDt = context.Session["GoodsCart"] as DataTable;
                        if (OrderDt.Rows.Count > 0)
                        {
                            DataRow[] dr = OrderDt.Select(" CartID=" + cartID);
                            OrderDt.Rows.Remove(dr[0]);
                        }
                        context.Session["GoodsCart"] = OrderDt;
                    }

                    //查询购物车商品数量、总价
                    DataTable dt = shopbll.SumCartNum(logUser.CompID.ToString(), logUser.DisID.ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        decimal TotalAmount = 0;
                        string ProID = "";
                        string ProIDD = "";
                        string ProType = "";

                        msg.SumCart = dt.Rows[0]["cart"].ToString().ToDecimal(0).ToString("0");
                        //订单总价
                        msg.AuditAmount = TotalAmount = dt.Rows[0]["SumAmount"].ToString().ToDecimal(0);

                        //订单促销
                        msg.ProAmount = Common.GetProPrice(TotalAmount, out ProID, out ProIDD, out ProType, logUser.CompID);
                        msg.Code = OrderInfoType.proOrderType(ProIDD.ToString(), msg.ProAmount.ToString(), ProType); ;
                        //合计
                        msg.SumAmount = (TotalAmount - msg.ProAmount);

                    }

                    msg.Result = true;
                    msg.Msg = "删除购物车商品成功！";
                }
                else
                {
                    //删除单个商品失败
                    msg.Result = false;
                    msg.Msg = "删除购物车商品失败！";
                }
            }
            catch (Exception ex)
            {
                msg.Msg = ex.Message;
            }
        }
        else
        {
            msg.Msg = "用户未登陆,请登录后操作。";
            msg.Code = "Login";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    
    /// <summary>
    /// 购物车转订单明细
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string CartTopOrder(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        
        
        Hi.BLL.DIS_ShopCart shopbll=new Hi.BLL.DIS_ShopCart();
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        try
        {
            if (logUser != null)
            {
                Clear(context);
                DataTable Cartdt = shopbll.GetGoodsCart(" sc.[CompID]=" + logUser.CompID + " and sc.[DisID]=" + logUser.DisID + "and sc.dr=0", "sc.[CreateDate] desc ");

                DataTable dt = null;
                string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", logUser.CompID);

                if (Cartdt != null && Cartdt.Rows.Count > 0)
                {
                    foreach (DataRow item in Cartdt.Rows)
                    {
                        if (context.Session["GoodsCart"] == null)
                        {
                            dt = new DataTable();
                            dt.Columns.Add("Id", typeof(string));     //订单明细Id
                            dt.Columns.Add("CartId", typeof(string));     //购物车Id
                            dt.Columns.Add("DisId", typeof(string));     //代理商Id
                            dt.Columns.Add("CompId", typeof(string));     //企业Id
                            dt.Columns.Add("GoodsID", typeof(Int64)); //商品基本档案ID
                            dt.Columns.Add("GoodsinfoID", typeof(Int64)); //商品ID
                            dt.Columns.Add("GoodsCode", typeof(string));   //商品编号
                            dt.Columns.Add("GoodsName", typeof(string));    //商品名称
                            dt.Columns.Add("GoodsInfos", typeof(string));   //商品属性信息
                            dt.Columns.Add("Price", typeof(decimal)); //商品价格
                            dt.Columns.Add("AuditAmount", typeof(decimal)); //审核后价格
                            dt.Columns.Add("Unit", typeof(string)); //商品计量单位
                            dt.Columns.Add("Pic", typeof(string)); //商品图片
                            dt.Columns.Add("GoodsNum", typeof(decimal)); //商品数量
                            dt.Columns.Add("Remark", typeof(string)); //备注
                            dt.Columns.Add("vdef1", typeof(string)); //是否促销商品
                            dt.Columns.Add("vdef2", typeof(string)); //促销商品数量
                            dt.Columns.Add("vdef3", typeof(string)); //促销
                            dt.Columns.Add("ProType", typeof(string));//促销ProType
                            dt.Columns.Add("Total", typeof(decimal)); //小计

                            DataRow dr1 = dt.NewRow();

                            dr1["Id"] = 0;
                            dr1["CartId"] = item["ID"];
                            dr1["DisId"] = item["DisID"];
                            dr1["CompId"] = item["CompID"];
                            dr1["GoodsID"] = item["GoodsID"];
                            dr1["GoodsinfoID"] = item["GoodsinfoID"];
                            dr1["GoodsCode"] = item["GoodsCode"];
                            dr1["GoodsName"] = item["GoodsName"];
                            dr1["GoodsInfos"] = item["GoodsInfos"];
                            dr1["Price"] = item["Price"];
                            dr1["AuditAmount"] = item["AuditAmount"];
                            dr1["Unit"] = item["Unit"];
                            dr1["Pic"] = item["Pic"];
                            dr1["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item["GoodsNum"].ToString().ToDecimal(0).ToString("#,####" + Digits)));
                            dr1["Remark"] = "";
                            dr1["vdef1"] = item["ProID"];
                            dr1["vdef2"] = decimal.Parse(string.Format("{0:N4}", item["ProNum"].ToString().ToDecimal(0).ToString("#,####" + Digits)));
                            dr1["vdef3"] = "";
                            dr1["ProType"] = item["ProType"];
                            dr1["Total"] = item["sumAmount"].ToString().ToDecimal(0).ToString("N");

                            dt.Rows.Add(dr1);
                        }
                        else
                        {
                            dt = HttpContext.Current.Session["GoodsCart"] as DataTable;
                            DataRow dr2 = dt.NewRow();

                            dr2["Id"] = 0;
                            dr2["CartId"] = item["ID"];
                            dr2["DisId"] = item["DisID"];
                            dr2["CompId"] = item["CompID"];
                            dr2["GoodsID"] = item["GoodsID"];
                            dr2["GoodsinfoID"] = item["GoodsinfoID"];
                            dr2["GoodsCode"] = item["GoodsCode"];
                            dr2["GoodsName"] = item["GoodsName"];
                            dr2["GoodsInfos"] = item["GoodsInfos"];
                            dr2["Price"] = item["Price"];
                            dr2["AuditAmount"] = item["AuditAmount"];
                            dr2["Unit"] = item["Unit"];
                            dr2["Pic"] = item["Pic"];
                            dr2["GoodsNum"] = decimal.Parse(string.Format("{0:N4}", item["GoodsNum"].ToString().ToDecimal(0).ToString("#,####" + Digits)));
                            dr2["Remark"] = "";
                            dr2["vdef1"] = item["ProID"];
                            dr2["vdef2"] = decimal.Parse(string.Format("{0:N4}", item["ProNum"].ToString().ToDecimal(0).ToString("#,####" + Digits)));
                            dr2["vdef3"] = "";
                            dr2["ProType"] = item["ProType"];
                            dr2["Total"] = item["sumAmount"].ToString().ToDecimal(0).ToString("N");
                            dt.Rows.Add(dr2);
                        }
                        dt.DefaultView.Sort = "id desc";
                        context.Session["GoodsCart"] = dt;
                    }
                    msg.Result = true;
                }
                else
                {
                    msg.Msg = "购物车没有商品";
                }
            }
            else
            {
                msg.Msg = "用户未登陆,请登录后操作。";
                msg.Code = "Login";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = ex.Message;
            msg.Result = false;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    
    /************************************* 购物车 end ******************************************************/


    /************************************* 购物车结算页面 region ******************************************************/
    /// <summary>
    /// 购物车结算页面添加商品备注
    /// </summary>
    /// <returns></returns>
    public string AddNoteOrderCart(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        string cart_ID = context.Request["cart_ID"] + "";
        string Re =Common.NoHTML( context.Request["Re"] )+ "";

        DataTable dt = context.Session["GoodsCart"] as DataTable;

        DataRow[] dr = dt.Select(string.Format(" CartId=" + cart_ID));

        if (dr.Length > 0)
        {
            dr[0]["Remark"] = Re;
            msg.Result = true;
        }
        else
        {
            msg.Result = false;
        }

        return (new JavaScriptSerializer().Serialize(msg));
    }
    /************************************* 购物车结算页面 end ******************************************************/
    
    /************************************* 订单修改 region ******************************************************/
    /// <summary>
    /// 修改单个商品备注
    /// </summary>
    /// <returns></returns>
    public string AddNote(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        string cart_ID =Common.NoHTML(  context.Request["od_ID"]) + "";
        string Re =Common.NoHTML(  context.Request["Re"]) + "";

        try
        {
            string strWhat = "Remark=@Remark";
            string strWhere = "[ID]=@ID";
            SqlParameter[] parameters = {
                    new SqlParameter("@Remark", SqlDbType.VarChar),
                    new SqlParameter("@ID", SqlDbType.Int),
                };
            parameters[0].Value = Re;
            parameters[1].Value = cart_ID;

            if (new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(strWhat, parameters, strWhere))
                msg.Result = true;
            else
                msg.Result = false;
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    /// <summary>
    /// 修改订单备注
    /// </summary>
    /// <returns></returns>
    public string AddOrderNote(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        string type =Common.NoHTML(  context.Request.QueryString["type"]) + "";
        string ID = Common.NoHTML( context.Request.QueryString["ID"]) + "";
        string strWhat = "";
        string strWhere = "";
        SqlParameter[] parameters = null;
        try
        {
            if (type == "UpdateLog")
            {
                //修改订单备注
                string _logNote =Common.NoHTML(  context.Request["_logr"]) + "";
                //修改订单的列
                strWhat = "Remark=@Remark";
                //修改订单条件
                strWhere = "[ID]=@ID";
                SqlParameter[] parameters1 = {
                    new SqlParameter("@Remark", SqlDbType.VarChar),
                    new SqlParameter("@ID", SqlDbType.Int),
                };
                parameters1[0].Value = _logNote;
                parameters1[1].Value = ID;

                parameters = parameters1;
            }
            else if (type == "UpdateAddr")
            {
                //修改收货地址
                //修改订单备注
                string AddrID =Common.NoHTML(  context.Request["AddrID"]) + "";
                string principal =Common.NoHTML(  context.Request["principal"]) + "";
                string phone =Common.NoHTML(  context.Request["phone"]) + "";
                string Address =Common.NoHTML(  context.Request["Address"]) + "";
                //修改订单的列
                strWhat = "AddrID=@AddrID,principal=@principal,phone=@phone,Address=@Address";
                //修改订单条件
                strWhere = "[ID]=@ID";
                SqlParameter[] parameters1 = {
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@principal", SqlDbType.VarChar),
                    new SqlParameter("@phone", SqlDbType.VarChar),
                    new SqlParameter("@Address", SqlDbType.VarChar),
                    new SqlParameter("@ID", SqlDbType.Int),
                };
                parameters1[0].Value = AddrID;
                parameters1[1].Value = principal;
                parameters1[2].Value = phone;
                parameters1[3].Value = Address;
                parameters1[4].Value = ID;

                parameters = parameters1;
            }

            if (new Hi.BLL.DIS_Order().UpdateOrder(strWhat, parameters, strWhere))
                msg.Result = true;
            else
                msg.Result = false;
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }

        return (new JavaScriptSerializer().Serialize(msg));
    }
    /************************************* 订单修改 end ******************************************************/

    /************************************* 收货地址修改 region ******************************************************/
    /// <summary>
    /// 修改地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string UpAddr(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        string principal =Common.NoHTML(  context.Request["principal"]) + "";
        string phone =Common.NoHTML(  context.Request["phone"] )+ "";
        string Address =Common.NoHTML(  context.Request["Address"]) + "";
        string Province =Common.NoHTML(  context.Request["Province"]) + "";
        string City =Common.NoHTML(  context.Request["City"]) + "";
        string Area = Common.NoHTML( context.Request["Area"]) + "";
        string id = Common.NoHTML( context.Request["id"]) + "";

        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        try
        {
            if (logUser != null)
            {
                Hi.Model.BD_DisAddr disAddrmodel = new Hi.BLL.BD_DisAddr().GetModel(id.ToInt(0));

                if (disAddrmodel != null)
                {
                    disAddrmodel.Principal = principal;
                    disAddrmodel.Phone = phone;
                    disAddrmodel.Province = Province;
                    disAddrmodel.City = City;
                    disAddrmodel.Area = Area;
                    disAddrmodel.Address = Address;
                    disAddrmodel.ts = DateTime.Now;
                    disAddrmodel.modifyuser = logUser.UserID;
                    if (new Hi.BLL.BD_DisAddr().Update(disAddrmodel))
                    {
                        msg.Result = true;
                        msg.Msg = "修改地址成功";
                    }
                    else
                    {
                        msg.Result = false;
                        msg.Msg = "修改地址失败！";
                    }
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "没有修改的地址信息";
                }
            }
            else
            {
                msg.Msg = "用户未登陆,请登录后操作。";
                msg.Code = "Login";
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    /// <summary>
    /// 新增地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string AddAddr(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        string principal =Common.NoHTML( context.Request["principal"]) + "";
        string phone =Common.NoHTML(  context.Request["phone"]) + "";
        string Address =Common.NoHTML(  context.Request["Address"]) + "";
        string Province = Common.NoHTML( context.Request["Province"]) + "";
        string City = Common.NoHTML( context.Request["City"] )+ "";
        string Area = Common.NoHTML( context.Request["Area"]) + "";
        string DisID = Common.NoHTML( context.Request["DisID"]) + "";
        string isdefault = "0";

        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        try
        {
            if (logUser != null)
            {
                //判断是否存在收货地址
                DataTable dtAddr = new Hi.BLL.BD_DisAddr().GetList("ID", "BD_DisAddr", " DisID=" + DisID + " and dr=0 and Address='" + Address + "'", "");
                if (dtAddr != null && dtAddr.Rows.Count > 0)
                {
                    msg.Result = false;
                    msg.Msg = "收货地址已存在！";
                    return (new JavaScriptSerializer().Serialize(msg));
                }

                //判断代理商是否有默认地址
                DataTable dt = new Hi.BLL.BD_DisAddr().GetList("ID", "BD_DisAddr", " DisID=" + DisID + " and dr=0 and isdefault=1", "");
                if (dt != null && dt.Rows.Count <= 0)
                {
                    isdefault = "1";
                }
                Hi.Model.BD_DisAddr addrModel = new Hi.Model.BD_DisAddr();

                addrModel.Principal = principal;
                addrModel.Phone = phone;
                addrModel.Province = Province;
                addrModel.City = City;
                addrModel.Area = Area;
                addrModel.Address = Address;
                addrModel.IsDefault = isdefault.ToInt(0);
                addrModel.DisID = DisID.ToInt(0);
                addrModel.CreateUserID = logUser.UserID;
                addrModel.CreateDate = DateTime.Now;
                addrModel.ts = DateTime.Now;
                addrModel.modifyuser = logUser.UserID;

                int ID = new Hi.BLL.BD_DisAddr().Add(addrModel);
                if (ID > 0)
                {
                    msg.Result = true;
                    msg.Code = ID.ToString();
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "新增地址失败！";
                }
            }
            else
            {
                msg.Msg = "用户未登陆,请登录后操作。";
                msg.Code = "Login";
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    /// <summary>
    /// 删除地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string delAddr(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        try
        {
            if (logUser != null)
            {
                string id = context.Request["id"] + "";
                string DisID = context.Request["DisID"] + "";

                string str = " ID=" + id + " and DisID=" + DisID + " and Isnull(dr,0)=0";
                List<Hi.Model.BD_DisAddr> l = new Hi.BLL.BD_DisAddr().GetList("", str, "");

                if (l != null && l.Count > 0)
                {
                    if (new Hi.BLL.BD_DisAddr().Delete(id.ToInt(0)))
                        msg.Result = true;
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "收货信息不存在！";
                }
            }
            else
            {
                msg.Result = false;
                msg.Msg = "请登录后操作！";
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    /// <summary>
    /// 设置默认地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string isdefAddr(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        try
        {
            if (logUser != null)
            {
                string id = context.Request["id"] + "";
                string DisID = context.Request["DisID"] + "";
                
                string str = " DisID=" + DisID + " and Isnull(dr,0)=0";
                List<Hi.Model.BD_DisAddr> l = new Hi.BLL.BD_DisAddr().GetList("", str, "");

                if (l != null && l.Count > 0)
                {
                   
                    Hi.Model.BD_DisAddr isddr = l.Find(p => p.IsDefault == 1);

                    if (isddr != null)
                    {
                        isddr.IsDefault = 0;
                        new Hi.BLL.BD_DisAddr().Update(isddr);
                    }
                    
                    Hi.Model.BD_DisAddr ddr = l.Find(p => p.ID == id.ToInt(0));
                    if (ddr != null)
                    {
                        ddr.IsDefault = 1;
                        if (new Hi.BLL.BD_DisAddr().Update(ddr))
                        {
                            msg.Result = true;
                            msg.Msg = "修改默认收货信息成功。";
                        }
                    }
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "收货信息不存在！";
                }
            }
            else
            {
                msg.Result = false;
                msg.Msg = "请登录后操作！";
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    
    /************************************* 收货地址修改 end ******************************************************/
    
    /// <summary>
    /// 添加购物车商品到收藏
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string AddDcGoods(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var GoodsID = context.Request["GoodsId"] + "";

                Hi.Model.BD_DisCollect collect = new Hi.Model.BD_DisCollect();
                collect.CompID = logUser.CompID;
                collect.DisID = logUser.DisID;
                collect.DisUserID = logUser.UserID;
                collect.GoodsID = GoodsID.ToInt(0);
                collect.IsEnabled = 1;
                collect.CreateDate = DateTime.Now;
                collect.CreateUserID = logUser.UserID;
                collect.ts = DateTime.Now;
                if (new Hi.BLL.BD_DisCollect().Add(collect) > 0)
                {
                    msg.Result = true;
                }
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string CalDcGoods(HttpContext context)
    {
        ResultMsgCart msg = new ResultMsgCart();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var GoodsID = context.Request["GoodsId"] + "";
                List<Hi.Model.BD_DisCollect> list = new Hi.BLL.BD_DisCollect().GetList("",
               " disID='" + logUser.DisID + "' and compID='" + logUser.CompID + "' and goodsID='" + GoodsID + "'", "");
                if (list != null && list.Count > 0)
                {
                    if (new Hi.BLL.BD_DisCollect().delete(logUser.DisID, int.Parse(GoodsID)))
                    {
                        msg.Result = true;
                    }
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "商品没有被收藏";
                }
            }
        }
        catch (Exception ex)
        {
            msg.Result = false;
            msg.Msg = ex.Message;
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    
    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public void Clear(HttpContext context)
    {
        if (context.Session["GoodsCart"] != null)
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                //清空购物车
                string str = " CompID=" + logUser.CompID + " and DisID=" + logUser.DisID;
                new Hi.BLL.DIS_ShopCart().CartEmpty(str);

                //清除全部Session
                context.Session["GoodsCart"] = null;
                context.Session.Remove("GoodsCart");
            }
        }
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}


[Serializable]
public class ResultMsgCart
{
    public ResultMsgCart()
    {
        Result = false;
        Msg = "";
        Code = "";
        SumCart = "0";
        SumAmount = 0;
        AuditAmount = 0;
        ProAmount = 0;
        TPrice = 0;
        Num = 0;
        IsRegi = false;
    }
    public bool Result;
    public bool IsRegi;
    public string Msg;
    public string Code;
    public string SumCart;
    public decimal SumAmount;
    public decimal AuditAmount;
    public decimal Num;
    public decimal ProAmount;
    public decimal TPrice;
}