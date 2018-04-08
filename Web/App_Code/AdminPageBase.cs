using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
/// <summary>
/// Summary description for AdminPageBase
/// </summary>
public class AdminPageBase : System.Web.UI.Page
{
    public static string SysAdminFoder = ConfigurationManager.AppSettings["SysAdminFoder"];
    private const string MyUserNameKey = "__MYUserName";
    private const string MyTokenKey = "__MYToken";
    /// <summary>
    /// 系统表单主键值
    /// </summary>
    public int KeyID
    {
        get { return Convert.ToInt32(ViewState["KeyID"]); }
        set { ViewState["KeyID"] = value; }
    }
    private string CurrentUser
    {
        get { return HttpContext.Current.Session[MyUserNameKey] as string; }
    }
    private string CurrentToken
    {
        get { return HttpContext.Current.Session[MyTokenKey] as string; }
    }

    #region  重新System.Web.UI.Page基类方法
    //重写基类中的抽象方法init
    override protected void OnInit(EventArgs e)
    {
        Page.Response.Expires = -1;
        base.OnInit(e);
        this.InitEventHandler();

        try
        {
            //Referer验证
            string url = Request.ServerVariables["HTTP_REFERER"];//获取页面来源
            //if (!Util.IsEmpty(url))
            //{
            //    if (url.Substring(0, 7) + url.Substring(7).Substring(0, url.Substring(7).IndexOf('/')) != Common.GetWebConfigKey("WebDomainName"))
            //    {
            //        HttpContext.Current.Response.Redirect("~/Error.aspx?errortype=1");
            //    }
            //}   //edit by hgh  先屏蔽
            //登录验证
            if (HttpContext.Current.Session["AdminUser"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Admin/login.aspx");

            }
            if (UserID == 0)
            {
                HttpContext.Current.Response.Redirect("~/login.aspx");
            }
            //防止跨站点请求伪造验证
            var repquestCookie = Request.Cookies[MyTokenKey];
            Guid requestCookieGuid;
            if (repquestCookie != null && Guid.TryParse(repquestCookie.Value, out requestCookieGuid))
            {
                var _myTokenValue = repquestCookie.Value;
                Page.ViewStateUserKey = _myTokenValue;
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/error.aspx?errortype=5");
            }
            //Token验证
            Page.PreLoad += Page_PreLoad;

            if (!string.IsNullOrWhiteSpace(Request["KeyID"]))
                KeyID = Request["KeyID"].ToInt(0);
            else
                KeyID = 0;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Token验证
    protected void Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState[MyTokenKey] = Page.ViewStateUserKey;
            ViewState[MyUserNameKey] = CurrentUser;
        }
        //判断Token是否赋值
        if (string.IsNullOrWhiteSpace(CurrentToken))
        {
            //跳转到登录页
            HttpContext.Current.Response.Redirect("~/login.aspx", true);
        }
        //判断Token是否一致
        if (CurrentToken != Page.ViewStateUserKey || CurrentUser != (string)ViewState[MyUserNameKey])
        {
            //HttpContext.Current.Response.Redirect("~/error.aspx?errortype=5");
        }
    }
    #endregion

    #region  公用属性

    //////系统登录信息
    /// <summary>
    /// 系统登录用户ID
    /// </summary>
    private int userID;
    /// <summary>
    /// 系统登录用户ID
    /// </summary>
    public int UserID
    {
        get { return Common.LoginID(); }
        set { userID = value; }
    }

    /// <summary>
    /// 系统登录用户名
    /// </summary>
    private string userName;
    /// <summary>
    /// 系统登录用户名
    /// </summary>
    public string UserName
    {
        get { return Common.LoginName(); }
        set { userName = value; }
    }

    /// <summary>
    /// 系统登录用户类型
    /// </summary>
    private int userType;
    /// <summary>
    /// 系统登录用户名
    /// </summary>
    public int UserType
    {
        get 
        {
            if (Session["UserType"] != null)
                return Convert.ToInt16(Session["UserType"].ToString());
            return 0;
        }
    }

    /// <summary>
    /// 获取机构ID
    /// </summary>
    public int OrgID
    {
        get
        {
            int Orgid = 0;
            Hi.Model.SYS_AdminUser AdminUserModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
            if (AdminUserModel != null) {
                if (AdminUserModel.UserType == 3 || AdminUserModel.UserType == 4) {
                    Orgid = AdminUserModel.OrgID;
                }
            }
            return Orgid;
        }
    }
    /// <summary>
    /// 获取机构业务员ID
    /// </summary>
    public int SalesManID
    {
        get
        {
            int SalesManID = 0;
            Hi.Model.SYS_AdminUser AdminUserModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
            if (AdminUserModel != null)
            {
                if (AdminUserModel.UserType == 3 || AdminUserModel.UserType == 4)
                {
                    SalesManID = AdminUserModel.SalesManID;
                }
            }
            return SalesManID;
        }
    }
    #endregion

    #region  javascript脚本注册提示
    /// <summary>
    /// 信息提示
    /// </summary>
    /// <param name="page">this</param>
    /// <param name="msg">提示信息</param>
    public static void ShowAlert(System.Web.UI.Page page, string msg)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>alert('" + msg + "');</script>");
    }

    /// <summary>
    /// 信息提示
    /// </summary>
    /// <param name="page">this</param>
    /// <param name="msg">提示信息</param>
    /// <param name="script">script</param>
    public static void ShowAlert(System.Web.UI.Page page, string msg, string script)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>alert('" + msg + "');" + script + "</script>");
    }



    /// <summary>
    /// 刷新父页面并且弹出提示信息
    /// </summary>
    /// <param name="page">this</param>
    /// <param name="msg">提示信息</param>
    public static void ShowParentRefresh(System.Web.UI.Page page, string msg)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>parent.location.reload();alert('" + msg + "。');</script>");
    }

    /// <summary>
    /// 刷新后执行脚本
    /// </summary>
    /// <param name="page">this</param>
    /// <param name="msg">提示信息</param>
    /// <param name="script">script</param>
    public static void ShowParentRefresh(System.Web.UI.Page page, string msg, string script)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>parent.location.reload();alert('" + msg + "。');" + script + "</script>");
    }

    #endregion


    virtual protected void InitEventHandler()
    {
    }

    public AdminPageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
}