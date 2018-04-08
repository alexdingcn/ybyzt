<%@ WebHandler Language="C#" Class="BankCodeBlur" %>

using System;
using System.Web;
using System.Data;
using DBUtility;

public class BankCodeBlur : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string json = "";
        string BankCode = Convert.ToString(request["BankCode"]);
        if (BankCode != null && BankCode != "")
        {
            BankCode = BankCode.Replace(" ", "");
        }
        if (BankCode.Length > 7)
        {
            BankCode = BankCode.Substring(0, 8);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        if (BankCode.Length > 6)
        {
            BankCode = BankCode.Substring(0, 7);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        if (BankCode.Length > 5)
        {
            BankCode = BankCode.Substring(0, 6);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        if (BankCode.Length > 4)
        {
            BankCode = BankCode.Substring(0, 5);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        if (BankCode.Length > 3)
        {
            BankCode = BankCode.Substring(0, 4);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        if (BankCode.Length > 2)
        {
            BankCode = BankCode.Substring(0, 3);
            json = GetBank(BankCode);
            if (json != "")
            {
                response.Write(json);
                response.End();
            }
        }
        
    }

    public string GetBank(string BankCode)
    {
        string result = "";
        try
        {
            string strSql = "select * from PAY_BankBIN where BIN = '" + BankCode + "'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                int BankInfo_id = SqlHelper.GetInt(ds.Tables[0].Rows[0]["BankInfo_id"]);
                Hi.Model.PAY_BankInfo BnakInfoM = new Hi.BLL.PAY_BankInfo().GetModel(BankInfo_id);
                result = "{\"BankCode\":\"" + BnakInfoM.BankCode + "\",\"BankName\":\"" + BnakInfoM.BankName + "\"}";
            }
        }
        catch { }
        return result;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}