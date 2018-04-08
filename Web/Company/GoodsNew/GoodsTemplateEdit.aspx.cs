using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public partial class Company_Goods_GoodsTemplateEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "AttrValueShow")
            {
                string id = Request["id"];//属性id
                Response.Write(GetAttrValue(id));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            if (Request["tempId"] == null)//新增
            {
                BindAttribute(1, "");// 属性绑定
            }
            else
            {
                string id = Request["tempId"].ToString();
                if (id != "")
                {
                    Hi.Model.BD_Template model = new Hi.BLL.BD_Template().GetModel(Convert.ToInt32(id));
                    if (model != null)
                    {
                        this.txtTemplate.Value = model.TemplateName;
                        BindAttribute(2, id);// 属性绑定
                    }
                }
            }
        }
    }
    /// <summary>
    /// 根据属性id得到属性值
    /// </summary>
    /// <returns></returns>
    public string GetAttrValue(string str)
    {
        string list = string.Empty;
        List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "AttributeID=" + str + " and ISNULL(dr,0)=0 and ComPid=" + this.CompID + " and isenabled=1", "");
        if (ll.Count > 0)
        {
            DataTable dt2 = Common.FillDataTable<Hi.Model.BD_AttributeValues>(ll);
            if (dt2.Rows.Count != 0)
            {
                return ConvertJson.ToJson(dt2);
            }
        }
        return "[{}]";
    }
    /// <summary>
    /// 属性绑定
    /// </summary>
    private void BindAttribute(int type, string id)
    {
        int y = 0;
        StringBuilder html = new StringBuilder();
        StringBuilder html2 = new StringBuilder();
        html.Append("<ul class=\"ng-scope\">");
        List<Hi.Model.BD_Attribute> ll = new Hi.BLL.BD_Attribute().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID, "");
        if (ll.Count > 0)
        {
            if (type == 2)
            {
                List<Hi.Model.BD_TemplateAttribute> l = new Hi.BLL.BD_TemplateAttribute().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and templateId=" + id, "");

                foreach (Hi.Model.BD_Attribute item in ll)
                {
                    string str = string.Empty;
                    int attrId = 0;
                    if (l.Count > 0)
                    {
                        foreach (Hi.Model.BD_TemplateAttribute item2 in l)
                        {
                            if (item2.AttributeID == item.ID)
                            {
                                attrId = item2.ID;
                                str = "checked=\"checked\"";
                                break;
                            }
                        }
                    }

                    html.Append("<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\" tip=\"" + item.ID + "\"><span class=\"customerManager m-l-10\"><input class=\"ng-pristine ng-untouched ng-valid\" " + str + " type=\"checkbox\" name=\"chekAttr\" value=\"" + item.ID + "\"/></span><span class=\"ng-binding\">" + item.AttributeName + "</span>");
                    if (item.Memo != "")
                    {
                        html.Append("<span class=\"ng-binding\">(" + item.Memo + ")</span>");
                    }
                    html.Append("</li>");
                    List<Hi.Model.BD_AttributeValues> lll = new Hi.BLL.BD_AttributeValues().GetList("", "isnull(dr,0)=0 and isenabled=1 and compId=" + this.CompID + " and attributeId=" + item.ID, "");
                    if (lll.Count > 0)
                    {
                        html2.Append("<ul class=\"ng-scope\" tip=\"" + item.ID + "\" " + (y != 0 ? "style=\"display:none;\"" : "") + ">");
                        foreach (Hi.Model.BD_AttributeValues item2 in lll)
                        {
                            string str2 = string.Empty;
                            if (attrId != 0)
                            {
                                List<Hi.Model.BD_TemplateAttrValue> llll = new Hi.BLL.BD_TemplateAttrValue().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and TemplateAttrID=" + attrId + " and attributevalueId=" + item2.ID, "");
                                if (llll.Count > 0)
                                {
                                    str2 = "checked=\"checked\"";
                                }
                                List<Hi.Model.BD_TemplateAttrValue> lllll = new Hi.BLL.BD_TemplateAttrValue().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and TemplateAttrID=" + attrId, "");
                                if (lllll.Count == 0)
                                {
                                    str2 = "checked=\"checked\"";
                                }
                            }
                            html2.Append("<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\" tip=\"" + item2.ID + "\"><span class=\"customerManager m-l-10\"><input class=\"ng-pristine ng-untouched ng-valid\" " + str2 + " type=\"checkbox\" name=\"chekAttrValue" + item.ID + "\" value=\"" + item2.ID + "\"/></span><span class=\"ng-binding\">" + item2.AttrValue + "</span></li>");
                            y++;
                        }
                        html2.Append("</ul>");
                    }
                  
                }
            }
            else
            {

                foreach (Hi.Model.BD_Attribute item in ll)
                {
                    html.Append("<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\" tip=\"" + item.ID + "\"><span class=\"customerManager m-l-10\"><input class=\"ng-pristine ng-untouched ng-valid\" type=\"checkbox\" name=\"chekAttr\" value=\"" + item.ID + "\"/></span><span class=\"ng-binding\">" + item.AttributeName + "</span>");
                    if (item.Memo != "")
                    {
                        html.Append("<span class=\"ng-binding\">(" + item.Memo + ")</span>");
                    }
                    html.Append("</li>");
                    List<Hi.Model.BD_AttributeValues> l = new Hi.BLL.BD_AttributeValues().GetList("", "isnull(dr,0)=0 and isenabled=1 and compId=" + this.CompID + " and attributeId=" + item.ID, "");
                    if (l.Count > 0)
                    {
                        html2.Append("<ul class=\"ng-scope\" tip=\"" + item.ID + "\" style=\"display:none;\">");
                        foreach (Hi.Model.BD_AttributeValues item2 in l)
                        {
                            html2.Append("<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\" tip=\"" + item2.ID + "\"><span class=\"customerManager m-l-10\"><input class=\"ng-pristine ng-untouched ng-valid\" type=\"checkbox\" name=\"chekAttrValue" + item.ID + "\" value=\"" + item2.ID + "\"/></span><span class=\"ng-binding\">" + item2.AttrValue + "</span></li>");
                        }
                        html2.Append("</ul>");
                    }
                }
            }
        }
        html.Append("</ul>");
        this.divAttr.InnerHtml = html.ToString();
        this.divAttrValue.InnerHtml = html2.ToString();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        try
        {
            Hi.Model.BD_Template model = null;
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            string str = Request["chekAttr"];//属性id List
            string template =Common.NoHTML( this.txtTemplate.Value.Trim());//模板名称
            object obj = Request["tempId"];
            if (obj != null)
            {
                string tempIds = obj.ToString();
                List<Hi.Model.BD_TemplateAttribute> l = new Hi.BLL.BD_TemplateAttribute().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and templateId=" + tempIds, "", Tran);
                if (l.Count > 0)
                {
                    foreach (Hi.Model.BD_TemplateAttribute item in l)
                    {
                        new Hi.BLL.BD_TemplateAttribute().Delete(item.ID, Tran);
                        new Hi.BLL.BD_TemplateAttrValue().Delete(item.ID, Tran);
                    }
                }
                model = new Hi.BLL.BD_Template().GetModel(Convert.ToInt32(tempIds), Tran);
            }

            model = new Hi.Model.BD_Template();
            model.CompID = this.CompID;
            model.CreateDate = DateTime.Now;
            model.CreateUserID = this.UserID;
            model.modifyuser = this.UserID;
            model.TemplateName = template;
            model.TemplateCode = "";
            model.ts = DateTime.Now;
            model.IsEnabled = 1;
            int tempId = 0;
            if (obj != null)
            {
                tempId = Convert.ToInt32(obj.ToString());
                model.ID = tempId;
                new Hi.BLL.BD_Template().Update(model, Tran);
            }
            else
            {
                tempId = new Hi.BLL.BD_Template().Add(model, Tran);
            }
            for (int i = 0; i < str.Split(',').Length; i++)
            {
                Hi.Model.BD_TemplateAttribute model2 = new Hi.Model.BD_TemplateAttribute();
                model2.AttributeID = Convert.ToInt32(str.Split(',')[i]);
                model2.CompID = this.CompID;
                model2.TemplateID = tempId;
                model2.CreateDate = DateTime.Now;
                model2.CreateUserID = this.UserID;
                model2.ts = DateTime.Now;
                model2.modifyuser = this.UserID;
                int TemplateAttrId = new Hi.BLL.BD_TemplateAttribute().Add(model2, Tran);
                string str2 = Request["chekAttrValue" + str.Split(',')[i]];
                for (int x = 0; x < str2.Split(',').Length; x++)
                {
                    Hi.Model.BD_TemplateAttrValue model3 = new Hi.Model.BD_TemplateAttrValue();
                    model3.CompID = this.CompID;
                    model3.AttributeValueID = Convert.ToInt32(str2.Split(',')[x]);
                    model3.TemplateAttrID = TemplateAttrId;
                    model3.CreateDate = DateTime.Now;
                    model3.CreateUserID = this.UserID;
                    model3.ts = DateTime.Now;
                    model3.modifyuser = this.UserID;
                    new Hi.BLL.BD_TemplateAttrValue().Add(model3, Tran);
                }
            }
            Tran.Commit();
            // Response.Write("<script>window.parent.CloseDialog(\"2\");window.opener.location=window.opener.location;</script>");
            this.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>parent.location.reload();window.parent.CloseDialog(\"2\");</script>");
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, "保存失败了", JScript.IconOption.错误);
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
}