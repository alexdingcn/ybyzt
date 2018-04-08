<%@ WebHandler Language="C#" Class="tx1350" %>

using System;
using System.Web;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class tx1350 : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
        PaymentEnvironment.Initialize(configPath);
        string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
        string Josn = "";
        string number = "";
        if (request["number"] != null)
        {
            number = request["number"].Trim().ToString();
        }

        try
        {
            Tx1350Request tx1350 = new Tx1350Request();
            tx1350.setInstitutionID(institutionID);
            tx1350.setSerialNumber(number);
            tx1350.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1350.getRequestMessage(), tx1350.getRequestSignature());

            Tx1350Response tx1350Response = new Tx1350Response(respMsg[0], respMsg[1]);
            if ("2000".Equals(tx1350Response.getCode()))
            {
                Josn += "[{";
                Josn += "\"SerialNumber\":\"" + tx1350Response.getSerialNumber() + "\"";
                Josn += ",\"OrderNo\":\"" + tx1350Response.getOrderNo() + "\"";
                Josn += ",\"Amount\":\"" + tx1350Response.getAmount() + "\"";
                Josn += ",\"Remark\":\"" + tx1350Response.getRemark() + "\"";
                Josn += ",\"AccountType\":\"" + (tx1350Response.getAccountType() == 11 ? "个人账户" : tx1350Response.getAccountType() == 12 ? "企业账户" : "支付平台内部账户") + "\"";
                Josn += ",\"PaymentAccountName\":\"" + tx1350Response.getPaymentAccountName() + "\"";
                Josn += ",\"PaymentAccountNumber\":\"" + tx1350Response.getPaymentAccountNumber() + "\"";
                Josn += ",\"BankID\":\"" + tx1350Response.getBankAccount().getBankID() + "\"";
                Josn += ",\"AccountName\":\"" + tx1350Response.getBankAccount().getAccountName() + "\"";
                Josn += ",\"AccountNumber\":\"" + tx1350Response.getBankAccount().getAccountNumber() + "\"";
                Josn += ",\"BranchName\":\"" + tx1350Response.getBankAccount().getBranchName() + "\"";
                Josn += ",\"Province\":\"" + tx1350Response.getBankAccount().getProvince() + "\"";
                Josn += ",\"City\":\"" + tx1350Response.getBankAccount().getCity() + "\"";
                Josn += ",\"Status\":\"" + (tx1350Response.getStatus() == 10 ? "已经受理" : tx1350Response.getStatus() == 30 ? "正在结算" : tx1350Response.getStatus() == 40 ? "已经执行(已发送转账指令)" : tx1350Response.getStatus() == 50 ? "转账退回" : "异常状态") + "\"";
                Josn += "}]";
            }
            else
            {
                Josn = "{\"error\":\"1\",\"msg\":\"" + tx1350Response.getMessage() + "！\"}";
            }
        }
        catch
        {
            Josn = "{\"error\":\"1\",\"msg\":\"接口调用失败！\"}";
        }
        finally
        {
            context.Response.Write(Josn);
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}