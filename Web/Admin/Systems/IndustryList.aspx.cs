
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public partial class Admin_Systems_IndustryList : AdminPageBase
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
            else if (action == "del")
            {
                string typeId = Request["typeId"];
                Response.Write(del(typeId));
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
                  "<td ><div  style=\"margin-left:50px;width:350px\"> <img class=\"Openimg2\" height='9' src='../../Company/images/"+(item.SVdef3 == null ? "menu_minus" : "menu_plus") +".gif' width='9'" +
                  "border='0' />&nbsp;" + item.TypeName + "</div></td><td><div class=\"tcle\">" +
                  "" + (item.IsEnabled ? "启用" : "禁用") + "</div></td>" +
                  "<td><div class=\"tcle\">" +
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' pname='" + item.TypeName + "' class=\"TypeChildAdd\">添加下级 |</a> " +
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' class=\"TypeEdit\"  pname='" + item.TypeName + "'>编辑 |</a>" +
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' class=\"TypeDel\">移除</a> " +
                  "</div></td>  </tr>";
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
                  "<td ><div  style=\"margin-left:80px;width:350px\"> <img class=\"Openimg3\" height='9' src='../../Company/images/" + (item.SVdef3 == null ? "menu_minus" : "menu_plus") + ".gif' width='9'" +
                  "border='0' />&nbsp;" + item.TypeName + "</div></td><td><div class=\"tcle\">" +
                  "" + (item.IsEnabled ? "启用" : "禁用") + "</div></td>"+
                  "<td><div class=\"tcle\">"+
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' pname='" + item.TypeName + "' class=\"TypeChildAdd\">添加下级 |</a> " +
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' class=\"TypeEdit\"  pname='" + item.TypeName + "'>编辑 |</a>" +
                  " <a href=\"javascript:void(0)\" tip='" + item.ID + "' class=\"TypeDel\">移除</a> " +
                  "</div></td>  </tr>";
        }
        return html;
    }

    public string del(string typeId)
    {
        Hi.Model.SYS_GType gType = new Hi.BLL.SYS_GType().GetModel(typeId.Trim().ToInt(0));
        Hi.BLL.SYS_GType BllGtype = new Hi.BLL.SYS_GType();
        if (gType != null)
        {
            if (gType.ParentId > 0)
            {
                gType.ts = DateTime.Now;
                gType.dr = true;

                //重置父级是否末节点
                List<Hi.Model.SYS_GType> oneList = new Hi.BLL.SYS_GType().GetList("top 1 TypeCode", " ParentId = '" + gType.ParentId + "' and dr = 0", "ID desc");
                if (oneList.Count > 0)
                {
                    Hi.Model.SYS_GType PgType = new Hi.BLL.SYS_GType().GetModel(gType.ParentId);
                    PgType.ts = DateTime.Now;
                    PgType.IsEnd = true;
                    BllGtype.Update(PgType);
                }
                BllGtype.Update(gType);

                //同时删除子分类
                List<Hi.Model.SYS_GType> childList = new Hi.BLL.SYS_GType().GetList("", " ParentId = '" + gType.ID + "' and dr = 0", "ID desc");
                foreach (Hi.Model.SYS_GType childType in childList)
                {
                    childType.ts = DateTime.Now;
                    childType.dr = true;
                    BllGtype.Update(childType);
                }

            }
            else
            {
                gType.ts = DateTime.Now;
                gType.dr = true;
                BllGtype.Update(gType);
            }
            return "1";
        }
        else
        {
            return "删除失败，此分类不存在！";
        }
    }

    //新增一级分类
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string Typename = Common.NoHTML(txtTypeName.Value.Trim());
        int Parentid = Common.NoHTML(hidePTypeId.Value.Trim()).ToInt(0);
        Hi.Model.SYS_GType gType = new Hi.Model.SYS_GType();
        gType.CreateDate = DateTime.Now;
        gType.CreateUser = UserID.ToString();
        gType.TypeName = Typename;
        gType.ts = gType.CreateDate;

        string[] codes = NewCategoryCode(Parentid);
        gType.TypeCode = codes[0];
        gType.FullCode = codes[1];
        gType.Deep = codes[2].ToInt(0);
        gType.ParentId = Parentid;

        gType.IsEnd = true;
        gType.IsEnabled = true;
        new Hi.BLL.SYS_GType().Add(gType);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>layerCommon.msg('新增成功！', IconOption.笑脸, 1500);location.href='IndustryList.aspx';</script>");
    }


    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_GType gType = new Hi.BLL.SYS_GType().GetModel(hideTypeId.Value.Trim().ToInt(0));
        if (gType != null)
        {
            gType.TypeName = txtTypeNames.Value.Trim();
            gType.ts = DateTime.Now;
            new Hi.BLL.SYS_GType().Update(gType);
            Response.Redirect("IndustryList.aspx");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('此分类不存在，请确认！', IconOption.错误, 2000);</script>");
        }
    }

    //生成新的分类0： code 和 1：Fullcode  和 2：Deep

    public string[] NewCategoryCode(int pid)
    {
        string[] codes = new string[3];

        if (pid > 0)
        {
            List<Hi.Model.SYS_GType> PList = new Hi.BLL.SYS_GType().GetList("top 1 *", " id = '" + pid + "' and dr = 0", "");
            if (PList.Count > 0)
            {
                codes[2] = (PList[0].Deep + 1).ToString(); //赋值deep级数
                List<Hi.Model.SYS_GType> oneList = new Hi.BLL.SYS_GType().GetList("top 1 TypeCode", " Deep = '" + codes[2] + "' and dr = 0", "ID desc");
                if (oneList.Count > 0)
                {
                    codes[0] = (oneList[0].TypeCode.ToInt()+1).ToString();
                }
                else
                {
                    if (codes[2].ToInt(0) == 2)
                    {
                        codes[0] = "100";
                    }
                    else
                    {
                        codes[0] = "1000";
                    }
                }
                codes[1] = PList[0].FullCode + "-" + codes[0];
                if (PList[0].IsEnd)
                {
                    PList[0].ts = DateTime.Now;
                    PList[0].IsEnd = false;
                    new Hi.BLL.SYS_GType().Update(PList[0]);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('父节点不存在，请确认！', IconOption.哭脸, 2000);</script>");
            }
        }
        else
        {
            codes[2] = "1";
            List<Hi.Model.SYS_GType> oneList = new Hi.BLL.SYS_GType().GetList("top 1 TypeCode", " ParentId = 0", "ID desc");
            if (oneList.Count > 0)
            {
                codes[1] = codes[0] = (oneList[0].TypeCode.ToInt(0) + 1).ToString();
            }
            else
            {
                codes[1] = codes[0] = "10";
            }
        }
        return codes;
    }



}