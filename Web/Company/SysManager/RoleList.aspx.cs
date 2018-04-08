using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_RoleList : CompPageBase
{
    public string page = "1";//默认初始页
    public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = this.CompUser;
        if (!IsPostBack)
        {
            this.txtPageSize.Value = PageSize.ToString();
            DataBinds();
            //if (!string.IsNullOrEmpty(Request["mszs"]) && Request["mszs"].ToString() == "1")
            //{
            //    ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>confirm('如果您的管理企业人员较少，请点击确认，或修改企业管理人员后点击下一步！', alinkorder, '提示');</script>");
            //}
            if (user.IsFirst == 0)
            {
                user.IsFirst = 4;
                user.modifyuser = user.ID;
                user.ts = DateTime.Now;
                new Hi.BLL.SYS_Users().Update(user);
            }
            
        }

        if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
        {
            atitle.InnerText = "我要开通";
            //btitle.InnerText = "设置岗位权限";

            //下一步可用
            libtnNext.Style.Add("display", "block;");
            add.Style.Add("color", "black");
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
        string where = " and compid=" + CompID + " and isnull(DisID,0)=0 and isnull(dr,0)=0";
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