<%@ WebHandler Language="C#" Class="GoodsUnit" %>

using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

public class GoodsUnit : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write(GetHtml(context));
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public string GetUnitList(HttpContext context)
    {
        //string res = "<div class=\'pullDown2\' style=\'display: none;\'><ul class=\'list\'>";
        string res = "<select class=\'select\' onchange='UnitAddClass(this)'><option>请选择</option>";
            
        List<Hi.Model.BD_DefDoc> l = new Hi.BLL.BD_DefDoc().GetList("", "AtName='" + "计量单位" + "' and compid=" + context.Request["compID"] + " and isnull(dr,0)=0", "");
        if (l.Count > 0)
        {
            List<Hi.Model.BD_DefDoc_B> ll = new Hi.BLL.BD_DefDoc_B().GetList("", "DefID=" + l[0].ID + "and ISNULL(dr,0)=0 and compid=" + context.Request["compID"], "id desc");
            if (ll.Count > 0)
            {
                //res += "<li><a href='javascript:;'>" + one.AtVal + "</a></li>";
                res = ll.Aggregate(res, (current, one) => current + ("<option>" + one.AtVal + "</option>"));
            }
        }
        res += "<option value='UnitAdd'>+ 新增</option>";
        res += "</select>";
        return res;
    }

    public string GetHtml(HttpContext context)
    {
        string res = "{\"html\":\"<tr>"
                + "  <td class=\'firsttd\'><img src=\'../images/t03.png\' title=\'删除\' style=\'cursor: pointer;\' onclick=\'DelClick(this)\'/></td>"
                + " <td><input name=\'txtGoodsName\' type=\'text\' class=\'textBox\' maxlength=\'30\' onblur='CheckGoodsName(this)'/></td>"
                + "  <td><input name=\'txtPrice\' type=\'text\'  class=\'textBox\' onkeyup=\'KeyInt2(this);\'/></td>"
                + " <td class='tr_one'>" 
                //+ GetUnitList(context)
                + " </td>"
                + " <td><input name=\'txtMeno\' type=\'text\'  class=\'textBox\' maxlength=\'100\'/></td>"
                + " </tr>\",\"select\":\"" + GetUnitList(context) + "\"}";
        return res;
    }

}