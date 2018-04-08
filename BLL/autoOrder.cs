using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using DBUtility;


/*******
 * 订单自己完成操作
 * by 2016-09-29
 * ********/
namespace Hi.BLL
{
    public static class autoOrder
    {

        /// <summary>
        /// 获取Users表用户名
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        public static string GetUserName(int Id)
        {
            Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(Id);
            if (usersModel != null)
            {
                return usersModel.TrueName != "" ? usersModel.TrueName : usersModel.UserName;
            }
            return "";
        }

        /// <summary>
        /// 已发货超时未签收的订单  自动签收
        /// </summary>
        /// <returns></returns>
        public static string SendShip()
        {
            try
            {
                //系统自动签收天数
                int Sign = System.Configuration.ConfigurationManager.AppSettings["Sign"] != null ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Sign"].ToString()) : 10;

                List<Hi.Model.BD_Company> coml = new Hi.BLL.BD_Company().GetList("ID", " isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and AuditState=2", "");
                //执行sql
                string str = string.Empty;

                if (coml != null && coml.Count > 0)
                {
                    foreach (var icom in coml)
                    {
                        //获取核心企业设置自动签收天数
                        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", "Name='订单自动签收' and CompID=" + icom.ID, "");
                        if (sl != null && sl.Count > 0)
                            Sign = Convert.ToInt32(sl[0].Value);

                        //当前时间
                        DateTime now = DateTime.Now;
                        DateTime oldTime = now.AddDays(-Sign);

                        //用户名ID、Name
                        int UserID = 0;
                        string userName = string.Empty;  //Common.GetUserName(UserID);

                        //查询超过签收天数条件
                        string signstr = " CompID=" + icom.ID + " and isnull(IsSign,0)=0 and SendDate<='" + oldTime + "'";

                        //查询超过签收天数未签收的订单
                        List<Hi.Model.DIS_OrderOut> outl = new Hi.BLL.DIS_OrderOut().GetList("", signstr, "");

                        if (outl == null && outl.Count == 0)
                            return "";

                        foreach (Hi.Model.DIS_OrderOut item in outl)
                        {

                            Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(item.OrderID);
                            List<Hi.Model.DIS_OrderDetail> od = new Hi.BLL.DIS_OrderDetail().GetList("", "OrderID=" + item.OrderID, "");
                            //查询该订单已发货的发货单
                            List<Hi.Model.DIS_OrderOut> lo = new Hi.BLL.DIS_OrderOut().GetList("", " OrderID=" + item.OrderID, "");
                            //订单明细商品总数量
                            decimal goodsnum = 0; //订单商品总数
                            if (od != null && od.Count > 0)
                            {
                                foreach (var item0 in od)
                                    goodsnum += item0.GoodsNum;
                            }

                            decimal outnum = 0;  //已发货商品数量
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
                                        outnum += item2.OutNum;
                                }
                            }

                            int IsOutState = (int)Enums.IsOutState.全部到货;
                            int OState = (int)Enums.OrderState.已到货;
                            //订单
                            //到货签收修改订单状态
                            if (OrderModel.IsOutState == (int)Enums.IsOutState.全部发货)
                            {
                                //已发货数量等于订单商品数量 ==全部到货
                                if (outnum == goodsnum)
                                {
                                    IsOutState = (int)Enums.IsOutState.全部到货;
                                    OState = (int)Enums.OrderState.已到货;
                                }
                                else
                                {
                                    IsOutState = (int)Enums.IsOutState.全部发货;
                                    OState = (int)Enums.OrderState.已到货;
                                }
                            }
                            else if (OrderModel.IsOutState == (int)Enums.IsOutState.部分发货 || OrderModel.IsOutState == (int)Enums.IsOutState.部分到货)
                            {
                                IsOutState = (int)Enums.IsOutState.部分到货;
                                OState = (int)Enums.OrderState.已发货;
                            }

                            //本次签收商品数量
                            List<Hi.Model.DIS_OrderOutDetail> lodd = new Hi.BLL.DIS_OrderOutDetail().GetList("", " OrderOutID=" + item.ID, "");
                            if (lodd != null && lodd.Count > 0 && OrderModel.IsOutState == (int)Enums.IsOutState.全部发货)
                            {
                                foreach (var item3 in lodd)
                                    str += "update DIS_OrderOutDetail set SignNum=OutNum where OrderOutID=" + item.ID + " and ID=" + item3.ID + ";";
                            }

                            userName = GetUserName(OrderModel.DisUserID);
                            UserID = OrderModel.DisUserID;

                            //判断订单状态
                            if (OrderModel.OState >= (int)Enums.OrderState.已发货 && OrderModel.IsOutState == (int)Enums.IsOutState.全部发货 && (OrderModel.ReturnState == (int)Enums.ReturnState.未退货 || OrderModel.ReturnState == (int)Enums.ReturnState.拒绝退货))
                            {
                                //订单状态
                                str += " update Dis_Order set OState=" + OState + ",IsOutState=" + IsOutState + ",modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.OrderID + ";";
                                //签收状态
                                str += " update Dis_OrderOut set SignDate='" + DateTime.Now + "',SignUser='" + userName + "',SignUserId=" + UserID + ",IsSign=1,modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.ID + ";";

                                //自动签收日志
                                str += string.Format("INSERT INTO SYS_SysBusinessLog VALUES({0},'{1}',{2},'{3}','{4}',{5},'{6}','{7}','{8}',{9},{10});", icom.ID, "Order", item.OrderID, "订单自动签收", DateTime.Now, UserID, userName, "", DateTime.Now, 0, UserID);

                            }
                            else
                                continue;
                        }
                    }
                }

                SqlTransaction TranSaction = null;
                SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
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
            catch (Exception ex)
            {
                //错误日志
                string strPath = @"../autoOrder/";
                strPath = strPath + "\\" + DateTime.Now.ToString("yyyyMMhh") + "_服务错误日志" + ".txt";
                StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
                fs.WriteLine(ex.Message);
                fs.WriteLine(ex.Source + "::" + ex.InnerException + "::" + ex.TargetSite);
                fs.WriteLine("**************************************");
                fs.Close();
            }
            return "1";
        }

        /// <summary>
        /// 超时未付款自动作废订单  自动取消
        /// </summary>
        /// <param name="CompID">核心企业ID</param>
        /// <returns></returns>
        public static string OffShip()
        {
            try
            {  
                int off = 30;
                 List<Hi.Model.BD_Company> coml = new Hi.BLL.BD_Company().GetList("ID", " isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and AuditState=2", "");
                //执行sql
                string str = string.Empty;

                if (coml != null && coml.Count > 0)
                {

                    foreach (var icomp in coml)
                    {


                        //获取核心企业设置自动签收天数
                        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", "Name='超时未付款自动作废订单' and CompID=" + icomp.ID, "");
                        if (sl != null && sl.Count > 0)
                            off = Convert.ToInt32(sl[0].Value);

                        //当前时间
                        DateTime now = DateTime.Now;
                        DateTime oldTime = now.AddDays(-off);

                        //用户名ID、Name
                        int UserID = 0;
                        string userName = string.Empty;  //Common.GetUserName(UserID);

                        //查询超时未付款自动作废订单条件  待审核，已审核未支付的订单取消
                        string offstr = " CompID=" + icomp.ID + "and OState in(1,2) and PayState=0 and ts<='" + oldTime + "'";

                     

                        //查询超时未付款自动作废订单的订单
                        List<Hi.Model.DIS_Order> ol = new Hi.BLL.DIS_Order().GetList("", offstr, "");

                        if (ol == null && ol.Count == 0)
                            return "";

                        foreach (Hi.Model.DIS_Order item in ol)
                        {
                            userName = GetUserName(item.DisUserID);
                            UserID = item.DisUserID;

                            //判断订单状态
                            if ((item.OState == (int)Enums.OrderState.待审核 || item.OState == (int)Enums.OrderState.已审) && item.PayState == (int)Enums.PayState.未支付)
                            {
                                //订单状态
                                str += " update Dis_Order set OState=" + (int)Enums.OrderState.已作废 + ",modifyuser=" + UserID + ",ts='" + DateTime.Now + "' where ID=" + item.ID + ";";
                                //超时未付款自动取消
                                str += string.Format("INSERT INTO SYS_SysBusinessLog VALUES({0},'{1}',{2},'{3}','{4}',{5},'{6}','{7}','{8}',{9},{10});", icomp.ID, "Order", item.ID, "超时未付款自动作废", DateTime.Now, UserID, userName, "", DateTime.Now, 0, UserID);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    SqlTransaction TranSaction = null;
                    SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
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
            }
            catch (Exception ex)
            {
                //错误日志
                string strPath = @"../autoOrder/";
                strPath = strPath + "\\" + DateTime.Now.ToString("yyyyMMhh") + "_服务错误日志" + ".txt";
                StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
                fs.WriteLine(ex.Message);
                fs.WriteLine(ex.Source + "::" + ex.InnerException + "::" + ex.TargetSite);
                fs.WriteLine("**************************************");
                fs.Close();
            }
            return "1";
        }
    }
}
