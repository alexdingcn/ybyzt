

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

public partial class Distributor_Pay_Pay :CompPageBase
{
    public int DisID = 0;
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
    public List<Hi.Model.PAY_Financing> PList = new List<Hi.Model.PAY_Financing>();
    public string GoodsName = string.Empty;
    public int KeyID = 0;
    public int Cmpid = 0;
    public decimal Price = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        

        if (Request["Orderid"] == "")
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Common.DesDecrypt(Request["Orderid"], Common.EncryptKey).ToInt(0);
        }

        Cmpid = this.CompID;


        //if (!Common.PageDisOperable("Order", KeyID, this.DisID))
        //{
        //    Response.Redirect("../../NoOperable.aspx", true);
        //    return;
        //}
        if (!IsPostBack)
        {

            Bind();//绑定
            BindPaySettings(Cmpid); //给页面隐藏域赋值

        }
    }

    public void Bind()
    {

        Hi.Model.Pay_Service service = new Hi.BLL.Pay_Service().GetModel(KeyID);
        Price = service.Price;
        //orderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        //decimal payPrice = orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount;
        //this.lblOrderNO.InnerText = orderModel.ReceiptNo.Trim().ToString();
        ////账单支付链接
        //if (orderModel.Otype == 9)
        //    this.lblOrderNO.HRef = "../OrderZDInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        //else
        //this.lblOrderNO.HRef = "../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        string outdata;//服务起始日期
                       //List<Hi.Model.Pay_Service> serviceord = new Hi.BLL.Pay_Service().GetList("*", " compid=" + this.CompID + " and isaudit=1 and outofdata=0 ", " createdate desc");
                       //if (serviceord.Count > 0)
                       //{
                       //    outdata = serviceord[0].OutData.ToString("yyyy-MM-dd");//存在有效服务  获取购买服务起始日期
                       //}
                       //else
                       //{
                       //    string CreateDate = string.Empty;
                       //    //获取购买服务的起始日期
                       //    Common.GetCompService(CompID.ToString(),out outdata);
                       //    if (outdata=="0")
                       //    {
                       //        outdata = DateTime.Now.ToString("yyyy-MM-dd");
                       //    }

        //}
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
        if (comp.EnabledEndDate.ToString() == "0001/1/1 0:00:00")
        {
            //没有任何服务记录
            if (service.ServiceType == 1)
                outdata = Convert.ToDateTime(service.OutData).AddYears(-1).ToString();
            else
                outdata = Convert.ToDateTime(service.OutData).AddMonths(-1).ToString();
        }
        else if (comp.EnabledEndDate < DateTime.Now.AddDays(1))
        {
            //服务已过期
            if (service.ServiceType == 1)
                outdata = Convert.ToDateTime(service.OutData).AddYears(-1).ToString();
            else
                outdata = Convert.ToDateTime(service.OutData).AddMonths(-1).ToString();
        }
        else
        {
            //服务日期有效
            if (service.ServiceType == 1)
                outdata = Convert.ToDateTime(service.OutData).AddYears(-1).AddDays(1).ToString();
            else
                outdata = Convert.ToDateTime(service.OutData).AddMonths(-1).AddDays(1).ToString();
        }
        

        Data.InnerHtml =Convert.ToDateTime(outdata).ToString("yyyy-MM-dd");
        DataEnd.InnerHtml= service.OutData.ToString("yyyy-MM-dd");
        this.hidOrderid.Value = KeyID.ToString();
        //this.lblPricePay.InnerText = service.Price.ToString("0.00");
        this.hidPricePay.Value = service.Price.ToString("0.00");
        //this.lblPriceO.InnerText = service.Price.ToString("0.00");
        if (this.txtPayOrder.Value == "")
            this.txtPayOrder.Value = service.Price.ToString("0.00");
        else
            this.txtPayOrder.Value = Convert.ToDecimal(this.txtPayOrder.Value).ToString("0.00");
        this.hidUserName.Value = this.UserName;

        //decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(orderModel.DisID, orderModel.CompID);
        //this.lblSumPrice.InnerText = sumPrice.ToString("0.00");
        this.hidSumPrice.Value = service.Price.ToString("0.00");

        //string strWhere = " 1=1 ";
        //if (this.DisID != 0)
        //{
        //    strWhere += " and DisID = " + this.DisID;
        //}
        //strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        //List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        //this.rptQpay.DataSource = fastList;
        //this.rptQpay.DataBind();
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
                //div_kjzfsxf.Attributes.Add("style", "display:none;");
                div_wyzfsxf.Attributes.Add("style", "display:none;");
                sum_payprice.Attributes.Add("style", "display:none;");
            }





        }
        else
        {
            //div_kjzfsxf.Attributes.Add("style", "display:none;");
            div_wyzfsxf.Attributes.Add("style", "display:none;");
            sum_payprice.Attributes.Add("style", "display:none;");
        }

    }



    protected void btnSuccess_Click(object sender, EventArgs e)
    {
        //List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        //if (PList != null && PList.Count > 0)
        //{
        //    Response.Redirect("../../Financing/FinancingDetailList.aspx");
        //}
        //else
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("orderPayList.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("orderDzfzdList.aspx");
        //    }
        //}
    }

    protected void btnFailure_Click(object sender, EventArgs e)
    {
        //List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        //if (PList != null && PList.Count > 0)
        //{
        //    Response.Redirect("../../Financing/FinancingDetailList.aspx");
        //}
        //else
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("orderPayList.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("orderDzfzdList.aspx");
        //    }
        //}
    }

    protected void btnPaySuccess_Click(object sender, EventArgs e)
    {
        //List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        //if (PList != null && PList.Count > 0)
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("orderPayList.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("orderDzfzdList.aspx");
        //    }
        //}
        //else
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderpaylist");
        //    }
        //    else
        //    {
        //        Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderZDList");
        //    }
        //}
    }

    protected void btnPayFailure_Click(object sender, EventArgs e)
    {
        //List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + this.DisID + " and OrderID=" + KeyID + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        //if (PList != null && PList.Count > 0)
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("orderPayList.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("orderDzfzdList.aspx");
        //    }
        //}
        //else
        //{
        //    if (orderModel.Otype != 9)
        //    {
        //        Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderpaylist");
        //    }
        //    else
        //    {
        //        this.
        //           Response.Redirect("../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type1=orderZDList");
        //    }
        //}
    }







    protected void QuitLogin_Click(object sender, EventArgs e)
    {
        //if (Context.Request.Cookies["loginmodel"] != null)
        //{
        //    HttpCookie cookie = new HttpCookie("loginmodel");
        //    cookie.Expires = DateTime.Now.AddDays(-1);
        //    cookie.HttpOnly = true;
        //    Response.Cookies.Add(cookie);
        //}
        //if (Context.Request.Cookies["token"] != null)
        //{
        //    HttpCookie cookie = new HttpCookie("token");
        //    cookie.Expires = DateTime.Now.AddDays(-1);
        //    cookie.HttpOnly = true;
        //    Response.Cookies.Add(cookie);
        //}
        //if (Context.Request.Cookies["login_type"] != null)
        //{
        //    HttpCookie cookie = new HttpCookie("login_type");
        //    cookie.Expires = DateTime.Now.AddDays(-1);
        //    cookie.HttpOnly = true;
        //    Response.Cookies.Add(cookie);
        //}
        //if (Context.Request.Cookies["login_state"] != null)
        //{
        //    HttpCookie cookie = new HttpCookie("login_state");
        //    cookie.Expires = DateTime.Now.AddDays(-1);
        //    cookie.HttpOnly = true;
        //    Response.Cookies.Add(cookie);
        //}
        //Session.Remove("UserModel");
        //Response.Redirect("login.aspx");
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
        //Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
        //Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(OrderModel.DisID);

        ////使用企业钱包
        //int prepayid = 0;

        //decimal txtPayOrder = Convert.ToDecimal(this.txtPayOrder.Value);//本次支付总金额
        //decimal price = Convert.ToDecimal(this.txtPrice.Value);//使用企业钱包金额
        //int yfk = Convert.ToInt32(this.hida1.Value);//是否使用企业钱包  1:是  0:否
        //string payPas = Common.NoHTML(Convert.ToString(padPaypas.Value));//企业钱包密码
        //#region 企业钱包支付 begin

        //if (disModel == null)
        //{
        //    ErrMessage("数据异常,代理商有误");
        //    return;
        //}

        //decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
        //int disid = this.DisID;//代理商ID
        //string password = disModel.Paypwd;

        //if (yfk == 1)
        //{
        //    if (payPas == null || payPas.Trim().ToString() == "")
        //    {

        //        ErrMessage("企业钱包密码不能为空");
        //        return;
        //    }
        //    payPas = payPas.Trim().ToString();
        //    if (password == Util.md5("123456"))
        //    {
        //        ErrMessage("请先修改企业钱包支付密码");
        //        return;
        //    }
        //    else
        //    {
        //        if (payPas == "")
        //        {
        //            ErrMessage("密码不能为空");

        //            return;
        //        }
        //    }
        //}

        //decimal payPrice = 0;//支付金额
        //if (txtPayOrder == 0)
        //{
        //    ErrMessage("支付金额不能为0");

        //    return;
        //}
        //if (yfk == 1 && txtPayOrder < price)
        //{
        //    ErrMessage("使用企业钱包大于支付金额!");
        //    return;
        //}
        //if (yfk == 1)
        //    payPrice = txtPayOrder - price;
        //else
        //    payPrice = txtPayOrder;


        //if (txtPayOrder > OrderModel.AuditAmount + OrderModel.OtherAmount - OrderModel.PayedAmount)
        //{
        //    ErrMessage("支付金额大于未支付金额，不能支付！");
        //    return;
        //}
        //if (!((
        //    (OrderModel.Otype == (int)Enums.OType.赊销订单 && (OrderModel.OState != (int)Enums.OrderState.退回 && OrderModel.OState != (int)Enums.OrderState.未提交 && OrderModel.OState != (int)Enums.OrderState.待审核) && (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)) ||
        //    (OrderModel.Otype != (int)Enums.OType.赊销订单 && OrderModel.Otype != (int)Enums.OType.推送账单 && (OrderModel.OState == (int)Enums.OrderState.已审 || OrderModel.OState == (int)Enums.OrderState.已发货 || OrderModel.OState == (int)Enums.OrderState.已到货) && (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)) ||
        //    (OrderModel.Otype == (int)Enums.OType.推送账单 && OrderModel.OState == (int)Enums.OrderState.已审 && (OrderModel.PayState == (int)Enums.PayState.部分支付 || OrderModel.PayState == (int)Enums.PayState.未支付))
        //    )
        //    && OrderModel.OState != (int)Enums.OrderState.已作废))
        //{
        //    if (OrderModel.Otype == (int)Enums.OType.推送账单)
        //    {
        //        ErrMessage("账单异常，不能支付");
        //    }
        //    else
        //    {
        //        ErrMessage("订单异常，不能支付");
        //    }
        //    return;
        //}

        //if (yfk == 1 && price > 0)
        //{
        //    //企业钱包处理
        //    if (sumPrice < price)
        //    {
        //        ErrMessage("企业钱包余额不足");

        //        return;
        //    }
        //    if (disModel.Paypwd != Util.md5(payPas))
        //    {
        //        ErrMessage("支付密码不正确");

        //        return;
        //    }
        //    Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
        //    prepayModel.CompID = disModel.CompID;
        //    prepayModel.DisID = disModel.ID;
        //    prepayModel.OrderID = OrderModel.ID;
        //    prepayModel.Start = 2;
        //    prepayModel.PreType = 5;
        //    prepayModel.price = price * -1;
        //    prepayModel.Paytime = DateTime.Now;
        //    prepayModel.CreatDate = DateTime.Now;
        //    prepayModel.CrateUser = this.UserID;
        //    prepayModel.AuditState = 2;
        //    prepayModel.IsEnabled = 1;
        //    prepayModel.ts = DateTime.Now;
        //    prepayModel.modifyuser = this.UserID;
        //    prepayModel.guid = Common.Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));
        //    // prepayModel.vdef1 = "订单支付";
        //    prepayid = new Hi.BLL.PAY_PrePayment().Add(prepayModel);
        //    int prepay = 0;
        //    int order = 0;
        //    if (prepayid > 0 && payPrice == 0)//payPrice（网银支付金额）= 0 ，只用企业钱包支付，修改状态
        //    {

        //        SqlConnection con = new SqlConnection(LocalSqlServer);
        //        con.Open();
        //        SqlTransaction sqlTrans = con.BeginTransaction();
        //        try
        //        {
        //            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayid, sqlTrans);//修改企业钱包状态
        //            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, OrderModel.ID, price, sqlTrans);//修改订单状态
        //            if (prepay > 0 && order > 0)
        //                sqlTrans.Commit();
        //            else
        //                sqlTrans.Rollback();
        //        }
        //        catch
        //        {
        //            prepay = 0;
        //            order = 0;
        //            sqlTrans.Rollback();
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //        if (prepay > 0 && order > 0)
        //        {
        //            try
        //            {
        //                if (OrderModel.Otype == (int)Enums.OType.推送账单)
        //                    Utils.AddSysBusinessLog(disModel.CompID, "Order", OrderModel.ID.ToString(), "账单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", this.UserID.ToString());
        //                else
        //                    Utils.AddSysBusinessLog(disModel.CompID, "Order", OrderModel.ID.ToString(), "订单支付", "支付：" + price + "元(企业钱包支付)", this.UserID.ToString());
        //                if (OrderModel.Otype != 9)
        //                {
        //                    OrderInfoType.AddIntegral(this.CompID, this.DisID, "1", 1, OrderModel.ID, price, "订单支付", "", this.UserID);
        //                }
        //                new Common().GetWxService("2", OrderModel.ID.ToString(), "1", price);


        //            }
        //            catch (Exception ex)
        //            {
        //                ErrMessage("支付失败");
        //                return;
        //            }

        //            if (OrderModel.Otype == (int)Enums.OType.推送账单)
        //            {

        //                Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(OrderModel.ID.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
        //            }
        //            else
        //            {

        //                Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(OrderModel.ID.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
        //            }

        //        }
        //    }

        //}

        //#endregion




        //#region  插入支付表记录

        //Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        //string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

        //string guid = Guid.NewGuid().ToString().Replace("-", "");
        //payModel.OrderID = OrderModel.ID;// orderid;
        //payModel.DisID = this.DisID;
        //payModel.Type = 0;// isDBPay;
        //payModel.PayUser = disModel.DisName;
        //payModel.PayPrice = payPrice;
        //payModel.IsAudit = 2;
        //payModel.guid = Common.Number_repeat(guid);
        //payModel.vdef3 = "1"; //1，订单支付，2，预付款充值、汇款
        //payModel.vdef4 = orderNo;
        //payModel.CreateDate = DateTime.Now;
        //payModel.CreateUserID = this.UserID;
        //payModel.ts = DateTime.Now;
        //payModel.modifyuser = this.UserID;
        //payModel.PrintNum = 1;//支付宝支付无需结算
        ////判断账户类型，判断支付渠道
        //payModel.Channel = "6";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付
        //payModel.State = 0;//手续费收取方
        //payModel.vdef5 = "0.00";//支付手续费
        //int payid = new Hi.BLL.PAY_Payment().Add(payModel);

        //if (prepayid > 0)
        //{
        //    Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
        //    prepayMent.vdef4 = payid.ToString();//与企业钱包关联
        //    new Hi.BLL.PAY_PrePayment().Update(prepayMent);
        //}

        //Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        //regModel.OrderId = OrderModel.ID;// orderid;
        //regModel.Ordercode = orderNo + payid.ToString();
        //regModel.number = payModel.guid;
        //regModel.Price = payPrice;
        //regModel.Payuse = "订单支付";
        //regModel.PayName = disModel.DisName;
        //regModel.DisID = OrderModel.DisID;
        //regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        //regModel.Remark = OrderModel.Remark;// orderModel.Remark;
        //regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
        //regModel.BankID = "支付宝支付";
        //regModel.CreateUser = this.UserID;
        //regModel.CreateDate = DateTime.Now;
        //regModel.LogType = 1311;
        //int regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
        //if (payid <= 0 || regid <= 0)
        //{
        //    Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败！", Common.EncryptKey), false);

        //}

        ////订单所有商品明细


        //DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + KeyID);
        //foreach (DataRow dr in l.Rows)
        //{
        //    GoodsName += dr["GoodsName"] + ",";
        //}
        //GoodsName = GoodsName.Substring(0, GoodsName.Length - 1);//去掉最后一个逗号
        //if (GoodsName.Length > 15)
        //    GoodsName = GoodsName.Substring(0, 10) + "...";
        //#endregion




        //#region  支付宝支付
        ////支付类型
        //string payment_type = "1";
        ////必填，不能修改
        ////服务器异步通知页面路径 
        //string notify_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Distributor/Pay/ReceiveNoticeAlipay.aspx";
        ////需http://格式的完整路径，不能加?id=123这类自定义参数


        //string return_url = string.Empty;

        ////页面跳转同步通知页面路径
        //return_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Distributor/OrderList.aspx";//"/Distributor/newOrder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);

        ////正式库地址
        //if (WebConfigurationManager.AppSettings["PayType"] == "1")
        //    return_url = "http://www.my1818.com/Distributor/newOrder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);

        ////卖家支付宝帐户
        ////string seller_email = "sczfb@lansiu.com";
        //string seller_email = Common.GetPayWxandAli(this.CompID).ali_seller_email;// ConfigurationManager.AppSettings["seller_email"] == null ? "shuzj@haiyusoft.com" : ConfigurationManager.AppSettings["seller_email"].ToString().Trim();

        ////必填
        ////商户订单号
        //string out_trade_no = payModel.guid;// OrderModel.GUID;
        ////商户网站订单系统中唯一订单号，必填



        ////订单名称
        //string subject = GoodsName;//OrderModel.GUID;
        ////必填
        ////付款金额
        ////string total_fee = Math.Round((OrderInfoModel.NeedPayAmount - OrderInfoModel.UseBalance), 2).ToString();
        //string total_fee = payPrice.ToString();

        //string paymethod = string.Empty;  //默认支付方式
        //string defaultbank = string.Empty;  //默认网银

        ////if (OrderInfoModel.BankId != "0" && OrderInfoModel.BankId != "")
        ////{
        ////    //默认支付方式
        ////    paymethod = "bankPay";
        ////    //必填
        ////    //默认网银
        ////    defaultbank = OrderInfoModel.BankId;
        ////}

        ////必填
        ////订单描述
        //string body = OrderModel.Remark;
        ////商品展示地址
        //string show_url = "http://www.my1818.com";
        ////需以http://开头的完整路径，

        ////防钓鱼时间戳
        //string anti_phishing_key = Submit.Query_timestamp();//"";
        ////若要使用请调用类文件submit中的query_timestamp函数

        ////客户端的IP地址
        //string exter_invoke_ip = Page.Request.UserHostAddress;
        ////非局域网的外网IP地址，如：221.0.0.1

        ////把请求参数打包成数组
        //SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
        //sParaTemp.Add("partner", Config.Partner);
        //sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
        //sParaTemp.Add("service", "create_direct_pay_by_user");
        //sParaTemp.Add("payment_type", payment_type);
        //sParaTemp.Add("notify_url", notify_url);
        //sParaTemp.Add("return_url", return_url);
        //sParaTemp.Add("seller_email", seller_email);
        //sParaTemp.Add("out_trade_no", out_trade_no);
        //sParaTemp.Add("subject", subject);
        //sParaTemp.Add("total_fee", total_fee);
        //sParaTemp.Add("body", body);

        ////if (OrderInfoModel.BankId != "0" && OrderInfoModel.BankId != "")
        ////{
        ////    sParaTemp.Add("paymethod", paymethod);
        ////    sParaTemp.Add("defaultbank", defaultbank);
        ////}

        //sParaTemp.Add("show_url", show_url);
        //sParaTemp.Add("anti_phishing_key", anti_phishing_key);
        //sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

        ////建立请求
        //string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
        //Response.Write(sHtmlText);
        //#endregion

    }

    /// <summary>
    ///支付宝支付 错误处理页面
    /// </summary>
    public void ErrMessage(string msg)
    {
        //Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt(msg, Common.EncryptKey), false);

    }


    /// <summary>
    /// 微信支付
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnWxPay_Click(object sender, EventArgs e)
    {
        StringBuilder sbHtml = new StringBuilder();

        //target='_blank'
        sbHtml.Append("<form id='wxfrom' name='alipaysubmit' action='WeChatPay.aspx'  method='post'>");
        sbHtml.Append("<input type='hidden' name='hidwxPayOrder' value='" + txtPayOrder.Value.Trim() + "'/>");
        sbHtml.Append("<input type='hidden' name='hidwxPrice' value='" + Price + "'/>");
        sbHtml.Append("<input type='hidden' name='hidwxisno' value='" + 0 + "'/>");
        sbHtml.Append("<input type='hidden' name='hidwxpaypas' value='" + padPaypas.Value.Trim() + "'/>");
        sbHtml.Append("<input type='hidden' name='hidwxOid' value='" + KeyID.ToString() + "'/>");

        //submit按钮控件请不要含有name属性
        sbHtml.Append("<input type='submit' value='确认' style='display:none;'></form>");

        sbHtml.Append("<script>document.forms['wxfrom'].submit();</script>");

        Response.Write(sbHtml.ToString());
    }

}

