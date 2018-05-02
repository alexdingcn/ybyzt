using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using LitJson;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;

public class DIS_Order
{
    public DIS_Order()
    {
    }

    #region 经销商

    /// <summary>
    /// 订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderList GetResellerOrderList(string JSon)
    {
        try
        {
            string strWhere = string.Empty;

            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string state = string.Empty;
            string oState = string.Empty;
            string payState = string.Empty;
            string compID = string.Empty;

            Hi.Model.BD_Distributor dis =null;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                JInfo["Sort"].ToString() != "" && JInfo["State"].ToString() != "" && JInfo["CompID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disID));
                if (dis==null)
                    return new ResultOrderList() { Result = "F", Description = "经销商异常" };
                compID = JInfo["CompID"].ToString();
                strWhere += " and DisID='" + disID + "' and ISNULL(dr,0)=0 and compid = '"+compID+"'";
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                state = JInfo["State"].ToString();
                if (JInfo["OState"].ToString() != "")
                    oState = JInfo["OState"].ToString();
                if (JInfo["PayState"].ToString() != "")
                    payState = JInfo["PayState"].ToString();
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };

            if (oState != "-2")
                strWhere += " and OState in (" + oState + ")";
            if (payState != "-1")
                strWhere += " and PayState in(" + payState + ") and OState <>6 ";


            JsonData JMsg = JInfo["Search"];
            if (JMsg.Count > 0)
            {
                if (JMsg["BeginDate"].ToString() != "-1")
                {
                    strWhere += " and CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
                }
                if (JMsg["EndDate"].ToString() != "-1")
                {
                    strWhere += " and CreateDate < '" + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
                }
                //根据出库单编号 锁定 订单ID
                if (JMsg["ExpressNo"].ToString() != "-1")
                {
                    //订单编号
                    strWhere += " and ( ReceiptNo like '%" + JMsg["ExpressNo"].ToString() + "%'";
                    //商品名称
                    string goodsName = Common.GetOrderByGoodsName(JMsg["ExpressNo"].ToString().Trim(),
                        dis.CompID.ToString(), dis.ID.ToString());
                    if (goodsName != "-1")
                    {
                        strWhere += " or ID in (" + goodsName + ") ";
                    }

                    
                    //物流编号
                    List<Hi.Model.DIS_OrderOut> orderOut = new Hi.BLL.DIS_OrderOut().GetList("",
                        " ReceiptNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                    if (orderOut.Count != 0)
                    {
                        strWhere += " or ID in (" + string.Join(",", orderOut.Select(p => p.OrderID)) + ")";
                    }
                    strWhere += " )";
                }
            }

            if (state != "0" && state != "1" && state != "3" && state != "4" && state != "5" && state != "6" && state != "7")
                return new ResultOrderList() {Result = "F", Description = "状态异常"};

            if (criticalOrderID == "-1")
            {
                switch (state) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消
                {
                    case "1":
                        strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and Otype<>9 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and DisID='" + disID + "' and ReturnState in(0,1) and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除
                        break;
                    case "3":
                        strWhere += " and ostate=" + (int) Enums.OrderState.已发货;
                        break;
                    case "4":
                        //strWhere += " and ostate=" + (int) Enums.OrderState.已到货 + " and ReturnState in (" +
                        //            (int) Enums.ReturnState.未退货 + "," + (int) Enums.ReturnState.新增退货 + ")";
                        strWhere += " and ostate = " + (int)Enums.OrderState.已到货 + "  ";
                        //strWhere += " and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.申请退货 + "," + (int)Enums.ReturnState.拒绝退货 + ")";
                        break;
                    case "5":
                        //strWhere += " and (ostate in (" + (int) Enums.OrderState.退货处理 + " ," + (int) Enums.OrderState.已退货 + ")"+
                        //            " or paystate in (" + (int)Enums.PayState.申请退款 + " ," + (int)Enums.PayState.已退款 + ")" +
                        //            " or ReturnState in (" + (int)Enums.ReturnState.申请退货 + " ," + (int) Enums.ReturnState.退货退款 + "))";
                        strWhere += " and ostate = "+(int)Enums.OrderState.已退货+" and ReturnState = "+(int)Enums.ReturnState.退货退款+" ";
                        break;
                    case "6":
                        strWhere += " and ostate=" + (int) Enums.OrderState.已审 + " and ReturnState=" +
                                    (int) Enums.ReturnState.未退货;
                        break;
                    case "7":
                        strWhere += " and ostate=" + (int) Enums.OrderState.待审核;
                        break;
                }
            }
            else
            {
                Hi.Model.DIS_Order Order = new Hi.BLL.DIS_Order().GetModel(int.Parse(criticalOrderID));
                if (Order != null && Order.dr == 0)
                {
                    switch (state)
                    {
                        case "1":
                            strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and Otype<>9 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and DisID='" + disID + "' and ReturnState in(0,1) and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除
                            if (Order.PayState != (int)Enums.PayState.未支付 && Order.PayState != (int)Enums.PayState.部分支付)
                                return new ResultOrderList() {Result = "F", Description = "该订单已支付"};
                            break;
                        case "3":
                            strWhere += " and ostate=" + (int) Enums.OrderState.已发货;
                            if (Order.OState != (int) Enums.OrderState.已发货)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                        case "4":
                            strWhere += " and ostate=" + (int) Enums.OrderState.已到货;
                            if (Order.OState != (int) Enums.OrderState.已到货)
                                return new ResultOrderList() {Result = "F", Description = "该订单未发货"};
                            break;
                        case "5":
                            //strWhere += " and (ostate=" + (int) Enums.OrderState.退货处理 +
                            //            " or ostate=" + (int) Enums.OrderState.已退货 +
                            //            " or paystate=" + (int) Enums.PayState.申请退款 +
                            //            " or paystate=" + (int) Enums.PayState.已退款 +
                            //            " or ReturnState=" + (int) Enums.ReturnState.申请退货 +
                            //            " or ReturnState=" + (int) Enums.ReturnState.退货退款 + ")";
                            strWhere += " and ostate = " + (int)Enums.OrderState.已退货 + " and ReturnState = " + (int)Enums.ReturnState.退货退款 + " ";
                            if (Order.OState != (int) Enums.OrderState.已退货 &&
                                Order.ReturnState != (int) Enums.ReturnState.退货退款)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                        case "6":
                            strWhere += " and ostate=" + (int) Enums.OrderState.已审;
                            if (Order.OState != (int) Enums.OrderState.已审)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                        case "7":
                            strWhere += " and ostate=" + (int) Enums.OrderState.待审核;
                            if (Order.OState != (int) Enums.OrderState.待审核)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                    }
                }
                else
                {
                    return new ResultOrderList() {Result = "F", Description = "异常"};
                }
            }

            strWhere += " and ISNULL(dr,0)=0 and OState <> 0 and Otype<>9";

            #endregion

            #region 模拟分页

            string tabName = " [dbo].[DIS_Order]"; //表名
            string strsql = string.Empty; //搜索sql

            if (sortType == "1") //价格排序
            {
                sortType = "CreateDate";
            }
            else if (sortType == "2") //价格排序
            {
                sortType = "TotalAmount";
            }
            else
            {
                sortType = "ID";
            }
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResultOrderList() {Result = "F", Description = "基础数据异常"};

            #endregion

            #region 赋值

            
            List<Order> OrderList = new List<Order>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResultOrderList() {Result = "T", Description = "没有更多数据"};
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                    return new ResultOrderList() {Result = "T", Description = "没有更多数据"};
                foreach (DataRow row in orderList.Rows)
                {
                    Order order = new Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "企业异常"};
                    order.CompName = comp.CompName;
                    order.State = state.Trim()=="0"?Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                        orderModel.ReturnState):state;

                    string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
                    Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                        orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
                    order.IsEnSend = IsEnSend;
                    order.IsEnPay = IsEnPay;
                    order.IsEnAudit = IsEnAudit;
                    order.IsEnReceive = IsEnReceive;
                    order.IsEnReturn = IsEnReturn;   
                       
                    order.OState = orderModel.OState.ToString();
                    order.AddType = orderModel.AddType.ToString();
                    order.Otype = orderModel.Otype.ToString();
                    order.PayState = orderModel.PayState.ToString();
                    order.ReturnState = orderModel.ReturnState.ToString();
                    order.DisID = orderModel.DisID.ToString();
                    dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
                    if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "经销信息商异常"};
                    order.DisName = dis.DisName;
                    order.DisUserID = orderModel.DisUserID.ToString();
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
                    if (user == null || user.IsEnabled == 0 || user.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "经销商用户信息异常"};
                    order.DisUserName = user.TrueName;
                    order.AddrID = orderModel.AddrID.ToString();
                    order.ReceiptNo = orderModel.ReceiptNo;
                    if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00")
                    order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
                    if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))
                    {
                        Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                        if (addr != null)
                        {
                            order.Zip = addr.Zip;
                        }
                    }
                    order.Address = orderModel.Address;
                    order.Contact = orderModel.Principal;
                    order.Phone = orderModel.Phone;
                    order.TotalAmount = orderModel.TotalAmount.ToString();
                    order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
                    order.PayedAmount = orderModel.PayedAmount.ToString();
                    order.CreateUserID = orderModel.CreateUserID.ToString();
                    order.CreateDate = orderModel.CreateDate.ToString("yyyy-MM-dd HH:mm");
                    order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
                    order.ReturnMoneyUser = orderModel.ReturnMoneyUser;
                    order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();

                    List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                        " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                        orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (outList.Count != 0)
                    {
                        Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                        orderOut = outList[0];
                       
                        order.SendID = orderOut.ID.ToString();
                        order.SendDate = orderOut.SendDate.ToString();
                        //order.Express = orderOut.Express;
                        //order.ExpressNo = orderOut.ExpressNo;
                        //order.ExpressPerson = orderOut.ExpressPerson;
                        //order.ExpressTel = orderOut.ExpressTel;
                        //order.ExpressBao = orderOut.ExpressBao;
                        //order.PostFee = orderOut.PostFee.ToString();
                        order.ActionUser = orderOut.ActionUser;
                        //order.SendRemark = orderModel.Remark.ToString();
                        List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                        if(exlist !=null)
                            order.SendRemark = exlist[0].Context;

                        order.IsAudit = orderOut.IsAudit.ToString();
                        order.AuditUserID = orderOut.AuditUserID.ToString();
                        order.AuditDate = orderOut.AuditDate.ToString();
                        order.AuditRemark = orderOut.AuditRemark == null ? "" : orderOut.AuditRemark.ToString();
                        order.SignDate = orderOut.SignDate.ToString();
                        order.IsSign = orderOut.IsSign.ToString();
                        order.SignUserId = orderOut.SignUserId.ToString();
                        order.SignUser = orderOut.SignUser;
                        order.SignRemark = orderOut.SignRemark;
                    }
                    
                    //todo:不知道的排序
                    //order.SortIndex = orderModel.SortIndex.ToString();               
                    order.IsDel = orderModel.dr.ToString();

                    //明细
                    List<OrderDetail> orderDetail = new List<OrderDetail>();
                    List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                        " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (detailList == null) //|| detailList.Count==0 没有明细的单 PC可以新建
                        return new ResultOrderList() {Result = "F", Description = "订单明细异常"};
                    List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        string SKUName = string.Empty;
                        OrderDetail ordetail = new OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        if (goodsInfo == null )
                        //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                            return new ResultOrderList() {Result = "F", Description = "SKU信息异常"};
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null )
                        //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                            return new ResultOrderList() {Result = "F", Description = "商品异常"};
                        ordetail.ProductName = goods.GoodsName;
                        SKUName += goods.GoodsName;

                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                            {
                                SKUName += attr.AttrsName;
                            }
                        }
                        ordetail.SKUName = SKUName;
                        //todo:描述是什么
                        ordetail.ValueInfo = goodsInfo.ValueInfo;
                        ordetail.SalePrice = detail.Price.ToString("0.00");
                        ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                        ordetail.Num = detail.GoodsNum.ToString("0.00");
                        ordetail.Remark = detail.Remark;
                        ordetail.IsPro = detail.vdef1 == null || detail.vdef1.Trim() == "0" || detail.vdef1.Trim() == "" ? "0" : "1"; //是否是促销商品

                        if (ordetail.IsPro != "0")
                        {
                            ordetail.ProNum = detail.vdef5;
                            if (detail.vdef1 != "" && detail.vdef1.Length > 0)
                            {
                                Hi.Model.BD_Promotion pro =
                                    new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.vdef1));
                                if (pro != null)
                                {
                                    List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList(
                                        "", " ProID=" + pro.ID + " and GoodInfoID ='" + ordetail.SKUID + "' and dr=0", "");
                                    string info = string.Empty;
                                    if (dList != null && dList.Count > 0)
                                    {
                                        if (pro.Type == 0 && pro.ProType == 1)
                                        {
                                            info = "赠品";
                                        }
                                        else if (pro.Type == 0 && pro.ProType == 2)
                                        {
                                            info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 3)
                                        {
                                            info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 4)
                                        {
                                            info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                        }
                                    }
                                    ordetail.proInfo = new DIS_Order.PromotionInfo()
                                    {
                                        ProID = detail.vdef1,
                                        ProTitle = pro.ProTitle,
                                        ProInfos = info,
                                        Type = pro.Type.ToString(),
                                        ProTpye = pro.ProType.ToString(),
                                        Discount = pro.Discount.ToString("0.00"),
                                        ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                        ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                    };
                                }
                            }
                        }
                        List<Pic> Pic = new List<Pic>();
                        if (goods.Pic.ToString() != "" && goods.Pic.ToString() != "X")
                        {
                            Pic pic = new Pic();
                            pic.ProductID = goodsInfo.GoodsID.ToString();
                            pic.IsDeafult = "1";
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                         goods.Pic;
                            Pic.Add(pic);
                        }
                        ordetail.ProductPicUrlList = Pic;

                        orderDetail.Add(ordetail);
                    }
                    order.OrderDetailList = orderDetail;
                    OrderList.Add(order);
                }
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "没有更多数据"};
            }

            #endregion

            return new ResultOrderList()
            {
                Result = "T",
                Description = "获取成功",
                OrderList = OrderList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderList：" + JSon);
            return new ResultOrderList() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 经销商订单信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderInfo GetResellerOrderDetail(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string ReceiptNo = string.Empty;
            string CreateDate = string.Empty;
            string Phone = string.Empty;
            string strWhere = string.Empty;
            string strsql = string.Empty;
            string pro_type = string.Empty;
            DataTable dt_operate = new DataTable();
            DataTable dt_pay = new DataTable();

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["ReceiptNo"].ToString() != "")
                // && JInfo["CreateDate"].ToString() != "" && JInfo["Phone"].ToString() != ""
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                ReceiptNo = JInfo["ReceiptNo"].ToString();
                CreateDate = JInfo["CreateDate"].ToString();
                Phone = JInfo["Phone"].ToString();
            }
            else
            {
                return new ResultOrderInfo() { Result = "F", Description = "传入参数异常" };
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one,0,int.Parse(disID == "" ? "0" : disID)))
                return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };

            #endregion

            ResultOrderInfo order = new ResultOrderInfo();
            Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (orderModel == null || orderModel.DisID.ToString() != disID || orderModel.dr == 1)
                return new ResultOrderInfo() {Result = "F", Description = "订单信息异常"};
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(orderModel.ID.ToString());
            //if(orderextModel == null)
                //return new ResultOrderInfo() { Result = "F", Description = "订单详情异常" };
            order.Result = "T";
            order.Description = "获取成功";
            order.OrderID = orderModel.ID.ToString();
            order.CompID = orderModel.CompID.ToString();
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
            if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                return new ResultOrderInfo() {Result = "F", Description = "企业异常"};
            order.CompName = comp.CompName;

            order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                orderModel.ReturnState);
            string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
            Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
            order.IsEnSend = IsEnSend;
            order.IsEnPay = IsEnPay;
            order.IsEnAudit = IsEnAudit;
            order.IsEnReceive = IsEnReceive;
            order.IsEnReturn = IsEnReturn;  

            order.AddType = orderModel.AddType.ToString();
            order.DisID = orderModel.DisID.ToString();
            order.DisUserID = orderModel.DisUserID.ToString();
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
            //if (user == null || user.IsEnabled == 0 || user.dr == 1)
            //    return new ResultOrderInfo() {Result = "F", Description = "经销商用户信息异常"};
            if (user != null && user.IsEnabled == 1 && user.dr == 0)
            {
                order.DisUserName = user.TrueName;
            }
            else
            {
                order.DisUserName = "";
            }
            order.DisUserName = user.TrueName;
            order.AddrID = orderModel.AddrID.ToString();
            order.ReceiptNo = orderModel.ReceiptNo;
            if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00")
            order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
            //获取
            //strsql = "select vdef8 from Dis_Order where ReceiptNo = '" + ReceiptNo + "' and DisID = '" + disID + "' and CompID = '" + orderModel.CompID.ToString() + "'";
            //strsql += " and isnull(dr,0) = 0";
            //order.Rebate = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
            order.OrderRemark = orderModel.Remark;//订单备注
            //if (orderextModel == null)
            //{
            //    order.Rebate = ClsSystem.gnvl(orderModel.vdef8, "0");//返利
            //    order.Bill = orderModel.BillNo;//发票号
            //    order.BillState = ClsSystem.gnvl(orderModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";//发票状态
            //}
            //else
            //{
                order.Rebate = ClsSystem.gnvl(orderModel.bateAmount, "0");//返利
                order.Bill = orderextModel.BillNo;//发票号
                order.BillState = ClsSystem.gnvl(orderextModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";//发票状态
            //}

            
            //根据促销ID取促销的TYPE
            //if (orderextModel == null)
            //{
            //    strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderModel.vdef4, "-1") + "";
            //}
            //else
            //{
                strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderextModel.ProID, "-1") + "";
            //}
            pro_type = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (pro_type == "2")//促销type为2的是整单促销，0是特价促销，1是商品促销
            {
                order.IsOrderPro = "1";//为整单促销
            }
            else
            {
                order.IsOrderPro = "0";//不是整单促销
            }
            //获取促销明细
            BD_GoodsCategory.ResultOrderPro orderpro = new BD_GoodsCategory.ResultOrderPro();
            //if (orderextModel == null)
            //{
            //    if (ClsSystem.gnvl(orderModel.vdef4, "") != "" && ClsSystem.gnvl(orderModel.vdef4, "") != "0")
            //    {
            //        orderpro.ProID = orderModel.vdef4;//促销ID

            //        strsql = "select protype from BD_Promotion where ID = " + orderModel.vdef4 + "";
            //        orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            //        orderpro.OrderPrice = ClsSystem.gnvl(orderModel.vdef5, "0.00");//促销金额
            //        string ProIDD = "";
            //        string ProType = "";
            //        string pro_id = "";
            //        //获取ProIDD，ProType为后面拼接促销详情提供参数
            //        decimal pro_num = Common.GetProPrice(orderModel.TotalAmount, out pro_id, out ProIDD, out ProType, orderModel.CompID);
            //        //调用方法拼接促销详情
            //        orderpro.Discount = Common.proOrderType(ProIDD, ClsSystem.gnvl(orderModel.vdef5, "0"), ProType);
            //    }
            //    else
            //    {
            //        orderpro.ProID = "0";
            //        orderpro.ProType = "0";
            //        orderpro.OrderPrice = "";
            //        orderpro.Discount = "";
            //    }
            //}
            //else
            //{
                if (ClsSystem.gnvl(orderextModel.ProID, "") != "" && ClsSystem.gnvl(orderextModel.ProID, "") != "0")
                {
                    orderpro.ProID = orderextModel.ProID.ToString();//促销ID

                    strsql = "select protype from BD_Promotion where ID = " + orderextModel.ProID + "";
                    orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    orderpro.OrderPrice = ClsSystem.gnvl(orderextModel.ProAmount, "0.00");//促销金额
                    string ProIDD = orderextModel.ProDID.ToString();
                    //调用方法拼接促销详情
                    //根据促销明细ID，取出促销明细实体
                    Hi.Model.BD_PromotionDetail2 model_prodetail2 = new Hi.BLL.BD_PromotionDetail2().GetModel(orderextModel.ProDID);
                    //拼接protype字符串，以便调用proOrderType获得促销详情
                    string ProType = "";
                    if (orderpro.ProType == "5")//表示满减
                        ProType = "5," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    else if (orderpro.ProType == "6")//表示满折
                        ProType = "6," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    else
                        return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
                    //调用方法拼接促销详情
                    orderpro.Discount = Common.proOrderType(ProIDD, ClsSystem.gnvl(orderextModel.ProAmount, "0"), ProType);
                }
                else
                {
                    orderpro.ProID = "0";
                    orderpro.ProType = "0";
                    orderpro.OrderPrice = "";
                    orderpro.Discount = "";
                }
            //}
            order.OrderPro = orderpro;


            if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))
            {
                Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                if (addr != null)
                {
                    order.Zip = addr.Zip;
                }
            }
            order.Address = orderModel.Address;
            order.Contact = orderModel.Principal;
            order.Phone = orderModel.Phone;

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
            if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                return new ResultOrderInfo() {Result = "F", Description = "经销商异常"};
            order.DisName = dis.DisName;
            order.Otype = orderModel.Otype.ToString();
            order.OtherAmount = orderModel.OtherAmount.ToString("0.00");
            if (orderModel.OState == 4)
            {

                List<Hi.Model.DIS_OrderOut> orderoutlist = new Hi.BLL.DIS_OrderOut().GetList("", "OrderID=" + orderModel.ID + " and isnull(issign,0) = 0 and isnull(IsAudit,0) <> 3 and isnull(dr,0) = 0", "");
                if (orderoutlist == null || orderoutlist.Count == 0)
                {
                    order.Ostate = "5";
                }
                else
                {
                    order.Ostate = orderModel.OState.ToString();
                }
            }
            else
            {
                order.Ostate = orderModel.OState.ToString();
            }
            order.PayState = orderModel.PayState.ToString();
            order.ReturnState = orderModel.ReturnState.ToString();
            if (ClsSystem.gnvl(orderModel.AuditUserID, "0") != "0")
            {
                Hi.Model.SYS_Users auditUser = new Hi.BLL.SYS_Users().GetModel(orderModel.AuditUserID);
                if (auditUser == null)//|| auditUser.IsEnabled == 0 || auditUser.dr == 1
                    return new ResultOrderInfo() {Result = "F", Description = "审核人信息异常"};
                order.AuditUserName = auditUser.UserName;
                order.AuditUserID = orderModel.AuditUserID.ToString();
                order.AuditDate = orderModel.AuditDate.ToString();
                order.AuditRemark = orderModel.AuditRemark;
            }
            order.TotalAmount = orderModel.TotalAmount.ToString("0.00");
            order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
            order.PayedAmount = orderModel.PayedAmount.ToString("0.00");
            order.CreateUserID = orderModel.CreateUserID.ToString();
            order.CreateDate = orderModel.CreateDate.ToString("yyyy-MM-dd HH:mm");
            order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
            order.ReturnMoneyUser = orderModel.ReturnMoneyUser;
            order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();
            
            //获取操作日志
#region
            List<Operating> list_operate = new List<Operating>();
            //将此订单的操作日志全取出放入dt中
            strsql = "select LogType,LogTime,OperatePerson,LogRemark from SYS_SysBusinessLog where ApplicationId = " + orderModel.ID + "";
            strsql += " and isnull(dr,0) = 0 and LogClass = 'Order' and CompID = "+orderModel.CompID+"";
            dt_operate = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_operate.Rows.Count; i++)
            {
                Operating operate = new Operating();
                operate.LogType = ClsSystem.gnvl(dt_operate.Rows[i]["LogType"], "");
                operate.LogTime = ClsSystem.gnvl(dt_operate.Rows[i]["LogTime"], "");
                operate.OperatePerson = ClsSystem.gnvl(dt_operate.Rows[i]["OperatePerson"], "");
                operate.LogRemark = ClsSystem.gnvl(dt_operate.Rows[i]["LogRemark"],"");
                list_operate.Add(operate);
            }
            order.LogList = list_operate;

            
#endregion

            //获取支付明细
#region
            List<Pay> list_pay = new List<Pay>();
            //将订单的支付明细取出放在DT里
            strsql = "select comp.CompName as 核心企业名称,pay.DisName as 经销商名称,pay.Source as 类型,pay.Price as 支付金额,pay.Date as 支付日期,pay.sxf as 手续费 ";
            strsql += " from CompCollection_view pay inner join BD_Company  comp on pay.CompID = comp.ID";
            strsql += " where OrderID = " + orderModel.ID + " and isnull(pay.vedf9,0) = 1 order by pay.storID desc ";
            dt_pay = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_pay.Rows.Count; i++)
            {
                Pay pay = new Pay();
                pay.CompName = ClsSystem.gnvl(dt_pay.Rows[i]["核心企业名称"], "");
                pay.ResellerName = ClsSystem.gnvl(dt_pay.Rows[i]["经销商名称"], "");
                pay.PayLogType = ClsSystem.gnvl(dt_pay.Rows[i]["类型"], "");
                pay.PayDate = ClsSystem.gnvl(dt_pay.Rows[i]["支付日期"], "");
                pay.PayAmount = ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0");
                pay.FeeAmount = ClsSystem.gnvl(dt_pay.Rows[i]["手续费"], "0");
                list_pay.Add(pay);
            }
            order.PayLogList = list_pay;
   
#endregion
            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (outList.Count != 0)
            {
                Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                foreach (Hi.Model.DIS_OrderOut Out in outList)
                {
                    orderOut = Out;
                }
                //根据发货单取对应的物流信息
                List<Hi.Model.DIS_Logistics> list_log = new Hi.BLL.DIS_Logistics().GetList("","OrderOutID = "+orderOut.ID+" and isnull(dr,0) = 0","");
                //一个发货单只有一条物流信息，所以list_log里也只有一条数据
                Hi.Model.DIS_Logistics model_log = list_log[0];
                order.SendID = orderOut.ID.ToString();
                order.SendDate = orderOut.SendDate.ToString();
                order.Express = model_log.ComPName;
                order.ExpressNo = model_log.LogisticsNo;
                //order.ExpressPerson = orderOut.ExpressPerson;
                //order.ExpressTel = orderOut.ExpressTel;
                //order.ExpressBao = orderOut.ExpressBao;
                order.PostFee = orderModel.PostFee.ToString("0.00");
                order.ActionUser = orderOut.ActionUser;
                List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                if (exlist != null)
                {
                    if (exlist[0].Context.IndexOf("context") >= 0 || exlist[0].Context.IndexOf("content") >= 0)
                    {

                        order.SendRemark = exlist[0].Context;
                    }
                }
                order.IsAudit = orderModel.IsAudit.ToString();
                order.SignDate = orderOut.SignDate.ToString();
                order.IsSign = orderOut.IsSign.ToString();
                order.SignUserId = orderOut.SignUserId.ToString();
                order.SignUser = orderOut.SignUser;
                order.SignRemark = orderOut.SignRemark;
            }
            //todo:不知道的排序
            //order.SortIndex = orderModel.SortIndex.ToString();               
            order.IsDel = orderModel.dr.ToString();

            //明细
            List<OrderDetail> orderDetail = new List<OrderDetail>();
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultOrderInfo() {Result = "F", Description = "订单明细异常"};
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                string SKUName = string.Empty;
                OrderDetail ordetail = new OrderDetail();
                ordetail.SKUID = detail.GoodsinfoID.ToString();
                //通过GoodsInfoID找到GoodsID
                Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                if (goodsInfo == null)
                //if (goodsInfo == null || goodsInfo.dr == 1 || goodsInfo.IsEnabled == false)
                    return new ResultOrderInfo() {Result = "F", Description = "SKU信息异常"};
                ordetail.ProductID = goodsInfo.GoodsID.ToString();

                //通过GoodsID找到GoodsName
                Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                if (goods == null )
                    return new ResultOrderInfo() {Result = "F", Description = "商品异常"};
                ordetail.ProductName = goods.GoodsName;
                ordetail.Unit = goods.Unit;
                SKUName += goods.GoodsName;

                list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                if (list_attrs != null && list_attrs.Count != 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                    {
                        SKUName += attr.AttrsName;
                    }
                }
                ordetail.SKUName = SKUName;
                //todo:描述是什么 GoodsInfo.ValueInfo?
                ordetail.ValueInfo = goodsInfo.ValueInfo;
                ordetail.SalePrice = detail.GoodsPrice.ToString("0.00");
                ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                ordetail.Num = detail.GoodsNum.ToString("0.00");
                ordetail.Remark = detail.Remark;
                ordetail.IsPro = ClsSystem.gnvl(detail.ProID, "").Trim() == "0" || ClsSystem.gnvl(detail.ProID, "").Trim() == "" ? "0" : detail.vdef1; //是否是促销商品
                if (ordetail.IsPro != "0")
                {
                    ordetail.ProNum = detail.ProNum;
                    if (ClsSystem.gnvl(detail.ProID, "") != "" && ClsSystem.gnvl(detail.ProID, "").Length > 0)
                    {
                        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.ProID));
                        if (pro != null)
                        {
                            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("",
                                " ProID=" + pro.ID + " and GoodInfoID ='" + ordetail.SKUID + "' and dr=0", "");
                            string info = string.Empty;
                            if (dList != null && dList.Count > 0)
                            {
                                if (pro.Type == 0 && pro.ProType == 1)
                                {
                                    info = "赠品";
                                }
                                else if (pro.Type == 0 && pro.ProType == 2)
                                {
                                    info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                }
                                else if (pro.Type == 1 && pro.ProType == 3)
                                {
                                    info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;

                                }
                                else if (pro.Type == 1 && pro.ProType == 4)
                                {
                                    info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                }
                            }
                            ordetail.proInfo = new DIS_Order.PromotionInfo()
                            {
                                ProID = detail.ProID,
                                ProTitle = pro.ProTitle,
                                ProInfos = info,
                                Type = pro.Type.ToString(),
                                ProTpye = pro.ProType.ToString(),
                                Discount = pro.Discount.ToString("0.00"),
                                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                            };
                        }
                    }
                }

                List<Pic> Pic = new List<Pic>();
                if (goods.Pic != "" && goods.Pic != "X")
                {
                    Pic pic = new Pic();
                    pic.ProductID = goodsInfo.GoodsID.ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 goods.Pic;
                    Pic.Add(pic);
                }
                ordetail.ProductPicUrlList = Pic;

                orderDetail.Add(ordetail);
            }
            order.OrderDetailList = orderDetail;

            return order;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderDetailList：" + JSon);
            return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 经销商确认收货
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOutConfirm ConfirmReceipt(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string disID = string.Empty;
            string ReceiptNo = string.Empty;
            string strWhere = string.Empty;
            //string ts_order = string.Empty;
            //int ists = 0;
            decimal signnum = 0;
            decimal goodsnum = 0;
            int isupdate = 0;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["ReceiptNo"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                ReceiptNo = JInfo["ReceiptNo"].ToString();
            }
            else
            {
                return new ResultOutConfirm() {Result = "F", Description = "参数异常"};
            }
            Hi.Model.SYS_Users userone = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out userone, 0,int.Parse(disID == "" ? "0" : disID)))
                return new ResultOutConfirm() { Result = "F", Description = "未找到用户信息" };
            strWhere += " DisID='" + disID + "' and ISNULL(dr,0)=0";


            #endregion

            Hi.Model.DIS_Order order = new Common().GetOrderByReceiptNo(ReceiptNo);
            //ts_order = order.ts.ToString();
            if (order == null)
                return new ResultOutConfirm() {Result = "F", Description = "订单编号异常"};
            if (order.dr == 1)
                return new ResultOutConfirm() {Result = "F", Description = "订单已删除"};
            if (order.OState != (int) Enums.OrderState.已发货)
                return new ResultOutConfirm() {Result = "F", Description = "订单非发货状态"};


            strWhere += " and OrderID='" + order.ID + "'";
            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("", strWhere, "");
            if (outList == null || outList.Count == 0)
                return new ResultOutConfirm() {Result = "F", Description = "发货单异常"};
            Hi.Model.DIS_OrderOut orderOut = new DIS_OrderOut();
            List<Hi.Model.DIS_OrderOutDetail>  outdetaillist = null;
            List<Hi.Model.DIS_OrderDetail> orderdetaillist = new Hi.BLL.DIS_OrderDetail().GetList("", "orderid = " + order.ID, "");
            //foreach (Hi.Model.DIS_OrderOut disOrderOut in outList)
            //{
            //    if (disOrderOut.IsSign == 1)
            //        continue;
            //    isupdate = 1;
            //    disOrderOut.IsSign = 1;
            //    disOrderOut.SignDate = DateTime.Now;
            //    disOrderOut.SignUserId = UserID.ToInt();
            //    disOrderOut.SignUser = userone.TrueName;
            //    disOrderOut.ts = DateTime.Now;
            //    disOrderOut.modifyuser = UserID.ToInt();
            //    bool res = new Hi.BLL.DIS_OrderOut().Update(orderOut, TranSaction);
            //}

            //order.OState = (int) Enums.OrderState.已到货;
            //order.ts = DateTime.Now;
            //order.modifyuser = int.Parse(UserID);
            //if (orderOut.IsSign == 1)
            //    return new ResultOutConfirm() {Result = "F", Description = "已经签收"};
            //orderOut.SignDate = DateTime.Now;
            //orderOut.IsSign = 1; //0：未签收 1：已签收
            //orderOut.SignUserId = int.Parse(UserID);
            //Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(orderOut.SignUserId);
            //if (user == null || user.IsEnabled == 0 || user.dr == 1)
            //    return new ResultOutConfirm() {Result = "F", Description = "操作人信息异常"};
            //orderOut.SignUser = user.TrueName;
            //orderOut.SignRemark = "";
            //orderOut.ts = DateTime.Now;
            //orderOut.modifyuser = int.Parse(UserID);

            SqlTransaction TranSaction = null;
            try
            {
                SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
                if (Connection.State.ToString().ToLower() != "open")
                {
                    Connection.Open();
                }
                TranSaction = Connection.BeginTransaction();
                foreach (Hi.Model.DIS_OrderDetail orderdetail in orderdetaillist)
                {
                    goodsnum += orderdetail.GoodsNum + decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum, "0") == "" ? "0" : ClsSystem.gnvl(orderdetail.ProNum, "0"));
                }
                //更新此订单对应的未签收的收货单
                foreach (Hi.Model.DIS_OrderOut disOrderOut in outList)
                {
                    outdetaillist = new  Common().GetOrderOutDetailList("", " OrderOutID=" + disOrderOut.ID, "", TranSaction);
                    if (disOrderOut.IsSign == 1)//此发货单已签收的话，跳过本次循环，并累计其明细表的签收数量
                    {
                        foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in outdetaillist)
                        {
                            signnum += orderoutdetail.SignNum;//已签收的累加签收数量
                        }
                        continue;
                    }
                //未签收的发货单，进行更新
                    isupdate = 1;
                    disOrderOut.IsSign = 1;
                    disOrderOut.SignDate = DateTime.Now;
                    disOrderOut.SignUserId = UserID.ToInt();
                    disOrderOut.SignUser = userone.TrueName;
                    disOrderOut.ts = DateTime.Now;
                    disOrderOut.modifyuser = UserID.ToInt();
                    //更新发货单
                    bool res = new Hi.BLL.DIS_OrderOut().Update(disOrderOut, TranSaction);
                    if (res == false)
                    {
                        TranSaction.Rollback();
                        return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
                    }
                    //未签收的发货单，累加发货数量
                    foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in outdetaillist)
                    {
                        signnum += orderoutdetail.OutNum;
                        orderoutdetail.SignNum = orderoutdetail.OutNum;
                        int conutd = new Hi.BLL.DIS_OrderOutDetail().Update(orderoutdetail, TranSaction);
                        if (conutd <= 0)
                        {
                            TranSaction.Rollback();
                            return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
                        }
                    }
                }
                if (isupdate == 0)
                {
                    TranSaction.Rollback();
                    return new ResultOutConfirm() { Result = "F", Description = "无可收货的发货单" };
                }
                if (order.IsOutState == (int)Enums.IsOutState.全部发货)
                {
                    //签收数量等于订单商品数量 ==全部到货
                    if (signnum == goodsnum)
                    {
                        
                        order.IsOutState = (int)Enums.IsOutState.全部到货;
                        order.OState = (int)Enums.OrderState.已到货;
                    }
                    else
                    {
                        order.IsOutState = (int)Enums.IsOutState.全部发货;
                        order.OState = (int)Enums.OrderState.已发货;
                    }
                }
                else if (order.IsOutState == (int)Enums.IsOutState.部分发货 || order.IsOutState == (int)Enums.IsOutState.部分到货)
                {
                    order.IsOutState = (int)Enums.IsOutState.部分到货;
                    order.OState = (int)Enums.OrderState.已发货;
                }
                order.ts = DateTime.Now;
                order.modifyuser = UserID.ToInt();
                //ists = new Hi.BLL.DIS_Order().Getts("DIS_Order",order.ID,ts_order.ToDateTime());
                //if (ists != 1)
                //{
                //    TranSaction.Rollback();
                //    return new ResultOutConfirm() { Result = "F", Description = "确认失败" };
                //}

                bool res_order= new Hi.BLL.DIS_Order().Update(order, TranSaction);
                if (res_order == false)
                {
                    TranSaction.Rollback();
                    return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
                }
                //bool res = new Hi.BLL.DIS_OrderOut().Update(orderOut, TranSaction);
                //if (res == false)
                //{
                //    TranSaction.Rollback();
                //    return new ResultOutConfirm() {Result = "F", Description = "确认失败"};
                //}

                //bool re = new Hi.BLL.DIS_Order().Update(order, TranSaction);
                //if (re == false)
                //{
                //    TranSaction.Rollback();
                //    return new ResultOutConfirm() {Result = "F", Description = "确认失败"};
                //}

                string str = Common.AddSysBusinessLog(order, userone, "Order",
                    order.ID.ToString(), "订单签收", "", TranSaction);
                if (str == "0" || str == "false")
                {
                    TranSaction.Rollback();
                    return new ResultOutConfirm() {Result = "F", Description = "确认收货失败"};
                }

                TranSaction.Commit();

                new MsgSend().GetWxService("3", order.ID.ToString(), "1");

                return new ResultOutConfirm() {Result = "T", Description = "收货成功", ReceiptNo = order.ReceiptNo};
            }
            catch
            {
                if (TranSaction != null)
                {
                    if (TranSaction.Connection != null)
                    {
                        TranSaction.Rollback();
                    }
                }
                return new ResultOutConfirm() {Result = "F", Description = "更新异常"};
            }
            finally
            {
                if (TranSaction != null)
                {
                    if (TranSaction.Connection != null)
                    {
                        TranSaction.Connection.Close();
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":"+ex.StackTrace, "ConfirmReceipt:"+JSon);
            return new ResultOutConfirm() {Result = "F", Description = "参数异常"};
        }
    }

    #endregion

    #region 企业

    /// <summary>
    /// 订单信息列表-按照商品group
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultGoodsSaleList GetGoodsSaleReport(string JSon)
    {
        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo.Count == 0 || !JInfo.Keys.Contains("UserID") || !JInfo.Keys.Contains("CompanyID"))
        {
            return new ResultGoodsSaleList() { Result = "F", Description = "参数异常" };
        }

        #region JSon取值
        string userID = JInfo["UserID"].ToString();
        string compID = JInfo["CompanyID"].ToString();

        if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(compID))
        {
            return new ResultGoodsSaleList() { Result = "F", Description = "参数异常" };
        }

        string strWhere = string.Empty;
        #endregion

        Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
        if (!new Common().IsLegitUser(int.Parse(userID), out one, int.Parse(compID == "" ? "0" : compID)))
        {
            return new ResultGoodsSaleList() { Result = "F", Description = "登录信息异常" };
        }

        JsonData JMsg = JInfo["Search"];
        if (JMsg.Count > 0)
        {
            if (JMsg["BeginDate"].ToString().Trim() != "-1")
            {
                strWhere += " and o.CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
            }
            if (JMsg["EndDate"].ToString().Trim() != "-1")
            {
                strWhere += " and o.CreateDate < '" + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
            }
        }

        try {
            string sqlstr = string.Format(@"
                SELECT g.CompID, g.Pic, gi.ValueInfo, g.GoodsName , gi.BarCode, r.*
                FROM
                [dbo].[BD_GoodsInfo] gi, [dbo].[BD_Goods] g,
                (SELECT d.GoodsinfoID, sum(d.SharePrice) as Total, 
                    sum(d.GoodsNum) as GoodsCount, count(1) as OrderCount,
                    min(d.AuditAmount) as MinPrice, max(d.AuditAmount) as MaxPrice
                FROM [dbo].[DIS_OrderDetail] d, [dbo].[DIS_Order] o
                where o.compID='{0}' and o.OState in (2,3,4,5,7)
                and ISNULL(o.dr,0)=0 and o.Otype != 9
                and o.ID = d.OrderId
                {1}
                group by d.GoodsinfoID) r
                where r.GoodsinfoID = gi.ID and gi.GoodsID = g.ID", JInfo["CompanyID"].ToString(), strWhere);
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
            if (ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return new ResultGoodsSaleList() { Result = "T", Description = "没有数据" };
            }

            DataTable orderList = ds.Tables[0];
            List<GoodsSaleReport> resultList = new List<GoodsSaleReport>();

            foreach (DataRow row in orderList.Rows)
            {
                GoodsSaleReport goodsReport = new GoodsSaleReport();
                goodsReport.GoodsInfoID = row["GoodsinfoID"].ToString();
                goodsReport.Barcode = row["BarCode"].ToString();
                goodsReport.TotalAmount = Convert.ToDecimal(row["Total"]).ToString("0.##");
                goodsReport.OrderCount = Convert.ToDecimal(row["OrderCount"]).ToString("0.##");
                goodsReport.GoodsCount = Convert.ToDecimal(row["GoodsCount"]).ToString("0.##");
                goodsReport.Description = row["ValueInfo"].ToString();
                goodsReport.GoodsName = row["GoodsName"].ToString();
                goodsReport.MinPrice = Convert.ToDecimal(row["MinPrice"]).ToString("0.##");
                goodsReport.MaxPrice = Convert.ToDecimal(row["MaxPrice"]).ToString("0.##");
                goodsReport.GoodsPicUrl = Common.GetPicURL(row["Pic"].ToString(), "resize400", Convert.ToInt32(row["CompID"]));

                resultList.Add(goodsReport);
            }

            return new ResultGoodsSaleList()
            {
                Result = "T",
                Description = "获取成功",
                GoodsSaleList = resultList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetGoodsSaleReport:" + JSon);
            return new ResultGoodsSaleList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 订单信息列表-按照经销商group
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderList GetCompanyOrderListGroupByDistributor(string JSon)
    {
        try
        {
            string strWhere = string.Empty;

            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string state = string.Empty;
            //string oState = string.Empty;
            string payState = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["State"].ToString() != "")
            {
                //{"CompanyID":"1004","CriticalOrderID":"-1","GetType":"1","OState":"-2","PayState":"-1","Rows":"10","Search":{},"Sort":"0","SortType":"0","State":"2","UserID":"1006"}

                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                strWhere = " where o.compID='" + compID + "' and o.OState between 1 and 5";

                state = JInfo["State"].ToString();
                //if (JInfo["OState"].ToString() != "")
                //    oState = JInfo["OState"].ToString();
                if (JInfo["PayState"].ToString() != "")
                {
                    payState = JInfo["PayState"].ToString();
                }
            }
            else
            {
                return new ResultOrderList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one, int.Parse(compID == "" ? "0" : compID)))
            {
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };
            }

            if (!string.IsNullOrEmpty(payState) && payState != "-1")
            {
                strWhere += " and o.PayState in(" + payState + ")";
            }
                

            JsonData JMsg = JInfo["Search"];
            if (JMsg.Count > 0)
            {
                if (JMsg["BeginDate"].ToString().Trim() != "-1")
                {
                    strWhere += " and o.CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
                }
                if (JMsg["EndDate"].ToString().Trim() != "-1")
                {
                    strWhere += " and o.CreateDate < '" + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
                }
            }

            if (state != "0" && state != "2" && state != "14" && state != "7" && state != "9" && state != "13")
            {
                return new ResultOrderList() { Result = "F", Description = "状态异常" };
            }
     
            switch (state) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 
                            //6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消 
                            //13:待支付 14:退货退款待审核
            {
                case "2":
                    strWhere += " and (o.ostate=" + (int)Enums.OrderState.已审 +
                                //" and ((Otype in (" + (int)Enums.OType.销售订单 + "," + (int)Enums.OType.特价订单 + " )" +
                                //" and paystate in (" + (int) Enums.PayState.已支付 + "," + (int) Enums.PayState.部分支付 + " )" +
                                "  or (o.ostate = " + (int)Enums.OrderState.已发货 +
                                " and  isnull(o.IsOutState,4) in (1,2))) and o.otype = 0";
                    break;
                case "7":
                    strWhere += " and o.ostate=" + (int)Enums.OrderState.待审核 + " and o.ReturnState=" +
                                (int)Enums.ReturnState.未退货;
                    break;
                case "9":
                    strWhere += " and o.ostate=" + (int)Enums.OrderState.已发货;
                    break;
                case "13":
                    strWhere += " and ((o.Otype=1 and o.OState not in (-1,0,1)  and o.PayState in (0,1) ) or( o.Otype<>1 and o.OState in(2,4,5) and o.PayState in (0,1) )) and o.OState<>6 and o.CompID='"
                        + compID + "' and o.ReturnState<> 3 and isnull(o.dr,0)=0";
                    break;
                case "14":
                    strWhere += " and o.OState=5 and o.ReturnState ='" + (int)Enums.ReturnState.申请退货 + "'";
                    break;
            }

            strWhere += " and ISNULL(o.dr,0)=0 and o.Otype!=9 and o.OState !=" + (int)Enums.OrderState.退回;
            strWhere += " and o.DisID = dis.ID";
            #endregion

            #region 赋值

            string tabName = " [dbo].[DIS_Order] o, [dbo].[BD_Distributor] dis"; //表名
            string groupBy = " group by dis.ID, dis.DisName";
            string strSql = "SELECT dis.ID as DisId, dis.DisName, sum(o.TotalAmount) as Total, sum( o.PayedAmount) as Paid, count(1) as Cnt FROM " + tabName + strWhere + groupBy;

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql);
            if (ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return new ResultOrderList() { Result = "T", Description = "没有数据" };
            }
               
            DataTable orderList = ds.Tables[0];
            List<Order> resultList = new List<Order>();

            foreach (DataRow row in orderList.Rows)
            {
                Order order = new Order();
                order.DisID = Convert.ToString(row["DisId"]);
                order.DisName = Convert.ToString(row["DisName"]);
                order.TotalAmount = Convert.ToDecimal(row["Total"]).ToString("0.00");
                order.PayedAmount = Convert.ToDecimal(row["Paid"]).ToString("0.00");
                order.OrderCount = Convert.ToInt32(row["Cnt"]);

                resultList.Add(order);
            }

            #endregion

            return new ResultOrderList()
            {
                Result = "T",
                Description = "获取成功",
                OrderList = resultList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanyOrderListGroupByDistributor:" + JSon);
            return new ResultOrderList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderList GetCompanyOrderList(string JSon)
    {
        try
        {
            string strWhere = string.Empty;

            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string state = string.Empty;
            //string oState = string.Empty;
            string payState = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                JInfo["Sort"].ToString() != "" && JInfo["State"].ToString() != "")
            {
                //{"CompanyID":"1004","CriticalOrderID":"-1","GetType":"1","OState":"-2","PayState":"-1","Rows":"10","Search":{},"Sort":"0","SortType":"0","State":"2","UserID":"1006"}

                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                strWhere += " and compID='" + compID + "' and ISNULL(dr,0)=0 and OState between 1 and 7";
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                state = JInfo["State"].ToString();
                //if (JInfo["OState"].ToString() != "")
                //    oState = JInfo["OState"].ToString();
                if (JInfo["PayState"].ToString() != "")
                    payState = JInfo["PayState"].ToString();
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one,int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };

            //if (oState!="-2")
            //    strWhere += " and OState in (" + oState + ")";
            if (payState != "-1")
                strWhere += " and PayState in(" + payState + ")";

            JsonData JMsg = JInfo["Search"];
            if (JMsg.Count > 0)
            {
                //if (JMsg["OrderID"].ToString() != "")
                //    strWhere += " and ReceiptNo like '" + JMsg["OrderID"].ToString() + "%'";
                if (JMsg["BeginDate"].ToString().Trim() != "-1")
                {
                    strWhere += " and CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
                }
                if (JMsg["EndDate"].ToString().Trim() != "-1")
                {
                    strWhere += " and CreateDate < '" + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
                }
                //根据出库单编号 锁定 订单ID
                if (JMsg["ExpressNo"].ToString().Trim() != "-1")
                {
                    //订单编号
                    strWhere += " and( ReceiptNo like '%" + JMsg["ExpressNo"].ToString() + "%'";
                    //商品名称
                    string goodsName = Common.GetOrderByGoodsName(JMsg["ExpressNo"].ToString().Trim(), compID);
                    if (goodsName != "-1")
                    {
                        strWhere += " or ID in (" + goodsName + ") ";
                    }
                   
                    //物流编号
                    List<Hi.Model.DIS_OrderOut> orderOut = new Hi.BLL.DIS_OrderOut().GetList("",
                        " ReceiptNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                    if (orderOut.Count != 0)
                    {
                        strWhere += " or ID in (" + string.Join(",",orderOut.Select(p => p.OrderID)) + ")";
                    }
                    strWhere += " )";
                }
            }

            if (state != "0" && state != "2" && state != "14" && state != "7" && state != "9" && state != "13")
                return new ResultOrderList() {Result = "F", Description = "状态异常"};

            if (criticalOrderID == "-1")
            {
                switch (state) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 
                    //6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消 
                    //13:待支付 14:退货退款待审核
                {
                    case "2":
                        strWhere += " and (ostate=" + (int) Enums.OrderState.已审 +
                                    //" and ((Otype in (" + (int)Enums.OType.销售订单 + "," + (int)Enums.OType.特价订单 + " )" +
                                    //" and paystate in (" + (int) Enums.PayState.已支付 + "," + (int) Enums.PayState.部分支付 + " )" +
                                    "  or (ostate = " + (int)Enums.OrderState.已发货 + 
                                    " and  isnull(IsOutState,4) in (1,2))) and otype = 0";
                        break;
                    case "7":
                        strWhere += " and ostate=" + (int) Enums.OrderState.待审核 + " and ReturnState=" +
                                    (int) Enums.ReturnState.未退货;
                        break;
                    case "9":
                        strWhere += " and ostate=" + (int) Enums.OrderState.已发货;
                        break;
                    case "13":
                        strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and CompID='"
                            + compID + "' and ReturnState<> 3 and isnull(dr,0)=0";
                        break;
                    case "14":
                        strWhere += " and OState=5 and ReturnState ='" + (int)Enums.ReturnState.申请退货 + "'";
                        break;
                }
            }
            else
            {
                Hi.Model.DIS_Order Order = new Hi.BLL.DIS_Order().GetModel(int.Parse(criticalOrderID));
                if (Order != null && Order.dr != 1)
                {
                    switch (state) 
                    {
                        case "2":
                            //strWhere += " and ((Otype=" + (int) Enums.OType.销售订单 + " and ostate=" +
                            //            (int)Enums.OrderState.已审 + " and paystate in (" + (int)Enums.PayState.已支付 + "," + (int)Enums.PayState.部分支付 +
                            //            ")) or (Otype=" + (int) Enums.OType.赊销订单 + " and ostate= " +
                            //            (int) Enums.OrderState.已审 + " )" +
                            //            " or (Otype=" + (int) Enums.OType.特价订单 + " and ostate= " +
                            //            (int)Enums.OrderState.已审 + " and paystate in (" + (int)Enums.PayState.已支付 + "," + (int)Enums.PayState.部分支付 + " ))";
                            //strWhere += " or (ostate = "+ (int) Enums.OrderState.已发货+" and isnull(IsOutState,4) in (1,2)))";
                            //if (Order.Otype == (int) Enums.OType.赊销订单)
                            //{
                            //    if (Order.OState != (int) Enums.OrderState.已审)
                            //        return new ResultOrderList() {Result = "F", Description = "-1"};
                            //}
                            //else
                            //{
                            //    if (Order.OState != (int) Enums.OrderState.已审 ||
                            //        (Order.PayState != (int) Enums.PayState.已支付 && Order.PayState != (int)Enums.PayState.部分支付))
                            //        return new ResultOrderList() {Result = "F", Description = "-1"};
                            //}
                            strWhere += " and (ostate = " + (int)Enums.OrderState.已审 + " or (ostate = " + (int)Enums.OrderState.已发货 + " and isnull(isoutstate,4) in (1,2))) and otype = 0";
                            break;
                        case "7":
                            strWhere += " and ostate=" + (int) Enums.OrderState.待审核;
                            if (Order.OState != (int) Enums.OrderState.待审核)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                        case "9":
                            strWhere += " and ostate=" + (int) Enums.OrderState.已发货;
                            if (Order.OState != (int) Enums.OrderState.已发货)
                                return new ResultOrderList() {Result = "F", Description = "-1"};
                            break;
                        case "13":
                            strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and CompID='"
                                + compID + "' and ReturnState in(0,1) ";
                            break;
                        case "14":
                            strWhere += " and OState=5 and ReturnState ='" + (int)Enums.ReturnState.申请退货 + "'";
                            break;
                    }
                }
                else
                {
                    return new ResultOrderList() {Result = "F", Description = "查询单号异常"};
                }
            }

            strWhere += " and ISNULL(dr,0)=0 and OState != 0 and Otype!=9 and OState !=" + (int)Enums.OrderState.退回;

            #endregion

            #region 模拟分页

            string tabName = " [dbo].[DIS_Order]"; //表名
            string strsql = string.Empty; //搜索sql

            if (sortType == "1") //价格排序
            {
                sortType = "CreateDate";
            }
            else if (sortType == "2") //价格排序
            {
                sortType = "TotalAmount";
            }
            else
            {
                sortType = "ID";
            }
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResultOrderList() {Result = "F", Description = "基础数据异常"};

            #endregion

            #region 赋值

          
            List<Order> OrderList = new List<Order>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResultOrderList() {Result = "T", Description = "没有更多数据"};
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                    return new ResultOrderList() {Result = "T", Description = "没有更多数据"};
                foreach (DataRow row in orderList.Rows)
                {
                    Order order = new Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    if (orderModel == null || orderModel.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "订单异常"};
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "企业异常"};
                    order.CompName = comp.CompName;
                    
                    order.State = state.Trim()=="0"?(Common.GetCompOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                        orderModel.ReturnState)):state;
                    string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
                    Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                        orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
                    order.IsEnSend = IsEnSend;
                    order.IsEnPay = IsEnPay;
                    order.IsEnAudit = IsEnAudit;
                    order.IsEnReceive = IsEnReceive;
                    order.IsEnReturn = IsEnReturn;   

                    order.Otype = orderModel.Otype.ToString();
                    order.AddType = orderModel.AddType.ToString();
                    order.OState = orderModel.OState.ToString();
                    order.PayState = orderModel.PayState.ToString();
                    order.ReturnState = orderModel.ReturnState.ToString();
                    order.DisID = orderModel.DisID.ToString();
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
                    if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                        return new ResultOrderList() { Result = "F", Description = "经销信息商异常" };
                    order.DisName = dis.DisName;
                    order.DisUserID = orderModel.DisUserID.ToString();
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
                    //if (user == null || user.IsEnabled == 0 || user.dr == 1)
                    //    return new ResultOrderList() { Result = "F", Description = "经销商用户信息异常" };
                    //order.DisUserName = orderModel.DisUserName;
                    if (user != null && user.IsEnabled == 1 && user.dr == 0)
                    {
                        order.DisUserName = user.TrueName;
                    }
                    else
                    {
                        order.DisUserName = "";
                    }
                    order.AddrID = orderModel.AddrID.ToString();
                    order.ReceiptNo = orderModel.ReceiptNo;
                    if(ClsSystem.gnvl(orderModel.ArriveDate,"") != "0001/1/1 0:00:00" && ClsSystem.gnvl(orderModel.ArriveDate,"")!= "")
                    order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
                    if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))
                    {
                        Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                        if (addr != null)
                        {
                            order.Zip = addr.Zip;
                        }
                    }
                    order.Address = orderModel.Address;
                    order.Contact = orderModel.Principal;
                    order.Phone = orderModel.Phone;
                    order.TotalAmount = orderModel.AuditAmount.ToString("0.00");
                    order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
                    order.PayedAmount = orderModel.PayedAmount.ToString("0.00");
                    order.CreateUserID = orderModel.CreateUserID.ToString();
                    order.CreateDate = orderModel.CreateDate.ToString("yyyy-MM-dd HH:mm");
                    order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
                    order.ReturnMoneyUser = orderModel.ReturnMoneyUser;
                    order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();

                    List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                        " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                        orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (outList.Count != 0)
                    {
                        Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                        foreach (Hi.Model.DIS_OrderOut Out in outList)
                        {
                            orderOut = Out;
                        }
                        order.SendID = orderOut.ID.ToString();
                        order.SendDate = orderOut.SendDate.ToString();
                        ////order.Express = orderOut.Express;
                        ////order.ExpressNo = orderOut.ExpressNo;
                        ////order.ExpressPerson = orderOut.ExpressPerson;
                        ////order.ExpressTel = orderOut.ExpressTel;
                        ////order.ExpressBao = orderOut.ExpressBao;
                        ////order.PostFee = orderOut.PostFee.ToString("0.00");
                        order.ActionUser = orderOut.ActionUser;
                        List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                        if (exlist != null)
                            order.SendRemark = exlist[0].Context;
                        order.IsAudit = orderOut.IsAudit.ToString();
                        order.AuditUserID = orderOut.AuditUserID.ToString();
                        order.AuditDate = orderOut.AuditDate.ToString();
                        order.AuditRemark = orderOut.AuditRemark == null ? "" : orderOut.AuditRemark.ToString();
                        order.SignDate = orderOut.SignDate.ToString();
                        order.IsSign = orderOut.IsSign.ToString();
                        order.SignUserId = orderOut.SignUserId.ToString();
                        order.SignUser = orderOut.SignUser;
                        order.SignRemark = orderOut.SignRemark;
                    }
                    //todo:不知道的排序
                    //order.SortIndex = orderModel.SortIndex.ToString();               
                    order.IsDel = orderModel.dr.ToString();

                    //明细
                    List<OrderDetail> orderDetail = new List<OrderDetail>();
                    List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                        " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (detailList == null) //|| detailList.Count==0
                        return new ResultOrderList() {Result = "F", Description = "订单明细异常"};
                    List<Hi.Model.BD_GoodsAttrs>  list_attrs= null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        string SKUName = string.Empty;
                        OrderDetail ordetail = new OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        if (goodsInfo == null )
                        //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                            return new ResultOrderList() {Result = "F", Description = "SKU信息异常"};
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null)
                        //if (goods == null || goods.IsEnabled == 0 | goods.dr == 1)
                            return new ResultOrderList() {Result = "F", Description = "商品异常"};
                        ordetail.ProductName = goods.GoodsName;
                        SKUName += goods.GoodsName;

                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                            {
                                SKUName += attr.AttrsName;
                            }
                        }
                        ordetail.SKUName = SKUName;
                        //todo:描述是什么
                        ordetail.ValueInfo = goodsInfo.ValueInfo;
                        ordetail.SalePrice = detail.Price.ToString("0.00");
                        ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                        ordetail.Num = detail.GoodsNum.ToString("0.00");
                        ordetail.Remark = detail.Remark;
                        //ordetail.IsPro =  .Trim() == "0" || detail.vdef1.Trim() == "" ? "0" : "1"; //是否是促销商品
                        //是否是促销商品
                        ordetail.IsPro = ClsSystem.gnvl(detail.vdef1, "").Trim() == "0" || ClsSystem.gnvl(detail.vdef1, "").Trim() == "" ? "0" : "1";

                        if (ordetail.IsPro != "0")
                        {
                            ordetail.ProNum = detail.vdef5;
                            if (detail.vdef1 != "" && detail.vdef1.Length > 0)
                            {
                                Hi.Model.BD_Promotion pro =
                                    new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.vdef1));
                                if (pro != null)
                                {
                                    List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList(
                                        "", " ProID=" + pro.ID + " and GoodInfoID ='" + ordetail.SKUID + "' and dr=0",
                                        "");
                                    string info = string.Empty;
                                    if (dList != null && dList.Count > 0)
                                    {
                                        if (pro.Type == 0 && pro.ProType == 1)
                                        {
                                            info = "赠品";
                                        }
                                        else if (pro.Type == 0 && pro.ProType == 2)
                                        {
                                            info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 3)
                                        {
                                            info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                        }
                                        else if (pro.Type == 1 && pro.ProType == 4)
                                        {
                                            info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                        }
                                    }
                                    ordetail.proInfo = new DIS_Order.PromotionInfo()
                                    {
                                        ProID = detail.vdef1,
                                        ProTitle = pro.ProTitle,
                                        ProInfos = info,
                                        Type = pro.Type.ToString(),
                                        ProTpye = pro.ProType.ToString(),
                                        Discount = pro.Discount.ToString("0.00"),
                                        ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                        ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                                    };
                                }
                            }
                        }

                        List<Pic> Pic = new List<Pic>();
                        if (goods.Pic != "" && goods.Pic != "X")
                        {
                            Pic pic = new Pic();
                            pic.ProductID = goodsInfo.GoodsID.ToString();
                            pic.IsDeafult = "1";
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                         goods.Pic;
                            Pic.Add(pic);
                        }
                        ordetail.ProductPicUrlList = Pic;

                        orderDetail.Add(ordetail);
                    }
                    order.OrderDetailList = orderDetail;
                    OrderList.Add(order);
                }
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "没有更多数据"};
            }

            #endregion

            return new ResultOrderList()
            {
                Result = "T",
                Description = "获取成功",
                OrderList = OrderList
            };
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message +":"+ex.StackTrace , "GetCompanyOrderList:"+ JSon);
            return new ResultOrderList() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderInfo GetCompanyOrderDetail(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string compID = string.Empty;
            string ReceiptNo = string.Empty;
            string CreateDate = string.Empty;
            string Phone = string.Empty;
            string strWhere = string.Empty;
            string strsql = string.Empty;
            DataTable dt_operate = new DataTable();
            DataTable dt_pay = new DataTable();
            string pro_type = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["CompanyID"].ToString() != "" && JInfo["ReceiptNo"].ToString() != "")
            //  JInfo.Count > 0 && JInfo["UserID"].ToString() != "" &&&& JInfo["CreateDate"].ToString() != "" && JInfo["Phone"].ToString() != ""
            {
                UserID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                ReceiptNo = JInfo["ReceiptNo"].ToString();
                CreateDate = JInfo["CreateDate"].ToString();
                Phone = JInfo["Phone"].ToString();
            }
            else
            {
                return new ResultOrderInfo() { Result = "F", Description = "传入参数异常" };
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one,int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };

            #endregion

            

            ResultOrderInfo order = new ResultOrderInfo();
            Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (orderModel == null || orderModel.CompID.ToString() != compID || orderModel.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "订单信息异常" };
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(orderModel.ID.ToString());
            //if (orderextModel == null)
            //return new ResultOrderInfo() { Result = "F", Description = "订单详情异常" };
            order.Result = "T";
            order.Description = "获取成功";
            order.OrderID = orderModel.ID.ToString();
            order.CompID = orderModel.CompID.ToString();
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
            if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "企业异常" };
            order.CompName = comp.CompName;
            //todo:订单状态
            order.State = Common.GetCompOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                orderModel.ReturnState);
            string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
            Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
            order.IsEnSend = IsEnSend;
            order.IsEnPay = IsEnPay;
            order.IsEnAudit = IsEnAudit;
            order.IsEnReceive = IsEnReceive;
            order.IsEnReturn = IsEnReturn;

            order.DisID = orderModel.DisID.ToString();
            order.DisUserID = orderModel.DisUserID.ToString();
            order.DisUserID = orderModel.DisUserID.ToString();
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
            if (user == null)//|| user.IsEnabled == 0 || user.dr == 1
                return new ResultOrderInfo() { Result = "F", Description = "经销商用户信息异常" };
            order.DisUserName = user.TrueName;
            order.AddrID = orderModel.AddrID.ToString();
            order.AddType = orderModel.AddType.ToString();
            order.ReceiptNo = orderModel.ReceiptNo;
            if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "")
                order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");

            order.OrderRemark = orderModel.Remark;
            //if (orderextModel == null)//改数据库结构之前的数据的查询
            //{
            //    order.Rebate = ClsSystem.gnvl(orderModel.vdef8, "0");
            //    order.Bill = orderModel.BillNo;
            //    order.BillState = ClsSystem.gnvl(orderModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";
            //}
            //else
            //{
                order.Rebate = ClsSystem.gnvl(orderModel.bateAmount, "0");//返利
                order.Bill = orderextModel.BillNo;//发票号
                order.BillState = ClsSystem.gnvl(orderextModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";//发票状态
            //}


            //获取操作日志
            #region
            List<Operating> list_operate = new List<Operating>();
            //将此订单的操作日志全取出放入dt中
            strsql = "select LogType,LogTime,OperatePerson,LogRemark from SYS_SysBusinessLog where ApplicationId = " + orderModel.ID + "";
            strsql += " and isnull(dr,0) = 0 and LogClass = 'Order' and CompID = " + orderModel.CompID + "";
            dt_operate = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_operate.Rows.Count; i++)
            {
                Operating operate = new Operating();
                operate.LogType = ClsSystem.gnvl(dt_operate.Rows[i]["LogType"], "");
                operate.LogTime = ClsSystem.gnvl(dt_operate.Rows[i]["LogTime"], "");
                operate.OperatePerson = ClsSystem.gnvl(dt_operate.Rows[i]["OperatePerson"], "");
                operate.LogRemark = ClsSystem.gnvl(dt_operate.Rows[i]["LogRemark"], "");
                list_operate.Add(operate);
            }
            order.LogList = list_operate;


            #endregion

            //获取支付明细
            #region
            List<Pay> list_pay = new List<Pay>();
            //将订单的支付明细取出放在DT里
            strsql = "select comp.CompName as 核心企业名称,pay.DisName as 经销商名称,pay.Source as 类型,pay.Price as 支付金额,pay.Date as 支付日期,pay.sxf as 手续费 ";
            strsql += " from CompCollection_view pay inner join BD_Company  comp on pay.CompID = comp.ID";
            strsql += " where OrderID = " + orderModel.ID + " and isnull(pay.vedf9,0) = 1 order by pay.storID desc ";
            dt_pay = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_pay.Rows.Count; i++)
            {
                Pay pay = new Pay();
                pay.CompName = ClsSystem.gnvl(dt_pay.Rows[i]["核心企业名称"], "");
                pay.ResellerName = ClsSystem.gnvl(dt_pay.Rows[i]["经销商名称"], "");
                pay.PayLogType = ClsSystem.gnvl(dt_pay.Rows[i]["类型"], "");
                pay.PayDate = ClsSystem.gnvl(dt_pay.Rows[i]["支付日期"], "");
                pay.PayAmount = ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0");
                pay.FeeAmount = ClsSystem.gnvl(dt_pay.Rows[i]["手续费"], "0");
                list_pay.Add(pay);
            }
            order.PayLogList = list_pay;

            #endregion

            //根据促销ID取促销的TYPE
            //if (orderextModel == null)
            //{
            //    strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderModel.vdef4, "-1") + "";
            //}
            //else
            //{
                strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderextModel.ProID, "-1") + "";
            //}
            pro_type = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (pro_type == "2")//促销type为2的是整单促销，0是特价促销，1是商品促销
            {
                order.IsOrderPro = "1";//为整单促销
            }
            else
            {
                order.IsOrderPro = "0";//不是整单促销
            }
            //获取促销明细
            BD_GoodsCategory.ResultOrderPro orderpro = new BD_GoodsCategory.ResultOrderPro();

            //if (orderextModel == null)
            //{
            //    if (ClsSystem.gnvl(orderModel.vdef4, "") != "" && ClsSystem.gnvl(orderModel.vdef4, "") != "0")
            //    {
            //        orderpro.ProID = orderModel.vdef4;//促销ID

            //        strsql = "select protype from BD_Promotion where ID = " + ClsSystem.gnvl(orderModel.vdef4, "-1") + "";
            //        orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            //        orderpro.OrderPrice = ClsSystem.gnvl(orderModel.vdef5, "0.00");//促销金额
            //        string ProIDD = "";
            //        string ProType = "";
            //        string pro_id = "";
            //        //获取ProIDD，ProType为后面拼接促销详情提供参数
            //        decimal pro_num = Common.GetProPrice(orderModel.TotalAmount, out pro_id, out ProIDD, out ProType, orderModel.CompID);
            //        //调用方法拼接促销详情
            //        orderpro.Discount = Common.proOrderType(ProIDD, ClsSystem.gnvl(orderModel.vdef5, "0"), ProType);
            //    }
            //    else
            //    {
            //        orderpro.ProID = "0";
            //        orderpro.ProType = "0";
            //        orderpro.OrderPrice = "";
            //        orderpro.Discount = "";
            //    }
            //}
            //else
            //{
                if (ClsSystem.gnvl(orderextModel.ProID, "") != "" && ClsSystem.gnvl(orderextModel.ProID, "") != "0")
                {
                    orderpro.ProID = orderextModel.ProID.ToString();//促销ID

                    strsql = "select protype from BD_Promotion where ID = " + ClsSystem.gnvl(orderextModel.ProID, "-1") + "";
                    orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    orderpro.OrderPrice = ClsSystem.gnvl(orderextModel.ProAmount, "0.00");//促销金额
                    string ProIDD = orderextModel.ProDID.ToString();
                    //根据促销明细ID，取出促销明细实体
                    Hi.Model.BD_PromotionDetail2 model_prodetail2 = new Hi.BLL.BD_PromotionDetail2().GetModel(orderextModel.ProDID);
                    //拼接protype字符串，以便调用proOrderType获得促销详情
                    string ProType = "";
                    if (orderpro.ProType == "5")//表示满减
                        ProType = "5," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    else if (orderpro.ProType == "6")//表示满折
                        ProType = "6," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    else
                        return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
                    //调用方法拼接促销详情
                    orderpro.Discount = Common.proOrderType(ProIDD, ClsSystem.gnvl(orderextModel.ProAmount, "0"), ProType);
                }
                else
                {
                    orderpro.ProID = "0";
                    orderpro.ProType = "0";
                    orderpro.OrderPrice = "";
                    orderpro.Discount = "";
                }
            //}
            order.OrderPro = orderpro;

            if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))
            {
                Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                if (addr != null)
                {
                    order.Zip = addr.Zip;
                }
            }
            order.Address = orderModel.Address;
            order.Contact = orderModel.Principal;
            order.Phone = orderModel.Phone;

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
            if (dis == null)//|| dis.IsEnabled == 0 || dis.dr == 1
                return new ResultOrderInfo() { Result = "F", Description = "经销商异常" };
            order.DisName = dis.DisName;
            order.Otype = orderModel.Otype.ToString();
            order.OtherAmount = orderModel.OtherAmount.ToString("0.00");
            //兼容版本3以下的版本，之前只有在审核后且付款的才能发货，现在不付款就可以发货，而且存在部分发货，要根据要求，更改返回的ostate与paystate值，让没付款的订单
            //跟部分发货的订单，能在手机上可发货的订单显示
            if (orderModel.OState == 4 && ClsSystem.gnvl(orderModel.IsOutState, "0").ToInt() != 3 && ClsSystem.gnvl(orderModel.IsOutState, "0").ToInt() != 4)
            {
                order.Ostate = "2";
                if (orderModel.PayState == 0)
                {
                    order.PayState = "1";
                }
                else
                {
                    order.PayState = orderModel.PayState.ToString();
                }
            }
            else
            {
                if (orderModel.ReturnState==(int)Enums.ReturnState.申请退货)
                {
                    order.Ostate = "5";
                }
                else if (orderModel.OState == 5)
                {
                    order.Ostate = "3";
                }
                else
                {
                    order.Ostate = orderModel.OState.ToString();
                }
                if (orderModel.OState == 2 && orderModel.PayState == 0)
                {
                    order.PayState = "1";
                }
                else
                {
                    order.PayState = orderModel.PayState.ToString();
                }
            }
            //order.PayState = orderModel.PayState.ToString();
            order.ReturnState = orderModel.ReturnState.ToString();
            //if (!string.IsNullOrEmpty(order.AuditUserID))
            //{
            //    Hi.Model.SYS_Users auditUser = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.AuditUserID));
            //    if (auditUser == null)//|| auditUser.IsEnabled == 0 || auditUser.dr == 1
            //        return new ResultOrderInfo() { Result = "F", Description = "审核人信息异常" };
            //    order.AuditUserName = auditUser.UserName;
            //}
            order.TotalAmount = orderModel.TotalAmount.ToString("0.00");
            order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
            order.PayedAmount = orderModel.PayedAmount.ToString("0.00");
            order.CreateUserID = orderModel.CreateUserID.ToString();
            order.CreateDate = orderModel.CreateDate.ToString("yyyy-MM-dd HH:mm");
            order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
            order.ReturnMoneyUser = orderModel.ReturnMoneyUser;
            order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();

            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (outList.Count != 0)
            {
                Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                foreach (Hi.Model.DIS_OrderOut Out in outList)
                {
                    orderOut = Out;
                }
                order.SendID = orderOut.ID.ToString();
                order.SendDate = orderOut.SendDate.ToString();

                //物流公司 物流单号 物流联系人 联系人电话 发货包数 运费 经办人
                //考虑—— 物流表的详细信息

                //根据发货单取对应的物流信息
                List<Hi.Model.DIS_Logistics> list_log = new Hi.BLL.DIS_Logistics().GetList("", "OrderOutID = " + orderOut.ID + " and isnull(dr,0) = 0", "");
                //一个发货单只有一条物流信息，所以list_log里也只有一条数据
                Hi.Model.DIS_Logistics model_log = list_log[0];
                order.Express = model_log.ComPName;
                order.ExpressNo = model_log.LogisticsNo;
                //order.ExpressPerson = orderOut.ExpressPerson;
                //order.ExpressTel = orderOut.ExpressTel;
                //order.ExpressBao = orderOut.ExpressBao;
                order.PostFee = orderModel.PostFee.ToString("0.00");
                order.ActionUser = orderOut.ActionUser;

                List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                if (exlist != null)
                    order.SendRemark = exlist[0].Context;
                order.SignDate = orderOut.SignDate.ToString();
                order.IsSign = orderOut.IsSign.ToString();
                order.SignUserId = orderOut.SignUserId.ToString();
                order.SignUser = orderOut.SignUser;
                order.SignRemark = orderOut.SignRemark;
            }
            order.IsAudit = orderModel.IsAudit.ToString();
            if (ClsSystem.gnvl(orderModel.AuditUserID, "0") != "0")
            {
                Hi.Model.SYS_Users auditUser = new Hi.BLL.SYS_Users().GetModel(orderModel.AuditUserID);
                if (auditUser == null)//|| auditUser.IsEnabled == 0 || auditUser.dr == 1
                    return new ResultOrderInfo() { Result = "F", Description = "审核人信息异常" };
                order.AuditUserName = auditUser.UserName;
                order.AuditUserID = orderModel.AuditUserID.ToString();
                order.AuditDate = orderModel.AuditDate.ToString();
                order.AuditRemark = orderModel.AuditRemark;
            }
            //todo:不知道的排序
            //order.SortIndex = orderModel.SortIndex.ToString();               
            order.IsDel = orderModel.dr.ToString();

            //明细
            List<OrderDetail> orderDetail = new List<OrderDetail>();
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultOrderInfo() { Result = "F", Description = "订单明细异常" };
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                string SKUName = string.Empty;
                OrderDetail ordetail = new OrderDetail();
                ordetail.SKUID = detail.GoodsinfoID.ToString();
                //通过GoodsInfoID找到GoodsID
                Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                if (goodsInfo == null)//|| goodsInfo.IsEnabled == false || goodsInfo.dr == 1
                    return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                ordetail.ProductID = goodsInfo.GoodsID.ToString();

                //通过GoodsID找到GoodsName
                Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                if (goods == null)
                    //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                    return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                ordetail.ProductName = goods.GoodsName;
                ordetail.Unit = goods.Unit;
                SKUName += goods.GoodsName;

                list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                if (list_attrs != null || list_attrs.Count != 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                    {
                        SKUName += attr.AttrsName;
                    }
                }
                ordetail.SKUName = SKUName;
                //todo:描述是什么 GoodsInfo.ValueInfo?
                ordetail.ValueInfo = goodsInfo.ValueInfo;
                ordetail.SalePrice = detail.GoodsPrice.ToString("0.00");
                ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                ordetail.Num = detail.GoodsNum.ToString("0.00");
                ordetail.Remark = detail.Remark;
                ordetail.IsPro = ClsSystem.gnvl(detail.ProID, "").Trim() == "0" || ClsSystem.gnvl(detail.ProID, "").Trim() == "" ? "0" : "1"; //是否是促销商品

                if (ordetail.IsPro != "0")
                {
                    ordetail.ProNum = ClsSystem.gnvl(ordetail.ProNum, 0);
                    if (ClsSystem.gnvl(detail.ProID, "") != "" && ClsSystem.gnvl(detail.ProID, "").Length > 0)
                    {
                        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.ProID));
                        if (pro != null)
                        {
                            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("",
                                " ProID=" + pro.ID + " and GoodInfoID ='" + ordetail.SKUID + "' and dr=0", "");
                            string info = string.Empty;
                            if (dList != null && dList.Count > 0)
                            {
                                if (pro.Type == 0 && pro.ProType == 1)
                                {
                                    info = "赠品";
                                }
                                else if (pro.Type == 0 && pro.ProType == 2)
                                {
                                    info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                                }
                                else if (pro.Type == 1 && pro.ProType == 3)
                                {
                                    info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                                }
                                else if (pro.Type == 1 && pro.ProType == 4)
                                {
                                    info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                                }
                            }
                            ordetail.proInfo = new DIS_Order.PromotionInfo()
                            {
                                ProID = detail.ProID,
                                ProTitle = pro.ProTitle,
                                ProInfos = info,
                                Type = pro.Type.ToString(),
                                ProTpye = pro.ProType.ToString(),
                                Discount = pro.Discount.ToString("0.00"),
                                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                            };
                        }
                    }
                }

                List<Pic> Pic = new List<Pic>();
                if (goods.Pic != "" && goods.Pic != "X")
                {
                    Pic pic = new Pic();
                    pic.ProductID = goodsInfo.GoodsID.ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 goods.Pic;
                    Pic.Add(pic);
                }
                ordetail.ProductPicUrlList = Pic;

                orderDetail.Add(ordetail);
            }
            order.OrderDetailList = orderDetail;

            return order;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanyOrderDetail:" + JSon);
            return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 订单审核
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAudit SubCompanyApprove(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string CompID = string.Empty;
            StringBuilder sqlInven = null;
            int IsInve = 0;
            //string ts_order = string.Empty;
            //int ists = 0;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["ApproveList"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultAudit() {Result = "F", Description = "参数异常"};
            }
            Hi.Model.SYS_Users userone = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out userone, int.Parse(CompID == "" ? "0" : CompID)))
                return new ResultAudit() { Result = "F", Description = "登录信息异常" };
            //判断核心企业的订单完成节点
            int endnode = Common.rdoOrderAudit("订单完成节点设置", Convert.ToInt32(CompID)).ToInt(0);
            
            #endregion

            List<string> list = new List<string>();
            SqlTransaction TranSaction = null;
            try
            {
                SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
                if (Connection.State.ToString().ToLower() != "open")
                {
                    Connection.Open();
                }
                TranSaction = Connection.BeginTransaction();

                JsonData rList = JInfo["ApproveList"];
                if (rList.Count == 0)
                    return new ResultAudit() {Result = "F", Description = "参数异常"};
                foreach (JsonData item in rList)
                {
                    string receiptNo = item["ReceiptNo"].ToString();
                    list.Add(receiptNo);
                    string approve = item["Approve"].ToString();
                    string approveText = Common.NoHTML(item["ApproveText"].ToString());

                    Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(receiptNo);
                    //ts_order = orderModel.ts.ToString();
                    if (orderModel == null || orderModel.OState != 1 ||
                        orderModel.dr == 1 || CompID != orderModel.CompID.ToString())
                    {
                        TranSaction.Rollback();
                        return new ResultAudit() { Result = "F", Description = "订单信息异常" };
                    }
                    if (orderModel.OState == 1)
                    {
                        orderModel.AuditUserID = int.Parse(UserID);
                        orderModel.AuditDate = DateTime.Now;
                        orderModel.AuditRemark = approveText;
                        orderModel.ts = DateTime.Now;
                        orderModel.modifyuser = int.Parse(UserID);
                        if (endnode != 2)
                        {
                            orderModel.OState = approve == "0" ? 2 : 6;
                        }
                        else
                        {
                            orderModel.OState = (int)Enums.OrderState.已到货;
                        }
                        string sql = approve == "0" ? "订单审核" : "订单作废";
                        //ists = new Hi.BLL.DIS_Order().Getts("DIS_Order", orderModel.ID, ts_order.ToDateTime());
                        //if (ists != 1)
                        //{
                        //    TranSaction.Rollback();
                        //    return new ResultAudit() { Result = "F", Description = "审核失败" };
                        //}
                        bool res = new Hi.BLL.DIS_Order().Update(orderModel, TranSaction);
                        if (res == false)
                        {
                            TranSaction.Rollback();
                            return new ResultAudit() {Result = "F", Description = "审核失败"};
                        }
                        //判断核心企业有没开启库存管理
                        IsInve = Common.rdoOrderAudit("商品是否启用库存",Convert.ToInt32(CompID)).ToInt(0);
 
                        //开启库存管理的审核退回需要退回此订单的商品库存
                        if (approve == "1"&&IsInve ==0)
                        {
                            sqlInven = new StringBuilder();
                            sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory("OrderID = "+orderModel.ID+" and ISNULL(dr,0) =0 ", null));
                            if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), TranSaction.Connection, TranSaction) <= 0)
                            {

                                TranSaction.Rollback();
                                return new ResultAudit() { Result = "F", Description = "审核失败" };
                            }
                        }

                        string str = Common.AddSysBusinessLog(orderModel, userone, "Order",
                            orderModel.ID.ToString(), sql, "", TranSaction);
                        if (str == "0" || str == "false")
                        {
                            TranSaction.Rollback();
                            return new ResultAudit() {Result = "F", Description = "审核失败"};
                        }
                    }
                }

                TranSaction.Commit();
                foreach (JsonData item in rList)
                {
                    Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(item["ReceiptNo"].ToString());
                    if (orderModel != null)
                    {
                        new MsgSend().GetWxService("42", orderModel.ID.ToString(), "0");
                    }
                }
                return new ResultAudit() {Result = "T", Description = "审核成功", ReceiptNoList = list};
            }
            catch
            {
                if (TranSaction != null)
                {
                    if (TranSaction.Connection != null)
                    {
                        TranSaction.Rollback();
                    }
                }
                return new ResultAudit() {Result = "F", Description = "更新异常"};
            }
            finally
            {
                if (TranSaction != null)
                {
                    if (TranSaction.Connection != null)
                    {
                        TranSaction.Connection.Close();
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message +":"+ ex.StackTrace, "SubCompanyApprove:"+JSon);
            return new ResultAudit() {Result = "F", Description = "参数异常"};
        }
    }
    
    /// <summary>
    /// 发货
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAudit SubProduct(string JSon)
    {
        try
        {
            #region JSon取值

            string result = "F";
            string UserID = string.Empty;
            string compID = string.Empty;
            //string ts_order = string.Empty;
            //int ists = 0;
            JsonData ReceiptNoList = new JsonData();

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["ReceiptNoList"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                ReceiptNoList = JInfo["ReceiptNoList"];
            }
            else
            {
                return new ResultAudit() {Result = "F", Description = "参数异常"};
            }
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, int.Parse(compID == "" ? "0" : compID)))
                return new ResultAudit() {Result = "F", Description = "登录信息异常"};

            int endnode = Common.rdoOrderAudit("订单完成节点设置", Convert.ToInt32(compID)).ToInt();

            #endregion

            List<string> list = new List<string>();
            string str = "0";
            //SqlTransaction TranSaction = null;
            try
            {
                //SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
                //Connection.Open();
                //TranSaction = Connection.BeginTransaction();

                foreach (JsonData item in ReceiptNoList)
                {
                    string ReceiptNo = item["ReceiptNo"].ToString();
                    string ComPName = Common.NoHTML(item["ComPName"].ToString());
                    string LogisticsNo = Common.NoHTML(item["LogisticsNo"].ToString());

                    list.Add(ReceiptNo.ToString());
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(ReceiptNo.ToString());
                    //ts_order = orderModel.ts.ToString();
                    //if (orderModel == null || orderModel.CompID.ToString() != compID || orderModel.OState != 2 || orderModel.dr == 1)
                    if (!(orderModel != null && ((orderModel.OState == (int)Enums.OrderState.已发货 && (orderModel.IsOutState == 0 || orderModel.IsOutState == (int)Enums.IsOutState.部分到货 || orderModel.IsOutState == (int)Enums.IsOutState.部分发货)) || orderModel.OState == (int)Enums.OrderState.已审)))
                        return new ResultAudit() {Result = "F", Description = "订单信息异常"};
                    if (endnode != 3)
                    {
                        orderModel.OState = (int)Enums.OrderState.已发货;//订单状态改成已发货
                    }
                    else 
                    {
                        orderModel.OState = (int)Enums.OrderState.已到货;
                    }
                    orderModel.ts = DateTime.Now;
                    orderModel.modifyuser = UserID.ToInt();
                    orderModel.IsOutState = (int)Enums.IsOutState.全部发货;//发货状态改为全部发货
# region//获取需要修改的订单明细实体和需要新增的发货单明细表实体
                    List<Hi.Model.DIS_OrderDetail> list_orderdetail = new Hi.BLL.DIS_OrderDetail().GetList("", "isnull(dr,0)=0 and OrderID=" + orderModel.ID, "");
                    List<Hi.Model.DIS_OrderDetail> list_orderde_update = new List<Hi.Model.DIS_OrderDetail>();
                    List<Hi.Model.DIS_OrderOutDetail> list_orderoutdetail = new List<DIS_OrderOutDetail>();
                    Hi.Model.DIS_OrderOutDetail outdetail = null;
                    int outdetailnum = 0;
                    foreach (Hi.Model.DIS_OrderDetail orderdetail in list_orderdetail)
                    {
                        if (orderdetail.OutNum >= orderdetail.GoodsNum + decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum, "0") == "" ? "0" : ClsSystem.gnvl(orderdetail.ProNum, "0")))
                            continue;
                     #region//获取需要新增的发货单明细实体
                        outdetailnum++;
                        outdetail = new Hi.Model.DIS_OrderOutDetail();
                        outdetail.OrderID = orderModel.ID;
                        outdetail.DisID = orderModel.DisID;
                        outdetail.GoodsinfoID = orderdetail.GoodsinfoID;
                        outdetail.OutNum = orderdetail.GoodsNum + decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum, "0") == "" ? "0" : ClsSystem.gnvl(orderdetail.ProNum, "0")) - ClsSystem.gnvl(orderdetail.OutNum, "0").ToDecimal();//将未发货的全部发完
                        outdetail.Remark = "";
                        outdetail.ts = DateTime.Now;
                        outdetail.modifyuser = UserID.ToInt();
                        list_orderoutdetail.Add(outdetail);
                     #endregion
                        orderdetail.OutNum = orderdetail.GoodsNum;//将货物全部发完
                        orderdetail.IsOut = 1;//将发货状态改为商品已全部发完
                        list_orderde_update.Add(orderdetail);

                    }
                    if(outdetailnum == 0)
                        return new ResultAudit(){Result = "F",Description = "订单已全部发货，不能再次发货"};
#endregion
                    #region//获取需要新增的发货单主表实体
                    Hi.Model.DIS_OrderOut orderout = new Hi.Model.DIS_OrderOut();
                    orderout.CompID = orderModel.CompID;
                    orderout.DisID = orderModel.DisID;
                    orderout.OrderID = orderModel.ID;
                    orderout.ActionUser = user.TrueName;
                    orderout.SendDate = DateTime.Now;
                    orderout.CreateUserID = UserID.ToInt();
                    orderout.CreateDate = DateTime.Now;
                    orderout.ts = DateTime.Now;
                    orderout.dr = 0;
                    orderout.modifyuser = UserID.ToInt();
                    orderout.ReceiptNo = orderModel.ReceiptNo + Common.GetCode("发货单", orderModel.ID.ToString());
                    #endregion

                    #region//新增物流信息
                    Hi.Model.DIS_Logistics log = new Hi.Model.DIS_Logistics();
                    log.OrderID = orderModel.ID;
                    log.ComPName = ComPName;
                    log.LogisticsNo = LogisticsNo;
                    log.CarUser = "";
                    log.CarNo = "";
                    log.Car = "";
                    log.CreateUserID = UserID.ToInt();
                    log.CreateDate = DateTime.Now;
                    log.ts = DateTime.Now;
                    log.modifyuser = UserID.ToInt();


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
                        else
                        {
                            log.Context = "";
                        }
                    }
                    #endregion
                    //ists = new Hi.BLL.DIS_Order().Getts("DIS_Order",orderModel.ID,ts_order.ToDateTime());
                    //if (ists != 1)
                    //{
                       
                    //    return new ResultAudit() { Result = "F", Description = "发货失败" };
                    //}
                   // int outid = new Hi.BLL.DIS_OrderOut().GetOutOrder(orderModel, list_orderde_update, orderout, list_orderoutdetail, log);
                    int outid = 0;
                    //bool res = new Hi.BLL.DIS_Order().Update(orderModel, TranSaction);
                    //if (!res)
                    //{
                    //    TranSaction.Rollback();
                    //    return new ResultAudit() {Result = "F", Description = "订单修改失败"};
                    //}

                    //Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                    //orderOut.ReceiptNo = orderModel.ReceiptNo + Common.GetCode("发货单", orderModel.ID.ToString());
                    //orderOut.CompID = orderModel.CompID;
                    //orderOut.DisID = orderModel.DisID;
                    //orderOut.OrderID = orderModel.ID;
                    //orderOut.SendDate = DateTime.Now;
                    //orderOut.CreateUserID = int.Parse(UserID);
                    //orderOut.CreateDate = DateTime.Now;
                    //orderOut.ts = orderOut.CreateDate;
                    //orderOut.dr = 0;
                    //orderOut.modifyuser = int.Parse(UserID);
                    //orderOut.Express = ComPName;
                    //orderOut.ExpressNo = LogisticsNo;
                    //int count = new Hi.BLL.DIS_OrderOut().Add(orderOut, TranSaction);
                    //if (count == 0)
                    //{
                    //    TranSaction.Rollback();
                    //    return new ResultAudit() {Result = "F", Description = "发货单生产失败"};
                    //}

                    //Hi.Model.DIS_Logistics model = new Hi.Model.DIS_Logistics();
                    //model.OrderID = orderModel.ID;
                    //model.OrderOutID = count;
                    //model.ComPName = ComPName;
                    //model.Type = LogisticsNo.Trim() == "" || LogisticsNo.Trim() == "0" ? 2 : 1;
                    //model.LogisticsNo = LogisticsNo.Trim();
                    //new Hi.BLL.DIS_Logistics().Add(model, TranSaction);
                    if (outid > 0)
                    {

                        str = Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "订单发货", "");
                        //if (str == "0" || str == "false")
                        //{
                            
                        //    return new ResultAudit() { Result = "F", Description = "发货失败" };
                        //}
                    }
                    else
                    {
                        return new ResultAudit() { Result = "F", Description = "发货失败" };
                    }

                    //if (str == "0" || str == "false")
                    //{
                    //    TranSaction.Rollback();
                    //    return new ResultAudit() {Result = "F", Description = "发货失败"};
                    //}
                }

                //TranSaction.Commit();
                result = "T";
                return new ResultAudit() {Result = "T", Description = "发货成功", ReceiptNoList = list};
            }
            catch(Exception ex)
            {
                //if (TranSaction != null)
                //{
                //    if (TranSaction.Connection != null)
                //    {
                //        TranSaction.Rollback();
                //    }
                //}
                return new ResultAudit() {Result = "F", Description = "更新异常"};
            }
            finally
            {
                if (result == "T")
                {
                    foreach (JsonData ReceiptNo in ReceiptNoList)
                    {
                        Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo["ReceiptNo"].ToString());
                        if (orderModel != null)
                        {
                            new MsgSend().GetWxService("43", orderModel.ID.ToString(), "0");
                        }
                    }
                }

                //if (TranSaction != null)
                //{
                //    if (TranSaction.Connection != null)
                //    {
                //        TranSaction.Connection.Close();
                //    }
                //}
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message +":"+ ex.StackTrace, "SubProduct:"+JSon);
            return new ResultAudit() {Result = "F", Description = "参数异常"};
        }
    }

    #endregion

    #region 微信

    /// <summary>
    /// 经销商订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ResultOrderList WXGetResellerOrderList(string JSon)
    {
        try
        {
            string disID = string.Empty;
            string state = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["ResellerID"].ToString() != "")
            {
                disID = JInfo["ResellerID"].ToString();
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "参数异常"};
            }

            return GetWXOrderList(JSon, disID);
        }
        catch
        {
            Common.CatchInfo(JSon, "WXGetResellerOrderList");
            return new ResultOrderList() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 经销商订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ResultOrderInfo WXGetResellerOrderDetail(string JSon)
    {
        try
        {
            string disID = string.Empty;
            string state = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["ResellerID"].ToString() != "")
            {
                disID = JInfo["ResellerID"].ToString();
            }
            else
            {
                return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
            }

            return GetWXOrderDetail(JSon, disID);
        }
        catch
        {
            Common.CatchInfo(JSon, "WXGetResellerOrderDetail");
            return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 企业订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ResultOrderList WXGetCompanyOrderList(string JSon)
    {
        try
        {
            string disID = string.Empty;
            string state = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["CompanyID"].ToString() != "")
            {
                disID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "参数异常"};
            }

            return GetWXOrderList(JSon, disID);
        }
        catch
        {
            Common.CatchInfo(JSon, "WXGetCompanyOrderList");
            return new ResultOrderList() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 企业订单信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ResultOrderInfo WXGetCompanyOrderDetail(string JSon)
    {
        try
        {
            string disID = string.Empty;
            string state = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["CompanyID"].ToString() != "")
            {
                disID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
            }

            return GetWXOrderDetail(JSon, disID);
        }
        catch
        {
            Common.CatchInfo(JSon, "WXGetCompanyOrderDetail");
            return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
        }
    }

    public ResultOrderList GetWXOrderList(string JSon, string BussID)
    {
        return new ResultOrderList()
        {
            Result = "F",
            Description = "接口服务禁用"
        };

        try
        {
            string strWhere = string.Empty;

            #region JSon取值

            string userID = string.Empty;
            string bussID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string state = string.Empty;
            string UserType = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                JInfo["Sort"].ToString() != "" && JInfo["State"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                bussID = BussID;

                // 判断用户角色
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(userID));
                if (user == null)
                    //if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(bussID == "" ? "0" : bussID)))
                    return new ResultOrderList() { Result = "F", Description = "用户异常" };
                else
                {
                    if (user.Type == 1 || user.Type == 5) //经销商
                    {
                        strWhere += " and DisID='" + bussID.Trim() + "' and ISNULL(dr,0)=0";
                        UserType = "0";
                    }
                    else if (user.Type == 3 || user.Type == 4)
                    {
                        strWhere += " and compID='" + bussID.Trim() + "' and ISNULL(dr,0)=0 and OState != 0";
                        UserType = "1";
                    }
                    else
                        return new ResultOrderList() { Result = "F", Description = "用户异常" };
                }
                

                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                state = JInfo["State"].ToString();
                
            }
            else
            {
                return new ResultOrderList() {Result = "F", Description = "参数异常"};
            }
            

            JsonData JMsg = JInfo["Search"];
            if (JMsg.Count > 0)
            {
                if (JMsg["OrderID"].ToString() != "")
                    strWhere += " and ReceiptNo like '" + JMsg["OrderID"].ToString() + "%'";
                if (JMsg["BeginDate"].ToString() != "")
                {
                    strWhere += " and CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
                }
                if (JMsg["EndDate"].ToString() != "")
                {
                    strWhere += " and CreateDate < " + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
                }
                //根据出库单编号 锁定 订单ID
                if (JMsg["ExpressNo"].ToString() != "")
                {
                    //strWhere += " and ReceiptNo like '" + JMsg["ExpressNo"].ToString() + "%'";
                    List<Hi.Model.DIS_OrderOut> orderOut = new Hi.BLL.DIS_OrderOut().GetList("",
                        " ExpressNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                    if (orderOut.Count == 0)
                        return new ResultOrderList() {Result = "F", Description = "参数异常"};
                    strWhere += " and ID in ( 0";
                    strWhere = orderOut.Aggregate(strWhere, (current, aout) => current + ("," + aout.OrderID));
                    strWhere += " )";
                }
            }

            switch (state) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消
            {
                case "0":
                    break;
                case "1":
                    strWhere += " and PayState=" + (int) Enums.PayState.未支付;
                    break;
                case "2":
                    strWhere += " and ((Otype=" + (int) Enums.OType.销售订单 + " and ostate=" + (int) Enums.OrderState.已审 +
                                " and paystate=" + (int) Enums.PayState.已支付
                                + ") or (Otype=" + (int) Enums.OType.赊销订单 + " and ostate= " +
                                (int) Enums.OrderState.已审 + " ))";
                    break;
                case "3":
                    strWhere += " and ostate=" + (int)Enums.OrderState.已发货;
                    break;
                case "4":
                    strWhere += " and ostate=" + (int)Enums.OrderState.已到货 + " and ReturnState in (" +
                                (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
                    break;
                case "9":
                    strWhere += " and ostate=" + (int) Enums.OrderState.已发货;
                    break;
                default:
                    return new ResultOrderList() { Result = "F", Description = "状态异常" };
            }
            strWhere += " and ISNULL(dr,0)=0 and ostate!=0 and Otype!=9";

            #endregion

            #region 模拟分页

            string tabName = " [dbo].[DIS_Order]"; //表名
            string strsql = string.Empty; //搜索sql

            if (sortType == "1") //价格排序
            {
                sortType += "CreateDate";
            }
            else if (sortType == "2") //价格排序
            {
                sortType += "TotalAmount";
            }
            else
            {
                sortType = "ID";
            }
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResultOrderList() {Result = "F", Description = "基础数据异常"};

            #endregion

            #region 赋值

            string SKUName = string.Empty;
            List<Order> OrderList = new List<Order>();
            DataTable orderList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if (orderList != null)
            {
                foreach (DataRow row in orderList.Rows)
                {
                    Order order = new Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "企业异常"};
                    order.CompName = comp.CompName;

                    if (UserType == "0") //经销商
                        order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                            orderModel.ReturnState);
                    else
                        order.State = Common.GetCompOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                            orderModel.ReturnState);
                        
                    order.State = Common.GetOrderType(order.State);
                    string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
                    Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                        orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
                    order.IsEnSend = IsEnSend;
                    order.IsEnPay = IsEnPay;
                    order.IsEnAudit = IsEnAudit;
                    order.IsEnReceive = IsEnReceive;
                    order.IsEnReturn = IsEnReturn;   

                    order.AddType = orderModel.AddType.ToString();
                    order.DisID = orderModel.DisID.ToString();
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
                    if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "经销信息商异常"};
                    order.DisUserID = orderModel.DisUserID.ToString();
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
                    if (user == null || user.IsEnabled == 0 | user.dr == 1)
                        return new ResultOrderList() {Result = "F", Description = "经销商用户信息异常"};
                    order.DisUserName = user.TrueName;
                    order.DisName = dis.DisName;
                    order.AddrID = orderModel.AddrID.ToString();
                    order.ReceiptNo = orderModel.ReceiptNo;
                    order.ArriveDate = orderModel.ArriveDate.ToString();
                    if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))
                    {
                        Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                        if (addr != null)
                        {
                            order.Zip = addr.Zip;
                        }
                    }
                    order.Address = orderModel.Address;
                    order.Contact = orderModel.Principal;
                    order.Phone = orderModel.Phone;
                    order.OState = Common.GetOState(orderModel.OState.ToString());
                    order.Otype = orderModel.Otype.ToString();
                    order.PayState = orderModel.PayState.ToString();
                    order.TotalAmount = orderModel.TotalAmount.ToString("0.00");
                    order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
                    order.PayedAmount = orderModel.PayedAmount.ToString("0.00");
                    order.CreateUserID = orderModel.CreateUserID.ToString();
                    order.CreateDate = orderModel.CreateDate.ToString();
                    order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
                    order.ReturnMoneyUser = orderModel.ReturnMoneyUser.ToString();
                    order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();

                    List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                        " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                        orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (outList.Count != 0)
                    {
                        Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                        foreach (Hi.Model.DIS_OrderOut Out in outList)
                        {
                            orderOut = Out;
                        }
                        order.SendID = orderOut.ID.ToString();
                        order.SendDate = orderOut.SendDate.ToString();
                        //order.Express = orderOut.Express.ToString();
                        //order.ExpressNo = orderOut.ExpressNo.ToString();
                        //order.ExpressPerson = orderOut.ExpressPerson.ToString();
                        //order.ExpressTel = orderOut.ExpressTel.ToString();
                        //order.ExpressBao = orderOut.ExpressBao.ToString();
                        //order.PostFee = orderOut.PostFee.ToString("0.00");
                        order.ActionUser = orderOut.ActionUser.ToString();
                        List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                        if (exlist != null)
                            order.SendRemark = exlist[0].Context.ToString();
                        order.IsAudit = orderOut.IsAudit.ToString();
                        order.AuditUserID = orderOut.AuditUserID.ToString();
                        order.AuditDate = orderOut.AuditDate.ToString();
                        order.AuditRemark = orderOut.AuditRemark == null ? "" : orderOut.AuditRemark.ToString();
                        order.SignDate = orderOut.SignDate.ToString();
                        order.IsSign = orderOut.IsSign.ToString();
                        order.SignUserId = orderOut.SignUserId.ToString();
                        order.SignUser = orderOut.SignUser.ToString();
                        order.SignRemark = orderOut.SignRemark.ToString();
                    }
                    order.SendRemark = orderModel.Remark.ToString();
                    //todo:不知道的排序
                    //order.SortIndex = orderModel.SortIndex.ToString();               
                    order.IsDel = orderModel.dr.ToString();

                    //明细
                    List<OrderDetail> orderDetail = new List<OrderDetail>();
                    List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                        " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    //if (detailList == null)
                    //    return new ResultOrderList() { Result = "F", Description = "订单明细异常" };
                    List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        OrderDetail ordetail = new OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                        if (goodsInfo == null)
                            return new ResultOrderList() {Result = "F", Description = "SKU信息异常"};
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null)
                        //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                            return new ResultOrderList() {Result = "F", Description = "商品异常"};
                        ordetail.ProductName = goods.GoodsName;
                        SKUName += goods.GoodsName;

                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                            {
                                SKUName += attr.AttrsName;
                            }
                        }
                        ordetail.SKUName = SKUName;
                        ordetail.ValueInfo = goodsInfo.ValueInfo;
                        ordetail.SalePrice = detail.Price.ToString("0.00");
                        ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                        ordetail.Num = detail.GoodsNum.ToString("0.00");
                        ordetail.Remark = detail.Remark;
                        ordetail.IsPro = detail.vdef1 == "1" ? "1" : "0"; //是否是促销商品

                        List<Pic> Pic = new List<Pic>();
                        
                        Pic pic = new Pic();
                        pic.ProductID = goodsInfo.GoodsID.ToString();
                        pic.IsDeafult = "1";
                        if (goods.Pic.ToString() != "" && goods.Pic.ToString() != "X")
                        {
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" + goods.Pic;
                        }
                        else
                        {
                            pic.PicUrl = "img/4848.jpg";
                        }
                        Pic.Add(pic);
                        
                        ordetail.ProductPicUrlList = Pic;

                        orderDetail.Add(ordetail);
                    }
                    order.OrderDetailList = orderDetail;
                    OrderList.Add(order);
                }
            }

            #endregion

            return new ResultOrderList()
            {
                Result = "T",
                Description = "获取成功",
                OrderList = OrderList
            };
        }
        catch
        {
            Common.CatchInfo(JSon, "GetWXOrderList");
            return new ResultOrderList() {Result = "F", Description = "参数异常"};
        }
    }

    public ResultOrderInfo GetWXOrderDetail(string JSon, string bussID)
    {
        return new ResultOrderInfo() { Result = "F", Description = "接口禁用" };

        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string BussID = string.Empty;
            string ReceiptNo = string.Empty;
            string CreateDate = string.Empty;
            string Phone = string.Empty;
            string strWhere = string.Empty;
            string UserType = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ReceiptNo"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                BussID = bussID;
                ReceiptNo = JInfo["ReceiptNo"].ToString();
                CreateDate = JInfo["CreateDate"].ToString();
                Phone = JInfo["Phone"].ToString();
            }

            #endregion

            // 判断用户角色
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(UserID));
            if (user == null)
                return new ResultOrderInfo() {Result = "F", Description = "用户异常"};
            switch (user.Type)
            {
                case 1:
                case 5:
                    strWhere += " and DisID='" + bussID.Trim() + "' and ISNULL(dr,0)=0";
                    UserType = "0";
                    break;
                case 3:
                case 4:
                    strWhere += " and compID='" + bussID.Trim() + "' and ISNULL(dr,0)=0 and OState != 0";
                    UserType = "1";
                    break;
                default:
                    return new ResultOrderInfo() {Result = "F", Description = "用户异常"};
            }
            string SKUName = string.Empty;

            ResultOrderInfo order = new ResultOrderInfo();
            Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (orderModel == null)
                return new ResultOrderInfo() {Result = "F", Description = "订单信息异常"};
            if (UserType == "0")
            {
                if (orderModel.DisID.ToString() != BussID)
                    return new ResultOrderInfo() {Result = "F", Description = "订单信息异常"};
            }
            else
            {
                if (orderModel.CompID.ToString() != BussID)
                    return new ResultOrderInfo() {Result = "F", Description = "订单信息异常"};
            }

            order.Result = "T";
            order.Description = "获取成功";
            order.OrderID = orderModel.ID.ToString();
            order.CompID = orderModel.CompID.ToString();
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
            if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                return new ResultOrderInfo() {Result = "F", Description = "企业异常"};
            order.CompName = comp.CompName;
            if (UserType == "0") //经销商
                order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                    orderModel.ReturnState);
            else
                order.State = Common.GetCompOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                    orderModel.ReturnState);
            order.State = Common.GetOrderType(order.State);
            string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
            Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
                orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
            order.IsEnSend = IsEnSend;
            order.IsEnPay = IsEnPay;
            order.IsEnAudit = IsEnAudit;
            order.IsEnReceive = IsEnReceive;
            order.IsEnReturn = IsEnReturn;   

            order.DisID = orderModel.DisID.ToString();
            order.DisUserID = orderModel.DisUserID.ToString();
            Hi.Model.SYS_Users disUser = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
            if (disUser == null || disUser.dr == 1 || disUser.IsEnabled == 0)
                return new ResultOrderInfo() {Result = "F", Description = "经销商用户信息异常"};
            order.DisUserName = disUser.TrueName;
            order.AddrID = orderModel.AddrID.ToString();
            order.ReceiptNo = orderModel.ReceiptNo;
            order.ArriveDate = orderModel.ArriveDate.ToString();

            if (!string.IsNullOrEmpty(orderModel.AddrID.ToString()))//收货地址id存在
            {
                Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                if (addr != null && addr.dr != 1)//收货地址存在且未被删除
                {
                    order.Address = addr.Address;
                    order.Zip = addr.Zip;
                    order.Contact = addr.Principal;
                    order.Phone = addr.Phone;
                }
            }

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
            if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                return new ResultOrderInfo() {Result = "F", Description = "经销商异常"};
            order.DisName = dis.DisName.ToString();
            order.Otype = orderModel.Otype.ToString();
            order.OtherAmount = orderModel.OtherAmount.ToString("0.00");
            order.Ostate = Common.GetOState(orderModel.OState.ToString());
            order.PayState = orderModel.PayState.ToString();
            order.ReturnState = orderModel.ReturnState.ToString();
            if (!string.IsNullOrEmpty(order.AuditUserID))
            {
                Hi.Model.SYS_Users auditUser = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.AuditUserID));
                if (auditUser == null || auditUser.IsEnabled == 0 || auditUser.dr == 1)
                    return new ResultOrderInfo() {Result = "F", Description = "审核人信息异常"};
                order.AuditUserName = auditUser.UserName.ToString();
            }
            order.TotalAmount = orderModel.TotalAmount.ToString("0.00");
            order.AuditTotalAmount = orderModel.AuditAmount.ToString("0.00");
            order.PayedAmount = orderModel.PayedAmount.ToString("0.00");
            order.CreateUserID = orderModel.CreateUserID.ToString();
            order.CreateDate = orderModel.CreateDate.ToString();
            order.ReturnMoneyDate = orderModel.ReturnMoneyDate.ToString();
            order.ReturnMoneyUser = orderModel.ReturnMoneyUser.ToString();
            order.ReturnMoneyUserId = orderModel.ReturnMoneyUserId.ToString();

            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                " OrderID='" + orderModel.ID + "' and CompID='" + orderModel.CompID + "' and DisID='" +
                orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (outList.Count != 0)
            {
                Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                foreach (Hi.Model.DIS_OrderOut Out in outList)
                {
                    orderOut = Out;
                }
                order.SendID = orderOut.ID.ToString();
                order.SendDate = orderOut.SendDate.ToString();
                //order.Express = orderOut.Express.ToString();
                //order.ExpressNo = orderOut.ExpressNo.ToString();
                //order.ExpressPerson = orderOut.ExpressPerson.ToString();
                //order.ExpressTel = orderOut.ExpressTel.ToString();
                //order.ExpressBao = orderOut.ExpressBao.ToString();
                //order.PostFee = orderOut.PostFee.ToString();
                order.ActionUser = orderOut.ActionUser.ToString();
                List<Hi.Model.DIS_Logistics> exlist = Common.GetExpress(orderOut.ID.ToString());
                if (exlist != null)
                    order.SendRemark = exlist[0].Context.ToString();
                order.IsAudit = orderOut.IsAudit.ToString();
                order.AuditUserID = orderOut.AuditUserID.ToString();
                order.AuditDate = orderOut.AuditDate.ToString();
                order.AuditRemark = orderOut.AuditRemark == null ? "" : orderOut.AuditRemark.ToString();
                order.SignDate = orderOut.SignDate.ToString();
                order.IsSign = orderOut.IsSign.ToString();
                order.SignUserId = orderOut.SignUserId.ToString();
                order.SignUser = orderOut.SignUser.ToString();
                order.SignRemark = orderOut.SignRemark.ToString();
            }
            order.SendRemark = orderModel.Remark.ToString();
            //todo:不知道的排序
            //order.SortIndex = orderModel.SortIndex.ToString();               
            order.IsDel = orderModel.dr.ToString();

            //明细
            List<OrderDetail> orderDetail = new List<OrderDetail>();
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultOrderInfo() {Result = "F", Description = "订单明细异常"};
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                OrderDetail ordetail = new OrderDetail();
                ordetail.SKUID = detail.GoodsinfoID.ToString();
                //通过GoodsInfoID找到GoodsID
                Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                if (goodsInfo == null)
                //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                    return new ResultOrderInfo() {Result = "F", Description = "SKU信息异常"};
                ordetail.ProductID = goodsInfo.GoodsID.ToString();

                //通过GoodsID找到GoodsName
                Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                if (goods == null)
                    return new ResultOrderInfo() {Result = "F", Description = "商品异常"};
                ordetail.ProductName = goods.GoodsName;
                ordetail.Unit = goods.Unit;
                SKUName += goods.GoodsName;

                list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                if (list_attrs != null && list_attrs.Count != 0)
                {
                    foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                    {
                        SKUName += attr.AttrsName;
                    }
                }
                ordetail.SKUName = SKUName;
                ordetail.ValueInfo = goodsInfo.ValueInfo;
                ordetail.SalePrice = detail.Price.ToString("0.00");
                ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                ordetail.Num = detail.GoodsNum.ToString("0.00");
                ordetail.Remark = detail.Remark;
                ordetail.IsPro = detail.vdef1 == "1" ? "1" : "0"; //是否是促销商品

                List<Pic> Pic = new List<Pic>();
                Pic pic = new Pic();
                pic.ProductID = goodsInfo.GoodsID.ToString();
                pic.IsDeafult = "1";
                if (goods.Pic.ToString() != "" && goods.Pic.ToString() != "X")
                {
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" + goods.Pic;
                }
                else
                {
                    pic.PicUrl = "img/4848.jpg";
                }
                Pic.Add(pic);
                ordetail.ProductPicUrlList = Pic;

                orderDetail.Add(ordetail);
            }
            order.OrderDetailList = orderDetail;

            return order;
        }
        catch
        {
            Common.CatchInfo(JSon, "GetWXOrderDetail");
            return new ResultOrderInfo() {Result = "F", Description = "参数异常"};
        }
    }

    public WXOrderList GetDisOrderList(string JSon)
    {
        try
        {
            string strWhere = string.Empty;
            string userID, disID, pageSize, pageIndex, orderBy, sort;
            Hi.Model.SYS_Users user = null;
            Hi.Model.BD_Distributor dis = null;

            #region JSon取值

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0)
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["DisID"].ToString();
                dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disID));
                if (dis == null)
                    return new WXOrderList() { Result = "F", Description = "经销商信息异常" };
                if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                    return new WXOrderList() { Result = "F", Description = "登录信息异常" };
                pageSize = JInfo["PageSize"].ToString();
                pageIndex = JInfo["PageIndex"].ToString();
                orderBy = JInfo["OrderBy"].ToString();
                sort = JInfo["Sort"].ToString();

                strWhere = " and disID='" + disID + "' and ISNULL(dr,0)=0";
                if (JInfo["State"].ToString() != "0")
                {
                    switch (JInfo["State"].ToString().Trim()) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消
                    {
                        case "1":
                            strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and Otype<>9 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and DisID='" + disID + "' and ReturnState in(0,1) and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除
                            break;
                        case "3":
                            strWhere += " and ostate=" + (int)Enums.OrderState.已发货;
                            break;
                        case "4":
                            strWhere += " and ostate=" + (int)Enums.OrderState.已到货 + " and ReturnState in (" +
                                        (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
                            break;
                        case "5":
                            strWhere += " and (ostate in (" + (int)Enums.OrderState.退货处理 + " ," + (int)Enums.OrderState.已退货 + ")" +
                                        " or paystate in (" + (int)Enums.PayState.申请退款 + " ," + (int)Enums.PayState.已退款 + ")" +
                                        " or ReturnState in (" + (int)Enums.ReturnState.申请退货 + " ," + (int)Enums.ReturnState.退货退款 + "))";
                            break;
                        case "6":
                            strWhere += " and ostate=" + (int)Enums.OrderState.已审 + " and ReturnState=" +
                                        (int)Enums.ReturnState.未退货;
                            break;
                        case "7":
                            strWhere += " and ostate=" + (int)Enums.OrderState.待审核;
                            break;
                    }
                }
                JsonData JMsg = JInfo["Search"];
                if (JMsg.Count > 0)
                {
                    if (JMsg["BeginDate"].ToString() != "-1")
                    {
                        strWhere += " and CreateDate >= '" + Convert.ToDateTime(JMsg["BeginDate"].ToString()) + "'";
                    }
                    if (JMsg["EndDate"].ToString() != "-1")
                    {
                        strWhere += " and CreateDate < '" + Convert.ToDateTime(JMsg["EndDate"].ToString()).AddDays(1) + "'";
                    }
                    //根据出库单编号 锁定 订单ID
                    if (JMsg["ExpressNo"].ToString() != "-1")
                    {
                        //订单编号
                        strWhere += " and ( ReceiptNo like '%" + JMsg["ExpressNo"].ToString() + "%'";
                        //商品名称
                        string goodsName = Common.GetOrderByGoodsName(JMsg["ExpressNo"].ToString().Trim(), disID,dis.CompID.ToString());
                        if (goodsName != "-1")
                        {
                            strWhere += " or ID in (" + goodsName + ") ";
                        }
                        //物流编号
                        List<Hi.Model.DIS_OrderOut> orderOut = new Hi.BLL.DIS_OrderOut().GetList("",
                            " ExpressNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                        if (orderOut.Count != 0)
                        {
                            strWhere += " or ID in ( -1";
                            strWhere += orderOut.Aggregate(strWhere, (current, aout) => current + ("," + aout.OrderID)) +")";
                        }
                        strWhere += " )";
                    }
                }
            }
            else
            {
                return new WXOrderList() { Result = "F", Description = "参数异常" };
            }

            #endregion

            int pageCount = 0, Counts = 0;
            List<Hi.Model.DIS_Order> orderList = new Hi.BLL.DIS_Order().GetList(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex),
                orderBy, sort.Trim() == "0", strWhere, out pageCount, out Counts);
            if (orderList == null || orderList.Count == 0)
                return new WXOrderList() { Result = "T", Description = "没有订单数据" };
            WXOrderList result = new WXOrderList()
            {
                Result = "T",
                Description = "",
                OrderList = orderList
            };
            return result;
        }
        catch
        {
            Common.CatchInfo(JSon, "GetDisOrderList");
            return new WXOrderList() { Result = "F", Description = "参数异常" };
        }
    }

    #endregion

    #region 返回实体

    public class ResultOrderList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<Order> OrderList { get; set; }
    }

    public class ResultGoodsSaleList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<GoodsSaleReport> GoodsSaleList { get; set; }
    }

    public class GoodsSaleReport
    {
        public String GoodsID { get; set; }
        public String GoodsInfoID { get; set; }
        public String GoodsName { get; set; }
        public String Barcode { get; set; }
        public String Description { get; set; }
        // 销售额
        public String TotalAmount { get; set; }
        // 销售数量
        public String GoodsCount { get; set; }
        public String OrderCount { get; set; }
        public String MinPrice { get; set; }
        public String MaxPrice { get; set; }
        public String GoodsPicUrl { get; set; }
    }

    public class Order
    {
        public int OrderCount { get; set; }
        public String OrderID { get; set; }
        public String CompID { get; set; }
        public String CompName { get; set; }
        public String State { get; set; }
        //订单状态（0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消）
        public String OState { get; set; }
        public String AddType { get; set; }
        public String PayState { get; set; }
        public string Otype { get; set; }
        public String ReturnState { get; set; }
        public String DisID { get; set; }
        public String DisName { get; set; }
        public String DisUserID { get; set; }
        public String DisUserName { get; set; }
        public String AddrID { get; set; }
        public String ReceiptNo { get; set; }
        public String ArriveDate { get; set; }
        public String Address { get; set; }
        public String Zip { get; set; }
        public String Contact { get; set; }
        public String Phone { get; set; }
        public String TotalAmount { get; set; }
        public String AuditTotalAmount { get; set; }
        public String PayedAmount { get; set; }
        public String CreateUserID { get; set; }
        public String CreateDate { get; set; }
        public String ReturnMoneyDate { get; set; }
        public String ReturnMoneyUserId { get; set; }
        public String ReturnMoneyUser { get; set; }
        public String SendID { get; set; }
        public String SendDate { get; set; }
        public String Express { get; set; }
        public String ExpressNo { get; set; }
        public String ExpressPerson { get; set; }
        public String ExpressTel { get; set; }
        public String ExpressBao { get; set; }
        public String PostFee { get; set; }
        public String ActionUser { get; set; }
        public String OrderRemark { get; set; }//订单校验时：-1 该商品异常（不存在，下架等）
        public String SendRemark { get; set; }
        public String IsAudit { get; set; }
        public String AuditUserID { get; set; }
        public String AuditDate { get; set; }
        public String AuditRemark { get; set; }
        public String SignDate { get; set; }
        public String IsSign { get; set; }
        public String SignUserId { get; set; }
        public String SignUser { get; set; }
        public String SignRemark { get; set; }
        public String SortIndex { get; set; }
        public String IsDel { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }

        public String IsEnSend          { get; set; }
        public String IsEnPay     { get; set; }
        public String IsEnAudit  { get; set; }
        public String IsEnReceive { get; set; }
        public String IsEnReturn   { get; set; }
        public BD_GoodsCategory.ResultOrderPro OrderPro { get; set; }//促销信息
        public List<Operating> LogList { get; set; }//操作日志
        public String Bill { get; set; }//发票号
        public String BillState { get; set; }//发票状态
        public List<Pay> PayLogList { get; set; }//支付日志
        public String Rebate { get; set; }//使用的返利金额
        public String IsOrderPro { get; set; }//是否整单优惠0不是1是
        public String RebateAmount { get; set; }//可用的返利金额
    }

    public class OrderDetail
    {
        public String ProductID { get; set; }
        public String SKUID { get; set; }
        public String ProductName { get; set; }
        public String SKUName { get; set; }
        public String ValueInfo { get; set; }
        public String SalePrice { get; set; }
        public String TinkerPrice { get; set; }
        public String IsPro { get; set; }
        public PromotionInfo proInfo { get; set; }
        public String Num { get; set; }
        public String ProNum { get; set; }
        public String Remark { get; set; }  //订单校验时：-1 该商品异常（不存在，下架等）
        public String Unit { get; set; }
        public List<Pic> ProductPicUrlList { get; set; }

        public String SumAmount { get; set; }//验证专用
        public String Price { get; set; }//验证专用
        public String NumEnable { get; set; }//可用库存
    }

    public class PromotionInfo
    {
        public string ProID { get; set; }
        public string ProTitle { get; set; }
        public string ProInfos { get; set; }

        public string Type { get; set; }//促销类型   0、特价促销 1、商品促销
        public string ProTpye { get; set; }//促销方式   特价促销（1、赠品  2、优惠 ）  商品促销（3、满送  4、打折）
        public string Discount { get; set; }//打折率   （ProType = 1、2 是0;     3是满件数  4是打折0-100）

        public string ProStartTime { get; set; }//促销开始时间
        public string ProEndTime { get; set; }//促销结束时间
    }

    public class Pic
    {
        public String ProductID { get; set; }
        public String IsDeafult { get; set; }
        public String PicUrl { get; set; }
    }

    public class ResultOrderInfo
    {
        public string DisName { get; set; }
        public string Otype { get; set; }
        public string OtherAmount { get; set; }
        public string Ostate { get; set; }
        public string PayState { get; set; }
        public string ReturnState { get; set; }
        public string AuditUserName { get; set; }
        public String Result { get; set; }
        public String Description { get; set; }
        public String OrderID { get; set; }
        public String CompID { get; set; }
        public String CompName { get; set; }
        public String AddType{ get; set; }
        public String State { get; set; }
        //订单状态（0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消）
        public String DisID { get; set; }
        public String DisUserID { get; set; }
        public String DisUserName { get; set; }
        public String AddrID { get; set; }
        public String ReceiptNo { get; set; }
        public String ArriveDate { get; set; }
        public String Address { get; set; }
        public String Zip { get; set; }
        public String Contact { get; set; }
        public String Phone { get; set; }
        public String TotalAmount { get; set; }
        public String AuditTotalAmount { get; set; }
        public String PayedAmount { get; set; }
        public String CreateUserID { get; set; }
        public String CreateDate { get; set; }
        public String ReturnMoneyDate { get; set; }
        public String ReturnMoneyUserId { get; set; }
        public String ReturnMoneyUser { get; set; }
        public String SendID { get; set; }
        public String SendDate { get; set; }
        public String Express { get; set; }
        public String ExpressNo { get; set; }
        public String ExpressPerson { get; set; }
        public String ExpressTel { get; set; }
        public String ExpressBao { get; set; }
        public String PostFee { get; set; }
        public String ActionUser { get; set; }
        public String OrderRemark { get; set; }
        public String SendRemark { get; set; }
        public String IsAudit { get; set; }
        public String AuditUserID { get; set; }
        public String AuditDate { get; set; }
        public String AuditRemark { get; set; }
        public String SignDate { get; set; }
        public String IsSign { get; set; }
        public String SignUserId { get; set; }
        public String SignUser { get; set; }
        public String SignRemark { get; set; }
        public String IsDel { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }

        public String IsEnSend { get; set; }
        public String IsEnPay { get; set; }
        public String IsEnAudit { get; set; }
        public String IsEnReceive { get; set; }
        public String IsEnReturn { get; set; }
        public BD_GoodsCategory.ResultOrderPro OrderPro { get; set; }//促销信息
        public List<Operating> LogList { get; set; }//操作日志
        public String Bill { get; set; }//发票号
        public String BillState { get; set; }//发票状态
        public List<Pay> PayLogList { get; set; }//支付日志
        public String Rebate { get; set; }//使用的返利金额
        public String IsOrderPro { get; set; }//是否整单优惠0不是1是
    }

    public class Operating
    {
        public string LogType { get; set; }//操作说明
        public string LogTime { get; set; }//操作时间
        public string OperatePerson { get; set; }//操作人
        public string LogRemark { get; set; }//备注
    }

    public class Pay
    {
        public String CompName { get; set; }//核心企业名称
        public String ResellerName { get; set; }//经销商名称
        public String PayLogType { get; set; }//类型
        public String PayAmount { get; set; }//支付金额
        public String PayDate { get; set; }//支付日期
        public String FeeAmount { get; set; }//手续费
    }
    public class ResultOutConfirm
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string ReceiptNo { get; set; }
    }

    public class ResultAudit
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<string> ReceiptNoList { get; set; }
    }

    #endregion

    #region 微信返回

    public class WXOrderList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<Hi.Model.DIS_Order> OrderList { get; set; }
    }

    public class WXOrder
    {
        public string OrderID { get; set; }
        public string ReceiptNo { get; set; }
        public string CompID { get; set; }
        public string DisID { get; set; }

        public string State { get; set; }//订单状态（0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消）
        public string OState { get; set; }
        public string AddType { get; set; }
        public string PayState { get; set; }
        public string Otype { get; set; }
        public string ReturnState { get; set; }


    }

    #endregion
}