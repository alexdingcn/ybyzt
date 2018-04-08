using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using System.Data;
using DBUtility;
using Hi.Model;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
///GetCompyBriefingList 的摘要说明
/// </summary>
public class GetCompyBriefingList
{
    public GetCompyBriefingList()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public ResultOrderList GetBriefingList(string JSon)
    {
        try
        {
            string UserID = string.Empty;
            string CompID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string datetype = string.Empty;
            string ordertype = string.Empty;
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != "" &&
                 JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                    JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                    JInfo["Sort"].ToString() != "" && JInfo["DateType"].ToString() != "" && JInfo["OrderType"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                datetype = JInfo["DateType"].ToString();
                ordertype = JInfo["OrderType"].ToString();
            }
            else
            {
                return new ResultOrderList() { Result = "F", Description = "参数异常" };
            }
            #endregion
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(CompID == "" ? "0" : CompID)))
                return new ResultOrderList() { Result = "F", Description = "登录信息异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultOrderList() { Result = "F", Description = "核心企业异常" };

            DateTime date = DateTime.Now;
            DateTime date_start = new DateTime();
            DateTime date_end = date.AddDays(1);
            //string strdate_start = string.Empty;
            // string strdate_end = date_end.ToString("yyyy-MM-dd");

            string strwhere = "";
            switch (datetype)
            {
                case "0":
                    date_start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    //strdate_start = date_start.ToString("yyyy-MM-dd");
                    break;
                case "1":
                    DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));
                    date_start = new DateTime(startWeek.Year, startWeek.Month, startWeek.Day, 0, 0, 0);
                    //strdate_start = date_start.ToString("yyyy-MM-dd");
                    break;
                case "2":
                    date_start = new DateTime(date.Year, date.Month, 1);
                    //strdate_start = date_start.ToString("yyyy-MM-dd");
                    break;
                default:
                    return new ResultOrderList() { Result = "F", Description = "时间段类型异常" };
                    break;
            }
            switch (ordertype)
            {
                case "0":
                    strwhere = " and  isnull(dr,0)=0 and Otype!=9 and CompID=" + comp.ID + " and OState in (2,3,4,5,7) and CreateDate>='" + date_start + "' and CreateDate<='" + date_end + "'";
                    break;
                case "1":
                    strwhere = " and  isnull(dr,0)=0 and CompID=" + comp.ID + " and  CreateDate>='" + date_start + "' and CreateDate<='" + date_end + "' and ReturnState =3";
                    break;
                case "2":
                    string sql = "SELECT distinct orderID FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype=9 or OState not in(2,3,4,5,7)) and CompID=" + comp.ID + ")  and status!=3 and CompID=" + comp.ID +
                      " and Date>='" + date_start + "' and Date<'" + date_end + "'  AND vedf9=1 ";
                    DataTable dt_pay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                    string where = "";
                    for (int i = 0; i < dt_pay.Rows.Count; i++)
                    {
                        if (ClsSystem.gnvl(dt_pay.Rows[i]["orderID"], "") != "")
                        {
                            where += ",";
                            where += ClsSystem.gnvl(dt_pay.Rows[i]["orderID"], "");
                        }

                    }
                    where = where.Substring(1, where.Length - 1);
                    strwhere = " and id in (" + where + ")";
                    break;
                default:
                    return new ResultOrderList() { Result = "F", Description = "订单类型异常" };
                    break;
            }

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

            string strsql = new Common().PageSqlString(criticalOrderID, "ID", "DIS_Order", sortType,
                    sort, strwhere, getType, rows);


            if (strsql == "")
                return new ResultOrderList() { Result = "F", Description = "基础数据异常" };


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
                    order.CompName = comp.CompName;

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
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetBriefingList：" + JSon);
            return new ResultOrderList() { Result = "F", Description = "参数异常" };
        }
    }

    public EditResult EditResellerLoginPassword(string JSon)
    {
        string UserID = string.Empty;
        string disID = string.Empty;
        string oldPassword = string.Empty;
        string newPassword = string.Empty;
        try
        {
            #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["ResellerID"].ToString() == "" || JInfo["OldPassword"].ToString() == "" ||
                JInfo["NewPassword"].ToString() == "")
                return new EditResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            disID = JInfo["ResellerID"].ToString();
            oldPassword = JInfo["OldPassword"].ToString();
            newPassword = JInfo["NewPassword"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, 0, Int32.Parse(disID == "" ? "0" : disID)))
            {
                return new EditResult() { Result = "F", Description = "登录信息异常" };
            }
            //判断经销商是否异常
            //Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(disID));
            //if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
            //    return new EditResult() { Result = "F", Description = "核心企业信息异常" };

            #endregion
            //判断旧密码是否正确
            if (one.UserPwd != new GetPhoneCode().md5(oldPassword))
                return new EditResult() { Result = "F", Description = "原密码错误" };
            //更新
            if (new Hi.BLL.SYS_Users().UpdatePassWord(new GetPhoneCode().md5(newPassword), UserID))
                return new EditResult() { Result = "T", Description = "修改成功" };
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditResellerLoginPassword:" + JSon);
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
    }


    public EditResult AddResellerShippingAddressList(string JSon)
    {
        string UserID = string.Empty;
        string disID = string.Empty;
        string Principal = string.Empty;
        string Phone = string.Empty;
        string Province = string.Empty;
        string City = string.Empty;
        string Area = string.Empty;
        string Address = string.Empty;
        string IsDefault = string.Empty;

        try
        {
            #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["ResellerID"].ToString() == ""
                || JInfo["Province"].ToString() == "" || JInfo["City"].ToString() == "" ||
                JInfo["Area"].ToString() == "" || JInfo["IsDefault"].ToString() == "")
                return new EditResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            disID = JInfo["ResellerID"].ToString();
            Province = JInfo["Province"].ToString();
            City = JInfo["City"].ToString();
            Area = JInfo["Area"].ToString();
            IsDefault = JInfo["IsDefault"].ToString();
            if (JInfo["Principal"].ToString() == "")
                return new EditResult() { Result = "F", Description = "联系人不能为空" };
            if (JInfo["Phone"].ToString() == "")
                return new EditResult() { Result = "F", Description = "联系电话不能为空" };
            if (JInfo["Address"].ToString() == "")
                return new EditResult() { Result = "F", Description = "详细地址不能为空" };
            Principal = JInfo["Principal"].ToString();
            Phone = JInfo["Phone"].ToString();
            Address = JInfo["Address"].ToString();
            #endregion
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, 0, Int32.Parse(disID == "" ? "0" : disID)))
            {
                return new EditResult() { Result = "F", Description = "登录信息异常" };
            }

            Hi.Model.BD_DisAddr disaddr = new Hi.Model.BD_DisAddr();
            disaddr.DisID = Int32.Parse(disID);
            disaddr.Principal = Principal;
            disaddr.Phone = Phone;
            disaddr.Province = Province;
            disaddr.City = City;
            disaddr.Area = Area;
            disaddr.Address = Address;
            disaddr.IsDefault = IsDefault == "0" ? 0 : 1;
            disaddr.CreateUserID = one.ID;
            disaddr.CreateDate = DateTime.Now;
            disaddr.ts = DateTime.Now;
            disaddr.dr = 0;
            disaddr.modifyuser = one.ID;
            List<Hi.Model.BD_DisAddr> list_addr = null;
            if (IsDefault != "0")
                list_addr = new Hi.BLL.BD_DisAddr().GetList("", "DisID=" + disID + " and isnull(IsDefault,0) =1 and isnull(dr,0) = 0", "");
            SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
            conn.Open();
            SqlTransaction mytran = conn.BeginTransaction();
            try
            {
                if (new Hi.BLL.BD_DisAddr().Add(disaddr, mytran) <= 0)
                {
                    mytran.Rollback();
                    return new EditResult() { Result = "F", Description = "新增失败" };
                }
                if (list_addr != null && list_addr.Count > 0)
                {
                    foreach (Hi.Model.BD_DisAddr addr in list_addr)
                    {
                        addr.IsDefault = 0;
                        if (!new Hi.BLL.BD_DisAddr().Update(addr, mytran))
                        {
                            mytran.Rollback();
                            return new EditResult() { Result = "F", Description = "新增失败" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mytran.Rollback();
                conn.Close();
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddResellerShippingAddressList:" + JSon);
                return new EditResult() { Result = "F", Description = "新增失败" };
            }
            mytran.Commit();
            return new EditResult() { Result = "T", Description = "新增成功" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddResellerShippingAddressList:" + JSon);
            return new EditResult() { Result = "F", Description = "新增失败" };
        }
    }

    #region 返回实体
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
    #endregion

    public class EditResult
    {
        public string Result { get; set; }
        public string Description { get; set; }
    }
}