using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;
using Com.Alipay;

public partial class Distributor_Pay_Recharge : DisPageBase
{
    public string action2531 = "../../Handler/Tx2531.ashx";
    public string action2532 = "../../Handler/Tx2532.ashx";
    public string action1375 = "../../Handler/ReTx1375.ashx";
    public string action1376 = "../../Handler/ReTx1376.ashx";
    public string action1311 = "../../Handler/ReTx1311.ashx";

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public int KeyID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["KeyID"] == "")
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
        }
        if (!Common.PageDisOperable("PayPre", KeyID, this.DisID))
        {
            Response.Redirect("../../NoOperable.aspx", true);
            return;
        }
        if (!IsPostBack)
        {
            Bind();
            BindPaySettings(this.CompID); //给页面隐藏域赋值
        }
    }
    /// <summary>
    /// 给页面隐藏域赋值，手续费比例。
    /// </summary>
    /// <param name="compid"></param>
    public void BindPaySettings(int compid)
    {
        //查询该企业的设置
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + compid, "");
        if (Sysl.Count > 0)
        {
            //手续费收取
            string sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);
            this.pay_sxfsq.Value = sxfsq;

            //支付方式--线上or线下
            string zffs = Convert.ToString(Sysl[0].pay_zffs);
            this.pay_zffs.Value = zffs;

            //免手续费支付次数
            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);
            this.Pay_mfcs.Value = mfcs + "";

            //收取代理商手续费 (没有免支付次数时，才计算手续费)
            if (sxfsq == "1" && mfcs <= 0)
            {
                //手续费比例
                this.pay_kjzfbl.Value = (Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000).ToString("0.000");
                this.pay_kjzfstart.Value = Convert.ToString(Sysl[0].pay_kjzfstart);
                this.pay_kjzfend.Value = Convert.ToString(Sysl[0].pay_kjzfend);

                this.pay_ylzfbl.Value = (Convert.ToDecimal(Sysl[0].pay_ylzfbl) / 1000).ToString("0.000");
                this.pay_ylzfstart.Value = Convert.ToString(Sysl[0].pay_ylzfstart);
                this.pay_ylzfend.Value = Convert.ToString(Sysl[0].pay_ylzfend);

                this.pay_b2cwyzfbl.Value = (Convert.ToDecimal(Sysl[0].pay_b2cwyzfbl) / 1000).ToString("0.000");
                this.pay_b2cwyzfstart.Value = Sysl[0].vdef1;//网银支付封底

                this.pay_b2bwyzf.Value = Convert.ToString(Sysl[0].pay_b2bwyzf);

                this.hid_PayType.Value = "11";
            }
            else
            {
                div_kjzfsxf.Attributes.Add("style", "display:none;");
                div_wyzfsxf.Attributes.Add("style", "display:none;");
                div_sumprice.Attributes.Add("style", "display:none;");
            }
        }
        else
        {
            div_kjzfsxf.Attributes.Add("style", "display:none;");
            div_wyzfsxf.Attributes.Add("style", "display:none;");
            div_sumprice.Attributes.Add("style", "display:none;");
        }

    }
    public void Bind()
    {
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
        if (prepayM == null)
        {
            JScript.AlertMethod(this, "该记录无效！", JScript.IconOption.错误, "function (){ location.replace('" + ("PrePayList.aspx") + "'); }");
            return;
        }
        //支付金额小于0，直接支付失败
        if (prepayM.price <= 0)
        {
            Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
            return;
        }


        if (prepayM.Start == 1)
        {
            Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
            return;
        }
        this.hidKeyID.Value = Convert.ToString(KeyID);
        this.lblOrderNO.InnerText = prepayM.guid; // KeyID.ToString();
        this.lblPrice.InnerText = prepayM.price.ToString("0.00"); ;

        string username = this.UserName;
        string strWhere = string.Empty;
        if (this.DisID != 0)
        {
            strWhere += " DisID = '" + this.DisID + "' ";
        }
        else
        {
            JScript.AlertMsgOne(this, "操作员没有对应的代理商！", JScript.IconOption.错误);
            return;
        }
        strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        this.rptQpay.DataSource = fastList;
        this.rptQpay.DataBind();
        List<Hi.Model.PAY_BankInfo> BankL = new Hi.BLL.PAY_BankInfo().GetList("", " vdef1=0", "");
        this.rptOtherBank.DataSource = BankL;
        this.rptOtherBank.DataBind();
    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        string strWhere = " 1=1 ";
        if (KeyID > 0)
        {
            strWhere += " and OrderID = " + KeyID;
        }
        strWhere += " and vdef3 = 2 and status = 20 and isnull(dr,0)=0";
        List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList("", strWhere, "");
        if (payL.Count > 0)
        {
            int prepay = 0;
            int pay = 0;
            SqlConnection con = new SqlConnection(LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();
            try
            {
                pay = new Hi.BLL.PAY_Payment().updatePayState(con, payL[0].ID, sqlTrans);
                prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, KeyID, sqlTrans);
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
                JScript.AlertMethod(this, "转账已成功！", JScript.IconOption.正确, "function (){ location.replace('" + ("PrePayList.aspx") + "'); }");
                return;
            }
            else
            {
                JScript.AlertMsgOne(this, "系统繁忙，请稍后！", JScript.IconOption.错误);
                return;
            }
        }

        string a2 = this.hida2.Value;//是否使用快捷支付
        string a3 = this.hida3.Value;//是否使用网银支付
        string a5 = this.hida5.Value;//是否使用其他支付
        if (a2 == "1" && a3 == "0" && a5 == "0")
        {
            tx1375();
        }
        else if (a3 == "1" && a2 == "0" && a5 == "0")
        {
            tx1311();
        }
        else if (a5 == "1" && a2 == "0" && a3 == "0")
        {
            if (hidWxorAplipay.Value == "zfb")
                btnApliay();
            else
                btnWx();
        }
        else
        {
            JScript.AlertMsgOne(this, "请选择支付方式！", JScript.IconOption.错误);
            return;
        }
    }
    public void tx1375()
    {
        try
        {
            int hidFastPay = Convert.ToInt32(this.hidFastPay.Value);
            if (hidFastPay > 0)
            {
                string phone = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).phone;
                this.phone.InnerHtml = "（已发送至" + phone.Substring(0, 3) + " **** " + phone.Substring(phone.Length - 4, 4) + "）";
            }
            Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
            decimal price = prepayM.price;

            #region     计算支付手续费 start
            string AccountType = hid_PayType.Value;
            string bankid = hidBank.Value;

            string sxfsq = "-1";
            decimal sxf = 0;
            // 获取手续费 begin  
            string[] Json = Common.GetFastPay_sxf(this.CompID, price);

            string strMsg = Json[2].ToString();
            if (!string.IsNullOrEmpty(strMsg))
            {
                Tx1376ErrResult(strMsg);
                return;
            }
            else
            {
                sxfsq = Json[1].ToString();
                sxf = Convert.ToDecimal(Json[0]);
            }

            //支付总金额（含手续费）
            decimal UNIT = 0.01M;
            price = price + Common.Round(sxf, UNIT);

            #endregion  计算支付手续费 end

            String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

            int payid = 0;
            int regid = 0;
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
            payModel.OrderID = KeyID;
            payModel.DisID = this.DisID;
            payModel.PayUser = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            payModel.PayPrice = price;
            payModel.guid = Common.Number_repeat(guid);
            payModel.IsAudit = 2;
            payModel.vdef3 = "2";
            payModel.vdef4 = orderNo;
            payModel.CreateDate = DateTime.Now;
            payModel.CreateUserID = this.UserID;
            payModel.ts = DateTime.Now;
            payModel.modifyuser = this.UserID;

            payModel.Channel = "1";//支付渠道
            payModel.State = Convert.ToInt32(sxfsq);//手续费收取方
            payModel.vdef5 = sxf.ToString("0.00");//支付手续费
            payModel.vdef9 = "1";//默认支付
            payid = new Hi.BLL.PAY_Payment().Add(payModel);
            ViewState["payid"] = payid;

            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
            regModel.OrderId = KeyID;
            regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
            regModel.number = payModel.guid;
            regModel.Price = price;
            regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
            regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            regModel.DisID = this.DisID;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = prepayM.vdef1;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;
            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
            regModel.CreateUser = this.UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 1375;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (payid <= 0 || regid <= 0)
            {
                //JScript.AlertMsg(this, "数据异常！");
                ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('数据异常！');</script>");
                return;
            }

            if (WebConfigurationManager.AppSettings["Paytest_zj"] != "1")
            {

                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                try
                {
                    PaymentEnvironment.Initialize(configPath);
                }
                catch
                {
                    throw new Exception("支付配置不正确，请联系管理员！");
                }

                String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];
                // String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);   为了统一清算，作废次方法
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
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = tx1375Request.getRequestPlainText();
                    regM.Start = tx1375Response.getCode();
                    regM.ResultMessage = tx1375Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                }
                catch { }

                if (!"2000".Equals(tx1375Response.getCode()))
                {
                    //JScript.AlertMsg(this, tx1375Response.getMessage());
                    ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('" + tx1375Response.getMessage() + "！');</script>");
                    return;
                }
            }
            else
            {
                try
                {
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = "";
                    regM.Start = "2000";
                    regM.ResultMessage = "OK.";
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                }
                catch { }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#txtPhoneNum').select();$('#msgtwo').hide();$('#msgone').show();msgTime(120);</script>");
            return;
        }
        catch (Exception ex)
        {
            //JScript.AlertMsg(this, ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('" + ex.Message + "！');</script>");
            return;
        }
    }

    protected void btnTx1376_Click(object sender, EventArgs e)
    {
        try
        {
            Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
            Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
            if (Request.QueryString["KeyID"] == "")
            {
                KeyID = 0;
            }
            else
            {
                KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
            }

            string phoneCode = this.txtPhoneNum.Value;//检验码
            decimal price = 0;//使用企业钱包金额
            int hidPay = 0;//支付表ID
            if (ViewState["payid"] == null || ViewState["payid"] == "")
            {
                //JScript.ShowAlert(this, "数据异常！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
                Tx1376ErrResult("数据异常！");
                return;
            }
            hidPay = Convert.ToInt32(ViewState["payid"]);
            ViewState["payid"] = null;
            payM = new Hi.BLL.PAY_Payment().GetModel(hidPay);
            int hidPrepay = KeyID;//企业钱包表ID
            if (hidPrepay <= 0)
            {
                //JScript.ShowAlert(this, "数据异常！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
                Tx1376ErrResult("数据异常！");
                return;
            }
            prepayM = new Hi.BLL.PAY_PrePayment().GetModel(Convert.ToInt32(hidPrepay));
            price = prepayM.price;
            int hidFastPay = Convert.ToInt32(this.hidFastPay.Value);
            int regid = 0;

            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
            regModel.OrderId = hidPrepay;
            regModel.Ordercode = payM.vdef4;// WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidPrepay);
            regModel.number = payM.guid;
            regModel.Price = price;
            regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
            regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            regModel.DisID = this.DisID;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = prepayM.vdef1;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;
            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
            regModel.CreateUser = this.UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 1376;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (regid <= 0)
            {
                Tx1376ErrResult("数据异常！");
                return;
            }

            if (WebConfigurationManager.AppSettings["Paytest_zj"] != "1")
            {

                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                try
                {
                    PaymentEnvironment.Initialize(configPath);
                }
                catch
                {
                    throw new Exception("支付配置不正确，请联系管理员！");
                }

                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码

                Tx1376Request tx1376Request = new Tx1376Request();

                tx1376Request.setInstitutionID(institutionID);
                tx1376Request.setOrderNo(payM.vdef4);//WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidPrepay)
                tx1376Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(Convert.ToInt32(hidPay)).guid);
                tx1376Request.setSmsValidationCode(phoneCode);

                tx1376Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1376Request.getRequestMessage(), tx1376Request.getRequestSignature());

                Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);

                try
                {
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = tx1376Request.getRequestPlainText();
                    regM.Start = tx1376Response.getCode();
                    regM.ResultMessage = tx1376Response.getMessage();
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                    Hi.Model.PAY_Payment payModel = new Hi.BLL.PAY_Payment().GetModel(hidPay);
                    payM.PayDate = DateTime.Now;
                    payModel.ts = DateTime.Now;
                    payModel.verifystatus = tx1376Response.getVerifyStatus();
                    payModel.status = tx1376Response.getStatus();
                    new Hi.BLL.PAY_Payment().Update(payModel);
                }
                catch { }
                if (!"2000".Equals(tx1376Response.getCode()))
                {
                    Tx1376ErrResult(tx1376Response.getMessage());
                    return;
                }
                int VerifyStatus = tx1376Response.getVerifyStatus();
                int Status = tx1376Response.getStatus();
                if (VerifyStatus != 40 || Status != 20)//VerifyStatus = 40 验证码验证成功，Status = 20 支付成功
                {
                    if (Status == 10)
                    {
                        //中金：支付处理中，钱会从账户中扣除，第二天会退回到账户中。
                        Tx1376ErrResult("代扣失败");
                        return;
                    }
                    Tx1376ErrResult(tx1376Response.getResponseMessage());
                    return;
                }

            }
            else
            {
                try
                {
                    Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    regM.PlanMessage = "";
                    regM.Start = "2000";
                    regM.ResultMessage = "OK.";
                    new Hi.BLL.PAY_RegisterLog().Update(regM);
                    Hi.Model.PAY_Payment payModel = new Hi.BLL.PAY_Payment().GetModel(hidPay);
                    payM.PayDate = DateTime.Now;
                    payModel.ts = DateTime.Now;
                    payModel.verifystatus = 40;
                    payModel.status = 20;
                    new Hi.BLL.PAY_Payment().Update(payModel);
                }
                catch { }
            }

            int prepay = 0;
            int pay = 0;
            SqlConnection con = new SqlConnection(LocalSqlServer);
            con.Open();
            SqlTransaction sqlTrans = con.BeginTransaction();
            try
            {
                pay = new Hi.BLL.PAY_Payment().updatePayState(con, hidPay, sqlTrans);
                prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, hidPrepay, sqlTrans);
                if (pay > 0 && prepay > 0)
                    sqlTrans.Commit();
                else
                    sqlTrans.Rollback();
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
            if (pay <= 0 || prepay <= 0)
            {
                Tx1376ErrResult("支付成功，但修改支付状态失败，请联系系统管理员，勿重复操作！");
                return;
            }
            Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey), false);
            return;
        }
        catch (Exception ex)
        {
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "function (){ $('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100); }");
            return;
        }
    }

    public void Tx1376ErrResult(string msg)
    {
        //检验验证码错误，返回信息，并关闭弹出层
        //JScript.ShowAlert(this, msg, "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
        ClientScript.RegisterStartupScript(this.GetType(), "payTx1376", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);$('#lblErr').html('" + msg + "！');</script>");
    }

    public void tx1311()
    {
        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        try
        {
            //账户类型
            string AccountType = Request["AccountType"];
            int accountType = Convert.ToInt32(AccountType);

            //银行编码
            string bankid = this.hidBank.Value;

            Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
            decimal price = prepayM.price;

            #region     计算支付手续费 start
            string sxfsq = "-1";
            decimal sxf = 0;
            decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。
            // 获取手续费 begin  
            string[] Json = Common.GetSxf(this.CompID, AccountType, bankid, price);

            string strMsg = Json[2].ToString();
            if (!string.IsNullOrEmpty(strMsg))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html(' 充值金额小于支付手续费，不允许支付 ！');</script>");
                return;
            }
            else
            {
                sxfsq = Json[1].ToString();
                sxf = Convert.ToDecimal(Json[0]);
                comp_sxf = Convert.ToDecimal(Json[3]);
            }

            //支付总金额（含手续费）
            decimal UNIT = 0.01M;
            price = price + Common.Round(sxf, UNIT);

            #endregion  计算支付手续费 end



            int payid = 0;
            int regid = 0;
            string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

            string guid = Guid.NewGuid().ToString().Replace("-", "");
            payModel.OrderID = KeyID;
            payModel.DisID = this.DisID;
            payModel.PayUser = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            payModel.PayPrice = price;
            payModel.IsAudit = 2;
            payModel.guid = Common.Number_repeat(guid);
            payModel.vdef3 = "2";
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
            }//个人贷记卡支付
            else if (AccountType == "13")
                payModel.Channel = "8";
            else
                payModel.Channel = "4";
            payModel.State = Convert.ToInt32(sxfsq);//手续费收取方
            if (sxfsq.Equals("2"))
                payModel.vdef5 = comp_sxf.ToString("0.00");
            else
                payModel.vdef5 = sxf.ToString("0.00");//支付手续费

            payid = new Hi.BLL.PAY_Payment().Add(payModel);

            regModel.OrderId = KeyID;
            regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
            regModel.number = payModel.guid;
            regModel.Price = price;
            regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
            regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            regModel.DisID = this.DisID;
            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            regModel.Remark = prepayM.vdef1;
            regModel.DisName = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;
            regModel.BankID = bankid;
            regModel.CreateUser = this.UserID;
            regModel.CreateDate = DateTime.Now;
            regModel.LogType = 1311;
            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

            if (payid <= 0 || regid <= 0)
            {
                //JScript.AlertMsg(this, "数据异常！");
                ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('数据异常！');</script>");
                return;
            }

            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            try
            {
                PaymentEnvironment.Initialize(configPath);
            }
            catch
            {
                //JScript.AlertMsg(this, "支付配置有误，请联系系统管理员！");
                //return;
                throw new Exception("支付配置有误，请联系系统管理员！");
            }

            String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];
            //原来传递的支付订单号----目前订单号运行重复
            //String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
            long amount = Convert.ToInt64(price * 100);
            long fee = 0;
            String payerID = this.DisID.ToString();
            String payerName = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
            String usage = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
            String remark = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";

            String notificationURL = string.Empty;

            if (WebConfigurationManager.AppSettings["PayType"] == "0")
                notificationURL = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/Handler/ReceiveNoticePage.ashx";
            else
                notificationURL = "http://www.my1818.com/Handler/ReceiveNoticePage.ashx";//回调页面地址

            String payees = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;


            //判断支付接口
            string Pay_type = WebConfigurationManager.AppSettings["PayType"];
            String bankID = string.Empty;
            if (Pay_type == "0")
                bankID = "700";//bankid;//
            else
                bankID = bankid;



            // 2.创建交易请求对象
            Tx1311Request tx1311Request = new Tx1311Request();
            tx1311Request.setInstitutionID(institutionID);//机构号码
            tx1311Request.setOrderNo(orderNo);//转账流水号
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

            // 3.执行报文处理
            tx1311Request.process();

            try
            {
                Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                regM.PlanMessage = tx1311Request.getRequestPlainText();
                new Hi.BLL.PAY_RegisterLog().Update(regM);
            }
            catch { }

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
            Context.Server.Transfer("ReRequest.aspx", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('" + ex.Message + "！');</script>");
            return;
        }
    }

    /// <summary>
    /// 支付宝支付
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApliay()
    {
        //企业钱包充值记录
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(KeyID);
        decimal price = prepayM.price;

        #region 支付记录
        int payid = 0;
        int regid = 0;
        string guid = Guid.NewGuid().ToString().Replace("-", "");
        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        payModel.OrderID = KeyID;
        payModel.DisID = this.DisID;
        payModel.PayUser = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
        payModel.PayPrice = price;
        payModel.guid = Common.Number_repeat(guid);
        payModel.IsAudit = 2;
        payModel.vdef3 = "2";
        payModel.CreateDate = DateTime.Now;
        payModel.CreateUserID = this.UserID;
        payModel.ts = DateTime.Now;
        payModel.modifyuser = this.UserID;

        payModel.Channel = "6";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付
        payModel.State = 0;//手续费收取方
        payModel.vdef5 = "0.00";//支付手续费
        payModel.PrintNum = 1;//结算标示

        payid = new Hi.BLL.PAY_Payment().Add(payModel);

        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        regModel.OrderId = KeyID;
        regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(KeyID);
        regModel.number = payModel.guid;
        regModel.Price = price;
        regModel.Payuse = prepayM.PreType == 6 ? "转账汇款" : prepayM.PreType == 1 ? "企业钱包充值" : "";
        regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(this.DisID).DisName;
        regModel.DisID = this.DisID;
        regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        regModel.Remark = prepayM.vdef1;
        regModel.DisName = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;
        regModel.BankID = "支付宝";
        regModel.CreateUser = this.UserID;
        regModel.CreateDate = DateTime.Now;
        regModel.LogType = 1375;
        regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);

        if (payid <= 0 || regid <= 0)
        {
            //JScript.AlertMsg(this, "数据异常！");
            ClientScript.RegisterStartupScript(this.GetType(), "payTx1375", "<script>$('#lblErr').html('数据异常！');</script>");
            return;
        }

        #endregion

        #region  支付宝支付
        //支付类型
        string payment_type = "1";
        //必填，不能修改
        //服务器异步通知页面路径 
        string notify_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Distributor/Pay/ReceiveNoticeAlipay.aspx";
        //需http://格式的完整路径，不能加?id=123这类自定义参数

        //页面跳转同步通知页面路径
        string return_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Distributor/pay/PrePayList.aspx";
        //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

        //卖家支付宝帐户
        //string seller_email = "sczfb@lansiu.com";
        string seller_email = Common.GetPayWxandAli(this.CompID).ali_seller_email;// ConfigurationManager.AppSettings["seller_email"] == null ? "shuzj@haiyusoft.com" : ConfigurationManager.AppSettings["seller_email"].ToString().Trim();

        //必填
        //商户订单号
        string out_trade_no = payModel.guid;// OrderModel.GUID;
        //商户网站订单系统中唯一订单号，必填



        //订单名称
        string subject = "钱包充值";//OrderModel.GUID;
        //必填
        //付款金额
        string total_fee = price.ToString("0.00");

        string paymethod = string.Empty;  //默认支付方式
        string defaultbank = string.Empty;  //默认网银

        //if (OrderInfoModel.BankId != "0" && OrderInfoModel.BankId != "")
        //{
        //    //默认支付方式
        //    paymethod = "bankPay";
        //    //必填
        //    //默认网银
        //    defaultbank = OrderInfoModel.BankId;
        //}

        //必填
        //订单描述
        string body = prepayM.vdef1;
        //商品展示地址
        string show_url = "http://www.my1818.com";
        //需以http://开头的完整路径，

        //防钓鱼时间戳
        string anti_phishing_key = Submit.Query_timestamp();//"";
        //若要使用请调用类文件submit中的query_timestamp函数

        //客户端的IP地址
        string exter_invoke_ip = Page.Request.UserHostAddress;
        //非局域网的外网IP地址，如：221.0.0.1

        //把请求参数打包成数组
        SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
        sParaTemp.Add("partner", Config.Partner);
        sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
        sParaTemp.Add("service", "create_direct_pay_by_user");
        sParaTemp.Add("payment_type", payment_type);
        sParaTemp.Add("notify_url", notify_url);
        sParaTemp.Add("return_url", return_url);
        sParaTemp.Add("seller_email", seller_email);
        sParaTemp.Add("out_trade_no", out_trade_no);
        sParaTemp.Add("subject", subject);
        sParaTemp.Add("total_fee", total_fee);
        sParaTemp.Add("body", body);

        //if (OrderInfoModel.BankId != "0" && OrderInfoModel.BankId != "")
        //{
        //    sParaTemp.Add("paymethod", paymethod);
        //    sParaTemp.Add("defaultbank", defaultbank);
        //}

        sParaTemp.Add("show_url", show_url);
        sParaTemp.Add("anti_phishing_key", anti_phishing_key);
        sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

        //建立请求
        string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
        Response.Write(sHtmlText);
        #endregion

    }

    /// <summary>
    ///支付宝支付 错误处理页面
    /// </summary>
    public void ErrMessage(string msg)
    {
        Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt(msg, Common.EncryptKey), false);

    }

    /// <summary>
    /// 微信支付
    /// </summary>
    protected void btnWx()
    {
        Response.Redirect("WeChatPay.aspx?pre=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
    }

}