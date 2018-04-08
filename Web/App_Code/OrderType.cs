using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using DBUtility;

/// <summary>
///OrderType 的摘要说明
/// </summary>
public class OrderType
{
    public static Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    public static Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();
    public static Hi.BLL.DIS_OrderExt OrderExtBll = new Hi.BLL.DIS_OrderExt();
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    static Hi.BLL.BD_Rebate bate = new Hi.BLL.BD_Rebate();
	public OrderType()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
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

    /// <summary>
    /// 获取订单状态
    /// </summary>
    /// <param name="Ostate">订单状态</param>
    /// <param name="IsOutState">订单发货状态</param>
    /// <returns></returns>
    public static string GetOState(string Ostate, string IsOutState)
    {
        string state = "待审核";

        switch (Ostate)
        {
            case "1":
                state = "待审核";
                break;
            case "2":
                state = "待发货";
                break;
            case "3":
                state = "退货处理";
                break;
            case "4":
                if (IsOutState == "1" || IsOutState == "2")
                    state = "待发货";
                else if (IsOutState == "3")
                    state = "待收货";
                else if (IsOutState == "4")
                    state = "已完成";
                else
                    state = "待发货";
                break;
            case "5":
                state = "已完成";
                break;
            case "6":
                state = "已作废";
                break;
            case "7":
                state = "已退货";
                break;
        }
        return state;
    }

    /// <summary>
    /// 获取文件大小
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetSize(string filepath)
    {
        string path = Common.GetWebConfigKey("ImgPath") + "OrderFJ/" + filepath;

        long lSize = 0;
        if (File.Exists(path))
            lSize = new FileInfo(path).Length;
        return (lSize / 1024).ToString(); ;

        //FileStream file = File.Open(path, FileMode.Open);
        //long size = file.Length;
        //return 
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
    /// 获取订单设置
    /// </summary>
    /// <param name="Name">设置名称</param>
    /// <param name="CompID">厂商ID</param>
    /// <returns>0、不审核,1、审核</returns>
    public static string rdoOrderAudit(string Name, int CompID)
    {
        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID + " and Name='" + Name + "' and dr=0", "");
        if (sl != null && sl.Count > 0)
        {
            return sl[0].Value;
        }
        return "0";
    }
    
    /// <summary>
    /// 新增订单
    /// </summary>
    /// <param name="OrderInfoModel"></param>
    /// <param name="OrderDetailList"></param>
    /// <returns></returns>
    public static int TansOrder(Hi.Model.DIS_Order OrderInfoModel, Hi.Model.DIS_OrderExt OrderExt, List<Hi.Model.DIS_OrderDetail> OrderDetailList)
    {
        int OrderId = 0;
        int OrderExtId = 0;
        System.Text.StringBuilder sqlInven = new System.Text.StringBuilder();
        int IsInve = rdoOrderAudit("商品是否启用库存", OrderInfoModel.CompID).ToInt(0);

        SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();
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
            OrderExt.OrderID = OrderId;
            OrderExtId = OrderExtBll.Add(sqlTrans.Connection, OrderExt, sqlTrans);
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
                        //sqlTrans.Rollback();
                    }
                }

                if (rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
                {
                    //订单支付返利启用
                    if (OrderInfoModel.bateAmount > 0)
                    {
                        //使用返利大于0;
                        if (bate.TransRebate(OrderInfoModel.DisID, OrderInfoModel.bateAmount, OrderId, OrderInfoModel.CreateUserID, sqlTrans))
                        {
                            sqlTrans.Commit();
                            return OrderId;
                        }
                        else
                        {
                            sqlTrans.Rollback();
                            OrderId = 0;
                            return OrderId;
                        }
                    }
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
    public static int UpdateOrder(DateTime dts,Hi.Model.DIS_Order OrderInfoModel, Hi.Model.DIS_OrderExt OrderExt, List<Hi.Model.DIS_OrderDetail> OrderDetailList, string delOrderD)
    {

        //判断订单时间
        if (new Hi.BLL.DIS_Order().Getts("Dis_Order", OrderInfoModel.ID, dts) == 0)
            return -1;

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
                if (OrderExt != null)
                {
                    //修改订单扩展表
                    if (!OrderExtBll.Update(sqlTrans.Connection, OrderExt, sqlTrans)) {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }

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
                            sqlInven.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", OrderDeModel.GoodsNum + Convert.ToDecimal(OrderDeModel.ProNum), OrderDeModel.GoodsinfoID);

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
                    //sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory(delOrderD,  OrderDetailList));
                    if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), sqlTrans.Connection, sqlTrans) <= 0)
                    {
                        OrderId = 0;
                        sqlTrans.Rollback();
                    }
                }

                if (OrderInfoType.rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
                {
                    //订单支付返利启用
                    if (bate.TransEditRebate(OrderInfoModel.DisID, OrderInfoModel.bateAmount, OrderInfoModel.ID, OrderInfoModel.CreateUserID, sqlTrans))
                    {
                        sqlTrans.Commit();
                        return OrderId;
                    }
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
        }

        return OrderId;
    }
}