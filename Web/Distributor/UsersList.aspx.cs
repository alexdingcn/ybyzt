using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_UsersList : DisPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BinDisUser();
        }
    }


    //public void DataBinds()
    //{
    //    int pageCount = 0;
    //    int Counts = 0;
    //    //每页显示的数据设置
    //    if (this.txtPageSize.Value.ToString() != "")
    //    {
    //        if (this.txtPageSize.Value.Trim().Length >= 5)
    //        {
    //            Pager.PageSize = 100;
    //            this.txtPageSize.Value = "100";
    //        }
    //        else
    //        {
    //            Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
    //        }
    //    }
    //    string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
    //    DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, " SYS_CompUser.id,UserID,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled ", JoinTableStr, SearchWhere(), out pageCount, out Counts);
    //    this.Rpt_Company.DataSource = LUser;
    //    this.Rpt_Company.DataBind();
    //    Pager.RecordCount = Counts;
    //    page = Pager.CurrentPageIndex.ToString();
    //}

    public void BinDisUser()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, "SYS_CompUser.id,SYS_CompUser.userid,SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled ", JoinTableStr,SearchWhere(), out pageCount, out Counts);
        this.Rpt_Company.DataSource = LUser;
        this.Rpt_Company.DataBind();
        page = Pager.CurrentPageIndex.ToString();
    }


    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        BinDisUser();
    }
    public void btnSearch_Click(object sender, EventArgs e)
    {
        BinDisUser();
    }


    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and utype in(1,5)  and ctype=2  and SYS_CompUser.DisID=" + DisID + " ";
        if (!string.IsNullOrEmpty(txtUsername.Value.Trim()))
        {
            where += " and UserName like '%" + Common.NoHTML(txtUsername.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtName.Value.Trim()))
        {
            where += " and TrueName like '%" + Common.NoHTML(txtName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and Phone like '%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        return where;
    }

}