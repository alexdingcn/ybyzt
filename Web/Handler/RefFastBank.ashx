<%@ WebHandler Language="C#" Class="RefFastBank" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using System.Web.SessionState;

public class RefFastBank : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int userid = Convert.ToInt32(context.Request["user"].Trim().ToString());
        //Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(userid);
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        
        string strWhere = " 1=1 ";
        if (logUser.DisID!= 0)
        {
            strWhere += " and DisID = " + logUser.DisID;
        }
        strWhere += " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        JavaScriptSerializer js = new JavaScriptSerializer();
        string Josn = js.Serialize(fastList);
        context.Response.Write(Josn);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}