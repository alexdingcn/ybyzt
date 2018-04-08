using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Data.SqlClient;
using DBUtility;
using LitJson;

public partial class Distributor_Pay_Request : DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public bool err = false;
    public int KeyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        int orderid = 0;

        if (!IsPostBack)
        {
            try
            {
                // 1.取得参数
                if (Request.Form["hidOid"] == "")
                {
                    orderid = 0;
                }
                else
                {
                    orderid = Common.DesDecrypt(Request.Form["hidOid"], Common.EncryptKey).ToInt(0);
                    KeyID = orderid;
                }
                if (!Common.PageDisOperable("Order", orderid, this.DisID))
                {
                    Response.Redirect("../../NoOperable.aspx", true);
                    return;
                }

                decimal txtPayOrder = Convert.ToDecimal(Request.Form["hidPayOrder"]);//本次支付总金额
                decimal price = Convert.ToDecimal(Request.Form["hidPrice"]);//使用企业钱包金额
                int yfk = Convert.ToInt32(Request.Form["hidIsPre"]);//是否使用企业钱包  1:是  0:否
                int isDBPay = Convert.ToInt32(Request.Form["hidIsDBPay"]);//是否是担保支付  1:是  0:否
                string bankid = Convert.ToString(Request.Form["hidBankNo"]);//银行编号
                string AccountType = Request.Form["hidAccountType"];//账户类型
                string payPas = Convert.ToString(Request.Form["hidPayPas"]);//预付款密码

                Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(this.DisID);

                Hi.Model.DIS_Order disOrderMOdel = new Hi.BLL.DIS_Order().GetModel(orderid);

                if (disModel == null)
                {
                    err = true;
                    ErrMessage("数据异常");
                    return;
                }

                decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disOrderMOdel.CompID);//剩余企业钱包
                int disid = this.DisID;//代理商ID
                string password = disModel.Paypwd;

                if (yfk == 1)
                {
                    if (payPas == null || payPas.Trim().ToString() == "")
                    {
                        err = true;
                        ErrMessage("企业钱包密码不能为空");
                        return;
                    }
                    payPas = payPas.Trim().ToString();
                    if (password == Util.md5("123456"))
                    {
                        err = true;
                        ErrMessage("请先修改企业钱包支付密码");
                        return;
                    }
                    else
                    {
                        if (payPas == "")
                        {
                            //string Josn = "{\"error\":\"1\",\"msg\":\"密码不能为空！\"}";
                            err = true;
                            ErrMessage("密码不能为空");

                            return;
                        }
                    }
                }

                decimal payPrice = 0;
                if (txtPayOrder == 0)
                {
                    err = true;
                    ErrMessage("支付金额不能为0");

                    return;
                }
                if (yfk == 1 && txtPayOrder < price)
                {
                    err = true;
                    ErrMessage("使用企业钱包大于支付金额!");
                    return;
                }
                if (yfk == 1)
                    payPrice = txtPayOrder - price;
                else
                    payPrice = txtPayOrder;
                if (orderid <= 0)
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"操作有误！\"}";
                    err = true;
                    ErrMessage("数据有误");

                    return;
                }
                Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
                if (orderModel == null)
                {
                    err = true;
                    ErrMessage("数据有误");

                    return;
                }
                if (txtPayOrder > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
                {
                    err = true;
                    ErrMessage("支付金额大于未支付金额，不能支付！");
                    return;
                }
                if (!((
                    (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
                    (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
                    (orderModel.Otype == (int)Enums.OType.推送账单 && orderModel.OState == (int)Enums.OrderState.已审 && (orderModel.PayState == (int)Enums.PayState.部分支付 || orderModel.PayState == (int)Enums.PayState.未支付))
                    )
                    && orderModel.OState != (int)Enums.OrderState.已作废))
                {
                    if (orderModel.Otype == (int)Enums.OType.推送账单)
                    {
                        err = true;
                        ErrMessage("账单异常，不能支付");
                    }
                    else
                    {
                        err = true;
                        ErrMessage("订单异常，不能支付");
                    }
                    return;
                }

                int prepayid = 0;

                if (yfk == 1 && price > 0)
                {
                    //企业钱包处理
                    if (sumPrice < price)
                    {
                        err = true;
                        ErrMessage("企业钱包余额不足");

                        return;
                    }
                    if (disModel.Paypwd != Util.md5(payPas))
                    {
                        err = true;
                        ErrMessage("支付密码不正确");

                        return;
                    }
                    Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
                    prepayModel.CompID = disOrderMOdel.CompID;
                    prepayModel.DisID = disModel.ID;
                    prepayModel.OrderID = orderid;
                    prepayModel.Start = 2;
                    prepayModel.PreType = 5;
                    prepayModel.price = price * -1;
                    prepayModel.Paytime = DateTime.Now;
                    prepayModel.CreatDate = DateTime.Now;
                    prepayModel.CrateUser = this.UserID;
                    prepayModel.AuditState = 2;
                    prepayModel.IsEnabled = 1;
                    prepayModel.ts = DateTime.Now;
                    prepayModel.modifyuser = this.UserID;
                    prepayModel.guid = Common.Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));
                    // prepayModel.vdef1 = "订单支付";
                    prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
                    int prepay = 0;
                    int order = 0;
                    if (prepayid > 0 && payPrice == 0)//payPrice（网银支付金额）= 0 ，只用企业钱包支付，修改状态
                    {

                        SqlConnection con = new SqlConnection(LocalSqlServer);
                        con.Open();
                        SqlTransaction sqlTrans = con.BeginTransaction();
                        try
                        {
                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);//修改企业钱包状态
                            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price, sqlTrans);//修改订单状态
                            if (prepay > 0 && order > 0)
                                sqlTrans.Commit();
                            else
                                sqlTrans.Rollback();
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
                            try
                            {
                                if (orderModel.Otype == (int)Enums.OType.推送账单)
                                    Utils.AddSysBusinessLog(disOrderMOdel.CompID, "Order", orderid.ToString(), "账单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", this.UserID.ToString());
                                else
                                    Utils.AddSysBusinessLog(disOrderMOdel.CompID, "Order", orderid.ToString(), "订单支付", "支付：" + price + "元(企业钱包支付)", this.UserID.ToString());
                                if (orderModel.Otype != 9)
                                {
                                    OrderInfoType.AddIntegral(disOrderMOdel.CompID, disOrderMOdel.DisID, "1", 1, orderid, price, "订单支付", "", this.UserID);
                                }
                                new Common().GetWxService("2", orderid.ToString(), "1", price);


                            }
                            catch (Exception ex) { throw ex; }

                            if (orderModel.Otype == (int)Enums.OType.推送账单)
                            {
                                err = true;//阻止进入网银支付
                                Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
                            }
                            else
                            {
                                err = true;//阻止进入网银支付
                                Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
                            }
                            return;
                        }
                        else
                        {
                            err = true;
                            ErrMessage("支付失败");
                            return;
                        }
                    }

                }

                if (payPrice <= 0)
                {
                    err = true;
                    ErrMessage("数据有误");
                    return;
                }


                #region     计算支付手续费 start
                string sxfsq = "-1";
                decimal sxf = 0;
                decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。
                // 获取手续费 begin  
                string[] Json = Common.GetSxf(disOrderMOdel.CompID, AccountType, bankid, payPrice);

                string strMsg = Json[2].ToString();
                if (!string.IsNullOrEmpty(strMsg))
                {
                    ErrMessage(strMsg);
                    return;
                }
                else 
                {
                    sxfsq = Json[1].ToString();
                    sxf = Convert.ToDecimal(Json[0]);
                    comp_sxf = Convert.ToDecimal(Json[3]);
                }
                // 获取手续费 end
  

                //支付总金额（含手续费）
                decimal UNIT = 0.01M;
                payPrice = payPrice + Common.Round(sxf, UNIT);

                #endregion  计算支付手续费 end


                int payid = 0;
                int regid = 0;
                Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
                string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

                string guid = Guid.NewGuid().ToString().Replace("-", "");
                payModel.OrderID = orderid;
                payModel.DisID = this.DisID;
                payModel.Type = isDBPay;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = payPrice;
                payModel.IsAudit = 2;
                payModel.guid = Common.Number_repeat(guid);
                payModel.vdef3 = "1";
                payModel.vdef4 = orderNo;
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = this.UserID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = this.UserID;
                //判断账户类型，判断支付渠道，然后赋值
                if (AccountType == "11")
                {
                    if (bankid == "888")
                        payModel.Channel = "2";
                    else
                        payModel.Channel = "3";
                }//信用卡支付
                else if (AccountType == "13")
                    payModel.Channel = "8";
                else//网银支付
                    payModel.Channel = "4";

                payModel.State = Convert.ToInt32(sxfsq);//手续费收取方

                if (sxfsq.Equals("2"))
                    payModel.vdef5 = comp_sxf.ToString("0.00");
                else
                    payModel.vdef5 = sxf.ToString("0.00");//支付手续费

                payid = new Hi.BLL.PAY_Payment().Add(payModel);
                if (prepayid > 0)
                {
                    Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                    prepayMent.vdef4 = payid.ToString();//与企业钱包关联
                    new Hi.BLL.PAY_PrePayment().Update(prepayMent);
                }

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = orderid;
                regModel.Ordercode = orderNo;// orderNo + payid.ToString();
                regModel.number = payModel.guid;
                regModel.Price = payPrice;
                regModel.Payuse = "订单支付";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disid;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = orderModel.Remark;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disOrderMOdel.CompID).CompName;
                regModel.BankID = bankid;
                regModel.CreateUser = this.UserID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1311;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
                if (payid <= 0 || regid <= 0)
                {
                    err = true;
                    ErrMessage("数据有误");
                    return;
                }               
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                try
                {
                    PaymentEnvironment.Initialize(configPath);
                }
                catch (Exception ex)
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"支付配置有误，请联系系统管理员！\"}";
                    err = true;
                    ErrMessage("支付配置有误，请联系系统管理员");

                    return;
                }

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构代码

                long amount = Convert.ToInt64(payPrice * 100);//支付金额（单位：分）
                long fee = 0;
                String usage = "支付订单";
                String remark = "订单支付";
                String notificationURL = "";
                //"http://www.my1818.com/Handler/ReceiveNoticePage.ashx";//Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port +

                if (WebConfigurationManager.AppSettings["PayType"] == "0")
                    notificationURL = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/Handler/ReceiveNoticePage.ashx";
                   // notificationURL = "https://www.yibanmed.com/Handler/ReceiveNoticePage.ashx";
                else 
                  notificationURL = "https://www.yibanmed.com/Handler/ReceiveNoticePage.ashx";//回调页面地址

                String payees = new Hi.BLL.BD_Company().GetModel(disOrderMOdel.CompID).CompName;

                //判断支付接口
                string Pay_type = WebConfigurationManager.AppSettings["PayType"];
                String bankID = string.Empty;
                if (Pay_type == "0")//测试接口
                    bankID = "700";//bankid;//
                else
                    bankID = bankid;//正式接口

                int accountType = Convert.ToInt32(AccountType);

                // 2.创建交易请求对象
                Tx1311Request tx1311Request = new Tx1311Request();
                tx1311Request.setInstitutionID(institutionID);//机构号码
                tx1311Request.setOrderNo(orderNo);//订单号orderNo + payid.ToString()
                tx1311Request.setPaymentNo(payModel.guid);//支付交易流水号
                tx1311Request.setAmount(amount);//支付金额 单位分
                tx1311Request.setFee(fee);//支付服务手续费 单位分
                tx1311Request.setPayerID("");//付款人注册ID
                tx1311Request.setPayerName("");//付款方名称
                tx1311Request.setUsage(usage);//资金用途
                tx1311Request.setRemark(remark);//备注
                tx1311Request.setNotificationURL(notificationURL);//机构接收支付通知的URL
                tx1311Request.addPayee("");//收款方名称

                if (accountType == 13)
                {
                    tx1311Request.setAccountType(11);//付款方帐号类型
                    tx1311Request.setCardType("02");//银行卡类型（01=借记卡、02=贷记卡）
                }
                else if (accountType == 11)
                {
                    tx1311Request.setAccountType(accountType);//付款方帐号类型
                    tx1311Request.setCardType("01");//银行卡类型（01=借记卡、02=贷记卡）
                }
                else if (accountType == 12)
                    tx1311Request.setAccountType(accountType);//付款方帐号类型

                tx1311Request.setBankID(bankID);//付款银行标识


                // 3.执行报文处理
                tx1311Request.process();
                try
                {
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = tx1311Request.getRequestPlainText();
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                }
                catch (Exception ex) { throw ex; }

                //跳转的url地址
                HttpContext.Current.Items["action"] = PaymentEnvironment.PaymentURL;
                //中金返回信息
                HttpContext.Current.Items["message"] = tx1311Request.getRequestMessage();
                //中金签名信息
                HttpContext.Current.Items["signature"] = tx1311Request.getRequestSignature();
            }
            catch (Exception ex)
            {
                err = true;
                ErrMessage(ex.Message);
                return;
            }
            finally
            {
                if (!err)
                {
                    Context.Server.Transfer("DoSubmit.aspx", false);
                }
            }
        }
    }
    /// <summary>
    /// 错误跳转页面
    /// </summary>
    /// <param name="msg">错误信息</param>
    public void ErrMessage(string msg)
    {
        Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt(msg, Common.EncryptKey), false);
    }


}