using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_DisAreaList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Form["Action"] != null) {
                if (Request.Form["Action"] == "Del")
                {
                    Response.Write(DelArea(Request.Form["Id"]));
                    Response.End();
                }
            }
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();

           
        }
    }

    public string DelArea(string id)
    {
        string json = string.Empty;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_DisArea> List = new Hi.BLL.BD_DisArea().GetList(null, " CompanyID=" + CompID + " and isnull(dr,0)=0  and   ParentId=" + id, null);
            if (List.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此类别下还有子级类别，请先删除子级类别!\"}";
            }
            List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList(null, " CompID=" + CompID + " and isnull(dr,0)=0  and   AreaID=" + id, null);
            if (Dis.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此区域已被使用，不允许删除!\"}";
            }
            List<Hi.Model.BD_DisPrice> disp = new Hi.BLL.BD_DisPrice().GetList(null, " CompID="+ CompID + " and type="+2+" and( One="+ id + " or two="+ id + " or three="+ id + ") and dr=0", null);
            if (disp.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此区域下有代理商价格，不允许删除!\"}";
            }
            Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(id.ToInt(0));
            area.dr = 1;
            area.ts = DateTime.Now;
            area.modifyuser = UserID;
            if (new Hi.BLL.BD_DisArea().Update(area))
            {
                return "{\"result\":true,\"code\":\"操作成功\"}";
            }

        }
        return json;
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }

        List<Hi.Model.BD_DisArea> LArea = new Hi.BLL.BD_DisArea().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SortIndex", false, " and CompanyID=" + CompID + " and ParentId=0  and isnull(dr,0)=0 ", out pageCount, out Counts);
        // LArea= LArea.OrderBy(L => L.SortIndex).ToList();
        this.rptDisAreaList.DataSource = LArea;
        this.rptDisAreaList.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    
    }

    string ChildString = string.Empty;
    /// <summary>
    /// 查询父类下所有子集
    /// </summary>
    /// <param name="ParentId">父Id</param>
    /// <returns></returns>
    public string FindChild(string ParentId,string Str="")
    {
        ChildString = Str;
        List<Hi.Model.BD_DisArea> LArea = new Hi.BLL.BD_DisArea().GetList("", "  CompanyID=" + CompID + " and ParentId='" + ParentId + "' and isnull(dr,0)=0", " sortindex asc");
        foreach (Hi.Model.BD_DisArea model in LArea)
        {
            ChildString += "<tr id='" + model.ID + "' parentid='" + model.ParentID + "' bgcolor='#fcfeff' style='height: 26px;width: 100%;display: none;'>";
            ChildString += "<td><div class=\"tcle\"><img id='Openimg' height='9' src='" + Simage(model.ID) + "' width='9' border='0' />&nbsp; <span class='span'>" + model.AreaName + "</span></div></td>";
            //ChildString += "<td>" + model.Areacode + "</td>";
            ChildString += "<td><div class=\"tcle\"> " + model.SortIndex + "</div></td>";

            ChildString += Str == "" ? "<td><div class=\"tcle\">　<a href='javascript:;' tip='" + model.ID + "' Pname='" + model.AreaName + @"' class='AreaChildAdd'>添加下级</a> |" : "<td><div class=\"tcle\">　";

            ChildString += "<a class='AreaEdit' href='javascript:;' tip='" + model.ID + "' sortid ='" + model.SortIndex + "'   Pname='" + model.AreaName + @"' >编辑</a> |
                <a href='javascript:;'  tip='" + model.ID + @"'  class='AreaDel'>移除</a>
                </div></td>";
            ChildString += "</tr>";
            FindChild(model.ID.ToString(), ChildString);
        }
        return ChildString;
    }



    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        DataBinds();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataBinds();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string Areaname =Common.NoHTML( txtAreaName.Value.Trim());
        string Parentid =Common.NoHTML( hideAreaId.Value.Trim());
        string sortid =Common.NoHTML( txtSortIndex.Value.Trim());
        //string areacode = txtAreacode.Value.Trim();
        Hi.Model.BD_DisArea DisArea = new Hi.Model.BD_DisArea();
        int Result = 0;
        if (string.IsNullOrEmpty(Areaname))
        {
            JScript.AlertMethod(this, "分类名称不能为空", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (IsExistsArea("AreaName", Areaname)) {
            JScript.AlertMethod(this, "此分类名已存在！", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (string.IsNullOrEmpty(Parentid) || !int.TryParse(Parentid, out Result))
        {
            DisArea.ParentID = 0;
        }
        else
        {
            DisArea.ParentID = Result;
        }
        DisArea.CompanyID = CompID;
        DisArea.AreaName = Areaname;
        //DisArea.Areacode = areacode;
        DisArea.SortIndex = sortid;
        DisArea.ts = DateTime.Now;
        DisArea.modifyuser = 0;
        if (new Hi.BLL.BD_DisArea().Add(DisArea) > 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>location.href='DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "&lefttype=" + Request["lefttype"] + "&type=" + Request["type"] + "';</script>");
        }
        else
        {
            JScript.AlertMethod(this, "添加失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string Areaname =Common.NoHTML( txtAreaNames.Value.Trim());
        string id =Common.NoHTML( hideAreaIds.Value.Trim());
        string sortid =Common.NoHTML( txtSortIndexs.Value.Trim());
        //string areacode = txtAreacodes.Value.Trim();
        int Result = 0;
        if (string.IsNullOrEmpty(Areaname))
        {
            JScript.AlertMethod(this, "分类名称不能为空", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (IsExistsArea("AreaName", Areaname, id))
        {
            JScript.AlertMethod(this, "此分类名已存在！", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (int.TryParse(id, out Result))
        {
            Hi.Model.BD_DisArea DisArea = new Hi.BLL.BD_DisArea().GetModel(Result);
            if (DisArea != null)
            {
                DisArea.AreaName = Areaname;
                DisArea.SortIndex  = sortid;
                //DisArea.Areacode = areacode;
                DisArea.ts = DateTime.Now;
                DisArea.modifyuser = 0;
                if (new Hi.BLL.BD_DisArea().Update(DisArea))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>location.href='DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "&lefttype=" + Request["lefttype"] + "&type=" + Request["type"] + "';</script>");
            }
            else
            {
                JScript.AlertMethod(this, "此分类不存在！", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
                return;
            }
        }
        else
        {
            JScript.AlertMethod(this, "分类ID错误！", JScript.IconOption.错误, "function (){ location.replace('" + ("DisAreaList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }

    }

    /// <summary>
    /// 判断属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExistsArea(string name,string value) {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_DisArea> List = new Hi.BLL.BD_DisArea().GetList("", name + "='" + value + "' and isnull(dr,0)=0 and CompanyID=" + CompID + "", "");
            if (List.Count > 0)
            {
                bfg = true;
            }
        }
        return bfg;
    }

    /// <summary>
    /// 判断属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExistsArea(string name, string value,string id)
    {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_DisArea> List = new Hi.BLL.BD_DisArea().GetList("", name + "='" + value + "' and id<>'" + id + "' and isnull(dr,0)=0 and CompanyID=" + CompID + "", "");
            if (List.Count > 0)
            {
                bfg = true;
            }
        }
        return bfg;
    }

    protected string Simage(object obj)
    {
        string image = "../images/menu_plus.gif";
        if (obj != null)
        {

            List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList(null, " CompanyID='" + CompID + "' and isnull(dr,0)=0 and  ParentId=" + obj.ToString(), null);
            if (l.Count > 0)
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
}