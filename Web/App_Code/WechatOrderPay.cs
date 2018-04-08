using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using LitJson;

/// <summary>
///WechatOrderPay 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class WechatOrderPay : System.Web.Services.WebService {

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public WechatOrderPay () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "快捷支付绑定（发送短信验证）")]
    public string Tx2531(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        int BankID = 0;
        string BankCode = "";
        string Name = "";
        string IdentityCard = "";
        string Telephone = "";
        JsonData Params = JsonMapper.ToObject(Json);
        JsonData jParams = new JsonData();
        if (Params["from"].ToString() == "android") {
            jParams = JsonMapper.ToObject(AESHelper.Decrypt_android(Params["Json"].ToString()));
        }
        if (jParams["UserID"].ToString() == "" || jParams["ResellerID"].ToString() == "" || jParams["BankID"].ToString() == "" || jParams["BankCode"].ToString() == "" || jParams["Name"].ToString() == "" || jParams["IdentityCard"].ToString() == "" || jParams["Telephone"].ToString() == "")
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
        }
        else
        {
            try
            {
                UserID = Convert.ToInt32(jParams["UserID"].ToString());
                ResellerID = Convert.ToInt32(jParams["ResellerID"].ToString());
                BankID = Convert.ToInt32(jParams["BankID"].ToString());
                BankCode = jParams["BankCode"].ToString();
                Name = jParams["Name"].ToString();
                IdentityCard = jParams["IdentityCard"].ToString();
                Telephone = jParams["Telephone"].ToString();
            }
            catch
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数有误！\"}");
                }
                else
                {
                    return "{\"result\":\"F\",\"Description\":\"参数有误！\"}";
                }
            }
        }
        Hi.Model.PAY_FastPayMent fastpayModel = new Hi.Model.PAY_FastPayMent();
        int id = 0;

        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);
        int disid = user.DisID;//代理商ID
        string banklogo = "../images/" + new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID.ToString()) + ".jpg";//银行logo

        string strWhere = string.Empty;
        if (BankCode != "")
        {
            strWhere += " bankcode = '" + BankCode + "' ";
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"银行卡号必须填写！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"银行卡号必须填写！\"}";
            }
        }
        strWhere += " and DisID = " + user.DisID + " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        if (fastList.Count > 0)
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"银行卡不能重复绑定！\"}");
            }
            else
            { 
                return "{\"result\":\"F\",\"Description\":\"银行卡不能重复绑定！\"}";
            }
        }
        try
        {
            fastpayModel.DisID = disid;
            fastpayModel.BankID = BankID;
            fastpayModel.Number = DateTime.Now.ToLongDateString();
            fastpayModel.AccountName = Name;
            fastpayModel.bankcode = BankCode;
            fastpayModel.bankName = new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID.ToString());
            fastpayModel.IdentityCode = IdentityCard;
            fastpayModel.phone = Telephone;
            fastpayModel.BankLogo = banklogo;
            fastpayModel.Start = 2;
            fastpayModel.vdef6 = "0";
            fastpayModel.CreateUser = Convert.ToInt32(user.ID.ToString());
            fastpayModel.CreateDate = DateTime.Now;
            fastpayModel.ts = DateTime.Now;
            fastpayModel.modifyuser = Convert.ToInt32(user.ID.ToString());
            id = new Hi.BLL.PAY_FastPayMent().Add(fastpayModel);
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
        if (id > 0)
        {
            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            PaymentEnvironment.Initialize(configPath);

            string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码

            Tx2531Request tx2531Request = new Tx2531Request();
            tx2531Request.setInstitutionID(institutionID);
            tx2531Request.setTxSNBinding(id.ToString());
            tx2531Request.setBankID(BankID.ToString());
            tx2531Request.setAccountName(Name);
            tx2531Request.setAccountNumber(BankCode);
            tx2531Request.setIdentificationType("0".ToString());
            tx2531Request.setIdentificationNumber(IdentityCard);
            tx2531Request.setPhoneNumber(Telephone);
            tx2531Request.setCardType("10".ToString());

            tx2531Request.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx2531Request.getRequestMessage(), tx2531Request.getRequestSignature());

            Tx2531Response tx2531Response = new Tx2531Response(respMsg[0], respMsg[1]);

            try
            {
                Hi.Model.PAY_FastPayMent fModel = new Hi.BLL.PAY_FastPayMent().GetModel(id);
                fModel.vdef1 = tx2531Response.getCode();
                fModel.vdef2 = tx2531Response.getMessage();
                fModel.ts = DateTime.Now;
                fModel.modifyuser = user.ID;
                new Hi.BLL.PAY_FastPayMent().Update(fModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if ("2000".Equals(tx2531Response.getCode()))
            {
                //企业根据自己的业务要求编写相应的业务处理代码
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"T\",\"FastPaymentBankID\":\"" + id + "\",\"Description\":\"验证码发送成功！\"}");
                }
                else
                { 
                    return "{\"Result\":\"T\",\"FastPaymentBankID\":\"" + id + "\",\"Description\":\"验证码发送成功！\"}";
                }
            }
            else
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"" + tx2531Response.getMessage() + "！\"}");
                }
                else
                { 
                    return "{\"Result\":\"F\",\"Description\":\"" + tx2531Response.getMessage() + "！\"}";
                }
            }
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}");
            }
            else
            { 
                return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
            }
        }
    }
    [WebMethod(Description = "快捷支付绑定（验证并绑定）")]
    public string Tx2532(string Json)
    {
        int UserID=0;
        int ResellerID=0; 
        int FastPaymentBankID=0;
        string MessageCode = "";
        JsonData Params = JsonMapper.ToObject(Json);
        JsonData jParams = new JsonData();
        if (Params["from"].ToString() == "android")
        {
            jParams = JsonMapper.ToObject(AESHelper.Decrypt_android(Params["Json"].ToString()));
        }
        if (jParams["UserID"].ToString() == "" || jParams["ResellerID"].ToString() == "" || jParams["FastPaymentBankID"].ToString() == "" || jParams["MessageCode"].ToString() == "")
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
        }
        else
        {
            try
            {
                UserID = Convert.ToInt32(jParams["UserID"].ToString());
                ResellerID = Convert.ToInt32(jParams["ResellerID"].ToString());
                FastPaymentBankID = Convert.ToInt32(jParams["FastPaymentBankID"].ToString());
                MessageCode = jParams["MessageCode"].ToString();
            }
            catch
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"参数有误！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
                }
            }
        }
        if (FastPaymentBankID > 0)
        {
            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            PaymentEnvironment.Initialize(configPath);

            string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
            Tx2532Request tx2532Request = new Tx2532Request();
            tx2532Request.setInstitutionID(institutionID);
            tx2532Request.setTxSNBinding(FastPaymentBankID.ToString());
            tx2532Request.setSMSValidationCode(MessageCode);

            tx2532Request.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx2532Request.getRequestMessage(), tx2532Request.getRequestSignature());
            Tx2532Response tx2532Response = new Tx2532Response(respMsg[0], respMsg[1]);

            try
            {
                Hi.Model.PAY_FastPayMent fModel = new Hi.BLL.PAY_FastPayMent().GetModel(FastPaymentBankID);
                fModel.vdef3 = tx2532Response.getCode();
                fModel.vdef4 = tx2532Response.getMessage();
                fModel.verifystatus = tx2532Response.getVerifyStatus();
                fModel.status = tx2532Response.getStatus();
                fModel.ts = DateTime.Now;
                fModel.modifyuser = UserID;
                new Hi.BLL.PAY_FastPayMent().Update(fModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if ("2000".Equals(tx2532Response.getCode()))
            {
                //绑定成功，修改状态
                int VerifyStatus = tx2532Response.getVerifyStatus();
                int Status = tx2532Response.getStatus();
                if (VerifyStatus == 40)
                {
                    if (Status == 30)
                    {
                        bool falg = false;
                        try
                        {
                            Hi.Model.PAY_FastPayMent fastModel = new Hi.BLL.PAY_FastPayMent().GetModel(FastPaymentBankID);
                            fastModel.Start = 1;
                            fastModel.ts = DateTime.Now;
                            fastModel.modifyuser = UserID;
                            falg = new Hi.BLL.PAY_FastPayMent().Update(fastModel);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if (falg)
                        {
                            if (Params["from"].ToString() == "android")
                            {
                                return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"绑定成功！\"}");
                            }
                            else
                            {
                                return "{\"Result\":\"T\",\"Description\":\"绑定成功！\"}";
                            }
                        }
                        else
                        {
                            if (Params["from"].ToString() == "android")
                            {
                                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"绑定失败！\"}");
                            }
                            else
                            {
                                return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                            }
                        }
                    }
                    else
                    {
                        if (Params["from"].ToString() == "android")
                        {
                            return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"绑定失败！\"}");
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                        }
                    }
                }
                else if (VerifyStatus == 20)
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码超时！\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"验证码超时！\"}";
                    }
                }
                else if (VerifyStatus == 30)
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码错误！\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"验证码错误！\"}";
                    }
                }
                else
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"绑定失败！\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                    }
                }
            }
            else
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"" + tx2532Response.getMessage() + "\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"" + tx2532Response.getMessage() + "\"}";
                }
            }
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"先获取手机验证码！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"先获取手机验证码！\"}";
            }
        }
    }
    [WebMethod(Description = "订单快捷支付（发送短信验证）")]
    public string Tx1375(string Json)
    {
        int UserID=0;
        int ResellerID=0;
        string ReceiptNo="";
        int FastPaymentBankID=0;
        decimal PrePrice=0;
        decimal FastpayPrice=0;
        string Prepassword = "";
        JsonData Params = JsonMapper.ToObject(Json);
        JsonData jParams = new JsonData();
        if (Params["from"].ToString() == "android")
        {
            jParams = JsonMapper.ToObject(AESHelper.Decrypt_android(Params["Json"].ToString()));
        }
        if (jParams["UserID"].ToString() == "" || jParams["ResellerID"].ToString() == "" || jParams["ReceiptNo"].ToString() == "" || jParams["FastPaymentBankID"].ToString() == "" || jParams["PrePrice"].ToString() == "" || jParams["FastpayPrice"].ToString() == "")
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
        }
        else
        {
            try
            {
                UserID = Convert.ToInt32(jParams["UserID"].ToString());
                ResellerID = Convert.ToInt32(jParams["ResellerID"].ToString());
                ReceiptNo = jParams["ReceiptNo"].ToString();
                FastPaymentBankID = Convert.ToInt32(jParams["FastPaymentBankID"].ToString());
                PrePrice = Convert.ToDecimal(jParams["PrePrice"].ToString());
                FastpayPrice = Convert.ToDecimal(jParams["FastpayPrice"].ToString());
                Prepassword = jParams["Prepassword"].ToString();
            }
            catch
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"参数有误！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
                }
            }
        }
        int payid = 0;
        int prepayid = 0;
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);//代理商对象
        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包

        string strWhere = " 1=1 ";
        if (ReceiptNo != "" && ReceiptNo != null)
        {
            strWhere += " and ReceiptNo=" + ReceiptNo;
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单编号必须填写！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"订单编号必须填写！\"}";
            }
        }
        strWhere += "and Otype!=9 and isnull(dr,0)=0 and PayState=0";
        List<Hi.Model.DIS_Order> orderList = new Hi.BLL.DIS_Order().GetList("",strWhere,"");
        if (orderList.Count == 0)
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单！\"}";
            }
        }

        Hi.Model.DIS_Order orderModel = orderList[0];
        if (orderModel.AuditAmount + orderModel.OtherAmount == 0)
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单,订单金额为0！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单,订单金额为0！\"}";
            }
        }
        if (orderModel.AuditAmount + orderModel.OtherAmount != PrePrice + FastpayPrice)
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付金额不正确！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"订单支付金额不正确！\"}";
            }
        }
        int orderid = orderModel.ID;
        bool flag = false;
        if (PrePrice>0)
        {
            //企业钱包处理
            if ((orderModel.AuditAmount + orderModel.OtherAmount) < PrePrice)
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"使用企业钱包大于订单金额！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"使用企业钱包大于订单金额！\"}";
                }
            }
            if (sumPrice < PrePrice)
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"企业钱包余额不足！\"}");
                }
                else
                {
                    return "{\"result\":\"F\",\"Description\":\"企业钱包余额不足！\"}";
                }
            }
            else
            {
                if (disModel.Paypwd == Util.md5(Prepassword))
                {
                    Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
                    string strWhere1 = string.Empty;
                    if (orderid != 0)
                    {
                        strWhere1 += " OrderID = '" + orderid + "'";
                    }
                    strWhere1 += " and IsEnabled=1 and isnull(dr,0)=0";
                    List<Hi.Model.PAY_PrePayment> prepayList = new Hi.BLL.PAY_PrePayment().GetList("", strWhere1, "");
                    try
                    {
                        if (prepayList.Count > 0)
                        {
                            prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(prepayList[0].ID);
                            prepayModel.price = PrePrice * -1;
                            prepayModel.ts = DateTime.Now;
                            prepayModel.modifyuser = UserID;
                            flag = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
                            if (flag)
                            {
                                prepayid = prepayList[0].ID;
                            }
                        }
                        else
                        {
                            prepayModel.CompID = disModel.CompID;
                            prepayModel.DisID = disModel.ID;
                            prepayModel.OrderID = orderid;
                            prepayModel.Start = 2;
                            prepayModel.PreType = 5;
                            prepayModel.price = PrePrice * -1;
                            prepayModel.Paytime = DateTime.Now;
                            prepayModel.CreatDate = DateTime.Now;
                            prepayModel.CrateUser = UserID;
                            prepayModel.AuditState = 2;
                            prepayModel.IsEnabled = 1;
                            prepayModel.ts = DateTime.Now;
                            prepayModel.modifyuser = UserID;
                            prepayModel.vdef1 = "订单支付";
                            prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    int prepay = 0;
                    int order = 0;
                    if (prepayid > 0 && (orderModel.AuditAmount + orderModel.OtherAmount) == PrePrice)
                    {

                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
                            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, PrePrice, sqlTrans);

                            sqlTrans.Commit(); ;
                        }
                        catch
                        {
                            prepay = 0;
                            order = 0;
                            sqlTrans.Rollback();
                        }
                        finally
                        {
                            con.Close();
                        }
                        if (prepay > 0 && order > 0)
                        {
                            //Utils.AddSysBusinessLog(disModel.CompID, "Order", orderid.ToString(), "订单支付", "");
                            try
                            {
                                new Common().GetWxService("2", orderModel.ID.ToString(), "1");
                            }
                            catch
                            {
                            }
                            finally
                            {
                                Utils.AddSysBusinessLog(disModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "");
                            }
                            if (Params["from"].ToString() == "android")
                            {
                                return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"订单支付成功！\"}");
                            }
                            else
                            {
                                return "{\"Result\":\"T\",\"Description\":\"订单支付成功！\"}";
                            }
                        }
                        else
                        {
                            if (Params["from"].ToString() == "android")
                            {
                                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付失败！\"}");
                            }
                            else
                            {
                                return "{\"Result\":\"F\",\"Description\":\"订单支付失败！\"}";
                            }
                        }
                    }
                }
                else
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"密码不正确！\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"密码不正确！\"}";
                    }
                }
            }
        }

        if (FastpayPrice > 0)
        {
            int regid = 0;
            Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
            try
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                payModel.OrderID = orderid;
                payModel.DisID = ResellerID;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = (orderModel.AuditAmount + orderModel.OtherAmount) - PrePrice;
                payModel.guid = Common.Number_repeat(guid);
                payModel.IsAudit = 2;
                payModel.vdef3 = "1";
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = UserID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = UserID;
                payid = new Hi.BLL.PAY_Payment().Add(payModel);

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = Convert.ToInt32(orderid);
                regModel.Ordercode = orderModel.ReceiptNo;
                regModel.number = payModel.guid;
                regModel.Price = (orderModel.AuditAmount + orderModel.OtherAmount) - PrePrice;
                regModel.Payuse = "订单支付";
                regModel.PayName = disModel.DisName;
                regModel.DisID = ResellerID;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = orderModel.Remark;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(FastPaymentBankID).BankID.ToString();
                regModel.CreateUser = UserID;
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
                String orderNo = orderModel.ReceiptNo.Trim().ToString();
                long amount = Convert.ToInt64(((orderModel.AuditAmount + orderModel.OtherAmount) - PrePrice) * 100);

                // 2.创建交易请求对象
                Tx1375Request tx1375Request = new Tx1375Request();
                tx1375Request.setInstitutionID(institutionID);
                tx1375Request.setOrderNo(orderNo);
                tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);
                tx1375Request.setTxSNBinding(Convert.ToString(FastPaymentBankID));
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
                    regModel.PlanMessage = tx1375Request.getRequestPlainText();
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
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"验证码发送成功！\",\"PayNumb\":\"" + new Hi.BLL.PAY_Payment().GetModel(payid).guid + "\",\"PayID\":\"" + payid + "\",\"PrepayID\":\"" + prepayid + "\",\"FastPaymentBankID\":\"" + FastPaymentBankID + "\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"T\",\"Description\":\"验证码发送成功！\",\"PayNumb\":\"" + new Hi.BLL.PAY_Payment().GetModel(payid).guid + "\",\"PayID\":\"" + payid + "\",\"PrepayID\":\"" + prepayid + "\",\"FastPaymentBankID\":\"" + FastPaymentBankID + "\"}";
                    }
                }
                else
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"" + tx1375Response.getMessage() + "\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"" + tx1375Response.getMessage() + "\"}";
                    }
                }
            }
            else
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码发送失败！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"验证码发送失败！\"}";
                }
            }
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码发送失败！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"验证码发送失败！\"}";
            }
        }
    }
    [WebMethod(Description = "订单快捷支付（验证并支付）")]
    public string Tx1376(string Json)
    {
        int UserID=0;
        int ResellerID=0;
        string ReceiptNo="";
        string PayNumb="";
        string MessageCode="";
        int PayID=0;
        int PrepayID=0;
        int FastPaymentBankID = 0;
        JsonData Params = JsonMapper.ToObject(Json);
        JsonData jParams = new JsonData();
        if (Params["from"].ToString() == "android")
        {
            jParams = JsonMapper.ToObject(AESHelper.Decrypt_android(Params["Json"].ToString()));
        }
        if (jParams["UserID"].ToString() == "" || jParams["ResellerID"].ToString() == "" || jParams["ReceiptNo"].ToString() == "" || jParams["PayNumb"].ToString() == "" || jParams["MessageCode"].ToString() == "" || jParams["PayID"].ToString() == "" || jParams["PrepayID"].ToString() == "" || jParams["FastPaymentBankID"].ToString() == "")
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
        }
        else
        {
            try
            {
                UserID = Convert.ToInt32(jParams["UserID"].ToString());
                ResellerID = Convert.ToInt32(jParams["ResellerID"].ToString());
                ReceiptNo = jParams["ReceiptNo"].ToString();
                PayNumb = jParams["PayNumb"].ToString();
                MessageCode = jParams["MessageCode"].ToString();
                PayID = Convert.ToInt32(jParams["PayID"].ToString());
                PrepayID = Convert.ToInt32(jParams["PrepayID"].ToString());
                FastPaymentBankID = Convert.ToInt32(jParams["FastPaymentBankID"].ToString());
            }
            catch
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"参数有误！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
                }
            }
        }
        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);//代理商对象
        decimal price = 0;//使用企业钱包金额
        if (PayID > 0)
        {
            payModel = new Hi.BLL.PAY_Payment().GetModel(PayID);
        }
        if (PrepayID > 0)
        {
            prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(PrepayID);
            price = prepayModel.price * -1;
        }
        string strWhere = " 1=1 ";
        if (ReceiptNo != "" && ReceiptNo != null)
        {
            strWhere += " and ReceiptNo=" + ReceiptNo;
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单编号必须填写！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"订单编号必须填写！\"}";
            }
        }
        strWhere += "and Otype!=9 and isnull(dr,0)=0 and PayState=0";
        List<Hi.Model.DIS_Order> orderList = new Hi.BLL.DIS_Order().GetList("", strWhere, "");
        if (orderList.Count == 0)
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"该订单为无效的支付订单！\"}";
            }
        }

        Hi.Model.DIS_Order orderModel = orderList[0];

        if (payModel != null)
        {
            int regid = 0;

            try
            {
                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = Convert.ToInt32(orderModel.ID);
                regModel.Ordercode = orderModel.ReceiptNo;
                regModel.number = PayNumb;
                regModel.Price = (orderModel.AuditAmount + orderModel.OtherAmount) - price;
                regModel.Payuse = "订单支付";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disModel.ID;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = orderModel.Remark;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(FastPaymentBankID).BankID.ToString();
                regModel.CreateUser = UserID;
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
                tx1376Request.setOrderNo(orderModel.ReceiptNo.Trim().ToString());
                tx1376Request.setPaymentNo(PayNumb);
                tx1376Request.setSmsValidationCode(MessageCode);

                tx1376Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1376Request.getRequestMessage(), tx1376Request.getRequestSignature());

                Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);

                try
                {
                    Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regModel.PlanMessage = tx1376Request.getRequestPlainText();
                    regModel.Start = tx1376Response.getCode();
                    regModel.ResultMessage = tx1376Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                    Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetModel(PayID);
                    payM.PayDate = DateTime.Now;
                    payM.ts = DateTime.Now;
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
                            if (prepayModel != null)
                            {
                                bool f = false;
                                try
                                {
                                    Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(PrepayID);
                                    prepayModel.Start = Convert.ToInt32(Enums.PrePayMentState.处理中);
                                    f = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (f)
                                {
                                    if (Params["from"].ToString() == "android")
                                    {
                                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}");
                                    }
                                    else
                                    {
                                        return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                                    }
                                }
                                else
                                {
                                    if (Params["from"].ToString() == "android")
                                    {
                                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}");
                                    }
                                    else
                                    {
                                        return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
                                    }
                                }
                            }
                            else
                            {
                                if (Params["from"].ToString() == "android")
                                {
                                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}");
                                }
                                else
                                {
                                    return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                                }
                            }
                        }
                        if (Status == 20)
                        {
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
                                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderModel.ID, payModel.PayPrice + price, sqlTrans);
                                if (prepayModel == null)
                                {
                                    prepay = 1;
                                    pay = new Hi.BLL.PAY_Payment().updatePayState(con, PayID, sqlTrans);
                                }
                                if (prepayModel != null)
                                {
                                    if ((orderModel.AuditAmount + orderModel.OtherAmount) == price)
                                    {
                                        prepay = 1;
                                        pay = 1;
                                    }
                                    else if ((orderModel.AuditAmount + orderModel.OtherAmount) > price)
                                    {
                                        if (price == 0)
                                        {
                                            prepay = 1;
                                        }
                                        else
                                        {
                                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, PrepayID, sqlTrans);
                                        }
                                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, PayID, sqlTrans);
                                    }
                                    else
                                    {
                                        sqlTrans.Rollback();
                                    }
                                }
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

                            if (order > 0 && prepay > 0 && pay > 0)
                            {
                                try
                                {
                                    new Common().GetWxService("2", orderModel.ID.ToString(), "1");
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    Utils.AddSysBusinessLog(disModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "");
                                }
                                if (Params["from"].ToString() == "android")
                                {
                                    return AESHelper.Encrypt_android("{\"Result\":\"t\",\"Description\":\"订单支付成功！\"}");
                                }
                                else
                                {
                                    return "{\"Result\":\"t\",\"Description\":\"订单支付成功！\"}";
                                }
                            }
                            else
                            {
                                if (Params["from"].ToString() == "android")
                                {
                                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}");
                                }
                                else
                                {
                                    return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                                }
                            }
                        }
                        else
                        {
                            if (Params["from"].ToString() == "android")
                            {
                                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}");
                            }
                            else
                            {
                                return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                            }
                        }
                    }
                    else if (VerifyStatus == 20)
                    {
                        if (Params["from"].ToString() == "android")
                        {
                            return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码超时！\"}");
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"验证码超时！\"}";
                        }
                    }
                    else if (VerifyStatus == 30)
                    {
                        if (Params["from"].ToString() == "android")
                        {
                            return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"验证码错误！\"}");
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"验证码错误！\"}";
                        }
                    }
                    else
                    {
                        if (Params["from"].ToString() == "android")
                        {
                            return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}");
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
                        }
                    }
                }
                else
                {
                    if (Params["from"].ToString() == "android")
                    {
                        return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"" + tx1376Response.getMessage() + "\"}");
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"" + tx1376Response.getMessage() + "\"}";
                    }
                }
            }
            else
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
                }
            }
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
            }
        }
    }
    [WebMethod(Description = "查询绑定银行卡")]
    public string GetFastpayBank(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        JsonData Params = JsonMapper.ToObject(Json);
        JsonData jParams = new JsonData();
        if (Params["from"].ToString() == "android")
        {
            jParams = JsonMapper.ToObject(AESHelper.Decrypt_android(Params["Json"].ToString()));
        }
        if (jParams["UserID"].ToString() == "" || jParams["ResellerID"].ToString() == "")
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
            }
            else
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
        }
        else
        {
            try
            {
                UserID = Convert.ToInt32(jParams["UserID"].ToString());
                ResellerID = Convert.ToInt32(jParams["ResellerID"].ToString());
            }
            catch
            {
                if (Params["from"].ToString() == "android")
                {
                    return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}");
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
                }
            }
        }
        Hi.Model.BD_Distributor disModel = new Hi.Model.BD_Distributor();
        if (ResellerID > 0)
        {
            disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"ResellerID参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"ResellerID参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
            }
        }
        if (disModel == null) {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"该代理商不存在！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}");
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"该代理商不存在！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
            }
        }
        int PrePwdIsDeafult = 0;
        if (disModel.Paypwd == Util.md5(""))
        {
            PrePwdIsDeafult = 1;
        }
        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(ResellerID, disModel.CompID);//剩余企业钱包
        string strWhere = " 1=1 ";
        if (ResellerID > 0)
        {
            strWhere += " and DisID = " + ResellerID;
        }
        strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        if (fastList.Count > 0)
        {
            BLL.Service.PAY_FastPayMent fastBll = new BLL.Service.PAY_FastPayMent();
            List<BLL.Service.PAY_FastPayMent.FastPayMent> FastPayMentList = fastBll.GetFastPayMent(fastList);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string BankCardList = js.Serialize(FastPayMentList);
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"成功！\",\"BankCardList\":" + BankCardList + ",\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}");
            }
            else
            {
                return "{\"Result\":\"T\",\"Description\":\"成功！\",\"BankCardList\":" + BankCardList + ",\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}";
            }
            //return "";
        }
        else
        {
            if (Params["from"].ToString() == "android")
            {
                return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"该代理商没有绑定快捷支付！\",\"BankCardList\":[],\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}");
            }
            else
            {
                return "{\"Result\":\"T\",\"Description\":\"该代理商没有绑定快捷支付！\",\"BankCardList\":[],\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}";
            }
        }
    }
}
