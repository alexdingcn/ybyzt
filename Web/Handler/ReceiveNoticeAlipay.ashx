<%@ WebHandler Language="C#" Class="ReceiveNoticeAlipay" %>

using System;
using System.Web;
using Com.Alipay;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data.SqlClient;
using DBUtility;
public class ReceiveNoticeAlipay : IHttpHandler
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        SortedDictionary<string, string> sPara = GetRequestGet();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            Notify aliNotify = new Notify();
            bool verifyResult = aliNotify.Verify(sPara, request.QueryString["notify_id"], request.QueryString["sign"]);

            if (verifyResult)//验证成功
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //请在这里加上商户的业务逻辑程序代码


                //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                //商户订单号
                string out_trade_no = request.QueryString["out_trade_no"];
                //支付宝交易号
                string trade_no = request.QueryString["trade_no"];

                //买家帐号
                string buyer_email = request.QueryString["buyer_email"];

                //支付金额
                string price = request.QueryString["total_fee"];

                //交易状态
                string trade_status = request.QueryString["trade_status"];

                if (request.QueryString["trade_status"] == "TRADE_FINISHED" || request.QueryString["trade_status"] == "TRADE_SUCCESS")
                {
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    string strWhere2 = string.Empty;
                    if (out_trade_no != "")
                    {
                        strWhere2 += " number = '" + out_trade_no + "'";
                    }
                    Hi.Model.PAY_RegisterLog regM = new Hi.Model.PAY_RegisterLog();
                    List<Hi.Model.PAY_RegisterLog> regL = new Hi.BLL.PAY_RegisterLog().GetList("", strWhere2, "");
                    if (regL == null || regL.Count <= 0)
                    {
                        //response.Redirect("../Distributor/Pay/Error.aspx?msg=未找到对应的支付记录！", false);
                        //return;
                        Console.WriteLine("未找到对应的支付记录！");

                    }
                    regM = regL[0];
                    string strWhere = string.Empty;
                    if (out_trade_no != "")
                    {
                        strWhere += " guid = '" + out_trade_no + "'";
                    }
                    strWhere += " and isnull(dr,0)=0";
                    Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
                    List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList("", strWhere, "");
                    if (payL == null || payL.Count <= 0)
                    {
                        //response.Redirect("../Distributor/Pay/Error.aspx?msg=未找到对应的支付记录！", false);
                        // return;
                        Console.WriteLine("未找到对应的支付记录！");

                    }
                    payM = payL[0];

                    payM.PayDate = DateTime.Now;
                    payM.ts = DateTime.Now;
                    payM.status = trade_status == "TRADE_FINISHED" ? 80 : 90;
                    new Hi.BLL.PAY_Payment().Update(payM);
                    regM.Start = trade_status;
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                    if (payM.IsAudit == 1)
                    {
                        Console.WriteLine("该支付记录状态已经修改成功，请不要重复操作！");

                    }

                    //！！！支付成功 ！！！
                    decimal prepayPrice = 0;

                    //企业钱包充值
                    Hi.Model.PAY_PrePayment prepayMnew = new Hi.Model.PAY_PrePayment();
                    string strWhere3 = string.Empty;
                    if (out_trade_no != "")
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
                        decimal prices = Convert.ToDecimal(price);
                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderModel.ID, payM.PayPrice + prepayPrice - Convert.ToDecimal(payM.vdef5), sqlTrans);
                            pay = new Hi.BLL.PAY_Payment().updatePayState(con, payM.ID, sqlTrans);


                            if (prepayPrice > 0)
                                prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);
                            else
                                prepay = 1;
                            sqlTrans.Commit();
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

                        try
                        {
                            //new Common().GetWxService("2", orderModel.ID.ToString(), "1");
                            if (orderModel.Otype != 9)
                            {
                                OrderInfoType.AddIntegral(orderModel.CompID, orderModel.DisID, "1", 1, orderModel.ID, (prepayPrice + (prices / 100)), "订单支付", "", orderModel.CreateUserID);
                            }
                        }
                        catch { }
                        if (orderModel.Otype == (int)Enums.OType.推送账单)
                            Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "账单支付", "支付：" + (prepayPrice + (prices / 100)).ToString("0.00") + "元(网银支付" + (prices / 100).ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());
                        else
                            Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (prepayPrice + (prices / 100)).ToString("0.00") + "元(网银支付" + (prices / 100).ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());

                        //微信和安卓消息推送
                        try
                        {
                            new Common().GetWxService("2", orderModel.ID.ToString(), "1", prepayPrice + (prices / 100));
                        }
                        catch { }

                        //Response.Write("success");  //请不要修改或删除
                        //打印页面
                        //Response.Write("支付成功<br />");
                    }

                }
                else//验证失败
                {
                    // Response.Write("fail");
                    //Response.Write("支付失败");
                   // response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败！", Common.EncryptKey), false);

                }
            }
            else
            {
                //this.Orderlink.PostBackUrl = "../UserCenter/memberorderdetail.aspx";
                //Response.Write("无返回参数");
               // response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败！", Common.EncryptKey), false);

            }

        }
    }
    /// <summary>
    /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestGet()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = HttpContext.Current.Request.QueryString;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
        }

        return sArray;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}