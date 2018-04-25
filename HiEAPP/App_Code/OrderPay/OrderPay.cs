using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using LitJson;
using System.Data.SqlClient;

using System.Web.Configuration;
using DBUtility;
using System.Data;
using System.Collections;

using Com.Alipay;
using CFCA.Payment.Api;

/// <summary>
///OrderPay 的摘要说明
/// </summary>
public class OrderPay
{
    public OrderPay()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public string Getzfsxf(string Json)
    {
        Common.CatchInfo(Json, "Getzfsxf", "1");
        int CompID = 0;
        decimal price = 0;
        JsonData Params = JsonMapper.ToObject(Json);


        try
        {
            if (Params["CompID"].ToString() == "" || Params["price"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                CompID = Convert.ToInt32(Params["CompID"].ToString());//企业ID
                price = Convert.ToDecimal(Params["price"].ToString());//支付金额
            }
        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }


        decimal sxf = 0;
        string sxfsq = "-1";//手续费收取方
        //查询该企业的设置
        if (price > 0)
        {
            List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompID, "");
            if (Sysl.Count > 0)
            {
                //手续费收取
                sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

                //支付方式--线上or线下
                string zffs = Convert.ToString(Sysl[0].pay_zffs);

                //免手续费支付次数
                int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

                //快捷支付比例
                decimal kjzfbl = Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000;
                decimal kjzfstart = Convert.ToDecimal(Sysl[0].pay_kjzfstart);
                decimal kjzfend = Convert.ToDecimal(Sysl[0].pay_kjzfend);


                //手续费收取方
                if (sxfsq == "1" && mfcs <= 0)
                {
                    sxf = price * kjzfbl;
                    if (sxf <= kjzfstart)
                        sxf = kjzfstart;
                    else if (sxf >= kjzfend)
                        sxf = kjzfend;
                    //计算手续费
                    sxf = Math.Round(sxf, 2);
                } //收取核心企业手续费 (没有免支付次数时，才计算手续费)
                else if (sxfsq == "2" && mfcs <= 0)
                {

                    sxf = price * kjzfbl;
                    if (sxf <= kjzfstart)
                        sxf = kjzfstart;
                    else if (sxf >= kjzfend)
                        sxf = kjzfend;

                    if (sxf > price)//支付金额小于手续费时，提示不允许支付。
                    {
                        return "{\"Result\":\"F\",\"Description\":\"支付金额小于手续费，不允许支付！\"}";
                    }

                    sxf = 0;
                }
            }
        }


        return "{\"Result\":\"T\",\"price\":\"" + price + "\",\"ShouXuFeiPrice\":\"" + sxf + "\",\"sxfsqf\":\"" + sxfsq + "\",\"Description\":\"手续费计算成功！\"}";

    }


    public string Tx2531(string Json)
    {
        Common.CatchInfo(Json, "Tx2531", "1");
        int UserID = 0;
        int ResellerID = 0;
        int BankID = 0;
        string BankCode = "";
        string Name = "";
        string IdentityCard = "";
        string Telephone = "";
        JsonData Params = JsonMapper.ToObject(Json);


        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["BankID"].ToString() == "" || Params["BankCode"].ToString() == "" || Params["Name"].ToString() == "" || Params["IdentityCard"].ToString() == "" || Params["Telephone"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                BankID = Convert.ToInt32(Params["BankID"].ToString());//银行ID，参考《银行编码表》
                BankCode = Params["BankCode"].ToString();//账户号码
                BankCode = BankCode.Replace(" ", "");
                Name = Params["Name"].ToString();//账户名称
                IdentityCard = Params["IdentityCard"].ToString();//省份证号码
                Telephone = Params["Telephone"].ToString();//手机号码
            }
        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }

        Hi.Model.PAY_FastPayMent fastpayModel = new Hi.Model.PAY_FastPayMent();
        int id = 0;

        // Hi.Model.SYS_CompUser CompUser = Common.Get_CompUser(UserID);
        int disid = ResellerID;//经销商ID
        string banklogo = WebConfigurationManager.AppSettings["ImgViewPath"] + "BankImg/" + new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID.ToString()) + ".jpg";//"../images/" + Common.GetBankName(BankID) + ".jpg";//银行logo

        string strWhere = string.Empty;
        if (BankCode != "")
        {
            strWhere += " bankcode = '" + BankCode + "' ";
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"银行卡号必须填写！\"}";
        }
        strWhere += " and DisID = " + ResellerID + " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        if (fastList.Count > 0)
        {
            return "{\"Result\":\"F\",\"Description\":\"银行卡不能重复绑定！\"}";
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
            fastpayModel.CreateUser = Convert.ToInt32(UserID.ToString());
            fastpayModel.CreateDate = DateTime.Now;
            fastpayModel.ts = DateTime.Now;
            fastpayModel.modifyuser = Convert.ToInt32(UserID.ToString());
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
            tx2531Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + id.ToString());
            tx2531Request.setBankID(BankID.ToString());
            tx2531Request.setAccountName(Name);
            tx2531Request.setAccountNumber(BankCode);
            tx2531Request.setIdentificationType("0".ToString());
            tx2531Request.setIdentificationNumber(IdentityCard);
            tx2531Request.setPhoneNumber(Telephone);
            tx2531Request.setCardType("10".ToString());//卡类型，10=个人借记，20=个人贷记

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
                fModel.modifyuser = UserID;
                new Hi.BLL.PAY_FastPayMent().Update(fModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if ("2000".Equals(tx2531Response.getCode()))
            {
                return "{\"Result\":\"T\",\"FastPaymentBankID\":\"" + id + "\",\"Description\":\"验证码发送成功！\"}";
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"" + tx2531Response.getMessage() + "！\"}";
            }
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
        }
    }

    public string Tx2532(string Json)
    {
        Common.CatchInfo(Json, "Tx2532", "1");
        int UserID = 0;
        int ResellerID = 0;
        int FastPaymentBankID = 0;
        string MessageCode = "";
        JsonData Params = JsonMapper.ToObject(Json);

        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["FastPaymentBankID"].ToString() == "" || Params["MessageCode"].ToString() == "")
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                FastPaymentBankID = Convert.ToInt32(Params["FastPaymentBankID"].ToString());//快捷支付表ID
                MessageCode = Params["MessageCode"].ToString();//手机验证码
            }
        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }

        if (FastPaymentBankID > 0)
        {
            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            PaymentEnvironment.Initialize(configPath);

            string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
            Tx2532Request tx2532Request = new Tx2532Request();
            tx2532Request.setInstitutionID(institutionID);
            tx2532Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + FastPaymentBankID.ToString());
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
                            return "{\"Result\":\"T\",\"Description\":\"绑定成功！\"}";
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                        }
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                    }
                }
                else if (VerifyStatus == 20)
                {
                    return "{\"Result\":\"F\",\"Description\":\"验证码超时！\"}";
                }
                else if (VerifyStatus == 30)
                {
                    return "{\"Result\":\"F\",\"Description\":\"验证码错误！\"}";
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"绑定失败！\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"" + tx2532Response.getMessage() + "\"}";
            }
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"先获取手机验证码！\"}";
        }
    }
    //{"UserID":"","ResellerID":"","ReceiptNo":"","FastPaymentBankID":"","PrePrice":"","FastpayPrice":"","Prepassword":""}
    public string Tx1375(string Json)
    {
        Common.CatchInfo(Json, "Tx1375", "1");
        int UserID = 0;
        int ResellerID = 0;

        int CompID = 0;//核心企业ID

        string ReceiptNo = "";
        int FastPaymentBankID = 0;
        decimal PrePrice = 0;
        decimal FastpayPrice = 0;
        string Prepassword = "";
        string ShouXuFeiPrice = string.Empty;
        string sxfsqf = string.Empty;
        JsonData Params = JsonMapper.ToObject(Json);

        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["ReceiptNo"].ToString() == "" || Params["FastPaymentBankID"].ToString() == "" || Params["PrePrice"].ToString() == "" || Params["FastpayPrice"].ToString() == "" || Params["ShouXuFeiPrice"].ToString() == "" || Params["sxfsqf"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;// Convert.ToInt32(Params["CompID"].ToString());//核心企业ID
                ReceiptNo = Params["ReceiptNo"].ToString();//市场订单号
                FastPaymentBankID = Convert.ToInt32(Params["FastPaymentBankID"].ToString());//快捷支付表ID
                PrePrice = Convert.ToDecimal(Params["PrePrice"].ToString());//使用企业钱包金额
                FastpayPrice = Convert.ToDecimal(Params["FastpayPrice"].ToString());//使用快捷支付金额
                Prepassword = Params["Prepassword"].ToString();//企业钱包支付密码
                ShouXuFeiPrice = Convert.ToString(Params["ShouXuFeiPrice"]);//支付手续费
                sxfsqf = Convert.ToString(Params["sxfsqf"]);//手续费收取方
            }
        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }

        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);//登录人对象

        string password = new Hi.BLL.BD_Distributor().GetModel(ResellerID).Paypwd;

        int payid = 0;
        int prepayid = 0;

        string strWhere = " 1=1 and ReceiptNo='" + ReceiptNo + "' and isnull(dr,0)=0";
        Hi.Model.DIS_Order orderM = new Hi.Model.DIS_Order();
        List<Hi.Model.DIS_Order> orderL = new Hi.BLL.DIS_Order().GetList("", strWhere, "");
        if (orderL.Count > 0)
        {
            orderM = orderL[0];
        }

        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);//经销商对象
        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID,CompID);//剩余企业钱包 
        int disid = ResellerID;//经销商ID

        string payPas = Prepassword;

        if (PrePrice > 0)
        {
            if (password == Common.md5("123456"))
            {
                return "{\"Result\":\"F\",\"Description\":\"请先修改支付密码！\"}";
            }
            else
            {
                if (payPas == "")
                {
                    return "{\"Result\":\"F\",\"Description\":\"密码不能为空！\"}";
                }
            }
        }
        decimal price = PrePrice;

        decimal txtPayOrder = PrePrice + FastpayPrice;

        decimal payPrice = FastpayPrice;

        int orderid = orderM.ID;//订单id

        Hi.Model.DIS_Order orderModel = orderM;
        if (txtPayOrder > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
        {
            return "{\"Result\":\"F\",\"Description\":\"支付金额大于订单未支付金额，不能支付！\"}";
        }
        if (!((//&& orderModel.Otype != (int)Enums.OType.推送账单
            (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (orderModel.Otype != (int)Enums.OType.赊销订单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付))
            )
            && orderModel.OState != (int)Enums.OrderState.已作废))
        {
            return "{\"Result\":\"1\",\"Description\":\"订单异常，不能支付！\"}";
        }
        int hidFastPay = FastPaymentBankID;
        if (hidFastPay > 0)
        {
            Hi.Model.PAY_FastPayMent fastM = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay);
            if (fastM != null)
            {
                if (fastM.DisID != ResellerID)
                {
                    return "{\"Result\":\"F\",\"Description\":\"操作有误！\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"操作有误！\"}";
            }
        }

        string strWhere1 = " 1=1 ";
        if (orderid != 0)
        {
            strWhere1 += " OrderID = '" + orderid + "'";
        }
        strWhere1 += " and IsEnabled=1 and Start=1 and isnull(dr,0)=0";

        if (price > 0)
        {
            //企业钱包处理
            if (sumPrice < price)
            {
                return "{\"Result\":\"F\",\"Description\":\"企业钱包余额不足！\"}";
            }
            else
            {
                if (disModel.Paypwd == Common.md5(payPas))
                {
                    Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
                    try
                    {
                        prepayModel.CompID = CompID;//核心企业Id
                        prepayModel.DisID = disModel.ID;
                        prepayModel.OrderID = orderid;
                        prepayModel.Start = 2;
                        prepayModel.PreType = 5;
                        prepayModel.price = price * -1;
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
                    catch
                    {

                    }
                    int prepay = 0;
                    int order = 0;
                    if (prepayid > 0 && payPrice == 0)
                    {

                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
                            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price, sqlTrans);
                            Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", sqlTrans);
                            sqlTrans.Commit();
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
                            new MsgSend().GetWxService("2", orderModel.ID.ToString(), "1", price);
                            return "{\"Result\":\"T\",\"Description\":\"订单支付成功！\"}";
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"订单支付失败！\"}";
                        }
                    }
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"密码不正确！\"}";
                }
            }
        }

        if (payPrice > 0)
        {
            int regid = 0;
            Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
            string strWhere2 = string.Empty;
            if (orderid != 0)
            {
                strWhere2 += " OrderID = " + orderid;
            }
            strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
            String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");
            try
            {
                #region  计算手续费 start

                decimal sxf = 0;
                string sxfsq = "-1";//手续费收取方
                //查询该企业的设置
                if (payPrice > 0)
                {
                    List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + orderM.CompID, "");
                    if (Sysl.Count > 0)
                    {
                        //手续费收取
                        sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

                        //支付方式--线上or线下
                        string zffs = Convert.ToString(Sysl[0].pay_zffs);

                        //免手续费支付次数
                        int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

                        //快捷支付比例
                        decimal kjzfbl = Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000;
                        decimal kjzfstart = Convert.ToDecimal(Sysl[0].pay_kjzfstart);
                        decimal kjzfend = Convert.ToDecimal(Sysl[0].pay_kjzfend);


                        //手续费收取方
                        if (sxfsq == "1" && mfcs <= 0)
                        {
                            sxf = payPrice * kjzfbl;
                            if (sxf <= kjzfstart)
                                sxf = kjzfstart;
                            else if (sxf >= kjzfend)
                                sxf = kjzfend;
                            //计算手续费
                            sxf = Math.Round(sxf, 2);
                        }
                        //收取核心企业手续费 (没有免支付次数时，才计算手续费)
                        else if (sxfsq == "2" && mfcs <= 0)
                        {

                            sxf = payPrice * kjzfbl;
                            if (sxf <= kjzfstart)
                                sxf = kjzfstart;
                            else if (sxf >= kjzfend)
                                sxf = kjzfend;

                            if (sxf > payPrice)//支付金额小于手续费时，提示不允许支付。
                            {
                                return "{\"Result\":\"F\",\"Description\":\"支付金额小于手续费，不允许支付！\"}";
                            }

                            sxf = 0;
                        }
                    }
                    if (sxf != Convert.ToDecimal(ShouXuFeiPrice))
                        return "{\"Result\":\"F\",\"Description\":\"支付手续费不一致！\"}";
                    payPrice = payPrice + sxf;
                }


                #endregion  计算手续费   end




                string guid = Guid.NewGuid().ToString().Replace("-", "");
                payModel.OrderID = orderid;
                payModel.DisID = ResellerID;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = payPrice;
                payModel.guid = Common.Number_repeat(guid);
                payModel.IsAudit = 2;

                payModel.vdef3 = "1";
                payModel.vdef4 = orderNo;
                payModel.State = Convert.ToInt32(sxfsqf);//手续费收取方
                payModel.vdef5 = sxf.ToString();//支付手续费
                payModel.Channel = "1";//支付渠道

                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = UserID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = UserID;
                payid = new Hi.BLL.PAY_Payment().Add(payModel);

                if (prepayid > 0)
                {
                    Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                    prepayMent.vdef4 = payid.ToString();
                    new Hi.BLL.PAY_PrePayment().Update(prepayMent);
                }

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = Convert.ToInt32(orderid);
                regModel.Ordercode = orderNo;// orderNo + payid.ToString();
                regModel.number = payModel.guid;
                regModel.Price = payPrice;
                regModel.Payuse = "订单支付";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disid;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = orderModel.Remark;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
                regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
                regModel.CreateUser = UserID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1375;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
            }
            catch
            {

            }
            if (payid > 0 && regid > 0)
            {
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];

                long amount = Convert.ToInt64(payPrice * 100);

                // 2.创建交易请求对象
                Tx1375Request tx1375Request = new Tx1375Request();
                tx1375Request.setInstitutionID(institutionID);
                tx1375Request.setOrderNo(orderNo); //orderNo + payid.ToString()
                tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);
                tx1375Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidFastPay));
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
                    return "{\"Result\":\"T\",\"Description\":\"验证码发送成功！\",\"PayNumb\":\"" + new Hi.BLL.PAY_Payment().GetModel(payid).guid + "\",\"PayID\":\"" + payid + "\",\"PrepayID\":\"" + prepayid + "\",\"FastPaymentBankID\":\"" + FastPaymentBankID + "\"}";
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"" + tx1375Response.getMessage() + "\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"验证码发送失败！\"}";
            }
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
        }
    }
    //{"UserID":"","ResellerID":"","ReceiptNo":"","PayNumb":"","MessageCode":"","PayID":"","PrepayID":"","FastPaymentBankID":""}
    public string Tx1376(string Json)
    {
        Common.CatchInfo(Json, "Tx1376", "1");
        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;

        string ReceiptNo = "";
        string PayNumb = "";
        string MessageCode = "";
        int PayID = 0;
        int PrepayID = 0;
        int FastPaymentBankID = 0;
        JsonData Params = JsonMapper.ToObject(Json);

        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["ReceiptNo"].ToString() == "" || Params["PayNumb"].ToString() == "" || Params["MessageCode"].ToString() == "" || Params["PayID"].ToString() == "" || Params["PrepayID"].ToString() == "" || Params["FastPaymentBankID"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                ReceiptNo = Params["ReceiptNo"].ToString();//市场订单号
                PayNumb = Params["PayNumb"].ToString();//支付交易流水号
                MessageCode = Params["MessageCode"].ToString();//手机验证码
                PayID = Convert.ToInt32(Params["PayID"].ToString());//支付表ID
                PrepayID = Convert.ToInt32(Params["PrepayID"].ToString());//企业钱包表ID
                FastPaymentBankID = Convert.ToInt32(Params["FastPaymentBankID"].ToString());//快捷支付表ID
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID
            }
        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);//登录人对象
        string phoneCode = MessageCode;//检验码
        decimal price = 0;//使用企业钱包金额

        string strWhere = " 1=1 and ReceiptNo='" + ReceiptNo + "' and isnull(dr,0)=0";
        Hi.Model.DIS_Order orderM = new Hi.Model.DIS_Order();
        List<Hi.Model.DIS_Order> orderL = new Hi.BLL.DIS_Order().GetList("", strWhere, "");
        if (orderL.Count > 0)
        {
            orderM = orderL[0];
        }
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);//经销商对象

        Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
        Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
        int payid = 0;
        decimal payPrice = 0;
        if (PayID > 0)
        {
            payid = PayID;
            payM = new Hi.BLL.PAY_Payment().GetModel(payid);
            if (payM != null)
                payPrice = payM.PayPrice;
        }
        int prepayid = 0;
        if (PrepayID > 0)
        {
            prepayid = PrepayID;
            prepayM = new Hi.BLL.PAY_PrePayment().GetModel(Convert.ToInt32(prepayid));
            if (prepayM != null)
                price = prepayM.price * -1;
        }
        int hidFastPay = FastPaymentBankID;//快捷支付表ID

        int orderid = orderM.ID;//订单id
        Hi.Model.DIS_Order orderModel = orderM;


        string strWhere1 = string.Empty;
        if (hidFastPay > 0)
        {
            strWhere1 = " ID = '" + hidFastPay + "'";
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
        }
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere1, "");

        if (fastList.Count > 0)
        {
            if (payPrice > 0)
            {
                int regid = 0;

                try
                {
                    Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                    regModel.OrderId = Convert.ToInt32(orderid);
                    regModel.Ordercode = payM.vdef4;// payM.vdef4 + payid.ToString();
                    regModel.number = payM.guid;
                    regModel.Price = payM.PayPrice;
                    regModel.Payuse = "订单支付";
                    regModel.PayName = disModel.DisName;
                    regModel.DisID = disModel.ID;
                    regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    regModel.Remark = orderModel.Remark;
                    regModel.DisName = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
                    regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
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
                    string id = fastList[0].ID.ToString();

                    Tx1376Request tx1376Request = new Tx1376Request();

                    tx1376Request.setInstitutionID(institutionID);
                    tx1376Request.setOrderNo(payM.vdef4);// payM.vdef4+ payid.ToString()
                    tx1376Request.setPaymentNo(payM.guid);
                    tx1376Request.setSmsValidationCode(phoneCode);

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
                        payM = new Hi.BLL.PAY_Payment().GetModel(payid);
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
                                return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
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
                                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price + payM.PayPrice - Convert.ToDecimal(payM.vdef5), sqlTrans);
                                    pay = new Hi.BLL.PAY_Payment().updatePayState(con, payid, sqlTrans);
                                    if (price > 0)
                                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
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

                                if (order > 0 && prepay > 0 && pay > 0)
                                {
                                    //Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (price + payPrice).ToString("0.00") + "元", sqlTrans);

                                    if (orderModel.Otype == (int)Enums.OType.推送账单)//新增账单日志
                                        Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "账单支付", "支付：" + (payPrice + price).ToString("0.00") + "元(快捷支付" + (prepayid > 0 ? "+企业钱包支付" : "") + "【含手续费" + payM.vdef5 + "元】)");
                                    else//新增订单日志
                                        Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (payPrice + price).ToString("0.00") + "元(快捷支付" + (prepayid > 0 ? "+企业钱包支付" : "") + "【含手续费" + payM.vdef5 + "元】)");

                                    new MsgSend().GetWxService("2", orderModel.ID.ToString(), "1", price + payM.PayPrice - Convert.ToDecimal(payM.vdef5));

                                    return "{\"Result\":\"T\",\"Description\":\"订单支付成功！\"}";
                                }
                                else
                                {
                                    return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                                }
                            }
                            else
                            {
                                return "{\"Result\":\"F\",\"Description\":\"订单支付失败，请重新支付！\"}";
                            }
                        }
                        else if (VerifyStatus == 20)
                        {
                            return "{\"Result\":\"F\",\"Description\":\"验证码超时！\"}";
                        }
                        else if (VerifyStatus == 30)
                        {
                            return "{\"Result\":\"F\",\"Description\":\"验证码错误！\"}";
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
                        }
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"" + tx1376Response.getMessage() + "\"}";
                    }
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
            }
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"系统繁忙，请稍后！\"}";
        }
    }
    public string GetFastpayBank(string Json)
    {
        Common.CatchInfo(Json, "GetFastpayBank", "1");
        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;
        JsonData Params = JsonMapper.ToObject(Json);


        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "")
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID
            }
        }
        catch
        {

            return "{\"Result\":\"F\",\"Description\":\"参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
        }

        Hi.Model.BD_Distributor disModel = new Hi.Model.BD_Distributor();
        if (ResellerID > 0)
        {
            disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"ResellerID参数有误！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
        }
        if (disModel == null)
        {
            return "{\"Result\":\"F\",\"Description\":\"该经销商不存在！\",\"BankCardList\":\"\",\"PrepayPrice\":\"" + 0 + "\",\"PrePwdIsDeafult\":\"" + 0 + "\"}";
        }
        int PrePwdIsDeafult = 0;
        if (disModel.Paypwd == Common.md5("123456"))
        {
            PrePwdIsDeafult = 1;
        }
        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(ResellerID, CompID);//剩余企业钱包
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

            return "{\"Result\":\"T\",\"Description\":\"成功！\",\"BankCardList\":" + BankCardList + ",\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}";
        }
        else
        {
            return "{\"Result\":\"T\",\"Description\":\"该经销商没有绑定快捷支付！\",\"BankCardList\":[],\"PrepayPrice\":\"" + sumPrice.ToString("0.00") + "\",\"PrePwdIsDeafult\":\"" + PrePwdIsDeafult + "\"}";
        }
    }

    public string ReTx1375(string Json)
    {
        string phone = "";

        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;
        int hidFastPay = 0;
        decimal price = 0;
        string remark = "";
        int PreType = 0;
        string ShouXuFeiPrice = "0";
        string sxfsqf = string.Empty;
        Hi.Model.SYS_Users user = null;
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(ResellerID);
        try
        {
            JsonData Params = JsonMapper.ToObject(Json);
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["hidFastPay"].ToString() == "" || Params["price"].ToString() == "" || Params["PreType"].ToString() == "" || Params["ShouXuFeiPrice"].ToString() == "" || Params["sxfsqf"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                hidFastPay = Convert.ToInt32(Params["hidFastPay"].ToString());//快捷支付表ID
                PreType = Convert.ToInt32(Params["PreType"].ToString());//支付类型，转账汇款 || 企业钱包充值
                remark = Params["remark"].ToString();//备注
                ShouXuFeiPrice = Convert.ToString(Params["ShouXuFeiPrice"]);//支付手续费
                sxfsqf = Convert.ToString(Params["sxfsqf"]);//手续费收取方
                price = Convert.ToDecimal(Params["price"].ToString());//金额
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID

                user = new Hi.BLL.SYS_Users().GetModel(UserID);

                #region  计算手续费 start

                decimal sxf = 0;
                string sxfsq = "-1";//手续费收取方
                //查询该企业的设置
                if (price > 0)
                {
                    List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" +CompID, "");
                    if (Sysl.Count > 0)
                    {
                        //手续费收取
                        sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

                        //支付方式--线上or线下
                        string zffs = Convert.ToString(Sysl[0].pay_zffs);

                        //免手续费支付次数
                        int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

                        //快捷支付比例
                        decimal kjzfbl = Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000;
                        decimal kjzfstart = Convert.ToDecimal(Sysl[0].pay_kjzfstart);
                        decimal kjzfend = Convert.ToDecimal(Sysl[0].pay_kjzfend);


                        //手续费收取方
                        if (sxfsq == "1" && mfcs <= 0)
                        {
                            sxf = price * kjzfbl;
                            if (sxf <= kjzfstart)
                                sxf = kjzfstart;
                            else if (sxf >= kjzfend)
                                sxf = kjzfend;
                            //计算手续费
                            sxf = Math.Round(sxf, 2);
                        }
                    }
                    if (sxf != Convert.ToDecimal(ShouXuFeiPrice))
                        return "{\"Result\":\"F\",\"Description\":\"支付手续费不一致！\"}";
                    price = price + sxf;
                }


                #endregion  计算手续费   end




            }
        }
        catch
        {

            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        if (user == null)
        {
            return "{\"Result\":\"F\",\"Description\":\"操作人不存在！\"}";
        }
        int id = 0;

        try
        {
            Hi.Model.PAY_PrePayment pM = new Hi.Model.PAY_PrePayment();
            pM.CompID =CompID;
            pM.DisID = ResellerID;
            pM.OrderID = 0;
            pM.Start = 2;
            if (PreType == 1 || PreType == 6)
            {
                pM.PreType = PreType;
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"参数传输有误！\"}";
            }
            pM.price = price - Convert.ToDecimal(ShouXuFeiPrice);//预付款金额=充值总金额-手续费;
            pM.Paytime = DateTime.Now;
            pM.CreatDate = DateTime.Now;
            pM.OldId = 0;
            pM.CrateUser = UserID;
            pM.AuditState = 2;
            pM.AuditUser = 0;
            pM.IsEnabled = 1;
            pM.ts = DateTime.Now;
            pM.modifyuser = UserID;
            pM.vdef1 = remark;
            id = new Hi.BLL.PAY_PrePayment().Add(pM);
        }
        catch { }
        if (id > 0)
        {
            if (hidFastPay > 0)
            {
                phone = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).phone;
                //this.phone.InnerHtml = "（已发送至" + phone.Substring(0, 3) + " **** " + phone.Substring(phone.Length - 4, 4) + "）";
            }
            int KeyID = id;
            Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);

            int payid = 0;
            int regid = 0;
            try
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
                payModel.OrderID = KeyID;
                payModel.DisID = ResellerID;
                payModel.PayUser = new Hi.BLL.BD_Distributor().GetModel(ResellerID).DisName;
                payModel.PayPrice = price;
                payModel.guid = Common.Number_repeat(guid);
                payModel.IsAudit = 2;
                payModel.vdef3 = "2";
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = UserID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = UserID;

                payModel.State = Convert.ToInt32(sxfsqf);//手续费收取方
                payModel.vdef5 = ShouXuFeiPrice;//支付手续费
                payModel.Channel = "1";//支付渠道

                payid = new Hi.BLL.PAY_Payment().Add(payModel);

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = KeyID;
                regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
                regModel.number = payModel.guid;
                regModel.Price = price;
                regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
                regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(ResellerID).DisName;
                regModel.DisID = ResellerID;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = prepayM.vdef1;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
                regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
                regModel.CreateUser = UserID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1375;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
            }
            catch { }
            if (payid > 0 && regid > 0)
            {
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];
                String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
                long amount = Convert.ToInt64(price * 100);

                // 2.创建交易请求对象
                Tx1375Request tx1375Request = new Tx1375Request();
                tx1375Request.setInstitutionID(institutionID);
                tx1375Request.setOrderNo(orderNo);
                tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);
                tx1375Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidFastPay));
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
                    //支付验证码发送成功
                    //string Josn = "{\"payid\":\"" + payid + "\",\"prepayid\":\"" + KeyID + "\",\"falg\":\"2\",\"msg\":\"验证码发送成功！\"}";
                    return "{\"Result\":\"T\",\"Description\":\"验证码发送成功！\",\"payid\":\"" + payid + "\",\"prepayid\":\"" + id + "\",\"hidFastPay\":\"" + hidFastPay + "\"}";
                }
                else
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"" + tx1375Response.getMessage() + "\"}";
                    return "{\"Result\":\"F\",\"Description\":\"" + tx1375Response.getMessage() + "！\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"保存失败！\"}";
            }
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"保存失败！\"}";
        }
    }

    public string ReTx1376(string Json)
    {
        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
        Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
        Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
        int UserID = 0;
        int ResellerID = 0;
        string phoneCode = "";//检验码
        int hidPay = 0;//支付表ID
        int hidFastPay = 0;
        int hidPrepay = 0;//企业钱包表ID
        int CompID = 0;//核心企业Id


        try
        {
            JsonData Params = JsonMapper.ToObject(Json);
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["hidFastPay"].ToString() == "" || Params["phoneCode"].ToString() == "" || Params["payid"].ToString() == "" || Params["prepayid"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                hidFastPay = Convert.ToInt32(Params["hidFastPay"].ToString());//快捷支付表ID
                phoneCode = Params["phoneCode"].ToString();//手机验证码
                hidPay = Convert.ToInt32(Params["payid"].ToString());//支付表ID
                hidPrepay = Convert.ToInt32(Params["prepayid"].ToString());//企业钱包表ID
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID
            }
        }
        catch
        {

            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        user = new Hi.BLL.SYS_Users().GetModel(UserID);
        payM = new Hi.BLL.PAY_Payment().GetModel(hidPay);
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(ResellerID);
        if (payM == null)
        {
            return "{\"Result\":\"F\",\"Description\":\"参数传输有误！\"}";
        }
        decimal price = 0;//使用企业钱包金额
        prepayM = new Hi.BLL.PAY_PrePayment().GetModel(hidPrepay);
        if (prepayM != null)
        {
            price = prepayM.price;
        }
        else
        {
            return "{\"Result\":\"F\",\"Description\":\"参数传输有误！\"}";
        }
        int regid = 0;
        try
        {
            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
            regModel.OrderId = hidPrepay;
            regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidPrepay);
            regModel.number = payM.guid;
            regModel.Price = price;
            regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
            regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(ResellerID).DisName;
            regModel.DisID = ResellerID;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = prepayM.vdef1;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
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
            tx1376Request.setOrderNo(WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidPrepay));
            tx1376Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(Convert.ToInt32(hidPay)).guid);
            tx1376Request.setSmsValidationCode(phoneCode);

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
                Hi.Model.PAY_Payment payModel = new Hi.BLL.PAY_Payment().GetModel(hidPay);
                payM.PayDate = DateTime.Now;
                payModel.ts = DateTime.Now;
                payModel.verifystatus = tx1376Response.getVerifyStatus();
                payModel.status = tx1376Response.getStatus();
                new Hi.BLL.PAY_Payment().Update(payModel);
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
                        bool f = false;
                        try
                        {
                            Hi.Model.PAY_PrePayment prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(hidPrepay);
                            prepayModel.Start = 3;
                            f = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if (f)
                        {
                            return "{\"Result\":\"F\",\"Description\":\"支付失败！\"}";
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"支付失败！\"}";
                        }
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
                            return "{\"Result\":\"T\",\"Description\":\"支付成功！\"}";
                        }
                        else
                        {
                            return "{\"Result\":\"F\",\"Description\":\"支付失败！\"}";
                        }
                    }
                    else
                    {
                        return "{\"Result\":\"F\",\"Description\":\"支付失败！\"}";
                    }
                }
                else if (VerifyStatus == 20)
                {
                    return "{\"Result\":\"F\",\"Description\":\"验证码超时！\"}";
                }
                else if (VerifyStatus == 30)
                {
                    return "{\"Result\":\"F\",\"Description\":\"验证码输入有误！\"}";
                }
                else
                {
                    return "{\"Result\":\"F\",\"Description\":\"支付失败！\"}";
                }
            }
            else
            {
                return "{\"Result\":\"F\",\"Description\":\"" + tx1376Response.getMessage() + "！\"}";
            }
        }
        return "";
    }

    public string GetPrepay(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;

        string sortType = "";
        string criticalOrderID = "";
        string sort = "";
        string getType = "";
        string rows = "";


        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                criticalOrderID = Params["CriticalProductID"].ToString();//当前ID
                sort = Params["sort"].ToString();//0:降序 默认 1：升序
                getType = Params["getType"].ToString();//0:加载更多 1:加载老数据
                rows = Params["rows"].ToString();//加载条数
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业Id
            }

        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);
        // Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(UserID);
        string price = new Hi.BLL.PAY_PrePayment().sums(ResellerID, CompID).ToString("0.00");

        #region 模拟分页 曾宪政

        string tabName = " [dbo].[PAY_PrePayment]"; //表名
        string strSql = string.Empty; //搜索sql
        sortType = sortType == "1" ? "CreatDate" : "ID";
        string strWhere = string.Empty;
        strWhere = " and Start=1  and PreType in (1,2,3,4,5) and ISNULL(dr,0)=0 and DisID=" + ResellerID;
        strSql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
            sort, strWhere, getType, rows);
        if (strSql == "")
            return "{\"Result\":\"F\",\"Description\":\"基础数据异常！\"}";

        #endregion

        ArrayList arrayList = new ArrayList();
        try
        {
            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    if (dataColumn.ColumnName == "PreType")
                    {
                        dictionary.Add(dataColumn.ColumnName, Common.GetPrePayStartName(Convert.ToInt32(dataRow[dataColumn.ColumnName])));
                    }
                    else if (dataColumn.ColumnName == "CrateUser")
                    {
                        dictionary.Add(dataColumn.ColumnName, new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(dataRow[dataColumn.ColumnName])) == null ? "" : new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(dataRow[dataColumn.ColumnName])).TrueName);
                    }
                    else if (dataColumn.ColumnName == "ID" || dataColumn.ColumnName == "CreatDate" || dataColumn.ColumnName == "vdef1")
                    {
                        dictionary.Add(dataColumn.ColumnName,dataRow[dataColumn.ColumnName]+"");
                    }
                    else if (dataColumn.ColumnName == "price")
                    {
                        dictionary.Add(dataColumn.ColumnName, Convert.ToDecimal(dataRow[dataColumn.ColumnName]).ToString("0.00"));
                    }
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
        }
        catch (Exception ex)
        {
            return "{\"Result\":\"F\",\"Description\":\"" + ex.Message + "！\"}";
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string PrepayList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"查询成功！\",\"Price\":\"" + price + "\",\"PrepayList\":" + PrepayList + "}";
    }

    /// <summary>
    /// 查询银行卡信息
    /// </summary>
    /// <returns></returns>
    public string GetVisiableBankList(string Json)
    {

        JsonData Params = JsonMapper.ToObject(Json);
        Hi.Model.PAY_BankInfo BnakInfoM = null;
        string return_str = string.Empty;
        try
        {
            if (Params["BankCode"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                string BankCode = Params["BankCode"].ToString();
                string BankName = string.Empty;

                for (int i = 8; i >2; i--) 
                {
                    if (GetBandCode(BankCode.Substring(0, i)) > 0)
                    {
                        BnakInfoM = new Hi.BLL.PAY_BankInfo().GetModel(GetBandCode(BankCode.Substring(0, i)));
                        BankName = BnakInfoM.BankName;
                        BankCode = BnakInfoM.BankCode;
                        return_str = "{\"Result\":\"T\",\"Description\":\"查询成功！\",\"BankName\":\"" + BankName + "\",\"BankID\":\"" + BankCode + "\"}";
                        break;
                    }
                
                }
           
               return return_str;
            }

        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"操作失败！\"}";
        }
    }


    public int GetBandCode(string BankCodes)
    {
        int BankInfo_id =0;
       
        string BankName = string.Empty;
        string BankCode = string.Empty;
        try
        {
            string strSql = "select BankInfo_id from PAY_BankBIN where BIN = '" + BankCodes + "'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                BankInfo_id =Convert.ToInt32(ds.Tables[0].Rows[0]["BankInfo_id"]);              
            }
        }
        catch
        {
            BankInfo_id = 0;
        }

        return BankInfo_id;
    }

    /// <summary>
    /// 2.3.69	经销商获取待支付账单列表
    /// </summary>
    /// <param name="Json"></param>
    /// <returns></returns>
    public string GetBillPaymentList(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        string sortType = "";
        string criticalOrderID = "";
        string sort = "";
        string getType = "";
        string rows = "";

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                criticalOrderID = Params["criticalProductID"].ToString();//当前ID
                sort = Params["sort"].ToString();//0:降序 默认 1：升序
                getType = Params["getType"].ToString();//0:加载更多 1:加载老数据
                rows = Params["rows"].ToString();//加载条数
            }

        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);


        #region 模拟分页 曾宪政

        string tabName = " [dbo].[DIS_Order]"; //表名
        string strSql = string.Empty; //搜索sql
        sortType = sortType == "1" ? "CreatDate" : "ID";
        string strWhere = string.Empty;
        strWhere = " and Otype=9 and PayState in (0,1)  and OState<>6   and DisID='" + ResellerID + "' and ReturnState in(0,1) and isnull(dr,0)=0";
        strSql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
            sort, strWhere, getType, rows);
        if (strSql == "")
            return "{\"Result\":\"F\",\"Description\":\"基础数据异常！\"}";

        #endregion

        ArrayList arrayList = new ArrayList();
        try
        {
            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    if (dataColumn.ColumnName == "ID")
                    {
                        //订单编号
                        string ReceiptNo = Convert.ToString(Common.GetOrderValue(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString()), "ReceiptNo"));
                        dictionary.Add("orderID", Convert.ToString(dataRow[dataColumn.ColumnName]));
                        dictionary.Add("BillNumber", ReceiptNo);
                    }
                    else if (dataColumn.ColumnName == "CreateDate")
                    {
                        dictionary.Add("BillDate", Convert.ToDateTime(dataRow[dataColumn.ColumnName]).ToString("yyyy-MM-dd"));
                    }
                    else if (dataColumn.ColumnName == "AuditAmount")
                    {
                        dictionary.Add("BillMoney", Convert.ToDecimal(Convert.ToString(dataRow[dataColumn.ColumnName]) == "" ? "0" : Convert.ToString(dataRow[dataColumn.ColumnName])).ToString("0.00"));
                    }
                    else if (dataColumn.ColumnName == "PayedAmount")
                    {
                        dictionary.Add("PaidMoney", Convert.ToDecimal(Convert.ToString(dataRow[dataColumn.ColumnName]) == "" ? "0" : Convert.ToString(dataRow[dataColumn.ColumnName])).ToString("0.00"));
                    }
                    else if (dataColumn.ColumnName == "vdef2")
                    {
                        dictionary.Add("FeeName", Convert.ToString(dataRow[dataColumn.ColumnName]));
                    }


                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
        }
        catch (Exception ex)
        {
            return "{\"Result\":\"F\",\"Description\":\"" + ex.Message + "！\"}";
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string PrepayList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"查询成功！\",\"PrepayList\":" + PrepayList + "}";


    }

    /// <summary>
    /// 2.3.68	经销商订单支付明细查询
    /// </summary>
    /// <param name="Json"></param>
    /// <returns></returns>
    public string GetOrdersPaymentList(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        string sortType = "";
        string criticalOrderID = "";
        string sort = "";
        string getType = "";
        string rows = "";
        string orderType = string.Empty;

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                criticalOrderID = Params["criticalProductID"].ToString();//当前ID
                sort = Params["sort"].ToString();//0:降序 默认 1：升序
                getType = Params["getType"].ToString();//0:加载更多 1:加载老数据
                rows = Params["rows"].ToString();//加载条数
                orderType = Params["orderType"].ToString();//单据类型   1:订单 2:账单
            }

        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);

        // string price = new Hi.BLL.PAY_PrePayment().sums(user.DisID, user.CompID).ToString("0.00");

        #region 模拟分页 曾宪政

        string tabName = " [dbo].[CompCollection_view]"; //表名
        string strSql = string.Empty; //搜索sql
        sortType = "Date"; //sortType == "1" ? "Date" : "ID";
        string strWhere = string.Empty;
        if (orderType == "1")//订单支付明细
            strWhere = " and OrderID not in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and DisID=" + ResellerID + ") and DisID=" + ResellerID;
        else//账单支付明细
            strWhere = " and OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and DisID=" + ResellerID + ") and DisID=" + ResellerID;



        strSql = new Common().PageSqlString(criticalOrderID, "storID", tabName, sortType,
            sort, strWhere, getType, rows);
        if (strSql == "")
            return "{\"Result\":\"F\",\"Description\":\"基础数据异常！\"}";

        #endregion

        ArrayList arrayList = new ArrayList();
        try
        {
            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    // Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "ReceiptNo")

                    // Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "AuditAmount")

                    if (dataColumn.ColumnName == "orderID")
                    {
                        //订单编号
                        string ReceiptNo = Convert.ToString(Common.GetOrderValue(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString()), "ReceiptNo"));
                        //订单金额
                        decimal AuditAmount = Convert.ToDecimal(Common.GetOrderValue(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString()), "AuditAmount"));

                        dictionary.Add("orderID", Convert.ToString(dataRow[dataColumn.ColumnName]));
                        dictionary.Add("OrderNumber", ReceiptNo);
                        dictionary.Add("OrderMoney", AuditAmount);

                    }
                    else if (dataColumn.ColumnName == "Date")
                    {
                        dictionary.Add("PaymentDate", Convert.ToString(dataRow[dataColumn.ColumnName]));
                    }
                    else if (dataColumn.ColumnName == "Source")
                    {
                        dictionary.Add("PaymentMethod", Convert.ToString(dataRow[dataColumn.ColumnName]));
                    }
                    else if (dataColumn.ColumnName == "Price")
                    {
                        dictionary.Add("PaymentMoney", Convert.ToDecimal(Convert.ToString(dataRow[dataColumn.ColumnName]) == "" ? "0" : Convert.ToString(dataRow[dataColumn.ColumnName])).ToString("0.00"));
                    }
                    else if (dataColumn.ColumnName == "sxf")
                    {

                        dictionary.Add("FormalitiesCost", Convert.ToDecimal(Convert.ToString(dataRow[dataColumn.ColumnName]) == "" ? "0" : Convert.ToString(dataRow[dataColumn.ColumnName])).ToString("0.00"));
                    }
                    else if (dataColumn.ColumnName == "storID")
                    {

                        dictionary.Add("storID", Convert.ToString(dataRow[dataColumn.ColumnName]));
                    }


                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
        }
        catch (Exception ex)
        {
            return "{\"Result\":\"F\",\"Description\":\"" + ex.Message + "！\"}";
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string PrepayList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"查询成功！\",\"PrepayList\":" + PrepayList + "}";


    }


    public string GetTransfers(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;
        string sortType = "";
        string criticalOrderID = "";
        string sort = "";
        string getType = "";
        string rows = "";

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "")
            {
                return "{\"Result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID
                criticalOrderID = Params["criticalOrderID"].ToString();//当前ID
                sort = Params["sort"].ToString();//0:降序 默认 1：升序
                getType = Params["getType"].ToString();//0:加载更多 1:加载老数据
                rows = Params["rows"].ToString();//加载条数
            }

        }
        catch
        {
            return "{\"Result\":\"F\",\"Description\":\"参数有误！\"}";
        }
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);
        // Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(UserID);
        string price = new Hi.BLL.PAY_PrePayment().sums(ResellerID,CompID).ToString("0.00");

        #region 模拟分页

        string tabName = " [dbo].[PAY_PrePayment]"; //表名
        string strSql = string.Empty; //搜索sql
        sortType = sortType == "1" ? "CreatDate" : "ID";
        string strWhere = string.Empty;
        strWhere = " and Start=1  and PreType = 6 and ISNULL(dr,0)=0 and DisID=" + ResellerID;
        strSql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
            sort, strWhere, getType, rows);
        if (strSql == "")
            return "{\"Result\":\"F\",\"Description\":\"基础数据异常！\"}";

        #endregion

        ArrayList arrayList = new ArrayList();
        try
        {
            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    if (dataColumn.ColumnName == "CrateUser")
                    {
                        dictionary.Add(dataColumn.ColumnName, new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString())).TrueName);
                    }
                    else if (dataColumn.ColumnName == "ID" || dataColumn.ColumnName == "CreatDate")
                    {
                        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                    }
                    else if (dataColumn.ColumnName == "price")
                    {
                        dictionary.Add(dataColumn.ColumnName, Convert.ToDecimal(dataRow[dataColumn.ColumnName].ToString()).ToString("0.00"));
                    }
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
        }
        catch (Exception ex)
        {
            return "{\"Result\":\"F\",\"Description\":\"" + ex.Message + "！\"}";
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        string TransferList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"查询成功！\",\"TransferList\":" + TransferList + "}";
    }

    /// <summary>
    /// 获取支付宝、微信，支付交易流水号
    /// </summary>
    /// <param name="Json">传递json数据</param>
    /// <returns></returns>
    public string GetPayID(string Json)
    {
        Common.CatchInfo(Json, "GetPayID", "1");
        int UserID = 0;
        int ResellerID = 0;
        int CompID = 0;
        string ReceiptNo = "";
        decimal PrePrice = 0;
        string Prepassword = "";
        decimal PayPrice = 0;
        string PayType = "";
        JsonData Params = JsonMapper.ToObject(Json);

        try
        {
            if (Params["UserID"].ToString() == "" || Params["ResellerID"].ToString() == "" || Params["ReceiptNo"].ToString() == "" || Params["PrePrice"].ToString() == "" || Params["PayPrice"].ToString() == "" || Params["PayType"].ToString() == "")
            {
                return getPayResult("F", "参数错误！", "", "", "", "", "","");
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                ReceiptNo = Params["ReceiptNo"].ToString();//订单号
                PrePrice = Convert.ToDecimal(Params["PrePrice"].ToString());//企业钱包支付金额
                Prepassword = Params["Prepassword"].ToString();//企业钱包密码
                PayPrice = Convert.ToDecimal(Params["PayPrice"].ToString());//微信、支付宝支付金额
                PayType = Convert.ToString(Params["PayType"].ToString());//支付类型，微信、支付宝
                CompID = new Hi.BLL.BD_Distributor().GetModel(ResellerID).CompID;//核心企业ID
            }
        }
        catch
        {
            return getPayResult("F", "参数有误！", "", "", "", "", "","");
        }
        try
        {
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);//登录人对象
            string password = new Hi.BLL.BD_Distributor().GetModel(ResellerID).Paypwd;

            int payid = 0;
            int prepayid = 0;

            string strWhere = " 1=1 and ReceiptNo='" + ReceiptNo + "' and isnull(dr,0)=0";
            Hi.Model.DIS_Order orderM = new Hi.Model.DIS_Order();
            List<Hi.Model.DIS_Order> orderL = new Hi.BLL.DIS_Order().GetList("", strWhere, "");
            if (orderL.Count > 0)
            {
                orderM = orderL[0];
            }
            else
            {
                return getPayResult("F", "订单不存在！", "", "", "", "", "","");
            }

            Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(ResellerID);//经销商对象
            decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(ResellerID,CompID);//剩余企业钱包
            int disid = ResellerID;//经销商ID

            string payPas = Prepassword;

            if (PrePrice > 0)
            {
                if (password == Common.md5("123456"))
                {
                    return getPayResult("F", "请先修改支付密码！", "", "", "", "", "","");
                }
                else
                {
                    if (payPas == "")
                    {
                        return getPayResult("F", "密码不能为空！", "", "", "", "", "","");
                    }
                }
            }
            decimal price = PrePrice;

            decimal txtPayOrder = PrePrice + PayPrice;

            decimal payPrice = PayPrice;

            int orderid = orderM.ID;//订单id

            Hi.Model.DIS_Order orderModel = orderM;
            if (txtPayOrder > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
            {
                return getPayResult("F", "支付金额大于订单未支付金额，不能支付！", "", "", "", "", "","");
            }
            if (!((
                (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
                (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付))
                )
                && orderModel.OState != (int)Enums.OrderState.已作废))
            {
                return getPayResult("F", "订单异常，不能支付！", "", "", "", "", "","");
            }

            if (price > 0)
            {
                //企业钱包处理
                if (sumPrice < price)
                {
                    return getPayResult("F", "企业钱包余额不足！", "", "", "", "", "","");
                }
                if (disModel.Paypwd != Common.md5(payPas))
                {
                    return getPayResult("F", "密码不正确！", "", "", "", "", "","");
                }
                Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
                prepayModel.CompID = CompID;
                prepayModel.DisID = disModel.ID;
                prepayModel.OrderID = orderid;
                prepayModel.Start = 2;
                prepayModel.PreType = 5;
                prepayModel.price = price * -1;
                prepayModel.Paytime = DateTime.Now;
                prepayModel.CreatDate = DateTime.Now;
                prepayModel.CrateUser = UserID;
                prepayModel.AuditState = 2;
                prepayModel.IsEnabled = 1;
                prepayModel.ts = DateTime.Now;
                prepayModel.modifyuser = UserID;
                prepayModel.vdef1 = "订单支付";
                prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
                int prepay = 0;
                int order = 0;
                if (prepayid > 0 && payPrice == 0)
                {
                    SqlConnection con = new SqlConnection(LocalSqlServer);
                    con.Open();
                    SqlTransaction sqlTrans = con.BeginTransaction();
                    try
                    {
                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
                        order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price, sqlTrans);
                        Common.AddSysBusinessLog(orderModel, user, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + price.ToString("0.00") + "元", sqlTrans);
                        sqlTrans.Commit();
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
                        new MsgSend().GetWxService("2", orderModel.ID.ToString(), "1", price);
                        return getPayResult("F", "订单支付成功！", "", "", "", "", "","");
                    }
                    else
                    {
                        return getPayResult("F", "订单支付失败！", "", "", "", "", "","");
                    }
                }
            }

            if (payPrice <= 0)
            {
                return getPayResult("F", "参数异常！", "", "", "", "", "","");
            }
            int regid = 0;
            Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
            string strWhere2 = string.Empty;
            if (orderid != 0)
            {
                strWhere2 += " OrderID = " + orderid;
            }
            strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
            String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");

            string guid = Guid.NewGuid().ToString().Replace("-", "");
            payModel.OrderID = orderid;
            payModel.DisID = ResellerID;
            payModel.PayUser = disModel.DisName;
            payModel.PayPrice = payPrice;
            payModel.guid = Common.Number_repeat(guid);
            payModel.IsAudit = 2;
            payModel.vdef3 = "1";
            payModel.vdef4 = orderNo;
            payModel.CreateDate = DateTime.Now;
            payModel.CreateUserID = UserID;
            payModel.ts = DateTime.Now;
            payModel.modifyuser = UserID;
            payModel.Channel = "6";
            payid = new Hi.BLL.PAY_Payment().Add(payModel);

            if (prepayid > 0)
            {
                Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                prepayMent.vdef4 = payid.ToString();
                new Hi.BLL.PAY_PrePayment().Update(prepayMent);
            }

            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
            regModel.OrderId = Convert.ToInt32(orderid);
            regModel.Ordercode = orderNo + payid.ToString();
            regModel.number = payModel.guid;
            regModel.Price = payPrice;
            regModel.Payuse = "订单支付";
            regModel.PayName = disModel.DisName;
            regModel.DisID = disid;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = orderModel.Remark;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
            regModel.BankID = "";
            regModel.CreateUser = UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 0;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (payid <= 0 || regid <= 0)
            {
                return getPayResult("F", "数据保存失败！", "", "", "", "", "","");

            }
            if (PayType == "1")//微信
            {
                string PrepayID = "";
                string NonceStr = "";
                string Timestamp = "";
                string Sign = "";



                return getPayResult("T", "生成成功！", payModel.guid, PrepayID, NonceStr, Timestamp, Sign,regModel.Ordercode);
            }
            return getPayResult("T", "生成成功！", payModel.guid, "", "", "", "",regModel.Ordercode); //支付宝
        }
        catch (Exception ex)
        {
            return getPayResult("F", ex.Message, "", "", "", "", "","");
        }
    }

    public string getPayResult(string Result, string Description, string PayNumb, string PrepayID, string NonceStr, string Timestamp, string Sign,string ordercode)
    {
        return "{\"Result\":\"" + Result + "\",\"Description\":\"" + Description + "\",\"PayNumb\":\"" + PayNumb + "\",\"OrderCode\":\"" + ordercode + "\",\"PrepayID\":\"" + PrepayID + "\",\"NonceStr\":\"" + NonceStr + "\",\"Timestamp\":\"" + Timestamp + "\",\"Sign\":\"" + Sign + "\"}";
    }

    public string GetPayState(string Json)
    {
        int UserID = 0;
        int ResellerID = 0;
        string PayNumb = "";
        int PayState = 0;
        int Type = 0;

        try
        {
            JsonData Params = JsonMapper.ToObject(Json);
            if (Convert.ToString(Params["UserID"]) == "" || Convert.ToString(Params["ResellerID"]) == "" || Convert.ToString(Params["PayNumb"]) == "" || Convert.ToString(Params["PayState"]) == "" || Convert.ToString(Params["Type"]) == "")
            {
                return PayStateResult("F", "参数错误！", "");
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());//用户ID
                ResellerID = Convert.ToInt32(Params["ResellerID"].ToString());//经销商ID
                PayNumb = Params["PayNumb"].ToString();//支付订单编号
                PayState = Convert.ToInt32(Params["PayState"].ToString());//app端返回状态
                Type = Convert.ToInt32(Params["Type"].ToString());//类型，支付宝、微信
            }
            if (PayNumb == "")
            {
                return PayStateResult("F", "PayNumb为空字符串！", "");
            }
            Hi.Model.PAY_RegisterLog regM = new Hi.Model.PAY_RegisterLog();
            List<Hi.Model.PAY_RegisterLog> regL = new Hi.BLL.PAY_RegisterLog().GetList("", " Ordercode = '" + PayNumb + "'", "");
            if (regL == null || regL.Count <= 0)
            {
                return PayStateResult("F", "找不到" + PayNumb, "");
            }
            regM = regL[0];
            string strWhere = string.Empty;
            strWhere += " guid = '" + regM.number + "'";
            strWhere += " and isnull(dr,0)=0";
            List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList(" top 1 * ", strWhere, "");
            if (payL == null || payL.Count <= 0)
            {
                return PayStateResult("F", "未找到改交易流水号对应的信息！", "");
            }
            Hi.Model.PAY_Payment payM = payL[0];
            if (payM.IsAudit == 1)
            {
                return PayStateResult("T", "支付成功！", payM.IsAudit == 1 ? "1" : "0");
            }
            if (Type == 1)
            {

                return PayStateResult("", "", "");
            }
            else
            {
                //支付宝交易号与企业网站订单号不能同时为空
                string trade_no = regM.ResultMessage;//支付宝交易号
                string out_trade_no = regM.number;//支付流水号
                //string out_trade_no = regM.Ordercode;//企业订单号

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "single_trade_query");
                sParaTemp.Add("trade_no", trade_no);
                sParaTemp.Add("out_trade_no", out_trade_no);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);

                //请在这里加上企业的业务逻辑程序代码
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sHtmlText);
                //string strXmlResponse = xmlDoc.SelectSingleNode("/alipay").InnerText;
                string isSuccess = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (isSuccess == "F")
                    return PayStateResult("T", xmlDoc.SelectSingleNode("/alipay/error").InnerText, "0");
                string tradeStatus = xmlDoc.SelectSingleNode("/alipay/response/trade/trade_status").InnerText;
                if (tradeStatus == "TRADE_SUCCESS")
                    return PayStateResult("T", "支付成功！", "1");
                return PayStateResult("T", tradeStatus, "0");
            }
        }
        catch (Exception ex)
        {
            return PayStateResult("F", ex.Message, "");
        }
    }

    public string PayStateResult(string Result, string Description, string PayState)
    {
        return "{\"Result\":\"" + Result + "\",\"Description\":\"" + Description + "\",\"PayState\":\"" + PayState + "\"}";
    }

    ///<summary>
    ///线下支付
    ///</summary>
    ///<param name = "JSon">传入的json数据</param>
    ///<returns></returns>
    public ResultOffLinePay OffLinePay(string JSon)
    {
        string paymoney = string.Empty;
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        string orderid = string.Empty;
        string payname = string.Empty;
        string paybank = string.Empty;
        string paycode = string.Empty;
        string paydate = string.Empty;
        string remark = string.Empty;
        string strsql = string.Empty;
        #region//json取值
        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["OrderID"].ToString().Trim() == "" ||
            JInfo["ResellerID"].ToString().Trim() == "" || JInfo["PayMoney"].ToString().Trim() == "" || decimal.Parse(JInfo["PayMoney"].ToString()) == 0)
        {
            return new ResultOffLinePay() { Result = "F", Description = "参数异常" };
        }
        UserID = JInfo["UserID"].ToString();
        CompID = JInfo["CompID"].ToString();
        DisID = JInfo["ResellerID"].ToString();
        orderid = JInfo["OrderID"].ToString();
        paymoney = Common.NoHTML(JInfo["PayMoney"].ToString());
        payname = Common.NoHTML(JInfo["PayName"].ToString());
        paybank = Common.NoHTML(JInfo["PayBank"].ToString());
        paycode = Common.NoHTML(JInfo["PayCode"].ToString());
        paydate = JInfo["PayDate"].ToString();
        remark = Common.NoHTML(JInfo["Remark"].ToString());

        #endregion

        #region//判断登录信息是否异常
        try
        {
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new ResultOffLinePay() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultOffLinePay() { Result = "F", Description = "核心企业信息异常" };
        #endregion

            //判断订单是否可以支付
            Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));
            if (ordermodel.AuditAmount - ordermodel.PayedAmount < Convert.ToDecimal(paymoney))
                return new ResultOffLinePay { Result = "F", Description = "本次收款金额大于未收款金额" };
            if (paydate == "")
                return new ResultOffLinePay { Result = "F", Description = "请选择支付日期" };
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            Hi.Model.PAY_Payment paymentmodel = new Hi.Model.PAY_Payment();

            paymentmodel.OrderID = Convert.ToInt32(orderid);
            paymentmodel.DisID = Convert.ToInt32(DisID);
            paymentmodel.PayPrice = Convert.ToDecimal(paymoney);
            paymentmodel.payName = payname;
            paymentmodel.paycode = paycode;
            paymentmodel.paybank = paybank;
            paymentmodel.guid = Common.Number_repeat(guid);
            if (paydate != "")
                paymentmodel.PayDate = Convert.ToDateTime(paydate);
            paymentmodel.Remark = remark;
            paymentmodel.PrintNum = 1;//下线支付无需结算，所以结算状态是1
            paymentmodel.vdef3 = "1";//（1，订单支付，2，预付款充值、汇款）
            paymentmodel.IsAudit = 2;//1，成功 ，2失败 
            paymentmodel.Channel = "5";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付 
            paymentmodel.CreateDate = DateTime.Now;
            paymentmodel.vdef5 = "0.00";//手续费

            paymentmodel.CreateUserID = Int32.Parse(UserID);


            paymentmodel.attach = "";//附件  

            //在数据库中插入一条失效的付款记录
            int num = new Hi.BLL.PAY_Payment().Add(paymentmodel);
            //记录插入成功的话修改订单状态与支付状态
            if (num > 0)
            {
                int order = 0;
                int pay = 0;
                SqlConnection con = new SqlConnection(LocalSqlServer);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                try
                {

                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, Convert.ToInt32(orderid), Convert.ToDecimal(paymoney), sqlTrans);//修改订单状态
                    pay = new Hi.BLL.PAY_Payment().updatePayState(con, num, sqlTrans);//修改支付表状态

                    if (order > 0 && pay > 0)
                        sqlTrans.Commit();
                    else
                    {
                        sqlTrans.Rollback();
                        return new ResultOffLinePay() { Result = "F", Description = "支付失败" };
                    }
                }
                catch
                {
                    sqlTrans.Rollback();
                    return new ResultOffLinePay() { Result = "F", Description = "更新异常" };
                }
                finally
                {
                    con.Close();
                    sqlTrans.Dispose();
                }
            }
            else
                return new ResultOffLinePay() { Result ="F",Description ="参数异常"};
            #region//返回信息
            List<class_ver3.Pay> list_pay = new List<class_ver3.Pay>();
            //将订单的支付明细取出放在DT里
            strsql = "select comp.CompName as 核心企业名称,pay.DisName as 经销商名称,pay.Source as 类型,pay.Price as 支付金额,pay.Date as 支付日期,pay.sxf as 手续费,pay_payment.guid as 支付流水号 ";
            strsql += " from CompCollection_view pay inner join BD_Company  comp on pay.CompID = comp.ID left join PAY_Payment pay_payment on pay.paymentID = ";
            strsql += " pay_payment.ID ";
            strsql += " where pay.OrderID = " + orderid + " and isnull(pay.vedf9,0) = 1 order by pay.storID desc";
            DataTable dt_pay = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            decimal nummoney = 0;
            //取出每条日志，放入日志list中
            for (int i = 0; i < dt_pay.Rows.Count; i++)
            {
                class_ver3.Pay pay = new class_ver3.Pay();
                pay.CompName = ClsSystem.gnvl(dt_pay.Rows[i]["核心企业名称"], "");
                pay.ResellerName = ClsSystem.gnvl(dt_pay.Rows[i]["经销商名称"], "");
                pay.PayLogType = ClsSystem.gnvl(dt_pay.Rows[i]["类型"], "");
                pay.PayDate = ClsSystem.gnvl(dt_pay.Rows[i]["支付日期"], "");
                pay.PayAmount = ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0");
                pay.FeeAmount = ClsSystem.gnvl(dt_pay.Rows[i]["手续费"], "0");
                pay.Guid = ClsSystem.gnvl(dt_pay.Rows[i]["支付流水号"], "");
                nummoney += decimal.Parse(ClsSystem.gnvl(dt_pay.Rows[i]["支付金额"], "0"));
                list_pay.Add(pay);
            }

            return new ResultOffLinePay() { Result = "T",Description = "返回成功",PayLogList = list_pay,NumMoney =nummoney.ToString("0.00")};
            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "OffLinePay:" + JSon);
            return new ResultOffLinePay() { Result = "F", Description = "参数异常" };
        }

    }


    //线下支付的返回参数
    public class ResultOffLinePay
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.Pay> PayLogList { get; set; }
        public String NumMoney { get; set; }
    }
}