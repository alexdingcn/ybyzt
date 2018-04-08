否<%@ WebHandler Language="C#" Class="GoodsEdit" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using DBUtility;
using System.IO;
using Newtonsoft.Json;
public class GoodsEdit : System.Web.SessionState.IRequiresSessionState, IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";
        string ActionType = context.Request["action"] + "";
        switch (ActionType)
        {
            case "addUnit":
                context.Response.Write(AddUnit(context));
                context.Response.End();
                break;
            case "onchange":
                context.Response.Write(GetTemplate(context));
                context.Response.End();
                break;
            case "delImg":
                string filepath = context.Request["filepath"];//图片名称
                string type = context.Request["type"];
                context.Response.Write(DelImg(filepath, type));
                context.Response.End();
                break;
            case "isChkCode":
                context.Response.Write(IsChkCode(context));
                context.Response.End();
                break;
            case "yanz":
                context.Response.Write(Yanz(context));
                context.Response.End();
                break;
            case "isDelAttrInfo":
                context.Response.Write(isDelAttrInfo(context));
                context.Response.End();
                break;
            case "isChk":
                context.Response.Write(isChk(context));
                context.Response.End();
                break;
            case "isDelAttr":
                context.Response.Write(isDelAttr(context));
                context.Response.End();
                break;

            case "isAddattr":
                context.Response.Write(isAddattr(context));
                context.Response.End();
                break;
        }
    }
    /// <summary>
    /// 是否可以添加规格
    /// </summary>
    /// <returns></returns>
    public string isAddattr(HttpContext context)
    {
        string goodsId = context.Request["goodsId"];
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + goodsId, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsInfo item in l)
                {
                    DataTable dt = new Hi.BLL.DIS_Order().GetList("b.GoodsinfoID", "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID", "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + logUser.CompID + " and b.goodsinfoid=" + item.ID, "");
                    if (dt.Rows.Count > 0)
                    {
                        return "ycz";
                    }
                }
            }
            return "bcz";
        }
        else
        {
            return "请先登录";
        }
    }
    /// <summary>
    /// 是否改删除规格
    /// </summary>
    /// <returns></returns>
    public string isDelAttr(HttpContext context)
    {
        string attrinfo = context.Request["attr"];//规格
        string goodsId = context.Request["goodsId"];
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + goodsId, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsInfo item in l)
                {
                    string valueinfo = item.ValueInfo;//sku
                    if (!Util.IsEmpty(valueinfo))
                    {
                        string[] valinfolist = valueinfo.Split('；');//根据；分隔取出数据
                        for (int i = 0; i < valinfolist.Length; i++)
                        {
                            if (valinfolist[i] != "")
                            {
                                string[] valinfolist2 = valinfolist[i].Split(':');//颜色：白色
                                if (valinfolist2[0] == attrinfo)
                                {//取出对应的goodsinfoid 去订单里面查询是否存在
                                    DataTable dt = new Hi.BLL.DIS_Order().GetList("b.GoodsinfoID", "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID", "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + logUser.CompID + " and b.goodsinfoid=" + item.ID, "");
                                    if (dt.Rows.Count > 0)
                                    {
                                        return "已有订单存在该规格商品，不能直接删除";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }
        else
        {
            return "请先登录";
        }
    }

    /// <summary>
    /// 检查是否可以修改多属性
    /// </summary>
    /// <returns></returns>
    public string isChk(HttpContext context)
    {
        string goodsId = context.Request["goodsId"];
        if (goodsId != "0")
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + goodsId, "");
                foreach (Hi.Model.BD_GoodsInfo item in l)
                {
                    DataTable dt = new Hi.BLL.DIS_Order().GetList("b.GoodsinfoID", "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID", "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + logUser.CompID + " and b.goodsinfoid=" + item.ID, "");
                    if (dt.Rows.Count > 0)
                    {
                        return "已有订单存在该商品，不能设置多规格";
                    }
                }
            }
            else
            {
                return "请先登录";
            }
        }
        return "";
    }
    /// <summary>
    /// 验证是否可以删除该属性
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string isDelAttrInfo(HttpContext context)
    {
        string attrinfo = context.Request["attrinfo"];//属性值
        string goodsId = context.Request["goodsId"];
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + goodsId, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsInfo item in l)
                {
                    string valueinfo = item.ValueInfo;//sku
                    if (!Util.IsEmpty(valueinfo))
                    {
                        string[] valinfolist = valueinfo.Split('；');//根据；分隔取出数据
                        for (int i = 0; i < valinfolist.Length; i++)
                        {
                            if (valinfolist[i] != "")
                            {
                                string[] valinfolist2 = valinfolist[i].Split(':');//颜色：白色
                                if (valinfolist2[1] == attrinfo)
                                {//取出对应的goodsinfoid 去订单里面查询是否存在
                                    DataTable dt = new Hi.BLL.DIS_Order().GetList("b.GoodsinfoID", "DIS_Order as a left join DIS_OrderDetail as b on a.ID=b.OrderID", "isnull(a.dr,0)=0 and isnull(b.dr,0)=0  and a.CompID=" + logUser.CompID + " and b.goodsinfoid=" + item.ID, "");
                                    if (dt.Rows.Count > 0)
                                    {
                                        return "已有订单存在该属性商品，不能直接删除";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }
        else
        {
            return "请先登录";
        }
    }

    /// <summary>
    /// 验证商品名称是否重复
    /// </summary>
    /// <param name="goodsname"></param>
    /// <returns></returns>
    public string Yanz(HttpContext context)
    {
        string goodsname = context.Request["goodsname"];
        string goodsId = context.Request["goodsId"];
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            if (goodsId != "0")
            {
                // Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(goodsInfoId));
                // if (model != null)
                // {
                List<Hi.Model.BD_Goods> count = new Hi.BLL.BD_Goods().GetList("", "goodsname='" + goodsname.Trim() + "' and isnull(dr,0)=0  and compid=" + logUser.CompID + " and id<>" + goodsId, "");
                if (count.Count > 0)
                {
                    return "1";// JScript.AlertMsg(this, "商品编号已存在");
                }
                else
                {
                    return "0";
                }
                //}
                // return "0";
            }
            else
            {

                List<Hi.Model.BD_Goods> count = new Hi.BLL.BD_Goods().GetList("", "goodsname='" + goodsname.Trim() + "' and isnull(dr,0)=0  and compid=" + logUser.CompID, "");
                if (count.Count > 0)
                {
                    return "1";// JScript.AlertMsg(this, "商品编号已存在");
                }
                else
                {
                    return "0";
                }
            }
        }
        else
        {
            return "2";
        }
    }
    /// <summary>
    /// 检验商品编码是否重复
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public string IsChkCode(HttpContext context)
    {
        string str = string.Empty;
        string strcode = context.Request["strcode"];
        object id = context.Request["keyId"];
        List<Hi.Model.BD_GoodsInfo> l = null;
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            string[] code = strcode.Substring(0, strcode.Length - 1).Split(',');
            for (int i = 0; i < code.Length; i++)
            {
                if (id != null && id.ToString() != "")
                {
                    l = new Hi.BLL.BD_GoodsInfo().GetList("", "goodsid!=" + id.ToString() + " and isnull(dr,0)=0 and compId=" + logUser.CompID + " and barcode='" + code[i] + "'", "");
                }
                else
                {
                    l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compId=" + logUser.CompID + " and barcode='" + code[i] + "'", "");
                }
                if (l.Count > 0)
                {
                    str = code[i];
                    break;
                }
            }
        }

        return str;
    }


    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public string DelImg(string filepath, string type)
    {
        try
        {
            string path = Common.GetWebConfigKey("ImgPath");
            if (!string.IsNullOrEmpty(filepath))
            {
                if (type == "0")
                {
                    string path2 = path + "GoodsImg/D" + filepath;
                    if (File.Exists(path2))
                    {
                        File.Delete(path2);
                    }
                    string path3 = path + "GoodsImg/X" + filepath;
                    if (File.Exists(path3))
                    {
                        File.Delete(path3);
                    }
                    string path4 = path + "GoodsImg/" + filepath;
                    if (File.Exists(path4))
                    {
                        File.Delete(path4);
                    }
                }
            }
            return "cg";
        }
        catch (Exception)
        {

            return "";
        }
    }

    /// <summary>
    /// 获取模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTemplate(HttpContext context)
    {
        string id = context.Request["id"];
        DataTable dt = null;
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            string sql = @"select COUNT(*) from BD_Template as a, BD_TemplateAttribute as b,BD_TemplateAttrValue as c
where a.ID=b.TemplateID and c.TemplateAttrID=b.ID and a.CompID=" + logUser.CompID + " and c.CompID=" + logUser.CompID + " and b.CompID=" + logUser.CompID + " and a.ID=" + id;
            object obj = SqlHelper.GetSingle(SqlHelper.LocalSqlServer, sql);
            if (obj != null)
            {
                if (obj.ToString() == "0")
                {
                    dt = GetIDByname("DISTINCT ID,AttributeName,STUFF((SELECT ','+AttrValue   FROM (select a.ID, AttributeName,AttrValue from BD_TemplateAttribute as a left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID  where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0  and a.CompID=" + logUser.CompID + " and b.CompID=" + logUser.CompID + " and c.CompID=" + logUser.CompID + " and a.TemplateID=" + id + ") as B WHERE b.AttributeName = A.AttributeName FOR XML PATH('')),1,1,'')AS AttrValue", "(select a.ID,AttributeName,AttrValue from BD_TemplateAttribute as a  left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and a.CompID=" + logUser.CompID + " and b.CompID=" + logUser.CompID + " and c.CompID=" + logUser.CompID + " and a.TemplateID=" + id + ") AS A", "1=1 order by a.ID ");
                }
                else
                {
                    dt = GetIDByname("DISTINCT ID,AttributeName,STUFF((SELECT ','+AttrValue   FROM (select a.ID, AttributeName,AttrValue from BD_TemplateAttribute as a left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID   left join BD_TemplateAttrValue as d on d.TemplateAttrID=a.ID and d.AttributeValueID=c.id where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and ISNULL(d.dr,0)=0 and d.CompID=" + logUser.CompID + "  and a.CompID=" + logUser.CompID + " and b.CompID=" + logUser.CompID + " and c.CompID=" + logUser.CompID + " and a.TemplateID=" + id + ") as B WHERE b.AttributeName = A.AttributeName FOR XML PATH('')),1,1,'')AS AttrValue", "(select a.ID,AttributeName,AttrValue from BD_TemplateAttribute as a  left join BD_Attribute as b on a.AttributeID=b.ID left join BD_AttributeValues as c on c.AttributeID=b.ID where ISNULL(a.dr,0)=0 and  ISNULL(b.dr,0)=0 and  ISNULL(c.dr,0)=0 and a.CompID=" + logUser.CompID + " and b.CompID=" + logUser.CompID + " and c.CompID=" + logUser.CompID + " and a.TemplateID=" + id + ") AS A", "1=1 order by a.ID ");
                }
            }
            StringBuilder html = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string attrN = dt.Rows[i]["AttributeName"].ToString().Trim();
                string attrV = dt.Rows[i]["AttrValue"].ToString().Trim();


                html.Append("<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\" style=\"width:150px;height: auto;min-height:30px\"><input type=\"text\" value=\"" + attrN + "\" maxlength=\"4\"  placeholder=\"规格名称(4字内) \" class=\"ui-input-dashed mulSpecName box2\" name=\"mulSpecName\" style=\"height: auto;min-height:30px\"/></div><a class=\"delMulSpec\" style=\"display:none\"></a> <div class=\"mulSpecValues\" style=\"width:680px;\" ><input type=\"text\" style=\"display: none;\" class=\"mulSpecInp selectized\" name=\"selectized\" tabindex=\"-1\" value=\"" + attrV.Replace(",", "@@") + "\" maxlength=\"15\"/><div class=\"selectize-control mulSpecInp multi plugin-remove_button\"><div class=\"selectize-input items not-full box1 fl\" style=\"width:720px;height: auto;min-height:30px\" placeholder=\"使用键盘“回车键”确认并添加多个规格值\">");
                //html.Append("<div class=\"mulSpecItem\"><div class=\"mulSpecProperty\"><input type=\"text\" class=\"ui-input-dashed mulSpecName\" style=\"width: 96px;\" maxlength=\"20\"  value=\"" + attrN + "\" name=\"mulSpecName\"></div><a class=\"delMulSpec\"></a><div class=\"mulSpecValues\"><input type=\"text\" value=\"" + attrV.Replace(",", "@@") + "\" tabindex=\"-1\" class=\"mulSpecInp selectized\" name=\"selectized\" style=\"display: none;\"><div class=\"selectize-control mulSpecInp multi plugin-remove_button\"><div class=\"selectize-input items not-full\">");
                for (int z = 0; z < attrV.Split(',').Length; z++)
                {
                    if (attrV.Split(',')[z].Trim() != "")
                    {
                        html.Append("<i class=\"o-t item\" data-value=\"" + attrV.Split(',')[z].Trim() + "\">" + attrV.Split(',')[z].Trim() + "<i  tabindex=\"-1\" class=\"remove close\"></i></i>");
                          
                    }
                }
                html.Append("<input type=\"text\" autocomplete=\"off\" tabindex=\"\" style=\"width: 4px; float:left;\" maxlength=\"15\"/><i class=\"del-i del-i-a\"></i></div><div class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\" style=\"display: none;\"><div class=\"selectize-dropdown-content\"></div> </div> </div></div> <div class=\"cb\"></div> </div>");
                //html.Append("<input type=\"text\" style=\"width: 4px; float:left;\" tabindex=\"\" autocomplete=\"off\"></div><div style=\"display: none;\" class=\"selectize-dropdown multi mulSpecInp plugin-remove_button\"><div class=\"selectize-dropdown-content\"></div></div></div></div><div class=\"cb\"></div></div>");

            }
            return html.ToString() + "@@@" + dt.Rows.Count;
        }
        else
        {
            return "@@@";
        }
    }
    /// <summary>
    /// 根据name找到对应ID
    /// </summary>
    /// <param name="clounms">列名</param>
    /// <param name="table">表名</param>
    /// <param name="wheres">条件</param> 
    /// <returns>Id值</returns>
    public DataTable GetIDByname(string clounms, string table, string wheres)
    {
        DataTable dt = null;
        try
        {
            Hi.BLL.PAY_PrePayment prebll = new Hi.BLL.PAY_PrePayment();
            dt = prebll.GetDate(clounms, table, wheres);//"id","BD_Goods",""
            return dt;
        }
        catch
        {
            dt = null;
        }
        return dt;

    }
    /// <summary>
    /// 添加计量单位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public string AddUnit(HttpContext context)
    {
        string unit = context.Request["unit"];
        SqlTransaction Tran = null;
        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser != null)
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                //新增数据字典
                Hi.Model.BD_DefDoc doc = new Hi.Model.BD_DefDoc();
                doc.CompID = logUser.CompID;
                doc.AtCode = "";
                doc.AtName = "计量单位";
                doc.ts = DateTime.Now;
                doc.modifyuser = logUser.UserID;
                doc.dr = 0;
                List<Hi.Model.BD_DefDoc> ll = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and atname='计量单位'", "");
                int defid = 0;
                if (ll.Count == 0)
                {
                    defid = new Hi.BLL.BD_DefDoc().Add(doc, Tran);
                }
                else
                {
                    defid = ll[0].ID;
                }
                if (defid != 0)
                {
                    Hi.Model.BD_DefDoc_B doc2 = new Hi.Model.BD_DefDoc_B();
                    doc2.CompID = logUser.CompID;
                    doc2.DefID = defid;
                    doc2.AtName = "计量单位";
                    doc2.AtVal = unit; //txtunits.Value.Trim();
                    doc2.ts = DateTime.Now;
                    doc2.dr = 0;
                    doc2.modifyuser = logUser.UserID;
                    List<Hi.Model.BD_DefDoc_B> lll = new Hi.BLL.BD_DefDoc_B().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and atname='计量单位' and defid=" + defid + " and atval='" + unit + "'", "");
                    if (lll.Count == 0)
                    {
                        new Hi.BLL.BD_DefDoc_B().Add(doc2, Tran);
                    }
                    else
                    {
                        //JScript.AlertMsg(this, "计量单位已存在");
                        return "[{\"AtVal\":\"ycz\"}]";
                    }
                }
                Tran.Commit();
                // Common.BindUnit(this.rptUnit, "计量单位", this.CompID);//绑定单位下拉
                List<Hi.Model.BD_DefDoc> l = new Hi.BLL.BD_DefDoc().GetList("", "AtName='计量单位' and compid=" + logUser.CompID + " and isnull(dr,0)=0", "");
                if (l.Count > 0)
                {
                    List<Hi.Model.BD_DefDoc_B> llll = new Hi.BLL.BD_DefDoc_B().GetList("", "DefID=" + l[0].ID + "and ISNULL(dr,0)=0 and compid=" + logUser.CompID, "id desc");
                    if (llll.Count > 0)
                    {
                        System.Data.DataTable dt = Common.FillDataTable(llll);
                        if (dt.Rows.Count != 0)
                        {
                            return ConvertJson.ToJson(dt);
                        }
                    }
                }

                return "[{\"AtVal\":\"cc\"}]";
            }
            else
            {
                return "[{\"AtVal\":\"sb\"}]";
            }
            // this.txtunit.Value = unit;
        }
        catch (Exception)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            return "[{\"AtVal\":\"sb\"}]";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
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
