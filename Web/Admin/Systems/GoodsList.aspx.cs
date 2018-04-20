using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_GoodsList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan, "全部");
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
        ViewState["strWhere"] = Where();
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
        string strWhere = "and ISNULL(dr,0)=0 and GoodsName!='' ";
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        strWhere += Where();
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
        string strWhere = "";
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
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            strWhere += " and exists(select 1 from BD_Company where id=CompID and CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (Org.SelectedValue != "-1")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + Org.SelectedValue + "')";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + salemanid.Value + "')";
        }
        return strWhere;
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr2(string id,string CompID)
    {
        string str = string.Empty;
        DataTable dt = new Hi.BLL.BD_Goods().GoodsAttr(id, CompID);//属性
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
        var lsit = new Hi.BLL.BD_GoodsCategory().GetList("", " isnull(dr,0)=0 and id=" + id, "");
        if (lsit.Count > 0)
        {
            return lsit[0].CategoryName;
        }
        return "";
    }

    public string GetPicURL(string goodsId, string compId, string format = null)
    {
        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
        if (goods != null)
        {
            return Common.GetPicURL(goods.Pic, format, compId);
        }

        return "../../images/havenopicsmallest.gif";
    }
}