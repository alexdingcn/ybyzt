using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Systems_CompUserList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Actiom = Request["Action"] + "";
            string value = Request["value"] + "";
            string OrgID = Request["OrgID"] + "";
            if (Actiom == "Action")
            {
                Response.Write(Common.getsaleman(OrgID));
                Response.End();
            }
            if (Actiom == "Getrol")
            {
                Response.Write(FindCompRol(value));
                Response.End();
            }
            if (Actiom == "GetPhone")
            {
                Response.Write(ExistsUserPhone(value));
                Response.End();
            }
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Common.BindOrgSale(Org, SaleMan, "全部");
            DataBinds();
        }
    }

    public string ExistsUserPhone(string Phone)
    {
        if (Common.GetUserExists("Phone", Phone))
        {
            return "{ \"result\":true}";
        }
        else
        {
            return "{ \"result\":false}"; ;
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 4)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", true, " SYS_CompUser.id,UserName,SYS_CompUser.compid,trueName,utype Type,Phone,SYS_CompUser.IsEnabled,SYS_CompUser.createdate ", JoinTableStr, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Comp.DataSource = LUser; 
        this.Rpt_Comp.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and utype in(3,4) and ctype=1 ";
        if (!string.IsNullOrEmpty(txtSUsername.Value.Trim()))
        {
            where += " and username like('%" + Common.NoHTML(txtSUsername.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtTrueName.Value.Trim()))
        {
            where += " and TrueName like'%" + Common.NoHTML(txtTrueName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (ddlDisbled.SelectedValue != "-1") {
            where += " and SYS_CompUser.IsEnabled=" + ddlDisbled.SelectedValue + "";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            where += " and exists(select 1 from BD_Company where id=SYS_CompUser.compid and compname like('%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%'))";
        }
        
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and phone like '%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and OrgID='" + org + "')";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "" && salemanid.Value != "0")
        {
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and SalesManID='" + salemanid.Value + "')";
        }
        if (OrgID > 0)
        {
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and SalesManID='" + OrgID + "')";
        }
        if (SalesManID > 0)
        {
            where += " and exists(select 1 from BD_Company  where id=SYS_CompUser.CompID and SalesManID='" + SalesManID + "')";
        }
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    public string FindCompRol(string Comid)
    {
        string rol = "[";
        string body = "";
        List<Hi.Model.SYS_Role> ll = new Hi.BLL.SYS_Role().GetList("", "IsEnabled=1 and ISNULL(dr,0)=0 and DisID=0 and CompID=" + Comid, "");
        foreach (Hi.Model.SYS_Role rl in ll) {
            body += "{\"name\":\"" + rl .RoleName+ "\",\"value\":\""+rl.ID+"\"},";
        }
        if (body != "")
        {
            body = body.Substring(0, body.Length - 1);
        }
        rol +=body+ "]";
        return rol;
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
        int rolid = 0;
        if (Common.GetUserExists(txtUserName.Value.Trim()))
        {
            JScript.AlertMsg(this, "该用户已存在。");
            return;
        }
        if (!int.TryParse(HidRoid.Value, out rolid) || HidRoid.Value == "-1")
        {
            JScript.AlertMsg(this, "未选择岗位。");
            return;
        }
        if (Common.GetUserExists("Phone", txtUserPhone.Value.Trim()))
        {
            JScript.AlertMsg(this, "手机号码已存在。");
            return;
        }
        SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        rolid = HidRoid.Value.ToInt(0);
        user.UserName = Common.NoHTML(txtUserName.Value.Trim());
        user.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
        user.UserPwd = Util.md5(txtUserPwd.Value.Trim());
        user.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
        user.AuditState = 2;
        user.IsEnabled = 1;
        user.CreateDate = DateTime.Now;
        user.CreateUserID = UserID;
        user.ts = DateTime.Now;
        user.modifyuser = UserID;
        int userid = new Hi.BLL.SYS_Users().Add(user, Tran);
        Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
        CompUser.CompID = TextComp.Compid.ToInt(0);
        CompUser.DisID = 0;
        CompUser.CreateDate = DateTime.Now;
        CompUser.CreateUserID = UserID;
        CompUser.modifyuser = UserID;
        CompUser.CType = 1;
        CompUser.UType = 3;
        CompUser.IsEnabled = 1;
        CompUser.IsAudit = 2;
        CompUser.ts = DateTime.Now;
        CompUser.dr = 0;
        CompUser.UserID = userid;
        CompUser.RoleID = rolid;
        new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
        Tran.Commit();
        string str = string.Format("医站通提示:\n尊敬的用户您好,管理员给您创建了一个帐号为{0}的用户,请登录网站查看\n【医站通】", user.UserName);
        JScript.AlertMsgMo(this, "添加成功", "function(){ window.location.href='CompUserList.aspx?page=" + page + "'; }");

    }
}