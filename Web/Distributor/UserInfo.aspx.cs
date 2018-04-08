

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_UserInfo : DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    //public int DisId;
    //public int CompID;
    //public int KeyID = 0;
    //public int UserID = 0;
    public string UID = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // UID = Common.DesDecrypt(Request["userID"].ToString(), Common.EncryptKey);
            DataBinds();
            //绑定权限  RepeaterRoles
            List<Hi.Model.SYS_Role> LDis = new Hi.BLL.SYS_Role().GetList("", " DisID=" + DisID + "  AND dr=0 AND IsEnabled=1 ", "Createdate", null);
            this.RepeaterRoles.DataSource = LDis;
            this.RepeaterRoles.DataBind();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            //Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(KeyID);
            int pageCount = 0;
            int Counts = 0;
            string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";

            DataTable dt = new Hi.BLL.SYS_CompUser().GetList(1, 1, "SYS_CompUser.ID", true, "SYS_CompUser.DisID,SYS_CompUser.id,Address,SYS_CompUser.RoleId,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,SYS_CompUser.CompID", JoinTableStr, "and SYS_CompUser.UserID= " + KeyID, out pageCount, out Counts);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["DisID"].ToString().ToInt(0) != this.DisID)
                {
                    JScript.AlertMsgOne(this, "该用户不允许查看！", JScript.IconOption.错误, 2500);
                    return;
                }
                if (dt.Rows[0]["IsEnabled"].ToString().ToInt(0) == 1)
                {
                    UseIcon.Visible = false;
                    NUseIcon.Visible = true;
                    DleteIcon.Visible = false;
                }
                else
                {
                    NUseIcon.Visible = false;
                    UseIcon.Visible = true;
                    DleteIcon.Visible = true;
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
                lblAddress.InnerText = dt.Rows[0]["Address"].ToString(); ;
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
        Hi.Model.SYS_CompUser User = new Hi.BLL.SYS_CompUser().GetModel(KeyID);
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and DisID=" + DisID + " and ctype=2 ", "");
        if (ListCompUser.Count > 0)
        {
            if (ListCompUser[0].UType == 5)
            {
                JScript.AlertMsgOne(this, "该用户为管理员，不可禁用！", JScript.IconOption.错误, 2500);
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
    protected void btn_Use(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and DisID=" + DisID + " and ctype=2 ", "");
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
            JScript.AlertMsgOne(this, "用户不存在！", JScript.IconOption.错误, 2500);
            return;
        }
        DataBinds(); 
    }

    protected void btn_Delete(object sender, EventArgs e)
    {
        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("top 1 *", "   UserID=" + KeyID + " and DisID=" + DisID + " and ctype=2 ", "");
        if (ListCompUser.Count > 0)
        {
            if (ListCompUser[0].UType == 5)
            {
                JScript.AlertMsgOne(this, "该用户为管理员，不可删除！", JScript.IconOption.错误, 2500);
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
                Response.Redirect("UsersList.aspx");
            }
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