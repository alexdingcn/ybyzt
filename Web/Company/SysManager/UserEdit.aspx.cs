using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Company_SysManager_UserEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Action = Request["Action"] + "";
            string Name = Request["Value"] + "";
            //if (Action == "GetPhone")
            //{
            //    Response.Write(ExistsUserPhone(Name));
            //    Response.End();
            //}
            List<Hi.Model.BD_DisSalesMan> ll = new Hi.BLL.BD_DisSalesMan().GetList("SalesName,ID", "IsEnabled=1 and ISNULL(dr,0)=0 and CompID="+CompID+"", "");
            if (ll != null && ll.Count > 0)
            {
                DisSalesMan.DataSource = ll;
                DisSalesMan.DataTextField = "SalesName";
                DisSalesMan.DataValueField = "ID";
                DisSalesMan.DataBind();
                DisSalesMan.Items.Insert(0, new ListItem("请选择", "-1"));
                DisSalesMan.SelectedIndex = 0;
            }
            Databinds();
            //绑定权限  RepeaterRoles
            List<Hi.Model.SYS_Role> LDis = new Hi.BLL.SYS_Role().GetList("", " CompID="+CompID+" AND DisID=0 AND dr=0 AND IsEnabled=1 ", "Createdate", null);
            this.RepeaterRoles.DataSource = LDis;
            this.RepeaterRoles.DataBind();
        }
    }
    public void Databinds()
    {
        #region 更新角色权限前的代码
        //if (KeyID != 0)
        //{
        //    int pageCount = 0;
        //    int Counts = 0;
        //    string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        //    DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", true, " SYS_CompUser.id,Users.id userid,Address,UserPwd, SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,SYS_CompUser.CompID,Utype ", JoinTableStr, " and SYS_CompUser.id=" + KeyID + " and SYS_CompUser.Compid=" + CompID + " and ctype=1 ", out pageCount, out Counts);
        //    if (User != null)
        //    {
        //        try
        //        {
        //            ViewState["Userid"] = LUser.Rows[0]["userid"].ToString();
        //            txtTrueName.Value = LUser.Rows[0]["TrueName"].ToString(); ;
        //            txtPhone.Value = LUser.Rows[0]["Phone"].ToString();
        //            //txtTel.Value = User.Tel;
        //            txtIdentitys.Value = LUser.Rows[0]["Identitys"].ToString();
        //            txtAddress.Value = LUser.Rows[0]["Address"].ToString();
        //            txtEmail.Value = LUser.Rows[0]["Email"].ToString();
        //            txtUserName.Value = LUser.Rows[0]["UserName"].ToString(); 
        //            txtUserPwd.Attributes.Add("value", Util.md5("123456"));
        //            txtPwd.Attributes.Add("value", Util.md5("123456"));
        //            if (LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) != 1)
        //            {
        //                rdEnabledNo.Checked = true;
        //                rdEnabledYes.Checked = false;
        //            }
        //            if (LUser.Rows[0]["Utype"].ToString() == "4")
        //            {
        //                txtPhone.Disabled = true;
        //                txtUserName.Disabled = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write("获取信息异常！");
        //            Response.End();
        //        }
        //    }
        //    else
        //    {
        //        JScript.AlertMethod(this, "用户不存在！", JScript.IconOption.错误, "function (){ history.go(-1) ; }");
        //        return;
        //    }
        //}
        //else
        //{
        //    //新增操作
        //} 
        #endregion
        if (KeyID!=0)
        {
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(KeyID);
            if (user != null)
            {
                try
                {
                    txtTrueName.Value = user.TrueName.ToString(); ;
                    txtPhone.Value = user.Phone.ToString();
                    txtIdentitys.Value = user.Identitys.ToString();
                    txtAddress.Value = user.Address.ToString();
                    txtEmail.Value = user.Email.ToString();
                    txtUserName.Value =user.UserName.ToString();
                    //txtUserPwd.Text = user.UserPwd.ToString();
                    //txtPwd.Text = user.UserPwd.ToString();
                    txtUserPwd.Attributes.Add("value", user.UserPwd.ToString());
                    txtPwd.Attributes.Add("value", user.UserPwd.ToString());
                    if (user.IsEnabled.ToString().ToInt(0) != 1)
                    {
                        rdEnabledNo.Checked = true;
                        rdEnabledYes.Checked = false;
                    }
                   
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", "   userid=" + KeyID + " and Compid=" + CompID + " and ctype=1 ", "");
                    if (ListCompUser.Count>0)
                    {
                        if (ListCompUser[0].DisSalesManID > 0)
                        {
                            DisSalesManID.Value = ListCompUser[0].DisSalesManID.ToString();
                            
                            Radio2.Checked = true;
                            Radio1.Checked = false;
                            Hi.Model.BD_DisSalesMan man = new Hi.BLL.BD_DisSalesMan().GetModel(ListCompUser[0].DisSalesManID);
                            SalesManNames.InnerText="(业务员：" + man.SalesName + ")";
                            DisSalesMan.SelectedValue = ListCompUser[0].DisSalesManID.ToString();

                        }
                    }
                    if (ListCompUser[0].UType.ToString().ToInt(0) == 4)
                    {
                        txtPhone.Disabled = true;
                        txtUserName.Disabled = true;
                        this.Radio1.Disabled = true;
                        this.Radio2.Disabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("获取信息异常！");
                    Response.End();
                }
            }
            else
            {
                JScript.AlertMethod(this, "用户不存在！", JScript.IconOption.错误, "function (){ history.go(-1) ; }");
                return;
            }
        }
    }

    public string ExistsUserPhone(string Phone)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList( "Userid", "   id=" + KeyID + " and Compid=" + CompID + " and ctype=1 ", "");
        string id = ListCompUser.Count > 0 ? ListCompUser[0].UserID.ToString() : "0";
        if (Common.GetUserExists("Phone", Phone, id))
        {
            return "{ \"result\":true}";
        }
        else
        {
            return "{ \"result\":false}"; ;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        #region 没有优化岗位权限前的代码
        //SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        //Hi.Model.SYS_Users User = null;
        //if (KeyID != 0)
        //{
        //    //修改
        //    int userid = KeyID;
        //    User = new Hi.BLL.SYS_Users().GetModel(userid);
        //    if (Common.GetUserExists("UserName", txtUserName.Value.Trim(), userid.ToString()))
        //    {
        //        JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
        //        return;
        //    }
        //    if (Common.GetUserExists("Phone", txtPhone.Value.Trim(), userid.ToString()))
        //    {
        //        JScript.AlertMsgOne(this, "该手机号码已存在！", JScript.IconOption.错误);
        //        return;
        //    }
        //    if (txtPwd.Text.Trim() != txtUserPwd.Text.Trim())
        //    {
        //        JScript.AlertMsgOne(this, "确认密码填写不一致！", JScript.IconOption.错误);
        //        return;
        //    }

        //    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " CompID="+CompID+" AND UserID="+KeyID+" AND dr=0 AND IsEnabled=1 ", "");
        //    if (ListCompUser.Count == 0)
        //    {
        //        JScript.AlertMethod(this, "员工帐号信息有误！", JScript.IconOption.错误, "function (){ history.go(-1) ; }");
        //        return;
        //    }
        //    if (ListCompUser[0].UType != 4)
        //    {
        //        User.UserName = txtUserName.Value.Trim();
        //        User.Phone = txtPhone.Value.Trim();
        //    }
        //    if (rdEnabledNo.Checked)
        //    {
        //        if (ListCompUser[0].UType == 4)
        //        {
        //            JScript.AlertMsgOne(this, "用户为系统管理员，不可禁用！", JScript.IconOption.错误);
        //            return;
        //        }
        //    }
        //    User.TrueName = txtTrueName.Value.Trim();
        //    User.Identitys = txtIdentitys.Value.Trim();
        //    User.Address = txtAddress.Value.Trim();
        //    User.Email = txtEmail.Value.Trim();
        //    ListCompUser[0].IsEnabled = rdEnabledYes.Checked ? 1 : 0;
        //    ListCompUser[0].ts = DateTime.Now;
        //    ListCompUser[0].modifyuser = UserID;
        //    if (txtUserPwd.Text.Trim() != Util.md5("123456"))
        //    {
        //        User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
        //    }
        //    User.ts = DateTime.Now;
        //    User.modifyuser = UserID;
        //    List<Hi.Model.SYS_RoleUser> list = new Hi.BLL.SYS_RoleUser().GetList("", " dr=0 and IsEnabled=1 and UserID=" + KeyID + "", "");
        //    if (list.Count > 0)
        //    {
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            Hi.Model.SYS_RoleUser roleu = new Hi.Model.SYS_RoleUser();
        //            roleu.ID = list[0].ID;
        //            roleu.FunType = list[0].FunType;
        //            roleu.UserID = list[0].UserID;
        //            roleu.RoleID = list[0].RoleID;
        //            roleu.IsEnabled = true;//禁用
        //            roleu.CreateUser = list[0].CreateUser;
        //            roleu.CreateDate = list[0].CreateDate;
        //            roleu.ts = DateTime.Now;
        //            roleu.dr = list[0].dr;
        //            bool sss = new Hi.BLL.SYS_RoleUser().Update(roleu);
        //        }
        //    }
        //    else
        //    {
        //        //岗位权限表
        //        if (hidMyRole.Value != "")
        //        {
        //            string[] rolestr = hidMyRole.Value.Split(',');
        //            Hi.BLL.SYS_RoleUser RoleUserService = new Hi.BLL.SYS_RoleUser();
        //            Hi.Model.SYS_RoleUser RoleUser = null;
        //            foreach (string str in rolestr)
        //            {
        //                if (str != "" && Convert.ToInt32(str) > 0)
        //                {
        //                    RoleUser = new Hi.Model.SYS_RoleUser();
        //                    RoleUser.FunType = 1;
        //                    RoleUser.UserID = userid;
        //                    RoleUser.RoleID = Convert.ToInt32(str);
        //                    RoleUser.IsEnabled = true;
        //                    RoleUser.CreateUser = this.UserID.ToString();
        //                    RoleUser.CreateDate = DateTime.Now;
        //                    RoleUser.ts = DateTime.Now;
        //                    RoleUser.dr = 0;
        //                    RoleUserService.Add(RoleUser, Tran);
        //                }
        //            }
        //        }
        //    }
        //    new Hi.BLL.SYS_Users().Update(User, Tran);
        //    new Hi.BLL.SYS_CompUser().Update(ListCompUser[0], Tran);
        //    Tran.Commit();
        //    JScript.AlertMethod(this, "操作成功!", JScript.IconOption.正确, "UserInfo.aspx?KeyId="+KeyID);
        //}
        //else
        //{
        //    //新增
        //    if (Common.GetUserExists("UserName", txtUserName.Value.Trim()))
        //    {
        //        JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
        //        return;
        //    }
        //    if (Common.GetUserExists("Phone", txtPhone.Value.Trim()))
        //    {
        //        JScript.AlertMsgOne(this, "该手机号码已存在！", JScript.IconOption.错误);
        //        return;
        //    }
        //    if (txtPwd.Text.Trim() != txtUserPwd.Text.Trim())
        //    {
        //        JScript.AlertMsgOne(this, "确认密码填写不一致！", JScript.IconOption.错误);
        //        return;
        //    }
        //    User = new Hi.Model.SYS_Users();
        //    User.CompID = CompID;//厂商ID
        //    User.DisID = 0;
        //    User.Type = 3;
        //    User.AuditState = 2;
        //    User.UserName = txtUserName.Value.Trim();
        //    User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
        //    User.TrueName = txtTrueName.Value.Trim();
        //    User.Phone = txtPhone.Value.Trim();
        //    User.Identitys = txtIdentitys.Value.Trim();
        //    User.Address = txtAddress.Value.Trim();
        //    User.Email = txtEmail.Value.Trim();
        //    User.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
        //    User.IsFirst = 0;
        //    User.CreateDate = DateTime.Now;
        //    User.CreateUserID = UserID;
        //    User.AuditUser = UserID.ToString();
        //    User.ts = DateTime.Now;
        //    User.modifyuser = UserID;
        //    int userid = new Hi.BLL.SYS_Users().Add(User, Tran);
        //    //多角色表
        //    Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
        //    CompUser.CompID = CompID;
        //    CompUser.DisID = 0;
        //    CompUser.CreateDate = DateTime.Now;
        //    CompUser.CreateUserID = UserID;
        //    CompUser.modifyuser = UserID;
        //    CompUser.CType = 1;
        //    CompUser.UType = 3;
        //    CompUser.RoleID = 0;//权限屏蔽掉
        //    CompUser.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
        //    CompUser.IsAudit = 2;
        //    CompUser.ts = DateTime.Now;
        //    CompUser.dr = 0;
        //    CompUser.UserID = userid;
        //    new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
        //    //岗位权限表
        //    if (hidMyRole.Value != "")
        //    {
        //        string[] rolestr = hidMyRole.Value.Split(',');
        //        Hi.BLL.SYS_RoleUser RoleUserService = new Hi.BLL.SYS_RoleUser();
        //        Hi.Model.SYS_RoleUser RoleUser = null;
        //        foreach (string str in rolestr)
        //        {
        //            if (str != "" && Convert.ToInt32(str) > 0)
        //            {
        //                RoleUser = new Hi.Model.SYS_RoleUser();
        //                RoleUser.FunType = 1;
        //                RoleUser.UserID = userid;
        //                RoleUser.RoleID = Convert.ToInt32(str);
        //                RoleUser.IsEnabled = true;
        //                RoleUser.CreateUser = this.UserID.ToString();
        //                RoleUser.CreateDate = DateTime.Now;
        //                RoleUser.ts = DateTime.Now;
        //                RoleUser.dr = 0;
        //                RoleUserService.Add(RoleUser, Tran);
        //            }
        //        }
        //    }
        //    Tran.Commit();
        //    //this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>cancel()</script>");
        //    Response.Redirect("UserInfo.aspx?KeyId=" + userid);
        //} 
        #endregion
        Hi.Model.SYS_Users User = null;
        Hi.Model.SYS_CompUser CompUser = null;
        Hi.Model.SYS_RoleUser RoleUser = null;
        Hi.BLL.SYS_RoleUser RoleUserService = new Hi.BLL.SYS_RoleUser();
        if (KeyID!=0)
        {
            SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //修改
            User = new Hi.BLL.SYS_Users().GetModel(KeyID);
            CompUser = new Hi.Model.SYS_CompUser();
            if (User.UserName!=txtUserName.Value.Trim())
            {
                if (Common.GetUserExists("UserName", txtUserName.Value.Trim()))
                {
                    JScript.AlertMsgOne(this, "该登录帐号已存在！", JScript.IconOption.错误);
                    return;
                }
            }
            if (User.Phone!=txtPhone.Value.Trim())
            {
                if (txtPhone.Value.Trim()=="")
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
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " CompID=" + CompID + " AND UserID=" + KeyID + " AND dr=0 AND IsEnabled=1 ", "");
            CompUser = ListCompUser[0];
            //禁用时判断
            if (rdEnabledNo.Checked)
            {               
                if (ListCompUser[0].UType == 4)
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
            User.UserName =Common.NoHTML( txtUserName.Value.Trim());
            User.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
            User.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
            if (txtPwd.Text.Trim()!= User.UserPwd)
            {
                User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
            }
            User.Address = Common.NoHTML(txtAddress.Value.Trim());
            User.Email = Common.NoHTML(txtEmail.Value.Trim());
            User.Type = 3;
            User.ts = DateTime.Now;
            User.modifyuser =UserID;
            if (DisSalesManID.Value != "0")
            {
                if (CompUser.UType == 4)
                {
                    JScript.AlertMsgOne(this, "用户为管理员，不可修改类型！", JScript.IconOption.错误);
                    return;
                }
                CompUser.UType = 6;
                CompUser.DisSalesManID = Convert.ToInt32(DisSalesManID.Value);
            }
            else
            {
                if (CompUser.UType == 4)
                    CompUser.UType = 4;
                else
                    CompUser.UType = 3;

                CompUser.DisSalesManID = 0;
            }


            //岗位权限表
            List<Hi.Model.SYS_RoleUser> roleuser = new Hi.BLL.SYS_RoleUser().GetList("", "  UserID=" + KeyID + "  AND dr=0 ", "");
            for (int i = 0; i < roleuser.Count; i++)
            {
                roleuser[i].IsEnabled = false;
                roleuser[i].ts = DateTime.Now;
                RoleUserService.Update(roleuser[i]);
            }
            if (hidMyRole.Value != "")
            {
                string[] rolestr = hidMyRole.Value.Substring(0,hidMyRole.Value.Length-1).Split(',');
                for (int i = 0; i < rolestr.Length; i++)
                {
                    List<Hi.Model.SYS_RoleUser> rolenew = new Hi.BLL.SYS_RoleUser().GetList("", "  UserID=" + KeyID + " AND dr=0 AND RoleID=" + rolestr[i].ToInt(0) + " ", "");
                    if (rolenew.Count >0)
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
            SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //新增
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
            User = new Hi.Model.SYS_Users();
            User.CompID = CompID;//厂商ID
            User.DisID = 0;
            User.AuditState = 2;
            User.UserName =Common.NoHTML( txtUserName.Value.Trim());
            User.UserPwd = Util.md5(txtUserPwd.Text.Trim());
            User.TrueName = Common.NoHTML(txtTrueName.Value.Trim());
            User.Phone = Common.NoHTML(txtPhone.Value.Trim());
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
            //多角色表
            CompUser = new Hi.Model.SYS_CompUser();
            CompUser.CompID = CompID;
            CompUser.DisID = 0;
            CompUser.CreateDate = DateTime.Now;
            CompUser.CreateUserID = UserID;
            CompUser.modifyuser = UserID;
            CompUser.CType = 1;
            if (DisSalesManID.Value != "0")
            {
                CompUser.UType = 6;
                CompUser.DisSalesManID = Convert.ToInt32(DisSalesManID.Value);
            }
            else
            {
                CompUser.UType = 3;
            }
            CompUser.RoleID = 0;//权限屏蔽掉
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