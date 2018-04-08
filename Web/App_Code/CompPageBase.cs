using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

/// <summary>
///CompPageBase 的摘要说明
/// </summary>
public class CompPageBase : System.Web.UI.Page
{
    public static string SysCompFoder = ConfigurationManager.AppSettings["SysCompFoder"];
    public static int PageSize = Convert.ToInt16(ConfigurationManager.AppSettings["PageSize"]);
    private const string MyUserNameKey = "__MYUserName";
    private const string MyTokenKey = "__MYToken";
    LoginModel model = null;

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
        this.Error += new EventHandler(CompPageBase_Error);
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
            HttpCookie cookie4 = Request.Cookies["login_state"];
            if (cookie4 != null)
            {
                if (cookie4.Value == "1")
                {
                    Session.Remove("UserModel");
                    HttpContext.Current.Response.Redirect("~/login.aspx", true);
                }
            }
            if (!(HttpContext.Current.Session["UserModel"] is LoginModel))
            {
                HttpContext.Current.Response.Redirect("~/login.aspx", true);
            }
            else
            {
                model = HttpContext.Current.Session["UserModel"] as LoginModel;
            }
            if ((TypeID != 3 && TypeID != 4&& TypeID != 6) || CompID == 0 || UserID == 0)
            {
                HttpContext.Current.Response.Redirect("~/login.aspx", true);
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

            int CInt = 0;
            if (Request["KeyID"] != null && Request["KeyID"].ToString() != "")
            {
                if (int.TryParse(Request["KeyID"].Trim(), out CInt))
                    KeyID = CInt;
                else
                {
                    string Str = Common.DesDecrypt(Request["KeyID"].Trim(), Common.EncryptKey);
                    if (Str == "")
                    {
                        KeyID = 0;
                    }
                    else
                    {
                        KeyID = Str.ToInt(0);
                    }
                }
            }
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
            HttpContext.Current.Response.Redirect("~/error.aspx?errortype=5");
        }
    }

    #endregion

    #region  公用属性

    /// <summary>
    /// 系统登录用户ID
    /// </summary>
    public int UserID
    {
        get { return model.UserID; }
    }
    public int Erptype
    {
        get { return model.Erptype; }
    }

    /// <summary>
    /// 系统登录用户名
    /// </summary>
    public string UserName
    {
        get { return model.UserName; }
    } /// <summary>
      /// 登录帐号是否绑定销售业务员
      /// </summary>
    public int DisSalesManID
    {
        get {
            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("DisSalesManID", "   userid=" + model.UserID + " and Compid=" + CompID + " and ctype=1 ", "");
            if (ListCompUser.Count>0)
            {
                return ListCompUser[0].DisSalesManID;
            }
            return 0;
        }
    }

    /// <summary>
    /// 企业CompID
    /// </summary>
    public int CompID
    {
        get { return model.CompID; }
    }

    /// <summary>
    /// 系统登录用户名
    /// </summary>
    public string CompName
    {
        get { return "医站通 B2B电子商务平台"; }
    }
    /// <summary>
    /// 用户类别Id
    /// </summary>
    public int TypeID
    {
        get { return model.TypeID; }
    }


    public Hi.Model.SYS_Users CompUser
    {
        get
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel((HttpContext.Current.Session["UserModel"] as LoginModel).UserID);
                return user;
            }
            return null;
        }
    }

    #endregion


    virtual protected void InitEventHandler()
    {
    }

    void CompPageBase_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();
    }

    public CompPageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
}