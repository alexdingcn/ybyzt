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
using System.Text.RegularExpressions;

/// <summary>
///Dis_Order_Version3 的摘要说明
/// </summary>
public class Dis_Order_Version3
{
	public Dis_Order_Version3()
	{

	}
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
            List<class_ver3.Att> attlist = null;
            DataTable dt_operate = new DataTable();
            DataTable dt_pay = new DataTable();
            int unsendnum = 0;

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
            if (!new Common().IsLegitUser(int.Parse(userID), out one, 0,int.Parse(disID == "" ? "0" : disID)))
                return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };

            #endregion

            string SKUName = string.Empty;
           
            string SKUName_unsendout = string.Empty;

            ResultOrderInfo returnorder = new ResultOrderInfo();
            class_ver3.Order order = new class_ver3.Order();
            class_ver3.OrderExt orderext = new class_ver3.OrderExt();//返回的发票信息
            
            Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (orderModel == null || orderModel.DisID.ToString() != disID || orderModel.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "订单信息异常" };
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(orderModel.ID.ToString());

            List<Hi.Model.DIS_OrderDetail> list_orderoutdetailinfo = null;//发货单明细对应的订单的明细信息
            List<Hi.Model.DIS_OrderDetail> list_unsendoutdetailinfo = null;//未发货商品对应的订单明细的信息
            Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();//实例化Hi.BLL.DIS_OrderDetail对象，由于后面调用此类的方法
            Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            Hi.Model.BD_GoodsInfo goodsInfo_orderoutd = null;
            Hi.Model.BD_Goods goods_orderoutd = null;
            DataTable dt_orderoutd = null;
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
            
            //if(orderextModel == null)
            //return new ResultOrderInfo() { Result = "F", Description = "订单详情异常" };
            returnorder.Result = "T";
            returnorder.Description = "获取成功";
            order.OrderID = orderModel.ID.ToString();
            order.CompID = orderModel.CompID.ToString();
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
            if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "企业异常" };
            order.CompName = comp.CompName;

            //order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
            //    orderModel.ReturnState);
            //string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
            //Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
            //    orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
            //order.IsEnSend = IsEnSend;
            //order.IsEnPay = IsEnPay;
            //order.IsEnAudit = IsEnAudit;
            //order.IsEnReceive = IsEnReceive;
            //order.IsEnReturn = IsEnReturn;
            order.ts = orderModel.ts.ToString();
            order.AddType = orderModel.AddType.ToString();
            order.DisID = orderModel.DisID.ToString();
            order.DisUserID = orderModel.DisUserID.ToString();
            //委托实现异步调用
            Common.Getattdel attdel = new Common.Getattdel(Common.Getattalist);
            IAsyncResult result =  attdel.BeginInvoke(orderModel.Atta,orderModel.ID,null,null);


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
            
            order.AddrID = orderModel.AddrID.ToString();
            order.ReceiptNo = orderModel.ReceiptNo;
            if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00")
                order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
            //获取
            //strsql = "select vdef8 from Dis_Order where ReceiptNo = '" + ReceiptNo + "' and DisID = '" + disID + "' and CompID = '" + orderModel.CompID.ToString() + "'";
            //strsql += " and isnull(dr,0) = 0";
            //order.Rebate = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
            order.OrderRemark = orderModel.Remark;//订单备注
            //if (orderextModel == null)//发票信息在数据库改版之前的数据从order的实体取
            //{
            //    order.Rebate = ClsSystem.gnvl(orderModel.vdef8, "0");//返利
            //    orderext.BillNo = orderModel.BillNo;//发票号
            //    orderext.IsBill = ClsSystem.gnvl(orderModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";//发票状态
            //}
            //else//发票信息在数据库改版之后的数据从orderext实体取
            //{
                order.Rebate = ClsSystem.gnvl(orderModel.bateAmount, "0");//返利
                orderext.BillNo = orderextModel.BillNo;//发票号
                orderext.IsBill = ClsSystem.gnvl(orderextModel.IsBill, "0");//发票状态
                orderext.OrderID = orderModel.ID.ToString();
                orderext.Rise = orderextModel.Rise;//抬头
                orderext.Content = orderextModel.Content;//发票内容
                orderext.OBank = orderextModel.OBank;//开户银行
                orderext.OAccount = orderextModel.OAccount;//开户账号
                orderext.TRNumber =ClsSystem.gnvl(orderextModel.TRNumber,"");//纳税人登记号
                orderext.IsOBill =ClsSystem.gnvl(orderextModel.IsOBill,"0");//是否开票
            //}
            order.Invoice = orderext;

            //根据促销ID取促销的TYPE
            //if (orderextModel == null)
            //{
            //    strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderModel.vdef4, "-1") + "";
            //}
            //else
            //{
            Hi.Model.BD_Promotion promotion = new Hi.BLL.BD_Promotion().GetModel(orderextModel.ProID);
            if (promotion != null)
                pro_type = ClsSystem.gnvl(promotion.Type,"0");
                //strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderextModel.ProID, "-1") + "";
            //}
            //pro_type = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (pro_type == "2")//促销type为2的是整单促销，0是特价促销，1是商品促销
            {
                order.IsOrderPro = "1";//为整单促销
            }
            else
            {
                order.IsOrderPro = "0";//不是整单促销
            }
            //获取促销明细
            class_ver3.OrderPro orderpro = new class_ver3.OrderPro();
            //if (orderextModel == null)//数据库改版前数据的取法
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
            //else//数据库改版后数据的取法
            //{
                if (ClsSystem.gnvl(orderextModel.ProID, "") != "" && ClsSystem.gnvl(orderextModel.ProID, "") != "0")
                {
                    orderpro.ProID = orderextModel.ProID.ToString();//促销ID

                    //strsql = "select protype from BD_Promotion where ID = " + orderextModel.ProID + "";
                    //orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    if (promotion != null)
                        orderpro.ProType = ClsSystem.gnvl(promotion.ProType, "");
                    else
                        orderpro.ProType = "";
                    orderpro.OrderPrice = ClsSystem.gnvl(orderextModel.ProAmount, "0.00");//促销金额
                    string ProIDD = orderextModel.ProDID.ToString();
                    if (orderpro.ProType != "5" && orderpro.ProType != "6")
                        return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
                    string ProType = orderextModel.Protype;
                    //调用方法拼接促销详情
                    //根据促销明细ID，取出促销明细实体
                    //Hi.Model.BD_PromotionDetail2 model_prodetail2 = new Hi.BLL.BD_PromotionDetail2().GetModel(orderextModel.ProDID);
                    ////拼接protype字符串，以便调用proOrderType获得促销详情
                    //string ProType = "";
                    //if (orderpro.ProType == "5")//表示满减
                    //    ProType = "5," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    //else if (orderpro.ProType == "6")//表示满折
                    //    ProType = "6," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    //else
                    //    return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
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
            order.ProInfo = orderpro;


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
                return new ResultOrderInfo() { Result = "F", Description = "经销商异常" };
            order.DisName = dis.DisName;
            order.Otype = orderModel.Otype.ToString();
            order.OtherAmount = orderModel.OtherAmount.ToString("0.00");
            order.Ostate = orderModel.OState.ToString();
            order.PayState = orderModel.PayState.ToString();
            order.ReturnState = orderModel.ReturnState.ToString();
            //order.ReturnState = orderModel.ReturnState.ToString();
            //存在审核人ID的话，说明订单已经审核，需要取出对应的审核信息
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
            List<class_ver3.Operating> list_operate = new List<class_ver3.Operating>();
            //将此订单的操作日志全取出放入dt中
            strsql = "select LogType,LogTime,OperatePerson,LogRemark from SYS_SysBusinessLog where ApplicationId = " + orderModel.ID + "";
            strsql += " and isnull(dr,0) = 0 and LogClass = 'Order' and CompID = " + orderModel.CompID + "";
            dt_operate = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_operate.Rows.Count; i++)
            {
                class_ver3.Operating operate = new class_ver3.Operating();
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
            List<class_ver3.Pay> list_pay = new List<class_ver3.Pay>();
            //将订单的支付明细取出放在DT里
            strsql = "select comp.CompName as 核心企业名称,pay.DisName as 经销商名称,pay.Source as 类型,pay.Price as 支付金额,pay.Date as 支付日期,pay.sxf as 手续费,pay_payment.guid as 支付流水号,pay.vedf9 支付类型,pay.PreType 线下";
            strsql += " from CompCollection_view pay inner join BD_Company  comp on pay.CompID = comp.ID left join PAY_Payment pay_payment on pay.paymentID = pay_payment.ID";
            strsql += " where pay.OrderID = " + orderModel.ID + "and pay.CompID=" + orderModel.CompID + " order by pay.storID desc";
            dt_pay = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_pay.Rows.Count; i++)
            {
                class_ver3.Pay pay = new class_ver3.Pay();
                pay.CompName = ClsSystem.gnvl(dt_pay.Rows[i]["核心企业名称"], "");
                pay.ResellerName = ClsSystem.gnvl(dt_pay.Rows[i]["经销商名称"], "");
                pay.PayLogType = ClsSystem.gnvl(dt_pay.Rows[i]["类型"], "");
                pay.PayDate = ClsSystem.gnvl(dt_pay.Rows[i]["支付日期"], "");
                pay.PayAmount = ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0");
                pay.FeeAmount = ClsSystem.gnvl(dt_pay.Rows[i]["手续费"], "0");
                pay.Guid = ClsSystem.gnvl(dt_pay.Rows[i]["支付流水号"], "");
                pay.vedf9 = ClsSystem.gnvl(dt_pay.Rows[i]["支付类型"], "");
                pay.PreType = ClsSystem.gnvl(dt_pay.Rows[i]["线下"], "0");
                list_pay.Add(pay);
            }
            order.PayLogList = list_pay;

            
            #endregion

            #region //获取此订单对应的发货单list
            List<class_ver3.OrderOut> list_orderout = new List<class_ver3.OrderOut>();
            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                " OrderID=" + orderModel.ID + " and CompID=" + orderModel.CompID + " and DisID=" +
                orderModel.DisID + " and ISNULL(dr,0)=0", "");
            List<class_ver3.Pic> Pic = null;
            if (outList.Count != 0)
            {
                //Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                List<Hi.Model.DIS_Logistics> exlist = new List<Hi.Model.DIS_Logistics>();//获取的物流信息list
                class_ver3.OrderOut orderOut = null;
                List<class_ver3.SendOutDetail> list_sendoutdetail = null;
                class_ver3.SendOutDetail sendoutdetail = null;
                class_ver3.OrderOutDetailInfo orderoutdetailinfo = null;
                List<Hi.Model.DIS_OrderOutDetail> list_orderoutlist = null;
                class_ver3.Wuliu Logistics = null;
                foreach (Hi.Model.DIS_OrderOut Out in outList)
                {
                    int goodsnum = 0;//发货单明细条数
                    orderOut = new class_ver3.OrderOut();//返回的返货单信息
                    list_sendoutdetail = new List<class_ver3.SendOutDetail>();
                    orderOut.OrderID = orderModel.ID.ToString();
                    orderOut.OrderOutID = Out.ID.ToString();
                    orderOut.SendDate = Out.SendDate.ToString("yyyy-MM-dd");
                    orderOut.ActionUser = Out.ActionUser;
                    orderOut.Remark = Out.Remark;
                    orderOut.ts = Out.ts.ToString();
                    orderOut.OrderOutNo = Out.ReceiptNo;
                    orderOut.ReceiptNo = orderModel.ReceiptNo;
                    orderOut.IsAudit = Out.IsAudit.ToString();
                    orderOut.CreateUserID = Out.CreateUserID.ToString();
                    orderOut.CreateDate = Out.CreateDate.ToString("yyyy-MM-dd");
                    orderOut.IsSign = ClsSystem.gnvl(Out.IsSign, "0");
                    if (ClsSystem.gnvl(orderOut.SignDate, "") != "" && ClsSystem.gnvl(orderOut.SignDate, "") != "0001/1/1 0:00:00")
                        orderOut.SignDate = Out.SignDate.ToString("yyyy-MM-dd");
                    orderOut.SignUserId = ClsSystem.gnvl(Out.SignUserId, "");
                    orderOut.SignUser = ClsSystem.gnvl(Out.SignUser, "");
                    orderOut.SignRemark = ClsSystem.gnvl(Out.SignRemark, "");
                    //取出此发货单对应的发货单明细表list
                   list_orderoutlist = new Hi.BLL.DIS_OrderOutDetail().GetList("",
                        "OrderOutID= " + Out.ID + " and isnull(dr,0) = 0 ", "");
                    //循环发货单明细表list，获取发货单明细
                    foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in list_orderoutlist)
                    {
                        string SKUname_orderoutd = string.Empty;
                        sendoutdetail = new class_ver3.SendOutDetail();//返回的发货单子表信息
                        //class_ver3.SendOutDetail sendoutdetail = new class_ver3.SendOutDetail();
                        sendoutdetail.OrderOutID = Out.ID.ToString();
                        sendoutdetail.SKUID = orderoutdetail.GoodsinfoID.ToString();
                        sendoutdetail.OrderID = orderModel.ID.ToString();
                        sendoutdetail.OutNum = decimal.Parse(ClsSystem.gnvl(orderoutdetail.OutNum, "0")).ToString("0.00");
                        sendoutdetail.SignNum = decimal.Parse(ClsSystem.gnvl(orderoutdetail.SignNum,"0")).ToString("0.00");
                        sendoutdetail.Remark = ClsSystem.gnvl(orderoutdetail.Remark, "");
                        //取出发货单子表对应的订单明细list（list里只有一条数据）
                        list_orderoutdetailinfo = bll_orderdetail.GetList("","OrderID = "+orderModel.ID+" and GoodsinfoID = "+orderoutdetail.GoodsinfoID+" and isnull(dr,0)=0","");
                        if(list_orderoutdetailinfo != null && list_orderoutdetailinfo.Count>0)
                        {
                            orderoutdetailinfo = new class_ver3.OrderOutDetailInfo();
                            orderoutdetailinfo.SKUID = orderoutdetail.GoodsinfoID.ToString();
                            //获取goodsinfo实体
                            goodsInfo_orderoutd = bll_goodsinfo.GetModel(orderoutdetail.GoodsinfoID);
                            if (goodsInfo_orderoutd == null)
                                return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                            orderoutdetailinfo.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            //获取goods实体
                            goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                            if(goods_orderoutd == null)
                                return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                            orderoutdetailinfo.ProductName = goods_orderoutd.GoodsName;

                            SKUname_orderoutd = goods_orderoutd.GoodsName;
                            list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                            if (list_attrs != null && list_attrs.Count != 0)
                            {
                                foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                                {
                                    SKUname_orderoutd += attr.AttrsName;
                                }
                            }

                            orderoutdetailinfo.SKUName = SKUname_orderoutd;
                            orderoutdetailinfo.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                            orderoutdetailinfo.TinkerPrice = list_orderoutdetailinfo[0].AuditAmount.ToString("0.00");
                            orderoutdetailinfo.Unit = goods_orderoutd.Unit;
                            sendoutdetail.AllNum = (list_orderoutdetailinfo[0].GoodsNum + decimal.Parse(ClsSystem.gnvl(list_orderoutdetailinfo[0].ProNum, "0"))).ToString("0.00");
                            //首先判断这条发货明细对应的订单明细中的促销满赠数量是否为0
                            if (decimal.Parse(list_orderoutdetailinfo[0].ProNum) == 0)
                                sendoutdetail.ProInfo = "";
                            else//不为0表示有满赠
                            {
                                string[] a = list_orderoutdetailinfo[0].Protype.Split(',');
                                sendoutdetail.ProInfo = decimal.Parse(a[3]).ToString("0.00") + "赠" + decimal.Parse(a[2]).ToString("0.00");
                            }

                            //取出商品的图片，一个商品有多张图片
                            Pic = new List<class_ver3.Pic>();
                            if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                            {
                                class_ver3.Pic pic = new class_ver3.Pic();
                                pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                                pic.IsDeafult = "1";
                                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                             goods_orderoutd.Pic;
                                Pic.Add(pic);
                            }
                            orderoutdetailinfo.ProductPicUrlList = Pic;
                            orderoutdetailinfo.ProductCode = ClsSystem.gnvl(goodsInfo_orderoutd.BarCode, "");
                            sendoutdetail.OrderOutDetailInfo = orderoutdetailinfo;
                        }

                        list_sendoutdetail.Add(sendoutdetail);
                        goodsnum++;//循环一次发货单明细条数加一

                    }
                    orderOut.SendOutDetailList = list_sendoutdetail;
                    //查出此发货单对应的物流信息
                    Logistics = new class_ver3.Wuliu();//返回的物流信息
                    Logistics.OrderID = orderModel.ID.ToString();
                    Logistics.OrderOutID = Out.ID.ToString();
                    Logistics.GoodsNum = goodsnum.ToString();
                    
                    exlist = Common.GetExpress(Out.ID.ToString());//根据返货单ID取出对应的物流实体的list
                    if (exlist != null)
                    {
                        Logistics.ComPName = ClsSystem.gnvl(exlist[0].ComPName, "");
                        Logistics.LogisticsNo = ClsSystem.gnvl(exlist[0].LogisticsNo, "");
                        Logistics.CarUser = ClsSystem.gnvl(exlist[0].CarUser, "");
                        Logistics.CarNo = ClsSystem.gnvl(exlist[0].CarNo, "");//司机手机号
                        Logistics.Car = ClsSystem.gnvl(exlist[0].Car, "");//车牌号
                        Logistics.Type = exlist[0].Type.ToString();
                        if (exlist[0].Context.IndexOf("context") >= 0 || exlist[0].Context.IndexOf("content") >= 0)
                        {

                            Logistics.Context = exlist[0].Context;
                        }
                    }
                    orderOut.Logistics = Logistics;
                    list_orderout.Add(orderOut);
                }
                order.OrderOutList = list_orderout;
            }
            #endregion

            #region
            //todo:不知道的排序
            //order.SortIndex = orderModel.SortIndex.ToString(); 
            order.PostFee = orderModel.PostFee.ToString("0.00");
            order.IsAudit = orderModel.IsAudit.ToString();  
            order.IsDel = orderModel.dr.ToString();
            order.IsOutState = ClsSystem.gnvl(orderModel.IsOutState,"0");
            order.GiveMode = ClsSystem.gnvl(orderModel.GiveMode, "送货");

            //订单明细
            List<class_ver3.OrderDetail> orderDetail = new List<class_ver3.OrderDetail>();//返回的订单明细list
            List<class_ver3.UnSendoutDetail> unsend_list = new List<class_ver3.UnSendoutDetail>();//返回的未发货明细的list
            Hi.Model.BD_GoodsInfo goodsInfo = new Hi.Model.BD_GoodsInfo();
            DataTable dt = new DataTable();
            Hi.Model.BD_Goods goods = new Hi.Model.BD_Goods();
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                " OrderID=" + orderModel.ID + " and DisID=" + orderModel.DisID + " and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultOrderInfo() { Result = "F", Description = "订单明细异常" };
            class_ver3.OrderDetail ordetail = null;
            class_ver3.UnSendoutDetail unsend = null;
            class_ver3.OrderOutDetailInfo unsendoutdetail = null;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                 ordetail = new class_ver3.OrderDetail();//返回的订单明细实体
                //取出未发货的订单明细list，既订货数量大于发货数量
                if (detail.GoodsNum - ClsSystem.gnvl(detail.OutNum, "0").ToDecimal() > 0)
                {
                    unsend = new class_ver3.UnSendoutDetail();//返回的未发货明细实体
                    unsend.SKUID = detail.GoodsinfoID.ToString();
                    unsend.OrderID = orderModel.ID.ToString();
                    unsend.AllNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl(detail.ProNum,"0"))).ToString("0.00");
                    unsend.OutNum = ClsSystem.gnvl(detail.OutNum, "0").ToDecimal().ToString("0.00");
                    unsend.UnOutNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl(detail.ProNum,"0")) - ClsSystem.gnvl(detail.OutNum, "0").ToDecimal()).ToString("0.00");
                    //取出此条未发货的商品对应的商品明细list（list只有一条数据）
                    list_unsendoutdetailinfo = bll_orderdetail.GetList("", "OrderID = " + orderModel.ID + " and GoodsinfoID = " + detail.GoodsinfoID + " and isnull(dr,0)=0", "");
                    if (list_unsendoutdetailinfo != null && list_unsendoutdetailinfo.Count > 0)
                    {
                        unsendoutdetail = new class_ver3.OrderOutDetailInfo();
                        unsendoutdetail.SKUID = detail.GoodsinfoID.ToString();
                        //获取goodsinfo实体
                        goodsInfo_orderoutd = bll_goodsinfo.GetModel(detail.GoodsinfoID);
                        if (goodsInfo_orderoutd == null)
                            return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                        unsendoutdetail.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                        //获取goods实体
                        goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                        if (goods_orderoutd == null)
                            return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                        unsendoutdetail.ProductName = goods_orderoutd.GoodsName;
                        SKUName_unsendout = goods_orderoutd.GoodsName;
                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs )
                            {
                                SKUName_unsendout += attr.AttrsName;
                            }
                        }
                        unsendoutdetail.SKUName = SKUName_unsendout;
                        unsendoutdetail.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                        unsendoutdetail.TinkerPrice = list_unsendoutdetailinfo[0].AuditAmount.ToString("0.00");
                        unsendoutdetail.Unit = goods_orderoutd.Unit;

                        //取出商品的图片，一个商品有多张图片
                        List<class_ver3.Pic> Pic_unsend = new List<class_ver3.Pic>();
                        if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                        {
                            class_ver3.Pic pic = new class_ver3.Pic();
                            pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            pic.IsDeafult = "1";
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                         goods_orderoutd.Pic;
                            Pic_unsend.Add(pic);
                        }
                        unsendoutdetail.ProductPicUrlList = Pic_unsend;
                        unsendoutdetail.ProductCode = ClsSystem.gnvl(goodsInfo_orderoutd.BarCode, "");
                        unsend.OrderOutDetailInfo = unsendoutdetail;

                    }
                    unsend_list.Add(unsend);
                    unsendnum++;
                }
                ordetail.SKUID = detail.GoodsinfoID.ToString();
                ordetail.OrderDetailID = detail.ID.ToString();
                //通过GoodsInfoID找到GoodsID
                goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                if (goodsInfo == null)
                    //if (goodsInfo == null || goodsInfo.dr == 1 || goodsInfo.IsEnabled == false)
                    return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                ordetail.ProductID = goodsInfo.GoodsID.ToString();

                //通过GoodsID找到GoodsName
                goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                if (goods == null)
                    return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                ordetail.ProductName = goods.GoodsName;
                ordetail.Unit = goods.Unit;
                //拼接sku名称，商品名称加上属性值名称
                SKUName = goods.GoodsName;

                list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
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
                ordetail.BarCode = goodsInfo.BarCode;
                ordetail.SalePrice = detail.GoodsPrice.ToString("0.00");
                ordetail.TinkerPrice = detail.AuditAmount.ToString("0.00");
                ordetail.Num = detail.GoodsNum.ToString("0.00");
                ordetail.Remark = detail.Remark;
                ordetail.IsPro = ClsSystem.gnvl(detail.ProID, "").Trim() == "0" || ClsSystem.gnvl(detail.ProID, "").Trim() == "" ? "0" : "1"; //是否是促销商品
                //如果商品是促销商品，则需要取出促销信息
                if (ordetail.IsPro != "0")
                {
                    ordetail.ProNum = detail.ProNum;
                    if (ClsSystem.gnvl(detail.ProID, "") != "" && ClsSystem.gnvl(detail.ProID, "").Length > 0)
                    {
                        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.ProID));
                        if (pro != null)
                        {
                            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("",
                                " ProID=" + pro.ID + " and GoodInfoID =" + ordetail.SKUID + " and dr=0", "");
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
                            //商品促销信息
                            ordetail.proInfo = new class_ver3.PromotionInfo()
                            {
                                ProID = detail.ProID,
                                ProTitle = pro.ProTitle,
                                ProInfos = info,
                                Type = pro.Type.ToString(),
                                ProType = pro.ProType.ToString(),
                                Discount = pro.Discount.ToString("0.00"),
                                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                            };
                        }
                    }
                }
                //取出商品的图片，一个商品有多张图片
                Pic = new List<class_ver3.Pic>();
                if (goods.Pic != "" && goods.Pic != "X")
                {
                    class_ver3.Pic pic = new class_ver3.Pic();
                    pic.ProductID = goodsInfo.GoodsID.ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 goods.Pic;
                    Pic.Add(pic);
                }
                ordetail.ProductPicUrlList = Pic;

                orderDetail.Add(ordetail);
            }
            //回调（需要传入result不然异步调用返回的结果就是null）
            attlist = attdel.EndInvoke(result);
            order.AttaList = attlist;
            order.OrderDetailList = orderDetail;
            order.UnSendoutDetailList = unsend_list;
            order.UnSendoutNum = unsendnum.ToString();
            #endregion

            returnorder.Order = order;


            return returnorder;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderDetailList：" + JSon);
            return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
        }
    }

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
            //string attname_all = string.Empty;//数据库中存的所有附件的名称
            //string attname_b = string.Empty;//原始附件的名称
            //string[] attname = null;
            //string size = string.Empty;
            //string url = string.Empty;
            //int last = 0;
            //int last_doub = 0;
            DataTable dt_operate = new DataTable();
            DataTable dt_pay = new DataTable();
            string pro_type = string.Empty;
            int unsendnum = 0;



            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["CompanyID"].ToString() != "" && JInfo["ReceiptNo"].ToString() != "")
            // && JInfo["CreateDate"].ToString() != "" && JInfo["Phone"].ToString() != ""
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
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };

            #endregion

            string SKUName = string.Empty;
            string SKUname_orderoutd = string.Empty;
            string SKUName_unsendout = string.Empty;

            ResultOrderInfo returnorder = new ResultOrderInfo();
            class_ver3.Order order = new class_ver3.Order();
            class_ver3.OrderExt orderext = new class_ver3.OrderExt();//返回的发票信息
            //class_ver3.Att atta =null;
            List<class_ver3.Att> attalist = null;
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;

            Hi.Model.DIS_Order orderModel = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (orderModel == null || orderModel.CompID.ToString() != compID || orderModel.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "订单信息异常" };
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(orderModel.ID.ToString());
            //if(orderextModel == null)
            //return new ResultOrderInfo() { Result = "F", Description = "订单详情异常" };
            returnorder.Result = "T";
            returnorder.Description = "获取成功";
            order.OrderID = orderModel.ID.ToString();
            order.CompID = orderModel.CompID.ToString();
            order.ts = orderModel.ts.ToString();

            //异步调用
            Common.Getattdel attdel = new Common.Getattdel(Common.Getattalist);
            IAsyncResult result = attdel.BeginInvoke(orderModel.Atta,orderModel.ID,null,null);
            #region//同步调用


           // //获取数据库中存入的附件名称
           // attname_all = orderModel.Atta;
           ////根据@@去将所有文件名分开
           // attname = Regex.Split(attname_all, "@@",RegexOptions.IgnoreCase);
           // foreach (string attname_c in attname)
           // {
           //     if (attname_c == "")
           //         continue;
           //     atta = new class_ver3.Att();
           //     //取出此文件名最后一次出现^^的索引
           //     last = attname_c.LastIndexOf("^^");
           //     //截除原本的文件名
           //     attname_b = attname_c.Substring(0,last);
           //     //取出文件名最后一次出现.的索引
           //     last_doub = attname_c.LastIndexOf(".");
           //     //原文件的全名就是原文件名加后缀名
           //     attname_b += attname_c.Substring(last_doub,attname_c.Length-last_doub);
           //     atta.AttName = attname_b;
           //     atta.AttUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "OrderFJ/" + attname_c;
           //     atta.OrderId = orderModel.ID.ToString();
           //     //取出文件大小
           //     size = Common.getsize(ConfigurationManager.AppSettings["ImgPath"].ToString().Trim() + "OrderFJ/" + attname_c);
           //     if (size == "0")
           //         return new ResultOrderInfo() { Result = "F", Description = "附件异常" };

           // }
            #endregion
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
            if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                return new ResultOrderInfo() { Result = "F", Description = "企业异常" };
            order.CompName = comp.CompName;


            List<Hi.Model.DIS_OrderDetail> list_orderoutdetailinfo = null;//发货单明细对应的订单的明细信息
            List<Hi.Model.DIS_OrderDetail> list_unsendoutdetailinfo = null;//未发货商品对应的订单明细的信息
            Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();//实例化Hi.BLL.DIS_OrderDetail对象，由于后面调用此类的方法
            Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            Hi.Model.BD_GoodsInfo goodsInfo_orderoutd = null;
            Hi.Model.BD_Goods goods_orderoutd = null;
            DataTable dt_orderoutd = null;

            //order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
            //    orderModel.ReturnState);
            //string IsEnSend, IsEnPay, IsEnReceive, IsEnReturn, IsEnAudit;
            //Common.GetEspecialType(orderModel.OState.ToString(), orderModel.PayState.ToString(), orderModel.Otype.ToString(),
            //    orderModel.ReturnState.ToString(), out IsEnSend, out IsEnPay, out IsEnAudit, out IsEnReceive, out IsEnReturn);
            //order.IsEnSend = IsEnSend;
            //order.IsEnPay = IsEnPay;
            //order.IsEnAudit = IsEnAudit;
            //order.IsEnReceive = IsEnReceive;
            //order.IsEnReturn = IsEnReturn;

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

            order.AddrID = orderModel.AddrID.ToString();
            order.ReceiptNo = orderModel.ReceiptNo;
            if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00")
                order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
            else
                order.ArriveDate = "";
            //获取
            //strsql = "select vdef8 from Dis_Order where ReceiptNo = '" + ReceiptNo + "' and DisID = '" + disID + "' and CompID = '" + orderModel.CompID.ToString() + "'";
            //strsql += " and isnull(dr,0) = 0";
            //order.Rebate = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
            order.OrderRemark = orderModel.Remark;//订单备注
            //if (orderextModel == null)//发票信息在数据库改版之前的数据从order的实体取
            //{
            //    order.Rebate = ClsSystem.gnvl(orderModel.vdef8, "0");//返利
            //    orderext.BillNo = orderModel.BillNo;//发票号
            //    orderext.IsBill = ClsSystem.gnvl(orderModel.IsBill, "0").ToInt() == 0 ? "未完成" : "已完成";//发票状态
            //}
            //else//发票信息在数据库改版之后的数据从orderext实体取
            //{
                order.Rebate = ClsSystem.gnvl(orderModel.bateAmount, "0");//返利
                orderext.BillNo = orderextModel.BillNo;//发票号
                orderext.IsBill = ClsSystem.gnvl(orderextModel.IsBill, "0");//发票状态
                orderext.OrderID = orderModel.ID.ToString();
                orderext.Rise = orderextModel.Rise;//抬头
                orderext.Content = orderextModel.Content;//发票内容
                orderext.OBank = orderextModel.OBank;//开户银行
                orderext.OAccount = orderextModel.OAccount;//开户账号
                orderext.TRNumber = ClsSystem.gnvl(orderextModel.TRNumber, "");//纳税人登记号
                orderext.IsOBill = ClsSystem.gnvl(orderextModel.IsOBill, "0");//是否开票
            //}
            order.Invoice = orderext;

            //根据促销ID取促销的TYPE
            //if (orderextModel == null)
            //{
            //    strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderModel.vdef4, "-1") + "";
            //}
            //else
            //{
            Hi.Model.BD_Promotion promotion = new Hi.BLL.BD_Promotion().GetModel(orderextModel.ProID);
            if (promotion != null)
                pro_type = ClsSystem.gnvl(promotion.Type, "0");
            //    strsql = "select Type from BD_Promotion where ID = " + ClsSystem.gnvl(orderextModel.ProID, "-1") + "";
            ////}
            //pro_type = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (pro_type == "2")//促销type为2的是整单促销，0是特价促销，1是商品促销
            {
                order.IsOrderPro = "1";//为整单促销
            }
            else
            {
                order.IsOrderPro = "0";//不是整单促销
            }
            //获取促销明细
            class_ver3.OrderPro orderpro = new class_ver3.OrderPro();
            //if (orderextModel == null)//数据库改版前数据的取法
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
            //else//数据库改版后数据的取法
            //{
                if (ClsSystem.gnvl(orderextModel.ProID, "") != "" && ClsSystem.gnvl(orderextModel.ProID, "") != "0")
                {
                    orderpro.ProID = orderextModel.ProID.ToString();//促销ID

                    //strsql = "select protype from BD_Promotion where ID = " + orderextModel.ProID + "";
                    //orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    if (promotion != null)
                        orderpro.ProType = ClsSystem.gnvl(promotion.ProType, "");
                    else
                        orderpro.ProType = "";
                    orderpro.OrderPrice = ClsSystem.gnvl(orderextModel.ProAmount, "0.00");//促销金额
                    string ProIDD = orderextModel.ProDID.ToString();
                    //根据促销明细ID，取出促销明细实体
                    //Hi.Model.BD_PromotionDetail2 model_prodetail2 = new Hi.BLL.BD_PromotionDetail2().GetModel(orderextModel.ProDID);
                    //拼接protype字符串，以便调用proOrderType获得促销详情
                    if(orderpro.ProType !="5"&&orderpro.ProType!="6")
                        return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
                    string ProType = orderextModel.Protype;
                    //if (orderpro.ProType == "5")//表示满减
                    ////ProType = "5,"+model_prodetail2.OrderPrice+","+model_prodetail2.Discount+"";
                    //else if (orderpro.ProType == "6")//表示满折
                    //    ProType = "6," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                    //else
                    //    return new ResultOrderInfo() { Result = "F", Description = "订单促销异常" };
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
            order.ProInfo = orderpro;


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
                return new ResultOrderInfo() { Result = "F", Description = "经销商异常" };
            order.DisName = dis.DisName;
            order.Otype = orderModel.Otype.ToString();
            order.OtherAmount = orderModel.OtherAmount.ToString("0.00");
            order.Ostate = orderModel.OState.ToString();
            order.PayState = orderModel.PayState.ToString();
            order.ReturnState = orderModel.ReturnState.ToString();
            //存在审核人ID的话，说明订单已经审核，需要取出对应的审核信息
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
            List<class_ver3.Operating> list_operate = new List<class_ver3.Operating>();
            //将此订单的操作日志全取出放入dt中
            strsql = "select LogType,LogTime,OperatePerson,LogRemark from SYS_SysBusinessLog where ApplicationId = " + orderModel.ID + "";
            strsql += " and isnull(dr,0) = 0 and LogClass = 'Order' and CompID = " + orderModel.CompID + "";
            dt_operate = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_operate.Rows.Count; i++)
            {
                class_ver3.Operating operate = new class_ver3.Operating();
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
            List<class_ver3.Pay> list_pay = new List<class_ver3.Pay>();
            //将订单的支付明细取出放在DT里
            strsql = "select comp.CompName as 核心企业名称,pay.DisName as 经销商名称,pay.Source as 类型,pay.Price as 支付金额,pay.Date as 支付日期,pay.sxf as 手续费,pay_payment.guid as 支付流水号 ";
            strsql += " from CompCollection_view pay inner join BD_Company  comp on pay.CompID = comp.ID left join PAY_Payment pay_payment on pay.paymentID =pay_payment.ID";
            strsql += " where pay.OrderID = " + orderModel.ID + " and isnull(pay.vedf9,0) = 1 order by pay.storID desc";
            dt_pay = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_pay.Rows.Count; i++)
            {
                class_ver3.Pay pay = new class_ver3.Pay();
                pay.CompName = ClsSystem.gnvl(dt_pay.Rows[i]["核心企业名称"], "");
                pay.ResellerName = ClsSystem.gnvl(dt_pay.Rows[i]["经销商名称"], "");
                pay.PayLogType = ClsSystem.gnvl(dt_pay.Rows[i]["类型"], "");
                pay.PayDate = ClsSystem.gnvl(dt_pay.Rows[i]["支付日期"], "");
                pay.PayAmount = ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0");
                pay.FeeAmount = ClsSystem.gnvl(dt_pay.Rows[i]["手续费"], "0");
                pay.Guid = ClsSystem.gnvl(dt_pay.Rows[i]["支付流水号"], "");
                list_pay.Add(pay);
            }
            order.PayLogList = list_pay;


            #endregion

            #region //获取此订单对应的发货单list
            List<Hi.Model.DIS_OrderOut> outList = new Hi.BLL.DIS_OrderOut().GetList("",
                " OrderID=" + orderModel.ID + " and CompID=" + orderModel.CompID + " and DisID=" +
                orderModel.DisID + " and ISNULL(dr,0)=0", "");
            List<class_ver3.Pic> Pic = null;
            if (outList.Count != 0)
            {
                List<class_ver3.OrderOut> list_orderout = new List<class_ver3.OrderOut>();
                //Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
                List<Hi.Model.DIS_Logistics> exlist = new List<Hi.Model.DIS_Logistics>();//获取的物流信息list
                class_ver3.OrderOut orderOut = null;
                List<class_ver3.SendOutDetail> list_sendoutdetail = null;
                List<Hi.Model.DIS_OrderOutDetail> list_orderoutlist = null;
                class_ver3.SendOutDetail sendoutdetail = null;
                class_ver3.OrderOutDetailInfo orderoutdetailinfo = null;
                class_ver3.Wuliu Logistics = null;
                foreach (Hi.Model.DIS_OrderOut Out in outList)
                {
                    int goodsnum = 0;//发货单明细条数
                    orderOut = new class_ver3.OrderOut();//返回的返货单信息
                    list_sendoutdetail = new List<class_ver3.SendOutDetail>();
                    orderOut.OrderID = orderModel.ID.ToString();
                    orderOut.OrderOutID = Out.ID.ToString();
                    orderOut.SendDate = Out.SendDate.ToString("yyyy-MM-dd");
                    orderOut.ActionUser = Out.ActionUser;
                    orderOut.Remark = Out.Remark;
                    orderOut.ts = Out.ts.ToString();
                    orderOut.ReceiptNo = orderModel.ReceiptNo;
                    orderOut.IsAudit = Out.IsAudit.ToString();
                    orderOut.CreateUserID = Out.CreateUserID.ToString();
                    orderOut.CreateDate = Out.CreateDate.ToString("yyyy-MM-dd");
                    orderOut.IsSign = ClsSystem.gnvl(Out.IsSign, "0");
                    orderOut.OrderOutNo = Out.ReceiptNo;
                    if (ClsSystem.gnvl(orderOut.SignDate, "") != "" && ClsSystem.gnvl(orderOut.SignDate, "") != "0001/1/1 0:00:00")
                        orderOut.SignDate = Out.SignDate.ToString("yyyy-MM-dd");
                    orderOut.SignUserId = ClsSystem.gnvl(Out.SignUserId, "");
                    orderOut.SignUser = ClsSystem.gnvl(Out.SignUser, "");
                    orderOut.SignRemark = ClsSystem.gnvl(Out.SignRemark, "");
                    //取出此发货单对应的发货单明细表list
                    list_orderoutlist = new Hi.BLL.DIS_OrderOutDetail().GetList("",
                        "OrderOutID= " + Out.ID + " and isnull(dr,0) = 0 ", "");
                    //循环发货单明细表list，获取发货单明细
                    foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in list_orderoutlist)
                    {
                        sendoutdetail = new class_ver3.SendOutDetail();//返回的发货单子表信息
                        //class_ver3.SendOutDetail sendoutdetail = new class_ver3.SendOutDetail();
                        sendoutdetail.OrderOutID = Out.ID.ToString();
                        sendoutdetail.SKUID = orderoutdetail.GoodsinfoID.ToString();
                        sendoutdetail.OrderID = orderModel.ID.ToString();
                        sendoutdetail.OutNum =decimal.Parse(ClsSystem.gnvl(orderoutdetail.OutNum, "0")).ToString("0.00");
                        sendoutdetail.SignNum =decimal.Parse(ClsSystem.gnvl(orderoutdetail.SignNum, "0")).ToString("0.00");
                        sendoutdetail.Remark = ClsSystem.gnvl(orderoutdetail.Remark, "");
                        //取出发货单子表对应的订单明细list（list里只有一条数据）
                        list_orderoutdetailinfo = bll_orderdetail.GetList("", "OrderID = " + orderModel.ID + " and GoodsinfoID = " + orderoutdetail.GoodsinfoID + " and isnull(dr,0)=0", "");
                        if (list_orderoutdetailinfo != null && list_orderoutdetailinfo.Count > 0)
                        {
                            orderoutdetailinfo = new class_ver3.OrderOutDetailInfo();
                            orderoutdetailinfo.SKUID = orderoutdetail.GoodsinfoID.ToString();
                            //获取goodsinfo实体
                            goodsInfo_orderoutd = bll_goodsinfo.GetModel(orderoutdetail.GoodsinfoID);
                            if (goodsInfo_orderoutd == null)
                                return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                            orderoutdetailinfo.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            //获取goods实体
                            goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                            if (goods_orderoutd == null)
                                return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                            orderoutdetailinfo.ProductName = goods_orderoutd.GoodsName;

                            SKUname_orderoutd = goods_orderoutd.GoodsName;
                            list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                            if (list_attrs != null && list_attrs.Count != 0)
                            {
                                foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                                {
                                    SKUname_orderoutd += attr.AttrsName;
                                }
                            }

                            orderoutdetailinfo.SKUName = SKUname_orderoutd;
                            orderoutdetailinfo.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                            orderoutdetailinfo.ProductCode = goodsInfo_orderoutd.BarCode;
                            orderoutdetailinfo.TinkerPrice = list_orderoutdetailinfo[0].AuditAmount.ToString("0.00");
                            orderoutdetailinfo.Unit = goods_orderoutd.Unit;
                            sendoutdetail.AllNum = (list_orderoutdetailinfo[0].GoodsNum + decimal.Parse(ClsSystem.gnvl(list_orderoutdetailinfo[0].ProNum, "0"))).ToString("0.00");
                            //首先判断这条发货明细对应的订单明细中的促销满赠数量是否为0
                            if (decimal.Parse(list_orderoutdetailinfo[0].ProNum) == 0)
                                sendoutdetail.ProInfo = "";
                            else//不为0表示有满赠
                            {
                                string[] a = list_orderoutdetailinfo[0].Protype.Split(',');
                                sendoutdetail.ProInfo = decimal.Parse(a[3]).ToString("0.00") + "赠" + decimal.Parse(a[2]).ToString("0.00");
                            }
                            //取出商品的图片，一个商品有多张图片
                            Pic = new List<class_ver3.Pic>();
                            if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                            {
                                class_ver3.Pic pic = new class_ver3.Pic();
                                pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                                pic.IsDeafult = "1";
                                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                             goods_orderoutd.Pic;
                                Pic.Add(pic);
                            }
                            orderoutdetailinfo.ProductPicUrlList = Pic;
                            sendoutdetail.OrderOutDetailInfo = orderoutdetailinfo;
                        }
                        list_sendoutdetail.Add(sendoutdetail);
                        goodsnum++;//循环一次发货单明细数加一
                    }
                    orderOut.SendOutDetailList = list_sendoutdetail;
                    //查出此发货单对应的物流信息
                    Logistics = new class_ver3.Wuliu();//返回的物流信息
                    Logistics.OrderID = orderModel.ID.ToString();
                    Logistics.OrderOutID = Out.ID.ToString();
                    Logistics.GoodsNum = goodsnum.ToString();
                    exlist = Common.GetExpress(Out.ID.ToString());//根据返货单ID取出对应的物流实体的list
                    if (exlist != null)
                    {
                        Logistics.ComPName = ClsSystem.gnvl(exlist[0].ComPName, "");
                        Logistics.LogisticsNo = ClsSystem.gnvl(exlist[0].LogisticsNo, "");
                        Logistics.CarUser = ClsSystem.gnvl(exlist[0].CarUser, "");
                        Logistics.CarNo = ClsSystem.gnvl(exlist[0].CarNo, "");//司机手机号
                        Logistics.Car = ClsSystem.gnvl(exlist[0].Car, "");//车牌号
                        Logistics.Type = exlist[0].Type.ToString();
                        if (exlist[0].Context.IndexOf("context") >= 0 || exlist[0].Context.IndexOf("content") >= 0)
                        {

                            Logistics.Context = exlist[0].Context;
                        }
                    }
                    orderOut.Logistics = Logistics;
                    list_orderout.Add(orderOut);
                }
                order.OrderOutList = list_orderout;
            }
            #endregion

            #region
            //todo:不知道的排序
            //order.SortIndex = orderModel.SortIndex.ToString(); 
            order.PostFee = orderModel.PostFee.ToString("0.00");
            order.IsAudit = orderModel.IsAudit.ToString();
            order.IsDel = orderModel.dr.ToString();
            order.IsOutState = ClsSystem.gnvl(orderModel.IsOutState, "0");
            order.GiveMode = ClsSystem.gnvl(orderModel.GiveMode, "送货");

            //订单明细
            List<class_ver3.OrderDetail> orderDetail = new List<class_ver3.OrderDetail>();//返回的订单明细list
            List<class_ver3.UnSendoutDetail> unsend_list = new List<class_ver3.UnSendoutDetail>();//返回的未发货明细的list
            Hi.Model.BD_GoodsInfo goodsInfo = new Hi.Model.BD_GoodsInfo();
            DataTable dt = new DataTable();
            Hi.Model.BD_Goods goods = new Hi.Model.BD_Goods();
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                " OrderID=" + orderModel.ID + " and DisID=" + orderModel.DisID + " and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultOrderInfo() { Result = "F", Description = "订单明细异常" };
            class_ver3.OrderDetail ordetail = null;
            class_ver3.UnSendoutDetail unsend = null;
            class_ver3.OrderOutDetailInfo unsendoutdetail = null;
            List<class_ver3.Pic> Pic_unsend = null;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                ordetail = new class_ver3.OrderDetail();//返回的订单明细实体
                //取出未发货的订单明细list，既订货数量大于发货数量
                if (detail.GoodsNum - ClsSystem.gnvl(detail.OutNum, "0").ToDecimal() > 0)
                {
                    unsend = new class_ver3.UnSendoutDetail();//返回的未发货明细实体
                    unsend.SKUID = detail.GoodsinfoID.ToString();
                    unsend.OrderID = orderModel.ID.ToString();
                    unsend.AllNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl(detail.ProNum, "0"))).ToString("0.00");
                    unsend.OutNum = ClsSystem.gnvl(detail.OutNum, "0").ToDecimal().ToString("0.00");
                    unsend.UnOutNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl( detail.ProNum,"0"))- ClsSystem.gnvl(detail.OutNum, "0").ToDecimal()).ToString("0.00");
                    //取出此条未发货的商品对应的商品明细list（list只有一条数据）
                    list_unsendoutdetailinfo = bll_orderdetail.GetList("", "OrderID = " + orderModel.ID + " and GoodsinfoID = " + detail.GoodsinfoID + " and isnull(dr,0)=0", "");
                    if (list_unsendoutdetailinfo != null && list_unsendoutdetailinfo.Count > 0)
                    {
                        unsendoutdetail = new class_ver3.OrderOutDetailInfo();
                        unsendoutdetail.SKUID = detail.GoodsinfoID.ToString();
                        //获取goodsinfo实体
                        goodsInfo_orderoutd = bll_goodsinfo.GetModel(detail.GoodsinfoID);
                        if (goodsInfo_orderoutd == null)
                            return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                        unsendoutdetail.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                        //获取goods实体
                        goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                        if (goods_orderoutd == null)
                            return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                        unsendoutdetail.ProductName = goods_orderoutd.GoodsName;
                        SKUName_unsendout = goods_orderoutd.GoodsName;
                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                            {
                                SKUName_unsendout += attr.AttrsName;
                            }
                        }
                        unsendoutdetail.SKUName = SKUName_unsendout;
                        unsendoutdetail.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                        unsendoutdetail.TinkerPrice = list_unsendoutdetailinfo[0].AuditAmount.ToString("0.00");
                        unsendoutdetail.Unit = goods_orderoutd.Unit;

                        //取出商品的图片，一个商品有多张图片
                        Pic_unsend = new List<class_ver3.Pic>();
                        if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                        {
                            class_ver3.Pic pic = new class_ver3.Pic();
                            pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            pic.IsDeafult = "1";
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                         goods_orderoutd.Pic;
                            Pic_unsend.Add(pic);
                        }
                        unsendoutdetail.ProductPicUrlList = Pic_unsend;
                        unsendoutdetail.ProductCode = ClsSystem.gnvl(goodsInfo_orderoutd.BarCode, "");
                        unsend.OrderOutDetailInfo = unsendoutdetail;

                    }
                    unsend_list.Add(unsend);
                    unsendnum++;
                }
                ordetail.SKUID = detail.GoodsinfoID.ToString();
                ordetail.OrderDetailID = detail.ID.ToString();
                //通过GoodsInfoID找到GoodsID
                goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                if (goodsInfo == null)
                    //if (goodsInfo == null || goodsInfo.dr == 1 || goodsInfo.IsEnabled == false)
                    return new ResultOrderInfo() { Result = "F", Description = "SKU信息异常" };
                ordetail.ProductID = goodsInfo.GoodsID.ToString();
                ordetail.Inventory = goodsInfo.Inventory.ToString("0.00");
                ordetail.BarCode = goodsInfo.BarCode;

                //通过GoodsID找到GoodsName
                goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                if (goods == null)
                    return new ResultOrderInfo() { Result = "F", Description = "商品异常" };
                ordetail.ProductName = goods.GoodsName;
                ordetail.Unit = goods.Unit;
                //拼接sku名称，商品名称加上属性值名称
                SKUName = goods.GoodsName;

                list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
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
                ordetail.IsPro = ClsSystem.gnvl(detail.ProID, "").Trim() == "0" || ClsSystem.gnvl(detail.ProID, "").Trim() == "" ? "0" : "1"; //是否是促销商品
                //如果商品是促销商品，则需要取出促销信息
                if (ordetail.IsPro != "0")
                {
                    ordetail.ProNum = detail.ProNum;
                    if (ClsSystem.gnvl(detail.ProID, "") != "" && ClsSystem.gnvl(detail.ProID, "").Length > 0)
                    {
                        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.ProID));
                        if (pro != null)
                        {
                            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("",
                                " ProID=" + pro.ID + " and GoodInfoID =" + ordetail.SKUID + " and dr=0", "");
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
                            //商品促销信息
                            ordetail.proInfo = new class_ver3.PromotionInfo()
                            {
                                ProID = detail.ProID,
                                ProTitle = pro.ProTitle,
                                ProInfos = info,
                                Type = pro.Type.ToString(),
                                ProType = pro.ProType.ToString(),
                                Discount = pro.Discount.ToString("0.00"),
                                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                            };
                        }
                    }
                }
                //取出商品的图片，一个商品有多张图片
                Pic = new List<class_ver3.Pic>();
                if (goods.Pic != "" && goods.Pic != "X")
                {
                    class_ver3.Pic pic = new class_ver3.Pic();
                    pic.ProductID = goodsInfo.GoodsID.ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 goods.Pic;
                    Pic.Add(pic);
                }
                ordetail.ProductPicUrlList = Pic;

                orderDetail.Add(ordetail);
            }
            //回调(需要传入result，不然异步调用的返回值就是null)
            attalist = attdel.EndInvoke(result);
            order.AttaList = attalist;
            order.OrderDetailList = orderDetail;
            order.UnSendoutDetailList = unsend_list;
            order.UnSendoutNum = unsendnum.ToString();
            #endregion

            returnorder.Order = order;


            return returnorder;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderDetailList：" + JSon);
            return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
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
            string OrderOutID = string.Empty;
            string strWhere = string.Empty;
            decimal signnum = 0;
            decimal goodsnum = 0;
            int isupdate = 0;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["ReceiptNo"].ToString() != ""&& JInfo["OrderOutID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                ReceiptNo = JInfo["ReceiptNo"].ToString();
                OrderOutID = JInfo["OrderOutID"].ToString();
            }
            else
            {
                return new ResultOutConfirm() { Result = "F", Description = "参数异常" };
            }
            Hi.Model.SYS_Users userone = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out userone, 0,int.Parse(disID == "" ? "0" : disID)))
                return new ResultOutConfirm() { Result = "F", Description = "未找到用户信息" };
            strWhere += " DisID=" + disID + " and isnull(IsSign,0) = 1 and ISNULL(dr,0)=0";


            #endregion
            //判断订单是否存在问题
            Hi.Model.DIS_Order order = new Common().GetOrderByReceiptNo(ReceiptNo);
            if (order == null)
                return new ResultOutConfirm() { Result = "F", Description = "订单编号异常" };
            if (order.dr == 1)
                return new ResultOutConfirm() { Result = "F", Description = "订单已删除" };
            if (order.OState != (int)Enums.OrderState.已发货)
                return new ResultOutConfirm() { Result = "F", Description = "订单非发货状态" };
            if(order.IsOutState == (int)Enums.IsOutState.全部到货)
                return new ResultOutConfirm() { Result = "F", Description = "订单已全部到货" };


            strWhere += " and OrderID=" + order.ID + " and isnull(IsAudit,0) <> 3 ";
            //获取此订单已签收的发货单
            List<Hi.Model.DIS_OrderOut> outList_issign = new Hi.BLL.DIS_OrderOut().GetList("", strWhere, "");


            Hi.Model.DIS_OrderOut orderOut = new Hi.BLL.DIS_OrderOut().GetModel(OrderOutID.ToInt(0));
            //判断发货单是否存在问题
            if(orderOut == null )
                return new ResultOutConfirm() { Result = "F", Description = "发货单异常" };
            if(orderOut.dr == 1)
                return new ResultOutConfirm() { Result = "F", Description = "发货单已删除" };
            if(orderOut.IsSign == 1)
                return new ResultOutConfirm() { Result = "F", Description = "发货单已收货" };
            if(orderOut.IsAudit == 3)
                return new ResultOutConfirm() { Result = "F", Description = "发货单已作废" };
            List<Hi.Model.DIS_OrderOutDetail> outdetaillist_sign = null;//已到货的发货单的明细
            List<Hi.Model.DIS_OrderOutDetail> outdetaillist = null;
            List<Hi.Model.DIS_OrderDetail> orderdetaillist = new Hi.BLL.DIS_OrderDetail().GetList("", "orderid = " + order.ID, "");

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
                    goodsnum += orderdetail.GoodsNum+decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum,"0"));//购买的商品总数量,用于跟发货数量比较，判断是否全部到货
                }
                //获取此订单所有已签收发货单的签收数量
                foreach (Hi.Model.DIS_OrderOut disOrderOut in outList_issign)
                {

                    outdetaillist_sign = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID=" + disOrderOut.ID + " and isnull(dr,0) = 0", "");
                    foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in outdetaillist_sign)
                        {
                            signnum += orderoutdetail.SignNum;//已签收的累加签收数量
                        }
                    

                }

                //对此发货单进行签收

                isupdate = 1;
                orderOut.IsSign = 1;
                orderOut.SignDate = DateTime.Now;
                orderOut.SignUserId = UserID.ToInt();
                orderOut.SignUser = userone.TrueName;
                orderOut.ts = DateTime.Now;
                orderOut.modifyuser = UserID.ToInt();
                //更新发货单
                bool res = new Hi.BLL.DIS_OrderOut().Update(orderOut, TranSaction);
                if (res == false)
                {
                    TranSaction.Rollback();
                    return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
                }
                //本次要签收的发货单，累加发货数量
                outdetaillist = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID=" + orderOut.ID + " and isnull(dr,0) = 0", "");
                foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in outdetaillist)
                {
                    signnum += orderoutdetail.OutNum;
                    orderoutdetail.SignNum = orderoutdetail.OutNum;
                    orderoutdetail.ts = DateTime.Now;
                    int conutd = new Hi.BLL.DIS_OrderOutDetail().Update(orderoutdetail, TranSaction);
                    if (conutd <= 0)
                    {
                        TranSaction.Rollback();
                        return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
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
                bool res_order = new Hi.BLL.DIS_Order().Update(order, TranSaction);
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
                    return new ResultOutConfirm() { Result = "F", Description = "确认收货失败" };
                }

                TranSaction.Commit();

                new MsgSend().GetWxService("3", order.ID.ToString(), "1");

                return new ResultOutConfirm() { Result = "T", Description = "确认收货成功", ReceiptNo = order.ReceiptNo };
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
                return new ResultOutConfirm() { Result = "F", Description = "更新异常" };
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
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "ConfirmReceipt:" + JSon);
            return new ResultOutConfirm() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 发货
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAudit SubProduct(string JSon,string version)
    {
        try
        {
            #region JSon取值

            string result = "F";
            string UserID = string.Empty;
            string compID = string.Empty;
            string CarUser = string.Empty;
            string CarNo = string.Empty;
            string Car = string.Empty;
            JsonData ReceiptNoList = new JsonData();
            JsonData detaillsit = new JsonData();
            string isint = string.Empty;
            int isint_out = 0;
            decimal num_deci = 0;
            int num_int = 0;

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
                return new ResultAudit() { Result = "F", Description = "参数异常" };
            }
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, int.Parse(compID == "" ? "0" : compID)))
                return new ResultAudit() { Result = "F", Description = "未找到用户信息" };

            #endregion

            isint = Common.rdoOrderAudit("订单下单数量是否取整", compID.ToInt());//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
            //判断核心企业的完成节点
            int endnode = Common.rdoOrderAudit("订单完成节点设置",compID.ToInt()).ToInt();
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
                    if (float.Parse(version) >= 6)
                    {
                        CarUser = Common.NoHTML(item["CarUser"].ToString());
                        CarNo = Common.NoHTML(item["CarNo"].ToString());
                        Car = Common.NoHTML(item["Car"].ToString());
                    }
                    detaillsit = item["OrderdetailList"];
                    if(detaillsit.ToString() == "")
                        return new ResultAudit() { Result = "F", Description = "参数异常" };
                    //Hi.Model.BD_GoodsInfo goodsinfo = null;
                    List<Hi.Model.DIS_OrderDetail> list_orderdetail =null;
                    decimal orderout_num = 0;//此次发货商品的总数量
                    decimal orderoutdetail_num = 0;//某个商品发货的数量
                    decimal unorderout_num = 0;//此订单未发货商品的总数量
                    //需要更新的订单明细list
                    List<Hi.Model.DIS_OrderDetail> list_orderde_update = new List<Hi.Model.DIS_OrderDetail>();
                    //需要新增的发货单明细的list
                    List<Hi.Model.DIS_OrderOutDetail> list_orderoutdetail = new List<DIS_OrderOutDetail>();

                    list.Add(ReceiptNo.ToString());
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(ReceiptNo.ToString());
                    //if (orderModel == null || orderModel.CompID.ToString() != compID || orderModel.OState != 2 || orderModel.dr == 1)
                    if (!(orderModel != null && ((orderModel.OState == (int)Enums.OrderState.已发货 && (orderModel.IsOutState == 0 || orderModel.IsOutState == (int)Enums.IsOutState.部分到货 || orderModel.IsOutState == (int)Enums.IsOutState.部分发货)) || orderModel.OState == (int)Enums.OrderState.已审)))
                        return new ResultAudit() { Result = "F", Description = "订单信息异常" };

                    orderModel.OState = (int)Enums.OrderState.已发货;//订单状态改成已发货
                    orderModel.ts = DateTime.Now;
                    orderModel.modifyuser = UserID.ToInt();
                    
                    //orderModel.IsOutState = (int)Enums.IsOutState.全部发货;//发货状态改为全部发货

                    foreach (JsonData OrderDetail in detaillsit)//循环传入的需要发货的订单明细信息
                    {
                        //判断发货数量是否为0，是0就跳出本次循环
                        if (decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0")) == 0)
                            continue;
                        //商品数量需要取整的时候，要判断传入数量是不是整数
                        if (isint == "0")
                        {
                            num_deci = decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0"));
                            num_int = (int)num_deci;
                            if (!int.TryParse(ClsSystem.gnvl(OrderDetail["Num"], "0"), out isint_out))
                            {
                                if (decimal.Parse(num_int.ToString()) != num_deci)
                                {
                                    return new ResultAudit() { Result = "F", Description = "商品数量应为整数" };
                                }
                            }
                        } 
                        # region//获取需要修改的订单明细实体和需要新增的发货单明细表实体
                        list_orderdetail = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID=" + orderModel.ID + " and GoodsinfoID = " + OrderDetail["SKUID"] + " and isnull(dr,0)=0", "");
                        if(list_orderdetail==null || list_orderdetail.Count <=0 )
                            return new ResultAudit() { Result = "F", Description = "订单明细信息异常" };
                        //判断此商品的库存是否足够
                        //goodsinfo = new Hi.BLL.BD_GoodsInfo().GetModel(int.Parse(OrderDetail["SKUID"].ToString()));
                        //if (goodsinfo.Inventory < decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0")))
                        //    return new ResultAudit() { Result = "F", Description = "商品库存不足" };
                      //累计本次发货商品的总数量
                        orderout_num += decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0"));
                        Hi.Model.DIS_OrderOutDetail outdetail = null;
                        foreach (Hi.Model.DIS_OrderDetail orderdetail in list_orderdetail)
                        {
                            
                            //判断此商品的本次发货数量是否大于未发货数量
                            if(orderdetail.GoodsNum - orderdetail.OutNum + decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum,"0"))< decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"],"0")))
                                return new ResultAudit() { Result = "F", Description = "商品的发货数量大于未发货数量" };

                            #region//获取需要新增的发货单明细实体
                            outdetail = new Hi.Model.DIS_OrderOutDetail();
                            outdetail.OrderID = orderModel.ID;
                            outdetail.DisID = orderModel.DisID;
                            outdetail.GoodsinfoID = orderdetail.GoodsinfoID;
                            outdetail.OutNum = decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0"));//发货数量
                            outdetail.Remark = "";
                            outdetail.ts = DateTime.Now;
                            outdetail.modifyuser = UserID.ToInt();
                            list_orderoutdetail.Add(outdetail);
                            #endregion
                            //计算此商品的已发货数量，并判断此商品有没全部发完
                            orderoutdetail_num = orderdetail.OutNum + decimal.Parse(ClsSystem.gnvl(OrderDetail["Num"], "0"));
                            orderdetail.OutNum = orderoutdetail_num;//明细的发货数量要加上本次的发货数量
                            if(orderoutdetail_num == orderdetail.GoodsNum+decimal.Parse(ClsSystem.gnvl(orderdetail.ProNum,"0")))//如果加上本次发货数量后商品已经发完，修改发货状态为全部发货
                            orderdetail.IsOut = 1;//将发货状态改为商品已全部发完
                            list_orderde_update.Add(orderdetail);

                        }
                    }
                    #endregion

                    //判断此订单的所有商品是否发完（此订单的所有商品的未发货数量是否等于本次发货商品的总数量）
                    List<Hi.Model.DIS_OrderDetail> listorderdetail_all = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID = "+orderModel.ID+" and isnull(dr,0) = 0", "");
                    foreach (Hi.Model.DIS_OrderDetail orderdetail_all in listorderdetail_all)
                    {
                        unorderout_num = orderdetail_all.GoodsNum+decimal.Parse(ClsSystem.gnvl( orderdetail_all.ProNum,"0")) - orderdetail_all.OutNum + unorderout_num;//累加未发货商品的总数量
                    }
                    //此次发货总数量等于此订单未发货数量，订单发货状态改为全部发货,否则为部分发货
                    if (orderout_num >= unorderout_num)
                    {
                        orderModel.IsOutState = (int)Enums.IsOutState.全部发货;
                        if (endnode == 3)
                        {
                            orderModel.OState = (int)Enums.OrderState.已到货;
                        }
                    }
                    else if(orderModel.IsOutState != (int)Enums.IsOutState.部分到货)
                    {
                        orderModel.IsOutState = (int)Enums.IsOutState.部分发货;
                    }

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
                    log.CarUser = CarUser;
                    log.CarNo = CarNo;
                    log.Car = Car;
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
                    //int outid = new Hi.BLL.DIS_OrderOut().GetOutOrder(orderModel, list_orderde_update, orderout, list_orderoutdetail, log);
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
                return new ResultAudit() { Result = "T", Description = "发货成功", ReceiptNoList = list };
            }
            catch (Exception ex)
            {
                //if (TranSaction != null)
                //{
                //    if (TranSaction.Connection != null)
                //    {
                //        TranSaction.Rollback();
                //    }
                //}
                return new ResultAudit() { Result = "F", Description = "更新异常" };
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
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "SubProduct:" + JSon);
            return new ResultAudit() { Result = "F", Description = "参数异常" };
        }
    }


    ///<summary>
    ///获取经销商的开票信息
    ///</summary>
    public ResultBillList GetResellerBillList(string JSon)
    {
        try
        {
            #region//JSon取值
            string UserID = string.Empty;
            string disID = string.Empty;
            string strsql = string.Empty;
            DataTable dt = null;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();

            }
            else
            {
                return new ResultBillList() { Result = "F", Description = "参数异常" };
            }
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultBillList() { Result = "F", Description = "登录信息异常" };
            #endregion

            //赋值
            //从数据库中查出此经销商对应所有的开票信息，放入dt中
            strsql = "select Rise,Content,OBank,OAccount,TRNumber from BD_DisAccount where DisID= "+disID+" and isnull(dr,0)=0 ";
            dt = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            //循环dt中数据将数据放入BillInfoList中
            List<class_ver3.BillInfo> BillInfoList = new List<class_ver3.BillInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                class_ver3.BillInfo BillInfo = new class_ver3.BillInfo();
                BillInfo.DisID = disID;
                BillInfo.Rise = ClsSystem.gnvl(dt.Rows[i]["Rise"], "");
                BillInfo.Content = ClsSystem.gnvl(dt.Rows[i]["Content"], "");
                BillInfo.OBank = ClsSystem.gnvl(dt.Rows[i]["OBank"], "");
                BillInfo.OAccount = ClsSystem.gnvl(dt.Rows[i]["OAccount"], "");
                BillInfo.TRNumber = ClsSystem.gnvl(dt.Rows[i]["TRNumber"], "");
                BillInfoList.Add(BillInfo);
            }
            return new ResultBillList() { Result = "T", Description = "获取成功",BillInfoList = BillInfoList};

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerBillList：" + JSon);
            return new ResultBillList() { Result = "F", Description = "参数异常" };
        }
    }



    /// <summary>
    /// 经销商确认收货
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderPrompt GetOrderPrompt(string JSon, string verson)
    {
        string UserID = string.Empty;
        string DisID = string.Empty;
        string CompID = string.Empty;
        string strsql = string.Empty;
        ResultOrderPrompt ResultOrderProm = new ResultOrderPrompt();
        ResultOrderProm.Result = "T";
        ResultOrderProm.Description = "返回成功";
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || (JInfo["ResellerID"].ToString().Trim() == "" && JInfo["CompanyID"].ToString().Trim() == ""))
            {
                return new ResultOrderPrompt() { Result = "F",Description = "参数异常"};
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                DisID = JInfo["ResellerID"].ToString();
                CompID = JInfo["CompanyID"].ToString();
            }
            //传入的是核心企业ID的话，就是查询核心企业的待审核，待退货审核，待发货订单数
            if (DisID == "")
            {
                //判断登录信息是否正确
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                    return new ResultOrderPrompt() { Result = "F", Description = "登录信息异常" };
                //判断核心企业信息是否异常
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
                if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                    return new ResultOrderPrompt() { Result = "F", Description = "核心企业异常" };
                //获取待审核订单（订单状态是待审核的）
                strsql = "select COUNT(*) from DIS_Order where compid ="+comp.ID+" and ISNULL(OState,1) = "+(int)Enums.OrderState.待审核+" ";
                strsql += " and ISNULL(dr,0) = 0 and ISNULL(Otype,0) !=9 ";
                string num_order = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                //获取待退货审核订单数(订单状态是已发货，退货状态是申请退货)
                strsql = "select COUNT(*) from DIS_Order where compid =" + comp.ID + " and ISNULL(OState,1) = "+(int)Enums.OrderState.已到货+"  ";
                strsql += "and ISNULL(ReturnState,0) =" + (int)Enums.ReturnState.申请退货 + " and ISNULL(dr,0) = 0 and ISNULL(Otype,0) !=9";
                string num_orderreturn = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                //获取待发货订单数（订单状态是已审，或订单状态是已发货但不是发货状态是部分发货，部分到货的）
                strsql = "select COUNT(*) from DIS_Order where compid ="+comp.ID+" and (ISNULL(OState,1) ="+(int)Enums.OrderState.已审+" or  ";
                strsql += " (isnull(OState,1) =" + (int)Enums.OrderState.已发货 + " and isnull(IsOutState,0) in (1,2))) and ISNULL(dr,0) = 0 and ISNULL(Otype,0) !=9";
                string num_orderout = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0");
                //获取待审核经销商数
                strsql = "select COUNT(*) from BD_Distributor where CompID = " + comp.ID + " and AuditState =0 and ISNULL(dr,0) = 0 and ISNULL(IsEnabled,0) =1";
                string num_disaudit = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                ResultOrderProm.OrderAudit = num_order;
                ResultOrderProm.OrderReturnAudit = num_orderreturn;
                ResultOrderProm.OrderSend = num_orderout;
                ResultOrderProm.ResellerAudit = num_disaudit;

            }
            //传入的是经销商ID的话，就是查询经销商的待付款订单数跟账单数
            else
            {
                //判断登录信息是否正确
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one,0, int.Parse(DisID == "" ? "0" : DisID)))
                    return new ResultOrderPrompt() { Result = "F", Description = "登录信息异常" };
                //判断经销商信息是否异常
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(DisID));
                if (dis == null || dis.dr == 1 || dis.IsEnabled == 0 || dis.AuditState == 0)
                    return new ResultOrderPrompt() { Result = "F", Description = "经销商信息异常" };
                //经销商代付款订单数（订单状态不是未审核，已作废，已退货的并且付款状态是未支付或部分支付的订单）
                strsql = "select COUNT(*) from DIS_Order where DisID  = "+dis.ID+" and ISNULL(Otype,0) != 9 and ISNULL(dr,0) = 0 ";
                strsql += "and ISNULL(OState,0) not in (1,6,7) and PayState in (0,1)";
                string num_order = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0");
                //经销商待付款账单（订单状态不是未审核，已作废，已退货的并且付款状态是未支付或部分支付的账单）
                strsql = "select COUNT(*) from DIS_Order where DisID  = " + dis.ID + " and ISNULL(Otype,0) = 9 and ISNULL(dr,0) = 0  ";
                strsql += "and ISNULL(OState,0) not in (1,6,7) and PayState in (0,1)";
                string num_bill = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0");
                ResultOrderProm.OrderNum = num_order;
                ResultOrderProm.BillNum = num_bill;
            }

            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetOrderPrompt:" + JSon);
            return new ResultOrderPrompt() { Result = "F", Description = "参数异常" };
        }
        return ResultOrderProm;
    }

    /// <summary>
    /// 获取核心企业简报 
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public ResultCompyBriefing GetCompyBriefing(string JSon,string version)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        int daynum = 0;
        int weeknum = 0;
        int monthnum = 0;
        decimal daymoney = 0;
        decimal weekmoney = 0;
        decimal monthmoney = 0;
        ResultCompyBriefing result = new ResultCompyBriefing();
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
            }
            else
            {
                return new ResultCompyBriefing() { Result = "F", Description = "参数异常" };
            }
            #endregion
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                return new ResultCompyBriefing() { Result = "F", Description = "登录信息异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultCompyBriefing() { Result = "F", Description = "核心企业异常" };
            List<Briefing> list_briefing = new List<Briefing>();
            Briefing briefing_order = new Briefing();

            //获取当前时间
            DateTime date = DateTime.Now;
            //当月第一天
            DateTime day1 = new DateTime(date.Year, date.Month, 1);
            //本月  最后一天  多加一天
            DateTime mothday = day1.AddMonths(1);

            //获取当前时间加一天
            DateTime Sday = date.AddDays(1);
            //当天的0点0分
            DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

            //本周周一  
            DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));
            startWeek = new DateTime(startWeek.Year, startWeek.Month, startWeek.Day, 0, 0, 0);
            //本周周日
            DateTime endWeek = startWeek.AddDays(6);
            #region//订单
            Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
            List<Hi.Model.DIS_Order> orderl = OrderBll.GetList("", " isnull(dr,0)=0 and Otype!=9 and CompID=" + comp.ID, "");
            if (orderl != null || orderl.Count > 0)
            {
                //当日订单数
                //daynum  = orderl.FindAll(order =>
                //                                    ((order.ReturnState == (int)Enums.ReturnState.未退货 || order.ReturnState == (int)Enums.ReturnState.拒绝退货) &&
                //                                     (order.OState == (int)Enums.OrderState.待审核 || order.OState == (int)Enums.OrderState.已到货 || 
                //                                     order.OState == (int)Enums.OrderState.已发货 || order.OState == (int)Enums.OrderState.已审) &&
                //                                     order.CreateDate >= day0)
                //                               ).Count;
                daynum = orderl.FindAll(order => ((order.OState == 2 || order.OState == 3 || order.OState == 4 || order.OState == 5 || order.OState == 7) &&
                                                         order.CreateDate >= day0 && order.CreateDate < Sday)).Count;
                //本月订单数
                //monthnum  = orderl.FindAll(p => (p.ReturnState == (int)Enums.ReturnState.未退货 || p.ReturnState == (int)Enums.ReturnState.拒绝退货) &&
                //    (p.OState == (int)Enums.OrderState.待审核 || p.OState == (int)Enums.OrderState.已到货 || p.OState == (int)Enums.OrderState.已发货 || 
                //    p.OState == (int)Enums.OrderState.已审)
                //    && p.CreateDate >= day1 && p.CreateDate <= Sday).Count;
                monthnum = orderl.FindAll(p => ((p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7)
                        && p.CreateDate >= day1 && p.CreateDate < Sday)).Count;

                //本周订单数
                //weeknum = orderl.FindAll(p => (p.ReturnState == (int)Enums.ReturnState.未退货 || p.ReturnState == (int)Enums.ReturnState.拒绝退货)
                //    && (p.OState == (int)Enums.OrderState.待审核 || p.OState == (int)Enums.OrderState.已到货 || p.OState == (int)Enums.OrderState.已发货 || 
                //    p.OState == (int)Enums.OrderState.已审) && p.CreateDate >= startWeek && p.CreateDate <= endWeek).Count;
                weeknum = orderl.FindAll(p =>
                         (p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7) && p.CreateDate >= startWeek && p.CreateDate < endWeek).Count;

            }
            //当天订单金额
            string wherestr = "CompID=" + comp.ID + " and CreateDate>='" + day0 + "' and CreateDate<='" + Sday + "' and OState in(2,3,4,5,7)";
            string sql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
                "where "+wherestr+"  and isnull(dr,0)=0 and Otype<>9 " ;


            DataTable dayDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dayDt != null)
            {
                if (dayDt.Rows.Count > 0)
                {
                     daymoney = Convert.ToDecimal(ClsSystem.gnvl(dayDt.Rows[0]["AuditAmount"],"0"));

                    
                }
            }
            //本周订单金额
            wherestr = "CompID=" + comp.ID + " and CreateDate>='" + startWeek + "' and CreateDate<='" + endWeek + "' and OState in(2,3,4,5,7)";
            sql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
                "where " + wherestr + " and  isnull(dr,0)=0 and Otype<>9 ";


            DataTable weekDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
           if (weekDt != null)
            {
                if (weekDt.Rows.Count > 0)
                {
                    weekmoney = Convert.ToDecimal(ClsSystem.gnvl(weekDt.Rows[0]["AuditAmount"],"0"));


                }
            }
            //本月订单金额
           wherestr = "CompID=" + comp.ID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "' and OState in(2,3,4,5,7)";
           sql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
               "where " + wherestr + " and  isnull(dr,0)=0 and Otype<>9 ";


           DataTable monthDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
           if (monthDt != null)
           {
               if (monthDt.Rows.Count > 0)
               {
                   monthmoney = Convert.ToDecimal(ClsSystem.gnvl(monthDt.Rows[0]["AuditAmount"],"0"));


               }
           }
            //订单统计数据
           briefing_order.Type = "0";
           briefing_order.DayNumb = daynum.ToString();
           briefing_order.DayMoney = daymoney.ToString("0.00");
           briefing_order.WeekNumb = weeknum.ToString();
           briefing_order.WeekMoney = weekmoney.ToString("0.00");
           briefing_order.MonthNumb = monthnum.ToString();
           briefing_order.MonthMoney = monthmoney.ToString("0.00");
           list_briefing.Add(briefing_order);
            #endregion
            #region//退货单
            Briefing briefing_return = new Briefing();
            briefing_return.Type = "1";
           Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();
           List<Hi.Model.DIS_OrderReturn> rOl = OrderReturnBll.GetList("", " isnull(dr,0)=0 and CompID=" + comp.ID, "");
           if (rOl != null && rOl.Count > 0)
           {
               //当天退货单数
               briefing_return.DayNumb = rOl.FindAll(R => R.CreateDate >= day0 && R.CreateDate <= Sday && (R.ReturnState == 3)).Count.ToString();
               //本周退货单数
               briefing_return.WeekNumb = rOl.FindAll(R => R.CreateDate >= startWeek && R.CreateDate <= endWeek && (R.ReturnState == 3)).Count.ToString();
               //本月退货单数
               briefing_return.MonthNumb = rOl.FindAll(R => R.CreateDate >= day1 && R.CreateDate <= Sday && (R.ReturnState == 3)).Count.ToString();

               //当天退款
               List<Hi.Model.DIS_OrderReturn> rl = rOl.FindAll(R => R.CreateDate >= day0 && R.CreateDate <= Sday && (R.ReturnState == 3));
               daymoney = 0;
               foreach (var item in rl)
               {
                   List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                   if (rol != null && rol.Count > 0)
                   {
                       foreach (var Pitem in rol)
                       {
                           daymoney += Convert.ToDecimal(ClsSystem.gnvl(Pitem.PayedAmount,"0"));
                       }
                   }
               }
               briefing_return.DayMoney = daymoney.ToString("0.00");

               //本周退款
               List<Hi.Model.DIS_OrderReturn> rwl = rOl.FindAll(R => R.CreateDate >= startWeek && R.CreateDate <= endWeek && (R.ReturnState == 3));
               weekmoney = 0;
               foreach (var item in rwl)
               {
                   List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                   if (rol != null && rol.Count > 0)
                   {
                       foreach (var Pitem in rol)
                       {
                           weekmoney += Convert.ToDecimal(ClsSystem.gnvl(Pitem.PayedAmount,"0"));
                       }
                   }
               }
               briefing_return.WeekMoney = weekmoney.ToString("0.00");

               //本月退款
               List<Hi.Model.DIS_OrderReturn> rml = rOl.FindAll(R => R.CreateDate >= day1 && R.CreateDate <= Sday && (R.ReturnState == 3));
               monthmoney = 0;
               foreach (var item in rml)
               {
                   List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                   if (rol != null && rol.Count > 0)
                   {
                       foreach (var Pitem in rol)
                       {
                           monthmoney += Convert.ToDecimal(ClsSystem.gnvl(Pitem.PayedAmount,"0"));
                       }
                   }
               }
               briefing_return.MonthMoney = monthmoney.ToString("0.00");
           }
           else
           {
               briefing_return.DayNumb = "0";
               briefing_return.DayMoney = "0.00";
               briefing_return.WeekMoney = "0.00";
               briefing_return.WeekNumb = "0";
               briefing_return.MonthNumb = "0";
               briefing_return.MonthMoney = "0.00";
           }
           list_briefing.Add(briefing_return);
            #endregion
            #region//付款金额(当天，本周，本月的付款笔数都是0)
            Briefing briefing_pay = new Briefing();
            briefing_pay.Type = "2";
            //briefing_pay.DayNumb = "0";
            //briefing_pay.WeekNumb = "0";
            //briefing_pay.MonthNumb = "0";
            decimal Price = 0;
            int num = 0;
            DataTable dt_pay = new DataTable();
            //当天收款金额
            //sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where";
            //sql+= " ISNULL(dr,0)=0 and Otype=9 and CompID=" + comp.ID + ") and CompID=" + comp.ID +
            //                       " and Date>='" + day0 + "' and Date<='" + Sday + "'";
            sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9) and CompID=" + comp.ID + ")  and status!=3 and CompID=" + comp.ID +
                    " and Date>='" + day0 + "' and Date<'" + Sday + "'  AND isnull(vedf9,0)=1 ";
            dt_pay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dt_pay != null)
            {
                if (dt_pay.Rows.Count > 0)
                {
                    Price =ClsSystem.gnvl(dt_pay.Rows[0]["Price"],"")== ""
                        ? 0
                        : Convert.ToDecimal(dt_pay.Rows[0]["Price"]);
                    //briefing_pay.DayMoney = (Price).ToString("0.00");
                    num = ClsSystem.gnvl(dt_pay.Rows[0]["num"], "") == ""
                        ? 0 : Int32.Parse(dt_pay.Rows[0]["num"].ToString());
                }
            }
            briefing_pay.DayMoney = (Price).ToString("0.00");
            briefing_pay.DayNumb = num.ToString();
            //本周收款金额
            //sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where";
            //sql+= " ISNULL(dr,0)=0 and Otype=9 and CompID=" + comp.ID + ") and CompID=" + comp.ID +
            //                       " and Date>='" + startWeek + "' and Date<='" + endWeek + "'";
            sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9) and CompID=" + comp.ID + ") and status!=3 and CompID=" + comp.ID +
                                    " and Date>='" + startWeek + "' and Date<'" + endWeek + "'  AND isnull(vedf9,0)=1 ";
            dt_pay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dt_pay != null)
            {
                if (dt_pay.Rows.Count > 0)
                {
                    Price = ClsSystem.gnvl(dt_pay.Rows[0]["Price"],"") == ""
                        ? 0
                        : Convert.ToDecimal(dt_pay.Rows[0]["Price"]);
                    //briefing_pay.WeekMoney = (Price).ToString("0.00");
                    num = ClsSystem.gnvl(dt_pay.Rows[0]["num"], "") == ""
                    ? 0 : Int32.Parse(dt_pay.Rows[0]["num"].ToString());
                }
            }
            briefing_pay.WeekMoney = (Price).ToString("0.00");
            briefing_pay.WeekNumb = num.ToString();
            //本月收款金额
            //sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where ";
            //sql += " ISNULL(dr,0)=0 and Otype=9 and CompID=" + comp.ID + ") and CompID=" + comp.ID + " and Date>='" + day1 + "' and Date<='" + Sday + "'  AND vedf9=1 ";
            //dt_pay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            sql = "SELECT count(*) as num,SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9) and CompID=" + comp.ID + ") and status!=3 and CompID=" + comp.ID + " and Date>='" + day1 + "' and Date<'" + mothday + "'  AND isnull(vedf9,0)=1 ";
            dt_pay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dt_pay != null)
            {
                if (dt_pay.Rows.Count > 0)
                {
                    Price = ClsSystem.gnvl(dt_pay.Rows[0]["Price"],"") == ""
                        ? 0
                        : Convert.ToDecimal(dt_pay.Rows[0]["Price"]);
                    //briefing_pay.MonthMoney = (Price).ToString("0.00");
                    num = ClsSystem.gnvl(dt_pay.Rows[0]["num"], "") == ""
                    ? 0 : Int32.Parse(dt_pay.Rows[0]["num"].ToString());
                }
            }
            briefing_pay.MonthMoney = (Price).ToString("0.00");
            briefing_pay.MonthNumb = num.ToString();
            list_briefing.Add(briefing_pay);
            result.Result="T";
            result.Description = "返回成功";
            result.BriefingList = list_briefing;
            return result;
            #endregion


        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompyBriefing:" + JSon);
            return new ResultCompyBriefing() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 获取核心企业统计
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public ResultCompanySta GetCompanySta(string JSon, string version)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DataType = string.Empty;
        string strsql = string.Empty;
        List<string> xlist = new List<string>();
        List<string> xvaluelist = new List<string>();

        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString().Trim() != "" && JInfo["CompID"].ToString().Trim() != "" && JInfo["DataType"].ToString().Trim() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                DataType = JInfo["DataType"].ToString();

            }
            else
            {
                return new ResultCompanySta() { Result = "F", Description = "参数异常" };
            }
            #endregion
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                return new ResultCompanySta() { Result = "F", Description = "登录信息异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultCompanySta() { Result = "F", Description = "核心企业异常" };
            if (DataType == "0")//当月订单额
            {
                //获取当前时间
                DateTime date = DateTime.Now;
                //当月第一天
                DateTime day1 = new DateTime(date.Year, date.Month, 1);
                //当月最后一天
                DateTime day2 = day1.AddMonths(1).AddDays(-1);

                //当月天数
                int days = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                //取出当月每天的订单额
                strsql = @"select CompID,Day([CreateDate]) sday,YEAR([CreateDate]) Years, Month([CreateDate]) smonth, SUM([AuditAmount]) as [TotalAmount],";
                strsql += "sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where compID=" + comp.ID  + "  ";
                strsql+= "and CreateDate BETWEEN DATEADD(mm, DATEDIFF(mm,0,getdate()), 0) and dateadd(d,-day(getdate()),dateadd(m,1,getdate())) and OState in(2,3,4,5,7)) M ";
                strsql += "where compID=" + comp.ID + " group by Day([CreateDate]),Month([CreateDate]),YEAR([CreateDate]),CompID order by sday asc";
                DataTable dt_day = SqlAccess.ExecuteSqlDataTable(strsql,SqlHelper.LocalSqlServer);
                if (dt_day != null && dt_day.Rows.Count > 0)
                {
                    //循环判断dt中是不是存在此日期的数据（dt中的sday是日期），存在的话将totalamount值放入xvaluelist中
                    for (int i = 0; i < days; i++)
                    {
                        DataRow[] dr = dt_day.Select(string.Format("sday='{0}'", day1.ToString("dd")));
                        if (dr.Length > 0)
                            xvaluelist.Add(Convert.ToDecimal(dr[0]["TotalAmount"]).ToString("0.00"));
                        else
                            xvaluelist.Add("0.00");
                        //将日期放入xlist中
                        xlist.Add(day1.ToString("dd"));
                        day1 = day1.AddDays(1);
                    }
                }
                else//dt中没数据的话，每天的数据都是0.00
                {
                    for (int i = 0; i < days; i++)
                    {
                        xvaluelist.Add("0.00");
                        xlist.Add(day1.ToString("dd"));
                        day1 = day1.AddDays(1);
                    }
                }
            }
            else if (DataType == "1")//半年订单额
            {
                //获取当前时间
                DateTime date = DateTime.Now;
                DateTime day1 = date;
                //取出半年的订单额
                strsql = @"select CompID,YEAR([CreateDate]) Years, Month([CreateDate]) smonth,SUM([AuditAmount]) as [TotalAmount],";
                strsql += "sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where ";
                strsql += "compID=" + comp.ID + "  and CreateDate BETWEEN dateadd(month,-6,getdate())  and getDate() and OState in(2,3,4,5,7)) M ";
                strsql += "where compID=" + comp.ID+ " group by Month([CreateDate]),YEAR([CreateDate]),CompID  order by  Years asc ,smonth asc ";
           
          
                DataTable dt_halfyear = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                if (dt_halfyear != null && dt_halfyear.Rows.Count > 0)
                {
                    //循环判断dt中是否存在此年此月的数据，存在的话将totalamount值放入xvaluelist中
                    for (int i = 0; i <= 6; i++)
                    {
                        DataRow[] dr = dt_halfyear.Select(string.Format("Years='{0}' and smonth='{1}'", day1.ToString("yyyy"), day1.ToString("MM")));
                        if (dr.Length > 0)
                            xvaluelist.Add(Convert.ToDecimal(dr[0]["TotalAmount"]).ToString("0.00"));
                        else
                            xvaluelist.Add("0.00");
                        //将年月放入xlist中
                        xlist.Add(day1.ToString("yyyy-MM"));
                        day1 = day1.AddMonths(-1);
                    }
                }
                else//dt中没数据的话,数据所有事0.00
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        xvaluelist.Add("0.00");
                        xlist.Add(day1.ToString("yyyy-MM"));
                        day1 = day1.AddMonths(1);
                    }
                }
                xvaluelist.Reverse();
                xlist.Reverse();
            }
            else if (DataType == "2")
            {
                //获取当前时间
                DateTime date = DateTime.Now;
                DateTime day1 = date;
                //取出全年的订单额
                strsql = @"select CompID,YEAR([CreateDate]) Years, Month([CreateDate]) smonth,SUM([AuditAmount]) as [TotalAmount],";
                strsql += "sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where ";
                strsql += "compID=" + comp.ID + "  and CreateDate BETWEEN dateadd(YEAR,-1,getdate())  and getDate() and OState in(2,3,4,5,7)) M ";
                strsql += "where compID=" + comp.ID + " group by Month([CreateDate]),YEAR([CreateDate]),CompID  order by  Years asc ,smonth asc ";
   
                DataTable dt_year = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                if (dt_year != null && dt_year.Rows.Count > 0)
                {
                    //循环判断dt中是否存在此年此月的数据，存在的话将totalamount值放入xvaluelist中
                    for (int i = 0; i <= 12; i++)
                    {
                        DataRow[] dr = dt_year.Select(string.Format("Years='{0}' and smonth='{1}'", day1.ToString("yyyy"), day1.ToString("MM")));
                        if (dr.Length > 0)
                            xvaluelist.Add(Convert.ToDecimal(dr[0]["TotalAmount"]).ToString("0.00"));
                        else
                            xvaluelist.Add("0.00");
                        //将年月放入xlist中
                        xlist.Add(day1.ToString("yyyy-MM"));
                         day1 = day1.AddMonths(-1);
                    }
                }
                else//dt中没数据的话,数据所有事0.00
                {
                    for (int i = 0; i <= 12; i++)
                    {
                        xvaluelist.Add("0.00");
                        xlist.Add(day1.ToString("yyyy-MM"));
                       day1= day1.AddMonths(-1);
                    }
                }
                xvaluelist.Reverse();
                xlist.Reverse();
            }
            else
            {
                return new ResultCompanySta() { Result = "F",Description = "类型错误"};
            }
            //返回参数
            ResultCompanySta result = new ResultCompanySta();
            result.Result = "T";
            result.Description = "返回成功";
            result.XValueList = xvaluelist;
            result.XList = xlist;
            return result;

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanySta:" + JSon);
            return new ResultCompanySta() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 获取核心企业对应的经销商列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public ResultResellerList GetResellerList(string JSon, string version)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string search = string.Empty;
        string strwhere = string.Empty;
        string classifyid = string.Empty;
        string isenabled = string.Empty;
        string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
        string getType = string.Empty; //方向
        string rows = string.Empty;
        string sortType = string.Empty;
        string sort = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != "" && JInfo["ClassifyID"].ToString() != "" && JInfo["IsEnabled"].ToString()!=""
                && JInfo["CriticalOrderID"].ToString().Trim() != "" && JInfo["GetType"].ToString().Trim() != "" && JInfo["Rows"].ToString().Trim()!=""
                && JInfo["SortType"].ToString().Trim()!="" && JInfo["Sort"].ToString().Trim()!="")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                search = JInfo["Search"].ToString();
                classifyid = JInfo["ClassifyID"].ToString();
                isenabled = JInfo["IsEnabled"].ToString();
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                
            }
            else
            {
                return new ResultResellerList() { Result = "F", Description = "参数异常" };
            }
            #endregion
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                return new ResultResellerList() { Result = "F", Description = "登录信息异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultResellerList() { Result = "F", Description = "核心企业异常" };
            //如果传入的分类id不为-1的话，需要取出此分类下的所有末级分类
            if (classifyid != "-1")
            {
                   Hi.BLL.BD_DisType bll_distype = new Hi.BLL.BD_DisType();
                   Hi.Model.BD_DisType distype = bll_distype.GetModel(Int32.Parse(classifyid));
                //判断经销商分类是否异常
                if (distype == null || distype.CompID != Int32.Parse(CompID))
                    return new ResultResellerList() { Result= "F",Description = "经销商分类异常"};
                if (distype.dr == 1)
                    return new ResultResellerList() { Result= "F",Description = "经销商分类已被删除"};
                //取出此核心企业的所有分类，避免重复查数据库
                List<Hi.Model.BD_DisType> list_distype = bll_distype.GetList("ID,parentid", "compid="+CompID+" and isnull(dr,0)=0","");

                List<Hi.Model.BD_DisType> list_classify = list_distype.FindAll(p => p.ParentId==distype.ID);
                //list_classify没数据就表示此分类为末级分类
                if (list_classify == null || list_classify.Count <= 0)
                    strwhere += " and dis.DisTypeID = " + classifyid + " ";
                else
                {
                    string str_in = string.Empty;
                    for (int i = 0; i < list_classify.Count; i++)
                    {
                        str_in += Common.GetClassifyID(list_classify[i].ID.ToString(), list_distype);
                    }
                    strwhere += " and dis.DisTypeID in (" + classifyid + "" + str_in + ") ";
                }
            }
            //根据核心企业取出对应的经销商list
            strwhere += " and compuser.CompID = " + comp.ID + " and ISNULL(dis.AuditState,0) =2  and ISNULL(dis.dr,0) = 0  and ISNULL(compuser.dr,0) = 0  and isnull(compuser.IsEnabled,0)=1 and CType =2 and UType =5";
            if (search != "")
                strwhere += " and (dis.DisCode like '%" + search + "%' or dis.DisName like '%" + search + "%')";
            if (isenabled != "-1")
            {
                strwhere += " and isnull(dis.IsEnabled,0)=" + isenabled + " ";
            }

            string strsql = new Common().PageSqlString_reseller(criticalOrderID, "ID", "BD_Distributor dis inner join SYS_CompUser compuser on compuser.DisID = dis.ID  ", "dis.CreateDate",
    "1", strwhere, getType, rows);
            DataTable dt = SqlAccess.ExecuteSqlDataTable(strsql,SqlHelper.LocalSqlServer);
            //List<Hi.Model.BD_Distributor> list_dis = new Hi.BLL.BD_Distributor().GetList("ID,DisName,DisCode,Province,City,Area", strwhere, "");
            //循环经销商list,将经销商名称跟ID放入返回的经销商list中
            List<class_ver3.ResellerSimple> list_reseller = new List<class_ver3.ResellerSimple>();
            class_ver3.ResellerSimple reseller = null;
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                reseller = new class_ver3.ResellerSimple();
                reseller.ResellerID =dt.Rows[i]["ID"].ToString();
                reseller.ResellerName = ClsSystem.gnvl(dt.Rows[i]["DisName"],"");
                reseller.Principal = ClsSystem.gnvl(dt.Rows[i]["Principal"], "");
                reseller.Phone = ClsSystem.gnvl(dt.Rows[i]["Phone"], "");
                if (dt.Rows[i]["City"].ToString() == "市辖区")
                    reseller.ResellerAddr = dt.Rows[i]["Province"].ToString() + dt.Rows[i]["Area"].ToString();
                else
                    reseller.ResellerAddr = dt.Rows[i]["City"].ToString() + dt.Rows[i]["Area"].ToString();
                list_reseller.Add(reseller);
            }
            //拼接返回参数
            ResultResellerList resultresellerlist = new ResultResellerList();
            resultresellerlist.Result = "T";
            resultresellerlist.Description = "返回成功";
            resultresellerlist.ResellerList = list_reseller;
            return resultresellerlist;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerList:" + JSon);
            return new ResultResellerList() { Result = "F", Description = "获取失败" };
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
            string disID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                JInfo["Sort"].ToString() != "" && JInfo["State"].ToString() != "" )
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
                disID = JInfo["ResellerID"].ToString();
                //if (JInfo["OState"].ToString() != "")
                //    oState = JInfo["OState"].ToString();
                if (JInfo["PayState"].ToString() != "")
                    payState = JInfo["PayState"].ToString();
            }
            else
            {
                return new ResultOrderList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one, int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };


            //if (oState!="-2")
            //    strWhere += " and OState in (" + oState + ")";
            if (payState != "-1")
                strWhere += " and PayState in(" + payState + ") ";

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
                    List<Hi.Model.DIS_Logistics> log = new Hi.BLL.DIS_Logistics().GetList("",
                        " LogisticsNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                    if (log.Count != 0)
                    {
                        strWhere += " or ID in (" + string.Join(",", log.Select(p => p.OrderID)) + ")";
                    }
                    strWhere += " )";
                }
            }

            //如果经销商id不是0，判断经销商是否存在，存在搜索对应的订单
            if (disID != "0")
            {
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Int32.Parse(disID));
                if (dis == null || dis.AuditState==0||dis.dr ==1)
                    return new ResultOrderList() { Result = "F", Description = "经销商信息异常" };
                strWhere += " and DisID = "+dis.ID+" ";
            }

            if (state != "0" && state != "1" && state != "2" && state != "3" && state != "4" && state != "5" && state != "6" && state != "7")
                return new ResultOrderList() { Result = "F", Description = "状态异常" };

            if (criticalOrderID == "-1")
            {
                switch (state) //0：全部 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 
                //6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已取消 
                //13:待支付 14:退货退款待审核
                {
                    case "2":
                        strWhere += " and (ostate=" + (int)Enums.OrderState.已审 +
                            //" and ((Otype in (" + (int)Enums.OType.销售订单 + "," + (int)Enums.OType.特价订单 + " )" +
                            //" and paystate in (" + (int) Enums.PayState.已支付 + "," + (int) Enums.PayState.部分支付 + " )" +
                                    "  or (ostate = " + (int)Enums.OrderState.已发货 +
                                    " and  isnull(IsOutState,4) in (1,2))) and otype = 0";
                        break;
                    case "1":
                        strWhere += " and ostate=" + (int)Enums.OrderState.待审核 + " and ReturnState=" +
                                    (int)Enums.ReturnState.未退货;
                        break;
                    case "7":
                        strWhere += " and ostate=" + (int)Enums.OrderState.已发货;
                        break;
                    case "13":
                        strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and CompID='"
                            + compID + "' and ReturnState<> 3 and isnull(dr,0)=0";
                        break;
                    case "3":
                        strWhere += " and OState=5 and ReturnState =" + (int)Enums.ReturnState.申请退货 + "";
                        break;
                    case "4":
                        strWhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState=" + (int)Enums.ReturnState.未退货 + "";
                        break;
                    case"5":
                        strWhere += " and OState="+(int)Enums.OrderState.已作废+"";
                        break;
                    case"6":
                        strWhere += "and OState="+(int)Enums.OrderState.已退货+"";
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
                        case "1":
                            strWhere += " and ostate=" + (int)Enums.OrderState.待审核;
                            if (Order.OState != (int)Enums.OrderState.待审核)
                                return new ResultOrderList() { Result = "F", Description = "-1" };
                            break;
                        case "7":
                            strWhere += " and ostate=" + (int)Enums.OrderState.已发货;
                            if (Order.OState != (int)Enums.OrderState.已发货)
                                return new ResultOrderList() { Result = "F", Description = "-1" };
                            break;
                        case "13":
                            strWhere += " and (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and CompID='"
                                + compID + "' and ReturnState in(0,1) ";
                            break;
                        case "3":
                            strWhere += " and OState=5 and ReturnState ='" + (int)Enums.ReturnState.申请退货 + "'";
                            break;
                        case "4":
                            strWhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState="+(int)Enums.ReturnState.未退货+"";
                            break;
                        case "5":
                            strWhere += " and OState=" + (int)Enums.OrderState.已作废 + "";
                            break;
                        case "6":
                            strWhere += "and OState=" + (int)Enums.OrderState.已退货 + "";
                            break;
                    }
                }
                else
                {
                    return new ResultOrderList() { Result = "F", Description = "查询单号异常" };
                }
            }

            strWhere += "   and Otype!=9 and OState !=" + (int)Enums.OrderState.退回;

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
                return new ResultOrderList() { Result = "F", Description = "基础数据异常" };

            #endregion

            #region 赋值


            List<Order> OrderList = new List<Order>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResultOrderList() { Result = "T", Description = "没有更多数据" };
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                    return new ResultOrderList() { Result = "T", Description = "没有更多数据" };
                foreach (DataRow row in orderList.Rows)
                {
                    Order order = new Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    if (orderModel == null || orderModel.dr == 1)
                        return new ResultOrderList() { Result = "F", Description = "订单异常" };
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultOrderList() { Result = "F", Description = "企业异常" };
                    order.CompName = comp.CompName;

                    order.State = state.Trim() == "0" ? (Common.GetCompOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                        orderModel.ReturnState)) : state;
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
                    if (dis == null)
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
                    if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "")
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
                        return new ResultOrderList() { Result = "F", Description = "订单明细异常" };
                    List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        string SKUName = string.Empty;
                        OrderDetail ordetail = new OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        if (goodsInfo == null)
                            //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                            return new ResultOrderList() { Result = "F", Description = "SKU信息异常" };
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null)
                            //if (goods == null || goods.IsEnabled == 0 | goods.dr == 1)
                            return new ResultOrderList() { Result = "F", Description = "商品异常" };
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
                                    ordetail.proInfo = new PromotionInfo()
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
                return new ResultOrderList() { Result = "F", Description = "没有更多数据" };
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
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanyOrderList:" + JSon);
            return new ResultOrderList() { Result = "F", Description = "参数异常" };
        }
    }



    /// <summary>
    /// 核心企业根据订单号，经销商代码，经销商名称搜索订单
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderList CompanyOrderSearch(string JSon)
    {
        try
        {
            string strWhere = string.Empty;

            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string condition = string.Empty;
            Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
            string strsql = string.Empty;
            DataTable dt_all = new DataTable();
            DataTable dt_receipt_disname = new DataTable();

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != ""&&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString()!="")
            {
                //{"CompanyID":"1004","CriticalOrderID":"-1","GetType":"1","OState":"-2","PayState":"-1","Rows":"10","Search":{},"Sort":"0","SortType":"0","State":"2","UserID":"1006"}

                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompID"].ToString();
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                condition = JInfo["Condition"].ToString();
            }
            else
            {
                return new ResultOrderList() { Result = "F", Description = "参数异常" };
            }
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out one, int.Parse(compID == "" ? "0" : compID)))
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };

            //如果condition不为空，即搜索条件不为空，返回搜索条件对应的订单，为空就是所有订单
            if (condition != "")
            {
                //因为不知道搜索条件是订单号，还是经销商名称或编码，所以搜索条件需要做三次匹配，三次的结果都输出
                //首先匹配订单号
                strWhere = " and compID=" + compID + " and ReceiptNo like '%" + condition + "%' and ISNULL(dr,0)=0 and OState between 1 and 7 ";
                strsql = new Common().PageSqlString(criticalOrderID, "ID", "dbo.DIS_Order", "CreateDate", "0", strWhere, getType, "10");
                if (strsql == "")
                    return new ResultOrderList() { Result = "F", Description = "基础数据异常" };
                DataTable dt_receiptno = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
                //匹配经销商名称
                //第一步在经销商表中，取出满足条件的经销商id
                List<Hi.Model.BD_Distributor> list_disid = bll_dis.GetList("id", "DisName like '%" + condition + "%' and ISNULL(AuditState,0)=2 and ISNULL(dr,0)=0", "");
                //将满足条件的id，拼接成字符串
                string disid_string = "";
                foreach (Hi.Model.BD_Distributor dis in list_disid)
                {
                    disid_string += dis.ID + ",";
                }
                //将拼接成的字符串的最后一个，截掉，用于sql语句
                if (disid_string.Length > 0)
                {
                    disid_string = disid_string.Substring(0, disid_string.Length - 2);

                    strWhere = " and compID=" + compID + " and DisID in (" + disid_string + ") and ISNULL(dr,0)=0 and OState between 1 and 7 ";
                    strsql = new Common().PageSqlString(criticalOrderID, "ID", "dbo.DIS_Order", "CreateDate", "0", strWhere, getType, "10");
                    if (strsql == "")
                        return new ResultOrderList() { Result = "F", Description = "基础数据异常" };
                    DataTable dt_disname = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
                    dt_receipt_disname = new Common().getNewDatatable(dt_receiptno, dt_disname);
                }
                else
                {
                    dt_receipt_disname = dt_receiptno;
                }
                //匹配经销商编码
                //第一步在经销商表中，取出满足条件的经销商id
                List<Hi.Model.BD_Distributor> list_disid_code = bll_dis.GetList("id", "DisCode like '%" + condition + "%' and ISNULL(AuditState,0)=2 and ISNULL(dr,0)=0", "");
                //将满足条件的id，拼接成字符串
                disid_string = "";
                foreach (Hi.Model.BD_Distributor dis in list_disid)
                {
                    disid_string += dis.ID + ",";
                }
                //将拼接成的字符串的最后一个，截掉，用于sql语句
                if (disid_string.Length > 0)
                {
                    disid_string = disid_string.Substring(0, disid_string.Length - 2);
                    strWhere = " and compID='" + compID + "' and DisID in (" + disid_string + ") and ISNULL(dr,0)=0 and OState between 1 and 7 ";
                    strsql = new Common().PageSqlString(criticalOrderID, "ID", "dbo.DIS_Order", "CreateDate", "0", strWhere, getType, "10");
                    if (strsql == "")
                        return new ResultOrderList() { Result = "F", Description = "基础数据异常" };
                    DataTable dt_discode = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
                    dt_all = new Common().getNewDatatable(dt_receipt_disname, dt_discode);
                }
                else
                {
                    dt_all = dt_receipt_disname;
                }

            }
            else//没有搜索条件搜索所有订单
            {
                strWhere = " and compID='" + compID + "'  and ISNULL(dr,0)=0 and OState between 1 and 7 ";
                strsql = new Common().PageSqlString(criticalOrderID, "ID", "dbo.DIS_Order", "CreateDate", "0", strWhere, getType, "10");
                if (strsql == "")
                    return new ResultOrderList() { Result = "F", Description = "基础数据异常" };
                dt_all = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            }


            List<Order> OrderList = new List<Order>();
            if (dt_all != null)
            {
                if (dt_all.Rows.Count == 0)
                    return new ResultOrderList() { Result = "T", Description = "没有更多数据" };
                foreach (DataRow row in dt_all.Rows)
                {
                    Order order = new Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    if (orderModel == null || orderModel.dr == 1)
                        return new ResultOrderList() { Result = "F", Description = "订单异常" };
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultOrderList() { Result = "F", Description = "企业异常" };
                    order.CompName = comp.CompName;
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
                    if (dis == null || dis.AuditState == 0 || dis.dr == 1)
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
                    if (ClsSystem.gnvl(orderModel.ArriveDate, "") != "0001/1/1 0:00:00" && ClsSystem.gnvl(orderModel.ArriveDate, "") != "")
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
                        return new ResultOrderList() { Result = "F", Description = "订单明细异常" };
                    List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        string SKUName = string.Empty;
                        OrderDetail ordetail = new OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        if (goodsInfo == null)
                            //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                            return new ResultOrderList() { Result = "F", Description = "SKU信息异常" };
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null)
                            //if (goods == null || goods.IsEnabled == 0 | goods.dr == 1)
                            return new ResultOrderList() { Result = "F", Description = "商品异常" };
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
                                    ordetail.proInfo = new PromotionInfo()
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
                return new ResultOrderList() { Result = "F", Description = "没有更多数据" };
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
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompanyOrderSearch:" + JSon);
            return new ResultOrderList() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 根据订单id，编辑订单(作废)
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultOrderInfo EditComPanyOrder_del(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        JsonData orderjson;
        Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
        Hi.BLL.DIS_Order bll_order = new Hi.BLL.DIS_Order();
        Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();
        #region//JSon取值
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString() == "" || (JInfo["CompID"].ToString() == "" && JInfo["ResellerID"].ToString() == "") ||
                JInfo["Order"].ToString() == "")
            {
                return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                DisID = JInfo["ResellerID"].ToString();
                orderjson = JInfo["Order"];
            }
        #endregion
            //判断登录信息是否正确
            if (CompID == "")//如果核心企业ID是空的话，表示经销商修改订单，判断经销商登录信息是否正确
            {
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, 0, int.Parse(DisID == "" ? "0" : DisID)))
                    return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };
                //判断经销商是否存在
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(DisID == "" ? "0" : DisID));
                if (dis == null || dis.AuditState == 0 || dis.dr == 1 || dis.IsEnabled == 0)
                    return new ResultOrderInfo() { Result = "F", Description = "经销商信息异常" };
                //根据经销商id获取核心企业实体
                comp = new Hi.BLL.BD_Company().GetModel(dis.CompID);
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultOrderInfo() { Result = "F", Description = "此经销商对应的核心企业信息异常" };
            }
            else//表示核心企业修改订单，判断核心企业登录信息是否异常
            {
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                {
                    return new ResultOrderInfo() { Result = "F", Description = "登录信息异常" };
                }
                //判断核心企业信息是否异常
                comp = new Hi.BLL.BD_Company().GetModel(int.Parse(CompID == "" ? "0" : CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultOrderInfo() { Result = "F", Description = "核心企业信息异常" };
            }
            //判断订单状态有没改变
            Hi.Model.DIS_Order ordermodel = bll_order.GetModel(Convert.ToInt32(orderjson["OrderID"].ToString() == "" ? "0" : orderjson["OrderID"].ToString()));
            if (ordermodel.OState != int.Parse(orderjson["Ostate"].ToString() == "" ? "0" : orderjson["Ostate"].ToString()))
                return new ResultOrderInfo() { Result = "1", Description = "订单状态已改变，请重新编辑" };
            //作废订单不允许编辑
            if (ordermodel.OState == 6)
                return new ResultOrderInfo() { Result = "F", Description = "订单已作废，请勿编辑" };
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            string isint = Common.rdoOrderAudit("订单下单数量是否取整", comp.ID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
            int isrebate = Common.rdoOrderAudit("订单支付返利是否启用", comp.ID).ToInt(0);//判断核心企业是否启用返利s
            //如果预计到货日期不为空，需要给到货日期更新，并判断到货日期要大于等于当前日期
            if (orderjson["ArriveDate"].ToString() != "")
            {
                if (DateTime.Compare(DateTime.Parse(orderjson["ArriveDate"].ToString()), DateTime.Now) < 0)
                    return new ResultOrderInfo() { Result = "F", Description = "到货日期必须大于当前时间" };
                ordermodel.ArriveDate = DateTime.Parse(orderjson["ArriveDate"].ToString());
            }
            ordermodel.Remark = orderjson["OrderRemark"].ToString();
            ordermodel.ts = DateTime.Now;
            ordermodel.modifyuser = Convert.ToInt32(UserID);
            ordermodel.GiveMode = orderjson["GiveMode"].ToString();
            ordermodel.PostFee = decimal.Parse(orderjson["PostFee"].ToString() == "" ? "0" : orderjson["PostFee"].ToString());
          
            //收货地址
            Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(ordermodel.AddrID);
            if (DisAddr == null)
            {
                return new ResultOrderInfo() { Result = "F", Description = "收货地址异常" };
            }
            ordermodel.AddrID = Convert.ToInt32(orderjson["AddrID"].ToString() == "" ? "0" : orderjson["AddrID"].ToString());
            ordermodel.Principal = orderjson["Contact"].ToString();
            ordermodel.Phone = orderjson["Phone"].ToString();
            ordermodel.Address = orderjson["Address"].ToString();
            #region //判断返利金额是否大于可用返利金额(只有在下单的时候没使用返利并且修改的时候也没启用返利，是不可以修改返利的)
            decimal rebate = decimal.Parse(orderjson["Rebate"].ToString() == "" ? "0" : orderjson["Rebate"].ToString());
            if (isrebate != 1 && ordermodel.bateAmount == 0)//表示不允许修改返利(只有在修改订单时，跟下单时都没有开启返利，修改订单不允许用返利)
            {
                if (rebate > 0)
                    return new ResultOrderInfo() { Result = "F", Description = "不允许修改返利" };
            }
            else
            {
                //取出可用的返利总额
                string strsql = "select sum(EnableAmount) as RebateSum from BD_Rebate where DisID = '" + DisID + "' ";
                strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
                strsql += " and isnull(RebateState,1) = 1";
                strsql += " and isnull(dr,0) = 0 and compid = '" + CompID + "'";
                string RebateSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                RebateSum = double.Parse(RebateSum).ToString("F2");
                if (rebate > decimal.Parse(RebateSum))
                {
                    return new ResultOrderInfo() { Result = "F", Description = "使用的返利金额大于可用返利金额" };
                }
                ordermodel.bateAmount = rebate;
            }

            #endregion
            #region//修改订单拓展表
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(ordermodel.ID.ToString());
            JsonData orderextjson = orderjson["Invoice"];
            if (orderextjson["IsOBill"].ToString() == "0")
            {
                orderextModel.DisAccID = null;
                orderextModel.Rise = "";
                orderextModel.Content = "";
                orderextModel.OBank = "";
                orderextModel.OAccount = "";
                orderextModel.TRNumber = "";
                orderextModel.IsOBill = 0;
                orderextModel.BillNo = "";
                orderextModel.IsBill = 0;
            }
            else
            {
                orderextModel.Rise = orderextjson["Rise"].ToString();
                orderextModel.Content = orderextjson["Content"].ToString();
                orderextModel.OBank = orderextjson["OBank"].ToString();
                orderextModel.OAccount = orderextjson["OAccount"].ToString();
                orderextModel.TRNumber = orderextjson["TRNumber"].ToString();
                orderextModel.IsOBill = int.Parse(orderextjson["IsOBill"].ToString());
                orderextModel.BillNo = orderextjson["BillNo"].ToString();
                orderextModel.IsBill = int.Parse(orderextjson["IsBill"].ToString());
            }
            #endregion
            #region//在订单未审核之前修改的需要判断订单子表
            List<Hi.Model.DIS_OrderDetail> list_odetailmodel = new List<DIS_OrderDetail>();
            //获取修改前订单子表的数据跟修改后的订单子表比较，只有在子表变化后才能重新计算价格
            List<Hi.Model.DIS_OrderDetail> list_orderdetailmodel = bll_orderdetail.GetList("", "OrderID=" + orderjson["OrderID"] + " and ISNULL(dr,0)==0 and DisID=" + orderjson["DisID"] + "", "");
            if (ordermodel.OState == 1)
            {
                int ischange = 0;//用于判断子表是否改变，0表示没变，1表示已经改变
           
                //判断是否删除了下单的商品
                if (orderjson["OrderDetailList"].Count != list_orderdetailmodel.Count)
                {
                    ischange = 1;
                }
                #region//友情提醒下面的代码慎看！！！用于判断除了删除下单商品之外的改变商品的数据的情况（同时在增加新商品，跟修改商品数量，并且启用库存的时候，判断库存是否足够）(同时在商品数量取整的时候判断商品数量是不是整数)

                //循环修改后的子表数据，跟数据库里的修改前的子表数据循环比对
                foreach (JsonData Jlist in orderjson["OrderDetailList"])
                {
                    //商品数量取整的时候判断商品数量是不是整数(就算下单的时候保留两位小数，但修改时要取整，修改后保存的商品数量也要取整，所以就算没修改数量的商品也要判断)
                    if (isint == "0")
                    {
                        decimal num_deci = decimal.Parse(Jlist["Num"].ToString());
                        int num_int = (int)num_deci;
                        int isint_out;
                        if (!int.TryParse(Jlist["Num"].ToString(), out isint_out))
                        {
                            if (decimal.Parse(num_int.ToString()) != num_deci)
                            {
                                return new ResultOrderInfo() { Result = "F", Description = "商品数量应为整数" };
                            }
                        }
                    }

                    int num = 0;
                    foreach (Hi.Model.DIS_OrderDetail orderdetailmodel in list_orderdetailmodel)
                    {
                        //如果两个skuid相同，则判断商品价格商品数量是否一致
                        if (Jlist["SKUID"].ToString() == orderdetailmodel.GoodsinfoID.ToString())
                        {
                            num++;
                            //判断商品数量是否一致,不一致需要判断库存
                            if (decimal.Parse(Jlist["Num"].ToString()) != orderdetailmodel.GoodsNum)
                            {
                                ischange = 1;
                                //启用库存，判断库存是否足够（现在商品库存等于goodsinfo表的库存加上下单数量）
                                if (IsInve == 0)
                                {
                                    Hi.Model.BD_GoodsInfo goodsinfo = bll_goodsinfo.GetModel(orderdetailmodel.GoodsinfoID);
                                    decimal inv = goodsinfo.Inventory + orderdetailmodel.GoodsNum;//可用库存
                                    if (decimal.Parse(Jlist["Num"].ToString()) > inv)
                                    {
                                        return new ResultOrderInfo() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                                    }
                                }

                            }
                            else if (decimal.Parse(Jlist["TinkerPrice"].ToString()) != orderdetailmodel.AuditAmount)
                            {
                                ischange = 1;
                            }
                            break;
                        }
                    }
                    if (num == 0)//num为零的话，表示商品匹配不到修改前的子表数据，表示商品是新加的
                    {
                        ischange = 1;
                        if (IsInve == 0)
                        {
                            Hi.Model.BD_GoodsInfo goodsinfo = bll_goodsinfo.GetModel(Convert.ToInt32(Jlist["SKUID"]));
                            if (decimal.Parse(Jlist["Num"].ToString()) > goodsinfo.Inventory)
                            {
                                return new ResultOrderInfo() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                            }
                        }
                    }
                }

                #endregion
                if (ischange == 1)//表示商品已发生变化，需要重新计算价格
                {
                    #region//取价格
                    Hi.Model.DIS_OrderDetail orderdetailmodel;
                    decimal totalamount = 0;
                    //循环修改后的子表数据，计算每个商品的单价
                    foreach (JsonData Jlist in orderjson["OrderDetailList"])
                    {
                        Hi.Model.BD_GoodsInfo goodsInfo = bll_goodsinfo.GetModel(Convert.ToInt32(Jlist["SKUID"]));
                        int ProID = 0;//促销ID
                        //获取促销ID
                        decimal price = Common.GetProPrice(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(), goodsInfo.CompID.ToString(), out ProID);
                        string Price = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), Convert.ToInt32(orderjson["DisID"]), Convert.ToInt32(Jlist["SKUID"])).ToString("0.00");
                        string pty;
                        string ppty;
                        string pid_num;
                        decimal ProNum =
                        Common.GetProNum(Jlist["ProductID"].ToString(), Jlist["SKUID"].ToString(), Convert.ToInt32(CompID), decimal.Parse(Jlist["Num"].ToString()), out pty, out ppty, out pid_num);
                        string Protype = ppty;

                        List<Hi.Model.DIS_OrderDetail> list_orderdetal = bll_orderdetail.GetList("",
    "OrderID= " + orderjson["OrderID"] + " and GoodsinfoID=" + Jlist["SKUID"] + " and ISNULL(dr,0) =0 and DisID=" + orderjson["DisID"] + "", "");
                        if (list_orderdetal == null || list_orderdetal.Count == 0)
                        {
                            orderdetailmodel = new Hi.Model.DIS_OrderDetail();
                            orderdetailmodel.OrderID = Int32.Parse(orderjson["OrderID"].ToString());
                            orderdetailmodel.DisID = Int32.Parse(orderjson["DisID"].ToString());
                            //根据传入的子表的商品ID获取商品实体
                            Hi.Model.BD_Goods goodsmodel = bll_goods.GetModel(goodsInfo.GoodsID);
                            //需要新增的字段
                            orderdetailmodel.GoodsinfoID = goodsInfo.ID;
                            orderdetailmodel.GoodsName = goodsmodel.GoodsName;
                            orderdetailmodel.GoodsCode = goodsmodel.GoodsCode;
                            orderdetailmodel.Unit = goodsmodel.Unit;
                            orderdetailmodel.GoodsInfos = goodsInfo.ValueInfo;
                            orderdetailmodel.GoodsNum = decimal.Parse(Jlist["Num"].ToString());
                            orderdetailmodel.OutNum = 0;
                            orderdetailmodel.IsOut = 0;

                        }
                        else
                        {
                            orderdetailmodel = list_orderdetal[0];
                        }
                        //需要更新的字段
                        orderdetailmodel.GoodsPrice = decimal.Parse(Price);
                        orderdetailmodel.AuditAmount = decimal.Parse(Price);
                        orderdetailmodel.sumAmount = decimal.Parse(Price) * decimal.Parse(Jlist["Num"].ToString());
                        orderdetailmodel.ts = DateTime.Now;
                        orderdetailmodel.modifyuser = Int32.Parse(UserID);
                        orderdetailmodel.ProID = ProID.ToString();
                        orderdetailmodel.ProNum = ProNum.ToString("0.00");
                        orderdetailmodel.Protype = Protype;
                        //整单总价
                        totalamount += orderdetailmodel.sumAmount;
                        list_odetailmodel.Add(orderdetailmodel);


                    }
                    //获取新的整单促销的信息
                    string pid = "0";
                    string ProIDD = "";
                    string protpye = "";
                    decimal pprice = Common.GetProPrice(totalamount, out pid, out ProIDD, out protpye,comp.ID);
                    //更新整单促销信息
                    orderextModel.ProID = pid.ToInt();
                    orderextModel.ProAmount = pprice;
                    orderextModel.ProDID = ProIDD.ToInt();
                    orderextModel.Protype = protpye;
                    //更新整单的总价
                    ordermodel.TotalAmount = totalamount - pprice - rebate;
                    ordermodel.AuditAmount = totalamount - pprice - rebate;

                    #endregion
                }



            }
            else//订单在其他状态下可以修改订单子表备注
            {
                //循环传入的子表跟之前的子表数据做比较判断备注是否改变
                foreach (JsonData Jlist in orderjson["OrderDetailList"])
                {
                    var odetail = from Hi.Model.DIS_OrderDetail p in list_orderdetailmodel
                                  where p.ID == Int32.Parse(Jlist["OrderDetailID"].ToString())
                                  select p;
                    Hi.Model.DIS_OrderDetail orderdetail = (Hi.Model.DIS_OrderDetail) odetail;
                    //传入的备注跟之前的不同话，就修改之前的备注
                    if (orderdetail.Remark != Jlist["Remark"].ToString())
                    {
                        orderdetail.Remark = Jlist["Remark"].ToString();
                        list_odetailmodel.Add(orderdetail);
                    }
                }
            }
            SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
            con.Open();
            SqlTransaction mytran = con.BeginTransaction();
            try
            {
                //更新订单主表
                int orderid = bll_order.UpdateOrder(con, ordermodel, mytran);
                //更新订单拓展表
                new Hi.BLL.DIS_OrderExt().Update(con, orderextModel, mytran);
            }
            catch (Exception ex)
            {
                mytran.Rollback();
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "ResultOrderInfo:" + JSon);
                return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
            }
            finally
            {
                con.Close();
                mytran.Dispose();
            }
            
            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "ResultOrderInfo:" + JSon);
            return new ResultOrderInfo() { Result = "F", Description = "参数异常" };
        }
        return new ResultOrderInfo();

    }


    /// <summary>
    /// 根据订单id，编辑订单
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEdit EditComPanyOrder(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        JsonData orderjson;
        Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
        Hi.BLL.DIS_Order bll_order = new Hi.BLL.DIS_Order();
        Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();
        #region//JSon取值
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString() == "" || (JInfo["CompID"].ToString() == "" && JInfo["ResellerID"].ToString() == "") ||
                JInfo["Order"].ToString() == "")
            {
                return new ResultEdit() { Result = "F", Description = "参数异常" };
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                DisID = JInfo["ResellerID"].ToString();
                orderjson = JInfo["Order"];
            }
        #endregion
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (CompID == "")//如果核心企业ID是空的话，表示经销商修改订单，判断经销商登录信息是否正确
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, 0, int.Parse(DisID == "" ? "0" : DisID)))
                    return new ResultEdit() { Result = "F", Description = "登录信息异常" };
                //判断经销商是否存在
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(DisID == "" ? "0" : DisID));
                if (dis == null || dis.AuditState == 0 || dis.dr == 1 || dis.IsEnabled == 0)
                    return new ResultEdit() { Result = "F", Description = "经销商信息异常" };
                //根据经销商id获取核心企业实体
                comp = new Hi.BLL.BD_Company().GetModel(dis.CompID);
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultEdit() { Result = "F", Description = "此经销商对应的核心企业信息异常" };
            }
            else//表示核心企业修改订单，判断核心企业登录信息是否异常
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                {
                    return new ResultEdit() { Result = "F", Description = "登录信息异常" };
                }
                //判断核心企业信息是否异常
                comp = new Hi.BLL.BD_Company().GetModel(int.Parse(CompID == "" ? "0" : CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            }
            //判断订单状态有没改变
            Hi.Model.DIS_Order ordermodel = bll_order.GetModel(Convert.ToInt32(orderjson["OrderID"].ToString() == "" ? "0" : orderjson["OrderID"].ToString()));
            if (ordermodel.OState != int.Parse(orderjson["Ostate"].ToString() == "" ? "0" : orderjson["Ostate"].ToString()) ||
                orderjson["ts"].ToString() != ordermodel.ts.ToString())
                return new ResultEdit() { Result = "1", Description = "订单状态已改变，请重新编辑" };
            //作废订单不允许编辑
            if (ordermodel.OState == 6)
                return new ResultEdit() { Result = "F", Description = "订单已作废，请勿编辑" };
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            string isint = Common.rdoOrderAudit("订单下单数量是否取整", comp.ID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
            int isrebate = Common.rdoOrderAudit("订单支付返利是否启用", comp.ID).ToInt(0);//判断核心企业是否启用返利s
            //如果预计到货日期不为空，需要给到货日期更新，并判断到货日期要大于等于当前日期
            if (orderjson["ArriveDate"].ToString() != "")
            {
                //if (DateTime.Compare(DateTime.Parse(orderjson["ArriveDate"].ToString()), DateTime.Now) < 0)
                //    return new ResultEdit() { Result = "F", Description = "到货日期必须大于当前时间" };
                ordermodel.ArriveDate = DateTime.Parse(orderjson["ArriveDate"].ToString());
            }
            ordermodel.Remark = Common.NoHTML(orderjson["OrderRemark"].ToString());
            ordermodel.ts = DateTime.Now;
            ordermodel.modifyuser = Convert.ToInt32(UserID);
            ordermodel.GiveMode = orderjson["GiveMode"].ToString();
            ordermodel.PostFee = decimal.Parse(orderjson["PostFee"].ToString() == "" ? "0" : orderjson["PostFee"].ToString());

            //收货地址
            Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(ordermodel.AddrID);
            if (DisAddr == null)
            {
                return new ResultEdit() { Result = "F", Description = "收货地址异常" };
            }
            ordermodel.AddrID = Convert.ToInt32(orderjson["AddrID"].ToString() == "" ? "0" : orderjson["AddrID"].ToString());
            ordermodel.Principal = orderjson["Contact"].ToString();
            ordermodel.Phone = orderjson["Phone"].ToString();
            ordermodel.Address = orderjson["Address"].ToString();
          
            #region//修改订单拓展表
            Hi.Model.DIS_OrderExt orderextModel = new Common().GetOrderExtByOrderID(ordermodel.ID.ToString());
            JsonData orderextjson = orderjson["Invoice"];
            if (orderextjson["IsOBill"].ToString() == "0")
            {
                orderextModel.DisAccID = null;
                orderextModel.Rise = "";
                orderextModel.Content = "";
                orderextModel.OBank = "";
                orderextModel.OAccount = "";
                orderextModel.TRNumber = "";
                orderextModel.IsOBill = 0;
                orderextModel.BillNo = "";
                orderextModel.IsBill = 0;
            }
            else
            {
                orderextModel.Rise = Common.NoHTML(orderextjson["Rise"].ToString());
                orderextModel.Content = Common.NoHTML(orderextjson["Content"].ToString());
                if (orderextjson["IsOBill"].ToString() == "2")
                {
                    orderextModel.OBank = Common.NoHTML(orderextjson["OBank"].ToString());
                    orderextModel.OAccount = Common.NoHTML(orderextjson["OAccount"].ToString());
                    orderextModel.TRNumber = Common.NoHTML(orderextjson["TRNumber"].ToString());
                }
                orderextModel.IsOBill = int.Parse(orderextjson["IsOBill"].ToString());
                orderextModel.BillNo = Common.NoHTML(orderextjson["BillNo"].ToString());
                orderextModel.IsBill = int.Parse(orderextjson["IsBill"].ToString());
            }
            #endregion
            #region//在订单未审核之前修改的需要判断订单子表
            List<Hi.Model.DIS_OrderDetail> list_odetailmodel = new List<DIS_OrderDetail>();
            //获取修改前订单子表的数据跟修改后的订单子表比较，只有在子表变化后才能重新计算价格
            List<Hi.Model.DIS_OrderDetail> list_orderdetailmodel = bll_orderdetail.GetList("", "OrderID=" + orderjson["OrderID"] + " and ISNULL(dr,0)=0 ", "");
            if (ordermodel.OState == 1)
            {
                #region //判断返利金额是否大于可用返利金额(只有在下单的时候没使用返利并且修改的时候也没启用返利，是不可以修改返利的)
                decimal rebate = decimal.Parse(orderjson["Rebate"].ToString() == "" ? "0" : orderjson["Rebate"].ToString());
                if (isrebate != 1 && ordermodel.bateAmount == 0)//表示不允许修改返利(只有在修改订单时，跟下单时都没有开启返利，修改订单不允许用返利)
                {
                    if (rebate > 0)
                        return new ResultEdit() { Result = "F", Description = "不允许修改返利" };
                }
                else
                {
                    //取出可用的返利总额
                    //string strsql = "select sum(EnableAmount) as RebateSum from BD_Rebate where DisID = " + orderjson["DisID"] + " ";
                    //strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
                    //strsql += " and isnull(RebateState,1) = 1";
                    //strsql += " and isnull(dr,0) = 0 and compid = '" + CompID + "'";
                    //string RebateSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                    string RebateSum = Common.GetRebate(Int32.Parse(orderjson["OrderID"].ToString()), Int32.Parse(orderjson["DisID"].ToString()));
                    RebateSum = double.Parse(RebateSum).ToString("F2");
                    if (rebate > decimal.Parse(RebateSum))
                    {
                        return new ResultEdit() { Result = "F", Description = "使用的返利金额大于可用返利金额" };
                    }
                    ordermodel.bateAmount = rebate;
                }

                #endregion

                    #region//取价格
                Hi.Model.DIS_OrderDetail orderdetailmodel;
                    decimal totalamount = 0;
                    //循环修改后的子表数据，计算每个商品的单价,并判断商品是否取整，库存是否足够
                    foreach (JsonData Jlist in orderjson["OrderDetailList"])
                    {
                        if (decimal.Parse(Jlist["Num"].ToString()) <= 0)
                            return new ResultEdit() { Result = "F",Description = "商品数量必须大于0"};
                        //商品数量取整的时候判断商品数量是不是整数(就算下单的时候保留两位小数，但修改时要取整，修改后保存的商品数量也要取整，所以就算没修改数量的商品也要判断)
                        if (isint == "0")
                        {
                            decimal num_deci = decimal.Parse(Jlist["Num"].ToString());
                            int num_int = (int)num_deci;
                            int isint_out;
                            if (!int.TryParse(Jlist["Num"].ToString(), out isint_out))
                            {
                                if (decimal.Parse(num_int.ToString()) != num_deci)
                                {
                                    return new ResultEdit() { Result = "F", Description = "商品数量应为整数" };
                                }
                            }
                        }
                        //判断商品是否可售
                        Hi.Model.BD_GoodsInfo goodsinfo = bll_goodsinfo.GetModel(Int32.Parse(Jlist["SKUID"].ToString()));
                        //判断这个商品有没被下架删除
                        Hi.Model.BD_Goods goodsmodel = bll_goods.GetModel(goodsinfo.GoodsID);
                        if (goodsinfo == null || goodsinfo.dr == 1 || !goodsinfo.IsEnabled || goodsinfo.IsOffline == 0)
                        {
                            return new ResultEdit() { Result = "F", Description = "" + goodsmodel.GoodsName + "" + goodsinfo.ValueInfo + "不可购买" };
                        }
                        //判断商品的可销售区域
                        List<Common.GoodsID> list = Common.DisEnAreaGoodsID(orderjson["DisID"].ToString(), comp.ID.ToString());
                        Common.GoodsID common_goodsid = new Common.GoodsID();
                        common_goodsid.goodsID = goodsmodel.ID.ToString();
                        if (list.Contains(common_goodsid))
                        {
                            return new ResultEdit() { Result = "F", Description = "" + goodsmodel.GoodsName + "" + goodsinfo.ValueInfo + "不可购买" };
                        }
                        //判断修改的商品库存够不够
                        int num = 0;
                        foreach (Hi.Model.DIS_OrderDetail orderdetailmodel1 in list_orderdetailmodel)
                        {
                            if (Jlist["SKUID"].ToString() == orderdetailmodel1.GoodsinfoID.ToString())
                            {
                                num++;
                               
                                //判断商品数量有没改变
                                if (decimal.Parse(Jlist["Num"].ToString()) != orderdetailmodel1.GoodsNum)
                                {
                                    //启用库存，判断库存是否足够（现在商品库存等于goodsinfo表的库存加上下单数量）
                                    if (IsInve == 0)
                                    {
                                        decimal inv = goodsinfo.Inventory + orderdetailmodel1.GoodsNum;//可用库存
                                        string pty_out, ppty_out, pid_num_out;
                                        decimal ProNum_out = Common.GetProNum(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(), goodsinfo.CompID, decimal.Parse(Jlist["Num"].ToString()), out pty_out, out ppty_out, out pid_num_out);
                                        if (decimal.Parse(Jlist["Num"].ToString())+ProNum_out > inv)
                                        {
                                            return new ResultEdit() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                                        }
                                    }

                                }
                                break;
                            }

                        }
                        if (num == 0)//num为零的话，表示商品匹配不到修改前的子表数据，表示商品是新加的
                        {
                            //判断库存够不够
                            if (IsInve == 0)
                            {
                                //Hi.Model.BD_GoodsInfo goodsinfo = bll_goodsinfo.GetModel(Convert.ToInt32(Jlist["SKUID"]));
                                string pty_out, ppty_out, pid_num_out;
                                decimal ProNum_out = Common.GetProNum(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(), goodsinfo.CompID, decimal.Parse(Jlist["Num"].ToString()), out pty_out, out ppty_out, out pid_num_out);
                                if (decimal.Parse(Jlist["Num"].ToString())+ProNum_out > goodsinfo.Inventory)
                                {
                                    return new ResultEdit() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                                }
                            }
                        }

                        Hi.Model.BD_GoodsInfo goodsInfo = bll_goodsinfo.GetModel(Convert.ToInt32(Jlist["SKUID"].ToString()));
                        int ProID = 0;//促销ID
                        //获取促销ID
                        decimal price = Common.GetProPrice(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(), goodsInfo.CompID.ToString(), out ProID);
                        string Price = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), Convert.ToInt32(orderjson["DisID"].ToString()), Convert.ToInt32(Jlist["SKUID"].ToString())).ToString("0.00");
                        string pty;
                        string ppty;
                        string pid_num;
                        decimal ProNum =
                        Common.GetProNum(Jlist["ProductID"].ToString(), Jlist["SKUID"].ToString(), Convert.ToInt32(CompID), decimal.Parse(Jlist["Num"].ToString()), out pty, out ppty, out pid_num);
                        string Protype = ppty;

    //                    List<Hi.Model.DIS_OrderDetail> list_orderdetal = bll_orderdetail.GetList("",
    //"OrderID= " + orderjson["OrderID"] + " and GoodsinfoID=" + Jlist["SKUID"] + " and ISNULL(dr,0) =0 and DisID=" + orderjson["DisID"] + "", "");
                        Hi.Model.DIS_OrderDetail isfind = list_orderdetailmodel.Find(p => p.GoodsinfoID == Int32.Parse(Jlist["SKUID"].ToString()));
                        if (isfind == null)
                        {
                            orderdetailmodel = new Hi.Model.DIS_OrderDetail();
                            orderdetailmodel.OrderID = Int32.Parse(orderjson["OrderID"].ToString());
                            orderdetailmodel.DisID = Int32.Parse(orderjson["DisID"].ToString());

                            //根据传入的子表的商品ID获取商品实体
                            //Hi.Model.BD_Goods goodsmodel = bll_goods.GetModel(goodsInfo.GoodsID);
                            //需要新增的字段
                            orderdetailmodel.GoodsinfoID = goodsInfo.ID;
                            orderdetailmodel.GoodsName = goodsmodel.GoodsName;
                            orderdetailmodel.GoodsCode = goodsmodel.GoodsCode;
                            orderdetailmodel.Unit = goodsmodel.Unit;
                            orderdetailmodel.GoodsInfos = goodsInfo.ValueInfo;
                            orderdetailmodel.OutNum = 0;
                            orderdetailmodel.IsOut = 0;

                        }
                        else
                        {
                            orderdetailmodel = isfind;
                        }
                        //需要更新的字段
                        orderdetailmodel.GoodsNum = decimal.Parse(Jlist["Num"].ToString());
                        orderdetailmodel.GoodsPrice = decimal.Parse(Price);//商品单价
                        orderdetailmodel.AuditAmount = decimal.Parse(Jlist["TinkerPrice"].ToString());
                        orderdetailmodel.sumAmount = decimal.Parse(Jlist["TinkerPrice"].ToString()) * decimal.Parse(Jlist["Num"].ToString());
                        orderdetailmodel.ts = DateTime.Now;
                        orderdetailmodel.modifyuser = Int32.Parse(UserID);
                        orderdetailmodel.ProID = ProID.ToString();
                        orderdetailmodel.ProNum = ProNum.ToString("0.00");
                        orderdetailmodel.Protype = Protype;
                        orderdetailmodel.Remark = Jlist["Remark"].ToString();
                        //整单总价
                        totalamount += orderdetailmodel.sumAmount;
                        list_odetailmodel.Add(orderdetailmodel);


                    }
                    //获取新的整单促销的信息
                    string pid = "0";
                    string ProIDD = "";
                    string protpye = "";
                    decimal pprice = Common.GetProPrice(totalamount, out pid, out ProIDD, out protpye, comp.ID);
                    //更新整单促销信息
                    orderextModel.ProID = pid.ToInt();
                    orderextModel.ProAmount = pprice;
                    orderextModel.ProDID = ProIDD.ToInt();
                    orderextModel.Protype = protpye;
                    //更新整单的总价
                    ordermodel.TotalAmount = totalamount;
                    ordermodel.AuditAmount = totalamount - pprice - rebate + ordermodel.PostFee;

                    #endregion
                //找到被删除的商品
                    string sqlstr = "";
                    foreach (Hi.Model.DIS_OrderDetail model_orderdetail in list_orderdetailmodel)
                    {
                        int i = 0;
                        foreach (JsonData Jlist in orderjson["OrderDetailList"])
                        {
                            if (model_orderdetail.GoodsinfoID == Int32.Parse(Jlist["SKUID"].ToString()))
                            {
                                i++;
                                break;
                            }
                        }
                        //i=0表示现在的子表中没有商品，及商品被删除
                        if (i == 0)
                        {
                            sqlstr = sqlstr + model_orderdetail.GoodsinfoID + ",";
                        }
                    }
                    string where = "";
                    if (sqlstr != "")
                    {
                        sqlstr = sqlstr.Substring(0,sqlstr.Length-1);
                        where = " DisID=" + orderjson["DisID"] + " and OrderID=" + orderjson["OrderID"] + " and GoodsInfoID in (" + sqlstr + ")";
                    }
                //更新数据
                    int orderId = Common.UpdateOrder(orderjson["ts"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(orderjson["ts"].ToString()), ordermodel, orderextModel, list_odetailmodel, where);
                    if (orderId <= 0)
                    {
                        return new ResultEdit() { Result = "F", Description = "订单提交失败" };
                    }


                 
            }
            else//订单在其他状态下可以修改订单子表备注
            {
                //循环传入的子表跟之前的子表数据做比较判断备注是否改变
                //foreach (JsonData Jlist in orderjson["OrderDetailList"])
                //{
                //    //var odetail = from Hi.Model.DIS_OrderDetail p in list_orderdetailmodel
                //    //              where p.ID == Int32.Parse(Jlist["OrderDetailID"].ToString()) 
                //    //              select p;
                //    //Hi.Model.DIS_OrderDetail orderdetail = (Hi.Model.DIS_OrderDetail)odetail;
                //    Hi.Model.DIS_OrderDetail orderdetail = list_orderdetailmodel.Find(p => p.ID == Int32.Parse(Jlist["OrderDetailID"].ToString()));
                //    //传入的备注跟之前的不同话，就修改之前的备注
                //    if (orderdetail.Remark != Jlist["Remark"].ToString())
                //    {
                //        orderdetail.Remark = Jlist["Remark"].ToString();
                //        list_odetailmodel.Add(orderdetail);
                //    }
                //}
                SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
                conn.Open();
                SqlTransaction mytran = conn.BeginTransaction();
                try
                {
                    //更新订单主表和订单拓展表
                    if (!bll_order.Update(ordermodel, mytran)||!new Hi.BLL.DIS_OrderExt().Update(conn, orderextModel, mytran))
                    {
                        mytran.Rollback();
                        return new ResultEdit() { Result ="F",Description ="订单提交失败"};
                    }
                    //int count = 0;
                    //foreach (Hi.Model.DIS_OrderDetail dis_orderdetailmodel in list_odetailmodel)
                    //{
                    //    count = bll_orderdetail.UpdateOrderDetail(conn, dis_orderdetailmodel, mytran);
                    //    if (count == 0)
                    //    {
                    //        mytran.Rollback();
                    //        return new ResultEdit() { Result = "F", Description = "订单提交失败" };
                    //    }
                    //}
                    mytran.Commit();
                }
                catch (Exception ex)
                {
                    mytran.Rollback();
                    Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditComPanyOrder:" + JSon);
                    return new ResultEdit() { Result = "F", Description = "更新异常" };

                }
                finally
                {
                    conn.Close();
                    mytran.Dispose();
                }
            }

            Common.AddSysBusinessLog(ordermodel, one, "Order", ordermodel.ID.ToString(), "订单修改", "");
            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditComPanyOrder:" + JSon);
            return new ResultEdit() { Result = "F", Description = "参数异常" };
        }
        return new ResultEdit() { Result = "T",Description ="订单提交成功"};

    }

    /// <summary>
    /// 根据发货单id，编辑发货单
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEdit EditOrderOut(String JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        JsonData orderoutjson;
        Hi.Model.BD_Distributor dis;
        Hi.Model.BD_Company comp;
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        Hi.BLL.BD_GoodsInfo  bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        try
        {
            #region//json取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["OrderOut"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "")
            {
                return new ResultEdit() { Result = "F", Description = "参数异常" };
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                orderoutjson = JInfo["OrderOut"];
            }
            //判断登录信息是否正确

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            comp = new Hi.BLL.BD_Company().GetModel(int.Parse(CompID == "" ? "0" : CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultEdit() { Result = "F", Description = "核心企业信息异常" };

            #endregion
            //根据发货单id获取返货单实体
            Hi.Model.DIS_OrderOut orderoutModel = new Hi.BLL.DIS_OrderOut().GetModel(Int32.Parse(orderoutjson["OrderOutID"].ToString()));
            //根据订单id获取订单实体
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(Int32.Parse(orderoutjson["OrderID"].ToString()));
            //判断发货单的状态是否已经改变和发货单已经被他人修改过
            if (orderoutjson["ts"].ToString() != orderoutModel.ts.ToString())
            {
                return new ResultEdit() { Result = "1", Description = "发货单状态已改变，请重新编辑" };
            }
            //判断发货单是否被作废了，签收了
            if (orderoutModel.IsAudit == 3 || orderoutModel.IsSign == 1)
            {
                return new ResultEdit() { Result = "F", Description = "发货单已作废或已签收，请勿编辑" };
            }
            if (orderModel.OState != (int)Enums.OrderState.已审 && orderModel.OState != (int)Enums.OrderState.已发货)
                return new ResultEdit() { Result = "F", Description = "订单状态不对，请勿编辑" };
            //修改发货单主表(dis_orderout)
            orderoutModel.ts = DateTime.Now;
            orderoutModel.modifyuser = Int32.Parse(UserID);
            List<Hi.Model.DIS_OrderDetail> list_orderdeupdate = new List<Hi.Model.DIS_OrderDetail>();
            List<Hi.Model.DIS_OrderOutDetail> list_orderoutdeupdate = new List<Hi.Model.DIS_OrderOutDetail>();
            #region//修改发货单子表(dis_orderoutdetail)及订单明细表
            //根据发货单取出对应的发货单明细表的List
            List<Hi.Model.DIS_OrderOutDetail> list_orderoutdeModel = new Hi.BLL.DIS_OrderOutDetail().GetList("", "OrderOutID = " + orderoutModel.ID + " and isnull(dr,0)=0", "");
            List<Hi.Model.DIS_OrderDetail> list_orderdeModel = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID=" + orderModel.ID + " and isnull(dr,0)=0", "");
            foreach (JsonData orderoutdetjson in orderoutjson["SendOutDetailList"])
            {
                //从订单明细表list中找到，此条发货单明细对应的订单明细
                //var orderde = from p in list_orderdeModel
                //                   where p.GoodsinfoID == orderoutdeModel.GoodsinfoID
                //                   select p;
                //Hi.Model.DIS_OrderDetail orderdeModel = (Hi.Model.DIS_OrderDetail)orderde;
                if (decimal.Parse(orderoutdetjson["OutNum"].ToString()) <= 0)
                    return new ResultEdit() { Result = "F", Description = "发货商品数量必须大于0" };
                Hi.Model.DIS_OrderOutDetail orderoutdeModel = list_orderoutdeModel.Find(p => p.GoodsinfoID == Int32.Parse(orderoutdetjson["SKUID"].ToString()));
                Hi.Model.DIS_OrderDetail orderdeModel = list_orderdeModel.Find(p => p.GoodsinfoID == orderoutdeModel.GoodsinfoID);
                decimal num_able = orderdeModel.GoodsNum - orderdeModel.OutNum + orderoutdeModel.OutNum+decimal.Parse(orderdeModel.ProNum);
                //判断修改后的发货数量是否大于可发货数量
                if (decimal.Parse(orderoutdetjson["OutNum"].ToString()) > num_able)
                {
                    Hi.Model.BD_GoodsInfo goodsinfomodel = bll_goodsinfo.GetModel(orderoutdeModel.GoodsinfoID);
                    Hi.Model.BD_Goods goodsmodel = bll_goods.GetModel(goodsinfomodel.GoodsID);
                    return new ResultEdit() { Result = "F", Description = "" + goodsmodel.GoodsName + "" + goodsinfomodel.ValueInfo + "的发货数量大于未发货数量" };
                }
                decimal outnum = orderdeModel.OutNum - orderoutdeModel.OutNum + decimal.Parse(orderoutdetjson["OutNum"].ToString());
                orderdeModel.OutNum = outnum;
                //如果修改后的发货数量等于商品数量，需要将订单子表的发货状态改已完成发货
                if (outnum == orderdeModel.GoodsNum)
                {
                    orderdeModel.IsOut = 1;
                }
                orderdeModel.ts = DateTime.Now;
                orderdeModel.modifyuser = Int32.Parse(UserID);
                list_orderdeupdate.Add(orderdeModel);
                //修改发货单明细表
                orderoutdeModel.OutNum = decimal.Parse(orderoutdetjson["OutNum"].ToString());
                orderoutdeModel.ts = DateTime.Now;
                orderoutdeModel.modifyuser = Int32.Parse(UserID);
                list_orderoutdeupdate.Add(orderoutdeModel);
            }
            #endregion
            //修改订单主表
            decimal num_all = 0;
            decimal num_send = 0;
            foreach (Hi.Model.DIS_OrderDetail orderdeModel in list_orderdeModel)
            {
                num_all += orderdeModel.GoodsNum + decimal.Parse(ClsSystem.gnvl(orderdeModel.ProNum, "0"));
                num_send += orderdeModel.OutNum;
            }
            //订单所有商品数量加上赠送数量等于所有订单明细的发货数量，则订单已经全部发货
            if (num_all == num_send)
                orderModel.IsOutState = (int)Enums.IsOutState.全部发货;
            orderModel.ts = DateTime.Now;
            orderModel.modifyuser = Int32.Parse(UserID);
            if (new Common().GetOutUpOrder(orderModel, orderoutModel, list_orderdeupdate, list_orderoutdeupdate) <= 0)
                return new ResultEdit() { Result = "F", Description = "提交失败" };
            Common.AddSysBusinessLog(orderModel, one, "Order", orderModel.ID.ToString(), "发货单修改", "");
            return new ResultEdit() { Result = "T",Description = "提交成功"};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditOrderOut:" + JSon);
            return new ResultEdit() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 修改订单时获取商品价格
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultProductPrice GetProductPrice(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        string OrderID = string.Empty;
        string Rebate = string.Empty;
        string PostFee = string.Empty;
        string Type = string.Empty;
        JsonData orderdetailjson;
        Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();
        Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
        Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
        List<class_ver3.OrderDetail> list_orderdetail = new List<class_ver3.OrderDetail>();
        List<class_ver3.OrderDetail> list_result = new List<class_ver3.OrderDetail>();
        #region//JSon取值
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["ResellerID"].ToString().Trim() == "" || JInfo["Rebate"].ToString().Trim()==""
                || JInfo["OrderID"].ToString().Trim() == "" || JInfo["PostFee"].ToString().Trim() == "" || JInfo["Type"].ToString().Trim()=="")
            {
                return new ResultProductPrice() { Result = "F", Description = "参数异常" };
            }
            else
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                DisID = JInfo["ResellerID"].ToString();
                OrderID = JInfo["OrderID"].ToString();
                Rebate = JInfo["Rebate"].ToString();
                PostFee = JInfo["PostFee"].ToString();
                Type = JInfo["Type"].ToString();
                orderdetailjson = JInfo["OrderDetailList"];
            }
            //判断登录信息是否正确
            Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
            Hi.Model.BD_Distributor dis = new Hi.Model.BD_Distributor();
            //判断经销商是否存在
            dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(DisID == "" ? "0" : DisID));
            if (dis == null || dis.AuditState == 0)
                return new ResultProductPrice() { Result = "F", Description = "经销商信息异常" };
            if (CompID == "")//如果核心企业ID是空的话，表示经销商修改订单，判断经销商登录信息是否正确
            {
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, 0, int.Parse(DisID == "" ? "0" : DisID)))
                    return new ResultProductPrice() { Result = "F", Description = "登录信息异常" };
                if (dis.dr == 1 || dis.IsEnabled == 0)
                    return new ResultProductPrice() { Result = "F", Description = "经销商信息异常" };
                //根据经销商id获取核心企业实体
                comp = new Hi.BLL.BD_Company().GetModel(dis.CompID);
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultProductPrice() { Result = "F", Description = "此经销商对应的核心企业信息异常" };
            }
            else//表示核心企业修改订单，判断核心企业登录信息是否异常
            {
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                {
                    return new ResultProductPrice() { Result = "F", Description = "登录信息异常" };
                }
                //判断核心企业信息是否异常
                comp = new Hi.BLL.BD_Company().GetModel(int.Parse(CompID == "" ? "0" : CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultProductPrice() { Result = "F", Description = "核心企业信息异常" };
            }
            List<class_ver3.OrderDetail> list_newprice = new List<class_ver3.OrderDetail>();
            //如果Type为1表示需要获取新价格
            if (Type == "1")
            {
                foreach (JsonData Jlist in orderdetailjson)
                {
                    
                    if (Jlist["OrderDetailID"].ToString() != "")
                    {
                        Hi.Model.BD_GoodsInfo goodsinfo_price = bll_goodsinfo.GetModel(Int32.Parse(Jlist["SKUID"].ToString()));
                        Hi.Model.DIS_OrderDetail orderdetailmodel = bll_orderdetail.GetModel(Int32.Parse(Jlist["OrderDetailID"].ToString()));
                        decimal Price = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), dis.ID, goodsinfo_price.ID);
                        if (Price != orderdetailmodel.GoodsPrice)
                        {
                            class_ver3.OrderDetail newpricemodel = new class_ver3.OrderDetail();
                            newpricemodel.OrderDetailID = Jlist["OrderDetailID"].ToString();
                            newpricemodel.SKUID = Jlist["SKUID"].ToString();
                            newpricemodel.NewPrice = Price.ToString("0.00");
                            list_newprice.Add(newpricemodel);
                        }

                    }
                }
                if (list_newprice != null && list_newprice.Count > 0)
                {
                    return new ResultProductPrice() { Result="JGBD",Description ="商品价格发生变动",OrderDetailList=list_newprice};
                }
            }

            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            string isint = Common.rdoOrderAudit("订单下单数量是否取整", comp.ID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
        #endregion
            List<Hi.Model.DIS_OrderDetail> list_orderdetailmodel = bll_orderdetail.GetList("", "OrderID=" +OrderID+ " and ISNULL(dr,0)=0 ", "");
            decimal sumamount = 0;
            foreach (JsonData Jlist in orderdetailjson)
            {
                #region//判断商品是否可售
                Hi.Model.BD_GoodsInfo goodsinfo = bll_goodsinfo.GetModel(Int32.Parse(Jlist["SKUID"].ToString()));
                Hi.Model.BD_Goods goodsmodel = bll_goods.GetModel(goodsinfo.GoodsID);
                //判断这个商品有没被下架删除
                if (goodsinfo == null || goodsinfo.dr == 1 || !goodsinfo.IsEnabled || goodsinfo.IsOffline == 0)
                {
                    return new ResultProductPrice() { Result = "F", Description = "" + goodsmodel.GoodsName + "" + goodsinfo.ValueInfo + "不可购买" };
                }
                //判断商品的可销售区域
                List<Common.GoodsID> list = Common.DisEnAreaGoodsID(dis.ID.ToString(), comp.ID.ToString());
                Common.GoodsID common_goodsid = new Common.GoodsID();
                common_goodsid.goodsID = goodsmodel.ID.ToString();
                if (list.Contains(common_goodsid))
                {
                    return new ResultProductPrice() { Result = "F", Description = "" + goodsmodel.GoodsName + "" + goodsinfo.ValueInfo + "不可购买" };
                }
                #endregion
   
                #region//判断商品是否正确（库存，商品数量取整）
                //商品数量取整的时候判断商品数量是不是整数(就算下单的时候保留两位小数，但修改时要取整，修改后保存的商品数量也要取整，所以就算没修改数量的商品也要判断)
                if (isint == "0")
                {
                    decimal num_deci = decimal.Parse(Jlist["Num"].ToString());
                    int num_int = (int)num_deci;
                    int isint_out;
                    if (!int.TryParse(Jlist["Num"].ToString(), out isint_out))
                    {
                        if (decimal.Parse(num_int.ToString()) != num_deci)
                        {
                            return new ResultProductPrice() { Result = "F", Description = "商品数量应为整数" };
                        }
                    }
                }
                        string pty_out, ppty_out, pid_num_out;
                        decimal ProNum_out = Common.GetProNum(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(), goodsinfo.CompID, decimal.Parse(Jlist["Num"].ToString()), out pty_out, out ppty_out, out pid_num_out);
                //orderdetailid传的话就表示不是新增的商品
                        if (Jlist["OrderDetailID"].ToString() != "")
                        {
                            Hi.Model.DIS_OrderDetail orderdetailmodel1 = list_orderdetailmodel.Find(p => p.GoodsinfoID == Int32.Parse(Jlist["SKUID"].ToString()));
                            if (orderdetailmodel1 == null)
                            {
                                return new ResultProductPrice() { Result = "F", Description = "参数异常" };
                            }
                            //判断商品数量有没改变
                            if (decimal.Parse(Jlist["Num"].ToString()) != orderdetailmodel1.GoodsNum)
                            {
                                //启用库存，判断库存是否足够（现在商品库存等于goodsinfo表的库存加上下单数量）
                                if (IsInve == 0)
                                {
                                    decimal inv = goodsinfo.Inventory + orderdetailmodel1.GoodsNum;//可用库存
                                    if (decimal.Parse(Jlist["Num"].ToString()) + ProNum_out > inv)
                                    {
                                        class_ver3.OrderDetail orderdetailresult = new class_ver3.OrderDetail();
                                        orderdetailresult.SKUID = Jlist["SKUID"].ToString();
                                        if (pty_out != "3")
                                        {
                                            orderdetailresult.NumEnable = inv.ToString("0.00");
                                        }
                                        else
                                        {
                                            //最大可购买数的算法
                                            decimal sumenable = 0;
                                            string[] discount_price = ppty_out.Split(',');
                                            //取出满多少件减
                                            decimal discount = decimal.Parse(discount_price[3]);
                                            //取出减多少件
                                            decimal goodprice_pro = decimal.Parse(discount_price[2]);
                                            //取出这种满减最多买多少组
                                            int group = (int)(inv / (discount + goodprice_pro));
                                            //算出买最多组时，需要发货的商品数量
                                            decimal group_num = group * (discount + goodprice_pro);
                                            //买了最多组后，剩余的库存
                                            decimal lift_num = inv - group_num;
                                            //能购买的商品最大数量（不算赠送的商品）
                                            if (lift_num != discount)
                                                sumenable = group * discount + lift_num;
                                            else
                                                sumenable = group * discount + lift_num - 1;

                                            orderdetailresult.NumEnable = sumenable.ToString("0.00");
                                        }
                                        list_orderdetail.Add(orderdetailresult);
                                        //return new ResultProductPrice() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                                    }
                                }

                            }
                        }
                        else
                        {
                            //判断库存够不够
                            if (IsInve == 0)
                            {
                                if (decimal.Parse(Jlist["Num"].ToString()) + ProNum_out > goodsinfo.Inventory)
                                {
                                    class_ver3.OrderDetail orderdetailresult = new class_ver3.OrderDetail();
                                    orderdetailresult.SKUID = Jlist["SKUID"].ToString();
                                    if (pty_out != "3")
                                    {
                                        orderdetailresult.NumEnable = goodsinfo.Inventory.ToString("0.00");
                                    }
                                    else
                                    {
                                        //最大可购买数的算法
                                        decimal sumenable = 0;
                                        string[] discount_price = ppty_out.Split(',');
                                        //取出满多少件减
                                        decimal discount = decimal.Parse(discount_price[3]);
                                        //取出减多少件
                                        decimal goodprice_pro = decimal.Parse(discount_price[2]);
                                        //取出这种满减最多买多少组
                                        int group = (int)(goodsinfo.Inventory / (discount + goodprice_pro));
                                        //算出买最多组时，需要发货的商品数量
                                        decimal group_num = group * (discount + goodprice_pro);
                                        //买了最多组后，剩余的库存
                                        decimal lift_num = goodsinfo.Inventory - group_num;
                                        //能购买的商品最大数量（不算赠送的商品）
                                        if (lift_num != discount)
                                            sumenable = group * discount + lift_num;
                                        else
                                            sumenable = group * discount + lift_num - 1;

                                        orderdetailresult.NumEnable = sumenable.ToString("0.00");
                                    }
                                    list_orderdetail.Add(orderdetailresult);
                                    //return new ResultProductPrice() { Result = "F", Description = "商品" + Jlist["SKUName"] + "库存不足" };
                                }
                            }
                        }
                
                #endregion
                        sumamount += decimal.Parse(Jlist["Num"].ToString()) * decimal.Parse(Jlist["TinkerPrice"].ToString());
                #region//获取每个商品的价格
                        //if (list_orderdetail == null && list_orderdetail.Count < 0)
                        //{
                        //    int ProID = 0;//促销ID
                        //    //获取促销ID
                        //    decimal price = Common.GetProPrice(goodsinfo.GoodsID.ToString(), goodsinfo.ID.ToString(), goodsinfo.CompID.ToString(), out ProID);
                        //    string Price = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID), dis.ID, goodsinfo.ID).ToString("0.00");
                        //    //返回信息
                        //    class_ver3.OrderDetail resultorderde = new class_ver3.OrderDetail();
                        //    resultorderde.ProductID = goodsmodel.ID.ToString();
                        //    resultorderde.SKUID = goodsinfo.ID.ToString();
                        //    resultorderde.IsPro = ProID == 0 ? "0" : "1";
                        //    resultorderde.ProductName = goodsmodel.GoodsName;
                        //    //拼接sku名称，商品名称加上属性值名称
                        //    string SKUName = goodsmodel.GoodsName;

                        //    List<Hi.Model.BD_GoodsAttrs> list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsmodel.ID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        //    if (list_attrs != null && list_attrs.Count != 0)
                        //    {
                        //        foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                        //        {
                        //            SKUName += attr.AttrsName;
                        //        }
                        //    }
                        //    resultorderde.SKUName = SKUName;
                        //    resultorderde.ValueInfo = goodsinfo.ValueInfo;
                        //    resultorderde.TinkerPrice = Price;
                        //    resultorderde.Inventory = goodsinfo.Inventory.ToString("0.00");
                        //    resultorderde.Num = Jlist["Num"].ToString();
                        //    resultorderde.Unit = goodsmodel.Unit;
                        //    resultorderde.BarCode = goodsinfo.BarCode;
                        //    resultorderde.ProNum = ProNum_out.ToString("0.00");
                        //    #region //取商品的促销详情
                        //    if (ProID != 0)
                        //    {
                        //        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                        //        if (pro != null)
                        //        {
                        //            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("",
                        //                " ProID=" + pro.ID + " and GoodInfoID =" + goodsinfo.ID + " and dr=0", "");
                        //            string info = string.Empty;
                        //            if (dList != null && dList.Count > 0)
                        //            {
                        //                if (pro.Type == 0 && pro.ProType == 1)
                        //                {
                        //                    info = "赠品";
                        //                }
                        //                else if (pro.Type == 0 && pro.ProType == 2)
                        //                {
                        //                    info = "商品优惠价" + dList[0].GoodsPrice.ToString("0.00");
                        //                }
                        //                else if (pro.Type == 1 && pro.ProType == 3)
                        //                {
                        //                    info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" + dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;

                        //                }
                        //                else if (pro.Type == 1 && pro.ProType == 4)
                        //                {
                        //                    info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                        //                }
                        //            }
                        //            //商品促销信息
                        //            resultorderde.proInfo = new class_ver3.PromotionInfo()
                        //            {
                        //                ProID = ProID.ToString(),
                        //                ProTitle = pro.ProTitle,
                        //                ProInfos = info,
                        //                Type = pro.Type.ToString(),
                        //                ProType = pro.ProType.ToString(),
                        //                Discount = pro.Discount.ToString("0.00"),
                        //                ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                        //                ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                        //            };
                        //        }
                        //    }
                        //    #endregion
                        //    //取出商品的图片，一个商品有多张图片
                        //    List<class_ver3.Pic> Pic = new List<class_ver3.Pic>();
                        //    if (goodsmodel.Pic != "" && goodsmodel.Pic != "X")
                        //    {
                        //        class_ver3.Pic pic = new class_ver3.Pic();
                        //        pic.ProductID = goodsinfo.GoodsID.ToString();
                        //        pic.IsDeafult = "1";
                        //        pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                        //                     goodsmodel.Pic;
                        //        Pic.Add(pic);
                        //    }
                        //    resultorderde.ProductPicUrlList = Pic;
                        //    list_result.Add(resultorderde);
                        //}
                #endregion
            }
            if (list_orderdetail != null && list_orderdetail.Count > 0)
                return new ResultProductPrice() { Result ="INV",Description ="库存不足",OrderDetailList = list_orderdetail};
            //成功的返回实体
            ResultProductPrice result = new ResultProductPrice();
            result.Result = "T";
            result.Description = "返回成功";
            result.TotalAmount = sumamount.ToString("0.00");
            //获取整单促销的促销金额
            string pid = "0";
            string ProIDD = "";
            string protpye = "";
            decimal pprice = Common.GetProPrice(sumamount, out pid, out ProIDD, out protpye, comp.ID);
            //付款金额等于商品金额-促销金额-返利+运费
            result.AuditTotalAmount = (sumamount - pprice - decimal.Parse(Rebate == "" ? "0" : Rebate) + decimal.Parse(PostFee == "" ? "0" : PostFee)).ToString("0.00");
            result.Reate = Rebate;
            result.PostFee = PostFee;
            if (pid == "0")
                result.IsOrderPro = "0";
            else
            {
                result.IsOrderPro = "1";
                class_ver3.OrderPro orderpro = new class_ver3.OrderPro();
                orderpro.ProID = pid;
                orderpro.OrderPrice = pprice.ToString("0.00");
                string  strsql = "select protype from BD_Promotion where ID = " + pid+ "";
                orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                //根据促销明细ID，取出促销明细实体
                Hi.Model.BD_PromotionDetail2 model_prodetail2 = new Hi.BLL.BD_PromotionDetail2().GetModel(Int32.Parse(ProIDD));
                string ProType = "";
                if (orderpro.ProType == "5")//表示满减
                    ProType = "5," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                else if (orderpro.ProType == "6")//表示满折
                    ProType = "6," + model_prodetail2.OrderPrice + "," + model_prodetail2.Discount + "";
                else
                    return new ResultProductPrice() { Result = "F", Description = "订单促销异常" };
                orderpro.Discount = Common.proOrderType(ProIDD, pprice.ToString("0.00"), ProType);
                result.ProInfo = orderpro;
            }
            return result; 
            
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetProductPrice:" + JSon);
            return new ResultProductPrice() { Result = "F", Description = "参数异常" };
        }

    }

    /// <summary>
    /// 核心企业订单获取发货信息列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultGetOrderOut GetOrderSendGoodsInfo(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string OrderID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["OrderID"].ToString().Trim() == "")
            {
                return new ResultGetOrderOut() { Result = "F", Description = "参数异常" };
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            OrderID = JInfo["OrderID"].ToString();
            #endregion

            #region//判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultGetOrderOut() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultGetOrderOut() { Result = "F", Description = "核心企业信息异常" };

            Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(Int32.Parse(OrderID));
            if (ordermodel == null || ordermodel.dr == 1)
                return new ResultGetOrderOut() { Result = "F", Description = "订单信息异常" };

            #endregion
            //返回的参数
            ResultGetOrderOut result = new ResultGetOrderOut();
            result.Result = "T";
            result.Description = "返回成功";
            List<Hi.Model.DIS_OrderOut> list_orderout = new Hi.BLL.DIS_OrderOut().GetList("",
                    " OrderID=" + ordermodel.ID + " and CompID=" + ordermodel.CompID + " and DisID=" +
                    ordermodel.DisID + " and ISNULL(dr,0)=0", "SendDate desc");
            Hi.BLL.DIS_OrderDetail bll_orderdetail = new Hi.BLL.DIS_OrderDetail();
            Hi.BLL.BD_GoodsInfo bll_goodsinfo = new Hi.BLL.BD_GoodsInfo();
            Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
            Hi.Model.BD_GoodsInfo goodsInfo_orderoutd = null;
            Hi.Model.BD_Goods goods_orderoutd = null;
            List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
            List<class_ver3.Pic> Pic = null;
            string SKUname_orderoutd = string.Empty;
            #region//获取返回的返货单
            if (list_orderout != null && list_orderout.Count > 0)
            {
                //返回的返货单列表
                List<Hi.Model.DIS_OrderDetail> list_orderoutdetailinfo = null;//发货单明细对应的订单的明细信息
                List<Hi.Model.DIS_Logistics> exlist = new List<Hi.Model.DIS_Logistics>();//获取的物流信息list
                List<class_ver3.OrderOut> list_orderoutresult = new List<class_ver3.OrderOut>();
                class_ver3.OrderOut orderOut = null;
                List<class_ver3.SendOutDetail> list_sendoutdetail = null;
                List<Hi.Model.DIS_OrderOutDetail> list_orderoutlist = null;
                class_ver3.SendOutDetail sendoutdetail = null;
                class_ver3.OrderOutDetailInfo orderoutdetailinfo = null;
                class_ver3.Wuliu Logistics = null;
                //循环数据库查出发货单数据，赋值给返回的发货单list
                foreach (Hi.Model.DIS_OrderOut Out in list_orderout)
                {
                    int goodsnum = 0;//发货单明细条数
                    orderOut = new class_ver3.OrderOut();//返回的返货单信息
                    list_sendoutdetail = new List<class_ver3.SendOutDetail>();
                    orderOut.OrderID = ordermodel.ID.ToString();
                    orderOut.OrderOutID = Out.ID.ToString();
                    orderOut.SendDate = Out.SendDate.ToString("yyyy-MM-dd");
                    orderOut.ActionUser = Out.ActionUser;
                    orderOut.Remark = Out.Remark;
                    orderOut.ts = Out.ts.ToString();
                    orderOut.ReceiptNo = ordermodel.ReceiptNo;
                    orderOut.IsAudit = Out.IsAudit.ToString();
                    orderOut.CreateUserID = Out.CreateUserID.ToString();
                    orderOut.CreateDate = Out.CreateDate.ToString("yyyy-MM-dd");
                    orderOut.IsSign = ClsSystem.gnvl(Out.IsSign, "0");
                    orderOut.OrderOutNo = Out.ReceiptNo;
                    if (ClsSystem.gnvl(orderOut.SignDate, "") != "" && ClsSystem.gnvl(orderOut.SignDate, "") != "0001/1/1 0:00:00")
                        orderOut.SignDate = Out.SignDate.ToString("yyyy-MM-dd");
                    orderOut.SignUserId = ClsSystem.gnvl(Out.SignUserId, "");
                    orderOut.SignUser = ClsSystem.gnvl(Out.SignUser, "");
                    orderOut.SignRemark = ClsSystem.gnvl(Out.SignRemark, "");
                    //取出此发货单对应的发货单明细表list
                    list_orderoutlist = new Hi.BLL.DIS_OrderOutDetail().GetList("",
                        "OrderOutID= " + Out.ID + " and isnull(dr,0) = 0 ", "");
                    //循环发货单明细表list，获取发货单明细
                    foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in list_orderoutlist)
                    {
                        sendoutdetail = new class_ver3.SendOutDetail();//返回的发货单子表信息
                        sendoutdetail.OrderOutID = Out.ID.ToString();
                        sendoutdetail.SKUID = orderoutdetail.GoodsinfoID.ToString();
                        sendoutdetail.OrderID = ordermodel.ID.ToString();
                        sendoutdetail.OutNum = decimal.Parse(ClsSystem.gnvl(orderoutdetail.OutNum, "0")).ToString("0.00");
                        sendoutdetail.SignNum = decimal.Parse(ClsSystem.gnvl(orderoutdetail.SignNum, "0")).ToString("0.00");
                        sendoutdetail.Remark = ClsSystem.gnvl(orderoutdetail.Remark, "");
                        //取出发货单子表对应的订单明细list（list里只有一条数据）
                        list_orderoutdetailinfo = bll_orderdetail.GetList("", "OrderID = " + ordermodel.ID + " and GoodsinfoID = " + orderoutdetail.GoodsinfoID + " and isnull(dr,0)=0", "");
                        if (list_orderoutdetailinfo != null && list_orderoutdetailinfo.Count > 0)
                        {
                            orderoutdetailinfo = new class_ver3.OrderOutDetailInfo();
                            orderoutdetailinfo.SKUID = orderoutdetail.GoodsinfoID.ToString();
                            //获取goodsinfo实体
                            goodsInfo_orderoutd = bll_goodsinfo.GetModel(orderoutdetail.GoodsinfoID);
                            if (goodsInfo_orderoutd == null)
                                return new ResultGetOrderOut() { Result = "F", Description = "SKU信息异常" };
                            orderoutdetailinfo.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            //获取goods实体
                            goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                            if (goods_orderoutd == null)
                                return new ResultGetOrderOut() { Result = "F", Description = "商品异常" };
                            orderoutdetailinfo.ProductName = goods_orderoutd.GoodsName;

                            SKUname_orderoutd = goods_orderoutd.GoodsName;
                            list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                            if (list_attrs != null && list_attrs.Count != 0)
                            {
                                foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                                {
                                    SKUname_orderoutd += attr.AttrsName;
                                }
                            }

                            orderoutdetailinfo.SKUName = SKUname_orderoutd;
                            orderoutdetailinfo.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                            orderoutdetailinfo.ProductCode = goodsInfo_orderoutd.BarCode;
                            orderoutdetailinfo.TinkerPrice = list_orderoutdetailinfo[0].AuditAmount.ToString("0.00");
                            orderoutdetailinfo.Unit = goods_orderoutd.Unit;
                            sendoutdetail.AllNum = (list_orderoutdetailinfo[0].GoodsNum + decimal.Parse(ClsSystem.gnvl(list_orderoutdetailinfo[0].ProNum, "0"))).ToString("0.00");
                            //首先判断这条发货明细对应的订单明细中的促销满赠数量是否为0
                            if (decimal.Parse(list_orderoutdetailinfo[0].ProNum) == 0)
                                sendoutdetail.ProInfo = "";
                            else//不为0表示有满赠
                            {
                                string[] a = list_orderoutdetailinfo[0].Protype.Split(',');
                                sendoutdetail.ProInfo = decimal.Parse(a[3]).ToString("0.00") + "赠" + decimal.Parse(a[2]).ToString("0.00");
                            }

                            //取出商品的图片，一个商品有多张图片
                            Pic = new List<class_ver3.Pic>();
                            if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                            {
                                class_ver3.Pic pic = new class_ver3.Pic();
                                pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                                pic.IsDeafult = "1";
                                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                             goods_orderoutd.Pic;
                                Pic.Add(pic);
                            }
                            orderoutdetailinfo.ProductPicUrlList = Pic;
                            sendoutdetail.OrderOutDetailInfo = orderoutdetailinfo;
                        }
                        list_sendoutdetail.Add(sendoutdetail);
                        goodsnum++;//循环一次发货单明细数加一
                    }
                    orderOut.SendOutDetailList = list_sendoutdetail;
                    //查出此发货单对应的物流信息
                    Logistics = new class_ver3.Wuliu();//返回的物流信息
                    Logistics.OrderID = ordermodel.ID.ToString();
                    Logistics.OrderOutID = Out.ID.ToString();
                    Logistics.GoodsNum = goodsnum.ToString();
                    exlist = Common.GetExpress(Out.ID.ToString());//根据返货单ID取出对应的物流实体的list
                    if (exlist != null)
                    {
                        Logistics.ComPName = ClsSystem.gnvl(exlist[0].ComPName, "");
                        Logistics.LogisticsNo = ClsSystem.gnvl(exlist[0].LogisticsNo, "");
                        Logistics.CarUser = ClsSystem.gnvl(exlist[0].CarUser, "");
                        Logistics.CarNo = ClsSystem.gnvl(exlist[0].CarNo, "");//司机手机号
                        Logistics.Car = ClsSystem.gnvl(exlist[0].Car, "");//车牌号
                        Logistics.Type = exlist[0].Type.ToString();
                        if (exlist[0].Context.IndexOf("context") >= 0 || exlist[0].Context.IndexOf("content") >= 0)
                        {

                            Logistics.Context = exlist[0].Context;
                        }
                    }
                    orderOut.Logistics = Logistics;
                    list_orderoutresult.Add(orderOut);
                }
                result.OrderOutList = list_orderoutresult;
            }
            #endregion
            #region//未发货商品信息
            List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                   " OrderID=" + ordermodel.ID + " and DisID=" + ordermodel.DisID + " and ISNULL(dr,0)=0", "");
            if (detailList == null)
                return new ResultGetOrderOut() { Result = "F", Description = "订单明细异常" };
            class_ver3.UnSendoutDetail unsend = null;
            class_ver3.OrderOutDetailInfo unsendoutdetail = null;
            List<class_ver3.Pic> Pic_unsend = null;
            List<class_ver3.UnSendoutDetail> unsend_list = new List<class_ver3.UnSendoutDetail>();//返回的未发货明细的list
            List<Hi.Model.DIS_OrderDetail> list_unsendoutdetailinfo = null;//未发货商品对应的订单明细的信息
            string SKUName_unsendout = string.Empty;
            int unsendnum = 0;
            foreach (Hi.Model.DIS_OrderDetail detail in detailList)
            {
                if (detail.GoodsNum - ClsSystem.gnvl(detail.OutNum, "0").ToDecimal() > 0)
                {
                    unsend = new class_ver3.UnSendoutDetail();//返回的未发货明细实体
                    unsend.SKUID = detail.GoodsinfoID.ToString();
                    unsend.OrderID = ordermodel.ID.ToString();
                    unsend.AllNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl(detail.ProNum, "0"))).ToString("0.00");
                    unsend.OutNum = ClsSystem.gnvl(detail.OutNum, "0").ToDecimal().ToString("0.00");
                    unsend.UnOutNum = (detail.GoodsNum + decimal.Parse(ClsSystem.gnvl(detail.ProNum, "0")) - ClsSystem.gnvl(detail.OutNum, "0").ToDecimal()).ToString("0.00");
                    //取出此条未发货的商品对应的商品明细list（list只有一条数据）
                    list_unsendoutdetailinfo = bll_orderdetail.GetList("", "OrderID = " + ordermodel.ID + " and GoodsinfoID = " + detail.GoodsinfoID + " and isnull(dr,0)=0", "");
                    if (list_unsendoutdetailinfo != null && list_unsendoutdetailinfo.Count > 0)
                    {
                        unsendoutdetail = new class_ver3.OrderOutDetailInfo();
                        unsendoutdetail.SKUID = detail.GoodsinfoID.ToString();
                        //获取goodsinfo实体
                        goodsInfo_orderoutd = bll_goodsinfo.GetModel(detail.GoodsinfoID);
                        if (goodsInfo_orderoutd == null)
                            return new ResultGetOrderOut() { Result = "F", Description = "SKU信息异常" };
                        unsendoutdetail.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                        //获取goods实体
                        goods_orderoutd = bll_goods.GetModel(goodsInfo_orderoutd.GoodsID);
                        if (goods_orderoutd == null)
                            return new ResultGetOrderOut() { Result = "F", Description = "商品异常" };
                        unsendoutdetail.ProductName = goods_orderoutd.GoodsName;
                        SKUName_unsendout = goods_orderoutd.GoodsName;
                        list_attrs = new Hi.BLL.BD_GoodsAttrs().GetList("AttrsName", "GoodsID = " + goodsInfo_orderoutd.GoodsID + " and CompID = " + comp.ID + " and ISNULL(dr,0) = 0", "");
                        if (list_attrs != null && list_attrs.Count != 0)
                        {
                            foreach (Hi.Model.BD_GoodsAttrs attr in list_attrs)
                            {
                                SKUName_unsendout += attr.AttrsName;
                            }
                        }
                        unsendoutdetail.SKUName = SKUName_unsendout;
                        unsendoutdetail.ValueInfo = goodsInfo_orderoutd.ValueInfo;
                        unsendoutdetail.TinkerPrice = list_unsendoutdetailinfo[0].AuditAmount.ToString("0.00");
                        unsendoutdetail.Unit = goods_orderoutd.Unit;

                        //取出商品的图片，一个商品有多张图片
                        Pic_unsend = new List<class_ver3.Pic>();
                        if (goods_orderoutd.Pic != "" && goods_orderoutd.Pic != "X")
                        {
                            class_ver3.Pic pic = new class_ver3.Pic();
                            pic.ProductID = goodsInfo_orderoutd.GoodsID.ToString();
                            pic.IsDeafult = "1";
                            pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                         goods_orderoutd.Pic;
                            Pic_unsend.Add(pic);
                        }
                        unsendoutdetail.ProductPicUrlList = Pic_unsend;
                        unsendoutdetail.ProductCode = goodsInfo_orderoutd.BarCode;
                        unsend.OrderOutDetailInfo = unsendoutdetail;

                    }
                    unsend_list.Add(unsend);
                    unsendnum++;
                }
            }
            #endregion
            result.UnSendoutDetailList = unsend_list;
            return result;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetOrderSendGoodsInfo:" + JSon);
            return new ResultGetOrderOut() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 发货单作废
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEdit CancelOrderOut(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string OrderOutID = string.Empty;
        string ts = string.Empty;
        string OrderID = string.Empty;
        try
        {
            #region//json取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["OrderOutID"].ToString().Trim() == ""
                || JInfo["ts"].ToString().Trim() == "" || JInfo["OrderID"].ToString().Trim() == "")
            {
                return new ResultEdit() { Result = "F", Description = "参数异常" };
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            OrderOutID = JInfo["OrderOutID"].ToString();
            ts = JInfo["ts"].ToString();
            OrderID = JInfo["OrderID"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            //判断发货单是否异常
            Hi.Model.DIS_OrderOut orderout = new Hi.BLL.DIS_OrderOut().GetModel(Int32.Parse(OrderOutID));
            if (orderout == null || orderout.IsAudit == 3)
                return new ResultEdit() { Result = "F", Description = "发货单信息异常" };
            if (DateTime.Parse(ts).ToString("yyyy/MM/dd HH:mm:dd") != orderout.ts.ToString("yyyy/MM/dd HH:mm:dd"))
                return new ResultEdit() { Result = "F", Description = "发货单已被操作，请稍后再试" };
            if (orderout.dr == 1)
                return new ResultEdit() { Result = "F", Description = "发货单已被删除" };
            if (orderout.IsSign == 1)
                return new ResultEdit() { Result = "F", Description = "发货单已被签收" };
            //判断订单是否异常
            Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(Int32.Parse(OrderID));
            if (order == null || order.OState != (int)Enums.OrderState.已发货)
                return new ResultEdit() { Result = "F", Description = "订单状态异常" };
            if (order.dr == 1)
                return new ResultEdit() { Result = "F", Description = "订单已被删除" };

            #endregion
            //查询出订单明细，及发货单明细
            List<Hi.Model.DIS_OrderDetail> list_orderdetail = new Hi.BLL.DIS_OrderDetail().GetList("", " isnull(dr,0)=0 and OrderID=" + OrderID + " ", "");
            List<Hi.Model.DIS_OrderOutDetail> list_orderoutdetail = new Hi.BLL.DIS_OrderOutDetail().GetList("", " isnull(dr,0)=0 and OrderOutID=" + OrderOutID + " ", "");
            if (list_orderdetail == null || list_orderdetail.Count <= 0)
            {
                return new ResultEdit() { Result = "F", Description = "订单商品异常" };
            }
            if (list_orderoutdetail == null || list_orderoutdetail.Count <= 0)
            {
                return new ResultEdit() { Result = "F", Description = "发货单商品异常" };
            }
            //循环发货单明细，返回订单明细的发货数量，并修改订单明细的发货状态
            List<Hi.Model.DIS_OrderDetail> list_updateorderdetail = new List<Hi.Model.DIS_OrderDetail>();
            Hi.Model.DIS_OrderDetail orderdetail = null;
            foreach (Hi.Model.DIS_OrderOutDetail orderoutdetail in list_orderoutdetail)
            {
                orderdetail = list_orderdetail.Find(p => p.GoodsinfoID == orderoutdetail.GoodsinfoID);
                orderdetail.OutNum = orderdetail.OutNum - orderoutdetail.OutNum;
                orderdetail.IsOut = 0;
                orderdetail.ts = DateTime.Now;
                orderdetail.modifyuser = one.ID;
                list_updateorderdetail.Add(orderdetail);

            }
            //修改订单状态，如果此发货单是唯一有效的发货单，则订单状态返回上一步
            List<Hi.Model.DIS_OrderOut> list_otherorderout = new Hi.BLL.DIS_OrderOut().GetList("", " OrderID = " + OrderID + " and isnull(dr,0)=0 and isnull(IsAudit,0)<>3 and ID<>" + OrderOutID + "", "");
            if (list_otherorderout == null || list_otherorderout.Count <= 0)
            {
                order.OState = (int)Enums.OrderState.已审;
                order.IsOutState = 0;
            }
            else if (order.IsOutState != 2)
            {
                order.IsOutState = (int)Enums.IsOutState.部分发货;
            }
            order.ts = DateTime.Now;
            order.modifyuser = one.ID;
            //修改发货单状态
            orderout.IsAudit = 3;
            orderout.ts = DateTime.Now;
            orderout.modifyuser = one.ID;
            //if (new Hi.BLL.DIS_OrderOut().GetCancelOut(order, orderout, list_updateorderdetail) > 0)
            //{
            //    Common.AddSysBusinessLog(order, one, "Order", OrderID, "发货单作废", "");
            //    return new ResultEdit() { Result = "T", Description = "作废成功" };
            //}
            //else
                return new ResultEdit() { Result = "F", Description = "作废失败" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CancelOrderOut:" + JSon);
            return new ResultEdit() { Result = "F", Description = "参数异常" };
        }
    }


    /// <summary>
    /// 订单作废
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEdit CancelOrder(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string OrderID = string.Empty;
        string ts = string.Empty;
        try
        {

            #region//JSON取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["OrderID"].ToString().Trim() == ""
                || JInfo["ts"].ToString().Trim() == "")
            {
                return new ResultEdit() { Result = "F", Description = "参数异常" };
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            OrderID = JInfo["OrderID"].ToString();
            ts = JInfo["ts"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(Int32.Parse(OrderID));
            if (order == null || order.OState == (int)Enums.OrderState.已作废)
                return new ResultEdit() { Result = "F",Description = "订单信息异常"};
            if (order.OState == (int)Enums.OrderState.已到货)
                return new ResultEdit() { Result = "F", Description = "订单已完成，不能作废" };
            if (order.dr == 1)
                return new ResultEdit() { Result = "F",Description = "订单已删除"};
            if (DateTime.Parse(ts) != order.ts)
                return new ResultEdit() { Result = "F",Description = "订单已被操作，请稍后再试"};
            #endregion
            //查询此订单是否已经发货，要是发货的需要将发货单一起作废
            List<Hi.Model.DIS_OrderOut> list_orderout = new Hi.BLL.DIS_OrderOut().GetList("", "OrderID=" + OrderID + " and isnull(dr,0)=0 and isnull(IsAudit,0)<>3", "");
            StringBuilder sql = new StringBuilder();
            if (list_orderout != null && list_orderout.Count > 0)
            {
                foreach (Hi.Model.DIS_OrderOut orderout in list_orderout)
                {
                    sql.Append(" update [DIS_OrderOut] set [IsAudit]=3,[ts]='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID=" + orderout.ID + ";");
                }
            }
            //作废订单
            sql.Append(" update [DIS_Order] set [OState]=" + (int)Enums.OrderState.已作废 + ",[ts]='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID=" + OrderID + ";");
            //判断是否启用库存，作废需要返回库存
            if (Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0) == 0)
                sql.Append(new Hi.BLL.DIS_Order().GetSqlAddInve(order.ID, null, 0));
            if (new Hi.BLL.DIS_Order().UpdateOrderState(sql.ToString()))
            {
                //作废订单，返回返利
                new Hi.BLL.BD_Rebate().TransCancel(order.DisID, order.ID, one.ID);
                //Common.AddSysBusinessLog(order.CompID, "Order", OrderID, "订单作废", "");
                Common.AddSysBusinessLog(order, one, "Order", OrderID, "订单作废", "");
                return new ResultEdit() { Result = "T",Description = "作废成功"};
            }
            else
                return new ResultEdit() { Result = "F",Description = "作废失败"};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CancelOrder:" + JSon);
            return new ResultEdit() { Result = "F", Description = "参数异常" };
        }
    }
    /// <summary>
    /// 附件上传
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEdit Unload(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        string StringImage = string.Empty;
        string FileName = string.Empty;
        string orderid  = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || (JInfo["CompID"].ToString().Trim() == "" && JInfo["ResellerID"].ToString().Trim() == "") ||
                JInfo["StringImage"].ToString().Trim() == "" || JInfo["FileName"].ToString().Trim() == "" || JInfo["OrderID"].ToString().Trim() == "")
            {
                return new ResultEdit() { Result = "F", Description = "参数异常" };
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            DisID = JInfo["ResellerID"].ToString();
            StringImage = JInfo["StringImage"].ToString();
            FileName = Common.NoHTML(JInfo["FileName"].ToString());
            orderid = JInfo["OrderID"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (CompID != "")//表示核心企业登录
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
                {
                    return new ResultEdit() { Result = "F", Description = "登录信息异常" };
                }
                //判断核心企业信息是否异常
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            }
            else//表示经销商登录
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, 0, Int32.Parse(DisID)))
                {
                    return new ResultEdit() { Result = "F", Description = "登录信息异常" };
                }
                //判断经销商信息是否异常
                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Int32.Parse(DisID));
                if (dis == null || dis.dr == 1 || dis.IsEnabled == 0 || dis.AuditState == 0)
                    return new ResultEdit() { Result = "F",Description ="经销商信息异常"};
            }
            #endregion
            string ext = FileName.Substring(FileName.LastIndexOf("."));
            if (!(ext.ToUpper() == ".PDF" || ext.ToUpper() == ".DOC" || ext.ToUpper() == ".XLS" || ext.ToUpper() == ".DOCX" || ext.ToUpper() == ".XLSX" ||
                ext.ToUpper() == ".TXT" || ext.ToUpper() == ".JPG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".BMP" || ext.ToUpper() == ".GIF" || ext.ToUpper() == ".RAR"
                || ext.ToUpper() == ".ZIP"))
            {
                return new ResultEdit() { Result = "F", Description = "附件格式不正确" };
            }
            string ImgFolder = "OrderFJ/";
            string name = Guid.NewGuid().ToString() + ext;
            string path = ConfigurationManager.AppSettings["ImgPath"].ToString().Trim() + ImgFolder;
            string viewPath = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + ImgFolder;
            //判断文件夹是否存在
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
            //保存在服务器的文件名
            string toFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + ext;
            string saveFile = path + FileName.Substring(0, FileName.LastIndexOf('.')) + "^^" + toFileName;
            byte[] b = Convert.FromBase64String(StringImage);
            FileStream fs = new FileStream(saveFile, FileMode.Create, FileAccess.Write);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();
            if (!new Common().Update(orderid, FileName.Substring(0, FileName.LastIndexOf('.')) + "^^" + toFileName, "edit", one))
            {
                return new ResultEdit() { Result = "F", Description = "附件上传失败" };
            }
            return new ResultEdit() { Result = "T",Description = "附件上传成功"};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "Unload:" + JSon);
            return new ResultEdit() { Result = "F", Description = "附件上传失败" };
        }
    }


    /// <summary>
    /// 修改物流信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEditLogistics EditLogistics(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string OrderOutID = string.Empty;
        string ComPName = string.Empty;
        string LogisticsNo = string.Empty;
        string CarUser = string.Empty;
        string CarNo = string.Empty;
        string Car = string.Empty;
        try
        {
            #region//JSon赋值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["OrderOutID"].ToString().Trim() == "")
            {
                return new ResultEditLogistics() { Result = "F",Description ="参数异常"};
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            OrderOutID = JInfo["OrderOutID"].ToString();
            ComPName = Common.NoHTML(JInfo["ComPName"].ToString());
            LogisticsNo = Common.NoHTML(JInfo["LogisticsNo"].ToString());
            CarUser = Common.NoHTML(JInfo["CarUser"].ToString());
            CarNo = Common.NoHTML(JInfo["CarNo"].ToString());
            Car = Common.NoHTML(JInfo["Car"].ToString());
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultEditLogistics() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultEditLogistics() { Result = "F", Description = "核心企业信息异常" };
            //判断发货单的状态是否正确
            Hi.Model.DIS_OrderOut orderout = new Hi.BLL.DIS_OrderOut().GetModel(Int32.Parse(OrderOutID));
            if (orderout == null)
                return new ResultEditLogistics() { Result = "F", Description = "发货单异常" };
            if (orderout.dr == 1)
                return new ResultEditLogistics() {Result = "F",Description ="发货单已被删除" };
            if (orderout.IsAudit == 3)
                return new ResultEditLogistics() { Result ="F",Description = "发货单已被作废"};
            #endregion
            //根据发货单ID获取对应的物流数据（发货单跟物流信息是一对一的）
            List<Hi.Model.DIS_Logistics> list_log = new Hi.BLL.DIS_Logistics().GetList("","Isnull(dr,0)=0 and OrderOutID=" + OrderOutID+"","");
            if (list_log == null || list_log.Count <= 0)
                return new ResultEditLogistics() { Result = "F",Description = "发货单不存在相应的物流"};
            Hi.Model.DIS_Logistics log = list_log[0];
            log.ComPName = ComPName;
            log.LogisticsNo = LogisticsNo;
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
            log.CarNo = CarNo;
            log.Car = Car;
            log.CarUser = CarUser;
            log.ts = DateTime.Now;
            log.modifyuser = Int32.Parse(UserID);
            if (!new Hi.BLL.DIS_Logistics().Update(log))
                return new ResultEditLogistics() { Result = "F",Description = "物流信息修改失败"};
            //返回信息
            class_ver3.Wuliu log_result = new class_ver3.Wuliu();
            log_result.OrderID = log.OrderID.ToString();
            log_result.OrderOutID = log.OrderOutID.ToString();
            log_result.ComPName = log.ComPName;
            log_result.LogisticsNo = log.LogisticsNo;
            log_result.CarUser = log.CarUser;
            log_result.CarNo = log.CarNo;
            log_result.Car = log.Car;
            log_result.Context = log.Context;
            log_result.Type = log.Type.ToString();
            return new ResultEditLogistics() { Result="T",Description = "修改物流信息成功",Logistics = log_result};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditLogistics:" + JSon);
            return new ResultEditLogistics() { Result = "F", Description = "物流信息修改失败" };
        }
    }

    /// <summary>
    /// 订单线下支付
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public OrderPay EditOrderPay(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string KeyID = string.Empty;//订单ID
        string DisID = string.Empty;//DisID
        string paymoney = string.Empty;//付款金额
        string bankname = string.Empty;//账户名称
        string bank = string.Empty;//收款银行
        string bankcode = string.Empty;//收款卡号
        string txtArriveDate = string.Empty;//支付日期
        string remark = string.Empty;//备 注
        string imgtype = string.Empty;  //图片上传类型  1、app   2、微信

        JsonData attachO;//上传附件
        string attach = string.Empty;//上传附件 

        int hid_type = 0;

        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "")
        {
            return new OrderPay() { Result = "F", Description = "参数异常" };
        }
        UserID = JInfo["UserID"].ToString();
        CompID = JInfo["CompID"].ToString();
        KeyID = JInfo["KeyID"].ToString();
        DisID = Common.NoHTML(JInfo["DisID"].ToString());
        paymoney = Common.NoHTML(JInfo["paymoney"].ToString());
        bankname = Common.NoHTML(JInfo["bankname"].ToString());
        bank = Common.NoHTML(JInfo["bank"].ToString());
        bankcode = Common.NoHTML(JInfo["bankcode"].ToString());
        txtArriveDate = Common.NoHTML(JInfo["texArriveDate"].ToString());
        remark = Common.NoHTML(JInfo["remark"].ToString());
        //attachO = Common.NoHTML(JInfo["attach"].ToString());
        attach = JInfo["attach"].ToString();
        imgtype = JInfo["imgtype"].ToString();

        //判断登录信息是否异常
        Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
        if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID),Int32.Parse(DisID)))
        {
            return new OrderPay() { Result = "F", Description = "登录信息异常" };
        }


        Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(KeyID);

        if (ordermodel.PayedAmount + Convert.ToDecimal(paymoney) > ordermodel.AuditAmount || ordermodel.PayedAmount == ordermodel.AuditAmount)
        {
            return new OrderPay() { Result = "F", Description = "付款失败，订单支付完成或支付金额大于订单未支付金额！" };
        }
        List<Hi.Model.DIS_OrderReturn> OrderReturn = new Hi.BLL.DIS_OrderReturn().GetList("", " CompID='" + ordermodel.CompID + "' and OrderID='" + ordermodel.ID + "'", "");
        if (ordermodel.OState == 3 || ordermodel.OState == 6 || ordermodel.OState == 7 || OrderReturn.Count > 0)
        {
            return new OrderPay() { Result = "F", Description = "付款失败，订单已申请退货,或已作废！" };
        }

        int order = 0;
        int pay = 0;
        SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
        con.Open();
        SqlTransaction sqlTrans = con.BeginTransaction();

        try
        {

            string guid = Guid.NewGuid().ToString().Replace("-", "");
            Hi.Model.PAY_Payment paymentmodel = new Hi.Model.PAY_Payment();
            paymentmodel.OrderID = ordermodel.ID;
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

            paymentmodel.CreateUserID = one.ID;
            if (hid_type == 1)
                paymentmodel.vdef9 = "1";

            if ("2".Equals(imgtype))
            {
                if (attach.Length > 0)
                {
                    string[] files = attach.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                    string df = string.Empty;

                    if (files.Length > 0)
                    {
                        WX wx = new WX();
                        foreach (var item in files)
                        {
                            string aa = wx.getJSImage(item);
                            df += df == "" ? aa : "&&" + aa;
                        }
                    }
                    paymentmodel.attach = df;
                }
            }
            else
            {
                paymentmodel.attach = attach;//附件  
            }

            //厂商直接就是成功

            int num = new Hi.BLL.PAY_Payment().Add(paymentmodel);
            if (num > 0)
            {
                //厂商直接支付成功，修改已支付金额
                if (hid_type == 1)
                {
                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, ordermodel.ID, Convert.ToDecimal(paymoney), sqlTrans);//修改订单状态
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
                return new OrderPay() { Result = "T", Description = "付款成功,等待厂商确认" };
            }
            else
            {
                return new OrderPay() { Result = "F", Description = "付款失败" };
            }
        }
        catch
        {
            return new OrderPay() { Result = "F", Description = "付款失败" };
        }
        finally
        {
            con.Close();
        }
        return new OrderPay() { Result = "T", Description = "付款成功" };
    }

    /// <summary>
    /// 订单线下支付上传附件
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public OrderPay OrderPayAttch(string JSon)
    {
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            string path = ConfigurationManager.AppSettings["OrderPayAttchPath"].ToString().Trim();
            string viewpath = ConfigurationManager.AppSettings["OrderPayAttchViewPath"].ToString().Trim();
            //判断文件夹是否存在
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
            string FileName = JInfo["fileName"].ToString();
            string StringImage = JInfo["base64"].ToString();

            string pat = @"data:image/(\w+);base64,";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            StringImage = r.Replace(StringImage, "");

            string ext = FileName.Substring(FileName.LastIndexOf("."));
            //保存在服务器的文件名
            string toFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + ext;
            string saveFile = path + FileName.Substring(0, FileName.LastIndexOf('.')) + "__" + toFileName;

            byte[] b = Convert.FromBase64String(StringImage);
            System.IO.File.WriteAllBytes(saveFile, b);

            string viewpathfile = FileName.Substring(0, FileName.LastIndexOf('.')) + "__" + toFileName;

            return new OrderPay() { Result = "T", Description = viewpathfile };
        }
        catch (Exception)
        {
            return new OrderPay() { Result = "F", Description = "上传失败" };
        }
    }

    //订单详情返回参数
    public class ResultOrderInfo
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public class_ver3.Order Order { get; set; }
    }
    //订单发货单修改的返回参数
    public class ResultEdit
    {
        public String Result { get; set; }
        public String Description { get; set; }

    }

    //核心企业订单获取发货信息列表的返回参数
    public class ResultGetOrderOut
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.OrderOut> OrderOutList { get; set; }
        public List<class_ver3.UnSendoutDetail> UnSendoutDetailList { get; set; }
    }
    //修改订单时获取商品价格的返回
    public class ResultProductPrice
    {
        public String Result{get;set;}
        public String Description { get; set; }
        public String Reate { get; set; }
        public String PostFee { get; set; }
        public String TotalAmount { get; set; }
        public String AuditTotalAmount { get; set; }
        public String IsOrderPro { get; set; }
        public class_ver3.OrderPro ProInfo { get; set; }
        public List<class_ver3.OrderDetail> OrderDetailList{get;set;}
    }
  
    //确认收货返回参数
    public class ResultOutConfirm
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String ReceiptNo { get; set; }
        public String OrderOutID { get; set; }
    }
   //经销商开票信息的返回参数
    public class ResultBillList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.BillInfo> BillInfoList { get; set; } 
    }


    //发货返回信息
    public class ResultAudit
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<string> ReceiptNoList { get; set; }
    }
    public class ResultOrderPrompt
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String OrderNum { get; set; }//经销商待付款订单数
        public String BillNum { get; set; }//经销商待付款账单数
        public String OrderAudit { get; set; }//核心企业待审核订单数
        public String OrderReturnAudit { get; set; }//核心企业待退货审核订单数
        public String OrderSend { get; set; }//核心企业待发货订单数
        public String ResellerAudit { get; set; }//核心企业待审核经销商数
    }
    //获取核心企业简报的返回参数
    public class ResultCompyBriefing
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<Briefing> BriefingList { get; set; }
    }
    public class Briefing
    {
        public String Type { get; set; }//类型(0：订单、1：退货单、2：订单收款)
        public String DayNumb { get; set; }//当天笔数
        public String WeekNumb { get; set; }//本周笔数
        public String MonthNumb { get; set; }//本月笔数
        public String DayMoney { get; set; }//当天金额
        public String WeekMoney { get; set; }//本周金额
        public String MonthMoney { get; set; }//本月金额
    }
    public class ResultCompanySta
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<string> XList { get; set; }
        public List<string> XValueList { get; set; }
    }
    public class ResultResellerList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.ResellerSimple> ResellerList { get; set; }
    }
    //public class Reseller
    //{
    //    public String ResellerName { get; set; }
    //    public String ResellerID { get; set; }
    //}


    public class ResultOrderList
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<Order> OrderList { get; set; }
    }

    public class Order
    {
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

    //修改物流信息的返回参数
    public class ResultEditLogistics
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public class_ver3.Wuliu Logistics { get; set; }
    }

    //订单线下支付的返回参数
    public class OrderPay
    {
        public String Result { get; set; }
        public String Description { get; set; }
    }
}