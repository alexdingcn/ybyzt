
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public partial class Admin_Systems_IndustryList : CompPageBase
{

    public int Count = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/html";
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "one")
            {
                string ParentId = Request["ParentId"];
                Response.Write(one(ParentId));
                Response.End();
            }
            else if (action == "two")
            {
                string ParentId = Request["ParentId"];
                Response.Write(two(ParentId));
                Response.End();
            }

        }

        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        List<Hi.Model.SYS_GType> GtypeList = new Hi.BLL.SYS_GType().GetList("ID,ParentId,TypeName,IsEnabled,(select top 1 '1'  from SYS_GType sg where sg.ParentId=SYS_GType.ID )SVdef3", "  dr= 0 and IsEnabled = 1 and ParentId=0", "");
        this.rptGTypeList.DataSource = GtypeList;
        this.rptGTypeList.DataBind();
    }

    public void btn_SearchClick(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0";
        return where;
    }

    /// <summary>
    /// 查询一级分类下的子类
    /// </summary>
    /// <param name="ParentId">一级分类ID</param>
    /// <returns></returns>
    public string one(string ParentId)
    {

        List<Hi.Model.SYS_GType> GtypeList = new Hi.BLL.SYS_GType().GetList("ID,ParentId,TypeName,IsEnabled,(select top 1 '1'  from SYS_GType sg where sg.ParentId=SYS_GType.ID )SVdef3", "  dr= 0 and IsEnabled = 1 and ParentId=" + ParentId + "", "");
        string html = "";
        foreach (var item in GtypeList)
        {
            html += "<tr id = '" + item.ID + "' class=\"tr" + item.ParentId + " tr2\" parentid = '" + item.ParentId + "' style = 'height: 26px;width: 100%;' >" +
                  "<td ><div  style=\"margin-left:50px;width:150px\"> <img class=\"Openimg2\" height='9' src='../../Company/images/"+(item.SVdef3 == null ? "menu_minus" : "menu_plus") +".gif' width='9'" +
                  "border='0' />&nbsp;" + item.TypeName + "</div></td><td><div class=\"tcle\">" +
                  "" + (item.IsEnabled ? "启用" : "禁用") + "</div></td> </tr>";
        }
        return html;
    }
    /// <summary>
    /// 查询二级分类下的子类
    /// </summary>
    /// <param name="ParentId">二级分类ID</param>
    /// <returns></returns>
    public string two(string ParentId)
    {

        List<Hi.Model.SYS_GType> GtypeList = new Hi.BLL.SYS_GType().GetList("ID,ParentId,TypeName,IsEnabled,(select top 1 '1'  from SYS_GType sg where sg.ParentId=SYS_GType.ID )SVdef3", "  dr= 0 and IsEnabled = 1 and ParentId=" + ParentId + "", "");
        string html = "";
        foreach (var item in GtypeList)
        {
            html += "<tr id = '" + item.ID + "' class=\"tr" + item.ParentId + " tr3\" parentid = '" + item.ParentId + "' style = 'height: 26px;width: 100%;' >" +
                  "<td ><div  style=\"margin-left:80px;width:150px\"> <img class=\"Openimg3\" height='9' src='../../Company/images/" + (item.SVdef3 == null ? "menu_minus" : "menu_plus") + ".gif' width='9'" +
                  "border='0' />&nbsp;" + item.TypeName + "</div></td><td><div class=\"tcle\">" +
                  "" + (item.IsEnabled ? "启用" : "禁用") + "</div></td></tr>";
        }
        return html;
    }

  





}