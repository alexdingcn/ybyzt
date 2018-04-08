<%@ WebHandler Language="C#" Class="Tx1311" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Web.SessionState;
public class Tx1311 : IHttpHandler,IRequiresSessionState
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        try
        {
            // 1.取得参数
            int isDBPay = Convert.ToInt32(request["isDBPay"]);
            string username = Convert.ToString(request.Form["hidUserName"]);
            string bankid = Convert.ToString(request.Form["hidBank"]);
            string yfk = Convert.ToString(request.Form["hida1"]);//是否用企业钱包 0:否，1:是
            string payPas = "";//企业钱包密码
            string kjzf = Convert.ToString(request.Form["hida2"]);//是否用快捷支付 0:否，1:是
            string wyzf = Convert.ToString(request.Form["hida3"]);//是否用网银 0:否，1:是
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

           // Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;


            if (logUser == null)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);
            if (disModel == null)
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
            int disid = logUser.DisID;//代理商ID
            string password = disModel.Paypwd;

            if (yfk == "1")
            {
                if (request["padPaypas"] == null || request["padPaypas"].Trim().ToString() == "")
                {
                    string Josn = ErrMessage("企业钱包密码不能为空");
                    context.Response.Write(Josn);
                    return;
                }
                payPas = request["padPaypas"].Trim().ToString();
                if (password == Util.md5("123456"))
                {
                    ;
                    //string Josn = "{\"error\":\"1\",\"msg\":\"请先修改企业钱包支付密码！\"}";
                    string Josn = ErrMessage("请先修改企业钱包支付密码");
                    context.Response.Write(Josn);
                    return;
                }
                else
                {
                    if (payPas == "")
                    {
                        //string Josn = "{\"error\":\"1\",\"msg\":\"密码不能为空！\"}";
                        string Josn = ErrMessage("密码不能为空");
                        context.Response.Write(Josn);
                        return;
                    }
                }
            }

            decimal payPrice = 0;
            decimal txtPayOrder = 0;
            if (request.Form["txtPayOrder"] == null || request.Form["txtPayOrder"].Trim().ToString() == "")
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }
            txtPayOrder = Convert.ToDecimal(request.Form["txtPayOrder"]);
            if (request.Form["txtPrice"] != null && request.Form["txtPrice"].Trim().ToString() != "")
            {
                price = Convert.ToDecimal(request.Form["txtPrice"]);
            }
            if (yfk == "1" && txtPayOrder < price)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"使用企业钱包大于支付金额！\"}";
                context.Response.Write(Josn);
                return;
            }
            if (yfk == "1")
                payPrice = txtPayOrder - price;
            else
                payPrice = txtPayOrder;
            int orderid = KeyID;//订单id
            if (orderid <= 0)
            {
                //string Josn = "{\"error\":\"1\",\"msg\":\"操作有误！\"}";
                string Josn = ErrMessage("数据有误");
                context.Response.Write(Josn);
                return;
            }
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
            if (orderModel == null)
            {
                string Josn = ErrMessage("数据有误");
                context.Response.Write(Josn);
                return;
            }
            if (txtPayOrder > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"支付金额大于未支付金额，不能支付！\"}";
                context.Response.Write(Josn);
                return;
            }
            if (!((
                (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
                (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
                (orderModel.Otype == (int)Enums.OType.推送账单 && orderModel.OState == (int)Enums.OrderState.已审 && (orderModel.PayState == (int)Enums.PayState.部分支付 || orderModel.PayState == (int)Enums.PayState.未支付))
                )
                && orderModel.OState != (int)Enums.OrderState.已作废))
            {
                string Josn = string.Empty;
                if (orderModel.Otype == (int)Enums.OType.推送账单)
                    Josn = ErrMessage("账单异常，不能支付");
                else
                    Josn = ErrMessage("订单异常，不能支付");
                context.Response.Write(Josn);
                return;
            }

            int prepayid = 0;

            if (yfk == "1" && price > 0)
            {
                //企业钱包处理
                if (sumPrice < price)
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"企业钱包余额不足！\"}";
                    string Josn = ErrMessage("企业钱包余额不足");
                    context.Response.Write(Josn);
                    return;
                }
                if (disModel.Paypwd != Util.md5(payPas))
                {
                    //string Josn = "{\"error\":\"1\",\"msg\":\"支付密码不正确！\"}";
                    string Josn = ErrMessage("支付密码不正确");
                    context.Response.Write(Josn);
                    return;
                }
                Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
                prepayModel.CompID = disModel.CompID;
                prepayModel.DisID = disModel.ID;
                prepayModel.OrderID = orderid;
                prepayModel.Start = 2;
                prepayModel.PreType = 5;
                prepayModel.price = price * -1;
                prepayModel.Paytime = DateTime.Now;
                prepayModel.CreatDate = DateTime.Now;
                prepayModel.CrateUser = logUser.UserID;
                prepayModel.AuditState = 2;
                prepayModel.IsEnabled = 1;
                prepayModel.ts = DateTime.Now;
                prepayModel.modifyuser = logUser.UserID;
                // prepayModel.vdef1 = "订单支付";
                prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
                int prepay = 0;
                int order = 0;
                if (prepayid > 0 && payPrice == 0)//payPrice（快捷支付金额）= 0 ，只用企业钱包支付，修改状态
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
                        string Josn = string.Empty;
                        if (orderModel.Otype == (int)Enums.OType.推送账单)
                            Josn = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "\"}";
                        else
                            Josn = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "\"}";
                        context.Response.Write(Josn);
                        try
                        {
                            if (orderModel.Otype == (int)Enums.OType.推送账单)
                                Utils.AddSysBusinessLog(disModel.CompID, "Order", orderid.ToString(), "账单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", logUser.UserID.ToString());
                            else
                                Utils.AddSysBusinessLog(disModel.CompID, "Order", orderid.ToString(), "订单支付", "支付：" + price + "元(企业钱包支付)", logUser.UserID.ToString());
                            if (orderModel.Otype != 9)
                            {
                                OrderInfoType.AddIntegral(logUser.CompID, logUser.DisID, "1", 1, orderid, price, "订单支付", "", logUser.UserID);
                            }
                            new Common().GetWxService("2", orderid.ToString(), "1");


                        }
                        catch (Exception ex) { }

                        return;
                    }
                    else
                    {
                        //string Josn = "{\"error\":\"1\",\"msg\":\"支付失败！\"}";
                        string Josn = ErrMessage("支付失败");
                        context.Response.Write(Josn);
                        return;
                    }
                }

            }

            if (payPrice > 0)
            {
                int payid = 0;
                int regid = 0;
                Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
                //string strWhere2 = string.Empty;
                //strWhere2 += " OrderID = " + orderid;
                //strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
                //List<Hi.Model.PAY_Payment> payList = new Hi.BLL.PAY_Payment().GetList("", strWhere2, "");
                string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

                string guid = Guid.NewGuid().ToString().Replace("-", "");
                payModel.OrderID = orderid;
                payModel.DisID = logUser.DisID;
                payModel.Type = isDBPay;
                payModel.PayUser = disModel.DisName;
                payModel.PayPrice = payPrice;
                payModel.IsAudit = 2;
                payModel.guid = Common.Number_repeat(guid);
                payModel.vdef3 = "1";
                payModel.vdef4 = orderNo;
                payModel.CreateDate = DateTime.Now;
                payModel.CreateUserID = logUser.UserID;
                payModel.ts = DateTime.Now;
                payModel.modifyuser = logUser.UserID;
                payid = new Hi.BLL.PAY_Payment().Add(payModel);
                if (prepayid > 0)
                {
                    Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                    prepayMent.vdef4 = payid.ToString();//与企业钱包关联
                    new Hi.BLL.PAY_PrePayment().Update(prepayMent);
                }

                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                regModel.OrderId = orderid;
                regModel.Ordercode = orderNo + payid.ToString();
                regModel.number = payModel.guid;
                regModel.Price = payPrice;
                regModel.Payuse = "订单支付";
                regModel.PayName = disModel.DisName;
                regModel.DisID = disid;
                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                regModel.Remark = orderModel.Remark;
                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
                regModel.BankID = bankid;
                regModel.CreateUser = logUser.UserID;
                regModel.CreateDate = DateTime.Now;
                regModel.LogType = 1311;
                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
                if (payid <= 0 || regid <= 0)
                {
                    string Josn = ErrMessage("数据有误");
                    context.Response.Write(Josn);
                    return;
                }
                string AccountType = request["AccountType"];
                //ClientScript.RegisterStartupScript(this.GetType(), "onSubmit3", "<script>onSubmit3()</script>");
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
                    return;
                }

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构代码

                long amount = Convert.ToInt64(payPrice * 100);//支付金额（单位：分）
                long fee = 0;
                //String payerID = disModel.ID.ToString();
                //String payerName = disModel.DisName;
                String usage = "支付订单";
                String remark = "订单支付";

                String notificationURL = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port + "/Handler/ReceiveNoticePage.ashx";//回调页面地址
                String payees = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;

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
                tx1311Request.setOrderNo(orderNo + payid.ToString());//订单号
                tx1311Request.setPaymentNo(payModel.guid);//支付交易流水号
                tx1311Request.setAmount(amount);//支付金额 单位分
                tx1311Request.setFee(fee);//支付服务手续费 单位分
                tx1311Request.setPayerID("");//付款人注册ID
                tx1311Request.setPayerName("");//付款方名称
                tx1311Request.setUsage(usage);//资金用途
                tx1311Request.setRemark(remark);//备注
                tx1311Request.setNotificationURL(notificationURL);//机构接收支付通知的URL
                tx1311Request.addPayee("");//收款方名称
                tx1311Request.setBankID(bankID);//付款银行标识
                tx1311Request.setAccountType(accountType);//付款方帐号类型r
                // 3.执行报文处理
                tx1311Request.process();
                try
                {
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = tx1311Request.getRequestPlainText();
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                }
                catch (Exception ex) { }
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

                // 5.转向Request.jsp页面;
                context.Session.Add("message", tx1311Request.getRequestMessage());
                context.Session.Add("signature", tx1311Request.getRequestSignature());
                string Josn1 = "{\"success\":\"1\"}";
                context.Response.Write(Josn1);
                return;
            }
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    public string ErrMessage(string msg)
    {
        return "{\"error\":\"1\",\"msg\":\"" + msg + "！\"}";
    }

}