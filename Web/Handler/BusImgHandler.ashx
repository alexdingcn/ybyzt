<%@ WebHandler Language="C#" Class="BusImgHandler" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;

public class BusImgHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string PageAction = context.Request["PageAction"];
        string ReturnMsg = "";
        switch (PageAction)
        {
            case "GetClassChild": ReturnMsg = GetClassChild(context); break;
        }
        context.Response.Write(ReturnMsg);
        context.Response.End();
    }


    //查询大分类下的二级/三级分类  以及大分类下的企业
    public string GetClassChild(HttpContext context)
    {
        string outHtml = string.Empty;
        string indid = context.Request["indid"];
        if (!string.IsNullOrWhiteSpace(indid) && indid.Trim().ToInt(0) > 0)
        {
            indid = indid.Trim().Replace("'", "''");
            List<Hi.Model.SYS_GType> typeList = new Hi.BLL.SYS_GType().GetList("ID,TypeName,ParentId,Deep", " dr=0 and IsEnabled=1 and FullCode like '" + indid + "-%'", "");
            if (typeList.Count > 0)
            {
                List<Hi.Model.SYS_GType> typeTwoList = typeList.Where(T => T.Deep == 2).ToList().OrderBy(T => T.ID).ToList();
                foreach (Hi.Model.SYS_GType typeTwo in typeTwoList)
                {
                    outHtml += " <h2 class=\"title\"><a href=\"/goodslist_" + typeTwo.ID + ".html\">" + typeTwo.TypeName + "</a></h2>";
                    List<Hi.Model.SYS_GType> threeList = typeList.Where(T => T.ParentId == typeTwo.ID).ToList().OrderBy(T => T.ID).ToList();
                    outHtml += " <ul class=\"list\">";
                    if (threeList.Count > 0)
                    {
                        int threeListcount = 0;
                        foreach (Hi.Model.SYS_GType typeThree in threeList)
                        {
                            threeListcount++;

                            string s = string.Empty;
                            s = threeListcount == threeList.Count ? "" : "<i class=\"fg\">|</i>";
                            outHtml += "<li><a href=\"/goodslist_" + typeThree.ID + ".html\">" + typeThree.TypeName + "</a>" + s + "</li>";
                        }
                    }
                    outHtml += " </ul>";
                }
            }

        }
        //outHtml += "<div class=\"m-logo\">";
        ////查询企业信息
        //List<Hi.Model.BD_Company> LComp = new Hi.BLL.BD_Company().GetList("top 7 CompName,ID,CompLogo,ShortName,ShopLogo",
        //" Indid = '" + indid.Trim().ToInt(0)  + "' and dr=0 and AuditState=2 and FirstShow in(1,2) and IsEnabled=1 and (CompLogo<>'' or ShopLogo<>'') ",
        //" SortIndex desc,id ");
        //if (LComp != null && LComp.Count > 0)
        //{
        //    outHtml = LComp.Where(item => !string.IsNullOrWhiteSpace(item.ShopLogo) || !string.IsNullOrWhiteSpace(item.CompLogo)).Aggregate(outHtml, ((current, item) => current + "<a href=\"/" + item.ID + ".html\" target=\"_blank\"><img src=\"" + (
        //      System.Configuration.ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + (item.ShopLogo == "" ? item.CompLogo : item.ShopLogo)) + "\" /></a>"));
        //}
        //outHtml += "</div>";
        return outHtml;
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}