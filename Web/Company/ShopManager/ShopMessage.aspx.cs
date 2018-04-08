using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_ShopManager_ShopMessage : CompPageBase
{
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object isread = Request["isread"];
            if (isread != null) {
                ddlRead.SelectedValue = isread.ToString();
            }
            this.txtPageSize.Value = PageSize.ToString();
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = " and compid=" + this.CompID+" and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            strwhere += " and remark like '%" + txtPhone.Value.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(ddlRead.SelectedValue.Trim()))
        {
            if (ddlRead.SelectedValue.Trim() == "0" || ddlRead.SelectedValue.Trim() == "1")
            {
                strwhere += " and isread =" + ddlRead.SelectedValue.Trim();
            }
        }
        //ViewState["strwhere"] = where;
        //if (ViewState["strwhere"] != null)
        //{
        //    strwhere += ViewState["strwhere"].ToString();
        //}
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

        List<Hi.Model.BD_ShopMessage> LDis = new Hi.BLL.BD_ShopMessage().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, strwhere, out pageCount, out Counts);
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
       
        DataBinds();
    }

    protected string GetTitle(string Title)
    {
        if (Title.Length != 0)
            Title = Title.Length > 20 ? Title.Substring(0, 20) + "..." : Title;
        else
        {
            Title = "...";
        }
        return Title;
    }
}