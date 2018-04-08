<%@ WebHandler Language="C#" Class="Trd13010" %>

using System;
using System.Web;
using System.Collections.Generic;
using LitJson;
using FinancingReferences;

public class Trd13010 : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        string username = Convert.ToString(request.Form["hidUserName"]);
       // Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);

        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        if (logUser == null)
        {
            string Josn = "0.00";
            context.Response.Write(Josn);
            context.Response.End();
        }
        List<Hi.Model.PAY_OpenAccount> LOpen = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + logUser.DisID + " and State=1 and dr=0", "");
        if (LOpen.Count == 0)
        {
            string Josn = "0.00";
            context.Response.Write(Josn);
            context.Response.End();
        }
        List<Hi.Model.PAY_Withdrawals> withdrawalsList = new Hi.BLL.PAY_Withdrawals().GetList("", " isnull(dr,0)=0  and DisID=" + logUser.DisID, "");
        if (withdrawalsList.Count == 0)
        {
            string Josn = "0.00";
            context.Response.Write(Josn);
            context.Response.End();
        }
        String msghd_rspcode = "";
        String msghd_rspmsg = "";
        String Json = "";
        try
        {
            IPubnetwk ipu = new IPubnetwk();
            Json = ipu.trd13010("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + Common.Number_repeat("1") + "\",\"cltacc_cltno\":\"" + LOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LOpen[0].AccName + "\",\"bkacc_accno\":\"" + withdrawalsList[0].AccNo + "\",\"bkacc_accnm\":\"" + withdrawalsList[0].AccNm + "\"}");
        }
        catch
        {
            string Josn = "0.00";
            context.Response.Write(Josn);
            context.Response.End();
        }
        try
        {
            JsonData Params = JsonMapper.ToObject(Json);
            msghd_rspcode = Params["msghd_rspcode"].ToString();
            msghd_rspmsg = Params["msghd_rspmsg"].ToString();
        }
        catch
        {
            string Josn = "0.00";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if ("000000".Equals(msghd_rspcode))
        {
            string Josn = "{\"success\":\"1\",\"msg\":\"成功！\",\"js\":\"window.location='orderPayList.aspx'\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        else
        {
            string Josn = "0.00";
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