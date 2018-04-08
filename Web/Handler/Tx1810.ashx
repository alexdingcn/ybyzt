<%@ WebHandler Language="C#" Class="Tx1810" %>

using System;
using System.Web;
using System.Collections;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class Tx1810 : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
        PaymentEnvironment.Initialize(configPath);
        string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
        string Josn = "";
        string date = "";
        if (request["date"]!=null)
        {
            date = request["date"].Trim().ToString();
        }
        try
        {
            Tx1810Request tx1810 = new Tx1810Request();
            tx1810.setInstitutionID(institutionID);
            tx1810.setDate(date);
            tx1810.process();

            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1810.getRequestMessage(), tx1810.getRequestSignature());
            Tx1810Response tx1810Response = new Tx1810Response(respMsg[0], respMsg[1]);
            if (tx1810Response.getCode() == "2000")
            {
                ArrayList txList = tx1810Response.getTxList();
                int size = txList.Count;
                Josn = "[";
                for (int i = 0; i < size; i++)
                {
                    Tx tx = (Tx)txList[i];
                    Josn += "{";
                    Josn += "\"TxType\":\"" + tx.getTxType();
                    Josn += "\",\"TxSn\":\"" + tx.getTxSn();
                    Josn += "\",\"TxAmount\":\"" + tx.getTxAmount();
                    Josn += "\",\"InstitutionAmount\":\"" + tx.getInstitutionAmount();
                    Josn += "\",\"PaymentAmount\":\"" + tx.getPaymentAmount();
                    Josn += "\",\"PayerFee\":\"" + tx.getPayerFee();
                    Josn += "\",\"InstitutionFee\":\"" + tx.getInstitutionFee();
                    Josn += "\",\"Remark\":\"" + tx.getRemark();
                    Josn += "\",\"BankNotificationTime\":\"" + (tx.getBankNotificationTime()==""?"":DateTime.ParseExact(tx.getBankNotificationTime(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss"));
                    Josn += "\"},";
                }
                if (size > 0)
                {
                    Josn = Josn.Substring(0, Josn.Length - 1);
                }
                Josn += "]";
            }
            else
            {
                Josn = "{\"error\":\"1\",\"msg\":\"" + tx1810Response.getMessage() + "！\"}";
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