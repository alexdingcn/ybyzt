<%@ WebHandler Language="C#" Class="Tx1320" %>

using System;
using System.Web;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class Tx1320 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
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
            Tx1320Request tx1320 = new Tx1320Request();
            tx1320.setInstitutionID(institutionID);
            tx1320.setPaymentNo(number);
            tx1320.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1320.getRequestMessage(), tx1320.getRequestSignature());

            Tx1320Response tx1320Response = new Tx1320Response(respMsg[0], respMsg[1]);
            if ("2000".Equals(tx1320Response.getCode()))
            {

                string str = tx1320Response.getBankNotificationTime();
                Josn += "[{";
                Josn += "\"PaymentNo\":\"" + tx1320Response.getPaymentNo() + "\"";
                Josn += ",\"Amount\":\"" + tx1320Response.getAmount() + "\"";
                Josn += ",\"Remark\":\"" + tx1320Response.getRemark() + "\"";
                Josn += ",\"Status\":\"" + (tx1320Response.getStatus() == 10 ? "未支付" : "已支付") + "\"";
                if (!string.IsNullOrEmpty(str))
                    Josn += ",\"BankNotificationTime\":\"" + DateTime.ParseExact(tx1320Response.getBankNotificationTime(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss") + "\"";
                Josn += "}]";
            }
            else
            {
                Josn = "{\"error\":\"1\",\"msg\":\"" + tx1320Response.getMessage() + "！\"}";
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}