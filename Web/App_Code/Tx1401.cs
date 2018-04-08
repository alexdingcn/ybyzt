using CFCA.Payment.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Tx1401 的摘要说明
/// </summary>
public class Tx1401
{
    public Tx1401()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 中金聚合支付（微信支付宝支付）
    /// </summary>
    /// <param name="paytype">类型（10=微信扫码支付  20=支付宝扫码支付）</param>
    /// <param name="KeyID">支付订单id</param>
    /// <param name="payPrice">本次支付金额</param>
    /// <returns></returns>
    public static  string[] tx1401(string paytype,string KeyID,decimal payPrice)
    {
        string imageUrl = string.Empty;
        bool fal = false;
        Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(KeyID));
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(OrderModel.DisID);

       


        #region     计算支付手续费 start
        string sxfsq = "-1";
        decimal sxf = 0;
        decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。
                             // 获取手续费 begin  
        string[] Json = Common.GetAli_Sxf(disModel.CompID, payPrice);

        string strMsg = Json[2].ToString();
        if (!string.IsNullOrEmpty(strMsg))
        {
            return  new string[] {"false", strMsg}; //"{\"sxf\":\"" + sxf + "\",\"sxfsq\":\"" + sxfsq + "\",\"strMsg\":\"" + str + "\"}";
           
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
        payModel.OrderID = OrderModel.ID;
        payModel.DisID = OrderModel.DisID;
        payModel.Type = Convert.ToInt32(WebConfigurationManager.AppSettings["IsDBPay"]);
        payModel.PayUser = disModel.DisName;
        payModel.PayPrice = payPrice;
        payModel.IsAudit = 2;
        payModel.guid = Common.Number_repeat(guid);
        payModel.vdef3 = "1";//1，订单支付，2，预付款充值、汇款
        payModel.vdef4 = orderNo;
        payModel.CreateDate = DateTime.Now;
        payModel.CreateUserID = OrderModel.CreateUserID;
        payModel.ts = DateTime.Now;
        payModel.modifyuser = OrderModel.CreateUserID;
        payModel.Channel = paytype.Equals("10") ? "7" : "6";//（微信支付6，支付宝7）

        payModel.State = Convert.ToInt32(sxfsq);//手续费收取方

        if (sxfsq.Equals("2"))
            payModel.vdef5 = comp_sxf.ToString("0.00");
        else
            payModel.vdef5 = sxf.ToString("0.00");//支付手续费

        payid = new Hi.BLL.PAY_Payment().Add(payModel);

        //插入日志表
        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
        regModel.OrderId = OrderModel.ID;
        regModel.Ordercode = orderNo;// orderNo + payid.ToString();
        regModel.number = payModel.guid;
        regModel.Price = payPrice;
        regModel.Payuse = "订单支付";
        regModel.PayName = disModel.DisName;
        regModel.DisID = OrderModel.DisID;
        regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        regModel.Remark = OrderModel.Remark;
        regModel.DisName = new Hi.BLL.BD_Company().GetModel(OrderModel.CompID).CompName;
        regModel.BankID = paytype;
        regModel.CreateUser = OrderModel.CreateUserID;
        regModel.CreateDate = DateTime.Now;
        regModel.LogType = 1401;
        regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
        if (payid <= 0 || regid <= 0)
        {
            return new string[] { "false", "数据有误" };
        }
        string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
        try
        {
            PaymentEnvironment.Initialize(configPath);
        }
        catch (Exception ex)
        {
            return new string[] { "false", "支付配置有误，请联系系统管理员" };
        }

        String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构代码

        string GoodsName = string.Empty;
        DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + KeyID);
        foreach (DataRow dr in l.Rows)
        {
            GoodsName += dr["GoodsName"] + ",";
        }
        if (GoodsName.Length > 15)
            GoodsName = GoodsName.Substring(0, 10) + "...";

        // 1.取得参数        
        int paymentWay = Convert.ToInt32(paytype);
        String expirePeriod = "5";//默认是五分钟
        String subject = GoodsName;
        String productID = KeyID.ToString();
        String goodsTag = "";
        String remark = "";
        String notificationURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Distributor/Pay/ReceiveNoticeAlipay.aspx";
        int limitPay = 10;

        long amount = Convert.ToInt64(payPrice * 100);//支付金额（单位：分）
        long discountAmount = 0;

        // 2.创建交易请求对象
        Tx1401Request tx1401Request = new Tx1401Request();
        tx1401Request.setInstitutionID(institutionID);
        tx1401Request.setOrderNo(orderNo);
        tx1401Request.setPaymentNo(payModel.guid);
        tx1401Request.setPaymentWay(paymentWay);
        tx1401Request.setAmount(amount);
        tx1401Request.setExpirePeriod(expirePeriod);
        tx1401Request.setSubject(subject);
        tx1401Request.setDiscountAmount(discountAmount);
        tx1401Request.setProductID(productID);
        tx1401Request.setGoodsTag(goodsTag);
        tx1401Request.setRemark(remark);
        tx1401Request.setNotificationURL(notificationURL);
        tx1401Request.setLimitPay(limitPay);

        // 3.执行报文处理
        tx1401Request.process();

        //2个信息参数
        HttpContext.Current.Items["txCode"] = "1401";
        HttpContext.Current.Items["txName"] = "市场订单聚合支付";

        // 与支付平台进行通讯
        TxMessenger txMessenger = new TxMessenger();
        String[] respMsg = txMessenger.send(tx1401Request.getRequestMessage(), tx1401Request.getRequestSignature());// 0:message; 1:signature

        String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));

        //Console.WriteLine("[message] = [" + respMsg[0] + "]");
        //Console.WriteLine("[signature] = [" + respMsg[1] + "]");
        //Console.WriteLine("[plaintext] = [" + plaintext + "]");

        Tx1401Response tx1401Response = new Tx1401Response(respMsg[0], respMsg[1]);
        HttpContext.Current.Items["plainText"] = tx1401Response.getResponsePlainText();
        if ("2000".Equals(tx1401Response.getCode()))
        {
            //图片URL
            imageUrl = tx1401Response.getImageUrl();
            //二维码代码
            string codeUrl = tx1401Response.getCodeUrl();
            //处理业务
            fal = true;
        
        }
        return new string[] {fal.ToString(), imageUrl , payModel.guid};
    }
}