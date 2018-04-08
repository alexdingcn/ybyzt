using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using DBUtility;
using NPOI.SS.Formula.Functions;

public partial class Company_TreeDemo : System.Web.UI.Page
{
    public string type = string.Empty;
    public string disID = string.Empty;
    public int CompID = 0;
    public int IsExepand = 1;
    public string WriteHTML = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if((HttpContext.Current.Session["UserModel"] as LoginModel)!=null)
            CompID = (HttpContext.Current.Session["UserModel"] as LoginModel).CompID;

        object obj2 = Request["action"];
        if (obj2 != null)
        {
            if (obj2.ToString() == "yanz")//验证是否还存在下级分类
            {
                int id = 0;
                if (Request["id"] != null)
                    id = Convert.ToInt32(Request["id"]);
                Response.Write(GetMinCategory(id));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            object obj = Request.QueryString["type"];
            if (obj != null)
            {
                type = obj.ToString();
                if (!Util.IsEmpty(type))
                {
                    if (Request.QueryString["disId"] != null)
                        disID = Request.QueryString["disId"];
                    else disID = "0";

                    if (type == "1")//商品分类
                    {
                       IsExepand = Common.RsertFolding(type, CompID);
                        string strwhere=string.Empty;
                        List<Hi.Model.SYS_GType> FindList = new List<Hi.Model.SYS_GType>();
                        List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("id,TypeName,ParentId", "isnull(dr,0)=0 and isenabled=1 ", " id");
                        if (string.IsNullOrWhiteSpace(Request["val"]))
                        {
                            FindList = l.Where(T => T.ParentId == 0).ToList();
                        }
                        else
                        {
                            string SearchValue = Request["val"].Replace("'", "''");
                            List<Hi.Model.SYS_GType> ParentList = new List<Hi.Model.SYS_GType>();
                            strwhere += " isnull(dr,0)=0 and isenabled=1 and TypeName like '%" + SearchValue + "%'";
                            FindList = new Hi.BLL.SYS_GType().GetList("id,TypeName,ParentId", strwhere, " id");
                            foreach (Hi.Model.SYS_GType model in FindList)
                            {
                                //string NewName = model.CategoryName.Replace(SearchValue, "<i style='color:red'>" + SearchValue + "</i>");
                                string NewName = model.TypeName;
                                var ParentModel = l.Where(T => T.ID == model.ParentId).ToList();
                                if (ParentModel.Count > 0)
                                {
                                    NewName = ParentModel[0].TypeName + " > " + NewName;
                                }
                                model.TypeName = NewName;
                                ParentList.Add(model);
                            }
                            FindList = ParentList;
                        }
                        if (l.Count > 0 && FindList.Count>0)
                        {
                            BindHTML(l, FindList, type);
                        }
                    }
                    else if (type == "2")//代理商区域
                    {
                        List<Hi.Model.BD_DisArea> FindList = new List<Hi.Model.BD_DisArea>();
                        List<Hi.Model.BD_DisArea> ll = new Hi.BLL.BD_DisArea().GetList("id,AreaName,ParentId", "CompanyID=" + this.CompID + " and isnull(dr,0)=0 ", "");
                        FindList = ll.Where(T => T.ParentID == 0).ToList();
                        if (ll.Count > 0 && FindList.Count > 0)
                        {
                            BindHTML(ll, FindList, type);
                        }
                    }
                    else if (type == "3")//代理商分类
                    {
                        List<Hi.Model.BD_DisType> FindList = new List<Hi.Model.BD_DisType>();
                        List<Hi.Model.BD_DisType> lll = new Hi.BLL.BD_DisType().GetList("id,ParentId,TypeName", "compid=" + this.CompID + " and isnull(dr,0)=0 ", "");
                        FindList = lll.Where(T => T.ParentId == 0).ToList();
                        if (lll.Count > 0 && FindList.Count > 0)
                        {
                            BindHTML(lll, FindList, type);
                        }
                    }
                    else if (type == "4")//代理商地址
                    {
                        List<Hi.Model.BD_DisAddr> llll = new Hi.BLL.BD_DisAddr().GetList("id,Address", "disid=" + disID + " and isnull(dr,0)=0", "");
                        if (llll.Count > 0)
                        {
                            BindHTML(llll, llll, type);
                        }
                    }
                    else if (type == "5")
                    {//代理商管理员
                        List<Hi.Model.BD_Distributor> lllll = new Hi.BLL.BD_Distributor().GetList("id,DisName", "compid=" + this.CompID + " and isnull(dr,0)=0", "Id desc");
                        if (lllll.Count > 0)
                        {
                            BindHTML(lllll, lllll, type);
                        }
                    }
                    else if (type == "6")
                    {//代理商管理员
                        List<Hi.Model.BD_Distributor> lllll = new Hi.BLL.BD_Distributor().GetList("id,DisName", "compid=" + this.CompID + " and isnull(dr,0)=0 and AuditState=2 and IsEnabled=1", "Id asc");
                        if (lllll.Count > 0)
                        {
                            BindHTML(lllll, lllll, type);
                        }
                    }
                }
            }
        }
    }

    public void BindHTML<T>(List<T> Alllist, List<T> FindList, string GetType) where T : class
    {
        try
        {
            string HTML = "";

            string ParentIDN = "ParentId";
            if (GetType == "2")
            {
                ParentIDN = "ParentID";
            }
            bool IsFind = (GetType == "1" || GetType == "2" || GetType == "3");
            foreach (T model in FindList)
            {
                var List2 = new List<T>();
                if (IsFind)
                {
                    List2 = Alllist.Where(H => H.GetType().GetProperty(ParentIDN).GetValue(H, null).ToString() == model.GetType().GetProperty("ID").GetValue(model, null).ToString()).ToList();
                }
                HTML += "<div class='itemTitle cur'> <a href='javascript:;' tip='" + model.GetType().GetProperty("ID").GetValue(model, null).ToString() + "'> " + model.GetType().GetProperty(GetName(GetType)).GetValue(model, null).ToString() + "</a> <i id='treeClass' class='" + (IsExepand == 0 ? "pdaIcon pdjIcon" : (List2.Count > 0 ? "pdaIcon" : "pdaIcon pdjIcon")) + "'></i> </div>";
                if (IsFind)
                {
                    if (List2.Count > 0)
                    {
                        HTML += "<div>";
                        foreach (T model2 in List2)
                        {
                            HTML += " <ul class='itemList " + (IsExepand == 0 ? "" : "none") + "'> <li class='item'><div class='title'> <a href='javascript:;' tip='" + model2.GetType().GetProperty("ID").GetValue(model2, null).ToString() + "'>" + model2.GetType().GetProperty(GetName(GetType)).GetValue(model2, null).ToString() + "</a><i class='lineIcon'></i></div>";
                            var List3 = Alllist.Where(H => H.GetType().GetProperty(ParentIDN).GetValue(H, null).ToString() == model2.GetType().GetProperty("ID").GetValue(model2, null).ToString()).ToList();
                            if (List3.Count > 0)
                            {
                                HTML += "<ul class='itemCon'>   ";
                                foreach (T model3 in List3)
                                {
                                    HTML += "<li class=''> <a href='javascript:;' tip='" + model3.GetType().GetProperty("ID").GetValue(model3, null).ToString() + "'>" + model3.GetType().GetProperty(GetName(GetType)).GetValue(model3, null).ToString() + "</a>  </li>";
                                }
                                HTML += "</ul>";
                            }
                            HTML += "</li></ul>";
                        }
                        HTML += "</div>";
                    }
                }
            }
            WriteHTML = HTML;
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// 获取名称
    /// </summary>
    public string GetName(string type)
    {
        string str = string.Empty;
        if (!Util.IsEmpty(type))
        {
            if (type == "1")
            {
                str = "TypeName";//商品分类
            }
            else if (type == "2")
            {
                str = "AreaName";//代理商区域
            }
            else if (type == "3")
            {
                str = "TypeName";//代理商分类
            }
            else if (type == "4")
            {
                str = "Address";//代理商地址
            }
            else if (type == "5")
            {
                str = "DisName";//代理商管理员
            }
            else if (type == "6")
            {
                str = "DisName";//代理商管理员
            }
        }
        return str;
    }

    /// <summary>
    /// 获取最底层的分类
    /// </summary>
    /// <returns></returns>
    public string GetMinCategory(int id)
    {
        List<Hi.Model.BD_GoodsCategory> ll = new Hi.BLL.BD_GoodsCategory().GetList("1", "isnull(dr,0)=0 and isenabled=1 and parentid=" + id, "");
        if (ll.Count > 0)
        {
            return "y";
        }
        return "";
    }
}