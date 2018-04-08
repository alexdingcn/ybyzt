using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Goods_GoodsAreasEdit : CompPageBase
{
    public int num = 0;
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDisAreaBox.CompID = CompID.ToString();
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
        }
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = " and ISNULL(dr,0)=0 and  isenabled=1 and ComPid=" + this.CompID;
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
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
        List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        this.rptGoods.DataSource = l;
        this.rptGoods.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>$(function(){     $(\"#CB_SelAll\").trigger(\"click\");})</script>");
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        int areaid = 0;
        if (!Util.IsEmpty(txtDisAreaBox.areaId))
        {
            areaid = Convert.ToInt32(txtDisAreaBox.areaId);
        }
        string strWhere = string.Empty;
        string goodsid = string.Empty;
        List<Hi.Model.BD_GoodsAreas> lll = new Hi.BLL.BD_GoodsAreas().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and areaid=" + areaid, "");
        if (lll.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsAreas item in lll)
            {
                goodsid += item.GoodsID + ",";
            }
        }
        if (!Util.IsEmpty(goodsid))
        {
            goodsid = goodsid.Substring(0, goodsid.Length - 1);
            strWhere += " and id not in(" + goodsid + ")";
        }
        //赋值
        string goodsName = this.txtGoodsName.Value.Trim();//商品名称
        string hideID = this.txtCategory.treeId;//类别id
        string idlist = string.Empty;
        if (!Util.IsEmpty(goodsName))
        {
            //List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList("", "goodsname like '%" + goodsName + "%'  and isnull(dr,0)=0 and isenabled=1  and compid=" + this.CompID, "");
            //if (l.Count > 0)
            //{
            //    foreach (Hi.Model.BD_Goods item in l)
            //    {
            //        idlist += item.ID + ",";
            //    }
            //}
            //if (!Util.IsEmpty(idlist))
            //{
            //    strWhere += string.Format(" and id in( {0})", idlist.Substring(0, idlist.Length - 1));
            //}
            //else
            //{
            //    strWhere += string.Format(" and id = ''");
            //}
            strWhere += " and goodsName like'%" + goodsName + "%'";
        }
        if (!Util.IsEmpty(hideID))
        {
            string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
            //List<Hi.Model.BD_Goods> ll = new Hi.BLL.BD_Goods().GetList("", "categoryID in(" + cateID + ") and isnull(dr,0)=0 and isenabled=1 and compId=" + this.CompID, "");
            //if (ll.Count > 0)
            //{
            //    foreach (Hi.Model.BD_Goods item in ll)
            //    {
            //        idlist += item.ID + ",";
            //    }
            //}
            //if (!Util.IsEmpty(idlist))
            //{
            //    strWhere += string.Format(" and id in( {0})", idlist.Substring(0, idlist.Length - 1));
            //}
            //else
            //{
            strWhere += " and categoryID in(" + cateID + ")";
            // }
        }
        return strWhere;
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtDisAreaBox.CompID = CompID.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr2(string id)
    {
        string str = string.Empty;
        DataTable dt = new Hi.BLL.BD_Goods().GoodsAttr(id, this.CompID.ToString());//属性
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str += dt.Rows[i]["AttributeName"].ToString() + ",";
        }
        return str == "" ? "" : str.Substring(0, str.Length - 1);
    }
    /// <summary>
    /// 商品类别
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GoodsCategory(string id)
    {
        var lsit = new Hi.BLL.BD_GoodsCategory().GetList("", " isnull(dr,0)=0  and isenabled=1 and compid=" + this.CompID + " and id=" + id, "");
        if (lsit.Count > 0)
        {
            return lsit[0].CategoryName;
        }
        return "";
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            List<int> l = new List<int>();
            List<int> ll = new List<int>();

            foreach (RepeaterItem row in this.rptGoods.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    HiddenField fld2 = row.FindControl("HF_CateID") as HiddenField;

                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        l.Add(id);
                        int cateid = Convert.ToInt32(fld2.Value);
                        ll.Add(cateid);
                    }
                }
            }
            if (l.Count == 0)
            {
                JScript.AlertMethod(this, "请勾商品",JScript.IconOption.错误);
                return;
            }
            int areaid = 0;
            if (!Util.IsEmpty(txtDisAreaBox.areaId))
            {
                areaid = Convert.ToInt32(txtDisAreaBox.areaId);
            }
            else
            {
                JScript.AlertMethod(this, "区域选择有误",JScript.IconOption.错误);
                return;
            }

            for (int i = 0; i < l.Count; i++)
            {
                Hi.Model.BD_GoodsAreas model = new Hi.Model.BD_GoodsAreas();
                model.CompID = this.CompID;
                model.areaID = areaid;
                model.GoodsID = l[i];
                model.CategoryID = ll[i];
                model.ts = DateTime.Now;
                model.modifyuser = this.UserID;
                new Hi.BLL.BD_GoodsAreas().Add(model);
            }
            // new Hi.BLL.BD_CategoryAttribute().Updates(this.UserID.ToString(), l, this.CompID.ToString());
            JScript.AlertMethod(this, "商品不可售区域设置成功",JScript.IconOption.正确, "function(){location.href='GoodsAreasList.aspx';}");
        }
        catch (Exception ex)
        {

            JScript.AlertMethod(this, "出错了",JScript.IconOption.错误);
            return;
        }
    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
}