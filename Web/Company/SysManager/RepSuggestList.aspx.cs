using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_RepSuggestList : CompPageBase
{
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPageSize.Value = PageSize.ToString();
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = " and compid=" + this.CompID;
        
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
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
        if (!IsPostBack)
        {
            if (Request.QueryString["type"] + "" == "2")
            {
                strwhere += " and IsAnswer=0";
                this.ddrlRep.SelectedValue = "0";
            }
        }

        List<Hi.Model.DIS_Suggest> LDis = new Hi.BLL.DIS_Suggest().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, strwhere, out pageCount, out Counts);
        this.Rpt_Role.DataSource = LDis;
        this.Rpt_Role.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        DataBinds();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string where = " and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" + Common.NoHTML(txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(ddrlRep.SelectedValue.Trim()))
        {
            if (ddrlRep.SelectedValue.Trim() == "0")
                where += " and ReplyDate is null";
            if (ddrlRep.SelectedValue.Trim() == "1")
                where += " and ReplyDate is not null";
        }

        ViewState["strwhere"] = where;
        DataBinds();
    }

    protected string GetTitle(string Title)
    {
        if (Title.Length!=0)
            Title = Title.Length > 10 ? Title.Substring(0, 10) + "..." : Title;
        else
        {
            Title = "...";
        }
        return Title;
    }
}