using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_Tree_ProductSort : AdminPageBase
{
    public List<Hi.Model.BD_GoodsCategory> goodsCategory = new List<Hi.Model.BD_GoodsCategory>();
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            if (obj.ToString() == "yanz")//验证是否还存在下级分类
            {
                int id = Convert.ToInt32(Request["id"]);
                Response.Write(GetMinCategory(id));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            GetGoodsCategory();//商品类别
            string jss = "var zNodes = [";
            foreach (Hi.Model.BD_GoodsCategory category in goodsCategory.FindAll(p => p.ParentId == 0))
            {
                jss += "{ id: " + category.ID + ", pId: 0, name: \"" + category.CategoryName + "\" ," + (GetGoodsCategory(category.ID) > 0 ? " open: true" : "isParent:true") + "},";
                foreach (Hi.Model.BD_GoodsCategory category2 in goodsCategory.FindAll(a => a.ParentId == category.ID))
                {
                    jss += "{ id: " + category2.ID + ", pId: " + category.ID + ", name: \"" + category2.CategoryName + "\" },";
                    foreach (Hi.Model.BD_GoodsCategory category3 in goodsCategory.FindAll(a => a.ParentId == category2.ID))
                    {
                        jss += "{ id: " + category3.ID + ", pId: " + category2.ID + ", name: \"" + category3.CategoryName + "\" },";
                    }
                }
            }
            jss += "]";
            ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + jss + "</script>");
        }
    }

    /// <summary>
    /// 获取最底层的分类
    /// </summary>
    /// <returns></returns>
    public string GetMinCategory(int id)
    {
        List<Hi.Model.BD_GoodsCategory> ll = new Hi.BLL.BD_GoodsCategory().GetList("", "isnull(dr,0)=0 and isenabled=1 and parentid=" + id, "");
        if (ll.Count > 0)
        {
            return "y";
        }
        return "";
    }

    /// <summary>
    /// 判断是否存在子类别
    /// </summary>
    /// <returns></returns>
    public int GetGoodsCategory(int id)
    {
        string CompId = Request.QueryString["hidCompID"].ToString();
        var list = new Hi.BLL.BD_GoodsCategory().GetList("", "parentid=" + id + " and ISNULL(dr,0)=0 and compid in(" + CompId + ")", "");//判断是否存在下级类别
        return list.Count;
    }
    /// <summary>
    /// 商品类别
    /// </summary>
    public void GetGoodsCategory()
    {
        string CompId = Request.QueryString["hidCompID"].ToString();
        List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", " ISNULL(dr,0)=0 and compid in(" + CompId + ")", "sortindex, parentid");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsCategory goodsCate in l)
            {
                goodsCategory.Add(goodsCate);
            }
        }
    }
}