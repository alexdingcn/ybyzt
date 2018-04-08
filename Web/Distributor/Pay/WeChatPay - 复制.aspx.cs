using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using DBUtility;
using System.Web.Script.Serialization;
using LitJson;
public partial class Distributor_Pay_WeChatPay : DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public Hi.Model.DIS_Order OrderModel = null;
    public Hi.Model.BD_Distributor disModel = null;
    public decimal txtPayOrder = 0;
    public int orderid = 0;
    public string GoodsName = string.Empty;

    public int payid = 0;
    public int prepayid = 0;

    public int rechengID = 0;//企业钱包充值preID
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string action = Request["action"] + "";
            if (action.Equals("sett"))
            {
                //清空隐藏域值

                this.hidguid.Value ="";
                this.hidprice.Value = "";
                this.hidordid.Value = "";
                this.hidpid.Value = "";
                this.hidppid.Value = "";


                string json = Request["hidjson"];
                JsonData Params = JsonMapper.ToObject(Common.DesDecrypt(json, Common.EncryptKey));
                string url = string.Empty;
                string result_url = string.Empty;
                Wxpay_Url wxpayurlmodel =null;
                if (Params["Preorord"].ToString().Equals("ord"))
                {
                    orderid = Convert.ToInt32(Params["OrderID"].ToString());
                    txtPayOrder = Convert.ToDecimal(Params["TxtPayOrder"].ToString());
                    decimal price = Convert.ToDecimal(Params["Price"].ToString());
                    int yfk = Convert.ToInt32(Params["Yfk"].ToString());
                    string payPas = Common.NoHTML(Convert.ToString(Params["PayPas"].ToString()));

                    wxpayurlmodel = PayOrder(orderid, txtPayOrder, price, yfk, payPas);

                    //url = GetPayUrl(wxpayurlmodel.Guid, wxpayurlmodel.OrderID, wxpayurlmodel.Amount);
                    //result_url = "WxPay.aspx?data=" + HttpUtility.UrlEncode(url);

                }
                else if (Params["Preorord"].ToString().Equals("pre"))
                {
                    rechengID = Convert.ToInt32(Params["OrderID"].ToString());
                    wxpayurlmodel = PayRechang(rechengID);
                }
                
                url = GetPayUrl(wxpayurlmodel.Guid, wxpayurlmodel.OrderID, wxpayurlmodel.Amount);
                result_url = "WxPay.aspx?data=" + HttpUtility.UrlEncode(url);

                string result = "{\"rel\":\"ok\",\"url\":\"" + result_url + "\",\"Hidguid\":\"" + wxpayurlmodel.Hidguid + "\",\"Hidprice\":\"" + wxpayurlmodel.Hidprice + "\",\"Hidordid\":\"" + wxpayurlmodel.Hidordid + "\",\"Hidpid\":\"" + wxpayurlmodel.Hidpid + "\",\"Hidppid\":\"" + wxpayurlmodel.Hidppid + "\"}";
                Response.Write(result);
                Response.End();
            }

            //企业钱包充值表ID，用来判断充值 OR 订单支付
            if (Request["pre"] != "" && Request["pre"] != null)
            {
                rechengID = Convert.ToInt32(Common.DesDecrypt(Request["pre"], Common.EncryptKey));

                #region 序列号数据
                Wxpay wxpaymodel = new Wxpay();
                wxpaymodel.OrderID = rechengID;//充值表Id
                wxpaymodel.TxtPayOrder = 0;
                wxpaymodel.Price = 0;
                wxpaymodel.Yfk = -1;
                wxpaymodel.PayPas = "";
                wxpaymodel.Preorord = "pre";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string Json = Common.DesEncrypt(js.Serialize(wxpaymodel), Common.EncryptKey);
                this.hidjson.Value = Json;
                #endregion
            }
            //企业钱包充值
            if (rechengID > 0)
            {
                Wxpay_Url wxpayurlmodel = PayRechang(rechengID);

                btnWxPay_Click(wxpayurlmodel.Guid, wxpayurlmodel.OrderID, wxpayurlmodel.Amount);

            }
            else //订单支付
            {
                orderid =Convert.ToInt32(Request.Form["hidwxOid"]);

                txtPayOrder = Convert.ToDecimal(Request.Form["hidwxPayOrder"]);//本次支付总金额

                decimal price = Convert.ToDecimal(Request.Form["hidwxPrice"]);//使用企业钱包金额

                int yfk = Convert.ToInt32(Request.Form["hidwxisno"]);//是否使用企业钱包  1:是  0:否     

                string payPas = Common.NoHTML(Convert.ToString(Request.Form["hidwxpaypas"]));//企业钱包密码


                Wxpay wxpaymodel = new Wxpay();
                wxpaymodel.OrderID = orderid;
                wxpaymodel.TxtPayOrder = txtPayOrder;
                wxpaymodel.Price = price;
                wxpaymodel.Yfk = yfk;
                wxpaymodel.PayPas = payPas;
                wxpaymodel.Preorord = "ord";
                JavaScriptSerializer js = new JavaScriptSerializer();
                string Json = Common.DesEncrypt(js.Serialize(wxpaymodel), Common.EncryptKey);
                this.hidjson.Value = Json;

                //调用订单支付，生产支付记录
                Wxpay_Url wxpayurlmodel = PayOrder(orderid, txtPayOrder, price, yfk, payPas);
                //调用二维码生产方法
                btnWxPay_Click(wxpayurlmodel.Guid, wxpayurlmodel.OrderID, wxpayurlmodel.Amount);
            }
        }

    }


    /// <summary>
    /// 微信支付
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnWxPay_Click(string guid, string ordid, int amount)
    {
        try
        {
            string url = GetPayUrl(guid, ordid, amount);
            Image.ImageUrl = "WxPay.aspx?data=" + HttpUtility.UrlEncode(url);
        }
        catch
        {

            ErrMessage("商户账户有误，请联系管理员", ordid);

        }
    }

    /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
    public string GetPayUrl(string guid, string ordid, int amount)
    {
        // Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

        WxPayData data = new WxPayData();
        data.SetValue("body", GoodsName);//商品描述
        data.SetValue("attach", rechengID > 0 ? "钱包充值" : "订单支付");//附加数据
        data.SetValue("out_trade_no", guid);//随机字符串
        data.SetValue("total_fee", amount);//总金额
        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
        data.SetValue("goods_tag", "jjj");//商品标记
        data.SetValue("trade_type", "NATIVE");//交易类型
        data.SetValue("product_id", ordid);//商品ID

        WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
        string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

        // Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
        return url;
    }

    /// <summary>
    /// 错误处理页面
    /// </summary>
    /// <param name="msg">提示消息</param>
    public void ErrMessage(string msg,string orderid)
    {
        Response.Redirect("Error.aspx?type=" + Common.DesEncrypt(rechengID > 0 ? "2" : "3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid, Common.EncryptKey) + "&msg=" + Common.DesEncrypt(msg, Common.EncryptKey), false);
    }

    /// <summary>
    /// 订单支付
    /// </summary>
    /// <param name="orderid">订单ID</param>
    /// <param name="txtPayOrder">支付金额</param>
    /// <param name="price">企业钱包金额</param>
    /// <param name="yfk">预付款</param>
    /// <param name="payPas"></param>
    public Wxpay_Url PayOrder(int orderid, decimal txtPayOrder, decimal price, int yfk, string payPas)
    {

        //使用企业钱包

        OrderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
        disModel = new Hi.BLL.BD_Distributor().GetModel(OrderModel.DisID);



        #region  界面上显示
        this.lblOrderNO.InnerText = OrderModel.ReceiptNo.Trim().ToString();
        this.fee.InnerText = this.CompName;//收款方
        // this.lblOrderNO.HRef = "../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey);
        #endregion 界面上显示


        #region 企业钱包支付 begin

        if (disModel == null)
        {
            ErrMessage("数据异常,代理商有误", orderid.ToString());
        }

        decimal sumPrice = new Hi.BLL.PAY_PrePayment().sums(disModel.ID, disModel.CompID);//剩余企业钱包
        int disid = OrderModel.DisID;//代理商ID
        string password = disModel.Paypwd;

        if (yfk == 1)
        {
            if (payPas == null || payPas.Trim().ToString() == "")
            {

                ErrMessage("企业钱包密码不能为空", orderid.ToString());
            }
            payPas = payPas.Trim().ToString();
            if (password == Util.md5("123456"))
            {
                ErrMessage("请先修改企业钱包支付密码", orderid.ToString());
            }
            else
            {
                if (payPas == "")
                {
                    ErrMessage("密码不能为空", orderid.ToString());
                }
            }
        }

        decimal payPrice = 0;//支付金额
        if (txtPayOrder == 0)
        {
            ErrMessage("支付金额不能为0", orderid.ToString());
        }
        if (yfk == 1 && txtPayOrder < price)
        {
            ErrMessage("使用企业钱包大于支付金额!", orderid.ToString());
        }
        if (yfk == 1)
            payPrice = txtPayOrder - price;
        else
            payPrice = txtPayOrder;


        if (txtPayOrder > OrderModel.AuditAmount + OrderModel.OtherAmount - OrderModel.PayedAmount)
        {
            ErrMessage("支付金额大于未支付金额，不能支付！", orderid.ToString());
        }
        if (!((
            (OrderModel.Otype == (int)Enums.OType.赊销订单 && (OrderModel.OState != (int)Enums.OrderState.退回 && OrderModel.OState != (int)Enums.OrderState.未提交 && OrderModel.OState != (int)Enums.OrderState.待审核) && (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (OrderModel.Otype != (int)Enums.OType.赊销订单 && OrderModel.Otype != (int)Enums.OType.推送账单 && (OrderModel.OState == (int)Enums.OrderState.已审 || OrderModel.OState == (int)Enums.OrderState.已发货 || OrderModel.OState == (int)Enums.OrderState.已到货) && (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (OrderModel.Otype == (int)Enums.OType.推送账单 && OrderModel.OState == (int)Enums.OrderState.已审 && (OrderModel.PayState == (int)Enums.PayState.部分支付 || OrderModel.PayState == (int)Enums.PayState.未支付))
            )
            && OrderModel.OState != (int)Enums.OrderState.已作废))
        {
            if (OrderModel.Otype == (int)Enums.OType.推送账单)
            {
                ErrMessage("账单异常，不能支付", orderid.ToString());
            }
            else
            {
                ErrMessage("订单异常，不能支付", orderid.ToString());
            }
        }

        if (yfk == 1 && price > 0)
        {
            //企业钱包处理
            if (sumPrice < price)
            {
                ErrMessage("企业钱包余额不足", orderid.ToString());
            }
            if (disModel.Paypwd != Util.md5(payPas))
            {
                ErrMessage("支付密码不正确", orderid.ToString());
            }
            Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();
            prepayModel.CompID = disModel.CompID;
            prepayModel.DisID = disModel.ID;
            prepayModel.OrderID = OrderModel.ID;
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
                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, OrderModel.ID, price, sqlTrans);//修改订单状态
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
                        if (OrderModel.Otype == (int)Enums.OType.推送账单)
                            Utils.AddSysBusinessLog(disModel.CompID, "Order", OrderModel.ID.ToString(), "账单支付", "支付：" + price.ToString("0.00") + "元(企业钱包支付)", this.UserID.ToString());
                        else
                            Utils.AddSysBusinessLog(disModel.CompID, "Order", OrderModel.ID.ToString(), "订单支付", "支付：" + price + "元(企业钱包支付)", this.UserID.ToString());
                        if (OrderModel.Otype != 9)
                        {
                            OrderInfoType.AddIntegral(this.CompID, this.DisID, "1", 1, OrderModel.ID, price, "订单支付", "", this.UserID);
                        }
                        new Common().GetWxService("2", OrderModel.ID.ToString(), "1", price);


                    }
                    catch (Exception ex)
                    {
                        ErrMessage("支付失败", orderid.ToString());
                    }

                    if (OrderModel.Otype == (int)Enums.OType.推送账单)
                    {

                        Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("2", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(OrderModel.ID.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
                    }
                    else
                    {

                        Response.Redirect("PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(OrderModel.ID.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0".ToString(), Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(prepayid.ToString(), Common.EncryptKey) + "&IsRef=Y", false);
                    }

                }
            }

        }

        #endregion




        #region  插入支付表记录

        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");//支付订单号前半部分

        string guid = Guid.NewGuid().ToString().Replace("-", "");
        payModel.OrderID = OrderModel.ID;// orderid;
        payModel.DisID = this.DisID;
        payModel.Type = 0;// isDBPay;
        payModel.PayUser = disModel.DisName;
        payModel.PayPrice = payPrice;
        payModel.IsAudit = 2;
        payModel.guid = Common.Number_repeat(guid);
        payModel.vdef3 = "1"; //1，订单支付，2，预付款充值、汇款
        payModel.vdef4 = orderNo;
        payModel.CreateDate = DateTime.Now;
        payModel.CreateUserID = this.UserID;
        payModel.ts = DateTime.Now;
        payModel.modifyuser = this.UserID;
        payModel.PrintNum = 1;//支付宝支付无需结算
        //判断账户类型，判断支付渠道
        payModel.Channel = "7";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付
        payModel.State = 0;//手续费收取方
        payModel.vdef5 = "0.00";//支付手续费
        payid = new Hi.BLL.PAY_Payment().Add(payModel);

        if (prepayid > 0)
        {
            Hi.Model.PAY_PrePayment prepayMent = new Hi.BLL.PAY_PrePayment().GetModel(prepayid);
            prepayMent.vdef4 = payid.ToString();//与企业钱包关联
            new Hi.BLL.PAY_PrePayment().Update(prepayMent);
        }

        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        regModel.OrderId = OrderModel.ID;// orderid;
        regModel.Ordercode = orderNo + payid.ToString();
        regModel.number = payModel.guid;
        regModel.Price = payPrice;
        regModel.Payuse = "订单支付";
        regModel.PayName = disModel.DisName;
        regModel.DisID = OrderModel.DisID;
        regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        regModel.Remark = OrderModel.Remark;// orderModel.Remark;
        regModel.DisName = new Hi.BLL.BD_Company().GetModel(disModel.CompID).CompName;
        regModel.BankID = "支付宝支付";
        regModel.CreateUser = this.UserID;
        regModel.CreateDate = DateTime.Now;
        regModel.LogType = 1311;
        int regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
        if (payid <= 0 || regid <= 0)
        {
            Response.Redirect("Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(Convert.ToString(KeyID), Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败！", Common.EncryptKey), false);

        }

        //订单所有商品明细


        DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + orderid);
        foreach (DataRow dr in l.Rows)
        {
            GoodsName += dr["GoodsName"] + ",";
        }
        GoodsName = GoodsName.Substring(0, GoodsName.Length - 1);//去掉最后一个逗号
        if (GoodsName.Length > 15)
            GoodsName = GoodsName.Substring(0, 10) + "...";



        #endregion


        int amount = Convert.ToInt32(payPrice * 100);//支付金额（单位：分）

        //隐藏域赋值
        this.hidguid.Value = payModel.guid;
        this.hidprice.Value = payPrice.ToString();
        this.hidordid.Value = payModel.OrderID.ToString();
        this.hidpid.Value = payid.ToString();
        this.hidppid.Value = prepayid.ToString();

        // btnWxPay_Click(payModel.guid, orderid.ToString(), amount);

        Wxpay_Url wxpaymodel = new Wxpay_Url();
        wxpaymodel.Guid = payModel.guid;
        wxpaymodel.OrderID = orderid.ToString();
        wxpaymodel.Amount = amount;


        wxpaymodel.Hidguid = payModel.guid;
        wxpaymodel.Hidprice =payModel.PayPrice.ToString();
        wxpaymodel.Hidordid = payModel.OrderID.ToString();
        wxpaymodel.Hidpid = payid.ToString();
        wxpaymodel.Hidppid = prepayid.ToString();

        return wxpaymodel;

    }




    /// <summary>
    /// 企业钱包充值
    /// </summary>
    /// <param name="rechengID">充值记录Id</param>
    public Wxpay_Url PayRechang(int rechengID)
    {

        //企业钱包充值记录
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(rechengID);
        decimal price = prepayM.price;

        #region  界面上显示
        txtPayOrder = price;
        this.lblOrderNO.InnerText = prepayM.guid.ToString();
        this.fee.InnerText = this.CompName;//收款方
        // this.lblOrderNO.HRef = "../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey);
        #endregion 界面上显示


        #region 支付记录
        int payid = 0;
        int regid = 0;
        string guid = Guid.NewGuid().ToString().Replace("-", "");
        Hi.Model.PAY_Payment payModel = new Hi.Model.PAY_Payment();
        payModel.OrderID = rechengID;
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
        payModel.PrintNum = 1;
        payModel.Channel = "7";//1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付，5，线下支付，6，支付宝支付 7，微信支付
        payModel.State = 0;//手续费收取方
        payModel.vdef5 = "0.00";//支付手续费

        payid = new Hi.BLL.PAY_Payment().Add(payModel);

        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        regModel.OrderId = rechengID;
        regModel.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(rechengID);
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
            ErrMessage("数据异常", rechengID.ToString());
        }
        GoodsName = "钱包充值";

        #endregion

        int amount = Convert.ToInt32(price * 100);//支付金额（单位：分）

        //隐藏域赋值
        this.hidguid.Value = payModel.guid;
        this.hidprice.Value = price.ToString();
        this.hidordid.Value = payModel.OrderID.ToString();
        this.hidpid.Value = payid.ToString();
        this.hidppid.Value = "recharge";// prepayid.ToString();


        Wxpay_Url wxpaymodel = new Wxpay_Url();
        wxpaymodel.Guid = payModel.guid;
        wxpaymodel.OrderID = orderid.ToString();
        wxpaymodel.Amount = amount;

        wxpaymodel.Hidguid = payModel.guid;
        wxpaymodel.Hidprice = price.ToString();
        wxpaymodel.Hidordid = payModel.OrderID.ToString();
        wxpaymodel.Hidpid= payid.ToString();
        wxpaymodel.Hidppid= "recharge";
        return wxpaymodel;

        //btnWxPay_Click(payModel.guid, orderid.ToString(), amount);
    }
}

/// <summary>
/// 微信支付帮助类
/// </summary>
public class Wxpay
{
    #region Model
    private int _orderid;
    private decimal _txtPayOrder;
    private decimal _price;
    private int _yfk;
    private string _payPas;
    private string _preorord;

    /// <summary>
    /// 
    /// </summary>
    public int OrderID
    {
        set { _orderid = value; }
        get { return _orderid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal TxtPayOrder
    {
        set { _txtPayOrder = value; }
        get { return _txtPayOrder; }
    }
    /// <summary>
    /// 
    /// </summary>
    public decimal Price
    {
        set { _price = value; }
        get { return _price; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int Yfk
    {
        set { _yfk = value; }
        get { return _yfk; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string PayPas
    {
        set { _payPas = value; }
        get { return _payPas; }
    }

    /// <summary>
    /// 判断是订单支付还是充值
    /// </summary>
    public string Preorord
    {
        set { _preorord = value; }
        get { return _preorord; }
    }



    #endregion Model
}


/// <summary>
/// 微信支付帮助类
/// </summary>
public class Wxpay_Url
{
    #region Model
    private string _guid;
    private string _orderid;
    private int _amount;

    private string  _hidguid;
    private string  _hidprice;
    private string  _hidordid;
    private string  _hidpid;
    private string  _hidppid;


    /// <summary>
    /// 
    /// </summary>
    public string Guid
    {
        set { _guid = value; }
        get { return _guid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string OrderID
    {
        set { _orderid = value; }
        get { return _orderid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public int Amount
    {
        set { _amount = value; }
        get { return _amount; }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Hidguid
    {
        set { _hidguid = value; }
        get { return _hidguid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string Hidordid
    {
        set { _hidordid = value; }
        get { return _hidordid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string Hidprice
    {
        set { _hidprice = value; }
        get { return _hidprice; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string Hidpid
    {
        set { _hidpid = value; }
        get { return _hidpid; }
    }
    /// <summary>
    /// 
    /// </summary>
    public string Hidppid
    {
        set { _hidppid = value; }
        get { return _hidppid; }
    }

    #endregion Model
}