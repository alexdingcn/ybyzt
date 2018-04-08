using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_GoodsTypeList : AdminPageBase
{

    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
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
        List<Hi.Model.SYS_GoodsType> LAGoodsType = new Hi.BLL.SYS_GoodsType().GetList(Pager.PageSize, Pager.CurrentPageIndex, "cast(SortIndex as int)", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = LAGoodsType;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
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
        if (!string.IsNullOrEmpty(txtname.Value.Trim()))
        {
            where += " and (GoodsTypeName like '%" + Common.NoHTML(txtname.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(txtsort.Value.Trim()))
        {
            where += " and (SortIndex = '" + Common.NoHTML(txtsort.Value.Trim().Replace("'", "''")) + "' )";
        }

        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and IsEnabled='" + sltIsAllow.SelectedValue + "'";
        }
        return where;
    }
}