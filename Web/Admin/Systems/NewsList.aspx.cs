using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_NewsList : AdminPageBase
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
                Pager.PageSize = 20;
                this.txtPageSize.Value = "20";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.SYS_NewsNotice> LANewsNotice = new Hi.BLL.SYS_NewsNotice().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = LANewsNotice;
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
        if (!string.IsNullOrEmpty(txtnewtitle.Value.Trim()))
        {
            where += " and (NewsTitle like '%" + Common.NoHTML(txtnewtitle.Value.Trim().Replace("'", "''")) + "%' )";
        }

        if (!string.IsNullOrEmpty(DropDownList1.SelectedValue))
        {
            where += " and NewsType='" + Common.NoHTML(DropDownList1.SelectedValue) + "'";
        }

        if (!string.IsNullOrEmpty(DropDownList2.SelectedValue))
        {
            where += " and IsTop='" + Common.NoHTML(DropDownList2.SelectedValue) + "'";
        }
        return where;
    }

    public string GetName(object obj)
    {
        string str = "";
        string newstype=obj.ToString();
        if (!string.IsNullOrEmpty(newstype))
        {
            if (newstype == "1")
            {
                str = "新闻";
            }
            else if (newstype == "2")
            {
                str = "公告";
            }
            else if (newstype == "3")
            {
                str = "资讯";
            }
            else {
                str = "生意经";
            }
        }
        return str;
    }
}