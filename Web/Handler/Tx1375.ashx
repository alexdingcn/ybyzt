<%@ WebHandler Language="C#" Class="Tx1375" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

using System.Web.SessionState;
public class Tx1375 : IHttpHandler, IRequiresSessionState
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();

        try
        {
            int isDBPay = Convert.ToInt32(request["isDBPay"]);
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                string Josn = ErrMessage("请先登录");
                context.Response.Write(Josn);
                return;
            }
            string password = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID).Paypwd;//企业钱包密码

            int KeyID = 0;
            int payid = 0;
            int prepayid = 0;
            string phoneHtml = "";
            if (request["KeyID"] == "")
            {
                KeyID = 0;
            }
            else
            {
                KeyID = Common.DesDecrypt(request["KeyID"], Common.EncryptKey).ToInt(0);
            }
            string yfk = "";//是否使用企业钱包 1:是
            if (request["hida1"] != null)
            {
                yfk = request["hida1"].Trim().ToString();
            }

            Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);//代理商对象
            decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
            int disid = logUser.DisID;//代理商ID

            string payPas = "";
            payPas = Convert.ToString(request["padPaypas"].Trim().ToString());//企业钱包密码
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
                    //string Josn = "{\"error\":\"1\",\"msg\":\"请先修改支付密码！\"}";
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
            decimal price = 0;
            if (yfk == "1" && request["txtPrice"] != null && request["txtPrice"].Trim().ToString() != "")
            {
                price = Convert.ToDecimal(request["txtPrice"].Trim().ToString());//使用企业钱包金额
            }
            decimal txtPayOrder = 0;//支付订单总金额
            if (request["txtPayOrder"] == null || request["txtPayOrder"] == "")
            {
                string Josn = ErrMessage("数据有误");
                context.Response.Write(Josn);
                return;
            }
            txtPayOrder = Convert.ToDecimal(request["txtPayOrder"].Trim().ToString());
            if (yfk == "1" && txtPayOrder < price)
            {
                //string Josn = "{\"error\":\"1\",\"msg\":\"使用企业钱包大于支付金额！\"}";
                string Josn = ErrMessage("使用企业钱包大于支付金额");
                context.Response.Write(Josn);
                return;
            }
            decimal payPrice = 0;//使用快捷支付金额
            if (txtPayOrder == 0)
            {
                string Josn = ErrMessage("数据异常");
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
                //string Josn = "{\"error\":\"1\",\"msg\":\"支付金额大于未支付金额，不能支付！\"}";
                string Josn = ErrMessage("支付金额大于未支付金额，不能支付");
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
            //string strWhere1 = " 1=1 ";
            //if (orderid != 0)
            //{
            //    strWhere1 += " OrderID = '" + orderid + "'";
            //}
            //strWhere1 += " and IsEnabled=1 and Start=1 and isnull(dr,0)=0";

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
                prepayModel.guid =Common.Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));
                //prepayModel.vdef1 = "";
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
                    if (prepay <= 0 || order <= 0)
                    {
                        string Josn = ErrMessage("支付失败");
                        context.Response.Write(Josn);
                        return;
                    }
                    string Josn1 = string.Empty;
                    if (orderModel.Otype == (int)Enums.OType.推送账单)
                        Josn1 = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=N" + "\"}";

                    else
                        Josn1 = "{\"success\":\"2\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=N" + "\"}";
                    context.Response.Write(Josn1);
                    try
                    {
                        if (orderModel.Otype == (int)Enums.OType.推送账单)
                            Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "账单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", logUser.UserID.ToString());
                        else
                            Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", logUser.UserID.ToString());
                        if (orderModel.Otype != 9)
                        {
                            OrderInfoType.AddIntegral(logUser.CompID, logUser.DisID, "1", 1, orderid, price, "订单支付", "", logUser.UserID);
                        }
                        new Common().GetWxService("2", orderid.ToString(), "1",price);
                    }
                    catch (Exception ex) { }

                    return;
                }
            }

            if (payPrice <= 0)
            {
                string Josn = ErrMessage("数据异常！");
                context.Response.Write(Josn);
                return;
            }
            int hidFastPay = 0;
            if (request["hidFastPay"] == "")
            {
                string Josn = ErrMessage("数据异常");
                context.Response.Write(Josn);
                return;
            }

            hidFastPay = Convert.ToInt32(request["hidFastPay"]);
            Hi.Model.PAY_FastPayMent fastM = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay);
            if (fastM == null)
            {
                string Josn = ErrMessage("数据异常，该快捷支付记录不存在！");
                context.Response.Write(Josn);

                return;
            }
            if (fastM.DisID != logUser.DisID)
            {
                string Josn = ErrMessage("数据异常，该快捷支付银行卡不属于本代理商！");
                context.Response.Write(Josn);
                return;
            }
            string phone = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).phone;
            phoneHtml = "（已发送至" + phone.Substring(0, 3) + " **** " + phone.Substring(phone.Length - 4, 4) + "）";


            #region     计算支付手续费 start
            string sxfsq ="-1";
            decimal sxf = 0;

            decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。
                                 //查询该企业的设置
            List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + orderModel.CompID, "");
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

                //收取代理商手续费 (没有免支付次数时，才计算手续费)
                if (sxfsq == "1" && mfcs <= 0)
                {

                    sxf = payPrice * kjzfbl;
                    //if (sxf <= kjzfstart)
                    //    sxf = kjzfstart;
                    //else if (sxf >= kjzfend)
                    //    sxf = kjzfend;
                }
                //收取代理商手续费 (没有免支付次数时，才计算手续费)
                else if (sxfsq == "2" && mfcs <= 0)
                {

                    sxf = payPrice * kjzfbl;
                    //if (sxf <= kjzfstart)
                    //    sxf = kjzfstart;
                    //else if (sxf >= kjzfend)
                    //    sxf = kjzfend;

                    if (sxf > payPrice)//支付金额小于手续费时，提示不允许支付。
                    {
                        string Josn = ErrMessage("支付金额小于手续费，不允许支付");
                        context.Response.Write(Josn);
                        return;
                    }
                    comp_sxf = sxf;
                    sxf = 0;
                }
            }
            //支付总金额（含手续费）四舍五入
            payPrice = payPrice + Math.Round(sxf, 2,MidpointRounding.AwayFromZero);

            #endregion  计算支付手续费 end



            int regid = 0;
            Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
            //string strWhere2 = string.Empty;
            //strWhere2 += " OrderID = " + orderid;
            //strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
            String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

            string guid = Guid.NewGuid().ToString().Replace("-", "");
            payModel.OrderID = orderid;
            payModel.DisID = logUser.DisID;
            payModel.Type = isDBPay;
            payModel.PayUser = disModel.DisName;
            payModel.PayPrice = payPrice;
            payModel.guid = Common.Number_repeat(guid);
            payModel.IsAudit = 2;
            payModel.vdef3 = "1";
            payModel.vdef4 = orderNo;
            payModel.CreateDate = DateTime.Now;
            payModel.CreateUserID = logUser.UserID;
            payModel.ts = DateTime.Now;
            payModel.modifyuser = logUser.UserID;
            payModel.Channel = "1";//支付渠道
            payModel.State = Convert.ToInt32(sxfsq);//手续费收取方
            if (sxfsq.Equals("2"))
                payModel.vdef5 = comp_sxf.ToString("0.00");
            else
                payModel.vdef5 = sxf.ToString("0.00");//手续费
            payid = new Hi.BLL.PAY_Payment().Add(payModel);

            if (prepayid > 0)
            {
                Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
                prepayMent.vdef4 = payid.ToString();//与企业钱包关联
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
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(orderModel.CompID).CompName;
            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
            regModel.CreateUser = logUser.UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 1375;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (payid <= 0 || regid <= 0)
            {
                string Josn = ErrMessage("数据有误");
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
                    return;
                }

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构代码

                long amount = Convert.ToInt64(payPrice * 100);//支付金额（单位：分）

                // 2.创建交易请求对象
                Tx1375Request tx1375Request = new Tx1375Request();
                tx1375Request.setInstitutionID(institutionID);
                tx1375Request.setOrderNo(orderNo);// (orderNo + payid.ToString());//支付订单号 = 支付订单号前半部分 + 支付表ID;
                tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);//支付交易流水号
                tx1375Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidFastPay));//快捷支付号
                tx1375Request.setAmount(amount);//支付金额
                tx1375Request.setRemark("快捷支付发送短信".ToString());

                // 3.执行报文处理
                tx1375Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1375Request.getRequestMessage(), tx1375Request.getRequestSignature());

                Tx1375Response tx1375Response = new Tx1375Response(respMsg[0], respMsg[1]);
                try
                {
                    regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regModel.PlanMessage = tx1375Request.getRequestPlainText();
                    regModel.Start = tx1375Response.getCode();
                    regModel.ResultMessage = tx1375Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                }
                catch (Exception ex) { }

                if (!("2000".Equals(tx1375Response.getCode())))//返回Code=2000代表成功
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#msgtwo').hide();$('#msgone').show();msgTime(120);</script>");
                    string Josn1 = ErrMessage(tx1375Response.getMessage());
                    context.Response.Write(Josn1);
                    return;
                }
            }
            else
            {
                //模拟短信发送成功，回填数据
                try
                {
                    regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regModel.PlanMessage = "";
                    regModel.Start = "2000";
                    regModel.ResultMessage = "OK.";
                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
                }
                catch (Exception ex) { }
            }
            string Josn2 = "{\"success\":\"1\",\"payid\":\"" + payid + "\",\"prepayid\":\"" + prepayid + "\",\"js\":\"$('#phone').html('" + phoneHtml + "');$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#msgtwo').hide();$('#msgone').show();msgTime(120);\"}";
            context.Response.Write(Josn2);
            return;
        }
        catch (Exception ex)
        {
            string Josn1 = ErrMessage(ex.Message);
            context.Response.Write(Josn1);
            return;
        }
        finally
        {
            context.Response.End();
        }
    }

    public string ErrMessage(string msg)
    {
        return "{\"error\":\"1\",\"msg\":\""+msg+"！\"}";
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}