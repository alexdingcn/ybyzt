<%@ WebHandler Language="C#" Class="CMHandler" %>

using System;
using System.Web;

public class CMHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string PageAction = context.Request["PageAction"];
        string ReturnMsg = "";
        switch (PageAction)
        {
            case "3": ReturnMsg = GetDis(context); break;
            case "2": ReturnMsg = GetArea(context); break;
        }
        context.Response.Write(ReturnMsg);
        context.Response.End();
    }

    /// <summary>
    /// 指定代理商
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string GetDis(HttpContext context)
    {
        string compId = context.Request["compid"];
        string name = context.Request["name"] + "";
        string ids = context.Request["ids"] + "";
        string str = "and cu.CompID=" + compId + " and isnull(dis.dr,0)=0";

        if (!"".Equals(name)) {
            str += " and dis.DisName like '%" + name + "%'";
        }
        if (!"".Equals(ids)) {
            str += " and dis.id in (" + ids + ")";
        }

        string sql = string.Format("select cu.disID ID,cu.compID,dis.DisName disname,cu.IsAudit AuditState,dis.CreateDate,cu.IsEnabled,dis.Principal,dis.phone,cu.AreaID,cu.DisTypeID  from SYS_CompUser cu left join BD_Distributor dis on cu.DisID=dis.ID where cu.CType=2 and cu.UType=5 and cu.IsAudit=2 and cu.IsEnabled=1 {0}", str);

        System.Data.DataTable LDis = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

        System.Collections.Generic.List<Hi.Model.BD_Distributor> l = new System.Collections.Generic.List<Hi.Model.BD_Distributor>();
        if (LDis != null && LDis.Rows.Count > 0)
        {
            Hi.Model.BD_Distributor dis = null;
            foreach (System.Data.DataRow item in LDis.Rows)
            {
                dis = new Hi.Model.BD_Distributor();
                dis.ID = item["ID"].ToString().ToInt(0);
                dis.DisName = item["DisName"].ToString();
                l.Add(dis);
            }
        }   
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(l).ToString();
    }

    /// <summary>
    /// 指定区域
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string GetArea(HttpContext context)
    {
        string compId = context.Request["compid"];
        string name = context.Request["name"] + "";
        string ids = context.Request["ids"] + "";        
        string str = " CompanyID=" + compId + " and isnull(dr,0)=0";
        if (!"".Equals(name))
        {
            str += " and AreaName like '%" + name + "%'";
        }
        if (!"".Equals(ids))
        {
            str += " and id in (" + ids + ")";
        }
        
        System.Collections.Generic.List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList("", str, "");
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(l).ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}