using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class NewsList : CompPageBase
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
            if (this.txtPageSize.Value.Trim().ToInt(0) >= 10000)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_CompNews> CompNewsNotice = new Hi.BLL.BD_CompNews().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = CompNewsNotice;
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
        string where = "  and isnull(dr,0)=0 and Compid="+CompID+"";
        if (!string.IsNullOrEmpty(txtnewtitle.Value.Trim()))
        {
            where += " and (NewsTitle like '%" + Common.NoHTML(txtnewtitle.Value.Trim().Replace("'", "''") )+ "%' )";
        }

        if (!string.IsNullOrEmpty(ddlNewType.SelectedValue))
        {
            where += " and NewsType='" + Common.NoHTML(ddlNewType.SelectedValue) + "'";
        }

        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and IsEnabled='" + sltIsAllow.SelectedValue + "'";
        }
        if (!string.IsNullOrEmpty(ddlNewTop.SelectedValue))
        {
            where += " and IsTop='" + Common.NoHTML(ddlNewTop.SelectedValue) + "'";
        }
        return where;
    }
}