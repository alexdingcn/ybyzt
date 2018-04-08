using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

public partial class Admin_Role_RoleInfo : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
            if (Request["ntype"] != null && Request["ntype"].ToString() == "1")
            {
                
            }
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            try
            {
                Hi.Model.SYS_Role role = new Hi.BLL.SYS_Role().GetModel(KeyID);
                if (role != null)
                {
                    //if (role.IsEnabled == 1)
                    //{
                    //    libtnUse.Visible = false;
                    //}
                    //else
                    //{
                    //    libtnDel.Visible = false;
                    //}
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
                Bind();
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
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public void reload_Click(object sender, EventArgs e)
    {
        Bind();
    }

    public void Bind()
    {
        int pageSize = 5;
        int pageCount = 0;
        int Counts = 0;

        Pager.PageSize = pageSize;
        List<Hi.Model.SYS_AdminUser> LAdminuser = new Hi.BLL.SYS_AdminUser().GetList
            (Pager.PageSize, Pager.CurrentPageIndex, "id", false, "and RoleID=" + KeyID + "  and isnull(dr,0)=0", out pageCount, out Counts);
        this.rpDtl.DataSource = LAdminuser;
        this.rpDtl.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    //protected void btn_Del(object sender, EventArgs e)
    //{
    //    List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and Compid=" + CompID + " ", "");
    //    if (ListRole.Count > 0)
    //    {
    //        if (ListRole[0].RoleName == "企业管理员")
    //        {
    //            JScript.AlertMsgOne(this, "企业管理员岗位不允许禁用！", JScript.IconOption.错误);
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        JScript.AlertMsgOne(this, "禁用的岗位不存在！", JScript.IconOption.错误);
    //        return;
    //    }

    //    SqlTransaction Tran = null;
    //    try
    //    {
    //        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
    //        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and  Ctype=1 ", "");
    //        foreach (Hi.Model.SYS_CompUser model in ListCompUser)
    //        {
    //            model.IsEnabled = 0;
    //            model.ts = DateTime.Now;
    //            model.modifyuser = UserID;
    //            new Hi.BLL.SYS_CompUser().Update(model, Tran);
    //        }
    //        Tran.Commit();
    //    }
    //    catch
    //    {
    //        if (Tran != null)
    //        {
    //            if (Tran.Connection != null)
    //                Tran.Rollback();
    //            JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
    //        }
    //    }

    //    ListRole[0].IsEnabled = 0;
    //    ListRole[0].ts = DateTime.Now;
    //    ListRole[0].modifyuser = UserID;
    //    if (new Hi.BLL.SYS_Role().Update(ListRole[0]))
    //    {
    //        if (Request["nextstep"] + "" == "1")
    //        {
    //            //JScript.AlertMsgMo(this, "操作成功！", "function(){ window.location.href='RoleList.aspx?nextstep=1'; }");
    //            Response.Redirect("RoleList.aspx?nextstep=1");
    //        }
    //        else
    //        {
    //            //JScript.AlertMsgMo(this, "操作成功！", "function(){ window.location.href='RoleList.aspx'; }");
    //            Response.Redirect("RoleList.aspx");
    //        }
    //    }
    //}

    //protected void btn_Use(object sender, EventArgs e)
    //{
    //    List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and Compid=" + CompID + " ", "");
    //    if (ListRole.Count == 0)
    //    {
    //        JScript.AlertMsgOne(this, "启用的岗位不存在！", JScript.IconOption.错误);
    //        return;
    //    }
    //    SqlTransaction Tran = null;
    //    try
    //    {
    //        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
    //        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and  Ctype=1 ", "");
    //        foreach (Hi.Model.SYS_CompUser model in ListCompUser)
    //        {
    //            model.IsEnabled = 1;
    //            model.ts = DateTime.Now;
    //            model.modifyuser = UserID;
    //            new Hi.BLL.SYS_CompUser().Update(model, Tran);
    //        }
    //        Tran.Commit();
    //    }
    //    catch
    //    {
    //        if (Tran != null)
    //        {
    //            if (Tran.Connection != null)
    //                Tran.Rollback();
    //            JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
    //        }
    //    }

    //    ListRole[0].IsEnabled = 1;
    //    ListRole[0].ts = DateTime.Now;
    //    ListRole[0].modifyuser = UserID;
    //    if (new Hi.BLL.SYS_Role().Update(ListRole[0]))
    //    {
    //        if (Request["nextstep"] + "" == "1")
    //        {
    //            Response.Redirect("RoleList.aspx?nextstep=1");
    //        }
    //        else
    //        {
    //            Response.Redirect("RoleList.aspx");
    //        }
    //    }
    //}
}