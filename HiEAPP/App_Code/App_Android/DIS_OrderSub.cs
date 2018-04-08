using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using LitJson;
using Hi.BLL;

public class DIS_OrderSub
{
    public DIS_OrderSub()
    {
    }

    /// <summary>
    /// 订单提交
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultSubOrder SubResellerOrder(string JSon,string version)
    {
        string UserID = string.Empty;
        string DisID = string.Empty; //当前列表最临界点产品ID:初始-1
        decimal bate_amount = 0;
        string strsql = "";
        string RebateSum = "";
        DataTable dt_info;
        string goodsid = "";
        string goodsinfovalue = "";
        string goodsname = "";
        string returnmsg = "";
        string GiveMode = "";
        string IsOBill = "";
        string Rise = string.Empty;
        string Content = string.Empty;
        string OBank = string.Empty;
        string OAccount = string.Empty;
        string TRNumber= string.Empty;
        string isint = string.Empty;
        int isint_out = 0;
        decimal num_deci = 0;
        int num_int = 0;
        //string ts_rebate = string.Empty;
        //string ts_goodsinfo = string.Empty;
       

        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            //if (version.ToLower() == "android" || version.ToLower() == "ios" || float.Parse(version) < 3)//在版本3之前的版本不传配送方式
            //{
                if (JInfo.Count == 0 || JInfo["UserID"].ToString() == "" ||
                    (JInfo["OrderList"].ToString() == "" && JInfo["ResellerID"].ToString() == ""))
                {
                    return new ResultSubOrder() { Result = "F", Description = "参数异常" };
                }

            //}
    //        else if(float.Parse(version) >= 3)//版本3及以上版本，需要传配送方式(值必须为送货或自提)，且为必输字段，需要判断是否为空
    //        {
    //            if (JInfo.Count == 0 || JInfo["UserID"].ToString() == "" ||
    //(JInfo["OrderList"].ToString() == "" && JInfo["ResellerID"].ToString() == "") || JInfo["GiveMode"].ToString() == ""||
    //               (JInfo["GiveMode"].ToString() != "送货" && JInfo["GiveMode"].ToString() != "自提") )
    //            {
    //                return new ResultSubOrder() { Result = "F", Description = "参数异常" };
    //            }
    //            GiveMode = JInfo["GiveMode"].ToString();
    //            //IsOBill = JInfo["BillInfo"]["IsOBill"].ToString();
    //        }
            //if (GiveMode == "")//版本3以前，没传配送方式字段，所以不会给GiveMode变量再次赋值，GiveMode就等于""，这时候配送方式默认为送货，GiveMode需要赋值为送货。
            //    GiveMode = "送货";


            UserID = JInfo["UserID"].ToString();
            DisID = JInfo["ResellerID"].ToString();
            
            
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one,0, int.Parse(DisID == "" ? "0" : DisID)))
                return new ResultSubOrder() { Result = "F", Description = "登录信息异常" };

            Hi.Model.BD_Distributor distributor = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(DisID));
            if (distributor == null||distributor.AuditState ==0 || distributor.IsEnabled ==0||distributor.dr==1)
                return new ResultSubOrder() {Result = "F", Description = "经销商信息异常"};

            List<Hi.Model.DIS_Order> Orders = new List<Hi.Model.DIS_Order>();
            List<OrderList> List = new List<OrderList>();
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", distributor.CompID).ToInt(0);//判断此核心企业是否启用库存
            isint = Common.rdoOrderAudit("订单下单数量是否取整", distributor.CompID);//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
            int isrebate = Common.rdoOrderAudit("订单支付返利是否启用", distributor.CompID).ToInt(0);//判断核心企业是否启用返利s
            int endnode = Common.rdoOrderAudit("订单完成节点设置",distributor.CompID).ToInt();//判断核心企业完成节点。
            foreach (JsonData JList in JInfo["OrderList"])
            {
                decimal totAmount = 0;
                OrderList aOrder = new OrderList();

                #region 主表

                string CompID = JList["CompID"].ToString();
                string ReceiptNo = JList["ReceiptNo"].ToString();
                string AddType = JList["AddType"].ToString(); //3：App经销商下单 4：App企业下单
                string SubType = JList["SubType"].ToString(); //0：新下订单 1：编辑订单
                string Otype = JList["Otype"].ToString(); //3：企业改价，非常重要
                string AddrID = JList["AddrID"].ToString();
                string ArriveDate = JList["ArriveDate"].ToString();
                string OrderRemark = Common.NoHTML(JList["OrderRemark"].ToString());
                string TotalAmount = JList["TotalAmount"].ToString();
                string Rebate = JList["Rebate"].ToString();
                if (version.ToLower() == "android" || version.ToLower() == "ios" || float.Parse(version) < 3)//在版本3之前的版本不传配送方式,发票信息
                {
                }
                else if (float.Parse(version) >= 3)//版本3及以上版本，需要传配送方式(值必须为送货或自提)，且为必输字段，需要判断是否为空,同时也需要判断billinfo是否为空，billinfo.isobill是否为空
                {
                    if (JList["GiveMode"].ToString() == "" || (JList["GiveMode"].ToString() != "送货" && JList["GiveMode"].ToString() != "自提")||
                        JList["BillInfo"].ToString() == "" || JList["BillInfo"]["IsOBill"].ToString() == ""||JList["BillInfo"]["DisID"].ToString() != DisID)
                    {
                        return new ResultSubOrder() { Result = "F", Description = "参数异常" };
                    }
                    GiveMode = JList["GiveMode"].ToString();
                    IsOBill = JList["BillInfo"]["IsOBill"].ToString();
                    Rise = Common.NoHTML(JList["BillInfo"]["Rise"].ToString());
                    Content = Common.NoHTML(JList["BillInfo"]["Content"].ToString());
                    OBank = Common.NoHTML(JList["BillInfo"]["OBank"].ToString());
                    OAccount = Common.NoHTML(JList["BillInfo"]["OAccount"].ToString());
                    TRNumber = Common.NoHTML(JList["BillInfo"]["TRNumber"].ToString());
                }

                if (GiveMode == "")//版本3以前，没传配送方式字段，所以不会给GiveMode变量再次赋值，GiveMode就等于""，这时候配送方式默认为送货，GiveMode需要赋值为送货。
                    GiveMode = "送货";

                if (IsOBill == "")//版本3以前，没传发票信息，所以不会给IsOBill变量再次赋值，IsOBill就等于""，这时候开票方式默认为不开票，IsOBill需要赋值为0。
                    IsOBill = "0";

                JsonData OrderDetailList = JList["OrderDetailList"];
                bate_amount += decimal.Parse(Rebate);
                if (isrebate == 1)
                {
                    //并发操作
                    //strsql = "select MAX(ts) from BD_Rebate where DisID = '" + DisID + "' ";
                    //strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
                    //strsql += " and isnull(RebateState,1) = 1";
                    //strsql += " and isnull(dr,0) = 0 and compid = '" + CompID + "'";
                    //ts_rebate = SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer).ToString();
                    strsql = "select sum(EnableAmount) as RebateSum from BD_Rebate where DisID = '" + DisID + "' ";
                    strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
                    strsql += " and isnull(RebateState,1) = 1";
                    strsql += " and isnull(dr,0) = 0 and compid = '" + CompID + "'";
                    RebateSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                    RebateSum = double.Parse(RebateSum).ToString("F2");
                    if (bate_amount > decimal.Parse(RebateSum))
                    {
                        return new ResultSubOrder()
                        {
                            Result = "3",
                            Description = "使用的返利金额大于可用返利金额",
                            OrderList = new List<DIS_Order.Order>()
                        {
                            new DIS_Order.Order()
                            {
                                ReceiptNo = ReceiptNo,
                                RebateAmount = RebateSum
                            }
                        }
                        };

                    }
                }
                else if (bate_amount > 0)
                {
                    return new ResultSubOrder()
                    {
                        Result = "3",
                        Description = "该核心企业没有启用返利支付",
                        OrderList = new List<DIS_Order.Order>()
                        {
                            new DIS_Order.Order()
                            {
                                ReceiptNo = ReceiptNo,
                                RebateAmount = "0"
                            }
                        }
                    };
                }
                

                Hi.Model.DIS_Order order = null;
                if (int.Parse(SubType) == 0)
                {
                    order = new Hi.Model.DIS_Order();
                    order.GUID = Guid.NewGuid().ToString().Replace("-", "");
                    
                    order.ReceiptNo = Common.GetNewCode("销售单");
                    order.CompID = Convert.ToInt32(CompID);
                    order.DisID = Convert.ToInt32(DisID);
                    order.DisUserID = Convert.ToInt32(UserID);
                    order.CreateUserID = Convert.ToInt32(UserID);
                    order.IsSettl = "0";//是否结算
                    order.GiveMode = GiveMode;//版本3之前版本默认是送货
                    order.CreateDate = DateTime.Now;
                    if (Otype == "1")
                    {
                        order.AuditUserID = Convert.ToInt32(UserID);
                        order.AuditDate = DateTime.Now;
                    }
                }
                if (int.Parse(SubType) == 1)
                {
                    order = new Common().GetOrderByReceiptNo(ReceiptNo);
                    if (order.OState != (int) Enums.OrderState.未提交 || order.ReceiptNo != ReceiptNo)
                        return new ResultSubOrder() {Result = "F", Description = "不能修改"};
                }
                if (ArriveDate != "")
                {
                    //if (DateTime.Compare(DateTime.Parse(ArriveDate), DateTime.Now) < 0)
                    //    return new ResultSubOrder() { Result="F",Description ="到货日期必须大于当前日期"};
                    order.ArriveDate = Convert.ToDateTime(ArriveDate);
                }
                //订单是否需审核
                int IsAudit = Common.OrderEnAudit(order.DisID, int.Parse(Otype));
                if (endnode != 1)
                {
                    order.OState = IsAudit == 1 ? (int)Enums.OrderState.已审 : (int)Enums.OrderState.待审核;
                }
                else
                {
                    order.OState = (int)Enums.OrderState.已到货;
                }
                order.IsAudit = IsAudit == 1 ? 1 : 0;
                if (IsAudit == 1)
                    order.AuditDate = DateTime.Now;

                //order.PayState = (int) Enums.PayState.未支付;
                order.Remark = OrderRemark;
                //order.ArriveDate = ArriveDate.ToDateTime();
                order.Otype = int.Parse(Otype);
                order.AddType = AddType == "4" ? (int) Enums.AddType.App企业补单 : (int) Enums.AddType.App下单;

                order.ts = DateTime.Now;
                order.bateAmount = decimal.Parse(Rebate);
                order.dr = 0;
                order.modifyuser = int.Parse(UserID);

                //收货地址
                Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(int.Parse(AddrID));
                if (DisAddr == null)
                {
                    return new ResultSubOrder() {Result = "F", Description = "收货地址异常"};
                }
                order.AddrID = int.Parse(AddrID);
                order.Phone = DisAddr.Phone;
                order.Principal = DisAddr.Principal;
                order.Address = DisAddr.Address;
                //总价统计在明细代码后
                
                order.OtherAmount = 0;

                #endregion

                decimal amount = 0; //用于保存打折优惠前的总价

                #region 订单明细
                List<DIS_Order.OrderDetail> orderDetailList_false = new List<DIS_Order.OrderDetail>();//存放存货不足的订单明细list
                List<Hi.Model.DIS_OrderDetail> detailList = new List<Hi.Model.DIS_OrderDetail>();
                if (OrderDetailList.Count == 0)
                    return new ResultSubOrder() {Result = "F", Description = "参数异常"};
                foreach (JsonData item in OrderDetailList)
                {
                    DIS_Order.OrderDetail orderdetail_false = new DIS_Order.OrderDetail();//存放存货不足的订单明细
                    //decimal Num_pro = 0;
                    string ProductID = item["ProductID"].ToString(); //goodsID
                    string SKUID = item["SKUID"].ToString(); //goodsInfoID
                    string Num = item["Num"].ToString();
                    //核心企业商品数量取整的时候，需要判断传入数量是不是整数
                    if (isint == "0")
                    {
                        num_deci = decimal.Parse(Num);
                        num_int = (int)num_deci;
                        if (!int.TryParse(Num, out isint_out))
                        {
                            if (decimal.Parse(num_int.ToString()) != num_deci)
                            {
                                return new ResultSubOrder() { Result = "F", Description = "商品数量应为整数" };
                            }
                        }
                    }
                    //string Remark = item["Remark"].ToString();
                    string IsPro = item["IsPro"].ToString();
                    Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(int.Parse(ProductID));

                    string Price = string.Empty;
                    string SumAmount = string.Empty;
                    
                    Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(int.Parse(SKUID));
                    if (!Common.IsOffline(int.Parse(SKUID)))
                        return new ResultSubOrder()
                        {
                            Result = "2", 
                            Description = "商品:" + goods.GoodsName + ""+goodsInfo.ValueInfo+"被禁用",
                            OrderList = new List<DIS_Order.Order>()
                            {
                                new DIS_Order.Order()
                                {
                                    ReceiptNo = ReceiptNo,
                                    OrderRemark = "-1",
                                    OrderDetailList = new List<DIS_Order.OrderDetail>()
                                    {
                                        new DIS_Order.OrderDetail()
                                        {
                                            SKUID = SKUID,
                                            ProductID = ProductID,
                                            Remark = "-1"
                                        }
                                    }
                                }
                            }
                        };

                    #region 取价格
                    //if (IsInve == 0)
                    //{
                    // //取出下单时的goodsinfo表的时间戳
                    //    //strsql = "select ts from bd_goodsinfo where id = " + SKUID + "";
                    //    //ts_goodsinfo = SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer).ToString();

                    ////判断此规格属性的商品的库存是否足够
                    //strsql = "select Inventory from bd_goodsinfo where id = " + SKUID + " ";
                    //decimal inv_sum = decimal.Parse(ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0"));
                    //strsql = "select goodsname from bd_goods where id = " + ClsSystem.gnvl(goodsInfo.GoodsID ,"0") + " ";
                    //goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    ////库存不足的话，需要提示商品名称规格属性的，跳出循环

                    //    if (inv_sum < Num.ToDecimal())
                    //    {
                    //        strsql = "select goodsid,valueinfo from bd_goodsinfo where id = '" + SKUID + "'";
                    //        dt_info = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                    //        goodsid = ClsSystem.gnvl(dt_info.Rows[0]["goodsid"], "");
                    //        goodsinfovalue = ClsSystem.gnvl(dt_info.Rows[0]["valueinfo"], "");
                    //        //strsql = "select goodsname from bd_goods where id = '" + goodsid + "' ";
                    //        //goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    //        //ResultCheck.Result = "3";

                    //        returnmsg += "" + goodsname + "" + goodsinfovalue + "的库存数量不足";
                    //        orderdetail_false.SKUID = SKUID;
                    //        orderdetail_false.NumEnable = inv_sum.ToString();
                    //        orderDetailList_false.Add(orderdetail_false);
                    //        break;


                    //    }
                    //}

                    int ProID = 0; //促销ID
                    
                    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                        " disID='" + DisID + "'" +
                        " and GoodsInfoID='" + SKUID + "' and ISNULL(dr,0)=0 and compid='" + CompID +
                        "' and IsEnabled=1", "");
                    amount += (goodsPrice != null && goodsPrice.Count > 0
                        ? goodsPrice[0].TinkerPrice
                        : goodsInfo.TinkerPrice)
                              *decimal.Parse(Num);
                    //特价商品，取传值的价格（经销商传值原价，企业改价）
                    if (Otype.Trim() == "3")
                    {
                        Price = item["Price"].ToString();
                    }
                        //取促销价格
                    else if (IsPro.Trim() == "1")
                    {
                        decimal price = Common.GetProPrice(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(),
                            goodsInfo.CompID.ToString(), out ProID);
                        if (price == 0)
                            return new ResultSubOrder()
                            {
                                Result = "2",
                                Description = "商品:" + SKUID + "异常",
                                OrderList = new List<DIS_Order.Order>()
                            {
                                new DIS_Order.Order()
                                {
                                    ReceiptNo = ReceiptNo,
                                    OrderRemark = "-1",
                                    OrderDetailList = new List<DIS_Order.OrderDetail>()
                                    {
                                        new DIS_Order.OrderDetail()
                                        {
                                            SKUID = SKUID,
                                            ProductID = ProductID,
                                            Remark = "-1"
                                        }
                                    }
                                }
                            }
                            };
                        //如果商品是满送的话需要算出赠送的数量
                        //if (ProType_goods == 3)
                        //{
                        //    Num_pro = Common.GetProNum(ProID,decimal.Parse(Num));
                        //}
                        ////Price = price.ToString();
                    }
                   

                        //普通商品，已经改价
                    //else if (goodsPrice != null && goodsPrice.Count > 0)
                    //{
                    //    Price = goodsPrice[0].TinkerPrice.ToString();
                    //}
                    //else //普通商品，未改价
                    //{
                    //    Price = goodsInfo.TinkerPrice.ToString();
                    //}
                    Price = BLL.Common.GetGoodsPrice(CompID.ToInt(),Convert.ToInt32(DisID),Convert.ToInt32(SKUID)).ToString("0.00");
                    SumAmount = (decimal.Parse(Price)*decimal.Parse(Num)).ToString();
                    totAmount += Convert.ToDecimal(SumAmount);

                    #endregion

                    if (decimal.Parse(item["Price"].ToString()) != decimal.Parse(Price) || decimal.Parse(item["SumAmount"].ToString()) != decimal.Parse(SumAmount))
                    {
                        return new ResultSubOrder()
                        {
                            Result = "1",
                            Description = "商品:" + SKUID + "被禁用",
                            OrderList = new List<DIS_Order.Order>()
                            {
                                new DIS_Order.Order()
                                {
                                    ReceiptNo = ReceiptNo,
                                    OrderRemark = "-1",
                                    OrderDetailList = new List<DIS_Order.OrderDetail>()
                                    {
                                        new DIS_Order.OrderDetail()
                                        {
                                            SKUID = SKUID,
                                            ProductID = ProductID,
                                            Price = Price,
                                            Remark = "-1"
                                        }
                                    }
                                }
                            }
                        };
                    }
                    Hi.Model.DIS_OrderDetail OrderDeModel = new Hi.Model.DIS_OrderDetail();
                    //OrderDeModel.Dts = ts_goodsinfo;
                    OrderDeModel.DisID = int.Parse(DisID);
                    OrderDeModel.GoodsinfoID = Convert.ToInt32(SKUID);
                    OrderDeModel.GoodsName = goods.GoodsName;//商品名称
                    OrderDeModel.GoodsCode =goodsInfo.BarCode;//商品编码
                    OrderDeModel.Unit = goods.Unit;//单位
                    //OrderDeModel.Remark = Remark;
                    OrderDeModel.GoodsNum = Convert.ToDecimal(Num);
                    OrderDeModel.ProID = ProID.ToString();
                    OrderDeModel.GoodsPrice = Convert.ToDecimal(Price);
                    //OrderDeModel.Price = Convert.ToDecimal(Price);
                    OrderDeModel.AuditAmount = Convert.ToDecimal(Price);
                    OrderDeModel.sumAmount = Convert.ToDecimal(SumAmount);
                    //时间戳
                    OrderDeModel.ts = DateTime.Now;
                    OrderDeModel.dr = 0;
                    OrderDeModel.Remark = "";
                    OrderDeModel.modifyuser = int.Parse(UserID);
                    OrderDeModel.GoodsInfos = goodsInfo.ValueInfo;
                    string pty;
                    string ppty;
                    string pid_num;
                    OrderDeModel.ProNum =
                        Common.GetProNum(ProductID, SKUID, Convert.ToInt32(CompID),decimal.Parse(Num), out pty,out ppty,out pid_num).ToString();
                    OrderDeModel.Protype = ppty;
                    //if (IsInve == 0)
                    //{
                    //    //取出下单时的goodsinfo表的时间戳
                    //    //strsql = "select ts from bd_goodsinfo where id = " + SKUID + "";
                    //    //ts_goodsinfo = SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer).ToString();

                    //    //判断此规格属性的商品的库存是否足够
                    //    strsql = "select Inventory from bd_goodsinfo where id = " + SKUID + " ";
                    //    decimal inv_sum = decimal.Parse(ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0"));
                    //    strsql = "select goodsname from bd_goods where id = " + ClsSystem.gnvl(goodsInfo.GoodsID, "0") + " ";
                    //    goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    //    //库存不足的话，需要提示商品名称规格属性的，跳出循环

                    //    if (inv_sum < Num.ToDecimal() + OrderDeModel.ProNum.ToDecimal())
                    //    {
                    //        strsql = "select goodsid,valueinfo from bd_goodsinfo where id = '" + SKUID + "'";
                    //        dt_info = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                    //        goodsid = ClsSystem.gnvl(dt_info.Rows[0]["goodsid"], "");
                    //        goodsinfovalue = ClsSystem.gnvl(dt_info.Rows[0]["valueinfo"], "");
                    //        //strsql = "select goodsname from bd_goods where id = '" + goodsid + "' ";
                    //        //goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                    //        //ResultCheck.Result = "3";

                    //        returnmsg += "" + goodsname + "" + goodsinfovalue + "的库存数量不足";
                    //        orderdetail_false.SKUID = SKUID;
                    //        orderdetail_false.NumEnable = inv_sum.ToString();
                    //        orderDetailList_false.Add(orderdetail_false);
                    //        break;


                    //    }
                    //}


                    if (IsInve == 0 && goodsInfo.Inventory < Num.ToDecimal() + OrderDeModel.ProNum.ToDecimal())
                    {
                        returnmsg += "" + goods.GoodsName + "" + goodsInfo.ValueInfo + "的库存数量不足";
                        orderdetail_false.SKUID = SKUID;
                        orderdetail_false.NumEnable = goodsInfo.Inventory.ToString();
                        orderDetailList_false.Add(orderdetail_false);
                        break;
                    }
                    detailList.Add(OrderDeModel);
                }
                if (orderDetailList_false.Count > 0)
                {
                    return new ResultSubOrder()
                    {
                        Result = "F",
                        Description = returnmsg,
                        OrderList = new List<DIS_Order.Order>()
                        {
                            new DIS_Order.Order()
                            {
                                OrderDetailList = orderDetailList_false
                            }
                        }
                    };
                }

                //主表总价统计
                //order.TotalAmount = amount;
                //order.TotalAmount = totAmount;

                #endregion

                //即使企业改价，也能做整单促销
                string pid = "0";
                string ProIDD = "";
                string protpye = "";
                decimal pprice = Common.GetProPrice(totAmount, out pid,out ProIDD,out protpye, order.CompID);

                //需要导入订单拓展表的数据
                Hi.Model.DIS_OrderExt orderext = new Hi.Model.DIS_OrderExt();
                orderext.ProID = pid.ToInt();
                orderext.ProAmount = pprice;
                orderext.ProDID = ProIDD.ToInt();
                orderext.Protype = protpye;

                if (IsOBill == "0")
                {
                    orderext.IsOBill = 0;
                    orderext.IsBill = 0;
                    orderext.Rise = "";
                    orderext.Content = "";
                    orderext.OBank = "";
                    orderext.OAccount = "";
                    orderext.TRNumber = "";
                }
                else
                {
                    orderext.IsOBill = IsOBill.ToInt();
                    orderext.IsBill = 0;
                    orderext.Rise = Rise;
                    orderext.Content = Content;
                    orderext.OBank = OBank;
                    orderext.OAccount = OAccount;
                    orderext.TRNumber = TRNumber;
                }
                //order.vdef4 = pid;
                //order.vdef5 = pprice.ToString();
                //order.vdef6 = ProIDD;
                //order.vdef7 = protpye;

                if (Convert.ToDecimal(TotalAmount) != totAmount - pprice-decimal.Parse(Rebate))
                return new ResultSubOrder() {
                        Result = "1", 
                        Description = "商品价格已修改",
                        OrderList = new List<DIS_Order.Order>()
                    {
                        new DIS_Order.Order(){ TotalAmount = (totAmount - pprice).ToString(),
                            AuditTotalAmount = (totAmount - pprice-decimal.Parse(Rebate)).ToString()}
                    }};
                order.TotalAmount = totAmount;
                order.AuditAmount = totAmount - pprice-decimal.Parse(Rebate);
                if (totAmount - pprice - decimal.Parse(Rebate) == 0)
                {
                    if (IsAudit == 1)
                    {
                        order.PayState = (int)Enums.PayState.已支付;
                    }

                }
                else
                {
                    order.PayState = (int)Enums.PayState.未支付;             
                }

                Orders.Add(order);

                aOrder.act = SubType;
                aOrder.order = order;
                aOrder.orderDetail = detailList;
                aOrder.orderext = orderext;
                //aOrder.ts_rebate = ts_rebate;
                List.Add(aOrder);
                
            }
            //将订单插入数据库
            int count = TransOrderList(List);

            if (count == 0)
            {
                return new ResultSubOrder() {Result = "F", Description = "操作失败"};
            }
            else
            {
                #region 赋值

                
                List<DIS_Order.Order> OrderList = new List<DIS_Order.Order>();

                if (Orders == null || Orders.Count == 0)
                    return new ResultSubOrder()
                    {
                        Result = "F",
                        Description = "没有更多数据",
                        IsCheck = distributor.IsCheck.ToString()
                    };

                foreach (Hi.Model.DIS_Order row in Orders)
                {
                    DIS_Order.Order order = new DIS_Order.Order();
                    Hi.Model.DIS_Order orderModel = row;
                    order.OrderID = count.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new ResultSubOrder()
                        {
                            Result = "T",
                            Description = "企业异常",
                            IsCheck = distributor.IsCheck.ToString()
                        };
                    order.CompName = comp.CompName;
                    order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                        orderModel.ReturnState);
                    order.OState = orderModel.OState.ToString();
                    order.AddType = orderModel.AddType.ToString();
                    order.Otype = orderModel.Otype.ToString();
                    order.PayState = orderModel.PayState.ToString();
                    order.ReturnState = orderModel.ReturnState.ToString();
                    order.DisID = orderModel.DisID.ToString();
                    order.RebateAmount = ClsSystem.gnvl(orderModel.bateAmount,"0.00");
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
                    if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                        return new ResultSubOrder()
                        {
                            Result = "T",
                            Description = "经销信息商异常",
                            IsCheck = distributor.IsCheck.ToString()
                        };
                    order.DisName = dis.DisName;
                    order.DisUserID = orderModel.DisUserID.ToString();
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(order.DisUserID));
                    if (user == null || user.IsEnabled == 0 || user.dr == 1)
                        return new ResultSubOrder()
                        {
                            Result = "T",
                            Description = "经销商用户信息异常",
                            IsCheck = distributor.IsCheck.ToString()
                        };
                    order.DisUserName = user.TrueName;
                    order.AddrID = orderModel.AddrID.ToString();
                    order.ReceiptNo = orderModel.ReceiptNo;
                    if (ClsSystem.gnvl(order.ArriveDate, "") != "")
                    {
                        order.ArriveDate = orderModel.ArriveDate.ToString("yyyy-MM-dd");
                    }
                   
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
                    order.CreateDate = orderModel.CreateDate.ToString();
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
                        //order.Express = orderOut.Express;
                        //order.ExpressNo = orderOut.ExpressNo;
                        //order.ExpressPerson = orderOut.ExpressPerson;
                        //order.ExpressTel = orderOut.ExpressTel;
                        //order.ExpressBao = orderOut.ExpressBao;
                        //order.PostFee = orderOut.PostFee.ToString("0.00");
                        order.ActionUser = orderOut.ActionUser;
                        order.SendRemark = orderOut.Remark;
                        order.IsAudit = orderOut.IsAudit.ToString();
                        order.AuditUserID = orderOut.AuditUserID.ToString();
                        order.AuditDate = orderOut.AuditDate.ToString();
                        order.AuditRemark = orderOut.AuditRemark;
                        order.SignDate = orderOut.SignDate.ToString();
                        order.IsSign = orderOut.IsSign.ToString();
                        order.SignUserId = orderOut.SignUserId.ToString();
                        order.SignUser = orderOut.SignUser;
                        order.SignRemark = orderOut.SignRemark;
                    }
                    order.SendRemark = orderModel.Remark;
                    //todo:不知道的排序
                    //order.SortIndex = orderModel.SortIndex.ToString();               
                    order.IsDel = orderModel.dr.ToString();

                    //明细
                    List<DIS_Order.OrderDetail> orderDetail = new List<DIS_Order.OrderDetail>();
                    List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                        " OrderID='" + count + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (detailList != null)
                    {
                        List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                        foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                        {
                            string SKUName = string.Empty;
                            DIS_Order.OrderDetail ordetail = new DIS_Order.OrderDetail();
                            ordetail.SKUID = detail.GoodsinfoID.ToString();
                            //通过GoodsInfoID找到GoodsID
                            Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                            if (goodsInfo == null)
                                //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                                return new ResultSubOrder()
                                {
                                    Result = "T",
                                    Description = "SKU信息异常",
                                    IsCheck = distributor.IsCheck.ToString()
                                };
                            ordetail.ProductID = goodsInfo.GoodsID.ToString();

                            //通过GoodsID找到GoodsName
                            Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                            if (goods == null)
                                //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                                return new ResultSubOrder()
                                {
                                    Result = "T",
                                    Description = "商品异常",
                                    IsCheck = distributor.IsCheck.ToString()
                                };
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
                            ordetail.NumEnable = detail.GoodsNum.ToString("0.00");
                            ordetail.Remark = detail.Remark;
                            ordetail.IsPro = ClsSystem.gnvl(detail.ProID, "0").Trim() == "0" || ClsSystem.gnvl(detail.ProID, "").Trim() == "" ? "0" : "1"; //是否是促销商品

                            if (ordetail.IsPro != "0")
                            {
                                ordetail.ProNum = detail.ProNum;
                                if (ClsSystem.gnvl(detail.ProID, "") != "" && ClsSystem.gnvl(detail.ProID, "").Length > 0)
                                {
                                    Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(detail.ProID));
                                    if (pro != null)
                                    {
                                        List<Hi.Model.BD_PromotionDetail> dList =
                                            new Hi.BLL.BD_PromotionDetail().GetList("",
                                                " ProID=" + pro.ID + " and GoodInfoID ='" + ordetail.SKUID +
                                                "' and dr=0", "");
                                        string info = string.Empty;
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

                            List<DIS_Order.Pic> Pic = new List<DIS_Order.Pic>();
                            if (goods.Pic.ToString() != "" && goods.Pic.ToString() != "X")
                            {
                                DIS_Order.Pic pic = new DIS_Order.Pic();
                                pic.ProductID = goodsInfo.GoodsID.ToString();
                                pic.IsDeafult = "1";
                                pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() +
                                             "GoodsImg/" + goods.Pic;
                                Pic.Add(pic);
                            }
                            ordetail.ProductPicUrlList = Pic;

                            orderDetail.Add(ordetail);
                        }
                    }
                    order.OrderDetailList = orderDetail;
                    OrderList.Add(order);
                }

                #endregion

                return new ResultSubOrder()
                {
                    Result = "T",
                    Description = "操作成功",
                    IsCheck = distributor.IsCheck.ToString(),
                    OrderList = OrderList
                };
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "SubResellerOrder:"+JSon);
            return new ResultSubOrder() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 退货申请
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAudit SubOrderReturn(string JSon)
    {
        try
        {
            string userID = string.Empty;
            string disID = string.Empty;
            List<OrderReturnList> orderList = new List<OrderReturnList>();
            List<String> ReceiptNoList = new List<string>();

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString().Trim() != "" &&
                JInfo["ReceiptNoList"].ToString().Trim() != "" && JInfo["ResellerID"].ToString() == "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
            }
            else
            {
                return new ResultAudit() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultAudit() { Result = "F", Description = "用户异常" };

            foreach (JsonData JList in JInfo["ReceiptNoList"])
            {
                string AddType = JList["AddType"].ToString();
                string ReceiptNo = JList["ReceiptNo"].ToString();
                ReceiptNoList.Add(ReceiptNo);
                string returnContent = JList["ReturnContent"].ToString();

                if (AddType.Trim() == "" || ReceiptNo.Trim() == "" || returnContent.Trim() == "")
                    return new ResultAudit() { Result = "F", Description = "参数异常" };

                Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(ReceiptNo.Trim());
                if (order == null || AddType != order.AddType.ToString())
                    return new ResultAudit() { Result = "F", Description = "订单异常" };

                Hi.Model.DIS_OrderReturn orderReturn = new Hi.Model.DIS_OrderReturn();
                orderReturn.CompID = order.CompID;
                orderReturn.DisID = order.DisID;
                orderReturn.OrderID = order.ID;
                orderReturn.CreateDate = DateTime.Now;
                orderReturn.CreateUserID = user.ID;
                orderReturn.ReturnContent = returnContent;
                orderReturn.ReturnState = (int) Enums.AuditState.提交;
                orderReturn.ts = DateTime.Now;
                orderReturn.modifyuser = user.ID;

                OrderReturnList returnModer = new OrderReturnList();
                order.ReturnMoneyUser = user.UserName;
                order.ReturnMoneyDate = DateTime.Now;
                order.ReturnMoneyUserId = user.ID;
                returnModer.order = order;
                returnModer.orderReturn = orderReturn;
                returnModer.user = user;
                orderList.Add(returnModer);
            }
            if (orderList.Count == 0)
                return new ResultAudit() { Result = "F", Description = "退货列表为空" };
            else
            {
                int res = TransOrderReturnList(orderList);
                if (res == 0)
                    return new ResultAudit() { Result = "F", Description = "退货列表为空" };
                else
                {
                    foreach (OrderReturnList order in orderList)
                    {
                        Common.AddSysBusinessLog(order.order, order.user, "Order", order.order.ID.ToString(),
                        "订单新增", order.orderReturn.ReturnContent);
                        MsgSend.Jpushdega jpushdega = new MsgSend.Jpushdega(new MsgSend().GetWxService);
                        jpushdega.BeginInvoke("4", order.order.ID.ToString(), "1", 0, null, null);
                        //new MsgSend().GetWxService("4", order.order.ID.ToString(), "1");
                    }
                    return new ResultAudit() { Result = "T", Description = "退货申请成功", ReceiptNoList = ReceiptNoList };
                }
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":"+ ex.StackTrace, "SubOrderReturn:" + JSon);
            return new ResultAudit() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 退货审核
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAudit AuditOrderReturn(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            //string ts_order = string.Empty;
            //int ists = 0;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ReceiptNoList"].ToString() != "" &&
                JInfo.Count > 0 && JInfo["CompanyID"].ToString() != "" )
            {
                compID = JInfo["CompanyID"].ToString();
                userID = JInfo["UserID"].ToString();
            }
            else
            {
                return new ResultAudit() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID == "" ? "0" : compID)))
                return new ResultAudit() { Result = "F", Description = "未找到用户信息" };

            #endregion

            #region 事务赋值

            List<string> list = new List<string>();
            List<string> orderList = new List<string>();
            List<OrderReturnAudit> auditList = new List<OrderReturnAudit>();

            foreach (JsonData JList in JInfo["ReceiptNoList"])
            {
                string Act = JList["Act"].ToString(); //审核通过0，退回为1
                string ReceiptNo = JList["ReceiptNo"].ToString();
                string AuditRemark = Common.NoHTML(JList["AuditRemark"].ToString());
                list.Add(ReceiptNo.ToString());
                
                Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(ReceiptNo.ToString());
                //ts_order = order.ts.ToString();
                if (order == null || order.dr == 1)
                    return new ResultAudit() { Result = "F", Description = "订单信息异常" };
                orderList.Add(order.ID.ToString());
                List<Hi.Model.DIS_OrderReturn> orderReturnList = new Hi.BLL.DIS_OrderReturn().GetList("", " orderID=" + order.ID + " and ReturnState =1", "");
                if (orderReturnList == null || orderReturnList.Count == 0 || orderReturnList.Count > 1 || orderReturnList[0].dr==1)
                    return new ResultAudit() { Result = "F", Description = "退货单信息异常" };
                Hi.Model.DIS_OrderReturn orderReturn = orderReturnList[0];

                OrderReturnAudit auditModel = new OrderReturnAudit();
                switch (Act)
                {
                    case "0"://审核通过
                    {
                        Hi.Model.PAY_PrePayment payPrePayment = new Hi.Model.PAY_PrePayment();
                        payPrePayment.CompID = order.CompID;
                        payPrePayment.DisID = order.DisID;
                        payPrePayment.OrderID = order.ID;
                        payPrePayment.Start = 1;
                        payPrePayment.PreType = 4;
                        payPrePayment.price = order.PayedAmount;  //已支付金额
                        payPrePayment.Paytime = DateTime.Now;
                        payPrePayment.CrateUser = user.ID;
                        payPrePayment.CreatDate = DateTime.Now;
                        payPrePayment.OldId = 0;
                        payPrePayment.AuditState = 2;
                        payPrePayment.IsEnabled = 1;
                        payPrePayment.AuditUser = user.ID;
                        payPrePayment.dr = 0;
                        payPrePayment.ts = DateTime.Now;
                    
                        auditModel.prePayment = payPrePayment;
                    }
                        break;
                    case "1"://审核退回
                    {
                        order.ReturnState = (int)Enums.ReturnState.拒绝退货;
                        order.modifyuser = user.ID;

                        orderReturn.AuditUserID = user.ID;
                        orderReturn.AuditDate = DateTime.Now;
                        orderReturn.AuditRemark = AuditRemark;
                        orderReturn.ReturnState = (int)Enums.AuditState.退回;
                        orderReturn.ts = DateTime.Now;
                        orderReturn.modifyuser = user.ID;
                    }
                        break;
                    default:
                        return new ResultAudit() { Result = "F", Description = "操作异常" };
                        break;
                }
                auditModel.Act = Act;
                auditModel.order = order;
                auditModel.orderReturn = orderReturn;
                auditModel.AuditRemark = AuditRemark;
                auditModel.user = user;
                auditList.Add(auditModel);
            }

            #endregion

            int result = 0;
            if (auditList.Count > 0)
            {
                result = TransOrderReturnAudit(auditList);
                if (result == 0) 
                    return new ResultAudit() { Result = "F", Description = "操作失败", ReceiptNoList = list };
                foreach (OrderReturnAudit order in auditList)
                {
                    if (order.Act == "0")
                    {
                        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(order.order.DisID);
                        //经销商手机号
                        string Phone = dis.Phone;
                        string msg = order.orderReturn.ReturnState == (int) Enums.AuditState.已审
                            ? "您的订单：" + order.order.ReceiptNo + "退货申请已通过！"
                            : "您的订单：" + order.order.ReceiptNo + "退货金额已退回您的企业钱包账户,请查收！";
                        //退款向经销商推送信息提示
                        Common.SendMsg(Phone, msg);
                    }
                    new MsgSend().GetWxService("4", order.order.ID.ToString(), "0");
                }
                return new ResultAudit() { Result = "T", Description = "退货申请审核成功", ReceiptNoList = list };
            }
            else
            {
                return new ResultAudit() { Result = "F", Description = "操作列表为空" };
            }
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace , "AuditOrderReturn:" + JSon);
            return new ResultAudit() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultCheck GetOrderCheckList(string JSon)
    {
        try
        {
            string strsql = "";
            string RebateSum = "";
            string returnmsg = "";
            string goodsid = "";
            string goodsname = "";
            string goodsinfovalue = "";
            decimal rebate_sum = 0;
            int isint_out = 0;
            decimal num_deci = 0;
            int num_int = 0;
            string isint = string.Empty;
            int bflag = 0;
            string[] discount_price = null;
            DataTable dt_info ;
            ResultCheck ResultCheck = new ResultCheck();
            ResultCheck.Result = "T";
            ResultCheck.Description = "返回成功";

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count == 0 || JInfo["UserID"].ToString() == "" || JInfo["CheckOrderList"].ToString() == "" )
            {
                return new ResultCheck() { Result = "F", Description = "参数异常" };
            }
            string userID = JInfo["UserID"].ToString();
            

            
            List<Check> checkList = new List<Check>();

            #region checkList


            foreach (JsonData JList in JInfo["CheckOrderList"])
            {
                decimal TotalAmount = 0;//订单促销前总价
                string ReceptNO = JList["ReceptNO"].ToString();
                string CompID = JList["CompID"].ToString();
                string DisID = JList["DisID"].ToString();
                string Rebate = JList["Rebate"].ToString();
                Hi.Model.BD_Goods goods = null;
                Hi.BLL.BD_Goods bll_goods = new Hi.BLL.BD_Goods();
                rebate_sum += Rebate.ToDecimal();
                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(DisID)))
                    return new ResultCheck() { Result = "F", Description = "未找到用户信息" };
                int IsInve = Common.rdoOrderAudit("商品是否启用库存", CompID.ToInt()).ToInt(0);//判断此核心企业是否启用库存
                isint = Common.rdoOrderAudit("订单下单数量是否取整", CompID.ToInt());//判断此核心企业发货数量是否取整，0表示取整，0.00表示有两位小数
                Check check = new Check();
                check.ReceptNO = ReceptNO;

                if (bflag == 0)//因为传入的返利是所有订单的使用的总返利，所有订单的经销商ID和核心企业ID都相同，所以只要查询一次可用的返利金额
                {
                    //取出该经销商可用的返利金额
                    strsql = "select sum(EnableAmount) as RebateSum from BD_Rebate where DisID = '" + DisID + "' ";
                    strsql += " and StartDate <= '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    strsql += " and EndDate > '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' and isnull(RebateState,1) = 1";
                    strsql += " and isnull(dr,0) = 0 and compid = '" + CompID + "'";
                    RebateSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                    if (decimal.Parse(RebateSum) < rebate_sum)
                    {
                        ResultCheck.Result = "2";
                        ResultCheck.Description = "返利金额不足";
                        check.RebateAmount = decimal.Parse(RebateSum).ToString("F2");
                        checkList.Add(check);
                        ResultCheck.CheckList = checkList;
                        return ResultCheck;

                    }
                   
                    bflag = 1;
                }


                check.RebateAmount = Rebate;
                List<DIS_Order.OrderDetail> OrderList = new List<DIS_Order.OrderDetail>();//放库存足够的订单明细
                List<DIS_Order.OrderDetail> OrderList2 = new List<DIS_Order.OrderDetail>();//放库存不足的订单明细
                foreach (JsonData Jitem in JList["CheckList"])
                {
                    string SKUID = Jitem["SKUID"].ToString();
                    string Num = Jitem["Num"].ToString();
                    if (decimal.Parse(Num) <= 0)
                        return new ResultCheck() { Result ="F",Description ="商品数量必须大于0"};
                    decimal Num_pro = 0;
                    //商品数量需要取整的时候，要判断传入的数量是不是整数
                    if (isint == "0")
                    {
                        num_deci = decimal.Parse(Num);
                        num_int = (int)num_deci;
                        if (!int.TryParse(Num, out isint_out))
                        {
                            if (decimal.Parse(num_int.ToString()) != num_deci)
                            {
                                return new ResultCheck() { Result = "F", Description = "商品数量应为整数" };
                            }
                        }
                    }
                   

                    Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(SKUID));
                    DIS_Order.OrderDetail orderDetail = new DIS_Order.OrderDetail();
                    ////判断此规格属性的商品的库存是否足够
                    //if (IsInve == 0)
                    //{
                    //    strsql = "select Inventory from bd_goodsinfo where id = '" + SKUID + "' ";
                    //    decimal inv_sum = decimal.Parse(ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), ""));
                    //    //库存不足的话，需要提示商品名称规格属性的，跳出循环
                    //    if (inv_sum < Num.ToDecimal())
                    //    {
                    //        strsql = "select goodsid,valueinfo from bd_goodsinfo where id = '" + SKUID + "'";
                    //        dt_info = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                    //        goodsid = ClsSystem.gnvl(dt_info.Rows[0]["goodsid"], "");//商品ID
                    //        goodsinfovalue = ClsSystem.gnvl(dt_info.Rows[0]["valueinfo"], "");
                    //        strsql = "select goodsname from bd_goods where id = '" + goodsid + "' ";
                    //        goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");//商品名称
                    //        ResultCheck.Result = "3";

                    //        returnmsg += "" + goodsname + "" + goodsinfovalue + "的库存数量不足";//拼接返回信息
                    //        orderDetail.SKUID = SKUID;
                    //        orderDetail.NumEnable = inv_sum.ToString();//库存不足商品的可用量
                    //        OrderList2.Add(orderDetail);
                    //        break;


                    //    }
                    //}

                    #region orderDetail

                    orderDetail.ProductID = goodsInfo.GoodsID.ToString();
                    orderDetail.SKUID = SKUID;
                    orderDetail.Num = Num;
                    orderDetail.SalePrice = goodsInfo.SalePrice.ToString();
                    int ProID = 0;
                    decimal price = Common.GetProPrice(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(), goodsInfo.CompID.ToString(), out ProID);
                    orderDetail.IsPro = ProID == 0 ? "0" : "1";

                    #region 价格

                    string Price = string.Empty;
                    List<Hi.Model.BD_GoodsPrice> goodsPrice = new Hi.BLL.BD_GoodsPrice().GetList("",
                        " disID='" + DisID + "' and GoodsInfoID='" + SKUID + "' and ISNULL(dr,0)=0 and IsEnabled=1", "");
                    string pty = string.Empty;
                    string ppty = string.Empty;
                    string pid_num = string.Empty;
                    if (orderDetail.IsPro.Trim() == "1")//取促销价格
                    {
                        //Price = price.ToString();
                        string info = string.Empty;
                        Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(ProID);
                        if (pro != null)
                        {
                            List<Hi.Model.BD_PromotionDetail> dList = new Hi.BLL.BD_PromotionDetail().GetList("", " ProID=" + pro.ID + " and GoodInfoID ='" + goodsInfo.ID + "' and dr=0", "");
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
                                info = "商品数量满" + pro.Discount.ToString("0.00") + "赠" +
                                       dList[0].GoodsPrice.ToString("0.00") + dList[0].GoodsUnit;
                            }
                            else if (pro.Type == 1 && pro.ProType == 4)
                            {
                                info = "商品打折" + pro.Discount.ToString("0.00") + "%";
                            }
                        }
                        orderDetail.proInfo = new DIS_Order.PromotionInfo()
                        {
                            ProID = ProID.ToString(),
                            ProTitle = pro.ProTitle,
                            ProInfos = info,
                            Type = pro.Type.ToString(),
                            ProTpye = pro.ProType.ToString(),
                            Discount = pro.Discount.ToString("0.00"),
                            ProStartTime = pro.ProStartTime.ToString("yy-MM-dd"),
                            ProEndTime = pro.ProEndTime.ToString("yy-MM-dd")
                        };
                        
                        orderDetail.ProNum = Common.GetProNum(goodsInfo.GoodsID.ToString(), goodsInfo.ID.ToString(), goodsInfo.CompID,decimal.Parse(orderDetail.Num), out pty,out ppty,out pid_num).ToString();
                        Num_pro = decimal.Parse(orderDetail.ProNum);
                    }
                    #region//判断此规格属性的商品的库存是否足够
                    //if (IsInve == 0)
                    //{
                    //    strsql = "select Inventory from bd_goodsinfo where id = '" + SKUID + "' ";
                    //    decimal inv_sum = decimal.Parse(ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), ""));
                    //    //库存不足的话，需要提示商品名称规格属性的，跳出循环
                    //    if (inv_sum < Num.ToDecimal()+Num_pro)
                    //    {
                    //        strsql = "select goodsid,valueinfo from bd_goodsinfo where id = '" + SKUID + "'";
                    //        dt_info = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                    //        goodsid = ClsSystem.gnvl(dt_info.Rows[0]["goodsid"], "");//商品ID
                    //        goodsinfovalue = ClsSystem.gnvl(dt_info.Rows[0]["valueinfo"], "");
                    //        strsql = "select goodsname from bd_goods where id = '" + goodsid + "' ";
                    //        goodsname = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");//商品名称
                    //        ResultCheck.Result = "3";

                    //        returnmsg += "" + goodsname + "" + goodsinfovalue + "的库存数量不足";//拼接返回信息
                    //        orderDetail.SKUID = SKUID;
                    //        if (pty != "3")
                    //        {
                    //            orderDetail.NumEnable = inv_sum.ToString();//库存不足商品的可用量
                    //        }
                    //        else//商品满减的时候需要算出最多能买多少
                    //        {
                    //            //最大可购买数的算法
                    //            decimal sumenable = 0;
                    //            discount_price = ppty.Split(',');
                    //            //取出满多少件减
                    //            decimal discount = decimal.Parse(discount_price[3]);
                    //            //取出减多少件
                    //            decimal goodprice_pro = decimal.Parse(discount_price[2]);
                    //            //取出这种满减最多买多少组
                    //            int group = (int)(inv_sum/(discount+goodprice_pro));
                    //            //算出买最多组时，需要发货的商品数量
                    //            decimal group_num = group * (discount + goodprice_pro);
                    //            //买了最多组后，剩余的库存
                    //            decimal lift_num = inv_sum - group_num;
                    //            //能购买的商品最大数量（不算赠送的商品）
                    //            if (lift_num != discount)
                    //                sumenable = group * discount + lift_num;
                    //            else
                    //                sumenable = group * discount+lift_num -1;
                                
                    //            orderDetail.NumEnable = sumenable.ToString("0.00");
                    //        }
                    //        OrderList2.Add(orderDetail);
                    //        break;
                            

                    //    }
                    //}

                    #endregion
                    if (IsInve == 0 && goodsInfo.Inventory < Num.ToDecimal() + Num_pro)
                    {
                        goods = bll_goods.GetModel(goodsInfo.GoodsID);
                        ResultCheck.Result = "3";

                        returnmsg += "" + goods.GoodsName + "" + goodsInfo.ValueInfo + "的库存数量不足";//拼接返回信息
                        orderDetail.SKUID = SKUID;
                        if (pty != "3")
                        {
                            orderDetail.NumEnable = goodsInfo.Inventory.ToString();//库存不足商品的可用量
                        }
                        else//商品满减的时候需要算出最多能买多少
                        {
                            //最大可购买数的算法
                            decimal sumenable = 0;
                            discount_price = ppty.Split(',');
                            //取出满多少件减
                            decimal discount = decimal.Parse(discount_price[3]);
                            //取出减多少件
                            decimal goodprice_pro = decimal.Parse(discount_price[2]);
                            //取出这种满减最多买多少组
                            int group = (int)(goodsInfo.Inventory / (discount + goodprice_pro));
                            //算出买最多组时，需要发货的商品数量
                            decimal group_num = group * (discount + goodprice_pro);
                            //买了最多组后，剩余的库存
                            decimal lift_num = goodsInfo.Inventory - group_num;
                            //能购买的商品最大数量（不算赠送的商品）
                            if (lift_num != discount)
                                sumenable = group * discount + lift_num;
                            else
                                sumenable = group * discount + lift_num - 1;

                            orderDetail.NumEnable = sumenable.ToString("0.00");
                        }
                        OrderList2.Add(orderDetail);
                        break;
                    }
                    //else if (goodsPrice != null && goodsPrice.Count > 0)//普通商品，已经改价
                    //{
                    //    Price = goodsPrice[0].TinkerPrice.ToString();
                    //}
                    //else //普通商品，未改价
                    //{
                    //    Price = goodsInfo.TinkerPrice.ToString();
                    //}
                    Price = BLL.Common.GetGoodsPrice(Convert.ToInt32(CompID),Convert.ToInt32(DisID),Convert.ToInt32(SKUID)).ToString("0.00");


                    #endregion

                    orderDetail.TinkerPrice = Price;
                    orderDetail.Price = Price;
                    orderDetail.SumAmount = (Convert.ToDecimal(Price) * Convert.ToDecimal(orderDetail.Num)).ToString();
                    TotalAmount += Convert.ToDecimal(Price) * Convert.ToDecimal(orderDetail.Num);

                    #endregion

                    //即使有问题，也取值，只是Remark赋-1
                    if (!Common.IsOffline(Convert.ToInt32(SKUID)))
                    {
                        ResultCheck.Result = "1";
                        ResultCheck.Description = "商品:" + SKUID + "被禁用";
                        orderDetail.Remark = "-1";
                        check.CheckRemark = "-1";
                    }


                    OrderList.Add(orderDetail);
                }
                //if (ResultCheck.Result == "F")
                //{
                //    return ResultCheck;
                //}

                //如果有库存不足的，只需要将库存不足的那几条订单明细返回；
                if (OrderList2.Count > 0)
                {
                    ResultCheck.Description = returnmsg;
                    check.OrderDetailList = OrderList2;
                }
                else
                {
                    check.OrderDetailList = OrderList;
                }

                check.TotalAmount = TotalAmount.ToString();
                string orderPro = string.Empty;
                decimal orderProNum = Common.GetProPrice(TotalAmount, out orderPro, Convert.ToInt32(CompID));
                check.AuditTotalAmount = (TotalAmount - orderProNum - decimal.Parse(Rebate)).ToString();
                if (check.AuditTotalAmount.ToDecimal() < 0)
                {
                    return new ResultCheck()
                    {
                        Result = "F",
                        Description = "使用的返利金额大于订单总金额！"
                    };


                }
                check.IsOrderPro = orderProNum == 0 ? "0" : "1";
                if (orderProNum != 0)
                {
                    //List<BD_GoodsCategory.ResultOrderPro> list = Common.ReturnOrderProList(CompID);
                    //foreach (var item in list)
                    //{
                    //    if (Convert.ToDecimal(item.OrderPrice) <= TotalAmount)
                    //    {
                    //        check.OrderPro = item;
                    //    }
                    //}
                    BD_GoodsCategory.ResultOrderPro  orderpro = new BD_GoodsCategory.ResultOrderPro();
                    if (orderPro != "" && orderPro != "0")
                    {
                        orderpro.ProID = orderPro;
                        strsql = "select protype from BD_Promotion where id = '" + orderPro + "'";
                        orderpro.ProType = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                        orderpro.OrderPrice = orderProNum.ToString("0.00");
                        string ProIDD = "";
                        string ProType = "";
                        string pro_id = "";
                        decimal pro_num = Common.GetProPrice(TotalAmount, out pro_id, out ProIDD, out ProType, CompID.ToInt());
                        orderpro.Discount = Common.proOrderType(ProIDD, orderProNum.ToString("F2"), ProType);
                    }
                    else
                    {
                        orderpro.ProID = "0";
                        orderpro.ProType = "0";
                        orderpro.OrderPrice = "";
                        orderpro.Discount = "";
                    }
                    check.OrderPro = orderpro;

                }
                checkList.Add(check);
            }

            #endregion 

            ResultCheck.CheckList = checkList;
            return ResultCheck;
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "SubResellerOrder：" + JSon );
            return new ResultCheck() { Result = "F", Description = "参数异常" };
        }
    }

    #region 订单操作方法

    /// <summary>
    /// 新增订单和日志
    /// </summary>
    /// <returns></returns>
    public int TansOrder(Hi.Model.DIS_Order OrderInfoModel, List<Hi.Model.DIS_OrderDetail> OrderDetailList)
    {
        int OrderId = 0;
        Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
        Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

        SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
        con.Open();
        SqlTransaction sqlTrans = con.BeginTransaction();
        //可以做循环   
        try
        {
            OrderId = OrderBll.AddOrder(con, OrderInfoModel, sqlTrans);
            if (OrderId == 0)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }
            foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
            {
                item.OrderID = OrderId;
                int count = OrderDetailBll.AddOrderDetail(con, item, sqlTrans);
                if (count == 0)
                {
                    OrderId = 0;
                    sqlTrans.Rollback();
                }
            }

            Hi.Model.SYS_Users userone = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(OrderInfoModel.CreateUserID));
            if (userone == null)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }

            string str = Common.AddSysBusinessLog(OrderInfoModel, userone, "Order", OrderId.ToString(),
                "订单新增", "订单新增", sqlTrans);
            if (str == "0" || str == "false")
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }

            sqlTrans.Commit();
        }
        catch
        {
            OrderId = 0;
            sqlTrans.Rollback();
        }
        finally
        {
            con.Close();
        }

        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(OrderInfoModel.CreateUserID);
        if (user != null)
        {
            string userType = string.Empty;
            string sendType = string.Empty;

            if (user.Type == 1 || user.Type == 5)//经销商
            {
                userType = "1";
                sendType = "1";
            }

            if (user.Type == 3 || user.Type == 4)
            {
                userType = "0";
                sendType = "41";
            }
            new MsgSend().GetWxService(sendType, OrderInfoModel.ID.ToString(), userType);
        }

        return OrderId;
    }

    /// <summary>
    /// 修改订单和日志
    /// </summary>
    /// <returns></returns>
    public int UpdateOrder(Hi.Model.DIS_Order OrderInfoModel, List<Hi.Model.DIS_OrderDetail> OrderDetailList)
    {
        int OrderId = 0;
        Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
        Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

        SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
        con.Open();
        SqlTransaction sqlTrans = con.BeginTransaction();
        //可以做循环   

        try
        {
            int count = 0;
            OrderId = OrderBll.UpdateOrder(con, OrderInfoModel, sqlTrans);
            if (OrderId == 0)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }
            foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
            {
                Hi.Model.DIS_OrderDetail OrderDeModel = OrderDetailBll.GetModel(item.ID);

                if (OrderDeModel != null)
                {
                    count = OrderDetailBll.UpdateOrderDetail(con, item, sqlTrans);
                    if (count == 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }
                else
                {
                    item.OrderID = OrderInfoModel.ID;
                    count = OrderDetailBll.AddOrderDetail(con, item, sqlTrans);
                    if (count == 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }
            }

            Hi.Model.SYS_Users userone = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(OrderInfoModel.CreateUserID));
            if (userone == null)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }

            string str = Common.AddSysBusinessLog(OrderInfoModel, userone, "Order", OrderId.ToString(),
                "订单修改", "", sqlTrans);
            if (str == "0" || str == "false")
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }

            sqlTrans.Commit();
        }
        catch
        {
            OrderId = 0;
            sqlTrans.Rollback();
        }
        finally
        {
            con.Close();
        }

        return OrderId;
    }

    /// <summary>
    /// 批量操作订单:新增或修改
    /// </summary>
    /// <param name="orderList"></param>
    /// <returns></returns>
    public int TransOrderList(List<OrderList> orderList)
    {
        List<int> IDList = new List<int>();
        int OrderId = 0;
        int OrderExtId = 0;
        Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
        Hi.BLL.DIS_OrderExt OrderExtBll = new Hi.BLL.DIS_OrderExt();
        Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();
        List<Hi.Model.DIS_Order> list = new List<Hi.Model.DIS_Order>();
        System.Text.StringBuilder sqlInven = new System.Text.StringBuilder();
        string strsql = string.Empty;
        string ts_rebate = string.Empty;

        SqlConnection con = new SqlConnection(SqlHelper.LocalSqlServer);
        con.Open();
        SqlTransaction sqlTrans = con.BeginTransaction();
        //可以做循环   
        try
        {
            foreach (OrderList order in orderList)
            {
                Hi.Model.DIS_Order OrderInfoModel = order.order;
                list.Add(OrderInfoModel);
                List<Hi.Model.DIS_OrderDetail> OrderDetailList = order.orderDetail;
                Hi.Model.DIS_OrderExt orderext = order.orderext;
                string str = string.Empty;
                int count = 0;
                int IsInve = Common.rdoOrderAudit("商品是否启用库存", OrderInfoModel.CompID).ToInt(0);
                string LogRemark = " 下单总价：" + OrderInfoModel.TotalAmount.ToString("N");
                if (ClsSystem.gnvl(orderext.ProID,"") != "0" && ClsSystem.gnvl(orderext.ProID,"") != "")
                {
                    LogRemark += " ,订单促销：" + orderext.ProAmount;
                }
                if (order.act == "0") //新增
                {
                    OrderId = OrderBll.AddOrder(con, OrderInfoModel, sqlTrans);//插入dis_order表
                    if (OrderId == 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                        return OrderId;
                    }
                    orderext.OrderID = OrderId;
                    OrderExtId = OrderExtBll.Add(con, orderext, sqlTrans);//插入dis_orderext表
                    if (OrderExtId == 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                        return OrderId;
                    }
                    if (OrderDetailList.Count <= 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                        return OrderId;
                    }
                    else
                    {
                        foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
                        {
                            item.OrderID = OrderId;
                            count = OrderDetailBll.AddOrderDetail(con, item, sqlTrans);
                            if (count == 0)
                            {
                                OrderId = 0;
                                sqlTrans.Rollback();
                            }
                        }


                        Hi.Model.SYS_Users userone = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(OrderInfoModel.CreateUserID));
                        if (userone == null)
                        {
                            OrderId = 0;
                            sqlTrans.Rollback();
                        }
                        //回写返利
                        if (Common.rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
                        {
                            //订单支付返利启用
                            if (OrderInfoModel.bateAmount > 0)
                            {
                                Hi.BLL.BD_Rebate bate = new Hi.BLL.BD_Rebate();
                                //strsql = "select MAX(ts) from BD_Rebate where DisID = '" + OrderInfoModel.DisID + "' ";
                                //strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
                                //strsql += " and isnull(RebateState,1) = 1";
                                //strsql += " and isnull(dr,0) = 0 and compid = '" + OrderInfoModel.CompID + "'";
                                //ts_rebate = SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer).ToString();
                                //if (ts_rebate != order.ts_rebate)
                                //{
                                //    OrderId = 0;
                                //    sqlTrans.Rollback();
                                //    return OrderId;
                                //}
                                //使用返利大于0;
                                if (bate.TransRebate(OrderInfoModel.DisID, OrderInfoModel.bateAmount, OrderId, OrderInfoModel.CreateUserID, sqlTrans))
                                {
                                    //sqlTrans.Commit();
                                    //return OrderId;
                                }
                                else
                                {

                                    OrderId = 0;
                                    sqlTrans.Rollback();
                                    return OrderId;
                                }
                            }
                        }
                        if (IsInve == 0)
                        {
                 
                            //新增订单，减商品库存
                            sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory("", OrderDetailList));
                            if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), sqlTrans.Connection, sqlTrans) <= 0)
                            {
                                OrderId = 0;
                                sqlTrans.Rollback();
                            }
                        }

                        str = Common.AddSysBusinessLog(OrderInfoModel, userone, "Order", OrderId.ToString(),
                            "订单新增", LogRemark, sqlTrans);
                        if (str == "0" || str == "false")
                        {
                            OrderId = 0;
                            sqlTrans.Rollback();
                        }
                        IDList.Add(OrderId);
                    }
                }
                else //修改
                {
                    OrderId = OrderBll.UpdateOrder(con, OrderInfoModel, sqlTrans);
                    foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
                    {
                        Hi.Model.DIS_OrderDetail OrderDeModel = OrderDetailBll.GetModel(item.ID);
                        if (OrderDeModel != null)
                        {
                            OrderDetailBll.UpdateOrderDetail(con, item, sqlTrans);
                        }
                        else
                        {
                            item.OrderID = OrderInfoModel.ID;
                            OrderDetailBll.AddOrderDetail(con, item, sqlTrans);
                        }
                    }

                    Hi.Model.SYS_Users userone = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(OrderInfoModel.modifyuser));
                    if (userone == null)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }

                    str = Common.AddSysBusinessLog(OrderInfoModel, userone, "Order", OrderId.ToString(),
                        "订单修改", LogRemark, sqlTrans);
                    if (str == "0" || str == "false")
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }
                
            }


            sqlTrans.Commit();
            foreach (int orderID in IDList)
            {
                MsgSend.Jpushdega jpushdega = new MsgSend.Jpushdega(new MsgSend().GetWxService);
                jpushdega.BeginInvoke("1", orderID.ToString(), "1",0,null,null);

                //new MsgSend().GetWxService("1", orderID.ToString(), "1");
            }
        }
        catch
        {
            OrderId = 0;
            sqlTrans.Rollback();
        }
        finally
        {
            con.Close();
            sqlTrans.Dispose();
        }

        //推送
        foreach (Hi.Model.DIS_Order order in list)
        {
            //根据order.CreateUserID判断 下单用户类型
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(order.CreateUserID);
            if (user != null)
            {
                string userType = string.Empty;
                string sendType = string.Empty;

                if (user.Type == 1 || user.Type == 5)//经销商
                {
                    userType = "1";
                    sendType = "1";
                }

                if (user.Type == 3 || user.Type == 4)
                {
                    userType = "0";
                    sendType = "41";
                }
                new MsgSend().GetWxService(sendType,order.ID.ToString(),userType);
            }
        }
    
        return OrderId;
    }

    /// <summary>
    /// 批量申请退货操作
    /// </summary>
    /// <param name="orderList"></param>
    /// <returns></returns>
    public int TransOrderReturnList(List<OrderReturnList> orderReturnList)
    {
        SqlTransaction TranSaction = null;
        try
        {
            SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();

            foreach (OrderReturnList order in orderReturnList)
            {
                //退货修改订单状态
                order.order.ReturnState = (int)Enums.ReturnState.申请退货;
                order.order.modifyuser = order.user.ID;
                order.order.ts = DateTime.Now;

                bool res = new Hi.BLL.DIS_Order().Update(order.order, TranSaction);
                if (!res)
                {
                    TranSaction.Rollback();
                    return 0;
                }
                //新增退货信息
                int count = new Hi.BLL.DIS_OrderReturn().Add(Connection, order.orderReturn, TranSaction);
                if (count == 0)
                {
                    TranSaction.Rollback();
                    return 0;
                }

                string str = Common.AddSysBusinessLog(order.order, order.user, "Order", order.order.ID.ToString(), "申请退货", "", TranSaction);
                if (str == "0" || str == "false")
                {
                    TranSaction.Rollback();
                    return 0;
                }
            }

            TranSaction.Commit();
            return 1;
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
            return 0;
        }
        finally
        {
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Connection.Close();
                    TranSaction.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// 批量审核退货操作
    /// </summary>
    /// <param name="orderReturnList"></param>
    /// <returns></returns>
    public int TransOrderReturnAudit(List<OrderReturnAudit> orderReturnList)
    {
        SqlTransaction TranSaction = null;
        int IsInve = 0;
        StringBuilder sqlInven = null;
        try
        {
            SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();

            foreach (OrderReturnAudit order in orderReturnList)
            {
                switch (order.Act)
                {
                    case "0":
                    {
                        #region 退回审核通过

                        int State;
                        int pay = order.order.PayState;
                        int prepay = 0;

                        if (order.order.Otype == (int) Enums.OType.赊销订单)
                        {
                            if (order.order.PayState == (int) Enums.PayState.已支付 || pay == (int) Enums.PayState.部分支付 ||
                                order.order.PayState == (int) Enums.PayState.已结算)
                            {

                                //State = (int)Enums.AuditState.已审;
                                State = (int) Enums.AuditState.已完结;
                                order.order.OState = (int) Enums.OrderState.已退货;
                                order.order.PayState = (int) Enums.PayState.已退款;
                                order.order.ReturnState = (int) Enums.ReturnState.退货退款;
                            }
                            else
                            {
                                order.order.OState = (int) Enums.OrderState.已退货;
                                State = (int) Enums.AuditState.已审;
                            }
                        }
                        else
                        {
                            //OrderModel.OState = (int)Enums.OrderState.退货处理;
                            //State = (int)Enums.AuditState.已审;
                            if (order.order.PayState == (int) Enums.PayState.已支付 || pay == (int) Enums.PayState.部分支付 ||
                                order.order.PayState == (int) Enums.PayState.已结算)
                            {
                                order.order.OState = (int) Enums.OrderState.已退货;
                                order.order.PayState = (int) Enums.PayState.已退款;
                                order.order.ReturnState = (int) Enums.ReturnState.退货退款;

                                State = (int) Enums.AuditState.已完结;
                            }
                            else
                            {
                                order.order.OState = (int)Enums.OrderState.已退货;
                                State = (int)Enums.AuditState.已审;
                                order.order.ReturnState = (int)Enums.ReturnState.退货退款;
                            }
                        }
                        order.order.ts = DateTime.Now;
                        order.order.modifyuser = order.user.ID;

                        order.orderReturn.AuditUserID = order.user.ID;
                        order.orderReturn.AuditDate = DateTime.Now;
                        order.orderReturn.AuditRemark = order.AuditRemark;
                        order.orderReturn.ReturnState = State;
                        order.orderReturn.ts = DateTime.Now;
                        order.orderReturn.modifyuser = order.user.ID;

                        int countOne = new Hi.BLL.DIS_Order().UpdateOrderByggh(Connection, order.order, TranSaction,
                            order.orderReturn.ID, (int) Enums.AuditState.已完结);
                        if (countOne == 0)
                        {
                            TranSaction.Rollback();
                            return 0;
                        }
                        int countTwo = ReturnOrderUpdate(Connection, TranSaction, order.orderReturn, order.order);
                        if (countTwo == 0)
                        {
                            TranSaction.Rollback();
                            return 0;
                        }



                        //支付的订单 生成企业钱包
                        if (pay == (int) Enums.PayState.已支付 || pay == (int) Enums.PayState.部分支付 ||
                            pay == (int) Enums.PayState.已结算)
                        {
                            prepay = new Hi.BLL.PAY_PrePayment().InsertPrepay(Connection, order.prePayment, TranSaction);
                        }
                        else
                        {
                            //未支付的订单 不生成企业钱包
                            prepay = 1;
                        }

                        if (prepay == 0)
                        {
                            TranSaction.Rollback();
                            return 0;
                        }

                        string str = Common.AddSysBusinessLog(order.order, order.user, "Order", order.order.ID.ToString(),
                            "退货审核通过", "", TranSaction);
                        if (str == "0" || str == "false")
                        {
                            TranSaction.Rollback();
                            return 0;
                        }
                        //开启库存需要退回订单商品库存
                        IsInve = Common.rdoOrderAudit("商品是否启用库存",order.order.CompID).ToInt(0);
                        if (IsInve == 0)
                        {
                            sqlInven = new StringBuilder();
                            sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory("OrderID = " + order.order.ID + " and ISNULL(dr,0) =0 ", null));
                            if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), TranSaction.Connection, TranSaction) <= 0)
                            {

                                TranSaction.Rollback();
                                return 0;
                            }
                        }
                        #endregion
                    }
                        break;
                    case "1":
                    {
                        #region 退货审核退回

                        //修改订单状态
                        bool res = new Hi.BLL.DIS_Order().Update(order.order, TranSaction);
                        if (!res)
                        {
                            TranSaction.Rollback();
                            return 0;
                        }
                        //修改退货信息
                        int count = new Hi.BLL.DIS_OrderReturn().Update(Connection, order.orderReturn, TranSaction);
                        if (count == 0)
                        {
                            TranSaction.Rollback();
                            return 0;
                        }

                        string str = Common.AddSysBusinessLog(order.order, order.user, "Order", order.order.ID.ToString(),
                            "退货审核退回", "", TranSaction);
                        if (str == "0" || str == "false")
                        {
                            TranSaction.Rollback();
                            return 0;
                        }

                        #endregion
                    }
                        break;
                    default:
                    {
                        TranSaction.Rollback();
                        return 0; 
                    }
                        break;
                }
            }

            TranSaction.Commit();
            return 1;
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
            return 0;
        }
        finally
        {
            if (TranSaction != null)
            {
                if (TranSaction.Connection != null)
                {
                    TranSaction.Connection.Close();
                    TranSaction.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// 退货审核用的
    /// </summary>
    /// <param name="sqlconn"></param>
    /// <param name="sqltans"></param>
    /// <param name="ReturnModel"></param>
    /// <param name="OrderModel"></param>
    /// <returns></returns>
    public int ReturnOrderUpdate(SqlConnection sqlconn, SqlTransaction sqltans, Hi.Model.DIS_OrderReturn ReturnModel, Hi.Model.DIS_Order OrderModel)
    {

        SqlTransaction TranSaction = null;
        try
        {
            SqlConnection Connection = sqlconn;
            TranSaction = sqltans;

            //修改订单状态
            bool res = new Hi.BLL.DIS_Order().Update(OrderModel, TranSaction);
            if (!res)
            {
                //TranSaction.Rollback();
                return 0;
            }
            //修改退货信息
            int count = new Hi.BLL.DIS_OrderReturn().Update(Connection, ReturnModel, TranSaction);
            if (count == 0)
            {
                //TranSaction.Rollback();
                return 0;
            }

            //TranSaction.Commit();
            return count;

        }
        catch
        {
            //if (TranSaction != null)
            //{
            //    if (TranSaction.Connection != null)
            //    {
            //        TranSaction.Rollback();
            //    }
            //}
            return 0;
        }
        finally
        {
            //if (TranSaction != null)
            //{
            //    if (TranSaction.Connection != null)
            //    {
            //        TranSaction.Connection.Close();
            //    }
            //}
        }
        return 0;
    }

    #endregion

    #region 返回

    public class ResultCheck
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string RebateAmount { get; set; }
        public List<Check> CheckList { get; set; }
    }

    public class Check
    {
        public string ReceptNO { get; set; }
        public List<DIS_Order.OrderDetail> OrderDetailList { get; set; }
        public string TotalAmount { get; set; }
        public string AuditTotalAmount { get; set; }
        
        public string IsOrderPro { get; set; }
        public BD_GoodsCategory.ResultOrderPro OrderPro { get; set; }
        
        public string CheckRemark { get; set; }
        public string RebateAmount { get; set; }//可用返利金额
        //public string SaleAmount { get; set; }//促销金额
        //public string SaleDetails { get; set; }//促销明细
    }

    public class OrderReturnAudit
    {
        public string Act { get; set; }
        public Hi.Model.SYS_Users user { get; set; }
        public Hi.Model.DIS_Order order { get; set; }
        public string AuditRemark { get; set;}
        public Hi.Model.DIS_OrderReturn orderReturn { get; set; }
        public Hi.Model.PAY_PrePayment prePayment { get; set; }
    }

    public class OrderReturnList
    {
        public Hi.Model.SYS_Users user { get; set; }
        public Hi.Model.DIS_Order order { get; set; }
        public Hi.Model.DIS_OrderReturn orderReturn  { get; set; }
    }

    public class OrderList
    {
        public string act { get; set; } //0新增  1修改
        public Hi.Model.DIS_Order order { get; set; }
        public List<Hi.Model.DIS_OrderDetail> orderDetail { get; set; }
        public Hi.Model.DIS_OrderExt orderext { get; set; }//订单拓展表的Model
        public string ts_rebate { get; set; }//返利单时间戳
    }

    public class ResultSubOrder
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string IsCheck { get; set; }
        
        public List<DIS_Order.Order> OrderList { get; set; }
    }

    public class Order
    {
        public string CompID { get; set; }
        public string ReceiptNo { get; set; }
        public string SubType { get; set; }
        public string Otype { get; set; }
        public string AddrID { get; set; }
        public string OrderRemark { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }
    }

    public class OrderDetail
    {
        public string ProductID { get; set; }
        public string SKUID { get; set; }
        public string Num { get; set; }
        public string Remark { get; set; }
    }

    public class ResultAudit
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<string> ReceiptNoList { get; set; }
    }

    #endregion
}