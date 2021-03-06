﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipay;
using System.Data.SqlClient;
using DBUtility;
using WxPayAPI;
using System.Xml.Linq;

/// <summary>
/// 功能：页面跳转同步通知页面
/// 版本：3.3
/// 日期：2012-07-10
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////////页面功能说明///////////////////////
/// 该页面可在本机电脑测试
/// 可放入HTML等美化页面的代码、商户业务逻辑程序代码
/// 该页面可以使用ASP.NET开发工具调试，也可以使用写文本函数LogResult进行调试
/// </summary>

public partial class Distributor_pay_ReceiveNoticeWxpay : System.Web.UI.Page
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    protected void Page_Load(object sender, EventArgs e)
    {

        //ResultNotify resultNotify = new ResultNotify(this);
        //resultNotify.ProcessNotify();
        string resultFromWx = getPostStr();
        //设置支付参数  
        var res = XDocument.Parse(resultFromWx);

        string return_code = res.Element("xml").Element("return_code").Value;

        string out_trade_no = res.Element("xml").Element("out_trade_no").Value;

        string result_code = res.Element("xml").Element("result_code").Value;

        string price = res.Element("xml").Element("total_fee").Value;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //请在这里加上商户的业务逻辑程序代码
        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

        if (return_code == "SUCCESS" || result_code == "SUCCESS")
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
            payM.status = 0;//trade_status == "TRADE_FINISHED" ? 80 : 90;
            new Hi.BLL.PAY_Payment().Update(payM);
            regM.Start = "";// trade_status;
            new Hi.BLL.PAY_RegisterLog().Update(regM);
            if (payM.IsAudit == 2)//该支付记录未修改成功进入，修改成功就不进入，请不要重复操作
            {
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
                            OrderInfoType.AddIntegral(orderModel.CompID, orderModel.DisID, "1", 1, orderModel.ID, (prepayPrice + prices), "订单支付", "", orderModel.CreateUserID);
                        }
                    }
                    catch { }
                    if (orderModel.Otype == (int)Enums.OType.推送账单)
                        Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "账单支付", "支付：" + (prepayPrice + prices).ToString("0.00") + "元(微信支付" + prices.ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());
                    else
                        Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (prepayPrice + prices).ToString("0.00") + "元(微信支付" + prices.ToString("0.00") + (prepayM.ID > 0 ? "+企业钱包支付" + prepayPrice.ToString("0.00") : "") + "【含手续费" + Convert.ToDecimal(payM.vdef5).ToString("0.00") + "元】)", payM.CreateUserID.ToString());

                    //微信和安卓消息推送
                    try
                    {
                        new Common().GetWxService("2", orderModel.ID.ToString(), "1", prepayPrice + prices);
                    }
                    catch { }


                    Response.Write("success");  //请不要修改或删除
                }//钱包充值
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
                        sqlTrans.Commit();
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
                }
                else
                {
                    //response.Redirect("../Distributor/Pay/Error.aspx?msg=未找到该支付订单！", false);
                    //return;

                    Console.WriteLine("未找到该支付记录！");
                }
            }
            else 
            {
                Console.WriteLine("success");
            }


            Response.Write("success");

        }
        else//验证失败
        {
            Response.Write("fail");
        }

    }

    //获得Post过来的数据  
    public string getPostStr()
    {
        Int32 intLen = Convert.ToInt32(Request.InputStream.Length);
        byte[] b = new byte[intLen];
        Request.InputStream.Read(b, 0, intLen);
        return System.Text.Encoding.UTF8.GetString(b);
    }

}
