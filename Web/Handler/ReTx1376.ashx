<%@ WebHandler Language="C#" Class="ReTx1376" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class ReTx1376 : IHttpHandler {

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string a2 = Convert.ToString(request["hida2"]);//是否用企业钱包
        string a3 = Convert.ToString(request["hida3"]);//是否用企业钱包
        string username = Convert.ToString(request["hidUserName"]);//登录人名称
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);//登录人对象
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);//代理商对象
        string bankcode = Convert.ToString(request["txtBankCode"]);//帐号号码
        string phoneCode = Convert.ToString(request["txtPhoneNum"]);//检验码
        decimal price = 0;//使用企业钱包金额
        int hidPay = Convert.ToInt32(request["hidPay"]);//支付表ID
        if (hidPay > 0)
        {
            payM = new Hi.BLL.PAY_Payment().GetModel(hidPay);
        }
        int hidPrepay = Convert.ToInt32(request["hidPrepay"]);//企业钱包表ID
        if (hidPrepay > 0)
        {
            prepayM = new Hi.BLL.PAY_PrePayment().GetModel(Convert.ToInt32(hidPrepay));
            price = prepayM.price;
        }
        int hidFastPay = Convert.ToInt32(request["hidFastPay"]);
        if (a2 == "1" && a3 == "0")
        {
            int regid = 0;
            try
            {
                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = hidPrepay;
                regModel.Ordercode = Convert.ToString(hidPrepay);
                regModel.number = payM.guid;
                regModel.Price = price;
                regModel.Payuse = "转账汇款";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disModel.ID;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = prepayM.vdef1;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                regModel.CreateUser = user.ID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1376;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (regid > 0)
            {
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
                
                Tx1376Request tx1376Request = new Tx1376Request();

                tx1376Request.setInstitutionID(institutionID);
                tx1376Request.setOrderNo(Convert.ToString(hidPrepay));
                tx1376Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(Convert.ToInt32(hidPay)).guid);
                tx1376Request.setSmsValidationCode(phoneCode);

                tx1376Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1376Request.getRequestMessage(), tx1376Request.getRequestSignature());

                bool f = JudgeStatus(respMsg,hidPay,hidPrepay,regid);
                if (f)
                {
                    string Josn = "{\"msg\":\"转账汇款成功,等待审核！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                else
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                /*
                Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);
                
                try
                {
                    Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regModel.Start = tx1376Response.getCode();
                    regModel.ResultMessage = tx1376Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                    Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetModel(hidPay);
                    payM.verifystatus = tx1376Response.getVerifyStatus();
                    payM.status = tx1376Response.getStatus();
                    new Hi.BLL.PAY_Payment().Update(payM);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if ("2000".Equals(tx1376Response.getCode()))
                {
                    int VerifyStatus = tx1376Response.getVerifyStatus();
                    int Status = tx1376Response.getStatus();
                    if (VerifyStatus == 40)
                    {
                        if (Status == 20)
                        {
                            int prepay = 0;
                            int pay = 0;
                            SqlConnection con = new SqlConnection(LocalSqlServer);
                            con.Open();
                            SqlTransaction sqlTrans = con.BeginTransaction();
                            try
                            {
                                pay = new Hi.BLL.PAY_Payment().updatePayState(con, hidPay, sqlTrans);
                                prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, hidPrepay, sqlTrans);
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
                            if (pay > 0 && prepay > 0)
                            {
                                string Josn = "{\"msg\":\"转账成功！\"}";
                                context.Response.Write(Josn);
                                context.Response.End();
                            }
                            else
                            {
                                string Josn = "{\"error\":\"1\",\"msg\":\"转账失败！\"}";
                                context.Response.Write(Josn);
                                context.Response.End();
                            }
                        }
                        else if(Status == 10)
                        {
                            
                        }
                        else
                        {
                            string Josn = "{\"error\":\"1\",\"msg\":\"" + tx1376Response.getResponseMessage() + "\"}";
                            context.Response.Write(Josn);
                            context.Response.End();
                        }
                    }
                    else
                    {
                        string Josn = "{\"error\":\"1\",\"msg\":\"" + tx1376Response.getResponseMessage() + "\"}";
                        context.Response.Write(Josn);
                        context.Response.End();
                    }
                }
                else
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"转账失败！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                */
            }
            else
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
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

    int i = 0;
    /// <summary>
    /// 处理接口返回值
    /// </summary>
    /// <param name="respMsg">Tx1376Request返回信息</param>
    /// <param name="hidPay">支付表ID</param>
    /// <param name="hidPrepay">企业钱包ID</param>
    /// <returns></returns>
    public bool JudgeStatus(String[] respMsg ,int hidPay,int hidPrepay,int regid)
    {
        if (i > 3) {
            return false;
        }
        i++;
        Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);

        try
        {
            Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
            regModel.Start = tx1376Response.getCode();
            regModel.ResultMessage = tx1376Response.getMessage();
            new Hi.BLL.PAY_RegisterLog().Update(regModel);
            Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetModel(hidPay);
            payM.verifystatus = tx1376Response.getVerifyStatus();
            payM.status = tx1376Response.getStatus();
            new Hi.BLL.PAY_Payment().Update(payM);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if ("2000".Equals(tx1376Response.getCode()))
        {
            int VerifyStatus = tx1376Response.getVerifyStatus();
            int Status = tx1376Response.getStatus();
            if (VerifyStatus == 40)
            {
                if (Status == 10)
                {
                    return JudgeStatus(respMsg, hidPay, hidPrepay, regid);
                }
                else if (Status == 20)
                {
                    int prepay = 0;
                    int pay = 0;
                    SqlConnection con = new SqlConnection(LocalSqlServer);
                    con.Open();
                    SqlTransaction sqlTrans = con.BeginTransaction();
                    try
                    {
                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, hidPay, sqlTrans);
                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, hidPrepay, sqlTrans);
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
                    if (pay > 0 && prepay > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return false;
    }

}