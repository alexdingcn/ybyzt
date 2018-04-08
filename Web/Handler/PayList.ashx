<%@ WebHandler Language="C#" Class="Tx1311" %>

using System;
using System.Web;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class Tx1311 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        
        String configPath = WebConfigurationManager.AppSettings["payment.config.path"];
        PaymentEnvironment.Initialize(configPath);       
        
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        // 1.取得参数
        String institutionID = "000020";//机构号码
        String orderNo = Convert.ToString(DateTime.Now.Ticks);//订单号
        String paymentNo = Guid.NewGuid().ToString().Replace("-", "");//支付流水号
        long amount = Convert.ToInt64("1000");//订单金额
        long fee = Convert.ToInt64("0");//支付服务手续费
        String payerID = !"88888888".Equals("") ? "88888888".Trim() : null;//付款者ID
        String payerName = !"中国金融认证中心".Equals("") ? "中国金融认证中心".Trim() : null;//付款者名称
        String usage = !"标书费".Equals("") ? "标书费".Trim() : null;//资金用途
        String remark = !"支付测试".Equals("") ? "支付测试".Trim() : null;//订单描述
        String appPath = request.ApplicationPath;
        if (!appPath.EndsWith("/"))
        {
            appPath += "/";
        }
        String notificationURL = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port + appPath;//!request.Form["NotificationURL"].Equals("") ? request.Form["NotificationURL"].Trim() : null;//通知URL
        String payees = "test";//收款人（以";"间隔）
        String bankID = "700";//银行ID
        int accountType = Convert.ToInt32("12");//账户类型

        // 2.创建交易请求对象
        Tx1311Request tx1311Request = new Tx1311Request();
        tx1311Request.setInstitutionID(institutionID);
        tx1311Request.setOrderNo(orderNo);
        tx1311Request.setPaymentNo(paymentNo);
        tx1311Request.setAmount(amount);
        tx1311Request.setFee(fee);
        tx1311Request.setPayerID(payerID);
        tx1311Request.setPayerName(payerName);
        tx1311Request.setUsage(usage);
        tx1311Request.setRemark(remark);
        tx1311Request.setNotificationURL(notificationURL);
        tx1311Request.setBankID(bankID);
        tx1311Request.setAccountType(accountType);
        if (null != payees && payees.Length > 0)
        {
            String[] payeeList = payees.Split(';');
            for (int i = 0; i < payeeList.Length; i++)
            {
                tx1311Request.addPayee(payeeList[i]);
            }
        }

        // 3.执行报文处理
        tx1311Request.process();

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
        context.Server.Transfer("../Dealer/Pay/Request.aspx", true);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}