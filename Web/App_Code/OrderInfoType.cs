using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DBUtility;
using System.Web.UI.WebControls;
using System.Data;
//using System.Web.UI.HtmlControls;

/// <summary>
///OrderInfoType 订单
/// </summary>
public class OrderInfoType
{
    public static Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    public static Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public OrderInfoType()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 订单来源
    /// </summary>
    /// <param name="type">0、网页下单 1、企业补单 2、APP下单</param>
    /// <returns></returns>
    public static string AddType(int type)
    {
        string AddType = string.Empty;

        if (type == (int)Enums.AddType.网页下单)
        {
            AddType = "网页下单";
        }
        else if (type == (int)Enums.AddType.企业补单)
        {
            AddType = "企业补单";
        }
        else if (type == (int)Enums.AddType.App下单)
        {
            AddType = "App下单";
        }
        else if (type == (int)Enums.AddType.App企业补单)
        {
            AddType = "App企业补单";
        }
        return AddType;
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string OType(int type)
    {
        string Otype = string.Empty;

        if (type == (int)Enums.OType.销售订单)
        {
            Otype = "销售订单";
        }
        else if (type == (int)Enums.OType.赊销订单)
        {
            Otype = "赊销订单";
        }
        else if (type == (int)Enums.OType.特价订单)
        {
            Otype = "特价订单";
        }

        return Otype;
    }

    /// <summary>
    /// 订单状态 
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string OState(int Oid)
    {
        string OState = string.Empty;

        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(Oid);

        if (OrderModel != null)
        {
            OState = "已到货";

            if (OrderModel.OState == (int)Enums.OrderState.退回)
            {
                OState = "退回";
            }
            else
                if (OrderModel.OState == (int)Enums.OrderState.未提交)
                {
                    OState = "未提交";
                }
                else
                    if (OrderModel.OState == (int)Enums.OrderState.待审核)
                    {
                        OState = "待审核";
                    }
                    else
                        if (OrderModel.OState == (int)Enums.OrderState.已审)
                        {
                            OState = "已审核";
                            //if (OrderModel.ReturnState == (int)Enums.ReturnState.申请退款)
                            //{
                            //    OState = "申请退款";
                            //}
                            //if ((OrderModel.ReturnState == (int)Enums.ReturnState.退货退款) && (OrderModel.PayState == (int)Enums.PayState.已退款))
                            //{
                            //    OState = "退款完结";
                            //}
                        }
                        else
                            if (OrderModel.OState == (int)Enums.OrderState.退货处理)
                            {
                                OState = "退货处理";
                            }
                            else
                                if (OrderModel.OState == (int)Enums.OrderState.已发货)
                                {
                                    OState = "已发货";
                                }
                                else
                                    if (OrderModel.OState == (int)Enums.OrderState.已到货)
                                    {
                                        OState = "已到货";
                                        //if (OrderModel.ReturnState == (int)Enums.ReturnState.拒绝退货)
                                        //{
                                        //    OState = "拒绝退货";
                                        //}
                                        //else if (OrderModel.ReturnState == (int)Enums.ReturnState.申请退货)
                                        //{
                                        //    OState = "申请退货";
                                        //}
                                        //else if (OrderModel.ReturnState == (int)Enums.ReturnState.退货退款)
                                        //{
                                        //    OState = "退货退款";
                                        //}
                                    }
                                    else
                                        if (OrderModel.OState == (int)Enums.OrderState.已作废)
                                        {
                                            OState = "已作废";
                                        }
                                        else
                                            if (OrderModel.OState == (int)Enums.OrderState.已退货)
                                            {
                                                OState = "已退货";
                                            }

        }
        return OState;
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string PayState(int state)
    {
        string payState = string.Empty;

        if (state == (int)Enums.PayState.未支付)
        {
            payState = "未支付";
        }
        else if (state == (int)Enums.PayState.部分支付)
        {
            payState = "部分支付";
        }
        else if (state == (int)Enums.PayState.已支付)
        {
            payState = "已支付";
        }
        else if (state == (int)Enums.PayState.申请退款)
        {
            payState = "申请退款";
        }
        else if (state == (int)Enums.PayState.已退款)
        {
            payState = "已退款";
        }
        //else if (state == (int)Enums.PayState.已结算)
        //{
        //    payState = "已结算";
        //}
        else if (state == (int)Enums.PayState.支付处理中)
        {
            payState = "支付处理中";
        }
        else
        {
            payState = "";
        }

        return payState;
    }

    /// <summary>
    /// 退货状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string ReturnState(int state)
    {
        string ReturnState = string.Empty;

        if (state == -1)
        {
            ReturnState = "已拒绝";
        }
        else if (state == 0)
        {
            ReturnState = "未提交";
        }
        else if (state == 1)
        {
            ReturnState = "待审核";
        }
        else if (state == 2)
        {
            ReturnState = "已退货";
        }
        else if (state == 3)
        {
            ReturnState = "已签收";
        }
        else
        {
            ReturnState = "已退货款";
        }

        return ReturnState;
    }

    /// <summary>
    /// 查询代理商是否可赊销
    /// </summary>
    /// <param name="DisId">代理商Id</param>
    /// <returns></returns>
    public static bool getCreditType(int DisId)
    {
        bool falg = false;

        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

        if (disModel != null)
        {
            if (disModel.CreditType.ToString().Equals("1"))
            {  //代理商可赊销
                falg = true;
            }
        }
        return falg;
    }

    /// <summary>
    /// 查询代理商企业授信
    /// </summary>
    /// <param name="DisId"></param>
    /// <returns></returns>
    public static decimal GetCreditAmount(int DisId)
    {
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

        if (disModel != null)
        {
            if (disModel.CreditType.ToString().Equals("1"))
            {  //代理商可赊销
                return disModel.CreditAmount;
            }
        }
        return 0;
    }

    /// <summary>
    /// 获取代理商已发货、未支付（部分支付）的订单总金额
    /// </summary>
    /// <param name="DisID">代理商Id</param>
    /// <param name="compid">厂商ID</param>
    /// <param name="orderID">订单ID 有值则是修改  本条订单的金额不参与计算总价</param>
    /// <returns></returns>
    public static decimal GetSumAmount(string DisID,string compid,int orderID)
    {
        string sql = @"select SUM(AuditAmount-PayedAmount) as Amount from DIS_Order where PayState in (0,1) and OState in (1,2,4,5) and DisID=" + DisID+ " and CompID="+ compid + " and dr<>1 and id<>"+ orderID + "";

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            return dt.Rows[0]["Amount"].ToString().ToDecimal(0);
        }
        return 0;
    }

    /// <summary>
    /// 判断订单是否需要审核
    /// </summary>
    /// <param name="DisId">代理商Id</param>
    /// <param name="Otype">订单类型</param>
    /// <returns></returns>
    public static int OrderEnAudit(int AddType, int DisId, int Otype)
    {
        /*
         * 销售订单、赊销订单 代理商审核标志为无需审时 
         * 特价订单  都需要审核
         **/
        int isAudit = 0;

        Hi.Model.BD_Distributor DisModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

        if (AddType == (int)Enums.AddType.企业补单)
        {
            //企业补单 销售订单、赊销订单、特价订单判断代理商订单审核权限
            if (DisModel != null)
            {
                if (DisModel.IsCheck == 0)
                {
                    //代理商不需审核
                    isAudit = 1;
                }
            }
        }
        else
        {
            //代理商下单 销售订单、赊销订单判断代理商订单审核权限，特价订单都需要审核
            if (Otype == 0 || Otype == 1)
            {
                if (DisModel != null)
                {
                    if (DisModel.IsCheck == 0)
                    {
                        //代理商不需审核
                        isAudit = 1;
                    }
                }
            }
        }
        return isAudit;
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public static string getOrder(object Id, string col)
    {
        string str_Nums = string.Empty;
        //object value = null;

        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(Id.ToString().ToInt(0));

        if (OrderModel != null)
        {
            if (col == "ReceiptNo")
            {
                str_Nums = OrderModel.ReceiptNo;
            }
            else if (col == "AddrID")
            {
                str_Nums = OrderModel.AddrID.ToString();
            }
            else if (col == "CreateDate")
            {
                str_Nums = OrderModel.CreateDate.ToString();
            }
            else if (col == "CreateUserID")
            {
                str_Nums = OrderModel.CreateUserID.ToString();
            }
            else if (col == "AuditAmount")
            {
                str_Nums = OrderModel.AuditAmount.ToString("N");
            }
            else if (col == "GoodsNum")
            {
                decimal sum = 0;
                List<Hi.Model.DIS_OrderDetail> l = OrderDetailBll.GetList("", " isnull(dr,0)=0 and OrderID=" + Id, "");
                foreach (Hi.Model.DIS_OrderDetail item in l)
                {
                    sum += item.GoodsNum;
                }
                str_Nums = sum.ToString("0.00");
            }
            //else
            //{
            //    foreach (System.Reflection.PropertyInfo info in OrderModel.GetType().GetProperties())
            //    {
            //        if (info.Name.ToLower() == col.ToLower())
            //        {
            //            return info.GetValue(OrderModel, null);
            //        }
            //    }
            //}
        }

        return str_Nums;
    }


    /// <summary>
    /// 订单信息
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public static string getOrderExt(object Id, string col)
    {
        string str_Nums = string.Empty;
        //object value = null;
        List<Hi.Model.DIS_OrderExt> oextl =new Hi.BLL.DIS_OrderExt().GetList(""," OrderID="+Id,"");

        if (oextl != null && oextl.Count>0)
        {
            if (col == "ProID")
            {
                str_Nums = oextl[0].ProID.ToString();
            }
            else if (col == "ProAmount")
            {
                str_Nums = oextl[0].ProAmount.ToString();
            }
            else if (col == "ProDID")
            {
                str_Nums = oextl[0].ProDID.ToString();
            }
            else if (col == "Protype")
            {
                str_Nums = oextl[0].Protype.ToString();
            }
        }
        return str_Nums;
    }

    static Hi.BLL.BD_Rebate bate = new Hi.BLL.BD_Rebate();

    /// <summary>
    /// 新增订单
    /// </summary>
    /// <param name="OrderInfoModel"></param>
    /// <param name="OrderDetailList"></param>
    /// <returns></returns>
    public static int TansOrder(Hi.Model.DIS_Order OrderInfoModel, List<Hi.Model.DIS_OrderDetail> OrderDetailList)
    {
        int OrderId = 0;
        System.Text.StringBuilder sqlInven = new System.Text.StringBuilder();
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", OrderInfoModel.CompID).ToInt(0);

        SqlTransaction sqlTrans =DBUtility.SqlHelper.CreateStoreTranSaction();
        //可以做循环   

        try
        {
            OrderId = OrderBll.AddOrder(sqlTrans.Connection, OrderInfoModel, sqlTrans);
            if (OrderId == 0)
            {
                OrderId = 0;
                sqlTrans.Rollback();
                return OrderId;
            }

            if (OrderDetailList.Count <= 0)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }
            else
            {
                foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
                {
                    item.OrderID = OrderId;
                    int count = OrderDetailBll.AddOrderDetail(sqlTrans.Connection, item, sqlTrans);
                    if (count == 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
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

                if (OrderInfoType.rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
                {
                    //订单支付返利启用
                    //if (OrderInfoModel.vdef8.ToDecimal(0) > 0)
                    //{
                    //    //使用返利大于0;
                    //    if (bate.TransRebate(OrderInfoModel.DisID, OrderInfoModel.vdef8.ToDecimal(0), OrderId, OrderInfoModel.CreateUserID, sqlTrans))
                    //    {
                    //        sqlTrans.Commit();
                    //        return OrderId;
                    //    }
                    //    else
                    //    {
                    //        sqlTrans.Rollback();
                    //        OrderId = 0;
                    //        return OrderId;
                    //    }
                    //}
                }
                sqlTrans.Commit();
            }
        }
        catch
        {
            OrderId = 0;
            sqlTrans.Rollback();
        }
        finally
        {
            //sqlTrans.Connection.Close();
        }

        return OrderId;
    }

    /// <summary>
    /// 修改订单
    /// </summary>
    /// <param name="OrderInfoModel"></param>
    /// <param name="OrderDetailList"></param>
    /// <returns></returns>
    public static int UpdateOrder(Hi.Model.DIS_Order OrderInfoModel, List<Hi.Model.DIS_OrderDetail> OrderDetailList,string delOrderD)
    {
        int OrderId = 0;
        //返回修改库存的sql
        System.Text.StringBuilder sqlInven = new System.Text.StringBuilder();
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", OrderInfoModel.CompID).ToInt(0);

        if (IsInve == 0)
        {
            //修改商品库存，先返还订单明细删除的商品库存
            sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory(delOrderD, OrderDetailList));
        }

        //SqlConnection con = new SqlConnection(LocalSqlServer);
        //con.Open();
        //System.Data.IsolationLevel.RepeatableRead
        SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();
        //可以做循环   

        try
        {
            //private object thislock = OrderInfoModel.ID as object;
            //lock (thislock)
            //{
            OrderId = OrderBll.UpdateOrder(sqlTrans.Connection, OrderInfoModel, sqlTrans);
            if (OrderDetailList.Count <= 0)
            {
                OrderId = 0;
                sqlTrans.Rollback();
            }
            else
            {
                if (!delOrderD.Equals(""))
                {
                    //修改时 删除商品后 清除在数据库中存在的该商品
                    if (OrderDetailBll.GetDel(delOrderD, sqlTrans.Connection, sqlTrans) < 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }

                foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
                {
                    Hi.Model.DIS_OrderDetail OrderDeModel = OrderDetailBll.GetModel(item.ID);
                    int count = 0;
                    if (OrderDeModel != null)
                    {
                        if (IsInve == 0)
                            //修改订单明细时，先返还商品库存
                            sqlInven.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", OrderDeModel.GoodsNum, OrderDeModel.GoodsinfoID);

                        item.OrderID = OrderInfoModel.ID;
                        //修改订单时，订单明细里存在该商品 修改商品信息
                        count = OrderDetailBll.UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans);
                        if (count == 0)
                        {
                            OrderId = 0;
                            sqlTrans.Rollback();
                        }
                    }
                    else
                    {
                        //修改订单时，订单明细里不存在该商品新增商品信息
                        item.OrderID = OrderInfoModel.ID;
                        count = OrderDetailBll.AddOrderDetail(sqlTrans.Connection, item, sqlTrans);
                        if (count == 0)
                        {
                            OrderId = 0;
                            sqlTrans.Rollback();
                        }
                    }
                }
                if (IsInve == 0)
                {
                    //修改商品库存，先返还订单明细删除的商品库存
                    //sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory(delOrderD, OrderDetailList));
                    if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), sqlTrans.Connection, sqlTrans) <= 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }

                if (OrderInfoType.rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
                {
                    //订单支付返利启用
                    //if (bate.TransEditRebate(OrderInfoModel.DisID, OrderInfoModel.vdef8.ToDecimal(0), OrderInfoModel.ID, OrderInfoModel.CreateUserID, sqlTrans))
                    //{
                    //    sqlTrans.Commit();
                    //    return OrderId;
                    //}
                }
                sqlTrans.Commit();
            }
            //}
        }
        catch
        {
            OrderId = 0;
            sqlTrans.Rollback();
        }
        finally
        {
            //sqlTrans.Connection.Close();
            //sqlTrans.Connection.Dispose();
        }

        return OrderId;
    }

    /// <summary>
    /// 订单发货
    /// </summary>
    /// <returns></returns>
    public static int ShopOrder(Hi.Model.DIS_Order orderModel)
    {
        SqlTransaction TranSaction = null;
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }
            //修改订单状态
            orderModel.ts = DateTime.Now;
            orderModel.OState = (int)Enums.OrderState.已发货;
            orderModel.modifyuser = AdminUserModel.UserID;
            
            //新增发货信息
            Hi.Model.DIS_OrderOut orderOut = new Hi.Model.DIS_OrderOut();
            orderOut.ReceiptNo = orderModel.ReceiptNo + SysCode.GetCode("发货单", orderModel.ID.ToString());
            orderOut.CompID = orderModel.CompID;
            orderOut.DisID = orderModel.DisID;
            orderOut.OrderID = orderModel.ID;
            orderOut.ActionUser = AdminUserModel.TrueName;
            orderOut.SendDate = DateTime.Now;
            orderOut.CreateUserID = AdminUserModel.UserID;
            orderOut.CreateDate = DateTime.Now;
            orderOut.ts = DateTime.Now;
            orderOut.dr = 0;
            orderOut.modifyuser = AdminUserModel.UserID;

            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();

            bool res = new Hi.BLL.DIS_Order().Update(orderModel, TranSaction);
            if (!res)
            {
                TranSaction.Rollback();
                return 0;
            }
            int count = new Hi.BLL.DIS_OrderOut().Add(orderOut, TranSaction);
            if (count == 0)
            {
                TranSaction.Rollback();
                return 0;
            }

            TranSaction.Commit();
            return count;
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
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 订单到货签收
    /// </summary>
    /// <returns></returns>
    public static int SignOrder(Hi.Model.DIS_OrderOut OrderOutModel, Hi.Model.DIS_Order orderModel)
    {
        SqlTransaction TranSaction = null;
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }

            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();
            //到货签收修改订单状态
            orderModel.OState = (int)Enums.OrderState.已到货;
            orderModel.ts = DateTime.Now;
            orderModel.modifyuser = AdminUserModel.UserID;

            bool res = new Hi.BLL.DIS_Order().Update(orderModel, TranSaction);
            if (!res)
            {
                TranSaction.Rollback();
                return 0;
            }
            //向发货表中添加到货信息
            bool count = new Hi.BLL.DIS_OrderOut().Update(OrderOutModel, TranSaction);
            if (!count)
            {
                TranSaction.Rollback();
                return 0;
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
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 新增订单退货
    /// </summary>
    /// <param name="ReturnModel">退货实体</param>
    /// <param name="OrderId">订单ID</param>
    /// <returns></returns>
    public static int ReturnOrderAdd(Hi.Model.DIS_OrderReturn ReturnModel, int OrderId)
    {
        SqlTransaction TranSaction = null;
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }
            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();

            Hi.Model.DIS_Order orderModel = OrderBll.GetModel(OrderId);
            if (orderModel == null)
            {
                return 0;
            }
            //退货修改订单状态
            orderModel.ReturnState = (int)Enums.ReturnState.新增退货;
            orderModel.ts = DateTime.Now;
            orderModel.modifyuser = AdminUserModel.UserID;

            bool res = new Hi.BLL.DIS_Order().Update(orderModel, TranSaction);
            if (!res)
            {
                TranSaction.Rollback();
                return 0;
            }
            //新增退货信息
            int count = new Hi.BLL.DIS_OrderReturn().Add(Connection, ReturnModel, TranSaction);
            if (count == 0)
            {
                TranSaction.Rollback();
                return 0;
            }

            TranSaction.Commit();
            return count;

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
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 修改订单退货退款
    /// </summary>
    /// <param name="ReturnModel">退货实体</param>
    /// <param name="OrderId">订单ID</param>
    /// <param name="State">订单状态</param>
    /// <returns></returns>
    public static int ReturnOrderUpdate(Hi.Model.DIS_OrderReturn ReturnModel, Hi.Model.DIS_Order OrderModel)
    {

        SqlTransaction TranSaction = null;
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }
            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();

            //修改订单状态
            bool res = new Hi.BLL.DIS_Order().Update(OrderModel, TranSaction);
            if (!res)
            {
                TranSaction.Rollback();
                return 0;
            }
            //修改退货信息
            int count = new Hi.BLL.DIS_OrderReturn().Update(Connection, ReturnModel, TranSaction);
            if (count == 0)
            {
                TranSaction.Rollback();
                return 0;
            }

            TranSaction.Commit();
            return count;

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
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 修改退货修改
    /// </summary>
    /// <param name="sqlconn"></param>
    /// <param name="sqltans"></param>
    /// <param name="ReturnModel"></param>
    /// <param name="OrderModel"></param>
    /// <returns></returns>
    public static int ReturnOrderUpdate(SqlConnection sqlconn, SqlTransaction sqltans, Hi.Model.DIS_OrderReturn ReturnModel, Hi.Model.DIS_Order OrderModel)
    {

        SqlTransaction TranSaction = null;
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }
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

    /// <summary>
    /// 商品是否可购买
    /// </summary>
    /// <param name="DisId">代理商ID</param>
    /// <param name="GoodsInfoId">商品信息ID</param>
    /// <param name="Pro">是否可以促销</param>
    /// <param name="Pro">厂商ID</param>
    /// <returns>1、商品不能购买   0、商品可以购买</returns>
    public static int IsGoodsShip(int DisId, int GoodsInfoId, int IsPro,string CompID, out string GoodsName,out string GoodMome)
    {
        //商品名称
        GoodsName = "";
        GoodMome = "";
        Hi.Model.BD_GoodsInfo infoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodsInfoId);

        if (infoModel != null)
        {
            Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(infoModel.GoodsID);

            if (goodsModel != null)
            {
                GoodsName = goodsModel.GoodsName;
                //商品是否禁用、是否删除、是否上架
                if (infoModel.IsEnabled == false || infoModel.IsOffline == 0 || infoModel.dr == 1)
                {
                    GoodMome = "已删除、禁用或未上架";
                    return 1;
                }

                //商品基本信息是否禁用、是否删除、是否上架
                if (goodsModel.IsEnabled == 0 || goodsModel.dr ==  1 || goodsModel.IsOffline == 0)
                {
                    GoodMome = "已删除、禁用或未上架";
                    return 1;
                }

                // 商品可销售区域黑名单
                List<Common.GoodsID> gl = Common.DisEnAreaGoodsID(DisId.ToString(), CompID);
                int glCount = 0;
                if (gl != null && gl.Count > 0)
                {
                    glCount = gl.FindAll(p => p.goodsID == goodsModel.ID.ToString()).Count;

                    if (glCount != 0)
                    {
                        GoodMome = "不在你的销售区内，不能购买";
                        return 1;
                    }
                }
                else { return 0; }

                //是否可以促销商品
                if (IsPro != 0)
                {
                    int ld = Common.GetPro(goodsModel.ID, GoodsInfoId.ToString(), goodsModel.CompID.ToString());

                    if (ld == 0)
                    {
                        GoodMome = "促销活动已结束";
                        return 1;
                    }
                    return ld;
                }
            }
            else { GoodMome = "不存在"; return 1; }
        }
        else { GoodMome = "不存在"; return 1; }

        return 0;
    }

    /// <summary>
    /// 已发货超时未签收的订单  自动签收
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string SendShip(int CompID)
    {
        //系统自动签收天数
        int Sign = System.Configuration.ConfigurationManager.AppSettings["Sign"] != null ? System.Configuration.ConfigurationManager.AppSettings["Sign"].ToString().ToInt(0) : 10;

        //获取厂商设置自动签收天数
        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", "Name='订单自动签收' and CompID=" + CompID, "");
        if (sl != null && sl.Count > 0)
            Sign = sl[0].Value.ToInt(0);

        //当前时间
        DateTime now = DateTime.Now;
        DateTime oldTime = now.AddDays(-Sign);

        //用户名ID、Name
        int UserID = 0;
        string userName = string.Empty;  //Common.GetUserName(UserID);

        //查询超过签收天数条件
        string signstr = " CompID=" + CompID + "and SendDate<='" + oldTime + "'";

        //执行sql
        string str = string.Empty;

        //查询超过签收天数未签收的订单
        List<Hi.Model.DIS_OrderOut> outl = new Hi.BLL.DIS_OrderOut().GetList("", signstr, "");

        if (outl == null && outl.Count == 0)
            return "";

        foreach (Hi.Model.DIS_OrderOut item in outl)
        {
            
            Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(item.OrderID);
            List<Hi.Model.DIS_OrderDetail> od = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID=" + item.OrderID, "");
            //查询该订单已经签收的条单
            List<Hi.Model.DIS_OrderOut> lo = new Hi.BLL.DIS_OrderOut().GetList("", "isnull(IsSign,0)=1 and OrderID=" + item.OrderID, "");
            //订单明细商品总数量
            decimal goodsnum = 0; //订单商品总数
            if (od != null && od.Count > 0)
            {
                foreach (var item0 in od)
                    goodsnum += item0.GoodsNum;
            }

            decimal signnum = 0;  //已签收商品数量
            if (lo != null && lo.Count > 0)
            {
                var idstr = "";
                foreach (var item1 in lo)
                    idstr += item1.ID + ",";

                if (idstr.Length > 0)
                    idstr = idstr.Substring(0, idstr.Length - 1);

                List<Hi.Model.DIS_OrderOutDetail> lod = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID in(" + idstr + ")", "");
                if (lod != null && lod.Count > 0)
                {
                    foreach (var item2 in lod)
                        signnum += item2.SignNum;
                }
            }

            int IsOutState = (int)Enums.IsOutState.部分到货;
            int OState = (int)Enums.OrderState.已发货;
            //订单
            //到货签收修改订单状态
            if (OrderModel.IsOutState == (int)Enums.IsOutState.全部发货)
            {
                //签收数量等于订单商品数量 ==全部到货
                if (signnum == goodsnum)
                {
                    IsOutState = (int)Enums.IsOutState.全部到货;
                    OState = (int)Enums.OrderState.已到货;
                }
                else
                {
                    IsOutState = (int)Enums.IsOutState.全部发货;
                    OState = (int)Enums.OrderState.已发货;
                }
            }
            else if (OrderModel.IsOutState == (int)Enums.IsOutState.部分发货 || OrderModel.IsOutState == (int)Enums.IsOutState.部分到货)
            {
                IsOutState = (int)Enums.IsOutState.部分到货;
                OState = (int)Enums.OrderState.已发货;
            }

            //本次签收商品数量
            List<Hi.Model.DIS_OrderOutDetail> lodd = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID=" + item.ID, "");
            if (lodd != null && lodd.Count > 0)
            {
                foreach (var item3 in lodd)
                    str += "update DIS_OrderOutDetail set SignNum=OutNum where OrderOutID=" + item.ID + " and ID" + item3.ID + ";";
            }

            userName = Common.GetUserName(OrderModel.DisUserID);
            UserID = OrderModel.DisUserID;

            //判断订单状态
            if (OrderModel.OState == (int)Enums.OrderState.已发货 && OrderModel.ReturnState == (int)Enums.ReturnState.未退货)
            {
                //订单状态
                str += " update Dis_Order set OState=" + OState + ",IsOutState=" + IsOutState + ",modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.OrderID + ";";
                //签收状态
                str += " update Dis_OrderOut set SignDate='" + DateTime.Now + "',SignUser='" + userName + "',SignUserId=" + UserID + ",IsSign=1,modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.ID + ";";

                //自动签收日志
                str += string.Format("INSERT INTO SYS_SysBusinessLog VALUES({0},'{1}',{2},'{3}','{4}',{5},'{6}','{7}','{8}',{9},{10});", CompID, "Order", item.OrderID, "订单自动签收", DateTime.Now, UserID, userName, "", DateTime.Now, 0, UserID);
            
            }
            else
                continue;
        }

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand(str.ToString(), Connection, TranSaction);
            cmd.CommandType = CommandType.Text;

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            if (rowsAffected > 0)
            {
                TranSaction.Commit();
                return "1";
            }
            else
            {
                TranSaction.Rollback();
                return "";
            }
        }
        catch (Exception ex)
        {
            TranSaction.Rollback();
            Console.WriteLine(ex.Message);
            return "";
        }
        finally
        {
            Connection.Dispose();
        }
    }

    /// <summary>
    /// 超时未付款自动作废订单  自动取消
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string OffShip(int CompID)
    {
        int off = 30;
        //获取厂商设置自动签收天数
        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", "Name='超时未付款自动作废订单' and CompID=" + CompID, "");
        if (sl != null && sl.Count > 0)
            off = sl[0].Value.ToInt(0);

        //当前时间
        DateTime now = DateTime.Now;
        DateTime oldTime = now.AddDays(-off);

        //用户名ID、Name
        int UserID = 0;
        string userName = string.Empty;  //Common.GetUserName(UserID);

        //查询超时未付款自动作废订单条件  待审核，已审核未支付的订单取消
        string offstr = " CompID=" + CompID + "and OState in(1,2) and PayState=0 and ts<='" + oldTime + "'";

        //执行sql
        string str = string.Empty;

        //查询超时未付款自动作废订单的订单
        List<Hi.Model.DIS_Order> ol = new Hi.BLL.DIS_Order().GetList("", offstr, "");

        if (ol == null && ol.Count == 0)
            return "";

        foreach (Hi.Model.DIS_Order item in ol)
        {
            userName = Common.GetUserName(item.DisUserID);
            UserID = item.DisUserID;

            //判断订单状态
            if ((item.OState == (int)Enums.OrderState.待审核 || item.OState == (int)Enums.OrderState.已审) && item.PayState == (int)Enums.PayState.未支付)
            {
                //订单状态
                str += " update Dis_Order set OState=" + (int)Enums.OrderState.已作废 + ",modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.ID + ";";
                //超时未付款自动取消
                str += string.Format("INSERT INTO SYS_SysBusinessLog VALUES({0},'{1}',{2},'{3}','{4}',{5},'{6}','{7}','{8}',{9},{10});", CompID, "Order", item.ID, "超时未付款自动作废", DateTime.Now, UserID, userName, "", DateTime.Now, 0, UserID);
            }
            else
            {
                continue;
            }
        }

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand(str.ToString(), Connection, TranSaction);
            cmd.CommandType = CommandType.Text;

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            if (rowsAffected > 0)
            {
                TranSaction.Commit();
                return "1";
            }
            else
            {
                TranSaction.Rollback();
                return "";
            }
        }
        catch (Exception ex)
        {
            TranSaction.Rollback();
            Console.WriteLine(ex.Message);
            return "";
        }
        finally
        {
            Connection.Dispose();
        }
    }

    /// <summary>
    /// 新增积分记录
    /// </summary>
    /// <param name="CompId">厂商ID</param>
    /// <param name="DisId">代理商ID</param>
    /// <param name="IntegralType">积分类型</param>
    /// <param name="type">加减积分  1、加积分  2、减积分 </param>
    /// <param name="OrderId">订单ID</param>
    /// <param name="Integral">积分数量</param>
    /// <param name="Source">积分来源</param>
    /// <param name="Remarks">备注</param>
    /// <param name="modifyuser">更新人ID</param>
    /// <returns>返回 0、新增失败   </returns>
    public static int AddIntegral(int CompId, int DisId, string IntegralType, int type, int OrderId, decimal Integral, string Source, string Remarks,int modifyuser)
    {
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

        //判断代理商是否存在
        if (disModel != null)
        {
            decimal DisIntegral = disModel.Integral;  //代理商已有积分
            decimal OldIntegral=DisIntegral;  //旧积分
            decimal NewIntegral=0;    //新积分
            if (type == 1)
            {
                //加积分
                NewIntegral = DisIntegral + Integral;
            }
            else
            {
                //减积分
                if (DisIntegral < Integral)
                {
                    //已有积分小于新增积分数  积分为0
                    NewIntegral = 0;
                }
                else
                {
                    NewIntegral = DisIntegral - Integral;
                }
            }

            string sql = string.Format("update BD_Distributor set Integral=" + NewIntegral + " where ID=" + DisId + ";");
            sql += string.Format("INSERT INTO DIS_Integral([CompID],[DisID],[OrderID],[IntegralType],[OldIntegral],[Integral],[NewIntegral],[Source],[Remarks],[CreateDate],[type],[IsView],[ts],[dr],[modifyuser]) VALUES({0},{1},{2},'{3}',{4},{5},{6} ,'{7}','{8}','{9}',{10},0,'{11}',0,{12})", CompId, DisId, OrderId, IntegralType, OldIntegral, Integral, NewIntegral, Source, Remarks, DateTime.Now, type, DateTime.Now, modifyuser);


            SqlTransaction TranSaction = null;
            SqlConnection Connection = new SqlConnection(LocalSqlServer);
            Connection.Open();
            TranSaction = Connection.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, Connection, TranSaction);
                cmd.CommandType = CommandType.Text;

                int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
                if (rowsAffected > 0)
                {
                    TranSaction.Commit();
                    return rowsAffected;
                }
                else
                {
                    TranSaction.Rollback();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                TranSaction.Rollback();
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                Connection.Dispose();
            }
        }

        return 0;
    }

    /// <summary>
    /// 判断未支付的是订单是否可以发货
    /// </summary>
    /// <param name="OrderId">订单ID</param>
    /// <returns>1、代理商赊销不能发货的  0、代理商赊销可以发货的</returns>
    public static int JuOrder(int OrderId)
    {
        Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(OrderId);

        if (OrderModel != null)
        {
            if (OrderModel.PayState == (int)Enums.PayState.部分支付 || OrderModel.PayState == (int)Enums.PayState.未支付)
            {
                //代理商是否可赊销
                bool falg = getCreditType(OrderModel.DisID);

                if (falg)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

    /// <summary>
    /// 获取订单设置
    /// </summary>
    /// <param name="Name">设置名称</param>
    /// <param name="CompID">厂商ID</param>
    /// <returns>0、不审核,1、审核</returns>
    public static string rdoOrderAudit(string Name,int CompID)
    {
        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("Value", " CompID=" + CompID + " and Name='" + Name + "' and dr=0", "");
        if (sl != null && sl.Count > 0)
        {
            return sl[0].Value;
        }
        return "0";
    }
    
    /// <summary>
    /// 返回促销提示
    /// </summary>
    /// <param name="val1"></param>
    /// <param name="val2"></param>
    /// <returns></returns>
    public static string proTypePrce(string val1, string val2, string val3)
    {
        string sql = "select Protype from dbo.BD_Promotion where ID=" + val1;

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        DateTime now = DateTime.Now;
        string str = string.Empty;
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (item["ProType"].ToString() != "3")
                {
                    str = "<span style=\" text-decoration:line-through;\">" + Convert.ToDecimal(val3).ToString("N") + "</span>";
                }
            }
            return str;
        }
        return "";
    }

    /// <summary>
    /// 返回促销提示
    /// </summary>
    /// <param name="val1">促销ID</param>
    /// <param name="val2">促销数量</param>
    /// <returns></returns>
    public static string proType(string val1, string val2)
    {
        int CompId = (HttpContext.Current.Session["UserModel"] as LoginModel).CompID;
        string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompId);

        if (val1 != "0" && val2 != Digits)
        {
            return "赠送(" + val2 + ")";
        }
        return "";
    }

    /// <summary>
    /// 订单促销返回提示
    /// </summary>
    /// <param name="ProIDD">订单促销明细ID</param>
    /// <param name="ProPrice"></param>
    /// <returns></returns>
    public static string proOrderType(string ProIDD, string ProPrice, string ProType)
    {
        string str = "";

        if (ProType != "")
        {
            string[] type = ProType.Split(new char[] { ',' });
            if (type.Length == 3)
            {
                if (type[0].ToString() == "5")
                {
                    //满减
                    str = "满减活动(满" + type[1].ToDecimal(0).ToString("N") + "减" + type[2].ToDecimal(0).ToString("N") + ")：<div>-￥<label>" + ProPrice + "</label></div>";
                }
                else if (type[0].ToString() == "6")
                {
                    str = "满折活动(满" + type[1].ToDecimal(0).ToString("N") + "打折" + type[2].ToDecimal(0).ToString("N") + "%)：<div>-￥<label>" + ProPrice + "</label></div>";
                }
            }

        }
        else if (ProIDD != "" && ProType == "")
        {
            string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice,prod2.OrderPrice,prod2.Discount as ProDiscount,prod2.ID as IDD from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID left join BD_PromotionDetail2 as prod2 on pro.ID=prod2.ProID  where prod2.ID={0} order by pro.CreateDate desc", ProIDD);

            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    decimal Price = 0;

                    Price = item["OrderPrice"].ToString().ToDecimal(0);

                    if (item["ProType"].ToString() == "5")
                    {
                        //满减
                        str = "满减活动(满" + Price.ToString("N") + "减" + item["ProDiscount"].ToString().ToDecimal(0).ToString("N") + ")：<div>-￥<label>" + ProPrice + "</label></div>";
                    }
                    else
                    {
                        str = "满折活动(满" + Price.ToString("N") + "打折" + item["ProDiscount"].ToString().ToDecimal(0).ToString("N") + "%)：<div>-￥<label>" + ProPrice + "</label></div>";
                    }
                }
            }
        }

        return str;
    }

    /// <summary>
    /// 数量显示小数位数
    /// </summary>
    /// <param name="Name">设置名称</param>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string GetNum(decimal num)
    {
        LoginModel louser = HttpContext.Current.Session["UserModel"] as LoginModel;
        string Digits = "0";
        if (louser != null)
        {
            List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + louser.CompID + " and Name='订单下单数量是否取整' and dr=0", "");
            if (sl != null && sl.Count > 0)
            {
                Digits = sl[0].Value;
            }
        }
        return decimal.Parse(string.Format("{0:N4}", (num).ToString("#,####" + Digits))).ToString();
    }

    /// <summary>
    /// 获取返利总金额
    /// </summary>
    /// <param name="OrderID">订单ID</param>
    /// <param name="DisID">代理商ID</param>
    /// <returns></returns>
    public static string GetRebate(int OrderID, int DisID)
    {
        if (OrderID != 0)
        {
            return (bate.GetEditEnableAmount(OrderID, DisID)).ToString("N");
        }
        else
        {
            return (bate.GetRebateEnableAmount(DisID)).ToString("N");
        }
    }
}

//订单类型
public class Otype
{
    private int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    private string value;

    public string Value
    {
        get { return this.value; }
        set { this.value = value; }
    }

    public static List<Otype> AddOtype(string falg)
    {
        List<Otype> Ol = new List<Otype>();
        Otype o = new Otype();
        o.Id = -1;
        o.Value = "请选择";

        Otype o1 = new Otype();
        o1.Id = 0;
        o1.Value = "销售订单";

        Otype o2 = new Otype();
        o2.Id = 1;
        o2.Value = "赊销订单";

        Otype o3 = new Otype();
        o3.Id = 2;
        o3.Value = "特价订单";

        Ol.Add(o);
        Ol.Add(o1);
        if (falg == "True")
        {
            Ol.Add(o2);
        }
        Ol.Add(o3);

        return Ol;
    }
    //绑定订单类型
    public static void OtypeAdd(System.Web.UI.HtmlControls.HtmlSelect ddlOtype, string falg)
    {
        List<Otype> Ol = Otype.AddOtype(falg);
        ddlOtype.Items.Clear();
        ddlOtype.DataSource = Ol;
        ddlOtype.DataValueField = "Id";
        ddlOtype.DataTextField = "Value";
        ddlOtype.DataBind();
    }
}