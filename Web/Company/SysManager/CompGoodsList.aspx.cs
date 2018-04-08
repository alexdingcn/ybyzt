using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_SysManager_CompGoodsList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();
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
        Bind();
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = "and ISNULL(dr,0)=0 and ComPid=" + this.CompID;
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
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = " and Compid=" + this.CompID;
        //赋值
        string goodsName = Common.NoHTML(this.txtGoodsName.Value.Trim().Replace("'", "''"));//商品名称
        string isoffline = Common.NoHTML(this.ddlState.SelectedItem.Value.Trim());//状态
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += string.Format(" and goodsName like '%{0}%' and ISNULL(dr,0)=0 ", goodsName);
        }
        if (isoffline != "")
        {
            strWhere += " and isoffline=" + isoffline;
        }
        return strWhere;
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
        var lsit = new Hi.BLL.BD_GoodsCategory().GetList("", " isnull(dr,0)=0  and isenabled=1  and compid=" + this.CompID + " and id=" + id, "");
        if (lsit.Count > 0)
        {
            return lsit[0].CategoryName;
        }
        return "";
    }
}