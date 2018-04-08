using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Specialized;
using System.Data.SqlClient;
using DBUtility;
using System.Configuration;
using Com.Alipay;

public partial class Distributor_Pay_RequestPay : System.Web.UI.Page
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.lblMsg.InnerText = Request["msg"];
        LogManager.LogFielPrefix = "jsnx";
        LogManager.LogPath = "C:/requestpag/";
        LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "进入回调页面");
        SortedDictionary<string, string> sPara = GetRequestPost();
        try
        {
            if (sPara == null || sPara.Count <= 0)
            {
                Response.Write("无通知参数");
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:无通知参数");
                return;
            }
            Notify aliNotify = new Notify();
            bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);
            if (!verifyResult)//验证失败
            {
                Response.Write("fail");
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:验证失败");
                return;
            }

            //企业订单号
            string out_trade_no = Common.NoHTML(Request.Form["out_trade_no"]);

            //支付宝交易号
            string trade_no = Common.NoHTML(Request.Form["trade_no"]);

            //交易状态
            string trade_status = Common.NoHTML(Request.Form["trade_status"]);

            decimal prepayPrice = 0;
            string guid = "";
            int orderid = 0;
            Hi.Model.PAY_RegisterLog regM = new Hi.Model.PAY_RegisterLog();
            List<Hi.Model.PAY_RegisterLog> regL = new Hi.BLL.PAY_RegisterLog().GetList("", " Ordercode = '" + out_trade_no + "'", "");
            if (regL == null || regL.Count <= 0)
            {
                Response.Write("找不到" + out_trade_no);
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:找不到" + out_trade_no);
                return;
            }
            regM = regL[0];
            try
            {
                regM.Start = trade_status;
                regM.ResultMessage = trade_no;
                new Hi.BLL.PAY_RegisterLog().Update(regM);
            }
            catch { }
            guid = regM.number;
            orderid = regM.OrderId;
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
            Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
            List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList("", " guid = '" + guid + "' ", "");
            if (payL == null || payL.Count <= 0)
            {
                Response.Write("找不到" + out_trade_no);
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:找不到" + out_trade_no);
                return;
            }
            payM = payL[0];
            if (payM.IsAudit == 1)
            {
                Response.Write("success");//请不要修改或删除,输出success后，支付宝将不再发送通知
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\n支付成功\r\n支付订单号：" + out_trade_no);
                return;
            }
            if (trade_status != "TRADE_SUCCESS")
            {
                Response.Write(GetTradeStatusByName(trade_status));
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:" + GetTradeStatusByName(trade_status) + "\r\n支付订单号：" + out_trade_no);
                return;
            }
            Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
            List<Hi.Model.PAY_PrePayment> prepayL = new Hi.BLL.PAY_PrePayment().GetList("", " vdef4 = '" + payM.ID + "' ", "");
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
                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, payM.PayPrice + prepayPrice, sqlTrans);
                pay = new Hi.BLL.PAY_Payment().updatePayState(con, payM.ID, sqlTrans);
                if (prepayPrice > 0)
                    prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);
                else
                    prepay = 1;
                sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                Response.Write("Error");
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:" + ex.Message + "\r\n支付订单号：" + out_trade_no);
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
                Response.Write("Error");
                LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\nError:支付成功，修改状态失败，等待下次页面通知\r\n支付订单号：" + out_trade_no);
                return;
            }
            
            try
            {
                new Common().GetWxService("2", orderModel.ID.ToString(), "1");
                if (orderModel.Otype != 9)
                {
                    OrderInfoType.AddIntegral(orderModel.CompID, orderModel.DisID, "1", 1, orderModel.ID, (prepayPrice + payM.PayPrice), "订单支付", "", orderModel.CreateUserID);
                }
            }
            catch { }
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "账单支付", "支付：" + (prepayPrice + payM.PayPrice).ToString("0.00") + "元(支付宝支付)", payM.CreateUserID.ToString());
            else
                Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (prepayPrice + payM.PayPrice).ToString("0.00") + "元(支付宝支付)", payM.CreateUserID.ToString());
            
            Response.Write("success");  //请不要修改或删除,输出success后，支付宝将不再发送通知
            LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "退出回调页面\r\n支付成功\r\n支付订单号：" + out_trade_no);
            return;
        }
        catch (Exception ex)
        {
            LogManager.LogFielPrefix = "requestpag";
            LogManager.LogPath = "C:/requestpag/";
            LogManager.WriteLog(LogFile.Error.ToString(), "Error：" + ex.Message + "\r\n");
            Response.Write(ex.Message);
            return;
        }
        finally
        {
            Response.End();
        }
    }

    /// <summary>
    /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestPost()
    {
        string param = "";
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = Request.Form;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            param += requestItem[i] + "=" + Request.Form[requestItem[i]] + "/r/n";
        }
        LogManager.WriteLog(LogFile.Trace.ToString(), "时间：" + DateTime.Now.ToString("yyyy-MMdd HH:mm:ss") + "\r\n" + "进入回调页面");
        return sArray;
    }

    public string GetTradeStatusByName(string str)
    {
        string result = "";
        switch (str)
        {
            case "WAIT_BUYER_PAY":
                result = "交易创建，等待买家付款";
                break;
            case "TRADE_CLOSED":
                result = "未付款交易超时关闭，或支付完成后全额退款";
                break;
            case "TRADE_SUCCESS":
                result = "交易支付成功";
                break;
            case "TRADE_FINISHED":
                result = "交易结束，不可退款";
                break;
            default:
                result = "未知返回状态：" + str;
                break;
        }
        return result;
    }
}