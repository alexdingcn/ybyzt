

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

public partial class Distributor_RoleInfo : DisPageBase
{
    Hi.Model.SYS_Users user = null;

    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["KeyID"] != null)
        {
            string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
            KeyID = Id.ToInt(0);
        }

        if (!IsPostBack)
        {
            DataBinds();
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
                    if (role.IsEnabled == 1)
                    {
                        UseIcon.Visible = false;
                    }
                    else
                    {
                        NUseIcon.Visible = false;
                    }
                    lblRoleName.InnerText = role.RoleName;
                    lblRemark.InnerText = role.Remark;
                    lblSortIndex.InnerText = role.SortIndex;
                    lblCreateDate.InnerText = role.CreateDate.ToShortDateString();
                    lblIsEnabled.InnerHtml = role.IsEnabled == 1 ? "启用" : "<font color=red>禁用</font>";
                }
                else
                {
                    JScript.AlertMsgOne(this, "此条数据不存在！", JScript.IconOption.错误);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    protected void btn_NUse(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and DisID=" + DisID + " ", "");
        if (ListRole.Count > 0)
        {
            if (ListRole[0].RoleName == "企业管理员")
            {
                JScript.AlertMsgOne(this, "代理商管理员岗位不允许禁用！", JScript.IconOption.错误);
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
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and Disid=" + DisID + " and  Ctype=2 ", "");
            foreach (Hi.Model.SYS_CompUser model in ListCompUser)
            {
                model.IsEnabled = 0;
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

        ListRole[0].IsEnabled = 0;
        ListRole[0].ts = DateTime.Now;
        ListRole[0].modifyuser = UserID;
        if (new Hi.BLL.SYS_Role().Update(ListRole[0]))
        {
            Response.Redirect("RoleList.aspx");
        }


    }

    protected void btn_Use(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("", " dr=0 and id=" + KeyID + " and DisID=" + DisID + " ", "");
        if (ListRole.Count == 0)
        {
            JScript.AlertMsgOne(this, "启用的岗位不存在！", JScript.IconOption.错误);
            return;
        }
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and RoleID=" + ListRole[0].ID + " and Disid=" + DisID + " and  Ctype=2 ", "");
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
            Response.Redirect("RoleList.aspx");
        }
    }
}