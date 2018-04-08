using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI.HtmlControls;

public partial class Distributor_RoleList : DisPageBase
{
    public string page = "1";//默认初始页
    //public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user=LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            this.txtPageSize.Value = "12";
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        //每页显示
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
        List<Hi.Model.SYS_Role> LDis = new Hi.BLL.SYS_Role().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
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
    public void A_Seek(object sender, EventArgs e)
    {
        DataBinds();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and DisID=" + this.DisID + " and isnull(dr,0)=0";
        if (!string.IsNullOrEmpty(txtRoleName.Value.Trim()))
        {
            where += " and ( RoleName like '%" + Common.NoHTML(txtRoleName.Value.Trim().Replace("'", "''")) + "%')";
        } 
        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and IsEnabled='" + Common.NoHTML(sltIsAllow.SelectedValue) + "'";
        }
        return where;
    }
}