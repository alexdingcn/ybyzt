using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_SysManager_DisUserList : CompPageBase
{
  
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
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

        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 inner join BD_Company on SYS_CompUser.CompID=BD_Company.ID and BD_Company.dr=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, "SYS_CompUser.CompID, SYS_CompUser.id,UserName,SYS_CompUser.Disid,trueName,utype Type,Users.Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate ", JoinTableStr, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Disuser.DataSource = LUser;
        this.Rpt_Disuser.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }


    public void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "  and utype in (1,5) and SYS_CompUser.compid=" + CompID + " ";
        if (DisSalesManID != 0)
        {
            where += "and SYS_CompUser.DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and exists(select 1 from BD_Distributor where id=SYS_CompUser.Disid and compid=" + CompID + " and Disname like '%" + Common.NoHTML(txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtuname.Value.Trim()))
        {
            where += " and username like '%" + Common.NoHTML(txtuname.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (ddlState.SelectedValue != "-1")
        {
            where += " and SYS_CompUser.IsEnabled ='" + Common.NoHTML(ddlState.SelectedValue )+ "' ";
        }
        if (txtCreateDate.Value.Trim() != "")
        {
            where += " and SYS_CompUser.createdate>='" + Common.NoHTML(txtCreateDate.Value.Trim()) + "' ";
        }
        if (txtEndCreateDate.Value.Trim() != "")
        {
            where += " and SYS_CompUser.createdate<='" + Common.NoHTML(txtEndCreateDate.Value.Trim()) + "' ";
        }
        return where;
    }

        /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Pager_PageChanged(object sender, EventArgs e) {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
}