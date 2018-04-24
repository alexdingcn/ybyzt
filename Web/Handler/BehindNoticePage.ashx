<%@ WebHandler Language="C#" Class="BehindNoticePage" %>

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;
using CFCA.Payment.Api;
using BehindUp;
using MY.Client;
using MY.ServiceStack.Common.Types;

public class BehindNoticePage : IHttpHandler
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public void ProcessRequest(HttpContext context)
    {
        Console.WriteLine("---------- Begin [ReceiveNotice] process......");

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        //1 获得参数message和signature
        String message = request.Form["message"];
        String signature = request.Form["signature"];


        Console.WriteLine("[message]=[" + message + "]");
        Console.WriteLine("[signature]=[" + signature + "]");




        ////本地日志记录
        LogManager.LogFielPrefix = "Tz_msg";
        LogManager.LogPath = "D:/后台通知日志/";
        LogManager.WriteLog(LogFile.Trace.ToString(), DateTime.Now.ToString() + "-进入后台启动>>>>>>>>>>>\r\n");



        //2 生成交易结果对象
        NoticeRequest noticeRequest = new NoticeRequest(message, signature);

        LogManager.WriteLog(LogFile.Trace.ToString(), "message：" + message + "\r\n");
        LogManager.WriteLog(LogFile.Trace.ToString(), "signature：" + signature + "\r\n");
        LogManager.WriteLog(LogFile.Trace.ToString(), "noticeRequest.getTxCode值：" + noticeRequest.getTxCode() + "\r\n");

        if ("1318".Equals(noticeRequest.getTxCode()))
        {
            Notice1318Request nr = new Notice1318Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！            
            string guid = nr.getPaymentNo();
            //支付状态
            int status= nr.getStatus();
            //支付金额
            decimal price = nr.getAmount();
            LogManager.WriteLog(LogFile.Trace.ToString(), "guid值：" + guid + "  ;nr.getStatus:" + nr.getStatus() + "机构坐标：" + guid.Contains("MYKJ") + "\r\n");

            //陌远科技流水处理
            if (guid.Contains("MYKJ"))
            {

                LogManager.WriteLog(LogFile.Trace.ToString(), "进入医站通回调程序\r\n");

                string strWhere2 = string.Empty;
                if (guid != "")
                {
                    strWhere2 += " number = '" + guid + "'";
                }
                Hi.Model.PAY_RegisterLog regM = new Hi.Model.PAY_RegisterLog();
                List<Hi.Model.PAY_RegisterLog> regL = new Hi.BLL.PAY_RegisterLog().GetList("", strWhere2, "");
                if (regL == null || regL.Count <= 0)
                {
                    Console.WriteLine("未找到对应的支付记录！");

                }
                regM = regL[0];
                string strWhere = string.Empty;
                if (guid != "")
                {
                    strWhere += " guid = '" + guid + "'";
                }
                strWhere += " and isnull(dr,0)=0";
                Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
                List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList("", strWhere, "");
                if (payL == null || payL.Count <= 0)
                {
                    Console.WriteLine("未找到对应的支付记录！");

                }
                payM = payL[0];

                payM.PayDate = DateTime.Now;
                payM.ts = DateTime.Now;
                payM.status = nr.getStatus();
                new Hi.BLL.PAY_Payment().Update(payM);
                regM.Start = Convert.ToString(nr.getStatus());
                new Hi.BLL.PAY_RegisterLog().Update(regM);

                if (payM.IsAudit == 1)
                {
                    Console.WriteLine("该支付记录状态已经修改成功，请不要重复操作！");

                }
                else
                {

                    if (nr.getStatus() != 20)
                    {
                        Console.WriteLine("支付失败！");

                    }
                    //！！！支付成功 ！！！
                    decimal prepayPrice = 0;

                    //企业钱包充值
                    Hi.Model.PAY_PrePayment prepayMnew = new Hi.Model.PAY_PrePayment();
                    string strWhere3 = string.Empty;
                    if (guid != "")
                    {
                        strWhere3 += " ID = " + payM.OrderID;
                    }
                    strWhere3 += " and isnull(dr,0)=0";
                    List<Hi.Model.PAY_PrePayment> plist = new Hi.BLL.PAY_PrePayment().GetList("", strWhere3, "");
                    if (plist.Count > 0)
                    {
                        prepayMnew = plist[0];
                    }

                    //订单
                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(payM.OrderID);
                    if (orderModel != null)
                    {


                        string strWhere1 = string.Empty;
                        strWhere1 += " vdef4 = '" + payM.ID + "'";
                        strWhere1 += " and isnull(dr,0)=0";
                        Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
                        List<Hi.Model.PAY_PrePayment> prepayL = new Hi.BLL.PAY_PrePayment().GetList("", strWhere1, "");
                        if (prepayL != null && prepayL.Count > 0)
                        {
                            prepayM = prepayL[0];
                            prepayPrice = prepayM.price * -1;
                        }

                        int order = 0;
                        int prepay = 0;
                        int pay = 0;

                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            Hi.Model.Pay_PaymentSettings  Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + orderModel.CompID, "")[0];
                            //手续费收取(0,平台 1，经销商 2，核心企业)
                            string  sxfsq = Convert.ToString(Sysl.pay_sxfsq);
                            if (sxfsq=="2")
                            {
                                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderModel.ID, payM.PayPrice + prepayPrice, sqlTrans);
                            }
                            else
                            {
                                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderModel.ID, payM.PayPrice + prepayPrice - Convert.ToDecimal(payM.vdef5), sqlTrans);
                            }

                            pay = new Hi.BLL.PAY_Payment().updatePayState(con, payM.ID, sqlTrans);

                            //修改免支付次数
                            // try
                            //{
                            //  Common.UpmzfcsByCompid(orderModel.CompID);
                            //}
                            // catch { }

                            if (prepayPrice > 0)
                                prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);
                            else
                                prepay = 1;

                            if (order > 0 && prepay > 0 && pay > 0)
                                sqlTrans.Commit();
                            else
                                sqlTrans.Rollback();

                        }
                        catch
                        {
                            order = 0;
                            prepay = 0;
                            pay = 0;
                            sqlTrans.Rollback();
                        }
                        finally
                        {
                            con.Close();
                        }

                        if (order <= 0 || prepay <= 0 || pay <= 0)
                        {
                            Console.WriteLine("支付成功，但修改支付状态失败，请联系系统管理员，勿重复操作！");

                        }

                        // try
                        // {
                        //     if (orderModel.Otype != 9)
                        //     {
                        //         OrderInfoType.AddIntegral(orderModel.CompID, orderModel.DisID, "1", 1, orderModel.ID, (prepayPrice + (price / 100)), "订单支付", "", orderModel.CreateUserID);
                        //     }
                        // }
                        // catch { }

                        // if (orderModel.Otype == (int)Enums.OType.推送账单)
                        //     Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "账单支付", "支付：" + (prepayPrice + (price / 100)).ToString("0.00") + "元(网银支付" + (price / 100).ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());
                        // else

                        Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (prepayPrice + (price / 100)).ToString("0.00") + "元(网银支付" + (price / 100).ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());

                        //微信和安卓消息推送
                        // try
                        // {
                        //     new Common().GetWxService("2", orderModel.ID.ToString(), "1");
                        //  }
                        //  catch { }

                        LogManager.WriteLog(LogFile.Trace.ToString(), "订单结束" + order + "--" + prepay + "--" + pay + "\r\n");

                    }
                    else if (prepayMnew != null)
                    {
                        int prepay = 0;
                        int pay = 0;
                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            pay = new Hi.BLL.PAY_Payment().updatePayState(con, payM.ID, sqlTrans);
                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayMnew.ID, sqlTrans);
                            if (pay > 0 && prepay > 0)
                                sqlTrans.Commit();
                            else
                                sqlTrans.Rollback();
                        }
                        catch
                        {
                            pay = 0;
                            prepay = 0;
                            sqlTrans.Rollback();
                        }
                        finally
                        {
                            con.Close();
                        }


                        LogManager.WriteLog(LogFile.Trace.ToString(), "充值结束" + prepay + "--" + pay + "\r\n");
                    }
                    else
                    {
                        Console.WriteLine("未找到该支付记录！");
                    }
                }
            }
        }
        else if ("1348".Equals(noticeRequest.getTxCode()))
        {
            Notice1348Request nr = new Notice1348Request(noticeRequest.getDocument());
            //市场订单结算状态变更通知
            string ordercode = nr.getOrderNo();
            int start = nr.getStatus();

            string InstitutionID = nr.getInstitutionID();
            string SerialNumber = nr.getSerialNumber();
            long Amount = nr.getAmount();
            string TransferTime = nr.getTransferTime();

            LogManager.WriteLog(LogFile.Trace.ToString(), "清算订单号：" + ordercode + "\r\n");
            LogManager.WriteLog(LogFile.Trace.ToString(), "清算状态：" + start + "\r\n");

            // LogManager.WriteLog(LogFile.Trace.ToString(), "其他信息：" + InstitutionID + "--" + SerialNumber + "--" + Amount + "--" + TransferTime + "\r\n");
            //陌远科技流水处理
            if (SerialNumber.Contains("MYKJ"))
            {
                LogManager.WriteLog(LogFile.Trace.ToString(), "进入my1818结算程序\r\n");
                //if (40 == start)
                //{
                string sql = string.Format(@"update PAY_PayLog set MarkName='{2}',MarkNumber='{0}' where  number='{1}'", DateTime.Now.ToString(), SerialNumber, start);
                //！！！ 在这里添加企业处理逻辑！！！
                try
                {
                    int num = new Hi.BLL.PAY_PrePayment().Up_PayLog(sql);
                    if (num > 0)
                    {
                        Console.WriteLine("结算状态回调成功！");
                        LogManager.WriteLog(LogFile.Trace.ToString(), "执行结果：回写成功,sql:" + sql + "\r\n");
                    }
                }
                catch
                {
                    Console.WriteLine("结算状态回调失败！");
                    LogManager.WriteLog(LogFile.Trace.ToString(), "执行结果：回写失败,sql:" + sql + "\r\n");
                }
                //}

                LogManager.WriteLog(LogFile.Trace.ToString(), "清算结束\r\n");

            }
        }

        Console.WriteLine("[plainText]=[" + noticeRequest.getPlainText() + "]");

        //4 响应支付平台
        String messageResponse = Convert.ToBase64String(Encoding.UTF8.GetBytes(new NoticeResponse().getMessage()));

        response.Clear();
        response.Write(messageResponse);
        //response.Flush();
        response.End();
        response.Close();

        Console.WriteLine("---------- End [ReceiveNotice] process.");

    }





    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}