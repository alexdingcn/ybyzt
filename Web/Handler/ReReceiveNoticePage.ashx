<%@ WebHandler Language="C#" Class="ReReceiveNoticePage" %>

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class ReReceiveNoticePage : IHttpHandler {

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        
         //1 获得参数message和signature
        String message = request.Form["message"];
        String signature = request.Form["signature"];

        Console.WriteLine("[message]=[" + message + "]");
        Console.WriteLine("[signature]=[" + signature + "]");

        //定义变量
        //String txName = "";

        //2 生成交易结果对象
        NoticeRequest noticeRequest = new NoticeRequest(message, signature);
        string a = noticeRequest.getTxCode();
        //3 业务处理
        if ("1318".Equals(noticeRequest.getTxCode()))
        {
            Notice1318Request nr = new Notice1318Request(noticeRequest.getDocument());
            string guid = nr.getPaymentNo();
            string strWhere2 = string.Empty;
            if (guid != "")
            {
                strWhere2 += " number = '" + guid + "'";
            }
            Hi.Model.PAY_RegisterLog regM = new Hi.BLL.PAY_RegisterLog().GetList("", strWhere2, "")[0];
            string strWhere = string.Empty;
            if (guid != "")
            {
                strWhere += " guid = '" + guid + "'";
            }
            strWhere += " and isnull(dr,0)=0";
            Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetList("", strWhere, "")[0];
            try
            {
                payM.PayDate = DateTime.Now;
                payM.ts = DateTime.Now;
                payM.status = nr.getStatus();
                new Hi.BLL.PAY_Payment().Update(payM);
                regM.Start = Convert.ToString(nr.getStatus());
                new Hi.BLL.PAY_RegisterLog().Update(regM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
            string strWhere1 = string.Empty;
            if (guid != "")
            {
                strWhere1 += " ID = " + payM.OrderID;
            }
            strWhere1 += " and isnull(dr,0)=0";
            prepayM = new Hi.BLL.PAY_PrePayment().GetList("", strWhere1, "")[0];
            if (nr.getStatus() == 20)
            {

                if (payM.IsAudit == 1)
                {

                    response.Redirect("/Distributor/Pay/PaySuccess.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(prepayM.ID.ToString(), Common.EncryptKey), true);
                    return;
                  
                }        
                
                
                int prepay = 0;
                int pay = 0;
                SqlConnection con = new SqlConnection(LocalSqlServer);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                try
                {
                    pay = new Hi.BLL.PAY_Payment().updatePayState(con, payM.ID, sqlTrans);
                    prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);

                    //修改免支付次数
                    Common.UpmzfcsByCompid(prepayM.CompID);
                    
                    sqlTrans.Commit();
                }
                catch
                {
                    pay = 0;
                    prepay = 0;
                    sqlTrans.Rollback();
                }
                finally
                {
                    con.Close();
                }
                if (pay > 0 && prepay > 0)
                {
                    response.Redirect("/Distributor/Pay/PaySuccess.aspx?type=" + Common.DesEncrypt("3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(prepayM.ID.ToString(), Common.EncryptKey), true);
                }
                else
                {
                    response.Redirect("/Distributor/Pay/Recharge.aspx?KeyID=" + Common.DesEncrypt(prepayM.ID.ToString(), Common.EncryptKey), true);
                }
            }
            else
            {
                response.Redirect("/Distributor/Pay/Recharge.aspx?KeyID=" + Common.DesEncrypt(prepayM.ID.ToString(), Common.EncryptKey), true);
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}