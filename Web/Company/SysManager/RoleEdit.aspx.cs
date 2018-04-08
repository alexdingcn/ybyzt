using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_SysManager_RoleEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                btitle.InnerText = "设置岗位权限";

            }
        }
    }

    public void DataBinds() {
        //string strTitle = "角色新增";
        if (Request["title"] == "1")
        {
            atitle.InnerText = "我要开通";
            btitle.InnerText = "设置岗位权限";
        }
        if (KeyID != 0)
        {
            try
            {
                //strTitle = "角色编辑";
                this.btnAdd.Text = "确定";
                Hi.Model.SYS_Role role = new Hi.BLL.SYS_Role().GetModel(KeyID);
                if (role != null)
                {
                    txtRoleName.Text = role.RoleName;
                    if (role.RoleName == "企业管理员")
                        txtRoleName.Enabled = false;
                    txtRemark.Value = role.Remark;
                    txtSortIndex.Value = role.SortIndex;
                    if (role.IsEnabled != 1)
                    {
                        rdAuditNo.Checked = true;
                        rdAuditYes.Checked = false;
                    }
                }
                else
                {
                    JScript.AlertMethod(this, "数据不存在！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleList.aspx") + "'); }");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //lblTitle.InnerText = strTitle;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Hi.Model.SYS_Role role = null;
            if (KeyID != 0)
            {
                //修改
                if (rdAuditNo.Checked)
                {
                    string sql = "select * from SYS_Users where ISNULL(dr,0)=0 and IsEnabled=1 and RoleID=" + KeyID + " and CompID=" + CompID;
                    DataTable userDT = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                    if (userDT.Rows.Count > 0)
                    {
                        if (Request["nextstep"] + "" == "1")
                        {
                            JScript.AlertMethod(this, "请先禁用该岗位下的人员！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleEdit.aspx?nextstep=1&KeyID=" + KeyID) + "'); }");
                            return;
                        }
                        else
                        {
                            JScript.AlertMethod(this, "请先禁用该岗位下的人员！", JScript.IconOption.错误, "function (){ location.replace('" + ("RoleEdit.aspx?KeyID=" + KeyID) + "'); }");
                            return;
                        }
                    }
                }
                role = new Hi.BLL.SYS_Role().GetModel(KeyID);
                role.RoleName =Common.NoHTML( txtRoleName.Text.Trim());
                if (RoleExistsAttribute("RoleName", role.RoleName, KeyID.ToString()))
                {
                    JScript.AlertMsgOne(this, "该角色名称已存在，不能重复！", JScript.IconOption.错误);
                    return;
                }
                role.IsEnabled = rdAuditYes.Checked ? 1 : 0;
                role.Remark =Common.NoHTML( txtRemark.Value.Trim());
                role.SortIndex =Common.NoHTML( txtSortIndex.Value.Trim());
                role.ts = DateTime.Now;
                role.modifyuser = UserID;
                if (new Hi.BLL.SYS_Role().Update(role))
                {
                    if (Request["nextstep"] + "" == "1")
                    {
                        JScript.AlertMethod(this, "编辑成功！", JScript.IconOption.正确, "function(){ window.location.href='RoleInfo.aspx?nextstep=1&KeyID=" + KeyID + "'; }");
                    }
                    else
                    {
                        JScript.AlertMethod(this, "编辑成功！", JScript.IconOption.正确, "function(){ window.location.href='RoleInfo.aspx?KeyID=" + KeyID + "'; }");
                    }
                }
                else
                {
                    JScript.AlertMsgOne(this, "编辑失败！", JScript.IconOption.错误);
                    return;
                }
            }
            else
            {
                //新增
                role = new Hi.Model.SYS_Role();
                role.CompID = CompID;
                role.RoleName =Common.NoHTML( txtRoleName.Text.Trim());
                if (RoleExistsAttribute("RoleName", role.RoleName))
                {
                    JScript.AlertMsgOne(this, "该角色名称已存在，不能重复！", JScript.IconOption.错误);
                    return;
                }
                role.IsEnabled = rdAuditYes.Checked ? 1 : 0;
                role.Remark =Common.NoHTML( txtRemark.Value.Trim());
                role.SortIndex =Common.NoHTML( txtSortIndex.Value.Trim());

                role.CreateDate = DateTime.Now;
                role.CreateUserID = UserID;
                role.ts = DateTime.Now;
                role.modifyuser = UserID;
                role.dr = 0;
                int newid = new Hi.BLL.SYS_Role().Add(role);

                if (newid > 0)
                {
                    //JScript.AlertMsgMo(this, "新增成功！", "function(){ window.location.href='RoleList.aspx'; }");
                    //下一步的操作
                    if (Request["nextstep"] + "" == "1")
                    {
                        Response.Redirect("MenuEdit.aspx?type=1&nextstep=1&RoleId=" + newid);
                    }
                    else
                    {
                        Response.Redirect("MenuEdit.aspx?type=1&RoleId=" + newid);
                    }
                }
                else
                {
                    JScript.AlertMsgOne(this, "新增失败！", JScript.IconOption.错误);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 判断名称在一个企业里是否重复
    /// </summary>
    /// <param name="name">要比较的字段</param>
    /// <param name="value">名称值</param>
    /// <param name="id">业务主键ID</param>
    /// <returns></returns>
    public bool RoleExistsAttribute(string name, string value, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.SYS_Role> Dis = new Hi.BLL.SYS_Role().GetList("", " " + name + "='" + value + "' and isnull(DisID,0)=0 and CompID=" + this.CompID + " and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.SYS_Role> Dis = new Hi.BLL.SYS_Role().GetList("", " " + name + "='" + value + "' and isnull(DisID,0)=0 and CompID=" + this.CompID + " and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }
}