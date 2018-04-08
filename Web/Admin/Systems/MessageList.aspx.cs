using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_MessageList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["State"] != null)
                this.drpState.SelectedValue = Request["State"].ToString();

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
        List<Hi.Model.SYS_UserMessage> LANewsNotice = new Hi.BLL.SYS_UserMessage().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
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
        string where = " and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(txtUserName.Value.Trim()))
        {
            where += " and (UserName like '%" + Common.NoHTML(txtUserName.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(txtUserPhone.Value.Trim()))
        {
            where += " and (UserPhone like '%" + Common.NoHTML(txtUserPhone.Value.Trim().Replace("'", "''")) + "%' )";
        } if (!string.IsNullOrEmpty(drpState.SelectedValue.Trim()))
        {
            where += " and (State=" + Common.NoHTML(drpState.SelectedValue.Trim()) + ")";
        }
        return where;
    }

}