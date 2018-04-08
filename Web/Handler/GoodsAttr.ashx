<%@ WebHandler Language="C#" Class="GoodsAttr" %>

using System;
using System.Web;
using System.Collections.Generic;
public class GoodsAttr : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string action = HttpContext.Current.Request["action"];
        if (action == "attr")//分类属性
        {
            string compId = HttpContext.Current.Request["compId"];
            string id = HttpContext.Current.Request["id"];
            string indx = HttpContext.Current.Request["inde"];
            string strAttr = string.Empty;//属性列表
            string strAttrID = string.Empty;//属性列表
            string html = "<table class=\"dh3\">";
            List<Hi.Model.BD_CategoryAttribute> l = new Hi.BLL.BD_CategoryAttribute().GetList("", "categoryid=" + id + " and compid=" + compId + " and ISNULL(dr,0)=0 ", "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_CategoryAttribute item in l)
                {
                    Hi.Model.BD_Attribute item2 = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(item.AttributeID));
                    if (item2 != null)
                    {
                        strAttr += item2.AttributeName + "、";
                        strAttrID += item2.ID + "、";
                        html += string.Format("<tr class=\"tr\"><td>{0}：</td><td>", item2.AttributeName);
                        List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "attributeid=" + item.ID + " and compid=" + compId + " and ISNULL(dr,0)=0 and isenabled=1", "");
                        if (ll.Count > 0)
                        {
                            foreach (Hi.Model.BD_AttributeValues item3 in ll)
                            {
                                html += string.Format("<input id=\"{0}\" name=\"chkList_{2}\" runat=\"server\" value=\"{0}\" type=\"checkbox\" /><label for=\"{0}\">{1}</label>&nbsp;&nbsp;", item3.ID, item3.AttrValue, item2.ID);
                            }
                        }
                        html += "</td></tr>";
                    }
                }
            }
            else
            {
                if (strAttr != "")
                {
                    strAttr = strAttr.Substring(0, strAttr.Length - 1);
                    strAttrID = strAttrID.Substring(0, strAttrID.Length - 1);
                }
                HttpContext.Current.Response.Write(strAttr + "@" + "<label style='color:#aaaaaa'>该商品分类没有绑定属性</label>");
                HttpContext.Current.Response.End();
            }
            html += "</table>";
            if (strAttr != "")
            {
                strAttr = strAttr.Substring(0, strAttr.Length - 1);
                strAttrID = strAttrID.Substring(0, strAttrID.Length - 1);
            }
            if (indx != "" && indx != null)
            {
                html += " <input type=\"hidden\" name=\"hideattr\" value=\"" + strAttrID + "\" id=\"hideattr" + indx + "\" runat=\"server\" />";
            }
            else
            {
                html += " <input type=\"hidden\" name=\"hideattr\" value=\"" + strAttrID + "\" id=\"hideattr\" runat=\"server\" />";
            }
            HttpContext.Current.Response.Write(strAttr + "@" + html);
            HttpContext.Current.Response.End();
        }
        if (action.ToString() == "minCate")
        {//验证是否最小分类
            string id = HttpContext.Current.Request["id"] != null ? HttpContext.Current.Request["id"] : "";
            string compId = HttpContext.Current.Request["compId"];
            string str = string.Empty;
            if (!Util.IsEmpty(id))
            {
                Hi.Model.BD_GoodsCategory model = new Hi.BLL.BD_GoodsCategory().GetModel(Convert.ToInt32(id));
                if (model != null)
                {
                    List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", "parentid=" + model.ID + " and isnull(dr,0)=0 and compid=" + compId, "sortIndex");
                    if (l.Count > 0)
                    {
                        str = "cz";//有子级
                    }
                }
            }
            if (Util.IsEmpty(str))
            {
                string strAttr = string.Empty;//属性列表
                string strAttrID = string.Empty;//属性列表
                string html = "<table class=\"dh3\">";
                List<Hi.Model.BD_CategoryAttribute> l = new Hi.BLL.BD_CategoryAttribute().GetList("", "categoryid=" + id + " and compid=" + compId + " and ISNULL(dr,0)=0 ", "");
                if (l.Count > 0)
                {
                    foreach (Hi.Model.BD_CategoryAttribute item in l)
                    {
                        Hi.Model.BD_Attribute item2 = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(item.AttributeID));
                        if (item2 != null)
                        {
                            strAttr += item2.AttributeName + "、";
                            strAttrID += item2.ID + "、";
                            html += string.Format("<tr class=\"tr\"><td>{0}：</td><td>", item2.AttributeName);
                            List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "attributeid=" + item.ID + " and compid=" + compId + " and ISNULL(dr,0)=0 and isenabled=1", "");
                            if (ll.Count > 0)
                            {
                                foreach (Hi.Model.BD_AttributeValues item3 in ll)
                                {
                                    html += string.Format("<input id=\"{0}\" name=\"chkList_{2}\" runat=\"server\" value=\"{0}\" type=\"checkbox\" /><label for=\"{0}\">{1}</label>&nbsp;&nbsp;", item3.ID, item3.AttrValue, item2.ID);
                                }
                            }
                            html += "</td></tr>";
                        }
                    }
                }
                else
                {
                    if (strAttr != "")
                    {
                        strAttr = strAttr.Substring(0, strAttr.Length - 1);
                        strAttrID = strAttrID.Substring(0, strAttrID.Length - 1);
                    }
                    HttpContext.Current.Response.Write(strAttr + "@" + "<label style='color:#aaaaaa'>该商品分类没有绑定属性</label>");
                    HttpContext.Current.Response.End();
                }
                html += "</table>";
                if (strAttr != "")
                {
                    strAttr = strAttr.Substring(0, strAttr.Length - 1);
                    strAttrID = strAttrID.Substring(0, strAttrID.Length - 1);
                }
                html += " <input type=\"hidden\" name=\"hideattr\" value=\"" + strAttrID + "\" id=\"hideattr\" runat=\"server\" />";
                HttpContext.Current.Response.Write(strAttr + "@" + html);
                HttpContext.Current.Response.End();
            }
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }
        if (action == "attr2")
        {
            string compId = HttpContext.Current.Request["compId"];
            string id = HttpContext.Current.Request["id"];
            string KeyID = HttpContext.Current.Request["keyId"];
            string strAttr = string.Empty;//属性列表
            string strAttrID = string.Empty;//属性列表
            string html = "<table class=\"dh3\">";
            List<Hi.Model.BD_CategoryAttribute> l = new Hi.BLL.BD_CategoryAttribute().GetList("", "categoryid=" + id + " and compid=" + compId + " and ISNULL(dr,0)=0 ", "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_CategoryAttribute item in l)
                {
                    Hi.Model.BD_Attribute item2 = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(item.AttributeID));
                    if (item2 != null)
                    {
                        strAttr += item2.AttributeName + "、";
                        strAttrID += item2.ID + "、";
                        html += string.Format("<tr class=\"tr\"><td>{0}：</td><td>", item2.AttributeName);
                        List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "attributeid=" + item.ID + " and compid=" + compId + " and ISNULL(dr,0)=0 and isenabled=1", "");
                        if (ll.Count > 0)
                        {
                            foreach (Hi.Model.BD_AttributeValues item3 in ll)
                            {
                                string disabled = string.Empty;
                                //edit by hgh  去掉了 BD_GoodsAttrValues 
                                //List<Hi.Model.BD_GoodsAttrValues> llll = new Hi.BLL.BD_GoodsAttrValues().GetList("", "goodsid=" + KeyID + "and isnull(dr,0)=0", "");
                                //if (llll.Count > 0)
                                //{
                                //    foreach (Hi.Model.BD_GoodsAttrValues item4 in llll)
                                //    {
                                //        if (item4.ValuesID == item3.ID)
                                //        {
                                //            disabled += string.Format("checked=\"checked\"");
                                //            disabled += string.Format(" disabled=\"disabled\"");
                                //            break;
                                //        }
                                //    }
                                //}
                                html += string.Format("<input id=\"{0}\" name=\"chkList_{2}\" {3}  value=\"{0}\" type=\"checkbox\" /><label for=\"{0}\">{1}</label>&nbsp;&nbsp;", item3.ID, item3.AttrValue, item2.ID, disabled);
                                html += string.Format("<input id=\"{0}\" name=\"hideList_{1}\" value=\"{0}\" type=\"hidden\" />&nbsp;&nbsp;", item3.ID, item2.ID);
                            }
                        }
                        html += "</td></tr>";
                    }
                }
            }
            html += "</table>";
            if (strAttr != "")
            {
                strAttr = strAttr.Substring(0, strAttr.Length - 1);
                strAttrID = strAttrID.Substring(0, strAttrID.Length - 1);
            }
            html += " <input type=\"hidden\" name=\"hideattr\" value=\"" + strAttrID + "\" id=\"hideattr\" runat=\"server\" />";
            HttpContext.Current.Response.Write(strAttr + "@" + html);
            HttpContext.Current.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}