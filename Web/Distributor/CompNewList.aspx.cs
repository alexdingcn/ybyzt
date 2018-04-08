

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_CompNewList : DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_CompNews> CompNewsNotice = new Hi.BLL.BD_CompNews().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
        rptCompNew.DataSource = CompNewsNotice;
        rptCompNew.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void A_Seek(object sender, EventArgs e)
    {
        Bind();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0 and Compid=" + this.CompID + " ";
        if (!string.IsNullOrEmpty(txtnewtitle.Value.Trim()))
        {
            where += " and (NewsTitle like '%" + Common.NoHTML(txtnewtitle.Value.Trim().Replace("'", "''")) + "%' )";
        }

        if (!string.IsNullOrEmpty(ddlNewType.SelectedValue))
        {
            where += " and NewsType='" + Common.NoHTML(ddlNewType.SelectedValue) + "'";
        }

        if (!string.IsNullOrEmpty(ddlNewTop.SelectedValue))
        {
            where += " and IsTop='" + Common.NoHTML(ddlNewTop.SelectedValue) + "'";
        }
        return where;
    }
}