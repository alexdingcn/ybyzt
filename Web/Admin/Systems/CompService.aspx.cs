using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_CompService : System.Web.UI.Page
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
        List<Hi.Model.Pay_Service> ServiceList = new Hi.BLL.Pay_Service().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
        this.ServiceList.DataSource = ServiceList;
        this.ServiceList.DataBind();
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
        string where = " and isnull(dr,0)=0 and IsAudit=1";
        if (!string.IsNullOrEmpty(CompName.Value.Trim()))
        {
            where += " and (CompName like '%" + Common.NoHTML(CompName.Value.Trim().Replace("'", "''")) + "%' )";
        }
        //if (!string.IsNullOrEmpty(txtUserPhone.Value.Trim()))
        //{
        //    where += " and (UserPhone like '%" + Common.NoHTML(txtUserPhone.Value.Trim().Replace("'", "''")) + "%' )";
        //}
        if (!string.IsNullOrEmpty(drpState.SelectedValue.Trim()))
        {
            where += " and (ServiceType=" + Common.NoHTML(drpState.SelectedValue.Trim()) + ")";
        }
        return where;
    }

}