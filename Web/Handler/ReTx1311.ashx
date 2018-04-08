<%@ WebHandler Language="C#" Class="ReTx1311" %>

using System;
using System.Web;
using System.Collections.Generic;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class ReTx1311 : IHttpHandler {

    Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
    Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string username = Convert.ToString(request.Form["hidUserName"]);
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);
        int disid = user.DisID;//代理商ID

        string bankid = Convert.ToString(request.Form["hidBank"]);
        string a2 = Convert.ToString(request.Form["hida2"]);//是否用快捷支付
        string a3 = Convert.ToString(request.Form["hida3"]);//是否用网银
        decimal price = 0;//使用企业钱包金额
        int KeyID = Convert.ToInt32(request["hidKeyID"]);//企业钱包ID
        if (KeyID == 0)
        {
            string Josn = "{\"msg\":\"系统繁忙，请稍后操作！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
        price = prepayM.price;
        if (a2 == "0" && a3 == "1")
        {
            int payid = 0;
            int regid = 0;

            try
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                payModel.OrderID = KeyID;
                payModel.DisID = user.DisID;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = price;
                payModel.IsAudit = 2;
                payModel.guid = Common.Number_repeat(guid);
                payModel.vdef3 = "2";
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = user.ID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = user.ID;
                payid = new Hi.BLL.PAY_Payment().Add(payModel);

                regModel.OrderId = KeyID;
                regModel.Ordercode = Convert.ToString(KeyID);
                regModel.number = payModel.guid;
                regModel.Price = price;
                regModel.Payuse = "转账汇款";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disid;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = prepayM.vdef1;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                regModel.CreateUser = user.ID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1311;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (payid > 0 && regid > 0)
            {
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];
                String orderNo = Convert.ToString(KeyID);
                long amount = Convert.ToInt64(price * 100);
                long fee = 0;
                String payerID = disModel.ID.ToString();
                String payerName = disModel.DisName;
                String usage = "支付订单";
                String remark = "订单支付";

                String notificationURL = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port + "/Handler/ReReceiveNoticePage.ashx";
                String payees = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                String bankID = "700";//bankid;
                int accountType = 12;

                // 2.创建交易请求对象
                Tx1311Request tx1311Request = new Tx1311Request();
                tx1311Request.setInstitutionID(institutionID);//机构号码
                tx1311Request.setOrderNo(orderNo);//转账流水号
                tx1311Request.setPaymentNo(payModel.guid);//支付交易流水号
                tx1311Request.setAmount(amount);//支付金额 单位分
                tx1311Request.setFee(fee);//支付服务手续费 单位分
                tx1311Request.setPayerID(payerID);//付款人注册ID
                tx1311Request.setPayerName(payerName);//付款方名称
                tx1311Request.setUsage(usage);//资金用途
                tx1311Request.setRemark(remark);//备注
                tx1311Request.setNotificationURL(notificationURL);//机构接收支付通知的URL
                tx1311Request.addPayee("");//收款方名称
                tx1311Request.setBankID(bankID);//付款银行标识
                tx1311Request.setAccountType(accountType);//付款方帐号类型
                /*
                if (null != payees && payees.Length > 0)
                {
                    String[] payeeList = payees.Split(';');
                    for (int i = 0; i < payeeList.Length; i++)
                    {
                        tx1311Request.addPayee(payeeList[i]);
                    }
                }
                */
                // 3.执行报文处理
                tx1311Request.process();


                // 4.将参数放置到request对象
                // //3个交易参数
                HttpContext.Current.Items["plainText"] = tx1311Request.getRequestPlainText();
                HttpContext.Current.Items["message"] = tx1311Request.getRequestMessage();
                HttpContext.Current.Items["signature"] = tx1311Request.getRequestSignature();
                // //2个信息参数
                HttpContext.Current.Items["txCode"] = "1311";
                HttpContext.Current.Items["txName"] = "市场订单支付直通车";
                // 1个action(支付平台地址)参数
                HttpContext.Current.Items["action"] = PaymentEnvironment.PaymentURL;

                // 5.转向Request.jsp页面
                context.Server.Transfer("../Distributor/Pay/Request.aspx", true);
            }
            else
            {
                string Josn = "{\"msg\":\"系统繁忙，请稍后再试！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}