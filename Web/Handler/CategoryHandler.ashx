<%@ WebHandler Language="C#" Class="CategoryHandler" %>

using System;
using System.Collections.Generic;
using System.Web;

public class CategoryHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write(GetCategory(context));
    }

    public string GetCategory(HttpContext context)
    {
        string ChildString = string.Empty;
        string parentID = string.Empty;
        string tableType = string.Empty;
        string Level = string.Empty;
        
        if (context.Request["ParentID"] != null)
        {
            parentID = context.Request["ParentID"].ToString();
            if (context.Request["TableType"] != null)
            {
                tableType = context.Request["TableType"].ToString();
                Level = context.Request["Level"].ToString();
                if (tableType == "1")
                {
                    List<Hi.Model.BD_GoodsCategory> cateList = new Hi.BLL.BD_GoodsCategory().GetList("", "ParentID='" + parentID + "' and Deep='" + (Convert.ToInt32(Level)+1).ToString() + "' and IsEnabled=1 and dr=0", "SortIndex");
                    if (cateList != null && cateList.Count > 0)
                    {
                        for (int i=0;i<cateList.Count;i++)
                        {
                            Hi.Model.BD_GoodsCategory model = cateList[i];
                            if (Level == "1") //当前层级,加载第二层
                            {
                                ChildString += "<tr id='" + model.ID + "' parentid='" + model.ParentId +
                                               "' bgcolor='#fcfeff' style='height: 26px;width: 100%; display:none;' isopen='0' level='2'>";
                                ChildString += "<td><div class=\"tcle\"> <img id='Openimg' height='9' src='" + Simage(model.ID.ToString()) +
                                               "' width='9' border='0' style='margin-left:39px;'/>&nbsp; <span class='span'>" +
                                               model.CategoryName + "</span><div></td>";
                            }
                            else
                            {
                                ChildString += "<tr id='" + model.ID + "' parentid='" + model.ParentId +
                                                   "' bgcolor='#fcfeff' style='height: 26px;width: 100%; display:none;' isopen='1' level='3'>";
                                ChildString += "<td><div class=\"tcle\"> <img id='Openimg' height='9' src='../images/menu_minus.gif' width='9' border='0' style='margin-left:78px;'/>&nbsp; <span class='span'>" +
                                               model.CategoryName + "</span></div></td>";
                            }
                            
                            ChildString += "<td><div class=\"tcle\">  ";
                            if (Level == "1")
                            {
                                ChildString += "<a href='javascript:;' tip='" + model.ID + "' Pname='" +
                                               model.CategoryName +
                                               @"' class='TypeChildAdd'>添加下级</a> | ";
                                if (i == 0)
                                {
                                    ChildString += "<a tip='" + model.ID + "' sortid='" +
                                                   model.SortIndex +
                                                   @"' class='TypeIndex' style='color: #c0c0c0;cursor:not-allowed;'>上移</a> | ";
                                }
                                else
                                {
                                    ChildString += "<a href='javascript:;' tip='" + model.ID + "' sortid='" +
                                                   model.SortIndex +
                                                   @"' class='TypeIndex'>上移</a> | ";
                                }
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    ChildString += "<a tip='" + model.ID + "' sortid='" + model.SortIndex +
                                           @"' class='TypeIndex' style='margin-left: 58px;color: #c0c0c0;cursor:not-allowed;'>上移</a> | ";
                                }
                                else
                                {
                                    ChildString += "<a href='javascript:;' tip='" + model.ID + "' sortid='" + model.SortIndex +
                                           @"' class='TypeIndex' style='margin-left: 58px;'>上移</a> | ";
                                }
                            }
                            if (i == cateList.Count - 1)
                            {
                                ChildString += "<a tip='" + model.ID + "' sortid='" +
                                               model.SortIndex +
                                               @"' class='TypeIndexDown' style='color: #c0c0c0;cursor:not-allowed;'>下移</a> | ";
                            }
                            else
                            {
                                ChildString += "<a href='javascript:;' tip='" + model.ID + "' sortid='" +
                                               model.SortIndex +
                                               @"' class='TypeIndexDown' >下移</a> | ";
                            }
                            ChildString += "<a class='TypeEdit' href='javascript:;' tip='" + model.ID + "' sortid ='" +
                                           model.SortIndex + "' Pname='" + model.CategoryName + @"' >编辑</a> | ";
                            ChildString += "<a href='javascript:;'  tip='" + model.ID + @"'  class='TypeDel'>移除</a></div></td></tr>";
                        }
                    }
                }
            }
        }
        return ChildString;
    }
    
    protected string Simage(string obj)
    {
        string image = "../images/menu_plus.gif";
        if (obj != null)
        {
            List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", " ParentID='" + obj + "' and dr=0 and IsEnabled=1", "");
            if (l != null && l.Count > 0)
            {
                image = "../images/menu_plus.gif";
            }
            else
            {
                image = "../images/menu_minus.gif";
            }
            return image;
        }
        return image;
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}