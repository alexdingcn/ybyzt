using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

public partial class Company_SysManager_RoleInfo : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DataBinds();
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                btitle.InnerText = "设置岗位权限";

            }
        }
    }

    public void DataBinds() {
        if (KeyID != 0)
        {
            try
            {
                Hi.Model.SYS_Role role = new Hi.BLL.SYS_Role().GetModel(KeyID);
                if (role != null)
                {
                    if (role.IsEnabled == 1)
                    {
                        libtnUse.Visible = false;
                    }
                    else
                    {
                        libtnDel.Visible = false;
                    }
                    lblRoleName.InnerText = role.RoleName;
                    lblRemark.InnerText = role.Remark;
                    lblSortIndex.InnerText = role.SortIndex;
                    lblCreateDate.InnerText = role.CreateDate.ToShortDateString();
                    lblIsEnabled.InnerHtml = role.IsEnabled == 1 ? "启用" : "<font color=red>禁用</font>";
                }
                else
                {
                    JScript.AlertMethod(this, "数据不存在！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                    return;
                }
               // BindUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void Pager_PageChanged(object sender, EventArgs e)
    //{
    //    page = Pager.CurrentPageIndex.ToString();
    //    BindUser();
    //}

    //public void reload_Click(object sender, EventArgs e)
    //{
    //    BindUser();
    //}

    //public void BindUser()
    //{
    //    int pageCount = 0;
    //    int Counts = 0;
    //    string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
    //    DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, " SYS_CompUser.id, SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled ", JoinTableStr, " and SYS_CompUser.RoleID=" + KeyID + " and utype in(3,4)  and ctype=1  and SYS_CompUser.compid=" + CompID + "  ", out pageCount, out Counts);
    //    this.rpDtl.DataSource = LUser;
    //    this.rpDtl.DataBind();
    //    page = Pager.CurrentPageIndex.ToString();
    //}

    protected void btn_Del(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and Compid=" + CompID + " ", "");
        if (ListRole.Count > 0)
        {
            if (ListRole[0].RoleName == "企业管理员")
            {
                JScript.AlertMsgOne(this, "企业管理员岗位不允许禁用！", JScript.IconOption.错误);
                return;
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "禁用的岗位不存在！", JScript.IconOption.错误);
            return;
        }

        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and  Ctype=1 ", "");
            foreach (Hi.Model.SYS_CompUser model in ListCompUser)
            {
                model.IsEnabled = 0;
                model.ts = DateTime.Now;
                model.modifyuser = UserID;
                new  Hi.BLL.SYS_CompUser().Update(model,Tran);
            }
            Tran.Commit();
        }
        catch
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
                JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
            }
        }

        ListRole[0].IsEnabled = 0;
        ListRole[0].ts = DateTime.Now;
        ListRole[0].modifyuser = UserID;
        if (new Hi.BLL.SYS_Role().Update(ListRole[0]))
        {
            if (Request["nextstep"] + "" == "1")
            {
                //JScript.AlertMsgMo(this, "操作成功！", "function(){ window.location.href='RoleList.aspx?nextstep=1'; }");
                Response.Redirect("RoleList.aspx?nextstep=1");
            }
            else
            {
                //JScript.AlertMsgMo(this, "操作成功！", "function(){ window.location.href='RoleList.aspx'; }");
                Response.Redirect("RoleList.aspx");
            }
        }
    }

    protected void btn_Use(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and Compid=" + CompID + " ", "");
        if (ListRole.Count== 0)
        {
            JScript.AlertMsgOne(this, "启用的岗位不存在！", JScript.IconOption.错误);
            return;
        }
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and  Ctype=1 ", "");
            foreach (Hi.Model.SYS_CompUser model in ListCompUser)
            {
                model.IsEnabled = 1;
                model.ts = DateTime.Now;
                model.modifyuser = UserID;
                new Hi.BLL.SYS_CompUser().Update(model, Tran);
            }
            Tran.Commit();
        }
        catch
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
                JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
            }
        }

        ListRole[0].IsEnabled = 1;
        ListRole[0].ts = DateTime.Now;
        ListRole[0].modifyuser = UserID;
        if (new Hi.BLL.SYS_Role().Update(ListRole[0]))
        {
            if (Request["nextstep"] + "" == "1")
            {
                Response.Redirect("RoleList.aspx?nextstep=1");
            }
            else
            {
                Response.Redirect("RoleList.aspx");
            }
        }
    }
}