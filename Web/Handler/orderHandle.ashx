<%@ WebHandler Language="C#" Class="orderHandle" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data;
using DBUtility;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class orderHandle : IRequiresSessionState, IHttpHandler
{
    //代理商Id
    public int DisId;
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";
    public int IsInve = 0;
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string ActionType = context.Request["ActionType"];
        switch (ActionType)
        {
            case "orderUpaddr": //订单修改收货信息
                context.Response.Write(orderUpaddr(context));
                context.Response.End();
                break;
            case "remarkview": //修改备注
                context.Response.Write(remarkview(context));
                context.Response.End();
                break;
            case "orderUpInvoi": //订单修改开票信息
                context.Response.Write(orderUpInvoi(context));
                context.Response.End();
                break;
            case "AddInvoi": //新增开票信息
                context.Response.Write(AddInvoi(context));
                context.Response.End();
                break;
            case "Sign": //到货签收
                context.Response.Write(SignOrder(context));
                context.Response.End();
                break;
            case "Buy"://再次购买
                context.Response.Write(BuyOrder(context));
                context.Response.End();
                break;
            case "Cancel"://作废订单
                context.Response.Write(CancelOrder(context));
                context.Response.End();
                break;
            case "amountof"://0、应付总额  1、修改运费
                context.Response.Write(Amountof(context));
                context.Response.End();
                break;
            case "orderAudit": // 订单审核 
                context.Response.Write(orderAudit(context));
                context.Response.End();
                break;
            case "outOrder": //发货
                context.Response.Write(outOrder(context));
                context.Response.End();
                break;
            case "upOut": //修改发货信息
                context.Response.Write(upOut(context));
                context.Response.End();
                break;
            case "CancelOut": //作废发货单
                context.Response.Write(CancelOut(context));
                context.Response.End();
                break;
            case "Payed"://线下支付
                context.Response.Write(Payed(context));
                context.Response.End();
                break;
            case "PayTovoid"://线下支付作废
                context.Response.Write(PayTovoid(context));
                context.Response.End();
                break;

            case "GoodsInfo":
                //修改商品信息 （价格，数量）
                context.Response.Write(GetDoogsPrice(context));
                context.Response.End();
                break;
            case "GoodsInfoList":
                //新增缓存 （价格，数量）
                context.Response.Write(GetGoodsInfoList(context));
                context.Response.End();
                break;
            case "Clear":

                //清除缓存 （价格，数量）
                // string goodsinfoId = Request["goodsInfoId"];  //商品信息Id
                context.Response.Write(SelectGoodsInfo.Clear());
                context.Response.End();
                break;
            case "DelGoods":
                int compId = Convert.ToInt32(context.Request["compId"]); //代理商
                int DisId = Convert.ToInt32(context.Request["disId"]); //代理商
                int goodsinfoId = Convert.ToInt32(context.Request["goodsInfoId"]);  //商品信息Id
                context.Response.Write(SelectGoodsInfo.DelGoods(goodsinfoId, DisId, compId));
                context.Response.End();
                break;
            case "getDisAdd":
                //根据代理商ID得到收货地址
                context.Response.Write(getDisAdder(context));
                context.Response.End();
                break;
            case "OrderDelete":
                //根据代理商ID得到收货地址
                context.Response.Write(orderdelete(context));
                context.Response.End();
                break;
            case "insertOrder":
                context.Response.Write(insertOrder(context));
                context.Response.End();
                break;
            case "EditStockInOut":
                context.Response.Write(EditStockInOut(context));
                context.Response.End();
                break;
            case "insertInventory":
                context.Response.Write(insertInventory(context));
                context.Response.End();
                break;
            case "ShengHe":
                context.Response.Write(ShengHe(context));
                context.Response.End();
                break;
            case "ShengHe1":
                context.Response.Write(ShengHe1(context));
                context.Response.End();
                break;
            case "DeleteOoder":
                context.Response.Write(DeleteOoder(context));
                context.Response.End();
                break;
            case "DelAll":
                context.Response.Write(DelAll(context));
                context.Response.End();
                break;
            case "DelAll2":
                context.Response.Write(DelAll2(context));
                context.Response.End();
                break;
            case "Select":
                context.Response.Write(Select(context));
                context.Response.End();
                break;
            //编辑合同
            case "ContractEdit":
                context.Response.Write(ContractEdit(context));
                context.Response.End();
                break;
            //编辑合同验证
            case "ContractCheck":
                context.Response.Write(ContractCheck(context));
                context.Response.End();
                break;
            //代理商入库单编辑
            case "disStorageEdit":
                context.Response.Write(disStorageEdit(context));
                context.Response.End();
                break;
            //代理商入库单审核
            case "auditStorage":
                context.Response.Write(auditStorage(context));
                context.Response.End();
                break;
            //代理商出库单编辑
            case "disLibraryEdit":
                context.Response.Write(disLibraryEdit(context));
                context.Response.End();
                break;
            //代理商出库单审核
            case "auditLibrary":
                context.Response.Write(auditLibrary(context));
                context.Response.End();
                break;
            //代理商编辑收款单
            case "disPaymentEdit":
                context.Response.Write(disPaymentEdit(context));
                context.Response.End();
                break;
            case "auditPayment":
                context.Response.Write(auditPayment(context));
                context.Response.End();
                break;
            case "GetCredit":
                context.Response.Write(GetCredit(context));
                context.Response.End();
                break;
        }
    }

    public string GetCredit(HttpContext context)
    {
        try
        {
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                int compID = (context.Request["CompID"] + "").ToInt(0);
                int KeyId=(context.Request["KeyId"] + "").ToInt(0);
                decimal CreditAmount = 0;
                if (BLL.Common.GetCredit(compID, logUser.DisID, out CreditAmount))
                {
                    decimal GetSumAmount = OrderInfoType.GetSumAmount(logUser.DisID.ToString(), compID.ToString(), KeyId);
                    if (GetSumAmount >= CreditAmount)
                    {
                        return "{\"rel\":\"OK\",\"Msg\":\"" + CreditAmount + "\"}";
                    }
                }
            }
            else
                return "{\"rel\":\"NO\",\"Msg\":\"登录异常！！！\"}";
        }
        catch (Exception e)
        {
            return "{\"rel\":\"NO\",\"Msg\":\"" + e.Message + "\"}";
        }
        return "{\"rel\":\"OK\",\"Msg\":\"\"}";
    }
     
    public string Select(HttpContext context)
    {
        string type = context.Request["type"];
        string li = "<li class=\"select\" >";
        if (type == "comp")
        {
            HttpCookie SelectComp = context.Request.Cookies["SelectComp"];
            if (SelectComp != null)
            {
                string value = HttpUtility.UrlDecode(SelectComp.Value, Encoding.GetEncoding("UTF-8"));
                if (string.IsNullOrWhiteSpace(value)) return "";
                string[] list = value.Split('&');
                int count = 0;
                for (int i = list.Length - 1; i > 0; i--)
                {
                    count++; if (count == 8) break;
                    li += "<p>" + list[i - 1] + "</p>";
                }
                li += "</li>";
            }
            else
            {
                return "";
            }
        }
        else
        {
            HttpCookie GoodsName = context.Request.Cookies["SelectGoods"];
            if (GoodsName != null)
            {
                string value = HttpUtility.UrlDecode(GoodsName.Value, Encoding.GetEncoding("UTF-8"));
                if (string.IsNullOrWhiteSpace(value)) return "";
                string[] list = value.Split('&');
                int count = 0;
                for (int i = list.Length - 1; i > 0; i--)
                {
                    count++; if (count == 8) break;
                    li += "<p style = \"width:430px;height:20px; line-height:26px; padding-left:10px; color:#999;border:1px solid #f6f6f6;border-top:none;background-color:white;\">" + list[i - 1] + "</p>";
                }



                li += "</li>";
            }
            else
            {
                return "";
            }
        }
        return li;
    }


    /// <summary>
    /// 删除入库、出库、盘点单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string DeleteOoder(HttpContext context)
    {
        string type = context.Request["type"] + "";//类型  (1.入库2.出库3.盘点)
        string no = Common.DesDecrypt(context.Request["no"] + "", Common.EncryptKey); ;//主表ID
        SqlTransaction tran = null;
        try
        {
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                tran = SqlHelper.CreateStoreTranSaction();
                //修改主表状态
                Hi.Model.DIS_StockOrder order = new Hi.BLL.DIS_StockOrder().GetModel(Convert.ToInt32(no));
                if (order == null) { return "主表不存在！"; }
                if (order.State == 2) { return "单据已经审核 无法删除！"; }
                order.dr = 1;
                order.modifyuser = logUser.CompID;
                bool result = new Hi.BLL.DIS_StockOrder().Update(order, tran);
                if (!result)
                {
                    tran.Rollback();
                    return "删除失败";
                }
                bool results = true;//修改商品库存的结果
                string strWhere = " StockOrderID='" + no + "'";
                if (type == "1" || type == "2")
                {
                    List<Hi.Model.DIS_StockInOut> list = new Hi.BLL.DIS_StockInOut().GetList("", strWhere, "");
                    foreach (var item in list)
                    {
                        Hi.Model.DIS_StockInOut StockInOut = new Hi.BLL.DIS_StockInOut().GetModel(item.ID);
                        StockInOut.dr = 1;
                        StockInOut.ts = DateTime.Now;
                        StockInOut.modifyuser = logUser.CompID;
                        results = new Hi.BLL.DIS_StockInOut().Update(StockInOut, tran);
                    }
                }
                if (type == "3")
                {
                    List<Hi.Model.DIS_StockChk> list = new Hi.BLL.DIS_StockChk().GetList("", strWhere, "");
                    foreach (var item in list)
                    {
                        Hi.Model.DIS_StockChk StockInOut = new Hi.BLL.DIS_StockChk().GetModel(item.ID);
                        StockInOut.dr = 1;
                        StockInOut.ts = DateTime.Now;
                        StockInOut.modifyuser = logUser.CompID;
                        results = new Hi.BLL.DIS_StockChk().Update(StockInOut, tran);
                    }
                }
                if (results)
                {
                    tran.Commit();
                    return "true";
                }
                else
                {
                    tran.Rollback();
                    return "删除失败!";
                }
            }
            return "请登录";
        }
        catch (Exception e)
        {
            tran.Rollback();
            return e.Message;
            throw;
        }
    }
    /// <summary>
    /// 批量删除订单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string DelAll(HttpContext context)
    {
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            try
            {
                int Count = 0;
                string orderid = string.Empty;
                string OrderIDstring = context.Request["IDlist"];
                string[] OrderIDlist = OrderIDstring.Split(',');
                foreach (var item in OrderIDlist)
                {
                    orderid = string.IsNullOrWhiteSpace(item) ? "0" : item;
                    Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));
                    if (order != null)
                    {
                        order.dr = 1;
                        order.ts = DateTime.Now;
                        order.modifyuser = logUser.UserID;
                        if (new Hi.BLL.DIS_Order().Update(order)) Count++;
                    }
                }
                return "{\"rel\":\"OK\",\"Msg\":\"成功删除" + Count + "条！\"}";
            }
            catch (Exception e)
            {

                return "{\"rel\":\"NO\",\"Msg\":\"" + e.Message + "\"}";
            }

        }
        return "{\"rel\":\"NO\",\"Msg\":\"登录异常！！！\"}";
    }
    /// <summary>
    /// 批量删除订单收款明细
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string DelAll2(HttpContext context)
    {
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            try
            {
                int Count = 0;
                string orderid = string.Empty;//订单ID
                string paymentID = string.Empty;//支付变或者预付款表ID
                string OrderIDandpaymentIDstring = context.Request["IDlist"].Substring(0, context.Request["IDlist"].Length - 1);//字符格式 orderid|paymentID,orderid|paymentID..........
                string[] OrderIDandpaymentIDlist = OrderIDandpaymentIDstring.Split(',');
                foreach (var item in OrderIDandpaymentIDlist)
                {
                    string[] IdList = item.Split('|');
                    orderid = IdList[0];
                    paymentID = IdList[1];
                    List<Hi.Model.PAY_Payment> order = new Hi.BLL.PAY_Payment().GetList("", " orderid=" + orderid + " and id=" + paymentID + "", "");
                    if (order.Count > 0)
                    {
                        order[0].dr = 1;
                        order[0].ts = DateTime.Now;
                        order[0].modifyuser = logUser.UserID;
                        if (new Hi.BLL.PAY_Payment().Update(order[0])) Count++;
                    }
                    List<Hi.Model.PAY_PrePayment> preorder = new Hi.BLL.PAY_PrePayment().GetList("", " orderid=" + orderid + " and id=" + paymentID + "", "");
                    if (preorder.Count > 0)
                    {
                        preorder[0].dr = 1;
                        preorder[0].ts = DateTime.Now;
                        preorder[0].modifyuser = logUser.UserID;
                        if (new Hi.BLL.PAY_PrePayment().Update(preorder[0])) Count++;
                    }
                }
                return "{\"rel\":\"OK\",\"Msg\":\"成功删除" + Count + "条！\"}";
            }
            catch (Exception e)
            {

                return "{\"rel\":\"NO\",\"Msg\":\"" + e.Message + "\"}";
            }

        }
        return "{\"rel\":\"NO\",\"Msg\":\"登录异常！！！\"}";
    }

    /// <summary>
    /// 审核入库出库单据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string ShengHe(HttpContext context)
    {
        SqlTransaction tran = null;
        try
        {
            
            
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {

                tran = SqlHelper.CreateStoreTranSaction();
                string type = context.Request["type"] + "";//出库或者入库
                string no = Common.DesDecrypt(context.Request["no"], Common.EncryptKey);//主表ID
                                                                                        //修改主表状态
                Hi.Model.DIS_StockOrder order = new Hi.BLL.DIS_StockOrder().GetModel(Convert.ToInt32(no));
                if (order == null) { return "主表不存在"; }
                if (order.State == 2) { return "单据已经审核 无法再次审核"; }
                if (order.dr == 1) { return "单据已经删除 无法审核"; }

                order.State = 2;
                order.modifyuser = logUser.CompID;
                bool result = new Hi.BLL.DIS_StockOrder().Update(order, tran);
                if (!result)
                {
                    tran.Rollback();
                    return "修改失败";
                }
                bool results = true;//修改商品库存的结果
                string strWhat = "";
                string strWhere = " StockOrderID='" + no + "'";
                List<Hi.Model.DIS_StockInOut> list = new Hi.BLL.DIS_StockInOut().GetList(strWhat, strWhere, "");
                int count = 0;
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        count++;
                        decimal stockNum = 0;
                        decimal StockTotalNum = 0;
                        decimal Inventory = 0;
                        
                        string Where = " ID='" + item.GoodsInfoID + "' ";
                        List<Hi.Model.BD_GoodsInfo> model = new Hi.BLL.BD_GoodsInfo().GetList("", Where, "");
                        if (model == null || model.Count <= 0)
                        {
                            if (type == "2")
                            {
                                return "商品:'" + GetName(model[0].GoodsID) + "'的信息有误，请重新输入！";
                            }
                        }
                        string stocksql = " GoodsInfo=" + item.GoodsInfoID + " and BatchNO='" + item.Batchno + "'";

                        List<Hi.Model.DIS_GoodsStock> GoodsStocklists = new Hi.BLL.DIS_GoodsStock().GetList("", " GoodsInfo=" + item.GoodsInfoID, "");
                        List<Hi.Model.DIS_GoodsStock> GoodsStocklist = GoodsStocklists.FindAll(p => p.BatchNO == item.Batchno);
                        
                        //new Hi.BLL.DIS_GoodsStock().GetList("", stocksql, "");
                        if (GoodsStocklist != null && GoodsStocklist.Count > 0)
                        {
                            Hi.Model.DIS_GoodsStock goodsStockModel = GoodsStocklist[0];
                            if (type == "2")
                            {
                                //出库
                                if (goodsStockModel.StockNum < item.StockNum)
                                {

                                    return "该批次" + item.Batchno + "商品:'" + GetName(model[0].GoodsID) + "'的库存数量小于要出库的数量，请重新输入！";
                                }

                                stockNum = goodsStockModel.StockNum - item.StockNum;
                                StockTotalNum = goodsStockModel.StockTotalNum -= item.StockNum;
                                Inventory -= item.StockNum;

                            }
                            else
                            {
                                //入库
                                stockNum = goodsStockModel.StockNum + item.StockNum;
                                StockTotalNum = goodsStockModel.StockTotalNum += item.StockNum;
                                Inventory += item.StockNum;
                            }
                            goodsStockModel.ts = DateTime.Now;
                            goodsStockModel.modifyuser = logUser.UserID;
                            goodsStockModel.StockNum = stockNum;

                            results = new Hi.BLL.DIS_GoodsStock().Update(goodsStockModel, tran);
                        }
                        else
                        {

                            if (type == "2")
                            {
                                return "该批次" + item.Batchno + "商品:'" + GetName(model[0].GoodsID) + "'没有库存数量，请重新输入！";
                            }

                            //出入库,没有商品库存信息，新增记录
                            Hi.Model.DIS_GoodsStock goodsStockModel = new Hi.Model.DIS_GoodsStock();
                            goodsStockModel.BatchNO = item.Batchno;
                            goodsStockModel.validDate = item.Validdate;
                            goodsStockModel.CompID = logUser.CompID;
                            goodsStockModel.CreateDate = DateTime.Now;
                            goodsStockModel.CreateUserID = logUser.UserID;
                            goodsStockModel.dr = 0;
                            goodsStockModel.GoodsID = item.GoodsID;
                            goodsStockModel.GoodsInfo = item.GoodsInfoID.ToString();
                            goodsStockModel.modifyuser = logUser.UserID;
                            goodsStockModel.ts = DateTime.Now;

                            goodsStockModel.StockNum = item.StockNum;
                            StockTotalNum = goodsStockModel.StockTotalNum = GoodsStocklists.Count > 0 ? GoodsStocklists[0].StockTotalNum + item.StockNum : item.StockNum;
                            Inventory = item.StockNum;

                            results = new Hi.BLL.DIS_GoodsStock().Add(goodsStockModel, tran) > 0;
                        }
                        if (GoodsStocklists != null && GoodsStocklists.Count > 0)
                        {
                            foreach (var itemTnum in GoodsStocklists)
                            {
                                itemTnum.StockTotalNum = StockTotalNum;
                                results = new Hi.BLL.DIS_GoodsStock().Update(itemTnum, tran);
                            }
                        }
                        model[0].Inventory += Inventory;
                        model[0].ts = DateTime.Now;
                        model[0].modifyuser = logUser.UserID;
                        results = new Hi.BLL.BD_GoodsInfo().Update(model[0], tran);
                        
                           
                    }
                }
                if (results)
                {
                    tran.Commit();
                    return "OK";
                }
                else
                {
                    tran.Rollback();
                    return "审核失败！请核对商品信息";
                }
            }
            return "请登录";
        }
        catch (Exception e)
        {
            tran.Rollback();
            return e.Message;
            throw;
        }



    }
    /// <summary>
    /// 商品盘点新增
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string insertInventory(HttpContext context)
    {
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        string soid = "0";
        if (logUser != null)
        {
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string NO = GetNo(3);//单号
                    string remark = Common.NoHTML(item["remark"].ToString().Trim());//单据备注
                    int ID = Convert.ToInt32(item["ID"].ToString().Trim());
                    string orderDetail = item["Inventorydetail"].ToString();//订单明细
                    if (orderDetail != "")
                    {
                        if (ID == 0)
                        {
                            Hi.Model.DIS_StockOrder StockOrder = new Hi.Model.DIS_StockOrder();//商品出入库盘点主表
                            StockOrder.CompID = logUser.CompID;
                            StockOrder.OrderNO = NO;
                            StockOrder.Type = Convert.ToInt32(item["type"].ToString().Trim());//1入库 2出库 3 盘点
                            StockOrder.StockType = "";
                            StockOrder.ChkDate = DateTime.Now;
                            StockOrder.Remark = remark;
                            StockOrder.State = 0;//0制单  2审核
                            StockOrder.CreateUserID = logUser.UserID;
                            StockOrder.CreateDate = DateTime.Now;
                            StockOrder.ts = DateTime.Now;
                            StockOrder.dr = 0;
                            StockOrder.modifyuser = logUser.UserID;
                            int StockOrderID = new Hi.BLL.DIS_StockOrder().Add(StockOrder);
                            if (StockOrderID > 0)
                            {
                                Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(orderDetail);
                                if (l.Count > 0)
                                {
                                    foreach (Newtonsoft.Json.Linq.JObject items in ll)
                                    {
                                        //商品盘点明细表
                                        Hi.Model.DIS_StockChk StockChk = new Hi.Model.DIS_StockChk();
                                        StockChk.CompID = logUser.CompID;
                                        StockChk.StockOrderID = StockOrderID;//盘点主表ID
                                        StockChk.GoodsID = Convert.ToInt32(items["id"].ToString());//商品ID
                                        StockChk.GoodsInfoID = Convert.ToInt32(items["skuid"].ToString());//商品详情ID
                                        string Where = " ID='" + StockChk.GoodsInfoID + "' ";
                                        List<Hi.Model.BD_GoodsInfo> model = new Hi.BLL.BD_GoodsInfo().GetList("", Where, "");
                                        if (model.Count > 0)
                                        {
                                            StockChk.StockOldNum = model[0].Inventory;
                                        }
                                        StockChk.StockNum = Convert.ToDecimal(items["goodsnum"].ToString());
                                        StockChk.Remark = Common.NoHTML(items["remark"].ToString());
                                        StockChk.CreateUserID = logUser.UserID;
                                        StockChk.CreateDate = DateTime.Now;
                                        StockChk.ts = DateTime.Now;
                                        StockChk.dr = 0;
                                        StockChk.modifyuser = logUser.UserID;
                                        new Hi.BLL.DIS_StockChk().Add(StockChk);
                                    }
                                }
                            }
                            soid = Common.DesEncrypt(StockOrderID.ToString(), Common.EncryptKey);
                        }
                        else
                        {
                            //修改主表
                            Hi.Model.DIS_StockOrder StockOrder = new Hi.BLL.DIS_StockOrder().GetModel(ID);
                            StockOrder.StockType = "";
                            StockOrder.Remark = remark;
                            StockOrder.ts = DateTime.Now;
                            StockOrder.CreateUserID = logUser.UserID;
                            StockOrder.CreateDate = DateTime.Now;
                            new Hi.BLL.DIS_StockOrder().Update(StockOrder);
                            List<Hi.Model.DIS_StockChk> list = new Hi.BLL.DIS_StockChk().GetList("ID", " StockOrderID=" + ID + "", "");
                            if (list.Count > 0)
                            {
                                List<int> listID = new List<int>();
                                foreach (var StockChk in list)
                                {
                                    listID.Add(StockChk.ID);
                                }
                                new Hi.BLL.DIS_StockChk().Delete(listID);
                            }
                            Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(orderDetail);
                            if (l.Count > 0)
                            {
                                foreach (Newtonsoft.Json.Linq.JObject items in ll)
                                {
                                    //商品盘点明细表
                                    Hi.Model.DIS_StockChk StockChk = new Hi.Model.DIS_StockChk();
                                    StockChk.CompID = logUser.CompID;
                                    StockChk.StockOrderID = ID;//盘点主表ID
                                    StockChk.GoodsID = Convert.ToInt32(items["id"].ToString());//商品ID
                                    StockChk.GoodsInfoID = Convert.ToInt32(items["skuid"].ToString());//商品详情ID
                                    string Where = " ID='" + StockChk.GoodsInfoID + "' ";
                                    List<Hi.Model.BD_GoodsInfo> model = new Hi.BLL.BD_GoodsInfo().GetList("", Where, "");
                                    if (model.Count > 0)
                                    {
                                        StockChk.StockOldNum = model[0].Inventory;
                                    }
                                    StockChk.StockNum = Convert.ToDecimal(items["goodsnum"].ToString());
                                    StockChk.Remark = Common.NoHTML(items["remark"].ToString());
                                    StockChk.CreateUserID = logUser.UserID;
                                    StockChk.CreateDate = DateTime.Now;
                                    StockChk.ts = DateTime.Now;
                                    StockChk.dr = 0;
                                    StockChk.modifyuser = logUser.UserID;
                                    new Hi.BLL.DIS_StockChk().Add(StockChk);
                                }
                            }
                            soid = Common.DesEncrypt(StockOrder.ID.ToString(), Common.EncryptKey);
                        }
                    }
                    else
                    {
                        return "至少添加一条商品！";
                    }
                }
            }
        }
        else
        {
            return "请登录";
        }
        return "{\"returns\":\"True\",\"no\":\"" + soid + "\"}";
    }

    /// <summary>
    /// 获取单号
    /// </summary>
    /// <returns></returns>
    public string GetNo(int type)
    {
        //try
        //{
        string title = string.Empty;
        if (type == 1) { title = "RK"; }
        else if (type == 2) { title = "CK"; }
        else { title = "PD"; }
        string no = string.Empty;
        int ID = new Hi.BLL.DIS_StockOrder().GetMaxId();
        if (ID < 1)
        {
            no = "RK20170209000";
        }
        else
        {
            no = new Hi.BLL.DIS_StockOrder().GetModel(ID).OrderNO;
        }

        int num = Convert.ToInt32(no.Substring(10)) + 1;
        if (num < 10)
        {
            no = "00" + num;
        }
        else if (num < 100)
        {
            no = "0" + num;
        }
        else
        {
            no = num.ToString();
        }


        string No = title + DateTime.Now.ToString("yyyyMMdd") + no;
        return No;
        //}
        //catch (Exception)
        //{
        //    GetNo(type);
        //    throw;
        //}


    }
    /// <summary>
    /// 审核盘点单据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string ShengHe1(HttpContext context)
    {
        SqlTransaction tran = null;
        try
        {
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                tran = SqlHelper.CreateStoreTranSaction();
                string type = "3";//出库或者入库
                string no = Common.DesDecrypt(context.Request["no"], Common.EncryptKey);//解密详情ID  + "";//主表ID
                                                                                        //修改主表状态
                Hi.Model.DIS_StockOrder order = new Hi.BLL.DIS_StockOrder().GetModel(Convert.ToInt32(no));
                if (order == null) { return "主表不存在"; }
                if (order.State == 2) { return "单据已经审核 无法再次审核"; }
                if (order.dr == 1) { return "单据已经删除 无法审核"; }
                order.State = 2;
                order.modifyuser = logUser.CompID;
                bool result = new Hi.BLL.DIS_StockOrder().Update(order, tran);
                if (!result)
                {
                    tran.Rollback();
                    return "修改失败";
                }
                bool results = true;//修改商品库存的结果
                string strWhat = "GoodsID,GoodsInfoID,StockNum";
                string strWhere = " StockOrderID='" + no + "'";
                List<Hi.Model.DIS_StockChk> list = new Hi.BLL.DIS_StockChk().GetList(strWhat, strWhere, "");
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        string Where = " ID='" + item.GoodsInfoID + "' ";
                        List<Hi.Model.BD_GoodsInfo> model = new Hi.BLL.BD_GoodsInfo().GetList("", Where, "");
                        if (model.Count > 0)
                        {
                            if (type == "3")
                            {
                                model[0].Inventory = item.StockNum;
                                item.StockOldNum = item.StockNum;
                            }
                            model[0].Inventory = model[0].Inventory < 0 ? 0 : model[0].Inventory;
                            model[0].modifyuser = logUser.CompID;
                            results = new Hi.BLL.BD_GoodsInfo().Update(model[0], tran);
                        }
                    }
                    if (results)
                    {
                        tran.Commit();
                        return "{\"returns\":\"True\",\"no\":\"" + context.Request["no"] + "\"}";
                    }
                    else
                    {
                        tran.Rollback();
                        return "审核失败！请核对商品信息";
                    }
                }
                else
                {
                    tran.Rollback();
                    return "审核失败！请添加商品信息";
                }
            }
            throw new ApplicationException("请先登录");
        }
        catch (Exception e)
        {
            tran.Rollback();
            return e.Message;
            throw;
        }
    }

    public string GetName(int ID)
    {
        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(ID);
        if (goods != null)
        {
            return goods.GoodsName;
        }
        else
        {
            return ID.ToString();
        }
    }


    /// <summary>
    /// 收款单审核
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string auditPayment(HttpContext context)
    {
        string PaymentID = context.Request["PaymentID"] + "";//传过来入库单ID

        if (string.IsNullOrWhiteSpace(PaymentID))
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        Hi.Model.YZT_Payment paymentModel = new Hi.BLL.YZT_Payment().GetModel(Convert.ToInt32(PaymentID));

        if (paymentModel == null)
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        if (paymentModel.IState == 1)
            return "{\"returns\":\"no\",\"msg\":\"单据已审核\"}";


        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser == null)
        {
            throw new ApplicationException("请先登录");
        }
        paymentModel.IState = 1;
        paymentModel.ts = DateTime.Now;
        paymentModel.modifyuser = logUser.UserID;
        if (!new Hi.BLL.YZT_Payment().Update(paymentModel))
            return "{\"returns\":\"no\",\"msg\":\"审核失败\"}";
        else
            return "{\"returns\":\"true\",\"msg\":\"审核成功\",\"PaymentID\":\"" + PaymentID + "\"}";
    }






    /// <summary>
    /// 审核代理商出库单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string auditLibrary(HttpContext context)
    {
        string LibraryID = context.Request["LibraryID"] + "";//传过来入库单ID

        if (string.IsNullOrWhiteSpace(LibraryID))
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        Hi.Model.YZT_Library libraryModel = new Hi.BLL.YZT_Library().GetModel(Convert.ToInt32(LibraryID));

        if (libraryModel == null)
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        if (libraryModel.IState == 1)
            return "{\"returns\":\"no\",\"msg\":\"单据已审核\"}";



        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            libraryModel.IState = 1;
            libraryModel.ts = DateTime.Now;
            libraryModel.modifyuser = logUser.UserID;
            if (!new Hi.BLL.YZT_Library().Update(libraryModel, tran))
                throw new ApplicationException("审核失败");

            //商品 审核入库
            Hi.Model.YZT_GoodsStock goodsStockModel = null;
            List<Hi.Model.YZT_LibraryDetail> libraryDetailList = new Hi.BLL.YZT_LibraryDetail().GetList("", " dr=0 and LibraryID=" + LibraryID + "", "");
            foreach (var item in libraryDetailList)
            {
                string where = string.Format("DisID={0} and  GoodsName='{1}' and ValueInfo='{2}' and id={3}", logUser.DisID, item.GoodsName, item.ValueInfo, item.StockID);
                List<Hi.Model.YZT_GoodsStock> GoodsStockList = new Hi.BLL.YZT_GoodsStock().GetList("", where, "");
                if (GoodsStockList.Count > 0)
                {
                    goodsStockModel = GoodsStockList[0];
                    if (goodsStockModel.StockNum < item.OutNum) throw new ApplicationException("商品：" + item.GoodsName + " 库存不足");
                    goodsStockModel.StockNum -= item.OutNum;
                    goodsStockModel.StockUseNum -= item.OutNum;
                    if (goodsStockModel.StockNum < 0) goodsStockModel.StockNum = 0;
                    goodsStockModel.modifyuser = logUser.UserID;
                    new Hi.BLL.YZT_GoodsStock().Update(goodsStockModel, tran);
                }
                else throw new ApplicationException("库存中无此商品：" + item.GoodsName + "");
            }

        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"no\",\"msg\":\"" + e.Message + "\"}";
        }


        tran.Commit();
        return "{\"returns\":\"true\",\"msg\":\"审核成功\",\"LibraryID\":\"" + LibraryID + "\"}";
    }


    /// <summary>
    /// 审核代理商入库单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string auditStorage(HttpContext context)
    {
        string StorageID = context.Request["StorageID"] + "";//传过来入库单ID

        if (string.IsNullOrWhiteSpace(StorageID))
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        Hi.Model.YZT_Storage storageModel = new Hi.BLL.YZT_Storage().GetModel(Convert.ToInt32(StorageID));

        if (storageModel == null)
            return "{\"returns\":\"no\",\"msg\":\"未找到单据\"}";

        if (storageModel.IState == 1)
            return "{\"returns\":\"no\",\"msg\":\"单据已审核\"}";

        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            storageModel.IState = 1;
            storageModel.ts = DateTime.Now;
            storageModel.modifyuser = logUser.UserID;
            if (!new Hi.BLL.YZT_Storage().Update(storageModel, tran))
                throw new ApplicationException("审核失败");

            //商品 审核入库
            Hi.Model.YZT_GoodsStock goodsStockModel = null;
            List<Hi.Model.YZT_StorageDetail> storageDetailList = new Hi.BLL.YZT_StorageDetail().GetList("", " dr=0 and StorageID=" + StorageID + "", "");
            foreach (var item in storageDetailList)
            {
                string where = string.Format("DisID={0} and GoodsID ='{1}' and GoodsName='{2}' and ValueInfo='{3}'", logUser.DisID, item.GoodsID, item.GoodsName, item.ValueInfo);
                List<Hi.Model.YZT_GoodsStock> GoodsStockList = new Hi.BLL.YZT_GoodsStock().GetList("", where, "");
                if (GoodsStockList.Count > 0)
                {
                    goodsStockModel = GoodsStockList[0];
                    goodsStockModel.StockNum += item.StorageNum;
                    goodsStockModel.modifyuser = logUser.UserID;
                    new Hi.BLL.YZT_GoodsStock().Update(goodsStockModel, tran);
                }
                else
                {
                    goodsStockModel = new Hi.Model.YZT_GoodsStock();
                    goodsStockModel.DisID = logUser.DisID;
                    goodsStockModel.CompID = item.CompID;
                    goodsStockModel.GoodsID = item.GoodsID;
                    goodsStockModel.GoodsCode = item.GoodsCode;
                    goodsStockModel.GoodsName = item.GoodsName;
                    goodsStockModel.ValueInfo = item.ValueInfo;
                    goodsStockModel.Unit = item.Unit;
                    goodsStockModel.BatchNO = item.BatchNO;
                    goodsStockModel.validDate = item.validDate;
                    goodsStockModel.StockNum = item.StorageNum;
                    goodsStockModel.CreateUserID = logUser.UserID;
                    goodsStockModel.CreateDate = DateTime.Now;
                    goodsStockModel.ts = DateTime.Now;
                    goodsStockModel.dr = 0;
                    goodsStockModel.modifyuser = logUser.UserID;
                    new Hi.BLL.YZT_GoodsStock().Add(goodsStockModel, tran);
                }
            }

        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"no\",\"msg\":\"" + e.Message + "\"}";
        }


        tran.Commit();
        return "{\"returns\":\"true\",\"msg\":\"审核成功\",\"StorageID\":\"" + StorageID + "\"}";
    }


    /// <summary>
    /// 编辑代理商入库单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string disStorageEdit(HttpContext context)
    {
        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string StorageID = "";
            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string ddrCompid = item["ddrCompid"].ToString().Trim();//厂商id
                    string StorageType = item["StorageType"].ToString().Trim();//入库类型
                    string txtStorageDate = item["txtStorageDate"].ToString().Trim();//入库日期
                    string OrderNote = item["OrderNote"].ToString().Trim();//备注
                    string StorageNO = item["StorageNO"].ToString().Trim();//单号
                    StorageID = item["StorageID"].ToString().Trim();//ID

                    if (getNo(StorageNO, "Storage", StorageID)) throw new ApplicationException("单据号重复");

                    if (string.IsNullOrWhiteSpace(StorageID))
                    {
                        //添加
                        Hi.Model.YZT_Storage storageModel = new Hi.Model.YZT_Storage();
                        storageModel.StorageDate = Convert.ToDateTime(txtStorageDate);
                        storageModel.DisID = logUser.DisID;
                        storageModel.StorageNO = "";
                        storageModel.StorageType = Convert.ToInt32(StorageType);
                        storageModel.IState = 0;
                        storageModel.StorageNO = StorageNO;
                        storageModel.Remark = OrderNote;
                        storageModel.CreateDate = DateTime.Now;
                        storageModel.CreateUserID = logUser.UserID;
                        storageModel.dr = 0;
                        storageModel.ts = DateTime.Now;
                        storageModel.modifyuser = logUser.UserID;
                        storageModel.CompID = Convert.ToInt32(ddrCompid);
                        StorageID = new Hi.BLL.YZT_Storage().Add(storageModel, tran).ToString();
                        if (!(Convert.ToInt32(StorageID) > 0)) throw new Exception("添加失败");
                    }
                    else
                    {
                        //修改
                        Hi.Model.YZT_Storage storageModel = new Hi.BLL.YZT_Storage().GetModel(Convert.ToInt32(StorageID));
                        storageModel.StorageDate = Convert.ToDateTime(txtStorageDate);
                        storageModel.StorageNO = "";
                        storageModel.StorageType = Convert.ToInt32(StorageType);
                        storageModel.IState = 0;
                        storageModel.StorageNO = StorageNO;
                        storageModel.Remark = OrderNote;
                        storageModel.ts = DateTime.Now;
                        storageModel.CompID = Convert.ToInt32(ddrCompid);
                        storageModel.modifyuser = logUser.UserID;
                        if (!new Hi.BLL.YZT_Storage().Update(storageModel, tran))
                            throw new Exception("修改失败");

                        //删除从表
                        List<Hi.Model.YZT_StorageDetail> storageDetailList = new Hi.BLL.YZT_StorageDetail().GetList("", " dr=0 and StorageID=" + StorageID + "", "");
                        if (storageDetailList.Count > 0)
                        {
                            List<int> listID = new List<int>();
                            foreach (var itemDelete in storageDetailList)
                            {
                                new Hi.BLL.YZT_StorageDetail().Delete(itemDelete.ID, tran);
                            }
                        }
                    }
                    string contractDetail = item["orderdetail"].ToString();//订单明细
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(contractDetail);
                    //添加子表
                    if (ll.Count > 0 && !string.IsNullOrWhiteSpace(StorageID))
                    {
                        Hi.Model.YZT_StorageDetail storageDetailModel = null;
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            storageDetailModel = new Hi.Model.YZT_StorageDetail();
                            storageDetailModel.DisID = logUser.DisID;
                            storageDetailModel.CompID = Convert.ToInt32(ddrCompid);
                            storageDetailModel.StorageID = Convert.ToInt32(StorageID);
                            storageDetailModel.GoodsID = Convert.ToInt32(items["tip"].ToString());
                            storageDetailModel.GoodsCode = items["GoodsCode"].ToString();
                            storageDetailModel.GoodsName = items["GoodsName"].ToString();
                            storageDetailModel.ValueInfo = items["ValueInfo"].ToString();
                            storageDetailModel.Unit = items["Unit"].ToString();
                            storageDetailModel.BatchNO = items["BatchNO"].ToString();
                            storageDetailModel.validDate = Convert.ToDateTime(items["validDate"].ToString());
                            storageDetailModel.StorageNum = Convert.ToDecimal(items["StorageNum"].ToString());
                            storageDetailModel.Remark = items["Remark"].ToString();
                            storageDetailModel.ts = DateTime.Now;
                            storageDetailModel.CreateDate = DateTime.Now;
                            storageDetailModel.CreateUserID = logUser.UserID;
                            storageDetailModel.modifyuser = logUser.UserID;
                            if (!(new Hi.BLL.YZT_StorageDetail().Add(storageDetailModel, tran) > 0))
                                throw new Exception("编辑失败！！！");
                        }
                    }
                };
                tran.Commit();
                return "{\"returns\":\"true\",\"msg\":\"编辑成功\",\"StorageID\":\"" + StorageID + "\"}";
            }
            else
            {

                throw new Exception("未找到参数");
            }
        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
        }
    }

    /// <summary>
    /// 检验单号是否重复
    /// </summary>
    /// <param name="no">单号</param>
    /// <param name="type">单据类型</param>
    /// <param name="id">单据ID</param>
    /// <returns></returns>
    public bool getNo(string no, string type, string id)
    {
        if (type == "Storage")
        {
            string where = " StorageNO ='" + no + "' and dr=0 ";
            if (!string.IsNullOrWhiteSpace(id)) where += " and id!=" + id + "";
            List<Hi.Model.YZT_Storage> model = new Hi.BLL.YZT_Storage().GetList("", where, "");
            return model.Count > 0;
        }
        else if (type == "Library")
        {
            string where = " LibraryNO ='" + no + "' and dr=0 ";
            if (!string.IsNullOrWhiteSpace(id)) where += " and id!=" + id + "";
            List<Hi.Model.YZT_Library> model = new Hi.BLL.YZT_Library().GetList("", where, "");
            return model.Count > 0;
        }
        else
        {
            string where = " PaymentNO ='" + no + "' and dr=0 ";
            if (!string.IsNullOrWhiteSpace(id)) where += " and id!=" + id + "";
            List<Hi.Model.YZT_Payment> model = new Hi.BLL.YZT_Payment().GetList("", where, "");
            return model.Count > 0;
        }


    }


    /// <summary>
    /// 代理商编辑收款单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string disPaymentEdit(HttpContext context)
    {
        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string PaymentID = "";
            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string PaymentNO = item["PaymentNO"].ToString().Trim();//收款单号
                    string PaymentDate = item["PaymentDate"].ToString().Trim();//收款日期
                    string IState = item["IState"].ToString().Trim();//状态
                    string HtDrop = item["HtDrop"].ToString().Trim();//医院
                    string PaymentAmount = item["PaymentAmount"].ToString().Trim();//账期
                    string Remark = item["Remark"].ToString().Trim();//到款日期
                    string HidFfileName = item["HidFfileName"].ToString().Trim();//附件
                    PaymentID = item["PaymentID"].ToString().Trim();//合同ID

                    if (getNo(PaymentNO, "", PaymentID)) throw new ApplicationException("单据号重复");


                    if (string.IsNullOrWhiteSpace(PaymentID))
                    {
                        //添加
                        Hi.Model.YZT_Payment paymentModel = new Hi.Model.YZT_Payment();
                        paymentModel.PaymentNO = PaymentNO;
                        paymentModel.PaymentDate = Convert.ToDateTime(PaymentDate);
                        paymentModel.IState = Convert.ToInt32(IState);
                        //paymentModel.HtID = Convert.ToInt32(HtDrop);
                        paymentModel.hospital = HtDrop;
                        paymentModel.PaymentAmount = Convert.ToDecimal(PaymentAmount);
                        paymentModel.DisID = logUser.DisID;
                        paymentModel.Remark = Remark;
                        paymentModel.CreateDate = DateTime.Now;
                        paymentModel.CreateUserID = logUser.UserID;
                        paymentModel.dr = 0;
                        paymentModel.ts = DateTime.Now;
                        paymentModel.modifyuser = logUser.UserID;
                        paymentModel.CompID = 0;
                        PaymentID = new Hi.BLL.YZT_Payment().Add(paymentModel, tran).ToString();
                        if (!(Convert.ToInt32(PaymentID) > 0)) throw new Exception("添加失败");
                    }
                    else
                    {
                        //修改
                        Hi.Model.YZT_Payment paymentModel = new Hi.BLL.YZT_Payment().GetModel(Convert.ToInt32(PaymentID));
                        paymentModel.PaymentNO = PaymentNO;
                        paymentModel.PaymentDate = Convert.ToDateTime(PaymentDate);
                        paymentModel.IState = Convert.ToInt32(IState);
                        //paymentModel.HtID = Convert.ToInt32(HtDrop);
                        paymentModel.hospital = HtDrop;
                        paymentModel.PaymentAmount = Convert.ToDecimal(PaymentAmount);
                        paymentModel.Remark = Remark;
                        paymentModel.ts = DateTime.Now;
                        paymentModel.modifyuser = logUser.UserID;
                        if (!new Hi.BLL.YZT_Payment().Update(paymentModel, tran))
                            throw new Exception("修改失败");

                        //删除从表
                        List<Hi.Model.YZT_PaymentDetail> PaymentDetailList = new Hi.BLL.YZT_PaymentDetail().GetList("", " dr=0 and PaymentID=" + PaymentID + "", "");
                        if (PaymentDetailList.Count > 0)
                        {
                            foreach (var itemDelete in PaymentDetailList)
                            {
                                new Hi.BLL.YZT_PaymentDetail().Delete(itemDelete.ID, tran);
                            }
                        }

                        //删除附件
                        List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + PaymentID + " and fileAlias=6 and type=11", "");
                        if (AnnexDelList.Count > 0)
                        {
                            foreach (var Annexitem in AnnexDelList)
                            {
                                if (!new Hi.BLL.YZT_Annex().Delete(Annexitem.ID)) throw new Exception("编辑附件失败！！！");
                            }
                        }



                    }
                    string contractDetail = item["orderdetail"].ToString();//订单明细
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(contractDetail);
                    //添加子表
                    if (ll.Count > 0 && !string.IsNullOrWhiteSpace(PaymentID))
                    {
                        Hi.Model.YZT_PaymentDetail PaymentDetailList = null;
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            PaymentDetailList = new Hi.Model.YZT_PaymentDetail();
                            PaymentDetailList.DisID = logUser.DisID;
                            PaymentDetailList.CompID = 0;
                            PaymentDetailList.PaymentID = PaymentID;
                            PaymentDetailList.LibraryID = items["tip"].ToString();
                            PaymentDetailList.GoodsCode = items["GoodsCode"].ToString();
                            PaymentDetailList.GoodsName = items["GoodsName"].ToString();
                            PaymentDetailList.ValueInfo = items["ValueInfo"].ToString();
                            PaymentDetailList.Unit = items["Unit"].ToString();
                            PaymentDetailList.BatchNO = items["BatchNO"].ToString();
                            PaymentDetailList.validDate = Convert.ToDateTime(items["validDate"].ToString());
                            PaymentDetailList.Num = Convert.ToDecimal(items["OutNum"].ToString());
                            PaymentDetailList.Remark = items["Remark"].ToString();
                            PaymentDetailList.AuditAmount = Convert.ToDecimal(items["AuditAmount"].ToString());
                            PaymentDetailList.sumAmount = Convert.ToDecimal(items["sumAmount"].ToString());
                            PaymentDetailList.ts = DateTime.Now;
                            PaymentDetailList.CreateDate = DateTime.Now;
                            PaymentDetailList.CreateUserID = logUser.UserID;
                            PaymentDetailList.modifyuser = logUser.UserID;
                            if (!(new Hi.BLL.YZT_PaymentDetail().Add(PaymentDetailList, tran) > 0))
                                throw new Exception("编辑失败！！！");
                        }
                    }

                    //添加附件
                    if (!string.IsNullOrWhiteSpace(HidFfileName))
                    {
                        string[] HidFfileNames = HidFfileName.Split(',');
                        Hi.Model.YZT_Annex annexModel = null;
                        foreach (var FfileNames in HidFfileNames)
                        {
                            annexModel = new Hi.Model.YZT_Annex();
                            DateTime time = DateTime.Now;
                            annexModel.fcID = Convert.ToInt32(PaymentID);
                            annexModel.type = 11;
                            annexModel.fileName = FfileNames;
                            annexModel.fileAlias = "6";
                            annexModel.validDate = time;
                            annexModel.CreateDate = time;
                            annexModel.dr = 0;
                            annexModel.ts = time;
                            annexModel.modifyuser = logUser.UserID;
                            annexModel.CreateUserID = logUser.UserID;
                            if (!(new Hi.BLL.YZT_Annex().Add(annexModel, tran) > 0)) throw new Exception("新增附件失败！！！");
                        }
                    }
                };
                tran.Commit();
                return "{\"returns\":\"true\",\"msg\":\"编辑成功\",\"PaymentID\":\"" + PaymentID + "\"}";
            }
            else
            {

                throw new Exception("未找到参数");
            }
        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
        }
    }

    /// <summary>
    /// 编辑代理商出库单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string disLibraryEdit(HttpContext context)
    {
        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string LibraryID = "";
            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string LibraryNO = item["LibraryNO"].ToString().Trim();//出库单号
                    string LibraryDate = item["LibraryDate"].ToString().Trim();//出库日期
                    string Salesman = item["Salesman"].ToString().Trim();//销售人员
                    string HtDrop = item["HtDrop"].ToString().Trim();//医院
                    string PaymentDays = item["PaymentDays"].ToString().Trim();//账期
                    if (string.IsNullOrWhiteSpace(PaymentDays)) PaymentDays = "0";
                    string MoneyDate = item["MoneyDate"].ToString().Trim();//到款日期

                    string OrderNote = item["OrderNote"].ToString().Trim();//备注
                    string HidFfileName = item["HidFfileName"].ToString().Trim();//附件
                    LibraryID = item["LibraryID"].ToString().Trim();//ID

                    if (getNo(LibraryNO, "Library", LibraryID)) throw new ApplicationException("单据号重复");

                    if (string.IsNullOrWhiteSpace(LibraryID))
                    {
                        //添加
                        Hi.Model.YZT_Library libraryModel = new Hi.Model.YZT_Library();
                        libraryModel.LibraryNO = LibraryNO;
                        libraryModel.LibraryDate = Convert.ToDateTime(LibraryDate);
                        libraryModel.Salesman = Salesman;
                        //libraryModel.HtID = Convert.ToInt32(HtDrop);
                        libraryModel.hospital = HtDrop;
                        libraryModel.PaymentDays = Convert.ToInt32(PaymentDays);
                        libraryModel.MoneyDate = Convert.ToDateTime(MoneyDate);
                        libraryModel.DisID = logUser.DisID;
                        libraryModel.Remark = OrderNote;
                        libraryModel.CreateDate = DateTime.Now;
                        libraryModel.CreateUserID = logUser.UserID;
                        libraryModel.dr = 0;
                        libraryModel.ts = DateTime.Now;
                        libraryModel.modifyuser = logUser.UserID;
                        libraryModel.CompID = 0;
                        LibraryID = new Hi.BLL.YZT_Library().Add(libraryModel, tran).ToString();
                        if (!(Convert.ToInt32(LibraryID) > 0)) throw new Exception("添加失败");
                    }
                    else
                    {
                        string stockids = "";
                        
                        //修改
                        Hi.Model.YZT_Library libraryModel = new Hi.BLL.YZT_Library().GetModel(Convert.ToInt32(LibraryID));
                        libraryModel.LibraryNO = LibraryNO;
                        libraryModel.LibraryDate = Convert.ToDateTime(LibraryDate);
                        libraryModel.Salesman = Salesman;
                        //libraryModel.HtID = Convert.ToInt32(HtDrop);
                        libraryModel.hospital = HtDrop;
                        libraryModel.PaymentDays = Convert.ToInt32(PaymentDays);
                        libraryModel.MoneyDate = Convert.ToDateTime(MoneyDate);
                        libraryModel.DisID = logUser.DisID;
                        libraryModel.Remark = OrderNote;
                        libraryModel.ts = DateTime.Now;
                        libraryModel.modifyuser = logUser.UserID;
                        if (!new Hi.BLL.YZT_Library().Update(libraryModel, tran))
                            throw new Exception("修改失败");

                        //删除从表
                        List<Hi.Model.YZT_LibraryDetail> LibraryDetailList = new Hi.BLL.YZT_LibraryDetail().GetList("", " dr=0 and LibraryID=" + LibraryID + "", "");

                        
                        if (LibraryDetailList.Count > 0)
                        {
                            foreach (var Lbitem in LibraryDetailList)
                            {
                                stockids += stockids == "" ? Lbitem.StockID.ToString() : "," + Lbitem.StockID.ToString();
                            }

                            List<Hi.Model.YZT_GoodsStock> gslist = new Hi.BLL.YZT_GoodsStock().GetList("", " ID in(" + stockids + ")", "");
                        
                            foreach (var itemDelete in LibraryDetailList)
                            {
                                new Hi.BLL.YZT_LibraryDetail().Delete(itemDelete.ID, tran);

                                //删除占用库存
                                Hi.Model.YZT_GoodsStock GoodsStock = gslist.Find(p => p.ID == itemDelete.StockID);
                                GoodsStock.StockUseNum = GoodsStock.StockUseNum - itemDelete.OutNum;
                                
                                new Hi.BLL.YZT_GoodsStock().Update(GoodsStock);
                            }
                        }

                        //删除附件
                        List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + LibraryID + " and fileAlias=5 and type=11", "");
                        if (AnnexDelList.Count > 0)
                        {
                            foreach (var Annexitem in AnnexDelList)
                            {
                                if (!new Hi.BLL.YZT_Annex().Delete(Annexitem.ID)) throw new Exception("编辑附件失败！！！");
                            }
                        }



                    }
                    string contractDetail = item["orderdetail"].ToString();//订单明细
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(contractDetail);
                    //添加子表
                    if (ll.Count > 0 && !string.IsNullOrWhiteSpace(LibraryID))
                    {
                        Hi.Model.YZT_LibraryDetail LibraryDetaillModel = null;

                        string StockIDS = "";
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            StockIDS += StockIDS == "" ? items["tip"].ToString() : "," + items["tip"].ToString();
                        }
                        List<Hi.Model.YZT_GoodsStock> gslist1 = new Hi.BLL.YZT_GoodsStock().GetList("", " ID in(" + StockIDS + ")", "");
                        
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            LibraryDetaillModel = new Hi.Model.YZT_LibraryDetail();
                            LibraryDetaillModel.DisID = logUser.DisID;
                            LibraryDetaillModel.CompID = 0;
                            LibraryDetaillModel.LibraryID = Convert.ToInt32(LibraryID);
                            LibraryDetaillModel.GoodsCode = items["GoodsCode"].ToString();
                            LibraryDetaillModel.GoodsName = items["GoodsName"].ToString();
                            LibraryDetaillModel.ValueInfo = items["ValueInfo"].ToString();
                            LibraryDetaillModel.Unit = items["Unit"].ToString();
                            LibraryDetaillModel.StockID = Convert.ToInt32(items["tip"].ToString());
                            LibraryDetaillModel.BatchNO = items["BatchNO"].ToString();
                            LibraryDetaillModel.validDate = Convert.ToDateTime(items["validDate"].ToString());
                            LibraryDetaillModel.OutNum = Convert.ToDecimal(items["OutNum"].ToString());
                            LibraryDetaillModel.Remark = items["Remark"].ToString();
                            LibraryDetaillModel.AuditAmount = Convert.ToDecimal(items["AuditAmount"].ToString());
                            LibraryDetaillModel.sumAmount = Convert.ToDecimal(items["sumAmount"].ToString());
                            LibraryDetaillModel.ts = DateTime.Now;
                            LibraryDetaillModel.CreateDate = DateTime.Now;
                            LibraryDetaillModel.CreateUserID = logUser.UserID;
                            LibraryDetaillModel.modifyuser = logUser.UserID;

                            //占用库存
                            Hi.Model.YZT_GoodsStock GoodsStock1 = gslist1.Find(p => p.ID == Convert.ToInt32(items["tip"].ToString()));
                            GoodsStock1.StockUseNum += Convert.ToDecimal(items["OutNum"].ToString());
                            new Hi.BLL.YZT_GoodsStock().Update(GoodsStock1, tran);
                            
                            if (!(new Hi.BLL.YZT_LibraryDetail().Add(LibraryDetaillModel, tran) > 0))
                                throw new Exception("编辑失败！！！");
                        }
                    }

                    //添加附件
                    if (!string.IsNullOrWhiteSpace(HidFfileName))
                    {
                        string[] HidFfileNames = HidFfileName.Split(',');
                        Hi.Model.YZT_Annex annexModel = null;
                        foreach (var FfileNames in HidFfileNames)
                        {
                            annexModel = new Hi.Model.YZT_Annex();
                            DateTime time = DateTime.Now;
                            annexModel.fcID = Convert.ToInt32(LibraryID);
                            annexModel.type = 11;
                            annexModel.fileName = FfileNames;
                            annexModel.fileAlias = "5";
                            annexModel.validDate = time;
                            annexModel.CreateDate = time;
                            annexModel.dr = 0;
                            annexModel.ts = time;
                            annexModel.modifyuser = logUser.UserID;
                            annexModel.CreateUserID = logUser.UserID;
                            if (!(new Hi.BLL.YZT_Annex().Add(annexModel, tran) > 0)) throw new Exception("新增附件失败！！！");
                        }
                    }
                }
                tran.Commit();
                return "{\"returns\":\"true\",\"msg\":\"编辑成功\",\"LibraryID\":\"" + LibraryID + "\"}";
            }
            else
            {

                throw new Exception("未找到参数");
            }
        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
        }
    }

    /// <summary>
    /// 编辑合同
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string ContractEdit(HttpContext context)
    {

        SqlTransaction tran = null;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();

            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string ContrctID = "";
            if (l.Count > 0)
            {

                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string txtcontractNO = item["txtcontractNO"].ToString().Trim();//合同号
                    string txtcontractDate = item["txtcontractDate"].ToString().Trim();//日期
                    string DropDis = item["DropDis"].ToString().Trim();//代理商
                    string CState = item["CState"].ToString().Trim();//状态
                    string txtForceDate = Common.NoHTML(item["txtForceDate"].ToString().Trim());//生效日期
                    string txtInvalidDate = item["txtInvalidDate"].ToString().Trim();//失效日期
                    string txtRemark = item["txtRemark"].ToString().Trim();//备注
                    ContrctID = item["Cid"].ToString().Trim();//合同ID
                    string HidFfileName = item["HidFfileName"].ToString().Trim();//附件

                    if (string.IsNullOrWhiteSpace(ContrctID))
                    {
                        //添加
                        Hi.Model.YZT_Contract contractModel = new Hi.Model.YZT_Contract();
                        contractModel.contractNO = txtcontractNO;
                        contractModel.contractDate = Convert.ToDateTime(txtcontractDate);
                        contractModel.DisID = Convert.ToInt32(DropDis);
                        contractModel.CState = Convert.ToInt32(CState);
                        contractModel.ForceDate = Convert.ToDateTime(txtForceDate);
                        contractModel.InvalidDate = Convert.ToDateTime(txtInvalidDate);
                        contractModel.Remark = txtRemark;
                        contractModel.CreateDate = DateTime.Now;
                        contractModel.CreateUserID = logUser.UserID;
                        contractModel.dr = 0;
                        contractModel.ts = DateTime.Now;
                        contractModel.modifyuser = logUser.UserID;
                        contractModel.CompID = logUser.CompID;
                        ContrctID = new Hi.BLL.YZT_Contract().Add(contractModel, tran).ToString();
                        if (!(Convert.ToInt32(ContrctID) > 0)) throw new Exception("添加失败");

                    }
                    else
                    {
                        //修改
                        Hi.Model.YZT_Contract contractModel = new Hi.BLL.YZT_Contract().GetModel(Convert.ToInt32(ContrctID));
                        contractModel.contractNO = txtcontractNO;
                        contractModel.contractDate = Convert.ToDateTime(txtcontractDate);
                        contractModel.DisID = Convert.ToInt32(DropDis);
                        contractModel.CState = Convert.ToInt32(CState);
                        contractModel.ForceDate = Convert.ToDateTime(txtForceDate);
                        contractModel.InvalidDate = Convert.ToDateTime(txtInvalidDate);
                        contractModel.Remark = txtRemark;
                        contractModel.ts = DateTime.Now;
                        contractModel.modifyuser = logUser.UserID;
                        if (!new Hi.BLL.YZT_Contract().Update(contractModel))
                            throw new Exception("修改失败");

                        //删除从表
                        List<Hi.Model.YZT_ContractDetail> contractDetailList = new Hi.BLL.YZT_ContractDetail().GetList("", " dr=0 and ContID=" + ContrctID + "", "");
                        if (contractDetailList.Count > 0)
                        {
                            List<int> listID = new List<int>();
                            foreach (var itemDelete in contractDetailList)
                            {
                                listID.Add(itemDelete.ID);
                            }
                            new Hi.BLL.YZT_ContractDetail().Delete(listID, tran);
                        }

                        //删除附件
                        List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + ContrctID + " and fileAlias=3", "");
                        if (AnnexDelList.Count > 0)
                        {
                            foreach (var Annexitem in AnnexDelList)
                            {
                                if (!new Hi.BLL.YZT_Annex().Delete(Annexitem.ID)) throw new Exception("编辑附件失败！！！");
                            }
                        }

                    }

                    string contractDetail = item["orderdetail"].ToString();//订单明细
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(contractDetail);
                    //添加子表
                    if (ll.Count > 0 && !string.IsNullOrWhiteSpace(ContrctID))
                    {
                        Hi.Model.YZT_ContractDetail contractDetailModel = null;
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            contractDetailModel = new Hi.Model.YZT_ContractDetail();
                            contractDetailModel.ContID = Convert.ToInt32(ContrctID);
                            contractDetailModel.GoodsID = Convert.ToInt32(items["skuid"].ToString());
                            contractDetailModel.GoodsName = items["GoodsName"].ToString();
                            contractDetailModel.GoodsCode = items["GoodsCode"].ToString();
                            contractDetailModel.ValueInfo = items["ValueInfo"].ToString();
                            contractDetailModel.HtID = Convert.ToInt32(items["hid_Htid"].ToString());
                            contractDetailModel.AreaID = Convert.ToInt32(items["AreaID"].ToString());
                            contractDetailModel.SalePrice = Convert.ToDecimal(items["SalePrice"].ToString());
                            contractDetailModel.TinkerPrice = Convert.ToDecimal(items["txtTinkerPrice"].ToString());
                            contractDetailModel.discount = Convert.ToDecimal(items["discount"].ToString());
                            contractDetailModel.target = Convert.ToDecimal(items["target"].ToString());
                            contractDetailModel.Remark = items["remark"].ToString();
                            contractDetailModel.ts = DateTime.Now;
                            contractDetailModel.FCID = Convert.ToInt32(items["FirstCampID"].ToString());
                            contractDetailModel.CreateDate = DateTime.Now;
                            contractDetailModel.CreateUserID = logUser.UserID;
                            contractDetailModel.modifyuser = logUser.UserID;
                            if (!(new Hi.BLL.YZT_ContractDetail().Add(contractDetailModel, tran) > 0))
                                throw new Exception("编辑失败！！！");
                        }
                    }


                    //新增附件
                    if (!string.IsNullOrEmpty(HidFfileName))
                    {
                        string[] HidFfileNames = HidFfileName.Split(',');
                        Hi.Model.YZT_Annex annexModel = null;
                        foreach (var FfileNames in HidFfileNames)
                        {
                            annexModel = new Hi.Model.YZT_Annex();
                            DateTime time = DateTime.Now;
                            annexModel.fcID = Convert.ToInt32(ContrctID);
                            annexModel.type = 10;
                            annexModel.fileName = FfileNames;
                            annexModel.fileAlias = "3";
                            annexModel.validDate = time;
                            annexModel.CreateDate = time;
                            annexModel.dr = 0;
                            annexModel.ts = time;
                            annexModel.modifyuser = logUser.UserID;
                            annexModel.CreateUserID = logUser.UserID;
                            if (!(new Hi.BLL.YZT_Annex().Add(annexModel, tran) > 0)) throw new Exception("新增附件失败！！！");
                        }
                    }


                };
                tran.Commit();
                return "{\"returns\":\"true\",\"msg\":\"编辑成功\",\"ContrctID\":\"" + ContrctID + "\"}";
            }
            else
            {

                throw new Exception("未找到参数");
            }
        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
        }
    }

    public string ContractCheck(HttpContext context) {
        try
        {
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string contractDetail = item["orderdetail"].ToString();//订单明细
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(contractDetail);
                    //添加子表
                    if (ll.Count > 0)
                    {
                        foreach (Newtonsoft.Json.Linq.JObject items in ll)
                        {
                            List<Hi.Model.BD_GoodsInfo> infolist = new Hi.BLL.BD_GoodsInfo().GetList("", " ID=" + items["skuid"].ToString(), "");
                            if (infolist != null && infolist.Count > 0)
                            {
                                List<Hi.Model.BD_GoodsInfo> infolist1 = new Hi.BLL.BD_GoodsInfo().GetList("", " GoodsID=" + infolist[0].GoodsID, "");
                                if (infolist1 != null && infolist1.Count > 1)
                                {
                                    return "{\"returns\":\"true\",\"msg\":\"列表中存在多规格商品，是否需要添加其他规格的商品\"}";
                                }
                            }
                        }
                    }
                }
            
            }
            else
            {

                throw new Exception("未找到参数");
            }
        }
        catch (Exception e)
        {
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
        }
        return "{\"returns\":\"False\",\"msg\":\"\"}";
    }

    /// <summary>
    /// 商品出入库添加修改
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string EditStockInOut(HttpContext context)
    {
        SqlTransaction tran = null;
        int StockOrderID = 0;
        try
        {
            tran = SqlHelper.CreateStoreTranSaction();
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                throw new ApplicationException("请先登录");
            }
            string json = context.Request["json"] + "";//传过来的JS
            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    string NO = Common.NoHTML(item["no"].ToString().Trim());//单号
                    string remark = Common.NoHTML(item["remark"].ToString().Trim());//单据备注
                    string type = Common.NoHTML(item["type"].ToString().Trim());//单据类型
                    StockOrderID = Convert.ToInt32(item["OrderID"].ToString().Trim());//单据ID
                    if (StockOrderID != 0)//修改主表  删除对应的从表(重新添加从表)
                    {
                        //修改主表
                        Hi.Model.DIS_StockOrder StockOrder = new Hi.BLL.DIS_StockOrder().GetModel(StockOrderID);
                        StockOrder.StockType = type;
                        StockOrder.Remark = remark;
                        StockOrder.ts = DateTime.Now;
                        StockOrder.CreateUserID = logUser.UserID;
                        StockOrder.CreateDate = DateTime.Now;
                        if (StockOrder.State == 2)
                        {

                            return "{\"returns\":\"False\",\"msg\":\"单据已经审核 无法修改\"}";
                        }
                        if (!new Hi.BLL.DIS_StockOrder().Update(StockOrder, tran))
                        {
                            tran.Rollback();
                            return "{\"returns\":\"False\",\"msg\":\"修改失败\"}";
                        }

                        //删除从表
                        List<Hi.Model.DIS_StockInOut> list = new Hi.BLL.DIS_StockInOut().GetList("ID", " StockOrderID=" + StockOrderID + "", "");
                        if (list.Count > 0)
                        {
                            List<int> listID = new List<int>();
                            foreach (var StockInOut in list)
                            {
                                listID.Add(StockInOut.ID);
                            }
                            if (!new Hi.BLL.DIS_StockInOut().Delete(listID, tran))
                            {
                                tran.Rollback();
                                return "{\"returns\":\"False\",\"msg\":\"修改失败\"}";
                            }
                        }
                    }
                    else//添加主表
                    {
                        Hi.Model.DIS_StockOrder StockOrder = new Hi.Model.DIS_StockOrder();//商品出入库盘点主表
                        StockOrder.CompID = logUser.CompID;
                        StockOrder.OrderNO = GetNo(Convert.ToInt32(item["Type"].ToString().Trim()));
                        StockOrder.Type = Convert.ToInt32(item["Type"].ToString().Trim());//1入库 2出库 3 盘点
                        StockOrder.StockType = type;
                        StockOrder.ChkDate = DateTime.Now;
                        StockOrder.Remark = remark;
                        StockOrder.State = 0;//0制单  2审核
                        StockOrder.CreateUserID = logUser.UserID;
                        StockOrder.CreateDate = DateTime.Now;
                        StockOrder.ts = DateTime.Now;
                        StockOrder.dr = 0;
                        StockOrder.modifyuser = logUser.UserID;
                        StockOrderID = new Hi.BLL.DIS_StockOrder().Add(StockOrder, tran);
                    }
                    if (StockOrderID > 0)//无论添加 还是修改都会执行的方法
                    {
                        string orderDetail = item["orderdetail"].ToString();//订单明细
                        Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(orderDetail);
                        if (ll.Count > 0)
                        {
                            int ID = 0;
                            foreach (Newtonsoft.Json.Linq.JObject items in ll)
                            {
                                ID = 0;
                                //商品出入库明细表
                                Hi.Model.DIS_StockInOut StockInOut = new Hi.Model.DIS_StockInOut();
                                StockInOut.CompID = logUser.CompID;
                                StockInOut.StockOrderID = StockOrderID;
                                StockInOut.GoodsID = Convert.ToInt32(items["id"].ToString());
                                StockInOut.GoodsInfoID = Convert.ToInt32(items["skuid"].ToString());
                                string Where = " GoodsInfo='" + StockInOut.GoodsInfoID + "' and BatchNO='" + Common.NoHTML(items["BatchNO"].ToString()) + "'";
                                List<Hi.Model.DIS_GoodsStock> model = new Hi.BLL.DIS_GoodsStock().GetList("", Where, "");
                                if (model.Count > 0)
                                {
                                    if (Convert.ToInt32(item["Type"].ToString().Trim()) == 2)
                                    {
                                        if (model[0].StockNum < Convert.ToDecimal(items["goodsnum"].ToString()))
                                        {
                                            throw new ApplicationException("该批次" + Common.NoHTML(items["BatchNO"].ToString()) + "商品:'" + GetName(model[0].GoodsID) + "'的库存数量小于要出库的数量，请重新输入！");
                                        }
                                        else
                                        {
                                            StockInOut.StockNum = Convert.ToDecimal(items["goodsnum"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        StockInOut.StockNum = Convert.ToDecimal(items["goodsnum"].ToString());
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(item["Type"].ToString().Trim()) == 2)
                                    {
                                        throw new ApplicationException("该批次" + Common.NoHTML(items["BatchNO"].ToString()) + "商品:'" + GetName(model[0].GoodsID) + "'没有库存数量，请重新输入！"); 
                                    }
                                    StockInOut.StockNum = Convert.ToDecimal(items["goodsnum"].ToString());
                                }
                                StockInOut.Remark = Common.NoHTML(items["remark"].ToString());
                                StockInOut.CreateUserID = logUser.CompID;
                                StockInOut.CreateDate = DateTime.Now;
                                StockInOut.ts = DateTime.Now;
                                StockInOut.dr = 0;
                                StockInOut.modifyuser = logUser.CompID;
                                StockInOut.Batchno = Common.NoHTML(items["BatchNO"].ToString());
                                StockInOut.Validdate = items["validDate"].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(Common.NoHTML(items["validDate"].ToString()));
                                
                                ID = new Hi.BLL.DIS_StockInOut().Add(StockInOut, tran);
                                if (ID < 0)
                                {
                                    tran.Rollback();
                                    return "{\"returns\":\"False\",\"msg\":\"操作失败\"}";
                                }
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return "{\"returns\":\"False\",\"msg\":\"至少添加一条商品\"}";
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return "{\"returns\":\"False\",\"msg\":\"操作失败\"}";
                    }

                }
            }
        }
        catch (Exception e)
        {
            tran.Rollback();
            return "{\"returns\":\"False\",\"msg\":\"" + e.Message + "\"}";
            throw;
        }

        tran.Commit();
        return "{\"returns\":\"True\",\"no\":\"" + Common.DesEncrypt(StockOrderID.ToString(), Common.EncryptKey) + "\"}";
    }

    /// <summary>
    /// 提交订单 
    /// </summary>
    /// <returns></returns>
    public string insertOrder(HttpContext context)
    {

        DateTime dts = DateTime.Now;
        int KeyID = Convert.ToInt32(context.Request["keyid"]);
        string str = string.Empty;
        string sqlstr = string.Empty;
        string result = string.Empty;//返回信息
        string Inventory = string.Empty;//商品现有库存
        decimal TotalAmount = 0;//商品总价
        decimal PriceAmount = 0;//应付价
        int disId = 0;
        string pty = string.Empty;//没什么用
        string ppty = string.Empty;//商品促销type
        string proid2 = string.Empty;//商品促销id
        string proid = string.Empty;//订单促销id
        string proidd = string.Empty;//订单促销明细id
        string protype = string.Empty;//订单促销type
        decimal proAmount = 0;//返利金额
        string LogRemark = string.Empty;
        string json = context.Request["json"] + "";
        string goodsinfolistId = context.Request["goodsinfoId"] + "";
        string DisId = context.Request["DisId"];//代理商ID
        string compID = context.Request["compID"];
        string type3 = string.Empty;//是否购物车购买
        if (goodsinfolistId != "")
        {
            goodsinfolistId = Common.NoHTML(goodsinfolistId.Substring(0, goodsinfolistId.Length - 1));
        }
        string utype = Common.NoHTML(context.Request["utype"]) + "";//区分代理商下单和代客下单 2代理商 1代客
        Hi.Model.DIS_Order model = null;
        Hi.Model.DIS_OrderExt model2 = null;
        Hi.Model.DIS_OrderDetail model5 = null;
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {




            string IsAudit = OrderType.rdoOrderAudit("代客下单是否需要审核", compID.ToInt(0));

            Newtonsoft.Json.Linq.JArray l = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (l.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JObject item in l)
                {
                    disId = Convert.ToInt32(item["disid"].ToString().Trim());//代理商id;

                    int IsAudit2 = OrderType.OrderEnAudit((int)Enums.AddType.正常下单, disId, 0);
                    if (KeyID != 0)
                    {
                        string GinfoId = goodsinfolistId.Trim();//删除的goodsInfoId
                        if (!Util.IsEmpty(GinfoId))
                        {
                            //删除掉已删除的商品，数据库中存在该商品也删除掉
                            //string GinfoId = ViewState["Del"].ToString();
                            sqlstr = " DisID=" + disId + " and OrderID=" + KeyID + " and GoodsInfoID in (" + GinfoId + ")";
                        }
                    }
                    List<Hi.Model.DIS_OrderDetail> lll = new List<Hi.Model.DIS_OrderDetail>();
                    string orderDetail = item["orderdetail"].ToString();//订单明细
                    decimal bateamount = Convert.ToDecimal(item["bateamount"].ToString().Trim());//返利金额
                    decimal postfee = Convert.ToDecimal(item["postfee"].ToString().Trim());//运费
                    decimal cux = Convert.ToDecimal(item["cx"].ToString().Trim());//促销
                    type3 = Common.NoHTML(item["type3"].ToString().Trim());
                    Newtonsoft.Json.Linq.JArray ll = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(orderDetail);
                    if (ll.Count > 0)
                    {
                        int z = 0;
                        foreach (Newtonsoft.Json.Linq.JObject item2 in ll)
                        {
                            z++;
                            bool bol = false;
                            int goodsinfoid = Convert.ToInt32(item2["goodsinfoid"].ToString().Trim());//商品goodsinfoid;
                            decimal num = Convert.ToDecimal(item2["goodsnum"].ToString().Trim());//购买数量
                            decimal price2 = Convert.ToDecimal(item2["price2"].ToString().Trim());//原始保存的价格便于对比是否修改过价格
                            decimal price = Convert.ToDecimal(item2["price"].ToString().Trim());//下单价

                            Hi.Model.BD_GoodsInfo model3 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsinfoid);
                            Hi.Model.BD_Goods model4 = new Hi.BLL.BD_Goods().GetModel(model3.GoodsID);

                            List<Hi.Model.BD_GoodsAreas> goodsareaslist = new Hi.BLL.BD_GoodsAreas().GetList("", "isnull(dr,0)=0 and compid=" + compID.ToInt(0) + " and disid=" + disId + " and goodsid=" + model3.GoodsID, "");
                            if (goodsareaslist.Count > 0)
                            {
                                return "{\"ds\":\"False\",\"msg\":\"第" + z + "行" + " " + model3.BarCode + " " + model4.GoodsName + " " + model3.ValueInfo + " " + "商品不可售\"}";
                            }
                            // string disid = item2["disid"].ToString().Trim();//代理商id;
                            bol = BLL.Common.GetGoodsInfo(goodsinfoid, out result);
                            if (bol)
                            {
                                return "{\"ds\":\"False\",\"msg\":\"第" + z + "行" + result + "\"}";
                            }
                            decimal zxprice = BLL.Common.GetGoodsPrice(compID.ToInt(0), disId, goodsinfoid);//最新价格
                            if (price2 != zxprice)
                            {
                                //return "{\"ds\":\"False\",\"msg\":\"第" + z + "行商品价格有新的变动，请修改后再下单\"}";
                                return "{\"ds\":\"False\",\"msg\":\"第" + z + "行" + " " + model3.BarCode + " " + model4.GoodsName + " " + model3.ValueInfo + " " + "价格有新的变动，最新价格" + zxprice.ToString("N") + "，需先删除后重新添加\"}";
                            }

                            decimal ProNum = BLL.Common.GetProNum(model4.ID.ToString(), goodsinfoid.ToString(), compID.ToInt(0), num, out pty, out ppty, out proid2);

                            bol = BLL.Common.GetInevntory(compID.ToInt(0), goodsinfoid, context.Request["type"] + "" == "2" ? 0 : KeyID, (num + ProNum), out Inventory);
                            if (bol)
                            {
                                return "{\"ds\":\"False\",\"msg\":\"第" + z + "行" + " " + model3.BarCode + " " + model4.GoodsName + " " + model3.ValueInfo + " " + "库存不足，现有库存" + Inventory + "\"}";
                            }


                            string gsremark = Common.NoHTML(item2["remark"].ToString().Trim());//商品备注
                            decimal djPrice = price * num;
                            TotalAmount += djPrice;




                            if (KeyID != 0 && context.Request["type"] + "" != "2")
                            {
                                if (item2["id"].ToString().Trim() != "")
                                {
                                    model5 = new Hi.BLL.DIS_OrderDetail().GetModel(Convert.ToInt32(item2["id"].ToString().Trim()));
                                }
                                else
                                {
                                    model5 = new Hi.Model.DIS_OrderDetail();
                                    model5.DisID = disId;
                                    model5.GoodsinfoID = goodsinfoid;
                                }
                            }
                            else
                            {
                                model5 = new Hi.Model.DIS_OrderDetail();
                                model5.DisID = disId;
                                model5.GoodsinfoID = goodsinfoid;
                            }
                            model5.GoodsInfos = model3.ValueInfo;
                            model5.GoodsName = model4.GoodsName;
                            model5.GoodsCode = model3.BarCode;
                            model5.Unit = model4.Unit;
                            model5.ProID = proid2;
                            model5.ProNum = ProNum.ToString("0.00");
                            model5.Protype = ppty;
                            model5.GoodsPrice = price2;
                            model5.Remark = gsremark;
                            model5.Price = price;
                            model5.AuditAmount = price;
                            model5.GoodsNum = num;
                            model5.sumAmount = djPrice;

                            model5.ts = DateTime.Now;
                            model5.modifyuser = logUser.UserID;
                            lll.Add(model5);
                        }
                        decimal isprice = BLL.Common.GetProPrice(TotalAmount, out proid, out proidd, out protype, compID.ToInt(0));//判断是否可以使用促销金额
                        if (isprice != cux)
                        {
                            return "{\"ds\":\"False\",\"msg\":\"订单促销有新的变动，请修改后下单\"}";
                        }

                        bool bol2 = BLL.Common.GetRebate(compID.ToInt(0), disId, context.Request["type"] + "" == "2" ? 0 : KeyID, bateamount, out result);
                        if (bol2)
                        {
                            return "{\"ds\":\"False\",\"msg\":\"" + result + "\"}";
                        }
                        PriceAmount = TotalAmount;
                        proAmount = BLL.Common.GetProPrice(TotalAmount, out proid, out proidd, out protype, compID.ToInt(0));//促销金额
                        PriceAmount = TotalAmount - proAmount - bateamount + postfee;
                        if (PriceAmount < 0)
                        {
                            PriceAmount = 0;
                        }
                    }
                    else
                    {
                        return "{\"ds\":\"False\",\"msg\":\"无商品数据\"}";
                    }
                    //判断厂商是否有赊销额度
                    string MSg = string.IsNullOrWhiteSpace(DisId) ? "您的赊销额度不够！" : "该代理商的赊销额度不够！";
                    DisId = string.IsNullOrWhiteSpace(DisId) ? logUser.DisID.ToString() : DisId;
                    //List<Hi.Model.BD_Distributor> disList = new Hi.BLL.BD_Distributor().GetList("", "  CompID=" + logUser.CompID.ToString() + " and ID=" + DisId + "", "");
                    //if (disList.Count > 0)
                    //{
                    //    if (disList[0].CreditType != 0)
                    //    {
                    //        decimal GetSumAmount = OrderInfoType.GetSumAmount(DisId, logUser.CompID.ToString(), KeyID);
                    //        if ((GetSumAmount + TotalAmount) >= disList[0].CreditAmount)
                    //        {
                    //            return "{\"ds\":\"False\",\"msg\":\"" + MSg + " 赊销额度：" + disList[0].CreditAmount + ",已用额度：" + GetSumAmount + "\"}";
                    //        }
                    //    }
                    //}
                    decimal CreditAmount = 0;
                    if (BLL.Common.GetCredit(compID.ToInt(0), DisId.ToInt(0), out CreditAmount))
                    {
                        decimal GetSumAmount = OrderInfoType.GetSumAmount(DisId, compID.ToString(), KeyID);
                        if ((GetSumAmount + TotalAmount) >= CreditAmount)
                        {
                            return "{\"ds\":\"False\",\"msg\":\"" + MSg + " 赊销额度：" + CreditAmount + ",已用额度：" + GetSumAmount + "\"}";
                        }
                    }
                    //判断厂商是否有赊销额度 end



                    string time = Common.NoHTML(item["arrivedate"].ToString().Trim());//交货日期
                    DateTime dtime = time == "" ? DateTime.MinValue : time.ToDateTime();
                    string remark = Common.NoHTML(Common.NoHTML(item["remark"].ToString().Trim()));//订单备注
                    string atta = Common.NoHTML(item["atta"].ToString().Trim());//附件
                    string givemode = Common.NoHTML(item["givemode"].ToString().Trim());//配送方式
                    string addrid = Common.NoHTML(item["addrid"].ToString().Trim());//收货信息id
                    string principal = Common.NoHTML(item["principal"].ToString().Trim());//收货人
                    string phone = Common.NoHTML(item["phone"].ToString().Trim());//收货人手机
                    string address = Common.NoHTML(item["address"].ToString().Trim());//收货地址

                    string disaccid = Common.NoHTML(item["disaccid"].ToString().Trim());//开票id
                    string rise = Common.NoHTML(item["rise"].ToString().Trim());//开票抬头
                    string content = Common.NoHTML(item["content"].ToString().Trim());//开票类容
                    string obank = Common.NoHTML(item["obank"].ToString().Trim());//开户银行
                    string oaccount = Common.NoHTML(item["oaccount"].ToString().Trim());//开户帐号
                    string trnumber = Common.NoHTML(item["trnumber"].ToString().Trim());//登记号
                    string isobill = Common.NoHTML(item["isobill"].ToString().Trim());//是否开发票
                    DateTime dtss = item["ts"].ToString().Trim() != "" ? Convert.ToDateTime(item["ts"].ToString().Trim()) : DateTime.MinValue;//是否开发票
                                                                                                                                              ////////////////////////////订单表开始/////////////////////////////////////
                    if (KeyID != 0 && context.Request["type"] + "" != "2")
                    {
                        model = new Hi.BLL.DIS_Order().GetModel(KeyID);
                        List<Hi.Model.DIS_OrderExt> llll = new Hi.BLL.DIS_OrderExt().GetList("", "orderId=" + KeyID, "");
                        if (llll.Count > 0)
                        {
                            foreach (Hi.Model.DIS_OrderExt item6 in llll)
                            {
                                model2 = new Hi.BLL.DIS_OrderExt().GetModel(item6.ID);
                            }
                        }
                    }
                    else
                    {
                        model = new Hi.Model.DIS_Order();
                        model.CompID = compID.ToInt(0);
                        model.DisID = disId;
                        model.DisUserID = logUser.UserID;
                        model.AddType = utype == "1" ? 2 : 1;
                        model.Otype = 0;
                        int OState = (int)Enums.OrderState.已到货;//已到货
                        int oostate = new Hi.BLL.DIS_Order().OstateAudit(compID.ToInt(0));//订单完成节点设置
                        if (oostate != 1)
                        {
                            if (utype == "1")//代客下单
                            {
                                if (IsAudit == "0")
                                {
                                    if (oostate == 2)//审核完成
                                    {
                                        OState = (int)Enums.OrderState.已到货;//已到货
                                    }
                                    else
                                    {
                                        OState = (int)Enums.OrderState.已审;
                                    }
                                    model.IsAudit = 1;
                                    model.AuditDate = DateTime.Now;
                                }
                                else
                                {
                                    OState = (int)Enums.OrderState.待审核;
                                    model.IsAudit = 0;
                                    model.AuditDate = DateTime.MinValue;
                                }
                            }
                            else
                            {
                                if (IsAudit2 == 1)
                                {
                                    //无需审核
                                    if (oostate == 2)//审核完成
                                    {
                                        OState = (int)Enums.OrderState.已到货;//已到货
                                    }
                                    else
                                    {
                                        OState = (int)Enums.OrderState.已审;
                                    }
                                    model.IsAudit = 1;
                                    model.AuditDate = DateTime.Now;
                                }
                                else
                                {
                                    OState = (int)Enums.OrderState.待审核;
                                    model.IsAudit = 0;
                                    model.AuditDate = DateTime.MinValue;
                                }
                            }
                        }
                        else
                        {
                            model.IsAudit = 0;
                            model.AuditDate = DateTime.MinValue;
                        }
                        model.PayState = (int)Enums.PayState.未支付;
                        model.OState = OState;
                        model.ReceiptNo = SysCode.GetNewCode("销售单");
                        model.GUID = Guid.NewGuid().ToString().Replace("-", "");
                        model.CreateDate = DateTime.Now;
                        model.CreateUserID = logUser.UserID;
                        model.dr = 0;
                        model2 = new Hi.Model.DIS_OrderExt();
                    }
                    model.ArriveDate = dtime;
                    model.TotalAmount = TotalAmount;
                    model.AuditAmount = PriceAmount;
                    model.OtherAmount = 0;
                    model.Remark = remark;
                    model.Atta = atta;
                    model.ts = DateTime.Now;
                    model.modifyuser = logUser.UserID;
                    model.IsSettl = "0";
                    model.GiveMode = givemode;
                    model.PostFee = Convert.ToDecimal(postfee);
                    model.AddrID = Convert.ToInt32(addrid);
                    model.Principal = principal;
                    model.Phone = phone;
                    model.Address = address;
                    model.bateAmount = bateamount;
                    LogRemark += " 下单总价：" + PriceAmount.ToString("N");
                    if (ProID != "0")
                    {
                        LogRemark += " ,订单促销：" + proAmount.ToString("N");
                    }
                    ////////////////////////////订单表结束/////////////////////////////////////
                    ///////////////////////////扩展表开始//////////////////////////////////////
                    if (isobill != "0" && disaccid != "")
                    {
                        model2.DisAccID = disaccid;
                    }
                    model2.Rise = rise;
                    model2.Content = content;
                    model2.OBank = obank;
                    model2.OAccount = oaccount;
                    model2.TRNumber = trnumber;
                    model2.IsOBill = Convert.ToInt32(isobill);//== "0" ? 0 : 1;
                    model2.ProID = Convert.ToInt32(proid);
                    model2.ProAmount = proAmount;
                    model2.ProDID = Convert.ToInt32(proidd);
                    model2.Protype = protype;
                    ///////////////////////////扩展表结束//////////////////////////////////////
                    if (TotalAmount - proAmount + Convert.ToDecimal(postfee) < bateamount)
                    {
                        return "{\"ds\":\"False\",\"msg\":\"返利金额不能大于商品总额-促销+运费\"}";
                    }
                    int orderId = 0;
                    if (KeyID != 0 && context.Request["type"] + "" != "2")
                    {
                        //dts = ts.Value.Trim().ToDateTime();
                        orderId = OrderType.UpdateOrder(dtss, model, model2, lll, sqlstr);
                        if (utype == "1")
                        {
                            new Common().GetWxService("46", KeyID.ToString(), "0");
                        }
                        else
                        {
                            new Common().GetWxService("5", KeyID.ToString(), "1");
                        }
                    }
                    else
                    {
                        orderId = OrderType.TansOrder(model, model2, lll);
                        if (utype == "1")
                        {
                            new Common().GetWxService("41", orderId.ToString(), "0");
                        }
                        else
                        {
                            new Common().GetWxService("1", orderId.ToString(), "1");
                        }
                    }
                    if (orderId > 0)
                    {
                        //分摊
                        if ((utype == "1" && IsAudit == "0") || (utype != "1" && IsAudit2 == 1))
                        {
                            new Hi.BLL.DIS_OrderDetail().GetSharePrice(orderId, TotalAmount, PriceAmount);
                        }
                        if (type3 == "1")
                        {
                            //清空购物车
                            string str2 = " CompID=" + compID.ToInt(0) + " and DisID=" + logUser.DisID;
                            new Hi.BLL.DIS_ShopCart().CartEmpty(str2);
                        }
                        Utils.AddSysBusinessLog(compID.ToInt(0), "Order", KeyID == 0 || context.Request["type"] + "" == "2" ? orderId.ToString() : KeyID.ToString(), "订单新增", LogRemark);
                        string Id = Common.DesEncrypt(KeyID == 0 || context.Request["type"] + "" == "2" ? orderId.ToString() : KeyID.ToString(), Common.EncryptKey);
                        // context.Response.Redirect("orderdetail.aspx?KeyID=" + Id + "&top=0", false);
                        return "{\"ds\":\"True\",\"msg\":\"提交订单成功\",\"KeyID\":\"" + Id + "\"}";
                    }
                    else if (orderId == -1)
                    {
                        return "{\"ds\":\"False\",\"msg\":\"订单已修改，保存失败\"}";
                    }
                    else
                    {
                        return "{\"ds\":\"False\",\"msg\":\"提交订单失败\"}";
                    }
                }
                return "{\"ds\":\"False\",\"msg\":\"订单数据有误\"}";
            }
            else
            {
                return "{\"ds\":\"False\",\"msg\":\"订单数据有误\"}";
            }
        }
        else
        {
            return "{\"ds\":\"False\",\"msg\":\"用户未登录\"}";
        }
    }
    /// <summary>
    /// 彻底删除订单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string orderdelete(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        SqlTransaction tran = null;
        tran = SqlHelper.CreateStoreTranSaction();
        try
        {
            var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
            int orderID = oID.ToInt(0);

            Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));


            string DIS_LogisticsSql = string.Format(" delete from DIS_Logistics where OrderID='{0}'", orderID);
            string DIS_OrderReturnSql = string.Format(" delete from DIS_OrderReturn where OrderID='{0}'", orderID);
            string DIS_OrderOutDetailSql = string.Format(" delete from DIS_OrderOutDetail where OrderID='{0}'", orderID);
            string DIS_OrderOutSql = string.Format(" delete from DIS_OrderOut where OrderID='{0}'", orderID);
            string DIS_OrderDetailSql = string.Format(" delete from DIS_OrderDetail where OrderID='{0}'", orderID);
            string DIS_OrderExtSql = string.Format(" delete from DIS_OrderExt where OrderID='{0}'", orderID);
            string orderSql = string.Format(" delete from DIS_Order where ID='{0}'", orderID);


            int DIS_LogisticsCount = SqlHelper.ExecuteSqlTran(DIS_LogisticsSql, tran);
            if (DIS_LogisticsCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            int DIS_OrderReturnCount = SqlHelper.ExecuteSqlTran(DIS_OrderReturnSql, tran);
            if (DIS_OrderReturnCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }


            int DIS_OrderOutDetailCount = SqlHelper.ExecuteSqlTran(DIS_OrderOutDetailSql, tran);
            if (DIS_OrderOutDetailCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            int DIS_OrderOutCount = SqlHelper.ExecuteSqlTran(DIS_OrderOutSql, tran);
            if (DIS_OrderOutCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            int DIS_OrderDetailCount = SqlHelper.ExecuteSqlTran(DIS_OrderDetailSql, tran);
            if (DIS_OrderDetailCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            int DIS_OrderExtCount = SqlHelper.ExecuteSqlTran(DIS_OrderExtSql, tran);
            if (DIS_OrderDetailCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            int orderCount = SqlHelper.ExecuteSqlTran(orderSql, tran);
            if (orderCount < 0)
            {
                tran.Rollback();
                msg.Result = false;
                msg.Msg = "删除失败";
                return (new JavaScriptSerializer().Serialize(msg));
            }

            tran.Commit();
            msg.Result = true;
            msg.Msg = "删除成功！";
            return (new JavaScriptSerializer().Serialize(msg));
        }
        catch (Exception ex)
        {
            tran.Rollback();
            msg.Result = false;
            msg.Msg = ex.Message;
            return (new JavaScriptSerializer().Serialize(msg));
        }


    }

    /// <summary>
    /// 订单修改收货信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string orderUpaddr(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                string orderid = Common.DesDecrypt(context.Request["ID"] + "", Common.EncryptKey);
                string AddrID = Common.NoHTML(context.Request["AddrID"]) + "";
                string Principal = Common.NoHTML(context.Request["Principal"]) + "";
                string Phone = Common.NoHTML(context.Request["Phone"]) + "";
                string Address = Common.NoHTML(context.Request["Address"]) + "";

                Hi.BLL.DIS_Order odrerbll = new Hi.BLL.DIS_Order();

                Hi.Model.DIS_Order ordermdeol = odrerbll.GetModel(orderid.ToInt(0));

                if (ordermdeol != null)
                {
                    ordermdeol.AddrID = AddrID.ToInt(0);
                    ordermdeol.Principal = Principal;
                    ordermdeol.Phone = Phone;
                    ordermdeol.Address = Address;
                    ordermdeol.ts = DateTime.Now;
                    ordermdeol.modifyuser = logUser.UserID;

                    if (odrerbll.Update(ordermdeol))
                    {
                        msg.Result = true;
                        msg.Msg = "修改订单收货信息成功。";
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
    /// 修改订单信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string remarkview(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            var type = context.Request["type"];
            var remark = Common.NoHTML(context.Request["remark"]);
            var KeyID = context.Request["KeyID"];

            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                if (type == "0")
                {
                    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
                    int oID = Common.DesDecrypt(KeyID, Common.EncryptKey).ToInt(0);
                    Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(oID);
                    if (OrderModel != null)
                    {
                        OrderModel.Remark = remark;
                        OrderModel.ts = DateTime.Now;
                        if (OrderBll.Update(OrderModel))
                        {
                            msg.Result = true;
                            msg.Code = OrderModel.ts.ToString("yyyy-MM-dd HH:mm");
                        }
                    }
                    else
                    {
                        msg.Code = "未查找到数据";
                    }
                }
                else if (type == "1")
                {
                    Hi.BLL.DIS_OrderDetail OrderBllDetail = new Hi.BLL.DIS_OrderDetail();
                    Hi.Model.DIS_OrderDetail OrderModelDetail = OrderBllDetail.GetModel(KeyID.ToInt(0));
                    if (OrderModelDetail != null)
                    {
                        OrderModelDetail.Remark = remark;
                        OrderModelDetail.ts = DateTime.Now;
                        if (OrderBllDetail.Update(OrderModelDetail))
                        {
                            msg.Result = true;
                        }
                    }
                }
                else
                {
                    msg.Result = true;
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
    /// 订单修改开票信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string orderUpInvoi(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                string orderid = Common.DesDecrypt(context.Request["ID"] + "", Common.EncryptKey);
                string DisID = Common.NoHTML(context.Request["DisID"]) + "";
                string DisAccID = Common.NoHTML(context.Request["DisAccID"]) + "";
                string Rise = Common.NoHTML(context.Request["Rise"]) + "";
                string Content = Common.NoHTML(context.Request["Content"]) + "";
                string OBank = Common.NoHTML(context.Request["OBank"]) + "";
                string OAccount = Common.NoHTML(context.Request["OAccount"]) + "";
                string TRNumber = Common.NoHTML(context.Request["TRNumber"]) + "";
                string val = Common.NoHTML(context.Request["val"]) + "";

                Hi.Model.DIS_Order oModel = new Hi.BLL.DIS_Order().GetModel(orderid.ToInt(0));

                if (oModel != null)
                {
                    List<Hi.Model.DIS_OrderExt> extl = new Hi.BLL.DIS_OrderExt().GetList("", " OrderID=" + orderid, "");

                    if (extl != null && extl.Count > 0)
                    {
                        extl[0].IsOBill = val.ToInt(0);
                        if (val == "0")
                        {
                            extl[0].DisAccID = "0";
                            extl[0].Rise = "";
                            extl[0].Content = "";
                            extl[0].OBank = "";
                            extl[0].OAccount = "";
                            extl[0].TRNumber = "";
                        }
                        else
                        {
                            extl[0].DisAccID = DisAccID;
                            extl[0].Rise = Rise;
                            extl[0].Content = Content;
                            extl[0].OBank = OBank;
                            extl[0].OAccount = OAccount;
                            extl[0].TRNumber = TRNumber;
                        }
                        oModel.ts = DateTime.Now;
                        oModel.modifyuser = logUser.UserID;

                        if (new Hi.BLL.DIS_Order().Update(oModel) && new Hi.BLL.DIS_OrderExt().Update(extl[0]))
                            msg.Result = true;
                    }
                    else
                    {
                        msg.Msg = "订单信息有误。";
                        msg.Code = "Message";
                    }
                }
                else
                {
                    msg.Msg = "没有该订单信息。";
                    msg.Code = "Message";
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
    /// 新增开票信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string AddInvoi(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                string DisID = Common.NoHTML(context.Request["disID"]) + "";
                string Rise = Common.NoHTML(context.Request["Rise"]) + "";
                string Content = Common.NoHTML(context.Request["Content"]) + "";
                string OBank = Common.NoHTML(context.Request["OBank"]) + "";
                string OAccount = Common.NoHTML(context.Request["OAccount"]) + "";
                string TRNumber = Common.NoHTML(context.Request["TRNumber"]) + "";

                List<Hi.Model.BD_DisAccount> l = new Hi.BLL.BD_DisAccount().GetList("", "isnull(dr,0)=0 and DisID=" + DisID, "");

                if (l != null && l.Count > 0)
                {
                    msg.Msg = "代理商已存在开票信息。";
                }
                else
                {
                    Hi.Model.BD_DisAccount accountModel = new Hi.Model.BD_DisAccount();
                    accountModel.DisID = DisID.ToInt(0);
                    accountModel.Rise = Rise;
                    accountModel.Content = Content;
                    accountModel.OBank = OBank;
                    accountModel.OAccount = OAccount;
                    accountModel.TRNumber = TRNumber;
                    accountModel.CreateUserID = logUser.UserID;
                    accountModel.CreateDate = DateTime.Now;
                    accountModel.ts = DateTime.Now;
                    accountModel.modifyuser = logUser.UserID;
                    int ID = new Hi.BLL.BD_DisAccount().Add(accountModel);
                    if (ID > 0)
                    {
                        msg.Result = true;
                        msg.Code = ID.ToString();
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
    /// 订单签收
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string SignOrder(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                var OutID = Common.DesDecrypt(context.Request["outID"] + "", Common.EncryptKey);
                var oid = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var dts = context.Request["ts"] + "";
                var str = context.Request["str"] + "";

                Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(oid.ToInt(0));
                Hi.Model.DIS_OrderOut outmodel = new Hi.BLL.DIS_OrderOut().GetModel(OutID.ToInt(0));

                if (Convert.ToDateTime(dts).ToString("yyyy/MM/dd HH:mm:dd") == outmodel.ts.ToString("yyyy/MM/dd HH:mm:dd"))
                {
                    //是否全部发货 0、未完成发货  1、全部发货
                    int IsOutState = 0;

                    if (omodel != null && outmodel != null)
                    {
                        if (outmodel.IsSign == 0)
                        {
                            if (outmodel.IsAudit == 3)
                            {
                                msg.Msg = "订单已被其他人修改，请刷新后再重新操作！。";
                                msg.Code = "Error";
                                return (new JavaScriptSerializer().Serialize(msg));
                            }
                            if (omodel.OState == (int)Enums.OrderState.已发货 && (omodel.IsOutState == (int)Enums.IsOutState.全部发货 || omodel.IsOutState == (int)Enums.IsOutState.部分到货 || omodel.IsOutState == (int)Enums.IsOutState.部分发货))
                            {
                                //订单明细商品总数量
                                decimal goodsnum = 0; //订单商品总数
                                List<Hi.Model.DIS_OrderDetail> od = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID=" + oid, "");
                                if (od != null && od.Count > 0)
                                {
                                    foreach (var item in od)
                                    {
                                        goodsnum += item.GoodsNum;
                                        //有促销满送商品的加上促销赠送的商品数量
                                        if (item.ProNum.ToString() != "")
                                            goodsnum += item.ProNum.ToString().ToDecimal();
                                    }
                                }
                                //已签收的发货单列表
                                decimal signnum = 0;  //已签收商品数量
                                List<Hi.Model.DIS_OrderOut> lo = new Hi.BLL.DIS_OrderOut().GetList("", "isnull(IsSign,0)=1 and OrderID=" + oid, "");
                                if (lo != null && lo.Count > 0)
                                {
                                    var idstr = "";
                                    foreach (var item in lo)
                                    {
                                        idstr += item.ID + ",";
                                    }
                                    if (idstr.Length > 0)
                                        idstr = idstr.Substring(0, idstr.Length - 1);

                                    List<Hi.Model.DIS_OrderOutDetail> lod = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID in(" + idstr + ")", "");
                                    if (lod != null && lod.Count > 0)
                                    {
                                        foreach (var item in lod)
                                        {
                                            signnum += item.SignNum;
                                        }
                                    }
                                }

                                //本次签收商品修改
                                List<Hi.Model.DIS_OrderOutDetail> nlodd = new List<Hi.Model.DIS_OrderOutDetail>();
                                //本次签收商品数量
                                List<Hi.Model.DIS_OrderOutDetail> lodd = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID=" + OutID, "");

                                var gl = str.Split(new string[] { "；" }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < gl.Length; i++)
                                {
                                    var ggl = gl[i].Split(new string[] { "：" }, StringSplitOptions.RemoveEmptyEntries);
                                }

                                if (lodd != null && lodd.Count > 0)
                                {
                                    foreach (var item in lodd)
                                    {
                                        signnum += item.OutNum;
                                        item.SignNum = item.OutNum;

                                        // 这里的1就是你要查找的值
                                        if (Array.IndexOf(gl, item.ID.ToString()) != -1)
                                        {
                                            var ggl = gl[Array.IndexOf(gl, item.ID.ToString())].Split(new string[] { "：" }, StringSplitOptions.RemoveEmptyEntries);

                                            item.Batchno = ggl[1].ToString();
                                            item.Validdate = ggl[2].ToString() == "" ? DateTime.MinValue : Convert.ToDateTime(ggl[2].ToString());
                                        }

                                        nlodd.Add(item);
                                    }
                                }

                                //订单
                                //到货签收修改订单状态
                                if (omodel.IsOutState == (int)Enums.IsOutState.全部发货)
                                {
                                    //签收数量等于订单商品数量 ==全部到货
                                    if (signnum == goodsnum)
                                    {
                                        IsOutState = 1;
                                        omodel.IsOutState = (int)Enums.IsOutState.全部到货;
                                        omodel.OState = (int)Enums.OrderState.已到货;
                                    }
                                    else
                                    {
                                        omodel.IsOutState = (int)Enums.IsOutState.全部发货;
                                        omodel.OState = (int)Enums.OrderState.已发货;
                                    }
                                }
                                else if (omodel.IsOutState == (int)Enums.IsOutState.部分发货 || omodel.IsOutState == (int)Enums.IsOutState.部分到货)
                                {
                                    omodel.IsOutState = (int)Enums.IsOutState.部分到货;
                                    omodel.OState = (int)Enums.OrderState.已发货;
                                }

                                omodel.ts = DateTime.Now;
                                omodel.modifyuser = logUser.UserID;

                                //发货单
                                //修改为签收状态
                                outmodel.IsSign = 1;
                                outmodel.SignDate = DateTime.Now;
                                outmodel.SignUserId = logUser.UserID;
                                outmodel.SignUser = logUser.UserName;
                                outmodel.ts = DateTime.Now;
                                outmodel.modifyuser = logUser.UserID;

                                SqlTransaction TranSaction = null;
                                SqlConnection Connection = new SqlConnection(LocalSqlServer);
                                Connection.Open();
                                TranSaction = Connection.BeginTransaction();

                                //修改订单
                                bool res = new Hi.BLL.DIS_Order().Update(omodel, TranSaction);
                                if (!res)
                                {
                                    TranSaction.Rollback();
                                    msg.Msg = "订单信息有误。";
                                    msg.Code = "Error";
                                    return (new JavaScriptSerializer().Serialize(msg));
                                }
                                //修改发货
                                //向发货表中添加到货信息
                                bool count = new Hi.BLL.DIS_OrderOut().Update(outmodel, TranSaction);
                                if (!count)
                                {
                                    TranSaction.Rollback();
                                    msg.Msg = "订单信息有误。";
                                    msg.Code = "Error";
                                    return (new JavaScriptSerializer().Serialize(msg));
                                }

                                if (nlodd != null && nlodd.Count > 0)
                                {
                                    foreach (var item in nlodd)
                                    {
                                        int conutd = new Hi.BLL.DIS_OrderOutDetail().Update(item, TranSaction);
                                        if (conutd <= 0)
                                        {
                                            TranSaction.Rollback();
                                            msg.Msg = "订单信息有误。";
                                            msg.Code = "Error";
                                            return (new JavaScriptSerializer().Serialize(msg));
                                        }
                                    }
                                }
                                else
                                {
                                    TranSaction.Rollback();
                                    msg.Msg = "订单信息有误。";
                                    msg.Code = "Error";
                                    return (new JavaScriptSerializer().Serialize(msg));
                                }

                                TranSaction.Commit();

                                //签收成功
                                msg.Result = true;
                                msg.Rvlue = IsOutState.ToString();
                                msg.Code = omodel.OState.ToString();
                                msg.Rdate = outmodel.SignDate.ToString("yyyy-MM-dd HH:mm");

                                //订单日志
                                Utils.AddSysBusinessLog(omodel.CompID, "Order", oid, "订单签收", "");
                                //订单推送
                                new Common().GetWxService("3", oid, "1");
                            }
                            else
                            {
                                msg.Msg = "订单处理中，请稍后再重新操作。";
                                msg.Code = "Error";
                            }
                        }
                        else
                        {
                            msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                            msg.Code = "Error";
                        }
                    }
                    else
                    {
                        msg.Msg = "订单信息有误。";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                    msg.Code = "Error";
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
    /// 再次购买
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string BuyOrder(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser != null)
            {
                int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", logUser.CompID).ToInt(0);
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                decimal TotalAmount = 0;

                //查询订单
                string where = "and isnull(o.dr,0)=0 and o.otype<>9 and o.DisID=" + context.Request["DisID"] + " and o.CompID=" + logUser.CompID + " and o.ID= " + oID;
                DataTable dt = new Hi.BLL.DIS_Order().GetList("", where);

                //订单所有商品明细
                DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + oID);

                if (dt != null && dt.Rows.Count > 0 && l != null && l.Rows.Count > 0)
                {
                    /**************************** 订单明细 start ***********************************/
                    List<Hi.Model.DIS_OrderDetail> dl = new List<Hi.Model.DIS_OrderDetail>();
                    //订单明细
                    Hi.Model.DIS_OrderDetail odmodel = null;

                    foreach (DataRow item in l.Rows)
                    {
                        odmodel = new Hi.Model.DIS_OrderDetail();

                        Hi.Model.BD_GoodsInfo infoModel = new Hi.BLL.BD_GoodsInfo().GetModel(item["GoodsinfoID"].ToString().ToInt(0));
                        if (infoModel.IsOffline == 0 || infoModel.dr == 1)
                        {
                            msg.Msg = "商品：" + SelectGoods.GoodsName(infoModel.GoodsID, "GoodsName") + "已下架或删除。<br>";
                            return (new JavaScriptSerializer().Serialize(msg));
                        }

                        if (IsInve == 0)
                        {
                            //判断商品库存
                            decimal Ine = Hi.BLL.DIS_OrderDetail.GetInevntory(item["GoodsinfoID"].ToString().ToInt(0), 0, item["GoodsNum"].ToString().ToDecimal(0));
                            if (Ine <= 0)
                            {
                                msg.Msg = "商品：" + SelectGoods.GoodsName(infoModel.GoodsID, "GoodsName") + "，库存不足，请修改";
                                return (new JavaScriptSerializer().Serialize(msg));
                            }
                        }
                        odmodel.GoodsinfoID = item["GoodsinfoID"].ToString().ToInt(0);
                        odmodel.DisID = item["DisID"].ToString().ToInt(0);
                        odmodel.GoodsCode = infoModel.BarCode;
                        odmodel.GoodsInfos = infoModel.ValueInfo;
                        odmodel.GoodsName = Common.NoHTML(item["GoodsName"].ToString());
                        odmodel.IsOut = 0;
                        odmodel.OutNum = 0;
                        odmodel.Unit = item["Unit"].ToString();
                        odmodel.Remark = Common.NoHTML(item["Remark"].ToString());

                        //获取商品最新价格
                        decimal Price = SelectGoods.GoodsNewPrice(item["GoodsinfoID"].ToString().ToInt(0), logUser.DisID, logUser.CompID);
                        odmodel.GoodsPrice = infoModel.TinkerPrice;
                        odmodel.Price = Price;
                        odmodel.AuditAmount = Price;
                        odmodel.GoodsNum = item["GoodsNum"].ToString().ToDecimal(0);
                        //该商品的总价
                        odmodel.sumAmount = Price * item["GoodsNum"].ToString().ToDecimal(0);

                        //订单总价
                        TotalAmount += item["sumAmount"].ToString().ToDecimal(0);

                        string pty = "";
                        string ppty = "";
                        int num = Common.GetProNum(infoModel.GoodsID.ToString(), item["GoodsinfoID"].ToString(), logUser.CompID, item["GoodsNum"].ToString(), out pty, out ppty); //促销商品数量 
                        int odProID = Common.GetPro(infoModel.GoodsID.ToString(), item["GoodsinfoID"].ToString(), logUser.CompID.ToString());

                        odmodel.ProID = odProID.ToString();  //是否促销商品 
                        if (pty == "3" && num <= 0)
                        {
                            odmodel.ProID = "0";
                        }
                        odmodel.ProNum = num.ToString();
                        odmodel.Protype = ppty;

                        odmodel.ts = DateTime.Now;
                        odmodel.dr = 0;
                        odmodel.modifyuser = 0;

                        dl.Add(odmodel);
                    }
                    /**************************** 订单明细 end ***********************************/

                    /**************************** 订单主表 start ***********************************/

                    //订单主表
                    Hi.Model.DIS_Order omodel = null;
                    //订单扩展表
                    Hi.Model.DIS_OrderExt oemodel = null;

                    foreach (DataRow item in dt.Rows)
                    {
                        omodel = new Hi.Model.DIS_Order();
                        oemodel = new Hi.Model.DIS_OrderExt();

                        String guid = Guid.NewGuid().ToString().Replace("-", "");
                        omodel.GUID = guid;
                        omodel.ReceiptNo = SysCode.GetNewCode("销售单");

                        omodel.CompID = logUser.CompID;
                        omodel.DisID = item["DisID"].ToString().ToInt(0);
                        omodel.DisUserID = item["DisUserID"].ToString().ToInt(0);
                        omodel.AddType = item["AddType"].ToString().ToInt(0);
                        omodel.Otype = item["Otype"].ToString().ToInt(0);

                        //订单状态
                        int Audit = OrderInfoType.OrderEnAudit((int)Enums.AddType.正常下单, omodel.DisID, omodel.Otype);
                        omodel.IsAudit = Audit;

                        int OState = 1;
                        if (Audit == 1)
                        {
                            //订单应付金客为0是，审核时，支付状态修改为已支付
                            if (omodel.AuditAmount <= 0)
                                omodel.PayState = (int)Enums.PayState.已支付;
                            //无需审核
                            omodel.IsAudit = 1;
                            OState = 2;
                        }
                        else
                        {
                            OState = 1;
                            omodel.IsAudit = 0;
                        }
                        omodel.OState = OState;
                        //订单支付状态
                        omodel.PayState = (int)Enums.PayState.未支付;

                        //收货地址
                        Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(item["AddrID"].ToString().ToInt(0));
                        if (DisAddr != null)
                        {
                            omodel.AddrID = DisAddr.ID;
                            omodel.Principal = DisAddr.Principal;
                            omodel.Phone = DisAddr.Phone;
                            omodel.Address = DisAddr.Address;
                        }
                        omodel.IsSettl = "0";
                        omodel.ReturnState = 0;//退货状态
                        omodel.ArriveDate = DateTime.MinValue; //发货日期
                        omodel.DisUserID = logUser.UserID;
                        omodel.CreateUserID = logUser.UserID;
                        omodel.CreateDate = DateTime.Now;

                        omodel.AuditUserID = 0;
                        omodel.AuditDate = DateTime.MinValue;
                        omodel.AuditRemark = "";
                        omodel.ReturnMoneyDate = DateTime.MinValue;
                        omodel.ReturnMoneyUserId = 0;
                        omodel.ReturnMoneyUser = "";
                        omodel.ts = DateTime.Now;
                        omodel.dr = 0;
                        omodel.modifyuser = 0;

                        omodel.bateAmount = 0;  //返利
                        omodel.IsPayColl = "0";//收款补录标示（0，未补录，1，已补录）
                        omodel.PostFee = 0;  //运费
                        omodel.CostSub = item["CostSub"].ToString();
                        omodel.Remark = "";
                        omodel.GiveMode = item["GiveMode"].ToString();//配送方式

                        //订单金额
                        string ProID = "0";
                        string ProIDD = "0";
                        string ProType = "";

                        //订单促销
                        decimal ProAmount = Common.GetProPrice(TotalAmount, out ProID, out ProIDD, out ProType, logUser.CompID);
                        decimal AuditAmount = (TotalAmount - ProAmount);

                        omodel.PayedAmount = 0;//支付金额
                        omodel.TotalAmount = TotalAmount; //总价
                        omodel.AuditAmount = AuditAmount; //审核后总价
                        omodel.OtherAmount = 0;

                        //订单促销
                        oemodel.ProID = ProID.ToInt(0);
                        oemodel.ProDID = ProIDD.ToInt(0);
                        oemodel.ProAmount = ProAmount;
                        oemodel.Protype = ProType;

                        //发票信息
                        oemodel.IsOBill = item["IsOBill"].ToString().ToInt(0);
                        oemodel.BillNo = "";
                        oemodel.IsBill = oemodel.IsBill;

                        //开票信息
                        oemodel.DisAccID = Common.NoHTML(item["DisAccID"].ToString());
                        oemodel.Rise = Common.NoHTML(item["Rise"].ToString());
                        oemodel.Content = Common.NoHTML(item["Content"].ToString());
                        oemodel.OBank = Common.NoHTML(item["OBank"].ToString());
                        oemodel.OAccount = Common.NoHTML(item["OAccount"].ToString());
                        oemodel.TRNumber = Common.NoHTML(item["TRNumber"].ToString());
                    }
                    /**************************** 订单主表 end ***********************************/
                    int orderId = OrderType.TansOrder(omodel, oemodel, dl);
                    if (orderId > 0)
                    {
                        msg.Result = true;
                        msg.Code = Common.DesEncrypt(orderId.ToString(), Common.EncryptKey);
                    }
                    else
                        msg.Msg = "再次购买失败。";
                }
                else
                {
                    msg.Msg = "订单信息有误。";
                    msg.Code = "Error";
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
    /// 作废订单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string CancelOrder(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var dts = context.Request["ts"] + "";

                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));

                if (OrderModel != null)
                {
                    //判断订单是否可修改
                    if (Convert.ToDateTime(dts) == OrderModel.ts)
                    {
                        //if (new Hi.BLL.DIS_Order().Getts("Dis_Order", OrderModel.ID, Convert.ToDateTime(dts)) == 1)
                        //{
                        //if ((OrderModel.OState == (int)Enums.OrderState.未提交 || OrderModel.OState == (int)Enums.OrderState.已审 || OrderModel.OState == (int)Enums.OrderState.待审核) && OrderModel.PayState == (int)Enums.PayState.未支付)
                        //{

                        //已发货的做作废发货单
                        List<Hi.Model.DIS_OrderOut> outl = new Hi.BLL.DIS_OrderOut().GetList("", " OrderID=" + oID + " and isnull(dr,0)=0 and isnull(IsAudit,0)<>3", "");
                        string sql = "";
                        if (outl != null && outl.Count > 0)
                        {
                            foreach (var item in outl)
                            {
                                sql += " update [DIS_OrderOut] set [IsAudit]=3,[ts]='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID=" + item.ID + ";";
                            }
                        }

                        sql += " update [DIS_Order] set [OState]=" + (int)Enums.OrderState.已作废 + ",[ts]='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID=" + oID + ";";

                        if (OrderInfoType.rdoOrderAudit("商品是否启用库存", OrderModel.CompID) == "0")
                            //作废订单，返还商品库存
                            sql += new Hi.BLL.DIS_Order().GetSqlAddInve(oID.ToInt(0), null, 0);

                        if (new Hi.BLL.DIS_Order().UpdateOrderState(sql))
                        {
                            //作废订单，返回返利
                            new Hi.BLL.BD_Rebate().TransCancel(OrderModel.DisID, OrderModel.ID, logUser.UserID);
                            Utils.AddSysBusinessLog(OrderModel.CompID, "Order", oID, "订单作废", "");

                            msg.Result = true;
                        }
                        //}
                        //else
                        //{
                        //    msg.Msg = "订单处理中,不能作废订单！";
                        //    msg.Code = "Error";
                        //}
                    }
                    else
                    {
                        msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单信息有误。";
                    msg.Code = "Error";
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
    /// 0、应付总额  1、修改运费
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Amountof(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                //订单ID
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                //0、应付总额  1、修改运费
                var type = context.Request["type"] + "";
                //金额
                var tatol = context.Request["tatol"] + "";
                //原有金额
                var t = context.Request["t"] + "";

                var dts = context.Request["ts"] + "";

                //修改运费
                Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));

                List<Hi.Model.DIS_OrderExt> extl = new Hi.BLL.DIS_OrderExt().GetList("", " OrderID=" + oID, "");

                if (ordermodel != null)
                {
                    if (ordermodel.OState <= (int)Enums.OrderState.已审)
                    {
                        if (type.ToInt(0) == 0)
                        {
                            //if (ordermodel.PostFee > tatol.ToDecimal(0))
                            //{
                            //    msg.Msg = "运费不能大于订单应付金额。";
                            //    return (new JavaScriptSerializer().Serialize(msg));
                            //}
                            decimal allPrice = tatol.ToDecimal(0); //修改的订单应付金额

                            #region
                            //修改应付总额、分摊到商品单价后
                            //List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + oID, "");
                            //List<Hi.Model.DIS_OrderDetail> ll = new List<Hi.Model.DIS_OrderDetail>();
                            //if (l != null && l.Count > 0)
                            //{
                            //    //订单商品个数
                            //    int ldline = l.Count;
                            //    decimal oPrice = allPrice / ldline;
                            //    decimal oline = 0;
                            //    int i = 0;
                            //    foreach (Hi.Model.DIS_OrderDetail item in l)
                            //    {
                            //        i++;

                            //        if (i == ldline)
                            //        {
                            //            //最后一个商品价格
                            //            oPrice = allPrice - oline;
                            //            //最后一个商品分摊后商品单价
                            //            item.Price = oPrice;
                            //            //最后一个商品分摊后单个商品总价
                            //            //item.sumAmount = oPrice * item.GoodsNum;
                            //        }
                            //        else
                            //        {
                            //            //Floor 返回小于或等于指定的取大整数
                            //            oline += Math.Floor(oPrice);
                            //            //分摊后商品单价
                            //            item.Price = Math.Floor(oPrice);
                            //            //分摊后单个商品总价
                            //            //item.sumAmount = oPrice * item.GoodsNum;
                            //        }
                            //        item.modifyuser = logUser.UserID;
                            //        item.ts = DateTime.Now;

                            //        ll.Add(item);
                            //    }
                            //}
                            //else
                            //{
                            //    msg.Result = false;
                            //    msg.Msg = "订单有误。";
                            //    msg.Code = "Error";
                            //}
                            #endregion
                            if (Convert.ToDateTime(dts) == ordermodel.ts)
                            {
                                ordermodel.AuditAmount = allPrice;
                                ordermodel.modifyuser = logUser.UserID;
                                ordermodel.ts = DateTime.Now;

                                //int result = OrderType.UpdateOrder(Convert.ToDateTime(dts), ordermodel, null, ll, "");
                                //if (result > 0)
                                //{
                                //    msg.Result = true;
                                //}
                                //else
                                //    msg.Msg = "修改应付金额失败。";

                                if (new Hi.BLL.DIS_Order().Update(ordermodel))
                                    msg.Result = true;
                            }
                            else
                            {
                                msg.Result = false;
                                msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                                msg.Code = "Error";
                            }

                        }
                        else if (type.ToInt(0) == 1)
                        {
                            if (Convert.ToDateTime(dts) == ordermodel.ts)
                            {
                                //if (ordermodel.AuditAmount < tatol.ToDecimal(0))
                                //{
                                //    msg.Msg = "运费不能大于订单应付金额。";
                                //    return (new JavaScriptSerializer().Serialize(msg));
                                //}

                                //修改订单运费
                                //decimal fee = ordermodel.PostFee - tatol.ToDecimal(0);
                                decimal AuditAmount = ordermodel.AuditAmount - ordermodel.PostFee;
                                AuditAmount += tatol.ToDecimal(0);

                                if (AuditAmount > 0)
                                    ordermodel.AuditAmount = AuditAmount;
                                else
                                    ordermodel.AuditAmount = 0;

                                ordermodel.PostFee = tatol.ToDecimal(0);
                                ordermodel.modifyuser = logUser.UserID;
                                ordermodel.ts = DateTime.Now;

                                //if (OrderInfoType.rdoOrderAudit("订单支付返利是否启用", ordermodel.CompID) == "1")
                                //{
                                //    //返利小于商品总额+运费
                                //    decimal price = (ordermodel.TotalAmount - extl[0].ProAmount.ToString().ToDecimal(0)) + ordermodel.PostFee;
                                //    if (price < ordermodel.bateAmount)
                                //    {
                                //        msg.Result = false;
                                //        msg.Msg = "商品总额和运费之和应大于使用的返利";
                                //        msg.Code = "Error";
                                //        return (new JavaScriptSerializer().Serialize(msg));
                                //    }

                                //    //修改运费时
                                //    //商品总额-促销金额+运费-应付金额>=返利
                                //    decimal amount = (ordermodel.TotalAmount - extl[0].ProAmount.ToString().ToDecimal(0)) + ordermodel.PostFee - ordermodel.AuditAmount;

                                //    if (amount < ordermodel.bateAmount)
                                //    {
                                //        msg.Result = false;
                                //        msg.Msg = "运费不能小于使用的返利";
                                //        msg.Code = "Error";
                                //        return (new JavaScriptSerializer().Serialize(msg));
                                //    }
                                //}
                                if (new Hi.BLL.DIS_Order().Update(ordermodel))
                                    msg.Result = true;
                            }
                            else
                            {
                                msg.Result = false;
                                msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                                msg.Code = "Error";
                            }
                        }
                    }
                    else
                    {
                        msg.Result = false;
                        msg.Msg = "订单处理中,请稍后再操作！";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "订单信息有误。";
                    msg.Code = "Error";
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
    /// 订单审核 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string orderAudit(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var dts = context.Request["ts"] + "";

                Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));

                //判断订单是否可修改
                if (Convert.ToDateTime(dts) == omodel.ts)
                {
                    if (omodel != null && omodel.OState == (int)Enums.OrderState.待审核)
                    {
                        string AuditRemark = string.Empty;

                        //订单应付金客为0是，审核时，支付状态修改为已支付
                        if (omodel.AuditAmount <= 0)
                            omodel.PayState = (int)Enums.PayState.已支付;

                        AuditRemark = "审核通过";

                        omodel.AuditUserID = logUser.UserID;
                        omodel.AuditDate = DateTime.Now;
                        omodel.AuditRemark = "";

                        //订单完成节点设置判断 1、提交完成  2、审核完成  3、发货完成 
                        if (new Hi.BLL.DIS_Order().OstateAudit(logUser.CompID) == 2)
                            omodel.OState = (int)Enums.OrderState.已到货;
                        else
                            omodel.OState = (int)Enums.OrderState.已审;

                        omodel.ts = DateTime.Now;
                        omodel.modifyuser = logUser.UserID;

                        AuditRemark += "   审核总价：" + omodel.AuditAmount.ToString("N");

                        if (new Hi.BLL.DIS_Order().Update(omodel))
                        {
                            //审核后分摊商品价格
                            new Hi.BLL.DIS_OrderDetail().GetSharePrice(oID.ToInt(0), omodel.TotalAmount, omodel.AuditAmount);

                            Utils.AddSysBusinessLog(logUser.CompID, "Order", oID.ToString(), "审核通过", AuditRemark);
                            // type : "4":订单发货通知;"3":订单状态变更通知(待发货、审批通过);"2":订单支付通知;"1":下单通知
                            //string JSon="{"type":"1","userID":"1027","orderID":"1030"}";
                            new Common().GetWxService("42", oID.ToString(), "0");

                            msg.Result = true;
                            msg.Code = "2";
                        }
                    }
                    else
                    {
                        msg.Msg = "订单处理中，请稍后再重新操作！";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                    msg.Code = "Error";
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
    /// 订单发货
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string outOrder(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var str = Common.NoHTML(context.Request["str"]) + "";
                var ComPName = Common.NoHTML(context.Request["ComPName"]) + "";
                var LogisticsNo = Common.NoHTML(context.Request["LogisticsNo"]) + "";
                var CarUser = Common.NoHTML(context.Request["CarUser"]) + "";
                var CarNo = Common.NoHTML(context.Request["CarNo"]) + "";
                var Car = Common.NoHTML(context.Request["Car"]) + "";
                var date = Common.NoHTML(context.Request["date"]) + "";
                var dts = Common.NoHTML(context.Request["ts"]) + "";

                Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));

                int Sd = 0;
                if (Convert.ToDateTime(dts) == omodel.ts)
                {
                    if (omodel != null && ((omodel.OState == (int)Enums.OrderState.已发货 && (omodel.IsOutState == 0 || omodel.IsOutState == (int)Enums.IsOutState.部分到货 || omodel.IsOutState == (int)Enums.IsOutState.部分发货)) || omodel.OState == (int)Enums.OrderState.已审))
                    {
                        //查出订单明细
                        List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", "isnull(dr,0)=0 and OrderID=" + oID, "");
                        //订单明细
                        List<Hi.Model.DIS_OrderDetail> ll = new List<Hi.Model.DIS_OrderDetail>();
                        //发货明细
                        List<Hi.Model.DIS_OrderOutDetail> llo = new List<Hi.Model.DIS_OrderOutDetail>();
                        Hi.Model.DIS_OrderOutDetail outdmodel = null;
                        
                        //出库明细
                        List<Hi.Model.DIS_StockInOut> llinOut = new List<Hi.Model.DIS_StockInOut>();
                        Hi.Model.DIS_StockInOut inoutModel = null;

                        if (l != null && l.Count > 0)
                        {
                            var gl = str.Split(new string[] { "；" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < gl.Length; i++)
                            {
                                outdmodel = new Hi.Model.DIS_OrderOutDetail();
                                inoutModel = new Hi.Model.DIS_StockInOut();
                                    
                                decimal num = 0;
                                var ggl = gl[i].Split(new string[] { "：" }, StringSplitOptions.None);

                                if (ggl[1].ToDecimal(0) <= 0)
                                    continue;

                                Hi.Model.DIS_OrderDetail od = l.Find(p => p.ID == ggl[0].ToInt(0));

                                if (od.GoodsNum + od.ProNum.ToDecimal(0) - od.OutNum > 0)
                                {
                                    num = od.GoodsNum + od.ProNum.ToDecimal(0) - (od.OutNum + ggl[1].ToDecimal(0));
                                    if (num == 0)
                                    {
                                        //商品完成发货
                                        od.OutNum = od.GoodsNum + od.ProNum.ToDecimal(0);
                                        od.IsOut = 1;
                                    }
                                    else
                                        //商品未完成发货
                                        od.OutNum = (od.OutNum + ggl[1].ToDecimal(0));

                                    ll.Add(od);

                                    //发货明细赋值
                                    outdmodel.OrderID = omodel.ID;
                                    outdmodel.DisID = omodel.DisID;
                                    outdmodel.GoodsinfoID = od.GoodsinfoID;
                                    decimal StockNum = 0;
                                    //判断发货商品库存信息 
                                    if (SelectGoodsInfo.GetIsStock(od.GoodsinfoID.ToString(), ggl[2].ToString(), ggl[1].ToDecimal(0), out StockNum))
                                    {
                                        if (!string.IsNullOrEmpty(ggl[2].ToString()))
                                        {
                                            msg.Msg = "该批次库存为" + StockNum + "，请修改后再重新操作！";
                                        }
                                        else
                                        {
                                            msg.Msg = "商品库存数量小于发货数量，请修改后再重新操作！";
                                        }
                                        msg.Code = "Error";
                                        return (new JavaScriptSerializer().Serialize(msg));
                                    }
                                    
                                    outdmodel.OutNum = ggl[1].ToDecimal(0);
                                    outdmodel.Batchno = ggl[2].ToString();
                                    outdmodel.Validdate = ggl[3].ToString().Trim() == "" ? DateTime.MinValue : Convert.ToDateTime(ggl[3].ToString());
                                    outdmodel.Remark = "";
                                    outdmodel.ts = DateTime.Now;
                                    outdmodel.modifyuser = logUser.UserID;
                                    llo.Add(outdmodel);

                                    //出库单明细
                                    inoutModel.Batchno = ggl[2].ToString();
                                    inoutModel.CompID = logUser.CompID;
                                    inoutModel.CreateDate = DateTime.Now;
                                    inoutModel.CreateUserID = logUser.UserID;
                                    inoutModel.ts = DateTime.Now;
                                    inoutModel.modifyuser = logUser.UserID;
                                    //inoutModel.GoodsID = ;
                                    inoutModel.GoodsInfoID = od.GoodsinfoID;
                                    inoutModel.Validdate = ggl[3].ToString().Trim() == "" ? DateTime.MinValue : Convert.ToDateTime(ggl[3].ToString());
                                    inoutModel.StockNum = ggl[1].ToDecimal(0);
                                    llinOut.Add(inoutModel);
                                }
                                else
                                {
                                    msg.Msg = "发货数量已被其他人修改，请刷新后再重新操作！";
                                    msg.Code = "Error";
                                    return (new JavaScriptSerializer().Serialize(msg));
                                }
                            }

                            //判断是否存在发货明细
                            if (llo != null && llo.Count <= 0)
                            {
                                msg.Msg = "发货数量有误，请刷新后再重新操作！";
                                msg.Code = "Error";
                                return (new JavaScriptSerializer().Serialize(msg));
                            }

                            //判断订单是否完成发货

                            //修改订单状态 Ostate、IsOutState
                            //订单商品数量
                            decimal goodsnum = 0;
                            //订单发货数量
                            decimal outnum = 0;
                            foreach (var item in l)
                            {
                                goodsnum += item.GoodsNum + item.ProNum.ToDecimal(0);

                                //查找是否本次修改的商品发货数量
                                Hi.Model.DIS_OrderDetail os = ll.Find(p => p.GoodsinfoID == item.GoodsinfoID && p.OrderID == oID.ToInt(0));
                                if (os != null)
                                    outnum += os.OutNum;
                                else
                                    outnum += item.OutNum;
                            }

                            //订单商品数量==订单发货数量
                            if (outnum != goodsnum)
                            {
                                //存在没有完成发货的商品
                                if (omodel.IsOutState == (int)Enums.IsOutState.部分到货)
                                    omodel.IsOutState = (int)Enums.IsOutState.部分到货;
                                else if (omodel.IsOutState == (int)Enums.IsOutState.全部发货)
                                    omodel.IsOutState = (int)Enums.IsOutState.全部发货;
                                else
                                    omodel.IsOutState = (int)Enums.IsOutState.部分发货;
                            }
                            else
                                //完全发货
                                omodel.IsOutState = (int)Enums.IsOutState.全部发货;

                            //订单完成节点设置判断 1、提交完成  2、审核完成  3、发货完成 
                            //订单商品全部发货时，判断
                            Sd = new Hi.BLL.DIS_Order().OstateAudit(logUser.CompID);
                            //签收状态
                            int IsSign = 0;
                            if (Sd == 3 && omodel.IsOutState == (int)Enums.IsOutState.全部发货)
                            {
                                //订单完成节点设置为发货后完成,订单商品全部发货时，修改签收状态
                                IsSign = 1;
                                omodel.OState = (int)Enums.OrderState.已到货;
                            }
                            else
                            {
                                Sd = 0;
                                omodel.OState = (int)Enums.OrderState.已发货;
                            }

                            omodel.ts = DateTime.Now;
                            omodel.modifyuser = logUser.UserID;

                            //新增发货信息
                            Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                            orderOut.ReceiptNo = omodel.ReceiptNo + SysCode.GetCode("发货单", omodel.ID.ToString());
                            orderOut.CompID = omodel.CompID;
                            orderOut.DisID = omodel.DisID;
                            orderOut.OrderID = omodel.ID;
                            orderOut.ActionUser = logUser.TrueName;
                            orderOut.SendDate = date == "" ? DateTime.Now : date.ToString().ToDateTime();
                            orderOut.CreateUserID = logUser.UserID;
                            orderOut.CreateDate = DateTime.Now;
                            orderOut.IsSign = IsSign;
                            orderOut.ts = DateTime.Now;
                            orderOut.dr = 0;
                            orderOut.modifyuser = logUser.UserID;

                            //出库单
                            Hi.Model.DIS_StockOrder stockOModel = new Hi.Model.DIS_StockOrder();
                            stockOModel.CompID = omodel.CompID;
                            stockOModel.OrderNO = GetNo(2);
                            stockOModel.State = 2;
                            stockOModel.StockType = "销售出库";
                            stockOModel.Type = 2;
                            stockOModel.ChkDate = DateTime.Now;
                            stockOModel.ts = DateTime.Now;
                            stockOModel.dr = 0;
                            stockOModel.modifyuser = logUser.UserID;
                            stockOModel.CreateUserID = logUser.UserID;
                            stockOModel.CreateDate = DateTime.Now;
                            
                            //新增物流信息
                            Hi.Model.DIS_Logistics log = new Hi.Model.DIS_Logistics();
                            log.OrderID = omodel.ID;
                            log.ComPName = ComPName;
                            log.LogisticsNo = LogisticsNo;
                            log.CarUser = CarUser;
                            log.CarNo = CarNo;
                            log.Car = Car;
                            log.CreateUserID = logUser.UserID;
                            log.CreateDate = DateTime.Now;
                            log.ts = DateTime.Now;
                            log.modifyuser = logUser.UserID;

                            if (ComPName != "" && LogisticsNo != "")
                            {
                                string ApiKey = "4088ed72ed034b61b4b5adf05870aeba";
                                string typeCom = ComPName;
                                typeCom = Information.TypeCom(typeCom);
                                string nu = LogisticsNo;
                                string apiurl = "http://www.aikuaidi.cn/rest/?key=" + ApiKey + "&order=" + nu + "&id=" + typeCom + "&ord=asc&show=json";
                                WebRequest request = WebRequest.Create(@apiurl);
                                WebResponse response = request.GetResponse();
                                Stream stream = response.GetResponseStream();
                                Encoding encode = Encoding.UTF8;
                                StreamReader reader = new StreamReader(stream, encode);
                                string detail = reader.ReadToEnd();
                                Logistics logistics = JsonConvert.DeserializeObject<Logistics>(detail);
                                if (logistics.errCode == "0")
                                {
                                    List<Information> information = logistics.data;
                                    log.Context = JsonConvert.SerializeObject(information);
                                }
                            }

                            int outid = new Hi.BLL.DIS_OrderOut().GetOutOrder(omodel, ll, orderOut, llo, log, stockOModel, llinOut);
                            if (outid > 0)
                            {
                                msg.Result = true;

                                Utils.AddSysBusinessLog(omodel.CompID, "Order", omodel.ID.ToString(), "订单发货", "");
                                new Common().GetWxService("43", omodel.ID.ToString(), "0");

                                //查询订单明细
                                DataTable lll = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " Isnull(o.dr,0)=0 and o.OrderID=" + oID);
                                if (lll != null && lll.Rows.Count > 0) {
                                    string infoids = string.Empty;

                                    DataRow[] dr = lll.Select("IsOut=0");
                                    if (dr.Length > 0)
                                    {
                                        foreach (DataRow item in dr)
                                        {
                                            //infoids = string.Join(",", item["GoodsinfoID"].ToString());
                                            infoids += infoids == "" ? item["GoodsinfoID"].ToString() : "," + item["GoodsinfoID"].ToString();
                                        }
                                        List<Hi.Model.DIS_GoodsStock> stocklist = new Hi.BLL.DIS_GoodsStock().GetList("", " CompID=" + logUser.CompID + " and GoodsInfo in (" + infoids + ") and StockNum>0", "CreateDate asc");

                                        msg.Stock = (new JavaScriptSerializer().Serialize(stocklist)).ToString();
                                    }
                                }
                                
                                //查询未完成发货的订单商品
                                msg.Code = ConvertJson.ToJson(SelectGoodsInfo.SreeenDataTable(lll, "IsOut=0"));

                                //本次发货商品
                                List<Hi.Model.DIS_OrderOutDetail> loud = new Hi.BLL.DIS_OrderOutDetail().GetList("", " isnull(dr,0)=0 and OrderOutID=" + outid, "");
                                //本次的发货单
                                DataTable lo = new Hi.BLL.DIS_OrderOut().GetList("", " isnull(o.dr,0)=0 and o.IsAudit<>3 and o.ID=" + outid + "Order by o.IsAudit ");
                                string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", omodel.CompID);
                                msg.Rvlue = SelectGoodsInfo.outbind(lo, lll, loud, Digits, 1);
                                msg.ts = omodel.ts.ToString();

                                if (omodel.IsOutState == (int)Enums.IsOutState.全部发货)
                                    msg.Rdate = orderOut.SendDate.ToString("yyyy-MM-dd HH:mm");
                                msg.Msg = Sd.ToString();
                                //msg.Code = (new JavaScriptSerializer().Serialize(lll)).ToString();
                            }
                        }
                    }
                    else
                    {
                        msg.Msg = "订单处理中，请稍后再重新操作。";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                    msg.Code = "Error";
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
    /// 修改发货信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string upOut(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var outid = Common.DesDecrypt((context.Request["outid"] + ""), Common.EncryptKey);
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var str = context.Request["str"] + "";
                var dts = context.Request["ts"] + "";

                Hi.Model.DIS_OrderOut outmodel = new Hi.BLL.DIS_OrderOut().GetModel(outid.ToInt(0));
                Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));

                if (Convert.ToDateTime(dts).ToString("yyyy/MM/dd HH:mm:dd") == outmodel.ts.ToString("yyyy/MM/dd HH:mm:dd"))
                {
                    if ((outmodel != null && outmodel.IsAudit != 3) && (omodel != null && (omodel.OState == (int)Enums.OrderState.已审 || omodel.OState == (int)Enums.OrderState.已发货)))
                    {
                        if (outmodel.IsSign != 1)
                        {
                            //发货单明细
                            List<Hi.Model.DIS_OrderOutDetail> ol = new Hi.BLL.DIS_OrderOutDetail().GetList("", "isnull(dr,0)=0 and OrderOutID=" + outid, "");
                            //订单明细
                            List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + oID, "");
                            //本次修改的发货单明细
                            List<Hi.Model.DIS_OrderOutDetail> oll = new List<Hi.Model.DIS_OrderOutDetail>();
                            Hi.Model.DIS_OrderOutDetail oudmodel = null;

                            //本次修改的订单明细
                            List<Hi.Model.DIS_OrderDetail> ll = new List<Hi.Model.DIS_OrderDetail>();
                            Hi.Model.DIS_OrderDetail od = null;

                            var gl = str.Split(new string[] { "；" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < gl.Length; i++)
                            {
                                var ggl = gl[i].Split(new string[] { "：" }, StringSplitOptions.None);
                                decimal num = ggl[1].ToDecimal(0);

                                //发货明细
                                oudmodel = ol.Find(p => p.ID == ggl[0].ToInt(0));
                                //原发货数量
                                decimal pnum = oudmodel.OutNum;

                                //订单明细
                                od = l.Find(p => p.OrderID == oID.ToInt(0) && p.GoodsinfoID == oudmodel.GoodsinfoID);
                                //已发货数量
                                decimal stopnum = od.OutNum;

                                //本次发货数量判断
                                decimal tnum = od.GoodsNum + od.ProNum.ToDecimal(0) - (stopnum - pnum);

                                if (tnum >= ggl[1].ToDecimal(0))
                                {
                                    //修改的发货
                                    oudmodel.OutNum = ggl[1].ToDecimal(0);

                                    oudmodel.Batchno = ggl[2].ToString();
                                    oudmodel.Validdate = ggl[3].ToString().Trim() == "" ? DateTime.MinValue : Convert.ToDateTime(ggl[3].ToString());

                                    //修改发货数量后的已发货数量
                                    od.OutNum = (stopnum - pnum) + ggl[1].ToDecimal(0);
                                    if ((od.GoodsNum + od.ProNum.ToDecimal(0)) - od.OutNum <= 0)
                                        od.IsOut = 1;// 该发货完成

                                    else
                                        od.IsOut = 0;//该商品发货未完成
                                }
                                else
                                {
                                    msg.Msg = "发货数量有误，修改发货信息失败。";
                                    return (new JavaScriptSerializer().Serialize(msg));
                                }

                                ll.Add(od);
                                oll.Add(oudmodel);
                            }
                            //修改订单状态 Ostate、IsOutState
                            //订单商品数量
                            decimal goodsnum = 0;
                            //订单发货数量
                            decimal outnum = 0;
                            foreach (var item in l)
                            {
                                goodsnum += item.GoodsNum + od.ProNum.ToDecimal(0);

                                //查找是否本次修改的商品发货数量
                                Hi.Model.DIS_OrderDetail os = ll.Find(p => p.GoodsinfoID == item.GoodsinfoID && p.OrderID == oID.ToInt(0));
                                if (os != null)
                                    outnum += os.OutNum;
                                else
                                    outnum += item.OutNum;
                            }
                            omodel.OState = (int)Enums.OrderState.已发货;

                            //订单商品数量==订单发货数量
                            if (outnum == goodsnum)
                            {
                                if (omodel.IsOutState == (int)Enums.IsOutState.部分发货 || omodel.IsOutState == (int)Enums.IsOutState.部分到货 || omodel.IsOutState == 0)
                                    omodel.IsOutState = (int)Enums.IsOutState.全部发货;
                            }
                            else
                            {
                                if (omodel.IsOutState == (int)Enums.IsOutState.部分发货 || omodel.IsOutState == (int)Enums.IsOutState.全部发货 || omodel.IsOutState == 0)
                                    omodel.IsOutState = (int)Enums.IsOutState.部分发货;
                                //omodel.OState = (int)Enums.OrderState.已审;
                            }
                            omodel.ts = DateTime.Now;
                            omodel.modifyuser = logUser.UserID;

                            outmodel.ts = omodel.ts;
                            outmodel.modifyuser = logUser.UserID;

                            if (new Hi.BLL.DIS_OrderOut().GetOutUpOrder(outmodel, omodel, ll, oll) > 0)
                            {
                                msg.Result = true;
                                msg.Msg = "修改发货单成功";

                                msg.Rdate = outmodel.SendDate.ToString("yyyy-MM-dd HH:mm");

                                //查询未完成发货的订单商品
                                DataTable lll = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " Isnull(o.dr,0)=0 and o.OrderID=" + oID + " and Isnull(IsOut,0)=0");

                                msg.ts = omodel.ts.ToString();

                                //msg.Code = (new JavaScriptSerializer().Serialize(lll)).ToString();
                                msg.Code = ConvertJson.ToJson(lll);
                            }
                        }
                        else
                        {
                            msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                        }
                    }
                    else
                    {
                        msg.Msg = "订单处理中，请稍后再重新操作。";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                    msg.Code = "Error";
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
    /// 作废发货单
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string CancelOut(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                var outid = Common.DesDecrypt((context.Request["outid"] + ""), Common.EncryptKey);
                var oID = Common.DesDecrypt(context.Request["oID"] + "", Common.EncryptKey);
                var dts = context.Request["ts"] + "";

                //查询订单下的所有发货单
                List<Hi.Model.DIS_OrderOut> lo = new Hi.BLL.DIS_OrderOut().GetList("", "Isnull(dr,0)=0 and Isnull(IsAudit,0)<>3 and OrderID=" + oID, "");
                //本次作废的发货单
                Hi.Model.DIS_OrderOut outmodel = lo.Find(p => p.ID == outid.ToInt(0));

                //销售出库单信息
                string stockOids = string.Empty;
                string stockInids = string.Empty;
                List<Hi.Model.DIS_StockOrder> stockOList = new Hi.BLL.DIS_StockOrder().GetList("", " OrderID=" + outmodel.ID, "");
                if (stockOList != null && stockOList.Count > 0)
                {
                    foreach (var item in stockOList)
                    {
                        stockOids += stockOids == "" ? item.ID.ToString() : "," + item.ID.ToString();
                    }
                }
                if (!string.IsNullOrWhiteSpace(stockOids))
                {
                    List<Hi.Model.DIS_StockInOut> stockIList = new Hi.BLL.DIS_StockInOut().GetList("", " StockOrderID in (" + stockOids + ")", "");
                    if (stockIList != null && stockIList.Count > 0)
                    {
                        foreach (var item in stockIList)
                        {
                            stockInids += stockInids == "" ? item.ID.ToString() : "," + item.ID.ToString();
                        }
                    }
                }
                
                if (Convert.ToDateTime(dts).ToString("yyyy/MM/dd HH:mm:dd") == outmodel.ts.ToString("yyyy/MM/dd HH:mm:dd"))
                {
                    //该订单多次发货的其他发货单
                    List<Hi.Model.DIS_OrderOut> loo = lo.FindAll(p => p.ID != outid.ToInt(0));

                    //订单
                    Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(oID.ToInt(0));
                    int IsOutState = 0;

                    if ((outmodel != null && outmodel.IsAudit != 3) && (omodel != null && (omodel.OState == (int)Enums.OrderState.已审 || omodel.OState == (int)Enums.OrderState.已发货)))
                    {
                        if (outmodel.IsSign != 1)
                        {
                            //修改发货单状态
                            outmodel.IsAudit = 3;
                            outmodel.modifyuser = logUser.UserID;
                            outmodel.ts = DateTime.Now;

                            //本次发货商品
                            List<Hi.Model.DIS_OrderOutDetail> loud = new Hi.BLL.DIS_OrderOutDetail().GetList("", " isnull(dr,0)=0 and OrderOutID=" + outid, "");

                            //订单明细
                            List<Hi.Model.DIS_OrderDetail> l = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + oID, "");
                            //作废发货单，要修改的订单商品明细list
                            List<Hi.Model.DIS_OrderDetail> ol = new List<Hi.Model.DIS_OrderDetail>();
                            Hi.Model.DIS_OrderDetail odmodel = null;

                            //作废发货单时，还回的商品数量
                            if (loud != null && loud.Count > 0 && l != null && l.Count > 0)
                            {
                                foreach (var item in loud)
                                {
                                    odmodel = new Hi.Model.DIS_OrderDetail();
                                    odmodel = l.Find(p => p.OrderID == oID.ToInt(0) && p.GoodsinfoID == item.GoodsinfoID);

                                    //订单已发货数量
                                    odmodel.OutNum = odmodel.OutNum - item.OutNum;
                                    if ((odmodel.GoodsNum + odmodel.ProNum.ToDecimal(0)) - odmodel.OutNum > 0)
                                        odmodel.IsOut = 0;
                                    else
                                        odmodel.IsOut = 1;
                                    odmodel.ts = outmodel.ts;
                                    odmodel.modifyuser = logUser.UserID;

                                    ol.Add(odmodel);
                                }

                                //作废发货单时，修改订单状态
                                if (loo != null && loo.Count > 0)
                                {
                                    //多次发货，只修改发货状态
                                    if (omodel.IsOutState == (int)Enums.IsOutState.全部发货)
                                    {
                                        IsOutState = 0;
                                        omodel.IsOutState = (int)Enums.IsOutState.部分发货;
                                    }
                                }
                                else
                                {
                                    //只存在一次发货，订单状态返回上一步
                                    IsOutState = 1;
                                    omodel.IsOutState = 0;
                                    omodel.OState = (int)Enums.OrderState.已审;
                                }
                                omodel.ts = outmodel.ts;
                                omodel.modifyuser = logUser.UserID;

                                if (new Hi.BLL.DIS_OrderOut().GetCancelOut(omodel, outmodel, ol, loud,stockOids ,stockInids) > 0)
                                {
                                    msg.Result = true;
                                    msg.Msg = "作废发货单成功";
                                    msg.ts = omodel.ts.ToString();
                                    //查询未完成发货的订单商品
                                    DataTable lll = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " Isnull(o.dr,0)=0 and o.OrderID=" + oID + " and Isnull(IsOut,0)=0");
                                    
                                    if (lll != null && lll.Rows.Count > 0)
                                    {
                                        string infoids = string.Empty;

                                        DataRow[] dr = lll.Select("IsOut=0");
                                        if (dr.Length > 0)
                                        {
                                            foreach (DataRow item in dr)
                                            {
                                                infoids += infoids == "" ? item["GoodsinfoID"].ToString() : "," + item["GoodsinfoID"].ToString();
                                            }
                                            List<Hi.Model.DIS_GoodsStock> stocklist = new Hi.BLL.DIS_GoodsStock().GetList("", " CompID=" + logUser.CompID + " and GoodsInfo in (" + infoids + ") and StockNum>0", "CreateDate asc");

                                            msg.Stock = (new JavaScriptSerializer().Serialize(stocklist)).ToString();
                                        }
                                    }
                                    
                                    //没有作废的发货单
                                    List<Hi.Model.DIS_OrderOut> lno = lo.FindAll(p => p.IsAudit != 3);
                                    lno.Sort();
                                    if (lno != null && lno.Count > 0)
                                        msg.Rdate = lno[0].SendDate.ToString() != "" ? lno[0].SendDate.ToString("yyyy-MM-dd HH:mm") : "";

                                    //订单状态返回上一步
                                    if (lll != null && lll.Rows.Count > 0)
                                    {
                                        msg.Rvlue = "1";
                                        msg.Rdate = "";
                                    }
                                    else
                                        msg.Rvlue = "0";

                                    //msg.Code = (new JavaScriptSerializer().Serialize(lll)).ToString();
                                    msg.Code = ConvertJson.ToJson(lll);
                                }
                                else
                                    msg.Msg = "订单处理失败。";
                            }
                            else
                                msg.Msg = "订单出错了，请联系管理员。";
                        }
                        else
                        {
                            msg.Msg = "订单已签收，请稍后再重新操作。";
                        }
                    }
                    else
                    {
                        msg.Msg = "订单处理中，请稍后再重新操作。";
                        msg.Code = "Error";
                    }
                }
                else
                {
                    msg.Msg = "订单已被其他人修改，请刷新后再重新操作！";
                    msg.Code = "Error";
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
    /// 线下支付保存
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string Payed(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();

        string KeyID = Common.DesDecrypt(context.Request["KeyID"] + "", Common.EncryptKey);//订单ID
        string DisID = Common.NoHTML(context.Request["DisID"]) + "";//DisID
        string paymoney = Common.NoHTML(context.Request["paymoney"]) + "";//付款金额
        string bankname = Common.NoHTML(context.Request["bankname"]) + "";//账户名称
        string bank = Common.NoHTML(context.Request["bank"]) + "";//收款银行
        string bankcode = Common.NoHTML(context.Request["bankcode"]) + "";//收款卡号
        string txtArriveDate = Common.NoHTML(context.Request["txtArriveDate"]) + "";//支付日期
        string remark = Common.NoHTML(context.Request["remark"]) + "";//备 注
        string attach = Common.NoHTML(context.Request["attach"]) + "";//上传附件

        int hid_type = Convert.ToInt32(context.Request["hid_type"]);

        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {

            Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(KeyID));

            if (ordermodel.PayedAmount + Convert.ToDecimal(paymoney) > ordermodel.AuditAmount || ordermodel.PayedAmount == ordermodel.AuditAmount)
            {
                msg.Result = false;
                msg.Msg = "付款失败，订单支付完成或支付金额大于订单未支付金额！";
                msg.Code = "error";
                return (new JavaScriptSerializer().Serialize(msg));
            }
            List<Hi.Model.DIS_OrderReturn> OrderReturn = new Hi.BLL.DIS_OrderReturn().GetList("", " CompID='" + ordermodel.CompID + "' and OrderID='" + ordermodel.ID + "'", "");
            if (ordermodel.OState == 3 || ordermodel.OState == 6 || ordermodel.OState == 7 || OrderReturn.Count > 0)
            {
                msg.Result = false;
                msg.Msg = "付款失败，订单已申请退货,或已作废！";
                msg.Code = "error";
                return (new JavaScriptSerializer().Serialize(msg));
            }



            int order = 0;
            int pay = 0;
            SqlConnection con = new SqlConnection(LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();

            try
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                Hi.Model.PAY_Payment paymentmodel = new Hi.Model.PAY_Payment();
                paymentmodel.OrderID = Convert.ToInt32(KeyID);
                paymentmodel.DisID = Convert.ToInt32(DisID);
                paymentmodel.PayPrice = Convert.ToDecimal(paymoney);
                paymentmodel.payName = bankname;
                paymentmodel.paycode = bankcode;
                paymentmodel.paybank = bank;
                paymentmodel.guid = Common.Number_repeat(guid);
                paymentmodel.PayDate = Convert.ToDateTime(txtArriveDate);
                paymentmodel.Remark = remark;
                paymentmodel.PrintNum = 1;//下线支付无需结算，所以结算状态是1
                paymentmodel.vdef3 = "1";//（1，订单支付，2，预付款充值、汇款）


                paymentmodel.IsAudit = 2;//1，成功 ，2失败 


                paymentmodel.Channel = "5";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付 
                paymentmodel.CreateDate = DateTime.Now;
                paymentmodel.vdef5 = "0.00";//手续费

                paymentmodel.CreateUserID = logUser.UserID;
                if (hid_type == 1)
                    paymentmodel.vdef9 = "1";

                paymentmodel.attach = attach;//附件  

                //附件cope
                string[] files = attach.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string file in files)
                {
                    if (File.Exists(context.Server.MapPath("../TempFile/") + file))
                    {
                        File.Copy(context.Server.MapPath("../TempFile/") + file, context.Server.MapPath("../UploadFile/") + file);
                    }
                }


                //厂商直接就是成功

                int num = new Hi.BLL.PAY_Payment().Add(paymentmodel);
                if (num > 0)
                {
                    //厂商直接支付成功，修改已支付金额
                    if (hid_type == 1)
                    {
                        order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, Convert.ToInt32(KeyID), Convert.ToDecimal(paymoney), sqlTrans);//修改订单状态
                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, num, sqlTrans);//修改支付表状态

                        if (order > 0 && pay > 0)
                            sqlTrans.Commit();
                        else
                            sqlTrans.Rollback();

                    }
                    else//代理商只修改支付状态，为成功
                    {
                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, num, sqlTrans);//修改支付表状态

                        if (pay > 0)
                            sqlTrans.Commit();
                        else
                            sqlTrans.Rollback();

                    }
                    msg.Result = true;
                    msg.Msg = "付款成功,等待厂商确认";
                    msg.Code = "success";
                }
                else
                {
                    msg.Result = false;
                    msg.Msg = "付款失败";
                    msg.Code = "error";
                }
            }
            catch
            {
                msg.Result = false;
                msg.Msg = "付款失败";
                msg.Code = "error";
            }
            finally
            {
                con.Close();

            }

        }


        return (new JavaScriptSerializer().Serialize(msg));
    }

    /// <summary>
    /// 线下支付作废、确认操作
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string PayTovoid(HttpContext context)
    {
        ResultMsgHd msg = new ResultMsgHd();
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            int order = 0;
            int pay = 0;
            SqlConnection con = new SqlConnection(LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();
            try
            {
                string paymentID = context.Request["paymentid"] + "";
                int smg = Convert.ToInt32(context.Request["smg"]);//确认、还是作废操作（1，确认，2，作废）


                Hi.Model.PAY_Payment paymentmodel = new Hi.BLL.PAY_Payment().GetModel(Convert.ToInt32(paymentID));


                Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(paymentmodel.OrderID);
                //确认
                if (smg == 1)
                {

                    if (ordermodel.PayedAmount + paymentmodel.PayPrice > ordermodel.AuditAmount)
                    {

                        msg.Result = false;
                        msg.Msg = "付款失败，支付金额大于订单未支付金额！";
                        msg.Code = "error";
                        return (new JavaScriptSerializer().Serialize(msg));
                    }

                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, paymentmodel.OrderID, paymentmodel.PayPrice, sqlTrans);//修改订单状态
                    paymentmodel.vdef9 = "1";
                    new Hi.BLL.PAY_Payment().Update(paymentmodel);

                    // pay = new Hi.BLL.PAY_Payment().updatePayState(con, Convert.ToInt32(paymentID), sqlTrans);//修改支付表状态

                    if (order > 0)
                    {
                        sqlTrans.Commit();
                        msg.Result = true;
                        msg.Msg = "付款成功";
                        msg.Code = "success";
                    }
                    else
                    {
                        sqlTrans.Rollback();
                        msg.Result = false;
                        msg.Msg = "付款失败";
                        msg.Code = "error";
                    }



                }//作废
                else
                {
                    if (ordermodel.PayedAmount != 0 && ordermodel.PayedAmount >= paymentmodel.PayPrice)
                    {
                        order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, paymentmodel.OrderID, paymentmodel.PayPrice * -1, sqlTrans);//修改订单状态
                                                                                                                                          //pay = new Hi.BLL.PAY_Payment().updatePayToVoid(con, paymentmodel.ID, sqlTrans);//修改线下支付的状态为作废

                        paymentmodel.vdef9 = "2";
                        new Hi.BLL.PAY_Payment().Update(paymentmodel);
                        if (order > 0)
                        {
                            sqlTrans.Commit();
                            msg.Result = true;
                            msg.Msg = "操作成功";
                            msg.Code = "success";
                        }
                        else
                        {
                            sqlTrans.Rollback();
                            msg.Result = false;
                            msg.Msg = "操作失败";
                            msg.Code = "error";
                        }
                    }
                    else
                    {
                        sqlTrans.Rollback();
                        msg.Result = false;
                        msg.Msg = "操作失败,作废金额大于已支付金额！";
                        msg.Code = "error";
                    }
                }
            }
            catch
            {
                msg.Result = false;
                msg.Msg = "操作失败";
                msg.Code = "error";
            }
            finally
            {
                con.Close();
            }
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }
    /// <summary>
    /// 修改商品数据
    /// </summary>
    /// <param name="GoodsInfoId">商品Id</param>
    /// <param name="num">数量</param>
    /// <param name="Remark">备注</param>
    public string GetDoogsPrice(HttpContext context)
    {
        string DisId = context.Request["disId"]; //代理商
        int compId = Convert.ToInt32(context.Request["compId"]);  //厂商
        string Json = string.Empty;
        //获取商品下单总价
        decimal TotalAmount = Convert.ToDecimal(context.Request["SumTotal"]); //SelectGoodsInfo.SumTotal(DisId.ToInt(0), compId);
        ProPrice = Common.GetProPrice(TotalAmount, out ProID, out ProIDD, out ProType, compId).ToString();
        Json = "{\"ds\":\"" + true + "\",\"proPrice\":\"" + ProPrice + "\"}";
        return Json;
    }
    /// <summary>
    /// 选择的商品Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public string GetGoodsInfoList(HttpContext context)
    {
        int DisId = Convert.ToInt32(context.Request["disId"]); //代理商
        int compId = Convert.ToInt32(context.Request["compId"]); //厂商
        string infoId = context.Request["goodsInfoId"];  //商品信息Id
                                                         //没有选中商品 ，直接返回
        if (infoId.Length <= 0)
        {
            return "";
        }
        string[] info = infoId.Split(new char[] { ',' });
        try
        {
            DataTable dt = new DataTable();

            dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;
            for (int i = 0; i < info.Length; i++)
            {
                if (dt != null)
                {
                    DataRow[] dr = dt.Select(string.Format(" DisId='{0}' and CompId='{1}' and GoodsinfoID='{2}'", DisId, compId, info[i]));
                    if (dr.Length > 0)
                    {
                        decimal num = dr[0]["GoodsNum"].ToString().ToDecimal() + 1;
                        //存在该商品 商品数据+1
                        SelectGoodsInfo.UpDateGoods(info[i].ToInt(0), DisId, compId, 0, num, "", "3");
                    }
                    else
                    {

                        //没有商品信息新增商品信息
                        SelectGoodsInfo.Goods(info[i].ToInt(0), DisId, compId);
                    }
                }
                else
                {
                    //没有商品信息新增商品信息
                    SelectGoodsInfo.Goods(info[i].ToInt(0), DisId, compId);
                }
            }
            return "cg";
        }
        catch (Exception)
        {
            return "";
        }
    }
    /// <summary>
    /// 根据代理商ID得到收货地址
    /// </summary>
    /// <param name="disid"></param>
    /// <returns></returns>
    public string getDisAdder(HttpContext context)
    {
        int disid = Convert.ToInt32(context.Request["disId"]); //代理商
        int keyId = Convert.ToInt32(context.Request["keyId"]); //代理商
        string str = string.Empty;
        List<Hi.Model.BD_DisAddr> list = new Hi.BLL.BD_DisAddr().GetList("", "isnull(dr,0)=0 and disId=" + disid, "");
        if (list.Count > 0)
        {
            Hi.Model.BD_DisAddr list2 = list.Find(x => x.IsDefault == 1);
            if (list2 != null)
            {
                str = list2.Principal + "@@" + list2.Phone + "@@" + list2.Address + "@@" + list2.ID;
                // str = "收货人：" + list2[0].Principal + "，联系电话：" + list2[0].Phone + "，收货地址：" + list2[0].Address + "@@" + list2[0].ID;
            }
            else
            {
                str = list[0].Principal + "@@" + list[0].Phone + "@@" + list[0].Address + "@@" + list[0].ID;
                //str = "收货人：" + list[0].Principal + "，联系电话：" + list[0].Phone + "，收货地址：" + list[0].Address + "@@" + list[0].ID;
            }
            str = str + "@@" + OrderType.GetRebate(keyId, Convert.ToInt32(disid));
        }
        else
        {
            str = "" + "@@" + "" + "@@" + "" + "@@" + "" + "@@" + OrderType.GetRebate(keyId, Convert.ToInt32(disid));
        }
        return str;
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
public class ResultMsgHd
{
    public ResultMsgHd()
    {
        Result = false;
        Msg = "";
        Code = "";
        Rvlue = "";
        Rdate = "";
        Stock = "";
    }
    public bool Result;
    public string Msg;
    public string Code;
    public string Rvlue;
    public string Rdate;
    public string ts;
    public string Stock;
}