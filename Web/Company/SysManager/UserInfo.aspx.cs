using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_UserContro_UserInfo : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txt_keyid.Value = Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) ;
            DataBinds();
            //绑定权限  RepeaterRoles
            List<Hi.Model.SYS_Role> LDis = new Hi.BLL.SYS_Role().GetList("", " CompID=" + CompID + " AND DisID=0 AND dr=0 AND IsEnabled=1 ", "Createdate", null);
            this.RepeaterRoles.DataSource = LDis;
            this.RepeaterRoles.DataBind();
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    public void DataBinds()
    {
        #region 优化角色权限之前的代码
        //if (KeyID!=0)
        //{
        //    int pageCount = 0;
        //    int Counts = 0;
        //    string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        //    DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.createdate", true, " SYS_CompUser.id,Address,SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,SYS_CompUser.CompID ", JoinTableStr, " and SYS_CompUser.id=" + KeyID + " and SYS_CompUser.Compid=" + CompID + " and ctype=1 ", out pageCount, out Counts);
        //    if (LUser.Rows.Count>0 )
        //    {
        //        if (LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1)
        //        {
        //            libtnUse.Visible = false;
        //        }
        //        else
        //        {
        //            libtnDel.Visible = false;
        //        }
        //        //Hi.Model.SYS_Role ll = new Hi.BLL.SYS_Role().GetModel(LUser.Rows[0]["RoleId"].ToString().ToInt(0));
        //        lblTrueName.InnerText = LUser.Rows[0]["TrueName"].ToString();
        //        //lblSex.InnerText = User.Sex;
        //        lblPhone.InnerText = LUser.Rows[0]["Phone"].ToString();
        //        //lblTel.InnerText = User.Tel;
        //        lblIdentitys.InnerText = LUser.Rows[0]["Identitys"].ToString();
        //        lblIsEnabled.InnerText = LUser.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1 ? "启用" : "禁用";
        //        lblEmail.InnerText = LUser.Rows[0]["Email"].ToString();
        //        //if (ll != null)
        //        //{
        //        //    lblRoleId.InnerText = ll.RoleName;
        //        //}
        //        //lblUserLoginName.InnerText = User.UserLoginName.ToString();
        //        lblUserName.InnerText = LUser.Rows[0]["UserName"].ToString();
        //        lblAddress.InnerText = LUser.Rows[0]["Address"].ToString();
        //    }
        //    else
        //    {
        //        JScript.AlertMethod(this, "用户不存在！", JScript.IconOption.错误, "function (){ if(window.parent.Layerclose){ window.parent.Layerclose(); } history.go(-1) ; }");
        //        return;
        //    }
        //}
        //else
        //{
        //    JScript.AlertMethod(this, "数据错误！", JScript.IconOption.错误, "function (){ if(window.parent.Layerclose){ window.parent.Layerclose(); }  history.go(-1) ; }");
        //    return;
        //} 
        
        //if (KeyID!=0)
        //{
        //    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(KeyID);
        //    lblTrueName.InnerText =user.TrueName.ToString();
        //    //lblSex.InnerText = User.Sex;
        //    lblPhone.InnerText = user.Phone.ToString();
        //    //lblTel.InnerText = User.Tel;
        //    lblIdentitys.InnerText = user.Identitys.ToString();
        //    lblIsEnabled.InnerText = user.IsEnabled.ToString().ToInt(0) == 1 ? "启用" : "禁用";
        //    //if (user.Type.ToString().ToInt(0) == 3)
        //    //{
        //    //    lbltype.InnerText = "用户";
        //    //}
        //    //if (user.Type.ToString().ToInt(0) == 4)
        //    //{
        //    //    lbltype.InnerText = "管理员";
        //    //}
        //    lblEmail.InnerText = user.Email.ToString();
        //    //if (ll != null)
        //    //{
        //    //    lblRoleId.InnerText = ll.RoleName;
        //    //}
        //    //lblUserLoginName.InnerText = User.UserLoginName.ToString();
        //    lblUserName.InnerText = user.UserName.ToString();
        //    lblAddress.InnerText = user.Address.ToString();
        //}
        //else
        //{
        //    JScript.AlertMethod(this, "数据错误！", JScript.IconOption.错误, "function (){ if(window.parent.Layerclose){ window.parent.Layerclose(); }  history.go(-1) ; }");
        //    return;
        //}

        #endregion
        if (KeyID != 0)
        {
            //Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(KeyID);
            int pageCount = 0;
            int Counts = 0;
            string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";

            DataTable dt = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.ID", true, "SYS_CompUser.DisID,SYS_CompUser.id,Address,SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,SYS_CompUser.CompID,SYS_CompUser.UType,SYS_CompUser.DisSalesManID", JoinTableStr, " and SYS_CompUser.CompID=" + this.CompID + " and SYS_CompUser.UserID= " + KeyID, out pageCount, out Counts);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1)
                {
                    libtnUse.Visible = false;
                    libtnDel.Visible = true;
                    libtnDelete.Visible = false;
                }
                else
                {
                    libtnDel.Visible = false;
                    libtnUse.Visible = true;
                    libtnDelete.Visible = true;
                }

                //Hi.Model.SYS_Role ll = new Hi.BLL.SYS_Role().GetModel(dt.Rows[0]["RoleID"].ToString().ToInt(0));
                lblTrueName.InnerText = dt.Rows[0]["TrueName"].ToString();
                //lblSex.InnerText = User.Sex;
                lblPhone.InnerText = dt.Rows[0]["Phone"].ToString();
                //lblTel.InnerText = User.Tel;
                lblIdentitys.InnerText = dt.Rows[0]["Identitys"].ToString();
                lblIsEnabled.InnerText = dt.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1 ? "启用" : "禁用";
                lblEmail.InnerText = dt.Rows[0]["Email"].ToString();
                //if (ll != null)
                //{
                //    lblRoleId.InnerText = ll.RoleName;
                //}
                //lblUserLoginName.InnerText = User.UserLoginName.ToString();
                lblUserName.InnerText = dt.Rows[0]["UserName"].ToString();
                lblAddress.InnerText = dt.Rows[0]["Address"].ToString();

                string UserType= dt.Rows[0]["UType"].ToString()=="1"? "代理商用户": dt.Rows[0]["UType"].ToString() == "2" ? "公共用户" : dt.Rows[0]["UType"].ToString() == "3" ? "厂商用户" : dt.Rows[0]["UType"].ToString() == "4" ? "厂商管理员" : dt.Rows[0]["UType"].ToString() == "5" ? "代理商管理员" : dt.Rows[0]["UType"].ToString() == "6" ? "企业销售员" : "无";
                if (dt.Rows[0]["UType"].ToString() == "6")
                {
                    Hi.Model.BD_DisSalesMan man = new Hi.BLL.BD_DisSalesMan().GetModel(Convert.ToInt32(dt.Rows[0]["DisSalesManID"].ToString()));
                    UserType += "("+ man .SalesName+ ")";
                }
                UserTypes.InnerText = UserType;//绑定业务员类型


            }
            else
            {
                JScript.AlertMsgOne(this, "用户不存在！", JScript.IconOption.错误, 2500);
                return;
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据错误！", JScript.IconOption.错误, 2500);
            return;
        }
    }
    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Del(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and Compid=" + CompID + " and ctype=1 ", "");
        if (ListCompUser.Count > 0)
        {
            if (ListCompUser[0].UType == 4)
            {
                JScript.AlertMsgOne(this, "该用户为系统管理员，不可禁用！", JScript.IconOption.错误, 2500);
                return;
            }
            else
            {
                ListCompUser[0].IsEnabled = 0;
                ListCompUser[0].ts = DateTime.Now;
                ListCompUser[0].modifyuser = UserID;
                if (new Hi.BLL.SYS_CompUser().Update(ListCompUser[0]))
                {
                    this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>window.parent.save();</script>");
                }
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "用户不存在！", JScript.IconOption.错误, 2500);
            return;
        }
        DataBinds();
    }
    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Use(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and Compid=" + CompID + " and ctype=1 ", "");
        if (ListCompUser.Count > 0)
        {
            List<Hi.Model.SYS_Role> ListRole = new Hi.BLL.SYS_Role().GetList("RoleName,IsEnabled", " dr=0 and id=" + ListCompUser[0].RoleID + "", "");
            if (ListRole.Count > 0)
            {
                if (ListRole[0].IsEnabled == 0)
                {
                    JScript.AlertMsgOne(this, "请先启用该人员所在岗位（" + ListRole[0].RoleName + "）！", JScript.IconOption.错误, 2500);
                    return;
                }
            }
            ListCompUser[0].IsEnabled = 1;
            ListCompUser[0].ts = DateTime.Now;
            ListCompUser[0].modifyuser = UserID;
            if (new Hi.BLL.SYS_CompUser().Update(ListCompUser[0]))
            {
                this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>window.parent.save();</script>");
            }
        }
        else
        {
            JScript.AlertMethod(this, "用户不存在！", JScript.IconOption.错误, "function (){ history.go(-1) ; }");
            return;
        }
        DataBinds();
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Delete(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and Compid=" + CompID + " and ctype=1 ", "");
        if (ListCompUser.Count > 0)
        {
            if (ListCompUser[0].UType == 4)
            {
                JScript.AlertMsgOne(this, "该用户为系统管理员，不可删除！", JScript.IconOption.错误, 2500);
                return;
            }
            else
            {
                List<int> ListUserid = new List<int>();
                List<int> ListDelUserid = new List<int>();
                foreach (Hi.Model.SYS_CompUser model in ListCompUser)
                {
                    if (!ListUserid.Contains(model.UserID))
                    {
                        List<Hi.Model.SYS_CompUser> luser = new Hi.BLL.SYS_CompUser().GetList("id", " dr=0 and Userid=" + model.UserID + " ", "");
                        if (luser.Count == 1)
                        {
                            ListDelUserid.Add(model.UserID);
                        }
                        ListUserid.Add(model.UserID);
                    }
                    model.dr = 1;
                    model.ts = DateTime.Now;
                    model.modifyuser = Common.UserID();
                    new Hi.BLL.SYS_CompUser().Update(model);
                }
                if (ListDelUserid.Count > 0)
                {
                    List<Hi.Model.SYS_Users> ListUsers = new Hi.BLL.SYS_Users().GetList("", " dr=0 and id in(" + string.Join(",", ListDelUserid) + ")", "");
                    foreach (Hi.Model.SYS_Users model in ListUsers)
                    {
                        model.dr = 1;
                        model.ts = DateTime.Now;
                        model.modifyuser = Common.UserID();
                        new Hi.BLL.SYS_Users().Update(model);
                    }
                }
                //this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>window.parent.save();</script>");
                Response.Redirect("UserList.aspx");
            }
        }
    }

    /// <summary>
    /// 发送短信：提示员工登录信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_SendOut(object sender, EventArgs e)
    {
        try
        {
            string Phone = lblPhone.InnerText;
            string name = lblUserName.InnerText;
            GetPhoneCode pc = new GetPhoneCode();
            string pp=pc.SendUser(Phone, name, "123456");
            if (pp == "Success")
            {
                JScript.AlertMsgOne(this, "发送成功！", JScript.IconOption.笑脸, 2500);
            }
            else
            {
                JScript.AlertMsgOne(this, "发送失败", JScript.IconOption.哭脸, 2500);
            }
            return;
        }
        catch (Exception ex)
        {
            JScript.AlertMsgOne(this, "发送失败！", JScript.IconOption.错误, 2500);
            return;
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