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
using Com.Alipay;
using WxPayAPI;
using System.Configuration;
using System.Data;
using System.Text;

public partial class Distributor_Pay_Pay : DisPageBase
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


    Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
    public Hi.Model.DIS_Order orderModel = new Hi.Model.DIS_Order();
    public List<Hi.Model.PAY_Financing> PList = new List<Hi.Model.PAY_Financing>();
    public string GoodsName = "账单支付";
    public int Cmpid = 0;
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

        Cmpid = this.CompID;


        if (!Common.PageDisOperable("Order", KeyID, this.DisID))
        {
            Response.Redirect("../../NoOperable.aspx", true);
            return;
        }
        if (!IsPostBack)
        {

            Bind();//绑定
            BindPaySettings(Cmpid); //给页面隐藏域赋值

        }
    }

    public void Bind()
    {
        orderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        if (!(
           (
           (orderModel.Otype == (int)Enums.OType.销售订单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付))
           ||
           (orderModel.Otype == (int)Enums.OType.推送账单 && orderModel.OState == (int)Enums.OrderState.已审 && (orderModel.PayState == (int)Enums.PayState.部分支付 || orderModel.PayState == (int)Enums.PayState.未支付))
           )
           && orderModel.OState != (int)Enums.OrderState.已作废))
        {
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                JScript.AlertMethod(this, "账单异常，不能支付！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderDzfzdList.aspx") + "'); }");
            else
                JScript.AlertMethod(this, "订单异常，不能支付！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderPayList.aspx") + "'); }");
            return;
        }
        if (orderModel == null)
        {
            if (orderModel.Otype == (int)Enums.OType.推送账单)
                JScript.AlertMethod(this, "无效的账单！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderDzfzdList.aspx") + "'); }");
            else
                JScript.AlertMethod(this, "无效的订单！", JScript.IconOption.错误, "function (){ location.replace('" + ("orderPayList.aspx") + "'); }");
            //this.lblPayError.InnerText = "无效的订单！";
            return;
        }

        if (orderModel.OState < 2)
        {
            JScript.AlertMethod(this, "该订单未审核,不能支付!", JScript.IconOption.错误, "function (){ location.replace('" + ("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)) + "'); }");
            return;
        }

        if (orderModel.PayState != (int)Enums.PayState.未支付 && orderModel.PayState != (int)Enums.PayState.部分支付)
        {
            Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
            return;
        }
        List<Hi.Model.PAY_OpenAccount> LOpen = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + this.DisID + " and State=1 and isnull(dr,0)=0", "");
        List<Hi.Model.PAY_Withdrawals> Lwith = new Hi.BLL.PAY_Withdrawals().GetList("", "DisID=" + this.DisID + " and state=1 and isnull(dr,0)=0", "");
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

        PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and State=3 and isnull(dr,0)=0", "");
        if (PList.Count > 0)
        {
            this.lblBalance5.InnerHtml = PList[0].AclAmt.ToString("0.00");
        }

        decimal payPrice = orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount;
        this.lblOrderNO.InnerText = orderModel.ReceiptNo.Trim().ToString();
        //账单支付链接
        if (orderModel.Otype == 9)
            this.lblOrderNO.HRef = "../OrderZDInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        else
            this.lblOrderNO.HRef = "../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        this.hidOrderid.Value = KeyID.ToString();
        this.lblPricePay.InnerText = payPrice.ToString("0.00");
        this.hidPricePay.Value = payPrice.ToString("0.00");
        this.lblPriceO.InnerText = (orderModel.AuditAmount + orderModel.OtherAmount).ToString("0.00");
        if (this.txtPayOrder.Value == "")
            this.txtPayOrder.Value = payPrice.ToString("0.00");
        else
            this.txtPayOrder.Value = Convert.ToDecimal(this.txtPayOrder.Value).ToString("0.00");
        this.hidUserName.Value = this.UserName;

        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(orderModel.DisID, orderModel.CompID);
        this.lblSumPrice.InnerText = sumPrice.ToString("0.00");
        this.hidSumPrice.Value = sumPrice.ToString("0.00");

        string strWhere = " 1=1 ";
        if (this.DisID != 0)
        {
            strWhere += " and DisID = " + this.DisID;
        }
        strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        this.rptQpay.DataSource = fastList;
        this.rptQpay.DataBind();
        List<Hi.Model.PAY_BankInfo> BankL = new Hi.BLL.PAY_BankInfo().GetList("", " vdef1=0", "");
        this.rptOtherBank.DataSource = BankL;
        this.rptOtherBank.DataBind();
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
                this.pay_b2cwyzfstart.Value = Sysl[0].vdef1;


                this.pay_b2bwyzf.Value = Convert.ToString(Sysl[0].pay_b2bwyzf);

                this.hid_PayType.Value = "11";
            }
            else
            {
                div_kjzfsxf.Attributes.Add("style", "display:none;");
                div_wyzfsxf.Attributes.Add("style", "display:none;");
                sum_payprice.Attributes.Add("style", "display:none;");
            }





        }
        else
        {
            div_kjzfsxf.Attributes.Add("style", "display:none;");
            div_wyzfsxf.Attributes.Add("style", "display:none;");
            sum_payprice.Attributes.Add("style", "display:none;");
        }

    }

    protected void btnSuccess_Click(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        if (PList != null && PList.Count > 0)
        {
            Response.Redirect("../../Financing/FinancingDetailList.aspx");
        }
        else
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
    }

    protected void btnFailure_Click(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        if (PList != null && PList.Count > 0)
        {
            Response.Redirect("../../Financing/FinancingDetailList.aspx");
        }
        else
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
    }

    protected void btnPaySuccess_Click(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
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
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
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
                this.
                   Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderZDList");
            }
        }
    }

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
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
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


    /// <summary>
    /// 支付宝支付
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApliay_Click(object sender, EventArgs e)
    {
        StringBuilder sbHtml = new StringBuilder();
        sbHtml.Append("<form id='alifrom' name='alisubmit' action='AliPay.aspx'  method='post'>");
        sbHtml.Append("<input type='hidden' name='hidPayprice' value='" + txtPayOrder.Value.Trim() + "'/>");
        sbHtml.Append("<input type='hidden' name='hidOid' value='" + KeyID.ToString() + "'/>");
        //submit按钮控件请不要含有name属性
        sbHtml.Append("<input type='submit' value='确认' style='display:none;'></form>");
        sbHtml.Append("<script>document.forms['alifrom'].submit();</script>");
        Response.Write(sbHtml.ToString());
        Response.End();
    }



    /// <summary>
    /// 微信支付
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnWxPay_Click(object sender, EventArgs e)
    {
        StringBuilder sbHtml = new StringBuilder();
        sbHtml.Append("<form id='wxfrom' name='wxsubmit' action='WeChatPay.aspx'  method='post'>");
        sbHtml.Append("<input type='hidden' name='hidPayprice' value='" + txtPayOrder.Value.Trim() + "'/>");
        sbHtml.Append("<input type='hidden' name='hidOid' value='" + KeyID.ToString() + "'/>");
        //submit按钮控件请不要含有name属性
        sbHtml.Append("<input type='submit' value='确认' style='display:none;'></form>");
        sbHtml.Append("<script>document.forms['wxfrom'].submit();</script>");
        Response.Write(sbHtml.ToString());
        Response.End();
    }
    public string tx1401(string paytype)
    {
        return "";
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

