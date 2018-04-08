<%@ WebHandler Language="C#" Class="Tx1372" %>

using System;
using System.Web;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class Tx1372 : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        string Josn = "";
        string number = "";
        if (request["number"] != null)
        {
            number = request["number"].Trim().ToString();
        }

        string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
        PaymentEnvironment.Initialize(configPath);
        string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
        try
        {
            Tx1372Request tx1372 = new Tx1372Request();
            tx1372.setInstitutionID(institutionID);
            tx1372.setPaymentNo(number);
            tx1372.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1372.getRequestMessage(), tx1372.getRequestSignature());

            Tx1372Response tx1372Response = new Tx1372Response(respMsg[0], respMsg[1]);
            if ("2000".Equals(tx1372Response.getCode()))
            {
                Josn += "[{";
                Josn += "\"OrderNo\":\"" + tx1372Response.getOrderNo() + "\"";
                Josn += ",\"PaymentNo\":\"" + tx1372Response.getPaymentNo() + "\"";
                Josn += ",\"Status\":\"" + (tx1372Response.getStatus() == 10 ? "处理中" : tx1372Response.getStatus()==20?"支付成功":"支付失败") + "\"";
                Josn += ",\"BankTxTime\":\"" + DateTime.ParseExact(tx1372Response.getBankTxTime(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss") + "\"";
                Josn += "}]";
            }
            else
            {
                Josn = "{\"error\":\"1\",\"msg\":\"" + tx1372Response.getMessage() + "！\"}";
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