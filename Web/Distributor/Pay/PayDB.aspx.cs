using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

using FinancingReferences;
using LitJson;

public partial class Distributor_Pay_PayDB : System.Web.UI.Page
{
    public string action2531 = "../../Handler/Tx2531.ashx";
    public string action2532 = "../../Handler/Tx2532.ashx";
    public string action1375 = "../../Handler/Tx1375.ashx";
    public string action1376 = "../../Handler/Tx1376.ashx";
    public string action1311 = "../../Handler/Tx1311.ashx";
    public string action17000 = "../../Handler/Trd17000.ashx";
    public string action71000 = "../../Handler/Trd71000.ashx";
    public string action13010 = "../../Handler/Trd13010.ashx";
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public LoginModel loguser = HttpContext.Current.Session["UserModel"] as LoginModel;
    Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
    public Hi.Model.DIS_Order orderModel = new Hi.Model.DIS_Order();
    public List<Hi.Model.PAY_Financing> PList = new List<Hi.Model.PAY_Financing>();

    public int KeyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["KeyID"] == "")
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Common.DesDecrypt(Request.QueryString["KeyID"], Common.EncryptKey).ToInt(0);
        }

        if (!Common.PageDisOperable("Order", KeyID, loguser.DisID))
        {
            Response.Redirect("../../NoOperable.aspx", true);
            return;
        }
        if (!IsPostBack)
        {

            Bind();
        }
    }

    public void Bind()
    {
        orderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        if (!((
            (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (orderModel.Otype == (int)Enums.OType.推送账单 && orderModel.OState == (int)Enums.OrderState.已审 && (orderModel.PayState == (int)Enums.PayState.部分支付 || orderModel.PayState == (int)Enums.PayState.未支付))
            )
            && orderModel.OState != (int)Enums.OrderState.已作废))
        {
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                JScript.AlertMethod(this, "账单异常，不能支付！", JScript.IconOption.错误, "function (){ location.replace('" + ( "orderDzfzdList.aspx") + "'); }");
            else
            JScript.AlertMethod(this, "订单异常，不能支付！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderPayList.aspx") + "'); }");
            return;
        }
        if (orderModel == null)
        {
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                JScript.AlertMethod(this, "无效的账单！", JScript.IconOption.错误, "function (){ location.replace('" + ( "orderDzfzdList.aspx") + "'); }");
            else
            JScript.AlertMethod(this, "无效的订单！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderPayList.aspx") + "'); }");
            //this.lblPayError.InnerText = "无效的订单！";
            return;
        }
        //if (orderModel.DisID != user.DisID)
        //{
        //    JScript.AlertMsg(this,"操作有误！");
        //    return;
        //}
        if (orderModel.OState < 2)
        {
            JScript.AlertMethod(this, "该订单未审核,不能支付!", JScript.IconOption.错误, "function (){ location.replace('" + ("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)) + "'); }");
            return;
        }
        if (orderModel.ReturnState == 0 || orderModel.ReturnState == 1)
        {

        }
        else
        {
            JScript.AlertMethod(this, "该订单已退货或正在退货,不能支付!", JScript.IconOption.错误, "function (){ location.replace('" + ("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)) + "'); }");
            return;
        }
        if (orderModel.PayState != (int)Enums.PayState.未支付 && orderModel.PayState != (int)Enums.PayState.部分支付)
        {
            Response.Redirect("PayError.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
            return;
        }
        List<Hi.Model.PAY_OpenAccount> LOpen = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + loguser.DisID + " and State=1 and isnull(dr,0)=0", "");
        List<Hi.Model.PAY_Withdrawals> Lwith = new Hi.BLL.PAY_Withdrawals().GetList("", "DisID=" + loguser.DisID + " and state=1 and isnull(dr,0)=0", "");
        if (LOpen.Count > 0 && Lwith.Count > 0)
        {
            String msghd_rspcode = "";
            String msghd_rspmsg = "";
            String amt_balamt = "";
            String msghd_rspcode1 = "";
            String msghd_rspmsg1 = "";
            String credit_nousedamt = "";
            String Json = "";
            String Json1 = "";
            try
            {
                IPubnetwk ipu = new IPubnetwk();
                Json = ipu.trd13010("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + Common.Number_repeat("") + "\",\"cltacc_cltno\":\"" + LOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LOpen[0].AccName + "\",\"bkacc_accno\":\"" + Lwith[0].AccNm + "\",\"bkacc_accnm\":\"" + LOpen[0].AccName + "\"}");
                Json1 = ipu.trd70000("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + Common.Number_repeat("") + "\",\"cltacc_cltno\":\"" + LOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LOpen[0].AccName + "\"}");
            }
            catch { }
            try
            {
                JsonData Params = JsonMapper.ToObject(Json);
                msghd_rspcode = Params["msghd_rspcode"].ToString();
                msghd_rspmsg = Params["msghd_rspmsg"].ToString();
                amt_balamt = Params["amt_balamt"].ToString();
                this.lblBalance1.InnerHtml = (Convert.ToDecimal(amt_balamt) / 100).ToString("N2");
                JsonData Params1 = JsonMapper.ToObject(Json1);
                msghd_rspcode1 = Params1["msghd_rspcode"].ToString();
                msghd_rspmsg1 = Params1["msghd_rspmsg"].ToString();
                credit_nousedamt = Params1["credit_nousedamt"].ToString();
                this.lblBalance3.InnerHtml = (Convert.ToDecimal(credit_nousedamt) / 100).ToString("N2");
            }
            catch { }
        }

        PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + loguser.DisID + " and OrderID=" + KeyID + " and State=3 and isnull(dr,0)=0", "");
        if (PList.Count > 0)
        {
            this.lblBalance5.InnerHtml = PList[0].AclAmt.ToString("0.00");
        }

        decimal payPrice = orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount;
        this.lblOrderNO.InnerText = orderModel.ReceiptNo.Trim().ToString();
        this.lblOrderNO.HRef = "../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        this.hidOrderid.Value = KeyID.ToString();
        this.lblPricePay.InnerText = payPrice.ToString("0.00");
        this.hidPricePay.Value = payPrice.ToString("0.00");
        this.lblPriceO.InnerText = (orderModel.AuditAmount + orderModel.OtherAmount).ToString("0.00");
        if (this.txtPayOrder.Value == "")
            this.txtPayOrder.Value = payPrice.ToString("0.00");
        else
            this.txtPayOrder.Value = Convert.ToDecimal(this.txtPayOrder.Value).ToString("0.00");
        this.hidUserName.Value = loguser.UserName;

        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(orderModel.DisID, orderModel.CompID);
        this.lblSumPrice.InnerText = sumPrice.ToString("0.00");
        this.hidSumPrice.Value = sumPrice.ToString("0.00");

        string strWhere = " 1=1 ";
        if (loguser.DisID != 0)
        {
            strWhere += " and DisID = " + loguser.DisID;
        }
        strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        this.rptQpay.DataSource = fastList;
        this.rptQpay.DataBind();
        List<Hi.Model.PAY_BankInfo> BankL = new Hi.BLL.PAY_BankInfo().GetList("", " vdef1=0", "");
        this.rptOtherBank.DataSource = BankL;
        this.rptOtherBank.DataBind();
    }

    protected void btnPaySuccess_Click(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + loguser.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        if (PList != null && PList.Count > 0)
        {
            if (orderModel.Otype != 9)
            {
                Response.Redirect("orderPayList.aspx");
            }
            else
            {
                Response.Redirect("orderDzfzdList.aspx");
            }
        }
        else
        {
            if (orderModel.Otype != 9)
            {
                Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderpaylist");
            }
            else
            {
                Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderZDList");
            }
        }
    }

    protected void btnPayFailure_Click(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + loguser.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        if (PList != null && PList.Count > 0)
        {
            if (orderModel.Otype != 9)
            {
                Response.Redirect("orderPayList.aspx");
            }
            else
            {
                Response.Redirect("orderDzfzdList.aspx");
            }
        }
        else
        {
            if (orderModel.Otype != 9)
            {
                Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderpaylist");
            }
            else
            {
                Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderZDList");
            }
        }
    }

    //protected void btnPay_Click(object sender, EventArgs e)
    //{
    //    user = LoginModel.IsLogined(this);
    //    if (Request.QueryString["KeyID"] == "")
    //    {
    //        KeyID = 0;
    //    }
    //    else
    //    {
    //        KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
    //    }

    //    string a1 = this.hida1.Value;
    //    string a2 = this.hida2.Value;
    //    string a3 = this.hida3.Value;

    //    string password = new Hi.BLL.BD_Distributor().GetModel(user.DisID).Paypwd;
    //    if (a1 == "1")
    //    {
    //        if (password == Util.md5("123456"))
    //        {
    //            //JScript.AlertMsg(this, "请先修改支付密码！", "../PayPWDEdit.aspx");
    //            this.lblPayError.InnerText = "请先修改支付密码！";
    //            return;
    //        }
    //        else
    //        {
    //            if (this.padPaypas.Value == "")
    //            {
    //                //JScript.AlertMsg(this, "密码不能为空！");
    //                this.lblPayError.InnerText = "密码不能为空！";
    //                return;
    //            }
    //        }
    //    }

    //    Hi.Model.DIS_Order orderM = new Hi.BLL.DIS_Order().GetModel(KeyID);
    //    if ((orderM.AuditAmount + orderM.OtherAmount) == 0)
    //    {
    //        //JScript.AlertMsg(this, "订单金额为0，不能支付！");
    //        this.lblPayError.InnerText = "订单金额为0，不能支付！";
    //        return;
    //    }

    //    decimal txtPayOrder = Convert.ToDecimal(this.txtPayOrder.Value);
    //    if (txtPayOrder > orderM.AuditAmount + orderM.OtherAmount - orderM.PayedAmount)
    //    {
    //        this.lblPayError.InnerText = "支付金额大于未支付金额，不能支付！";
    //        return;
    //    }
    //    decimal prePrice = Convert.ToDecimal(this.txtPrice.Value);
    //    if (a1 == "1" && txtPayOrder < prePrice)
    //    {
    //        //JScript.AlertMsg(this, "使用企业钱包大于订单金额！");
    //        this.lblPayError.InnerText = "使用企业钱包大于支付金额！";
    //        return;
    //    }
    //    if ((a2 == "1" && a3 == "0") || (a2 == "0" && a3 == "0"))
    //    {
    //        tx1375(a1);
    //    }
    //    else if (a3 == "1" && a2 == "0")
    //    {
    //        tx1311(a1);
    //    }
    //    else
    //    {
    //        //JScript.AlertMsg(this, "操作有误！");
    //        this.lblPayError.InnerText = "操作有误！";
    //        return;
    //    }
    //}
    /// <summary>
    /// 快捷支付，获取验证码接口
    /// </summary>
    /// <param name="a1"></param>
    //public void tx1375(string a1)
    //{
    //    int payid = 0;
    //    int prepayid = 0;
    //    if (Request.QueryString["KeyID"] == "")
    //    {
    //        KeyID = 0;
    //    }
    //    else
    //    {
    //        KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
    //    }

    //    Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);//代理商对象
    //    decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
    //    int disid = user.DisID;//代理商ID

    //    string payPas = "";
    //    if (this.padPaypas.Value != "")
    //    {
    //        payPas = Convert.ToString(this.padPaypas.Value);//企业钱包密码
    //    }
    //    decimal price = 0;
    //    if (this.txtPrice.Value != "")
    //    {
    //        price = Convert.ToDecimal(this.txtPrice.Value);//使用企业钱包金额
    //    }
    //    decimal payPrice = 0;
    //    if (this.txtPayOrder.Value != "")
    //    {
    //        if (a1 == "1")
    //            payPrice = Convert.ToDecimal(this.txtPayOrder.Value) - price;
    //        else
    //            payPrice = Convert.ToDecimal(this.txtPayOrder.Value);
    //    }
    //    int hidFastPay = 0;
    //    if (this.hidFastPay.Value != "")
    //    {
    //        hidFastPay = Convert.ToInt32(this.hidFastPay.Value);
    //    }
    //    else
    //    {
    //        this.lblPayError.InnerText = "请选择快捷支付！";
    //        return;
    //    }
    //    if (hidFastPay > 0)
    //    {
    //        Hi.Model.PAY_FastPayMent fastM = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay);
    //        if (fastM != null)
    //        {
    //            if (fastM.DisID != user.DisID)
    //            {
    //                //JScript.AlertMsg(this,"操作有误！");
    //                this.lblPayError.InnerText = "操作有误！";
    //                return;
    //            }
    //            else
    //            {
    //                string phone = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).phone;
    //                this.phone.InnerHtml = "（已发送至" + phone.Substring(0, 3) + " **** " + phone.Substring(phone.Length - 4, 4) + "）";
    //            }
    //        }
    //        else
    //        {
    //            //JScript.AlertMsg(this,"操作有误！");
    //            this.lblPayError.InnerText = "操作有误！";
    //            return;
    //        }
    //    }
    //    int orderid = KeyID;//订单id
    //    if (orderid == 0)
    //    {
    //        //JScript.AlertMsg(this, "系统繁忙，请稍后！");
    //        this.lblPayError.InnerText = "操作有误！";
    //        return;
    //    }
    //    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
    //    string strWhere1 = " 1=1 ";
    //    if (orderid != 0)
    //    {
    //        strWhere1 += " OrderID = '" + orderid + "'";
    //    }
    //    strWhere1 += " and IsEnabled=1 and Start=1 and isnull(dr,0)=0";

    //    if (a1 == "1" && price > 0)
    //    {
    //        //企业钱包处理
    //        if (sumPrice < price)
    //        {
    //            //JScript.AlertMsg(this, "企业钱包余额不足！");
    //            this.lblPayError.InnerText = "企业钱包余额不足！";
    //            return;
    //        }
    //        else
    //        {
    //            if (disModel.Paypwd == Util.md5(payPas))
    //            {
    //                Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
    //                //string strWhere = string.Empty;
    //                //if (orderid != 0)
    //                //{
    //                //    strWhere += " OrderID = '" + orderid + "'";
    //                //}
    //                //strWhere += " and IsEnabled=1 and isnull(dr,0)=0";
    //                //List<Hi.Model.PAY_PrePayment> prepayList = new Hi.BLL.PAY_PrePayment().GetList("", strWhere, "");
    //                try
    //                {
    //                    //if (prepayList.Count > 0)
    //                    //{
    //                    //    prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(prepayList[0].ID);
    //                    //    prepayModel.price = price * -1;
    //                    //    prepayModel.ts = DateTime.Now;
    //                    //    prepayModel.modifyuser = user.ID;
    //                    //    flag = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
    //                    //    if (flag)
    //                    //    {
    //                    //        prepayid = prepayList[0].ID;
    //                    //    }
    //                    //}   
    //                    //else
    //                    //{
    //                    prepayModel.CompID = disModel.CompID;
    //                    prepayModel.DisID = disModel.ID;
    //                    prepayModel.OrderID = orderid;
    //                    prepayModel.Start = 2;
    //                    prepayModel.PreType = 5;
    //                    prepayModel.price = price * -1;
    //                    prepayModel.Paytime = DateTime.Now;
    //                    prepayModel.CreatDate = DateTime.Now;
    //                    prepayModel.CrateUser = user.ID;
    //                    prepayModel.AuditState = 2;
    //                    prepayModel.IsEnabled = 1;
    //                    prepayModel.ts = DateTime.Now;
    //                    prepayModel.modifyuser = user.ID;
    //                    prepayModel.vdef1 = "订单支付";
    //                    prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
    //                    //}
    //                    ViewState["prepayid"] = prepayid;
    //                }
    //                catch
    //                {
    //                    this.lblPayError.InnerText = "系统繁忙，请稍后再试！";
    //                    return;
    //                }
    //                int prepay = 0;
    //                int order = 0;
    //                if (prepayid > 0 && payPrice == 0)
    //                {

    //                    SqlConnection con = new SqlConnection(LocalSqlServer);
    //                    con.Open();
    //                    SqlTransaction sqlTrans = con.BeginTransaction();
    //                    try
    //                    {
    //                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
    //                        order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price, sqlTrans);

    //                        sqlTrans.Commit();
    //                    }
    //                    catch
    //                    {
    //                        prepay = 0;
    //                        order = 0;
    //                        sqlTrans.Rollback();
    //                    }
    //                    finally
    //                    {
    //                        con.Close();
    //                    }
    //                    if (prepay > 0 && order > 0)
    //                    {
    //                        try
    //                        {
    //                            new Common().GetWxService("2", orderid.ToString(), "1");
    //                        }
    //                        catch { }
    //                        finally
    //                        {
    //                            //Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "");
    //                            Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "支付：" + price.ToString("0.00") + "元");
    //                        }
    //                        Response.Redirect("PaySuccess.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey));
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        //JScript.AlertMsg(this, "系统繁忙，请稍后！");
    //                        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                        return;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                //JScript.AlertMsg(this, "密码不正确！");
    //                this.lblPayError.InnerText = "密码不正确！";
    //                return;
    //            }
    //        }
    //    }

    //    if (payPrice > 0)
    //    {
    //        //if (a1 == "0")
    //        //{
    //        //    price = 0;
    //        //}
    //        int regid = 0;
    //        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
    //        string strWhere2 = string.Empty;
    //        if (orderid != 0)
    //        {
    //            strWhere2 += " OrderID = " + orderid;
    //        }
    //        strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
    //        String orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");
    //        try
    //        {
    //            string guid = Guid.NewGuid().ToString().Replace("-", "");
    //            payModel.OrderID = orderid;
    //            payModel.DisID = user.DisID;
    //            payModel.PayUser = disModel.DisName;
    //            payModel.PayPrice = payPrice;// (orderModel.AuditAmount + orderModel.OtherAmount) - price;
    //            payModel.guid = Common.Number_repeat(guid);
    //            payModel.IsAudit = 2;
    //            payModel.vdef3 = "1";
    //            payModel.vdef4 = orderNo;
    //            payModel.CreateDate = DateTime.Now;
    //            payModel.CreateUserID = user.ID;
    //            payModel.ts = DateTime.Now;
    //            payModel.modifyuser = user.ID;
    //            payid = new Hi.BLL.PAY_Payment().Add(payModel);
    //            ViewState["payid"] = payid;
    //            if (prepayid > 0)
    //            {
    //                Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
    //                prepayMent.vdef4 = payid.ToString();
    //                new Hi.BLL.PAY_PrePayment().Update(prepayMent);
    //            }

    //            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
    //            regModel.OrderId = Convert.ToInt32(orderid);
    //            regModel.Ordercode = orderNo + payid.ToString();
    //            regModel.number = payModel.guid;
    //            regModel.Price = payPrice;// (orderModel.AuditAmount + orderModel.OtherAmount) - price;
    //            regModel.Payuse = "订单支付";
    //            regModel.PayName = disModel.DisName;
    //            regModel.DisID = disid;
    //            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    //            regModel.Remark = orderModel.Remark;
    //            regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
    //            regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
    //            regModel.CreateUser = user.ID;
    //            regModel.CreateDate = DateTime.Now;
    //            regModel.LogType = 1375;
    //            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
    //        }
    //        catch
    //        {
    //            this.lblPayError.InnerText = "系统繁忙，请稍后再试！";
    //            return;
    //        }
    //        if (payid > 0 && regid > 0)
    //        {
    //            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
    //            PaymentEnvironment.Initialize(configPath);

    //            String institutionID = "001520";

    //            long amount = Convert.ToInt64(payPrice * 100);

    //            // 2.创建交易请求对象
    //            Tx1375Request tx1375Request = new Tx1375Request();
    //            tx1375Request.setInstitutionID(institutionID);
    //            tx1375Request.setOrderNo(orderNo + payid.ToString());
    //            tx1375Request.setPaymentNo(new Hi.BLL.PAY_Payment().GetModel(payid).guid);
    //            tx1375Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(hidFastPay));
    //            tx1375Request.setAmount(amount);
    //            tx1375Request.setRemark("快捷支付发送短信".ToString());

    //            // 3.执行报文处理
    //            tx1375Request.process();

    //            TxMessenger txMessenger = new TxMessenger();
    //            String[] respMsg = txMessenger.send(tx1375Request.getRequestMessage(), tx1375Request.getRequestSignature());

    //            Tx1375Response tx1375Response = new Tx1375Response(respMsg[0], respMsg[1]);
    //            try
    //            {
    //                Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
    //                regModel.PlanMessage = tx1375Request.getRequestPlainText();
    //                regModel.Start = tx1375Response.getCode();
    //                regModel.ResultMessage = tx1375Response.getMessage();
    //                new Hi.BLL.PAY_RegisterLog().Update(regModel);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }

    //            if ("2000".Equals(tx1375Response.getCode()))
    //            {
    //                //支付验证码发送成功
    //                //string Josn = "{\"payid\":\"" + payid + "\",\"prepayid\":\"" + prepayid + "\",\"falg\":\"2\",\"msg\":\"验证码发送成功！\"}";
    //                //JScript.ShowAlert(this, "验证码发送成功！", "$('#hidPay').val(" + payid + ");$('hidPrepay').val("+prepayid+");$('.opacity').fadeIn(200);$('.tip').fadeIn(200);");
    //                ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#msgtwo').hide();$('#msgone').show();msgTime(120);</script>");
    //                return;
    //            }
    //            else
    //            {
    //                JScript.AlertMsg(this, tx1375Response.getMessage());
    //                return;
    //            }
    //            //HttpContext.Current.Items["plainText"] = tx1375Response.getResponsePlainText();
    //            //context.Server.Transfer("../Distributor/Pay/Response.aspx", true);
    //        }
    //        else
    //        {
    //            //JScript.AlertMsg(this, "系统繁忙，请稍后！");
    //            this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //            return;
    //        }
    //    }
    //}

    /// <summary>
    /// 快捷支付，检验验证码接口
    /// </summary>
    /// <param name="a1"></param>
    //protected void btnTx1376_Click(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["KeyID"] == "")
    //    {
    //        KeyID = 0;
    //    }
    //    else
    //    {
    //        KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
    //    }
    //    user = LoginModel.IsLogined(this);
    //    Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);//代理商对象
    //    string phoneCode = this.txtPhoneNum.Value;//检验码
    //    string a1 = this.hida1.Value;
    //    decimal price = 0;//使用企业钱包金额
    //    int payid = 0;
    //    decimal payPrice = 0;
    //    if (ViewState["payid"] != null)
    //    {
    //        payid = Convert.ToInt32(ViewState["payid"].ToString());
    //        ViewState["payid"] = null;
    //        payM = new Hi.BLL.PAY_Payment().GetModel(payid);
    //        payPrice = payM.PayPrice;
    //    }
    //    if (payM == null)
    //    {
    //        //JScript.ShowAlert(this, "系统繁忙，请稍后！");
    //        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //        return;
    //    }
    //    int prepayid = 0;
    //    if (ViewState["prepayid"] != null)
    //    {
    //        prepayid = Convert.ToInt32(ViewState["prepayid"].ToString());
    //        ViewState["prepayid"] = null;
    //        prepayM = new Hi.BLL.PAY_PrePayment().GetModel(Convert.ToInt32(prepayid));
    //        price = prepayM.price * -1;
    //    }
    //    int hidFastPay = Convert.ToInt32(this.hidFastPay.Value);//快捷支付表ID

    //    int orderid = KeyID;//订单id
    //    if (!(orderid > 0))
    //    {
    //        //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //        return;
    //    }
    //    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));

    //    if (a1 == "1" && ((orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount) == price || (orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount) < price))
    //    {
    //        //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //        return;
    //    }

    //    string strWhere = string.Empty;
    //    if (hidFastPay > 0)
    //    {
    //        strWhere = " ID = '" + hidFastPay + "'";
    //    }
    //    else
    //    {
    //        //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //        return;
    //    }
    //    List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");

    //    if (fastList.Count > 0)
    //    {
    //        if (payPrice > 0)
    //        {
    //            int regid = 0;

    //            try
    //            {
    //                Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
    //                regModel.OrderId = Convert.ToInt32(orderid);
    //                regModel.Ordercode = payM.vdef4 + payid.ToString();
    //                regModel.number = payM.guid;
    //                regModel.Price = payM.PayPrice;
    //                regModel.Payuse = "订单支付";
    //                regModel.PayName = disModel.DisName;
    //                regModel.DisID = disModel.ID;
    //                regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    //                regModel.Remark = orderModel.Remark;
    //                regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
    //                regModel.BankID = new Hi.BLL.PAY_FastPayMent().GetModel(hidFastPay).BankID.ToString();
    //                regModel.CreateUser = user.ID;
    //                regModel.CreateDate = DateTime.Now;
    //                regModel.LogType = 1376;
    //                regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }

    //            if (regid > 0)
    //            {
    //                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
    //                PaymentEnvironment.Initialize(configPath);

    //                string institutionID = "001520";//机构号码
    //                string id = fastList[0].ID.ToString();

    //                Tx1376Request tx1376Request = new Tx1376Request();

    //                tx1376Request.setInstitutionID(institutionID);
    //                tx1376Request.setOrderNo(payM.vdef4 + payid.ToString());
    //                tx1376Request.setPaymentNo(payM.guid);
    //                tx1376Request.setSmsValidationCode(phoneCode);

    //                tx1376Request.process();

    //                TxMessenger txMessenger = new TxMessenger();
    //                String[] respMsg = txMessenger.send(tx1376Request.getRequestMessage(), tx1376Request.getRequestSignature());

    //                Tx1376Response tx1376Response = new Tx1376Response(respMsg[0], respMsg[1]);

    //                try
    //                {
    //                    Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
    //                    regModel.PlanMessage = tx1376Request.getRequestPlainText();
    //                    regModel.Start = tx1376Response.getCode();
    //                    regModel.ResultMessage = tx1376Response.getMessage();
    //                    new Hi.BLL.PAY_RegisterLog().Update(regModel);
    //                    payM = new Hi.BLL.PAY_Payment().GetModel(payid);
    //                    payM.PayDate = DateTime.Now;
    //                    payM.ts = DateTime.Now;
    //                    payM.verifystatus = tx1376Response.getVerifyStatus();
    //                    payM.status = tx1376Response.getStatus();
    //                    new Hi.BLL.PAY_Payment().Update(payM);
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }

    //                if ("2000".Equals(tx1376Response.getCode()))
    //                {
    //                    int VerifyStatus = tx1376Response.getVerifyStatus();
    //                    int Status = tx1376Response.getStatus();
    //                    if (VerifyStatus == 40)
    //                    {
    //                        if (Status == 10)
    //                        {
    //                            bool f = false;
    //                            try
    //                            {
    //                                Hi.Model.PAY_PrePayment prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
    //                                prepayModel.Start = Convert.ToInt32(Enums.PrePayMentState.处理中);
    //                                f = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                throw ex;
    //                            }
    //                            if (f)
    //                            {
    //                                //JScript.AlertMsg(this,"");
    //                                //Context.Server.Transfer("../OrderInfo.aspx?KeyID=" + orderid, true);
    //                                //JScript.ShowAlert(this, "支付失败，请重新支付！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                                ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                                this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                                return;
    //                            }
    //                            else
    //                            {
    //                                //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                                ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                                this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                                return;
    //                            }
    //                        }
    //                        if (Status == 20)
    //                        {
    //                            //支付成功,修改状态
    //                            //企业钱包修改状态
    //                            int order = 0;
    //                            int prepay = 0;
    //                            int pay = 0;
    //                            SqlConnection con = new SqlConnection(LocalSqlServer);
    //                            con.Open();
    //                            SqlTransaction sqlTrans = con.BeginTransaction();
    //                            try
    //                            {
    //                                order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, payM.PayPrice + price, sqlTrans);
    //                                pay = new Hi.BLL.PAY_Payment().updatePayState(con, payid, sqlTrans);
    //                                if (price > 0)
    //                                    prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
    //                                else
    //                                    prepay = 1;
    //                                /*
    //                                if (a1 == "0")
    //                                {
    //                                    prepay = 1;
    //                                    pay = new Hi.BLL.PAY_Payment().updatePayState(con, payid, sqlTrans);
    //                                }
    //                                if (a1 == "1")
    //                                {
    //                                    if (payPrice == price)
    //                                    {
    //                                        prepay = 1;
    //                                        pay = 1;
    //                                    }
    //                                    else if (payPrice > price)
    //                                    {
    //                                        if (price == 0)
    //                                        {
    //                                            prepay = 1;
    //                                        }
    //                                        else
    //                                        {
    //                                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con,prepayid, sqlTrans);
    //                                        }
    //                                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, payid, sqlTrans);
    //                                    }
    //                                    else
    //                                    {
    //                                        sqlTrans.Rollback();
    //                                    }
    //                                }
    //                                 * */
    //                                sqlTrans.Commit();
    //                            }
    //                            catch
    //                            {
    //                                order = 0;
    //                                prepay = 0;
    //                                pay = 0;
    //                                sqlTrans.Rollback();
    //                            }
    //                            finally
    //                            {
    //                                con.Close();
    //                            }

    //                            if (order > 0 && prepay > 0 && pay > 0)
    //                            {
    //                                //Context.Server.Transfer("PaySuccess.aspx?KeyID=" + orderid, true);
    //                                try
    //                                {
    //                                    new Common().GetWxService("2", orderid.ToString(), "1");
    //                                }
    //                                catch
    //                                {
    //                                }
    //                                finally
    //                                {
    //                                    Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "支付：" + (payM.PayPrice + price).ToString("0.00") + "元");
    //                                }
    //                                //JSon : {"type":"1","userID":"1027","orderID":"1030"}
    //                                //type : "4":订单发货通知;"3":订单状态变更通知(待发货、审批通过);"2":订单支付通知;"1":下单通知
    //                                Response.Redirect("PaySuccess.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(payid.ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey));
    //                                return;
    //                            }
    //                            else
    //                            {
    //                                //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                                ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                                this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                                return;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                            ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                            this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                            return;
    //                        }
    //                    }
    //                    else if (VerifyStatus == 20)
    //                    {
    //                        //JScript.ShowAlert(this, "验证码超时！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                        this.lblPayError.InnerText = "验证码超时！";
    //                        return;
    //                    }
    //                    else if (VerifyStatus == 30)
    //                    {
    //                        //JScript.ShowAlert(this, "验证码输入有误！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                        this.lblPayError.InnerText = "验证码输入有误！";
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                        ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);</script>");
    //                        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                    this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                    return;
    //                }
    //                //HttpContext.Current.Items["plainText"] = tx1376Response.getResponsePlainText();
    //                //context.Server.Transfer("../Distributor/Pay/Response.aspx", true);

    //            }
    //            else
    //            {
    //                //JScript.ShowAlert(this, "系统繁忙，请稍后！", "$('#txtPhoneNum').val('');$('.tip').fadeOut(100);$('.opacity').fadeOut(100);");
    //                this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                return;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnTx1311_Click(object sender, EventArgs e)
    //{
    //    if (Session["message"] != null && Session["signature"] != null)
    //    {
    //        HttpContext.Current.Items["message"] = Session["message"].ToString();
    //        HttpContext.Current.Items["signature"] = Session["signature"].ToString();
    //        Session.Remove("message");
    //        Session.Remove("signature");
    //        HttpContext.Current.Items["action"] = PaymentEnvironment.PaymentURL;
    //        Context.Server.Transfer("Request.aspx", true);
    //    }
    //    else
    //    {
    //        this.lblPayError.InnerHtml = "数据异常!";
    //    }
    //}

    /// <summary>
    /// 支付接口，网银支付
    /// </summary>
    /// <param name="a1"></param>
    //public void tx1311(string a1)
    //{
    //    string username = user.UserName;
    //    Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(user.DisID);
    //    decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
    //    int disid = user.DisID;//代理商ID

    //    string bankid = this.hidBank.Value;
    //    string payPas = this.padPaypas.Value;//企业钱包密码
    //    decimal price = Convert.ToDecimal(this.txtPrice.Value);//使用企业钱包金额
    //    decimal payPrice = 0;
    //    decimal txtPayOrder = Convert.ToDecimal(this.txtPayOrder.Value);
    //    if (this.txtPayOrder.Value != "")
    //    {
    //        if (a1 == "1")
    //            payPrice = txtPayOrder - price;
    //        else
    //            payPrice = txtPayOrder;
    //    }
    //    int orderid = Convert.ToInt32(this.hidOrderid.Value);//订单id
    //    if (orderid == 0)
    //    {
    //        JScript.AlertMsg(this, "订单为空！");
    //    }
    //    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
    //    int prepayid = 0;

    //    if (a1 == "1" && Convert.ToDecimal(price) > 0)
    //    {
    //        //企业钱包处理
    //        if (txtPayOrder < price)
    //        {
    //            //JScript.AlertMsg(this, "使用企业钱包大于订单金额！");
    //            this.lblPayError.InnerText = "使用企业钱包大于支付金额，不能支付！";
    //            return;
    //        }
    //        if (sumPrice < price)
    //        {
    //            //JScript.AlertMsg(this, "企业钱包余额不足！");
    //            this.lblPayError.InnerText = "企业钱包余额不足！";
    //            return;
    //        }
    //        else
    //        {
    //            if (disModel.Paypwd == Util.md5(payPas))
    //            {
    //                Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
    //                //string strWhere = string.Empty;
    //                //if (orderid != 0)
    //                //{
    //                //    strWhere += " OrderID = '" + orderid + "'";
    //                //}
    //                //strWhere += " and IsEnabled=1 and isnull(dr,0)=0";
    //                //List<Hi.Model.PAY_PrePayment> prepayList = new Hi.BLL.PAY_PrePayment().GetList("", strWhere, "");
    //                try
    //                {
    //                    //if (prepayList.Count > 0)
    //                    //{
    //                    //    prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(prepayList[0].ID);
    //                    //    prepayModel.price = price * -1;
    //                    //    prepayModel.ts = DateTime.Now;
    //                    //    prepayModel.modifyuser = user.ID;
    //                    //    flag = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
    //                    //    if (flag)
    //                    //    {
    //                    //        prepayid = prepayList[0].ID;
    //                    //    }
    //                    //}
    //                    //else
    //                    //{
    //                    prepayModel.CompID = disModel.CompID;
    //                    prepayModel.DisID = disModel.ID;
    //                    prepayModel.OrderID = orderid;
    //                    prepayModel.Start = 2;
    //                    prepayModel.PreType = 5;
    //                    prepayModel.price = price * -1;
    //                    prepayModel.Paytime = DateTime.Now;
    //                    prepayModel.CreatDate = DateTime.Now;
    //                    prepayModel.CrateUser = user.ID;
    //                    prepayModel.AuditState = 2;
    //                    prepayModel.IsEnabled = 1;
    //                    prepayModel.ts = DateTime.Now;
    //                    prepayModel.modifyuser = user.ID;
    //                    prepayModel.vdef1 = "订单支付";
    //                    prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
    //                    //}
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }
    //                int prepay = 0;
    //                int order = 0;
    //                if (prepayid > 0 && payPrice == 0)
    //                {

    //                    SqlConnection con = new SqlConnection(LocalSqlServer);
    //                    con.Open();
    //                    SqlTransaction sqlTrans = con.BeginTransaction();
    //                    try
    //                    {
    //                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);
    //                        order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderid, price, sqlTrans);

    //                        sqlTrans.Commit(); ;
    //                    }
    //                    catch
    //                    {
    //                        prepay = 0;
    //                        order = 0;
    //                        sqlTrans.Rollback();
    //                    }
    //                    finally
    //                    {
    //                        con.Close();
    //                    }
    //                    if (prepay > 0 && order > 0)
    //                    {
    //                        try
    //                        {
    //                            new Common().GetWxService("2", orderid.ToString(), "1");
    //                        }
    //                        catch { }
    //                        finally
    //                        {
    //                            Utils.AddSysBusinessLog(disModel.CompID, "Order", KeyID.ToString(), "订单支付", "支付：" + price + "元");
    //                        }
    //                        Context.Server.Transfer("PaySuccess.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=0&PPid=0", true);
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        //JScript.AlertMsg(this, "系统繁忙，请稍后！");
    //                        this.lblPayError.InnerText = "系统繁忙，请稍后！";
    //                        return;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                //JScript.AlertMsg(this, "密码不正确！");
    //                this.lblPayError.InnerText = "密码不正确！";
    //                return;
    //            }
    //        }
    //    }

    //    if (payPrice > 0)
    //    {
    //        if (a1 == "0")
    //        {
    //            price = 0;
    //        }

    //        int payid = 0;
    //        int regid = 0;
    //        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
    //        string strWhere2 = string.Empty;
    //        if (orderid != 0)
    //        {
    //            strWhere2 += " OrderID = " + orderid;
    //        }
    //        strWhere2 += " and isaudit = 2 and isnull(dr,0)=0";
    //        List<Hi.Model.PAY_Payment> payList = new Hi.BLL.PAY_Payment().GetList("", strWhere2, "");
    //        string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");
    //        try
    //        {
    //            string guid = Guid.NewGuid().ToString().Replace("-", "");
    //            payModel.OrderID = orderid;
    //            payModel.DisID = user.DisID;
    //            payModel.PayUser = disModel.DisName;
    //            payModel.PayPrice = payPrice;
    //            payModel.IsAudit = 2;
    //            payModel.guid = Common.Number_repeat(guid);
    //            payModel.vdef3 = "1";
    //            payModel.vdef4 = orderNo;
    //            payModel.CreateDate = DateTime.Now;
    //            payModel.CreateUserID = user.ID;
    //            payModel.ts = DateTime.Now;
    //            payModel.modifyuser = user.ID;
    //            payid = new Hi.BLL.PAY_Payment().Add(payModel);
    //            if (prepayid > 0)
    //            {
    //                Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
    //                prepayMent.vdef4 = payid.ToString();
    //                new Hi.BLL.PAY_PrePayment().Update(prepayMent);
    //            }

    //            Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
    //            regModel.OrderId = orderid;
    //            regModel.Ordercode = orderNo + payid.ToString();
    //            regModel.number = payModel.guid;
    //            regModel.Price = payPrice;
    //            regModel.Payuse = "订单支付";
    //            regModel.PayName = disModel.DisName;
    //            regModel.DisID = disid;
    //            regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    //            regModel.Remark = orderModel.Remark;
    //            regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
    //            regModel.BankID = bankid;
    //            regModel.CreateUser = user.ID;
    //            regModel.CreateDate = DateTime.Now;
    //            regModel.LogType = 1311;
    //            regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        if (payid > 0 && regid > 0)
    //        {
    //            string AccountType = Request["AccountType"];
    //            //ClientScript.RegisterStartupScript(this.GetType(), "onSubmit3", "<script>onSubmit3()</script>");
    //            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
    //            PaymentEnvironment.Initialize(configPath);

    //            String institutionID = "001520";

    //            long amount = Convert.ToInt64(payPrice * 100);
    //            long fee = 0;
    //            String payerID = disModel.ID.ToString();
    //            String payerName = disModel.DisName;
    //            String usage = "支付订单";
    //            String remark = "订单支付";

    //            String notificationURL = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/Handler/ReceiveNoticePage.ashx";
    //            String payees = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
    //            String bankID = "700";//bankid;//
    //            int accountType = Convert.ToInt32(AccountType);

    //            // 2.创建交易请求对象
    //            Tx1311Request tx1311Request = new Tx1311Request();
    //            tx1311Request.setInstitutionID(institutionID);//机构号码
    //            tx1311Request.setOrderNo(orderNo + payid.ToString());//订单号
    //            tx1311Request.setPaymentNo(payModel.guid);//支付交易流水号
    //            tx1311Request.setAmount(amount);//支付金额 单位分
    //            tx1311Request.setFee(fee);//支付服务手续费 单位分
    //            tx1311Request.setPayerID("");//付款人注册ID
    //            tx1311Request.setPayerName("");//付款方名称
    //            tx1311Request.setUsage(usage);//资金用途
    //            tx1311Request.setRemark(remark);//备注
    //            tx1311Request.setNotificationURL(notificationURL);//机构接收支付通知的URL
    //            tx1311Request.addPayee("");//收款方名称
    //            tx1311Request.setBankID(bankID);//付款银行标识
    //            tx1311Request.setAccountType(accountType);//付款方帐号类型
    //            /*
    //            if (null != payees && payees.Length > 0)
    //            {
    //                String[] payeeList = payees.Split(';');
    //                for (int i = 0; i < payeeList.Length; i++)
    //                {
    //                    tx1311Request.addPayee(payeeList[i]);//收款方名称
    //                }
    //            }
    //            */
    //            // 3.执行报文处理
    //            tx1311Request.process();
    //            try
    //            {
    //                Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
    //                regM.PlanMessage = tx1311Request.getRequestPlainText();
    //                new Hi.BLL.PAY_RegisterLog().Update(regM);
    //            }
    //            catch
    //            {

    //            }
    //            // 4.将参数放置到request对象
    //            // //3个交易参数
    //            HttpContext.Current.Items["plainText"] = tx1311Request.getRequestPlainText();
    //            HttpContext.Current.Items["message"] = tx1311Request.getRequestMessage();
    //            HttpContext.Current.Items["signature"] = tx1311Request.getRequestSignature();
    //            // //2个信息参数
    //            HttpContext.Current.Items["txCode"] = "1311";
    //            HttpContext.Current.Items["txName"] = "市场订单支付直通车";
    //            // 1个action(支付平台地址)参数
    //            HttpContext.Current.Items["action"] = PaymentEnvironment.PaymentURL;

    //            // 5.转向Request.jsp页面;
    //            Context.Server.Transfer("Request.aspx", true);
    //        }
    //        else
    //        {
    //            //JScript.AlertMsg(this, "系统繁忙，请稍后再试！");
    //            this.lblPayError.InnerText = "系统繁忙，请稍后再试！";
    //            return;
    //        }
    //    }
    //}


    protected void QuitLogin_Click(object sender, EventArgs e)
    {
        if (Context.Request.Cookies["loginmodel"] != null)
        {
            HttpCookie cookie = new HttpCookie("loginmodel");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["token"] != null)
        {
            HttpCookie cookie = new HttpCookie("token");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_type"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_type");
            cookie.Expires = DateTime.Now.AddDays(-1); cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_state"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_state");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        Session.Remove("UserModel");
        Response.Redirect("login.aspx");
    }
    protected void rptQpay_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
}