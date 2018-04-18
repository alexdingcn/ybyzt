using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;

public partial class SSOLogin : System.Web.UI.Page
{
    //类型
    private string type;
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
    //用户名
    private string username;
    public string Username
    {
        get { return username; }
        set { username = value; }
    }
    //密码
    private string password;
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    //logo图标
    private string logo;
    public string Logo
    {
        get { return logo; }
        set { logo = value; }
    }
    //企业id
    private string compid;
    public string Compid
    {
        get { return compid; }
        set { compid = value; }
    }

    string loginUrl = "SSOLogin.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Url = Request["user"] + "";
            string sign = Request["sign"] + "";
            string valid = Request["valid"] + "";

            if (sign == "-1" || valid == "-1" || Url == "加密失败")
            {
                this.pError.InnerHtml = "登录信息异常！";
                return;
            } 

            //解密
            string userUrl = new AESHelper().KeyDecrypt(sign, valid ,Url);

            if (userUrl == "-1")
            {
                this.pError.InnerHtml = "认证过期！";
                return;
            }
            else if (userUrl == "-2")
            {
                this.pError.InnerHtml = "认证异常！";
                return;
            }

            JsonData JInfo = JsonMapper.ToObject(userUrl);

            if ( JInfo["username"].ToString() != "" && JInfo["password"].ToString() != "")
            {
                type = JInfo["type"].ToString();
                username = JInfo["username"].ToString();
                password = JInfo["password"].ToString();
                logo = JInfo["logo"].ToString();
                //add by hgh
                compid = JInfo["compid"].ToString();
            }
            else
            {
                this.pError.InnerHtml = "url参数不能为空！";
                return;
            }

            if (!DBHelper.IsOpen())
            {
                this.pError.InnerHtml = "-系统无法连接数据库服务器，请联系管理员！";
                return;
            }
            List<Hi.Model.SYS_CompUser> ListCompUser = null;
            List<Hi.Model.SYS_Users> ListUsers = null;

            ListUsers = new Hi.BLL.SYS_Users().GetListUser("top 1 *", "Username", username, "");
            if (ListUsers.Count > 0)
            {
                if (ListUsers.Where(T => T.IsEnabled == 1).ToList().Count == 0)
                {
                    this.pError.InnerHtml = "用户已被禁用！";
                    return;
                }
                if (Util.md5(Password) != ListUsers[0].UserPwd)
                {
                    //登录录日志
                    Utils.EditLog("安全日志", Username, "用户" + Username + "登录管理系统失败，密码错误。", "系统安全模块", loginUrl, 0, 0, ListUsers[0].Type);
                    this.pError.InnerHtml = "登录密码错误！";
                    return;
                }
                ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and Compid='" + compid + "' and Userid=" + ListUsers[0].ID + "", " createdate ");
                if (ListCompUser.Count > 0)
                {
                    if (ListCompUser.Where(T => T.IsEnabled == 1).ToList().Count == 0)
                    {
                        //登录录日志
                        //Utils.EditLog("安全日志", Username, "用户" + Username + "登录管理系统失败，用户帐号所有角色已被禁用。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                        this.pError.InnerHtml = "您的账户已被禁用！";
                        return;
                    }
                }
                else
                {
                    //登录录日志
                    //Utils.EditLog("安全日志", Username, "用户" + Username + "登录管理系统失败，用户明细表（SYS_CompUser）异常。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                    //Msg.Msg = "用户异常，请联系网站客服！";
                    this.pError.InnerHtml = "用户名或密码错误！";
                    return;
                }

                //UserLogin(ListUsers[0]);
                Hi.Model.SYS_Users User = ListUsers[0];
                LoginModel Umodel = new LoginModel();
                //Umodel.Url = loginUrl;
                if (compid == "1203")
                    Umodel.Url = "/jlc/";
                Umodel.UserName = User.UserName;
                Umodel.TrueName = User.TrueName;
                Umodel.UserID = User.ID;
                Umodel.TypeID = ListCompUser[0].UType;
                Umodel.Ctype = ListCompUser[0].CType;
                Umodel.CompID = ListCompUser[0].CompID;
                Umodel.DisID = ListCompUser[0].DisID;
                Umodel.Phone = User.Phone;
                Umodel.CUID = Common.DesEncrypt(ListCompUser[0].ID.ToString(), Common.EncryptKey);
                //Umodel.IsPhoneLogin = IsphoneLogin;
                Session.Remove("UserModel");
                //string sql = "select rf.FunCode from SYS_RoleSysFun rf join SYS_CompUser u on u.RoleID=rf.RoleID  where u.UserID=" + User.ID;
                //DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                //var query = dt.AsEnumerable().Select(t => t.Field<string>("FunCode"));
                //string Code = string.Join(",", query);
                //Umodel.UserPowerCode = Code;
                Session["UserModel"] = Umodel;

                //if (Umodel.TypeID.ToString() == "1" || Umodel.TypeID.ToString() == "5")//代理商
                //{
                //    Response.Redirect("/Distributor/UserIndex.aspx");
                //}
                if (Umodel.TypeID.ToString() == "3" || Umodel.TypeID.ToString() == "4")//厂商
                {
                    Response.Redirect("/Company/jsc.aspx");
                }
                else
                {
                    Response.Redirect("/Distributor/UserIndex.aspx");
                }

                Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录管理系统成功。", "系统安全模块", loginUrl, 0, 1, ListCompUser[0].UType);

                HttpCookie cookie = new HttpCookie("loginmodel", System.Web.HttpUtility.UrlEncode(User.UserName));
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

            }
            else
            {
                this.pError.InnerHtml = "用户名或密码错误！";
                return;
            }

        }
    }

    protected void UserLogin(Hi.Model.SYS_Users user)
    {
        int Erptype = 0;
        if (user.IsEnabled == 1 && user.AuditState == 2 && user.dr == 0)// AuditState:0：未审 2:已审 
        {
            string username = string.IsNullOrEmpty(user.TrueName) ? user.UserLoginName : user.TrueName;
            LoginModel Umodel = new LoginModel();
            Umodel.UserID = user.ID;
            Umodel.UserName = user.UserName;
            Umodel.TrueName = user.TrueName;
            Umodel.CompID = user.CompID;
            Umodel.DisID = user.DisID;
            Umodel.TypeID = user.Type;
            Umodel.Erptype = Erptype;

            string loginname = user.UserName;

            HttpCookie cookie = null;

            if (user.Type == 1 || user.Type == 5)
            {
                //代理商管理员、代理商用户

                Session["UserModel"] = Umodel;
                //登录成功记录日志
                Utils.EditLog("安全日志", loginname, "单点登录：用户" + loginname + "登录管理系统成功。", "系统安全模块", loginUrl, 0, 1, user.Type);
                cookie = new HttpCookie("loginmodel", System.Web.HttpUtility.UrlEncode(user.UserName));
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                ClientScript.RegisterStartupScript(GetType(), "ssologin", "<script>window.location.href='Distributor/UserIndex.aspx'</script>");
            }
            else if (user.Type == 3 || user.Type == 4)
            {
                //企业用户、企业管理员

                Session["UserModel"] = Umodel;
                //登录成功记录日志
                Utils.EditLog("安全日志", loginname, "单点登录：用户" + loginname + "登录管理系统成功。", "系统安全模块", loginUrl, 0, 1, user.Type);
                cookie = new HttpCookie("loginmodel", System.Web.HttpUtility.UrlEncode(user.UserName));
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                ClientScript.RegisterStartupScript(GetType(), "ssologin", "<script>window.location.href='Company/main.aspx'</script>");
            }
            else if (user.Type == 2)
            {
                //公共用户
            }
            else
            {
                //其他
                this.pError.InnerHtml = "-用户名或密码错误！";
            }
            return;
        }
        else
        {
            this.pError.InnerHtml = "-该用户为禁用状态！";
            return;
        } 
    }

}