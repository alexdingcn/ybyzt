using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_OrgUserList : AdminPageBase
{
    public string page = "1";//默认初始页
    public DropDownList allSaleMan=null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Common.BindOrgSale(ddlOrg, allSaleMan, "请选择");
            Common.BindOrgSale(allorg, allSaleMan,"全部");
            DataBinds();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e) {
        Hi.Model.SYS_AdminUser user = new Hi.Model.SYS_AdminUser();
        user.UserType = 4;
        if (Common.SysUserExistsAttribute("LoginName", txtUserName.Value.Trim())) {
            JScript.AlertMsg(this, "该用户已存在。");
            return;
        }
        if (ddlOrg.SelectedValue == "-1") {
            JScript.AlertMsg(this, "请选择机构。");
            return;
        }
        if (txtUserName.Value.Trim() == "")
        {
            JScript.AlertMsg(this, "登录帐号不能为空。");
            return;
        }
        user.LoginName = Common.NoHTML(txtUserName.Value.Trim());
        user.LoginPwd = Util.md5(txtUserPwd.Value.Trim());
        user.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
        user.TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
        user.CreateDate = DateTime.Now;
        user.IsEnabled = 1;
        user.CreateUserID = UserID;
        user.ts = DateTime.Now;
        user.modifyuser = UserID;
        user.OrgID = int.Parse(ddlOrg.SelectedValue);
        new Hi.BLL.SYS_AdminUser().Add(user);
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>window.location.href='OrgUserList.aspx';</script>");
    }
    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        if (this.txtPageSize.Value.ToString() != "" && int.TryParse(txtPageSize.Value.Trim(), out pageCount))
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
        List<Hi.Model.SYS_AdminUser> Lorg = new Hi.BLL.SYS_AdminUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_OrgUser.DataSource = Lorg;
        this.Rpt_OrgUser.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        //this.ddlOrg.SelectedValue = this.OrgID == 0 ? ddlOrg.SelectedValue : this.OrgID.ToString();
        this.ddlOrg.SelectedIndex = 0;

    }

    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0 and Usertype in (3,4)";
        if (!string.IsNullOrEmpty(txtLoginName.Value.Trim()))
        {
            where += " and ( LoginName like '%" + Common.NoHTML(txtLoginName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.ddrOtype.Value != "-1")
        {
            where += " and IsEnabled=" + this.ddrOtype.Value;
        }
        if (!string.IsNullOrEmpty(txtTrueName.Value.Trim()))
        {

            where += " and TrueName like '%" + Common.NoHTML(txtTrueName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and Phone like '%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (allorg.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? ddlOrg.SelectedValue.Replace("'", "''") : this.OrgID.ToString();
            where += " and OrgID='" + org + "'";
        }
        if (UserType == 3)
        {
            where += " and OrgID='" + OrgID + "'";
        }
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }



}