using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Distributor_UserAdd : DisPageBase
{
    public string UID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Action = Request["Action"] + "";
            string Name = Request["Value"] + "";

            if (Action == "GetPhone")
            {
                Response.Write(ExistsUserPhone(Name));
                Response.End();
            }
            Databinds();
            //绑定权限  RepeaterRoles
            List<Hi.Model.SYS_Role> LDis = new Hi.BLL.SYS_Role().GetList("", " DisID=" + DisID + "  AND dr=0 AND IsEnabled=1 ", "Createdate", null);
            this.RepeaterRoles.DataSource = LDis;
            this.RepeaterRoles.DataBind();
        }
    }

    public string ExistsUserPhone(string Phone)
    {
        if (KeyID != 0)
        {
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("Userid", "   id=" + KeyID + " and DisID=" + DisID + " and ctype=2 ", "");
            string id = ListCompUser.Count > 0 ? ListCompUser[0].UserID.ToString() : "0";
            if (Common.GetUserExists("Phone",Phone, id))
            {
                return "{ \"result\":true}";
            }
            else
            {
                return "{ \"result\":false}";
            }
        }
        else
        {
            if (Common.GetUserExists("Phone", Phone))
            {
                return "{ \"result\":true}";
            }
            else
            {
                return "{ \"result\":false}"; 
            }
        }
    }
    public void Databinds()
    {
        if (KeyID != 0)
        {
            int pageCount = 0;
            int Counts = 0;
            string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
            DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", true, " SYS_CompUser.id,Users.id userid,Address,UserPwd, SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,SYS_CompUser.CompID,Utype ", JoinTableStr, " and SYS_CompUser.UserID=" + KeyID + " and SYS_CompUser.DisID=" + DisID + " and ctype=2 ", out pageCount, out Counts);
            if (LUser.Rows.Count > 0)
            {
                try
                {
                    ViewState["Userid"] = LUser.Rows[0]["userid"].ToString();
                    //Hi.Model.SYS_Role role = new Hi.BLL.SYS_Role().GetModel(LUser.Rows[0]["RoleId"].ToString().ToInt(0));
                    txtTrueName.Value = LUser.Rows[0]["TrueName"].ToString();
                    txtPhone.Value = LUser.Rows[0]["Phone"].ToString();
                    //txtTel.Value = User.Tel;
                    txtIdentitys.Value = LUser.Rows[0]["Identitys"].ToString();
                    txtAddress.Value = LUser.Rows[0]["Address"].ToString();
                    txtEmail.Value = LUser.Rows[0]["Email"].ToString();
                    //if (role != null)
                    //{
                    //    ddlRoleId.InnerText = role.RoleName.ToString();
                    //}
                    txtUserName.Value = LUser.Rows[0]["UserName"].ToString();
                    txtUserPwd.Attributes.Add("value", LUser.Rows[0]["UserPwd"].ToString());
                    txtPwd.Attributes.Add("value", LUser.Rows[0]["UserPwd"].ToString());
                    //txtUserLoginName.Value = User.UserLoginName;
                    if (LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) != 1)
                    {
                        rdEnabledNo.Checked = true;
                        rdEnabledYes.Checked = false;
                    }

                    if (LUser.Rows[0]["Utype"].ToString() == "5")
                    {
                        this.txtPhone.Disabled = true;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据错误！", JScript.IconOption.错误, 2500);
                return;
            }
        }
        //else
        //{
        //    if (string.IsNullOrWhiteSpace((Request["RoleID"] + "").Trim()))
        //    {
        //        Response.Write("岗位编号错误");
        //        Response.End();
        //    }
        //    List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("Compid,DisID", " dr=0 and id=" + (Request["RoleID"] + "") + "", "");
        //    if (ListRole.Count > 0)
        //    {
        //        if (ListRole[0].CompID != CompID || ListRole[0].DisID != DisID)
        //        {
        //            Response.Write("岗位编号错误");
        //            Response.End();
        //        }
        //    }
        //    if (Request["RoleID"] != "")
        //    {
        //        btnClose.InnerText = "关闭";
        //        Hi.Model.SYS_Role role = new Hi.BLL.SYS_Role().GetModel(Convert.ToInt32(Request["RoleID"].ToString()));
        //        this.ddlRoleId.InnerText = role.RoleName;
        //    }
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_Users User = null;
        Hi.Model.SYS_CompUser CompUser = null;
        Hi.Model.SYS_RoleUser RoleUser = null;
        Hi.BLL.SYS_RoleUser RoleUserService = new Hi.BLL.SYS_RoleUser();
        SqlTransaction Tran = null;
        if (KeyID != 0)
        {
            #region 优化权限前的代码
            //int userid = ViewState["Userid"].ToString().ToInt(0);
            //if (Common.GetUserExists("UserName", txtUserName.Value.Trim(), userid.ToString()))
            //{
            //    JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
            //    return;
            //}
            //if (Common.GetUserExists("Phone", txtPhone.Value.Trim(), userid.ToString()))
            //{
            //    JScript.AlertMsgOne(this, "该手机号码已存在！", JScript.IconOption.错误);
            //    return;
            //}
            //if (txtPwd.Text.Trim() != txtUserPwd.Text.Trim())
            //{
            //    JScript.AlertMsgOne(this, "确认密码填写不一致！", JScript.IconOption.错误);
            //    return;
            //}
            //List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", "id=" + KeyID + " and isnull(dr,0)=0", "");
            //if (ListCompUser.Count == 0)
            //{
            //    JScript.AlertMethod(this, "用户明细数据有误！", JScript.IconOption.错误, "function (){ history.go(-1) ; }");
            //    return;
            //}
            //if (rdEnabledNo.Checked)
            //{
            //    if (ListCompUser[0].UType == 5)
            //    {
            //        JScript.AlertMsgOne(this, "用户为系统管理员，不可禁用！", JScript.IconOption.错误);
            //        return;
            //    }
            //}
            //else
            //{
            //    if (ListCompUser[0].UType != 5)
            //    {
            //        List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("RoleName,IsEnabled", " dr=0 and id=" + ListCompUser[0].RoleID + "", "");
            //        if (ListRole.Count > 0)
            //        {
            //            if (ListRole[0].IsEnabled == 0)
            //            {
            //                JScript.AlertMsgOne(this, "请先启用该人员所在岗位（" + ListRole[0].RoleName + "）！", JScript.IconOption.错误, 2500);
            //                return;
            //            }
            //        }
            //    }
            //}
            //Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //User = new Hi.BLL.SYS_Users().GetModel(userid);
            //User.TrueName = txtTrueName.Value.Trim();
            //User.Identitys = txtIdentitys.Value.Trim();
            //User.Address = txtAddress.Value.Trim();
            //User.Email = txtEmail.Value.Trim();
            //ListCompUser[0].IsEnabled = rdEnabledYes.Checked ? 1 : 0;
            //ListCompUser[0].ts = DateTime.Now;
            //ListCompUser[0].modifyuser = UserID;
            //if (txtUserPwd.Text.Trim() != Util.md5("123456"))
            //{
            //    User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
            //}
            //if (ListCompUser[0].UType != 4)
            //{
            //    User.UserName = txtUserName.Value.Trim();
            //    User.Phone = txtPhone.Value.Trim();
            //}
            //User.ts = DateTime.Now;
            //User.modifyuser = UserID;
            //new Hi.BLL.SYS_Users().Update(User, Tran);
            //new Hi.BLL.SYS_CompUser().Update(ListCompUser[0], Tran);
            //Tran.Commit();
            //JScript.AlertMethod(this, "操作成功!", JScript.IconOption.正确, "function(){ cancel(); }"); 
            #endregion
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //修改
            User = new Hi.BLL.SYS_Users().GetModel(KeyID);
            CompUser = new Hi.Model.SYS_CompUser();
            if (User.UserName != txtUserName.Value.Trim())
            {
                if (Common.GetUserExists("UserName", txtUserName.Value.Trim()))
                {
                    JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
                    return;
                }
            }
            if (User.Phone != txtPhone.Value.Trim())
            {
                if (txtPhone.Value.Trim() == "")
                {
                    JScript.AlertMsgOne(this, "手机号码不能为空！", JScript.IconOption.错误);
                    return;
                }
                if (Common.GetUserExists("Phone", txtPhone.Value.Trim()))
                {
                    JScript.AlertMsgOne(this, "该手机号码已存在！", JScript.IconOption.错误);
                    return;
                }
            }
            if (txtPwd.Text.Trim() != txtUserPwd.Text.Trim())
            {
                JScript.AlertMsgOne(this, "确认密码填写不一致！", JScript.IconOption.错误);
                return;
            }
            //禁用时判断
            if (rdEnabledNo.Checked)
            {
                List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " CompID=" + CompID + " AND UserID=" + KeyID + " AND dr=0 AND IsEnabled=1 ", "");
                if (ListCompUser[0].UType == 5)
                {
                    JScript.AlertMsgOne(this, "用户为管理员，不可禁用！", JScript.IconOption.错误);
                    return;
                }
                else
                {
                    User.IsEnabled = 0;
                    CompUser.IsEnabled = 0;
                    List<Hi.Model.SYS_RoleUser> roleusers = new Hi.BLL.SYS_RoleUser().GetList("", " UserID=" + KeyID + " AND dr=0 ", "");
                    if (roleusers.Count > 0)
                    {
                        for (int i = 0; i < roleusers.Count; i++)
                        {
                            roleusers[i].IsEnabled = false;
                        }
                    }
                }
            }
            if (rdEnabledYes.Checked)
            {
                User.IsEnabled = 1;
                CompUser.IsEnabled = 1;
            }
            User.UserName = Common.NoHTML(txtUserName.Value.Trim());
            User.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
            User.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
            User.Phone = Common.NoHTML(txtPhone.Value.Trim());
            if (txtPwd.Text.Trim() != User.UserPwd)
            {
                User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
            }
            User.Address = Common.NoHTML(txtAddress.Value.Trim());
            User.Email = Common.NoHTML(txtEmail.Value.Trim());
            User.Type = 1;
            User.ts = DateTime.Now;
            User.modifyuser = UserID;
            CompUser.UType = 1;
            //岗位权限表
            List<Hi.Model.SYS_RoleUser> roleuser = new Hi.BLL.SYS_RoleUser().GetList("", "  UserID=" + KeyID + "  AND dr=0 ", "");
            for (int i = 0; i < roleuser.Count; i++)
            {
                roleuser[i].IsEnabled = false;
                roleuser[i].ts = DateTime.Now;
                RoleUserService.Update(roleuser[i]);
            }
            if (hidMyRole.Value!="")
            {
                string[] rolestr = hidMyRole.Value.Substring(0, hidMyRole.Value.Length - 1).Split(',');
                for (int i = 0; i < rolestr.Length; i++)
                {
                    List<Hi.Model.SYS_RoleUser> rolenew = new Hi.BLL.SYS_RoleUser().GetList("", "  UserID=" + KeyID + " AND dr=0 AND RoleID=" + rolestr[i].ToInt(0) + " ", "");
                    if (rolenew.Count > 0)
                    {
                        rolenew[0].IsEnabled = true;
                        rolenew[0].ts = DateTime.Now;
                        RoleUserService.Update(rolenew[0]);
                    }
                    else
                    {
                        RoleUser = new Hi.Model.SYS_RoleUser();
                        RoleUser.FunType = 1;
                        RoleUser.UserID = KeyID;
                        RoleUser.RoleID = rolestr[i].ToInt(0);
                        RoleUser.IsEnabled = true;
                        RoleUser.CreateUser = this.UserID.ToString();
                        RoleUser.CreateDate = DateTime.Now;
                        RoleUser.ts = DateTime.Now;
                        RoleUser.dr = 0;
                        RoleUserService.Add(RoleUser, Tran);
                    }
                }
            }
            new Hi.BLL.SYS_Users().Update(User, Tran);
            new Hi.BLL.SYS_CompUser().Update(CompUser, Tran);
            Tran.Commit();
            Response.Redirect("UserInfo.aspx?KeyId=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) );
        }
        else
        {
            if (Common.GetUserExists("UserName", txtUserName.Value.Trim()))
            {
                JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
                return;
            }
            if (txtPhone.Value.Trim() == "")
            {
                JScript.AlertMsgOne(this, "手机号码不能为空！", JScript.IconOption.错误);
                return;
            }
            if (Common.GetUserExists("Phone", txtPhone.Value.Trim()))
            {
                JScript.AlertMsgOne(this, "该手机号码已存在！", JScript.IconOption.错误);
                return;
            }
            if (txtPwd.Text.Trim() != txtUserPwd.Text.Trim())
            {
                JScript.AlertMsgOne(this, "确认密码填写不一致！", JScript.IconOption.错误);
                return;
            }
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            User = new Hi.Model.SYS_Users();
            User.UserName = Common.NoHTML(txtUserName.Value.Trim());
            User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
            //User.UserLoginName = txtUserLoginName.Value.Trim();
            User.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
            //User.Sex = rdSexYes.Checked ? "男" : "女";
            User.Phone = Common.NoHTML(txtPhone.Value.Trim());
            //User.Tel = txtTel.Value.Trim();
            User.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
            User.Address = Common.NoHTML(txtAddress.Value.Trim());
            User.Email = Common.NoHTML(txtEmail.Value.Trim());
            User.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
            User.IsFirst = 0;
            User.CreateDate = DateTime.Now;
            User.CreateUserID = UserID;
            User.AuditUser = UserID.ToString();
            User.ts = DateTime.Now;
            User.modifyuser = UserID;
            int userid = new Hi.BLL.SYS_Users().Add(User, Tran);
            CompUser = new Hi.Model.SYS_CompUser();
            CompUser.CompID = CompID;
            CompUser.DisID = DisID;
            CompUser.CreateDate = DateTime.Now;
            CompUser.CreateUserID = UserID;
            CompUser.modifyuser = UserID;
            CompUser.CType = 2;
            CompUser.UType = 1;//用户类型
            CompUser.RoleID = 0;
            CompUser.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
            CompUser.IsAudit = 2;
            CompUser.ts = DateTime.Now;
            CompUser.dr = 0;
            CompUser.UserID = userid;
            new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
            //岗位权限表
            if (hidMyRole.Value != "")
            {
                string[] rolestr = hidMyRole.Value.Split(',');
                foreach (string str in rolestr)
                {
                    if (str != "" && Convert.ToInt32(str) > 0)
                    {
                        RoleUser = new Hi.Model.SYS_RoleUser();
                        RoleUser.FunType = 1;
                        RoleUser.UserID = userid;
                        RoleUser.RoleID = Convert.ToInt32(str);
                        RoleUser.IsEnabled = true;
                        RoleUser.CreateUser = this.UserID.ToString();
                        RoleUser.CreateDate = DateTime.Now;
                        RoleUser.ts = DateTime.Now;
                        RoleUser.dr = 0;
                        RoleUserService.Add(RoleUser, Tran);
                    }
                }
            }
            Tran.Commit();
            Response.Redirect("UserInfo.aspx?KeyId=" + Common.DesEncrypt(userid.ToString(), Common.EncryptKey) );
            //this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>cancel()</script>");
        }
    }

    /// <summary>
    /// 初始加载是否绑定
    /// </summary>
    /// <param name="NodeID"></param>
    /// <returns></returns>
    public string BindRoleSysFun(string NodeID)
    {
        int keyid = KeyID;
        string Role = NodeID;
        List<Hi.Model.SYS_RoleUser> List = new Hi.BLL.SYS_RoleUser().GetList(null, " RoleID=" + Role.ToInt(0) + " and  UserID=" + keyid + "  and dr=0  and IsEnabled=1  ", null);
        foreach (Hi.Model.SYS_RoleUser Model in List)
        {
            Role = "checked=\"checked\"";
            break;
        }
        return Role;
    }
}