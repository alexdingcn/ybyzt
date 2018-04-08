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

            txtEndCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            txtCreateDate.Value= DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
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
        List<Hi.Model.SYS_Select> LANewsNotice = new Hi.BLL.SYS_Select().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, SearchWhere(), out pageCount, out Counts);
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
        
        string CreateDate = txtCreateDate.Value+ " 0:0:0";
        string EndCreateDate = txtEndCreateDate.Value + " 23:59:59";
        string where = " and isnull(dr,0)=0  and CreateDate between '"+ CreateDate + "' and '"+ EndCreateDate + "'";
        if (!string.IsNullOrEmpty(drpState.SelectedValue.Trim()))
        {
            where += " and (Type=" + Common.NoHTML(drpState.SelectedValue.Trim()) + ")";
        }
        if(!string.IsNullOrWhiteSpace(this.txtSelectName.Value))
            where += " and  SelectNamestring like '%"+ this.txtSelectName.Value + "%'";
        return where;
    }

}