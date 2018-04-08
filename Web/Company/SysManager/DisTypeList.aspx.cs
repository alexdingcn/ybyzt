using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_DisTypeList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Hi.Model.BD_DisType> list = new Hi.BLL.BD_DisType().GetAllList();
        if (list != null && list.Count > 0)
        {
            foreach (var type in list)
            {
                if (string.IsNullOrEmpty(type.SortIndex) || Convert.ToInt32(type.SortIndex) < 1000)
                {
                    type.SortIndex = NewCateId().ToString();
                    type.ts = DateTime.Now;
                    type.modifyuser = UserID;
                    new Hi.BLL.BD_DisType().Update(type);
                }
            }
        }

        if (!IsPostBack)
        {
            if (Request.Form["Action"] != null)
            {
                if (Request.Form["Action"] == "Del")
                {
                    Response.Write(DelType(Request.Form["Id"]));
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

    public string DelType(string id)
    {
        string json = string.Empty;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_DisType> List = new Hi.BLL.BD_DisType().GetList(null, " CompID=" + CompID + " and isnull(dr,0)=0  and  ParentId=" + id, null);
            if (List.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此类别下还有子级类别，请先删除子级类别!\"}";
            }
            List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList(null, " CompID=" + CompID + " and isnull(dr,0)=0  and   DisTypeID=" + id, null);
            if (Dis.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此分类已被使用，不允许删除!\"}";
            }
            List<Hi.Model.BD_DisPrice> disp = new Hi.BLL.BD_DisPrice().GetList(null, " CompID=" + CompID + " and type=" + 1 + " and( One=" + id + " or two=" + id + " or three=" + id + ") and dr=0", null);
            if (disp.Count > 0)
            {
                return "{\"result\":false,\"code\":\"此区域下有代理商价格，不允许删除!\"}";
            }

            Hi.Model.BD_DisType type = new Hi.BLL.BD_DisType().GetModel(id.ToInt(0));
            type.dr = 1;
            type.ts = DateTime.Now;
            type.modifyuser = UserID;
            if (new Hi.BLL.BD_DisType().Update(type))
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

        List<Hi.Model.BD_DisType> LType = new Hi.BLL.BD_DisType().GetList(Pager.PageSize, Pager.CurrentPageIndex, " Id", false, " and CompID=" + CompID + " and isnull(ParentId,0)=0 and isnull(dr,0)=0 ", out pageCount, out Counts);
        LType = LType.OrderBy(L => L.SortIndex).ToList();
        this.rptDisTypeList.DataSource = LType;
        this.rptDisTypeList.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }

    string ChildString = string.Empty;
    /// <summary>
    /// 查询父类下所有子集
    /// </summary>
    /// <param name="ParentId">父Id</param>
    /// <returns></returns>
    public string FindChild(string ParentId, string Str = "")
    {
        ChildString = Str;
        List<Hi.Model.BD_DisType> LType = new Hi.BLL.BD_DisType().GetList("", "  CompID=" + CompID + " and ParentId='" + ParentId + "' and isnull(dr,0)=0", " sortindex asc");
        foreach (Hi.Model.BD_DisType model in LType)
        {
            ChildString += "<tr id='" + model.ID + "' parentid='" + model.ParentId + "' bgcolor='#fcfeff' style='height: 26px;width: 100%;display: none;'>";
            ChildString += "<td> <div class=\"tcle\"><img id='Openimg' height='9' src='" + Simage(model.ID) + "' width='9' border='0' />&nbsp; <span class='span'>" + model.TypeName + "</span></div></td>";

            ChildString += Str == "" ? "<td><div class=\"tcle\">　<a href='javascript:;' tip='" + model.ID + "' Pname='" + model.TypeName + @"' class='TypeChildAdd'>添加下级</a> | " : "<td><div class=\"tcle\">　";

            ChildString +=  "<a class='TypeEdit' href='javascript:;' tip='" + model.ID + "' sortid ='" + model.SortIndex + "'   Pname='" + model.TypeName + @"' >编辑</a> |
                <a href='javascript:;'  tip='" + model.ID + @"'  class='TypeDel'>移除</a>
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
        string Typename =Common.NoHTML( txtTypeName.Value.Trim());
        string Parentid =Common.NoHTML( hideTypeId.Value.Trim());
        string sortid = NewCateId().ToString();

        //string Typecode = txtTypecode.Value.Trim();
        Hi.Model.BD_DisType DisType = new Hi.Model.BD_DisType();
        int Result = 0;
        if (string.IsNullOrEmpty(Typename))
        {
            JScript.AlertMethod(this, "分类名称不能为空", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (IsExistsType("TypeName", Typename))
        {
            JScript.AlertMethod(this, "此分类名已存在", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (string.IsNullOrEmpty(Parentid) || !int.TryParse(Parentid, out Result))
        {
            DisType.ParentId = 0;
        }
        else
        {
            DisType.ParentId = Result;
        }
        DisType.CompID = CompID;
        DisType.TypeName = Typename;
        //DisType.TypeCode = Typecode;
        DisType.SortIndex = sortid;
        DisType.CreateDate = DateTime.Now;
        DisType.CreateUserID = 0;
        DisType.ts = DateTime.Now;
        DisType.modifyuser = 0;
        if (new Hi.BLL.BD_DisType().Add(DisType) > 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>location.href='DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "&lefttype=" + Request["lefttype"] + "&type=" + Request["type"] + "';</script>");
        }
        else
        {
            JScript.AlertMethod(this, "添加失败", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string Typename =Common.NoHTML( txtTypeNames.Value.Trim());
        string id =Common.NoHTML( hideTypeIds.Value.Trim());
        //string sortid = txtSortIndexs.Value.Trim();
        //string typecode = txtTypecodes.Value.Trim();
        int Result = 0;
        if (string.IsNullOrEmpty(Typename))
        {
            JScript.AlertMethod(this, "分类名称不能为空", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (IsExistsType("TypeName", Typename, id))
        {
            JScript.AlertMethod(this, "此分类名已存在", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }
        if (int.TryParse(id, out Result))
        {
            Hi.Model.BD_DisType DisType = new Hi.BLL.BD_DisType().GetModel(Result);
            if (DisType != null)
            {
                DisType.TypeName = Typename;
                //DisType.SortIndex = sortid;
                //DisType.TypeCode = typecode;
                DisType.ts = DateTime.Now;
                DisType.modifyuser = 0;
                if (new Hi.BLL.BD_DisType().Update(DisType))
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Result2", "<script>location.href='DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "&lefttype=" + Request["lefttype"] + "&type=" + Request["type"] + "';</script>");
            }
            else
            {
                JScript.AlertMethod(this, "此分类不存在", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
                return;
            }
        }
        else
        {
            JScript.AlertMethod(this, "分类ID错误", JScript.IconOption.错误, "function (){ location.replace('" + ("DisTypeList.aspx?page=" + Pager.CurrentPageIndex + "") + "'); }");
            return;
        }

    }

    /// <summary>
    /// 判断属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsExistsType(string name, string value)
    {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_DisType> List = new Hi.BLL.BD_DisType().GetList("", name + "='" + value + "' and CompID=" + CompID + " and isnull(dr,0)=0", "");
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
    public bool IsExistsType(string name, string value, string id)
    {
        bool bfg = false;
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            List<Hi.Model.BD_DisType> List = new Hi.BLL.BD_DisType().GetList("", name + "='" + value + "' and id<>'" + id + "' and CompID=" + CompID + " and isnull(dr,0)=0", "");
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

            List<Hi.Model.BD_DisType> l = new Hi.BLL.BD_DisType().GetList(null, " CompID='" + CompID + "' and isnull(dr,0)=0 and  ParentId=" + obj.ToString(), null);
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

    /// <summary>
    /// 最新 的排序号
    /// </summary>
    /// <returns></returns>
    public static int NewCateId()
    {
        List<int> intList = new List<int>();
        List<Hi.Model.BD_DisType> goodsCategoryList = new Hi.BLL.BD_DisType().GetAllList();
        if (goodsCategoryList != null && goodsCategoryList.Count > 0)
        {
            foreach (var item in goodsCategoryList)
            {
                intList.Add(Convert.ToInt32(string.IsNullOrEmpty(item.SortIndex) ? "0" : item.SortIndex));
            }
            return intList.Max() != 0 ? intList.Max() + 1 : 1000;
        }
        else
        {
            return 1000;
        }
    }
}