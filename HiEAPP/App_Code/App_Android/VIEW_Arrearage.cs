using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;

/// <summary>
///VIEW_ArrearageRpt 的摘要说明
/// </summary>
public class VIEW_Arrearage
{
	public VIEW_Arrearage()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 应收应付账款列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultDisAccountList GetDisAccountList(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string Type = string.Empty; // 0：企业  1:经销商
            string BusinessID = string.Empty; // 根据Type存放 disID 或者 CompID

            string criticalOrderID = string.Empty; //当前列表临界点的DisID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;

            string compID = string.Empty;
            string orderType = string.Empty;//单据类型   1:订单 2:账单

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["Type"].ToString() != "" &&
                JInfo["BussID"].ToString().Trim() != "" && JInfo["CriticalOrderID"].ToString() != "" &&
                JInfo["GetType"].ToString().Trim() != "" && JInfo["Rows"].ToString() != "" &&
                JInfo["Sort"].ToString().Trim() != "" && JInfo["SortType"].ToString() != "" && JInfo["orderType"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                Type = JInfo["Type"].ToString();
                BusinessID = JInfo["BussID"].ToString();

                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString(); //1:更多 else 老数据
                rows = JInfo["Rows"].ToString();
                sort = JInfo["Sort"].ToString();
                sortType = JInfo["SortType"].ToString();
                orderType = JInfo["orderType"].ToString();

            }
            else
            {
                return new ResultDisAccountList() {Result = "F", Description = "参数为空异常"};
            }

            string strWhere = string.Empty;
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (Type == "0")
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out user, int.Parse(BusinessID)))
                return new ResultDisAccountList() { Result = "F", Description = "用户异常" };
            }
            else
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0, int.Parse(BusinessID)))
                return new ResultDisAccountList() { Result = "F", Description = "用户异常" };
            }
            
            switch (Type)
            {
                case "0":
                {
                    compID = BusinessID;
                    strWhere += " and CompID='" + BusinessID + "'";
                }
                    break;
                case "1":
                {
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(UserID));
                    if(dis==null||dis.dr==1||dis.IsEnabled==0) return new ResultDisAccountList { Result = "F", Description = "经销商异常" };
                    compID = dis.CompID.ToString();
                    strWhere += " and disID='" + BusinessID + "'";
                }
                    break;
                default:
                    return new ResultDisAccountList() {Result = "F", Description = "参数为空异常"};
            }

            #endregion

            #region 模拟分页
            string tabName=string.Empty;
            if(orderType=="1")
                tabName = " [dbo].[Api_Arrearage]"; //订单表名
            else
                tabName = " [dbo].[Api_Arrearage_ZD]"; //账单应收

            string strsql = string.Empty; //搜索sql
            sortType = sortType == "0" ? "AuditAmount" : "DisID"; //orderBy
            sort = sort == "1" ? " asc" : " desc";

            strsql = new Common().PageSqlString(criticalOrderID, "disID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResultDisAccountList() {Result = "F", Description = "基础数据异常"};

            #endregion

            #region 赋值

            List<DisAccountModel> disAccountList = new List<DisAccountModel>();

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResultDisAccountList() {Result = "F", Description = "没有更多数据"};
            DataTable orderList = ds.Tables[0];
            if (orderList == null)
                return new ResultDisAccountList() {Result = "F", Description = "没有更多数据"};
            if (orderList.Rows.Count == 0)
                return new ResultDisAccountList() {Result = "F", Description = "没有更多数据"};
            foreach (DataRow row in orderList.Rows)
            {
                DisAccountModel disAccount = new DisAccountModel();

                List<Hi.Model.BD_Distributor> disList = new Hi.BLL.BD_Distributor().GetList("",
                    " ID='" + row["DisID"].ToString() + "'", "");
                if (disList == null || disList.Count == 0 || disList.Count > 1)
                    return new ResultDisAccountList() {Result = "F", Description = "经销商数据异常"};

                disAccount.DisID = row["DisID"].ToString();
                disAccount.DisName = disList[0].DisName;
                disAccount.AuditAmount = row["AuditAmount"].ToString();

                //disAccount.DisAccount = row["DisAccount"].ToString();
                disAccount.DisAccount = new Hi.BLL.PAY_PrePayment().sums(Convert.ToInt32(disAccount.DisID), Convert.ToInt32(compID)).ToString();

                disAccount.YearOne = row["year1"].ToString();
                disAccount.YearTwo = row["year2"].ToString();
                disAccount.YearThree = row["year3"].ToString();
                //todo:
                decimal netAccount = decimal.Parse(disAccount.AuditAmount) - decimal.Parse(disAccount.DisAccount);
                disAccount.NetAccount = netAccount > 0 ? netAccount.ToString() : "0";
                disAccountList.Add(disAccount);
            }
            return new ResultDisAccountList()
            {
                Result = "T",
                Description = "获取成功",
                DisAccountList = disAccountList
            };

            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetDisAccountList:" + JSon);
            return new ResultDisAccountList() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 应收应付账款订单列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public DIS_Order.ResultOrderList GetDisAccountInfo(string JSon)
    {
        try
        {
            string strWhere = " and Ostate in (2,4,5) and PayState in (0,1)";

            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;

            string orderType = string.Empty;//单据类型   1:订单 2:账单

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" && JInfo["Sort"].ToString() != "" && JInfo["orderType"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                strWhere += " and DisID='" + disID + "' and ISNULL(dr,0)=0";
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                orderType = JInfo["orderType"].ToString();
            }
            else
            {
                return new DIS_Order.ResultOrderList() { Result = "F", Description = "参数异常" };
            }

            if(orderType=="1")//订单
               strWhere += " and ISNULL(dr,0)=0 and OState != 0 and Otype!=9";
            else//账单
                strWhere += " and ISNULL(dr,0)=0 and OState != 0 and Otype=9";

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
                    strWhere += " and CreateDate < " + Convert.ToDateTime(JInfo["EndDate"].ToString()).AddDays(1) + "'";
                }
                //根据出库单编号 锁定 订单ID
                if (JMsg["ExpressNo"].ToString() != "")
                {
                    //strWhere += " and ReceiptNo like '" + JMsg["ExpressNo"].ToString() + "%'";
                    List<Hi.Model.DIS_OrderOut> orderOut = new Hi.BLL.DIS_OrderOut().GetList("",
                        " ExpressNo like '%" + JMsg["ExpressNo"].ToString() + "%'", "");
                    if (orderOut.Count == 0)
                        return new DIS_Order.ResultOrderList() { Result = "F", Description = "参数异常" };
                    strWhere += " and ID in ( 0";
                    strWhere = orderOut.Aggregate(strWhere, (current, aout) => current + ("," + aout.OrderID));
                    strWhere += " )";
                }
            }

            #endregion

            #region 模拟分页

            string tabName = " [dbo].[DIS_Order]"; //表名
            string strsql = string.Empty; //搜索sql
            sortType = "ID";
            
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new DIS_Order.ResultOrderList() { Result = "F", Description = "基础数据异常" };

            #endregion

            #region 赋值

            string SKUName = string.Empty;
            List<DIS_Order.Order> OrderList = new List<DIS_Order.Order>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new DIS_Order.ResultOrderList() { Result = "F", Description = "没有更多数据" };
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                    return new DIS_Order.ResultOrderList() { Result = "F", Description = "没有更多数据" };
                foreach (DataRow row in orderList.Rows)
                {
                    DIS_Order.Order order = new DIS_Order.Order();
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(int.Parse(row["ID"].ToString()));
                    order.OrderID = orderModel.ID.ToString();
                    order.CompID = orderModel.CompID.ToString();
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(orderModel.CompID);
                    if (comp == null || comp.IsEnabled == 0 || comp.dr == 1)
                        return new DIS_Order.ResultOrderList() { Result = "F", Description = "企业异常" };
                    order.CompName = comp.CompName;
                    order.State = Common.GetDisOrderType(orderModel.OState, orderModel.PayState, orderModel.Otype,
                        orderModel.ReturnState);
                    order.OState = orderModel.OState.ToString();
                    order.AddType = orderModel.AddType.ToString();
                    order.Otype = orderModel.Otype.ToString();
                    order.PayState = orderModel.PayState.ToString();
                    order.ReturnState = orderModel.ReturnState.ToString();
                    order.DisID = orderModel.DisID.ToString();
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(orderModel.DisID);
                    if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                        return new DIS_Order.ResultOrderList() { Result = "F", Description = "经销信息商异常" };
                    order.DisName = dis.DisName;
                    order.DisUserID = orderModel.DisUserID.ToString();
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(userID));
                    if (user == null || user.IsEnabled == 0 || user.dr == 1)
                        return new DIS_Order.ResultOrderList() { Result = "F", Description = "经销商用户信息异常" };
                    order.DisUserName = user.TrueName;
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
                        //根据发货单取对应的物流信息
                        List<Hi.Model.DIS_Logistics> list_log = new Hi.BLL.DIS_Logistics().GetList("", "OrderOutID = " + orderOut.ID + " and isnull(dr,0) = 0", "");
                        //一个发货单只有一条物流信息，所以list_log里也只有一条数据
                        Hi.Model.DIS_Logistics model_log = list_log[0];
                        order.Express = Convert.ToString(model_log.ComPName);
                        order.ExpressNo = Convert.ToString(model_log.LogisticsNo);
                        //order.ExpressPerson = Convert.ToString(orderOut.ExpressPerson);
                        //order.ExpressTel = Convert.ToString(orderOut.ExpressTel);
                        //order.ExpressBao =Convert.ToString(orderOut.ExpressBao);
                        order.PostFee = orderModel.PostFee.ToString("0.00");
                        order.ActionUser = Convert.ToString(orderOut.ActionUser);
                        order.SendRemark = Convert.ToString(orderOut.Remark);
                        order.IsAudit = Convert.ToString(orderOut.IsAudit);
                        order.AuditUserID = Convert.ToString(orderOut.AuditUserID);
                        order.AuditDate = Convert.ToString(orderOut.AuditDate);
                        order.AuditRemark = orderOut.AuditRemark == null ? "" : orderOut.AuditRemark.ToString();
                        order.SignDate = Convert.ToString(orderOut.SignDate);
                        order.IsSign = Convert.ToString(orderOut.IsSign);
                        order.SignUserId = Convert.ToString(orderOut.SignUserId);
                        order.SignUser = Convert.ToString(orderOut.SignUser);
                        order.SignRemark =Convert.ToString(orderOut.SignRemark);
                    }
                    order.SendRemark = Convert.ToString(orderModel.Remark);
                    //todo:不知道的排序
                    //order.SortIndex = orderModel.SortIndex.ToString();               
                    order.IsDel = Convert.ToString(orderModel.dr);

                    //明细
                    List<DIS_Order.OrderDetail> orderDetail = new List<DIS_Order.OrderDetail>();
                    List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("",
                        " OrderID='" + orderModel.ID + "' and DisID='" + orderModel.DisID + "' and ISNULL(dr,0)=0", "");
                    if (detailList == null) //|| detailList.Count==0 没有明细的单 PC可以新建
                        return new DIS_Order.ResultOrderList() { Result = "F", Description = "订单明细异常" };
                    List<Hi.Model.BD_GoodsAttrs> list_attrs = null;
                    foreach (Hi.Model.DIS_OrderDetail detail in detailList)
                    {
                        DIS_Order.OrderDetail ordetail = new DIS_Order.OrderDetail();
                        ordetail.SKUID = detail.GoodsinfoID.ToString();
                        //通过GoodsInfoID找到GoodsID
                        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
                        if (goodsInfo == null)
                            //if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1)
                            return new DIS_Order.ResultOrderList() { Result = "F", Description = "SKU信息异常" };
                        ordetail.ProductID = goodsInfo.GoodsID.ToString();

                        //通过GoodsID找到GoodsName
                        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                        if (goods == null)
                            //if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
                            return new DIS_Order.ResultOrderList() { Result = "F", Description = "商品异常" };
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

                        List<DIS_Order.Pic> Pic = new List<DIS_Order.Pic>();
                        if (goods.Pic.ToString() != "" && goods.Pic.ToString() != "X")
                        {
                            DIS_Order.Pic pic = new DIS_Order.Pic();
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
                return new DIS_Order.ResultOrderList() { Result = "F", Description = "没有更多数据" };
            }

            #endregion

            return new DIS_Order.ResultOrderList()
            {
                Result = "T",
                Description = "获取成功",
                OrderList = OrderList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetDisAccountInfo:" + JSon);
            return new DIS_Order.ResultOrderList() {Result = "F", Description = "参数异常"};
        }

    }

    #region 返回

    public class ResultDisAccountList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<DisAccountModel> DisAccountList { get; set; }
    }

    public class DisAccountModel
    {
        public string DisID { get; set; }
        public string DisName { get; set; }
        public string AuditAmount { get; set; } //应收总款
        public string YearOne { get; set; } //一年内应收款
        public string YearTwo { get; set; } //两年内应收款
        public string YearThree { get; set; } //三年及以上应收款
        public string DisAccount { get; set; }  //企业钱包余额
        public string NetAccount { get; set; }  //轧差
    }

    #endregion
}