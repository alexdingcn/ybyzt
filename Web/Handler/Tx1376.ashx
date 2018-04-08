<%@ WebHandler Language="C#" Class="Tx1376" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

using System.Web.SessionState;

public class Tx1376 : IHttpHandler, IRequiresSessionState
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();

        try
        {
            string yfk = Convert.ToString(request["hida1"]);//是否用企业钱包  0:否，1:是
            string kjzf = Convert.ToString(request["hida2"]);//是否用快捷支付  0:否，1:是
            string wyzf = Convert.ToString(request["hida3"]);//是否用网银支付  0:否，1:是

            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                string Josn = ErrMessage("请先登录");
                context.Response.Write(Josn);
                return;
            }

            string bankcode = Convert.ToString(request["txtBankCode"]);//帐号号码
            string phoneCode = Convert.ToString(request["txtPhoneNum"]);//检验码
            int hidFastPay = Convert.ToInt32(request["hidFastPay"]);//快捷支付表ID
            decimal price = 0;//使用企业钱包金额
            int KeyID = 0;

            if (request["KeyID"] == "")
            {
                KeyID = 0;
            }
            else
            {
                KeyID = Common.DesDecrypt(request["KeyID"], Common.EncryptKey).ToInt(0);
            }
            Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);//代理商对象
            if (disModel == null)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            int payid = 0;
            decimal payPrice = 0;
            if (request["payid"] == "")
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            payid = Convert.ToInt32(request["payid"].ToString());
            payM = new Hi.BLL.PAY_Payment().GetModel(payid);
            if (payM == null)//快捷支付
            {
                //string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            payPrice = payM.PayPrice;

            int prepayid = 0;
            if (request["prepayid"] != "")//是否使用企业钱包
            {
                prepayid = Convert.ToInt32(request["prepayid"].ToString());
                prepayM = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                if (prepayM != null)
                    price = Math.Abs(prepayM.price);
            }

            int orderid = KeyID;//订单id
            if (orderid <= 0)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
                //string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));
            if (orderModel == null)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }

            if (yfk == "1" && (orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount) <= price)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
                //string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }

            string strWhere = string.Empty;
            if (hidFastPay <= 0)
            {
                //string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;

            }
            strWhere = " ID = '" + hidFastPay + "'";
            List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");

            if (fastList == null || fastList.Count <= 0)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            if (payPrice <= 0)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            int regid = 0;
            //支付日志表
            regModel.OrderId = Convert.ToInt32(orderid);
            regModel.Ordercode = payM.vdef4;// payM.vdef4 + payid.ToString();//支付订单号
            regModel.number = payM.guid;
            regModel.Price = payM.PayPrice;
            regModel.Payuse = "订单支付";
            regModel.PayName = disModel.DisName;
            regModel.DisID = disModel.ID;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = orderModel.Remark;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(orderModel.CompID).CompName;
            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
            regModel.CreateUser = logUser.UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 1376;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (regid <= 0)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }

            if (WebConfigurationManager.AppSettings["Paytest_zj"] != "1")//是否是测试，测试不走支付接口
            {

                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                try
                {
                    PaymentEnvironment.Initialize(configPath);
                }
                catch (Exception ex)
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"支付配置有误，请联系系统管理员！\"}";
                    string Josn = ErrMessage("支付配置有误，请联系系统管理员");
                    context.Response.Write(Josn);
                    context.Response.End();
                }

                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
                string id = fastList[0].ID.ToString();

                Tx1376Request tx1376Request = new Tx1376Request();

                tx1376Request.setInstitutionID(institutionID);
                tx1376Request.setOrderNo(payM.vdef4);//支付订单号 payM.vdef4 + payid.ToString()
                tx1376Request.setPaymentNo(payM.guid);
                tx1376Request.setSmsValidationCode(phoneCode);

                tx1376Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1376Request.getRequestMessage(), tx1376Request.getRequestSignature());

                Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);

                try
                {
                    //返回参数回填
                    regModel.PlanMessage = tx1376Request.getRequestPlainText();
                    regModel.Start = tx1376Response.getCode();
                    regModel.ResultMessage = tx1376Response.getMessage();
                    regModel.Remark = tx1376Response.getResponseMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                    payM = new Hi.BLL.PAY_Payment().GetModel(payid);
                    payM.PayDate = DateTime.Now;
                    payM.ts = DateTime.Now;
                    payM.verifystatus = tx1376Response.getVerifyStatus();
                    payM.status = tx1376Response.getStatus();
                    new Hi.BLL.PAY_Payment().Update(payM);
                }
                catch { }

                if (!("2000".Equals(tx1376Response.getCode())))//返回"2000"接口成功
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"" + tx1376Response.getMessage() + "\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                    string Josn = ErrMessage(tx1376Response.getMessage());
                    context.Response.Write(Josn);
                    return;
                }
                int VerifyStatus = tx1376Response.getVerifyStatus();
                int Status = tx1376Response.getStatus();
                string message = tx1376Response.getMessage();
                if (VerifyStatus != 40 || Status != 20)//VerifyStatus = 40 验证码验证成功，Status = 20 支付成功
                {
                    string Josn = "";
                    if (Status == 10)
                    {
                        //中金：支付处理中，钱会从账户中扣除，第二天会退回到账户中。
                        Josn = ErrMessage("代扣失败");
                        context.Response.Write(Josn);
                        return;
                    }
                    Josn = ErrMessage(tx1376Response.getResponseMessage());
                    context.Response.Write(Josn);
                    return;
                }
            }
            else
            {
                //模拟检验验证码验证成功，回填数据
                try
                {
                    //返回参数回填
                    regModel.PlanMessage = "";
                    regModel.Start = "2000";
                    regModel.ResultMessage = "OK.";
                    regModel.Remark = "";
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                    payM = new Hi.BLL.PAY_Payment().GetModel(payid);
                    payM.PayDate = DateTime.Now;
                    payM.ts = DateTime.Now;
                    payM.verifystatus = 40;//订单支付明细需要用到
                    payM.status = 20;//订单支付明细需要用到
                    new Hi.BLL.PAY_Payment().Update(payM);
                }
                catch { }
            }
            //支付成功,修改状态
            //企业钱包修改状态
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
              if(sxfsq=="2")
                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, payM.PayPrice + price, sqlTrans);//修改订单状态            
              else
                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, payM.PayPrice + price - Convert.ToDecimal(payM.vdef5 == "" ? "0" : payM.vdef5), sqlTrans);//修改订单状态
            
                pay = new Hi.BLL.PAY_Payment().updatePayState(con, payid, sqlTrans);//修改支付表状态
                if (price > 0)
                    prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);//修改企业钱包表状态
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
                string Josn = "{\"error\":\"1\",\"msg\":\"支付成功，但修改支付状态失败，请联系系统管理员，勿重复操作！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
                context.Response.Write(Josn);
                return;
            }
            string Josn1 = string.Empty;
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                Josn1 = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "\"}";

            else
                Josn1 = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "\"}";
            context.Response.Write(Josn1);
            try
            {
                if (orderModel.Otype == (int)Enums.OType.推送账单)//新增账单日志
                    Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "账单支付", "支付：" + (payM.PayPrice + price).ToString("0.00") + "元(快捷支付" + payM.PayPrice.ToString("0.00") + (prepayid > 0 ? "+企业钱包支付" + price.ToString("0.00") : "") + "【含手续费" + payM.vdef5 + "元】)", logUser.UserID.ToString());
                else//新增订单日志
                    Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "支付：" + (payM.PayPrice + price).ToString("0.00") + "元(快捷支付" + payM.PayPrice.ToString("0.00") + (prepayid > 0 ? "+企业钱包支付" + price.ToString("0.00") : "") + "【含手续费" + payM.vdef5 + "元】)", logUser.UserID.ToString());
                if (orderModel.Otype != 9)//新建订单积分
                {
                    OrderInfoType.AddIntegral(logUser.CompID, logUser.DisID, "1", 1, orderid, (payM.PayPrice + price), "订单支付", "", logUser.UserID);
                }
                //微信和安卓消息推送
                try
                {
                    new Common().GetWxService("2", orderModel.ID.ToString(), "1",payM.PayPrice + price);
                }
                catch { }

            }
            catch (Exception ex)
            { }

            return;
        }
        catch (Exception ex)
        {
            string Josn = ErrMessage(ex.Message);
            context.Response.Write(Josn);
            return;
        }
        finally
        {
            context.Response.End();
        }
    }

    public string ErrMessage(string msg)
    {
        return "{\"error\":\"1\",\"msg\":\"" + msg + "！\",\"js\":\"$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);\"}";
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}