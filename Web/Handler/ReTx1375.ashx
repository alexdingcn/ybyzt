<%@ WebHandler Language="C#" Class="ReTx1375" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class ReTx1375 : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string username = request["hidUserName"];//登录人名称
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);//登录人对象
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);//代理商对象
        int disid = user.DisID;//代理商ID

        string a2 = Convert.ToString(request["hida2"]);//是否用快捷支付
        string a3 = Convert.ToString(request["hida3"]);//是否用网银
        int KeyID = Convert.ToInt32(request["hidKeyID"]);//企业钱包ID
        if (KeyID == 0)
        {
            string Josn = "{\"msg\":\"系统繁忙，请重新操作！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        int hidFastPay = Convert.ToInt32(request["hidFastPay"]);
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
        decimal price = prepayM.price;
        if (a2 == "1" && a3 == "0")
        {
            int payid = 0;
            int regid = 0;
            try
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
                payModel.OrderID = KeyID;
                payModel.DisID = user.DisID;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = price;
                payModel.guid = Common.Number_repeat(guid);
                payModel.IsAudit = 2;
                payModel.vdef3 = "2";
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = user.ID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = user.ID;
                payid = new Hi.BLL.PAY_Payment().Add(payModel);

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
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
                regModel.LogType = 1375;
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

                // 2.创建交易请求对象
                Tx1375Request tx1375Request = new Tx1375Request();
                tx1375Request.setInstitutionID(institutionID);
                tx1375Request.setOrderNo(orderNo);
                tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);
                tx1375Request.setTxSNBinding(Convert.ToString(hidFastPay));
                tx1375Request.setAmount(amount);
                tx1375Request.setRemark("快捷支付发送短信".ToString());

                // 3.执行报文处理
                tx1375Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1375Request.getRequestMessage(), tx1375Request.getRequestSignature());

                Tx1375Response tx1375Response = new Tx1375Response(respMsg[0], respMsg[1]);
                try
                {
                    Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regModel.Start = tx1375Response.getCode();
                    regModel.ResultMessage = tx1375Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if ("2000".Equals(tx1375Response.getCode()))
                {
                    //支付验证码发送成功
                    string Josn = "{\"payid\":\"" + payid + "\",\"prepayid\":\"" + KeyID + "\",\"falg\":\"2\",\"msg\":\"验证码发送成功！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                else
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + tx1375Response.getMessage() + "\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
            }
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"操作错误！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}