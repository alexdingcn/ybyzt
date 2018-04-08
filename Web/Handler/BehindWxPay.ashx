<%@ WebHandler Language="C#" Class="BehindWxPay" %>

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Data;


public class BehindWxPay : IHttpHandler
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public void ProcessRequest(HttpContext context)
    {
      
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        //1 获得参数message和signature
        String hidguid = request["hidguid"];
        String hidprice = request["hidprice"];
        String hidordid = request["hidordid"];
        String hidpid = request["hidpid"];
        String hidppid = request["hidppid"];

        string Josn = string.Empty;
        DataTable dt=null;
        if(hidppid=="recharge")
          dt= new Hi.BLL.PAY_PrePayment().GetDate("count(1) as num", " PAY_Payment", " id=" + hidpid + " and guid='" + hidguid + "' and PayPrice =" + hidprice + "  and  IsAudit=1 and OrderID=" + hidordid);
        else
          dt = new Hi.BLL.PAY_PrePayment().GetDate("count(1) as num", " PAY_Payment", " id=" + hidpid + " and guid='" + hidguid + "' and PayPrice =" + hidprice + "  and  IsAudit=1");
        
        
        if (dt.Rows.Count > 0)
        {
            int num = Convert.ToInt32(dt.Rows[0]["num"]);
            if (num > 0)
            {

                if (hidppid == "recharge")
                    Josn = "{\"rel\":\"ok\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(hidordid, Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(hidpid, Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(hidppid, Common.EncryptKey) + "\"}";

                else
                Josn = "{\"rel\":\"ok\",\"js\":\"" + "PaySuccess.aspx?type=" + Common.DesEncrypt("1", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(hidordid, Common.EncryptKey) + "&Pid=" + Common.DesEncrypt(hidpid, Common.EncryptKey) + "&PPid=" + Common.DesEncrypt(hidppid, Common.EncryptKey) + "\"}";

                
                
            }

            else
                Josn = "{\"rel\":\"error\",\"js\":\"" + "Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(hidordid, Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败", Common.EncryptKey) + "\"}";

        }
        else
            Josn = "{\"rel\":\"error\",\"js\":\"" + "Error.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(hidordid, Common.EncryptKey) + "&msg=" + Common.DesEncrypt("支付失败", Common.EncryptKey) + "\"}";
         
        context.Response.Write(Josn);
        context.Response.End();

    }






    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}