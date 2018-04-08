using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Goods_GoodsAreasList : CompPageBase
{
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
            Bind();
        }
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = " and ISNULL(dr,0)=0 and ComPid=" + this.CompID;
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
        List<Hi.Model.BD_GoodsAreas> l = new Hi.BLL.BD_GoodsAreas().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        this.rptGoodsAreas.DataSource = l;
        this.rptGoodsAreas.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        List<int> l = new List<int>();
        foreach (RepeaterItem row in this.rptGoodsAreas.Items)
        {
            CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
            if (cb != null && cb.Checked)
            {
                HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                if (fld != null)
                {
                    int id = Convert.ToInt32(fld.Value);
                    l.Add(id);
                    Hi.Model.BD_GoodsAreas model = new Hi.BLL.BD_GoodsAreas().GetModel(id);
                    if (model != null)
                    {
                        model.ts = DateTime.Now;
                        model.modifyuser = this.UserID;
                        model.dr = 1;
                        model.CompID = this.CompID;
                        bool bol = new Hi.BLL.BD_GoodsAreas().Update(model);
                    }
                }
            }
        }
        Bind();
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
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = string.Empty;
        //赋值
        string areaId =Common.NoHTML( this.txtDisAreaBox.areaId);//区域
        string goodsName =Common.NoHTML( this.txtGoodsName.Value.Trim().Replace("'", "''"));//商品名称
        string hideID =Common.NoHTML( this.txtCategory.treeId);//类别id
        string idlist = string.Empty;
        if (!Util.IsEmpty(areaId))
        {
            string aredIdList = Common.DisAreaId(Convert.ToInt32(areaId), this.CompID);//递归得到区域id
            strWhere += string.Format(" and areaId in({0})", aredIdList);
        }
        if (!Util.IsEmpty(goodsName))
        {
            List<Hi.Model.BD_Goods> l = new Hi.BLL.BD_Goods().GetList("", "goodsname like '%" +Common.NoHTML( goodsName) + "%' and isnull(dr,0)=0 and isenabled=1  and compid=" + this.CompID, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_Goods item in l)
                {
                    idlist += item.ID + ",";
                }
            }
            if (!Util.IsEmpty(idlist))
            {
                strWhere += string.Format(" and Goodsid in( {0})", idlist.Substring(0, idlist.Length - 1));
            }
            else
            {
                strWhere += string.Format(" and Goodsid =''");
            }
        }
        if (!Util.IsEmpty(hideID) && txtCategory.treeName != "")
        {
            string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
            List<Hi.Model.BD_Goods> ll = new Hi.BLL.BD_Goods().GetList("", "categoryID in(" + cateID + ") and isnull(dr,0)=0 and isenabled=1", "");
            if (ll.Count > 0)
            {
                foreach (Hi.Model.BD_Goods item in ll)
                {
                    idlist += item.ID + ",";
                }
            }
            if (!Util.IsEmpty(idlist))
            {
                strWhere += string.Format(" and Goodsid in( {0})", idlist.Substring(0, idlist.Length - 1));
            }
            else
            {
                strWhere += string.Format(" and Goodsid =''");
            }
        }
        return strWhere;
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