<%@ WebHandler Language="C#" Class="login" %>

using System;
using System.Web;
using DBUtility;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

public class login : IHttpHandler, IRequiresSessionState
{
    JavaScriptSerializer js = new JavaScriptSerializer();
    AccountCheck Acheck = null;
    public void ProcessRequest(HttpContext context)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        string loginUrl = "";
        if (context.Request.UrlReferrer != null)
        {
            loginUrl = context.Request.UrlReferrer.PathAndQuery;

            if (loginUrl == "/")
            {
                loginUrl = "index.html";
            }
            else
            {
                loginUrl = loginUrl.Replace("/", "");
            }
        }
        if (!DBHelper.IsOpen())
        {
            context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-系统无法连接数据服务器，请联系网站客服！\"}");
            return;
        }
        List<Hi.Model.SYS_CompUser> ListCompUser = null;
        List<Hi.Model.SYS_Users> ListUsers = null;
        Hi.Model.SYS_PhoneCode PhoneModel = null;
        string Username = "";
        string Password = "";
        string Code = "";
        string PhoneCode = "";
        string Phone = "";
        string SubmitAcion = context.Request["SubmitAcion"];
        string Chckcode = context.Session["CheckCode"] != null ? context.Session["CheckCode"].ToString() : "";
        bool IsphoneLogin = false;
        bool IsAccountCheck = false;
        bool isReturnMsg = false;
        bool ischklgoin = false;//是否自动登录
        try
        {
            if (context.Session["UserModel"] != null && (SubmitAcion == "AccuntLogin" || SubmitAcion == "PhoneLogin"))
            {
                LoginModel Umodel = context.Session["UserModel"] as LoginModel;
                if (Umodel != null)
                {
                    if (Umodel.CompID != 0)
                    {
                        Msg.Msg = "您的帐号已经登录，请退出后操作。";
                        return;
                    }
                    else
                    {
                        context.Session.Remove("UserModel");
                    }
                }
                else
                {
                    context.Session.Remove("UserModel");
                }
            }
            switch (SubmitAcion)
            {
                //帐号登录
                case "AccuntLogin":
                    Boolean.TryParse((context.Request["chklogin"] + "").Trim(),out ischklgoin);
                    Username =Common.NoHTML( (context.Request["Username"] + "").Trim());
                    Password = context.Request["Password"].Trim();
                    Code = (context.Request["Code"] + "").Trim();
                    if (string.IsNullOrWhiteSpace(Username))
                    {
                        Msg.Msg = "用户名不能为空";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        Msg.Msg = "登录密码不能为空";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Code))
                    {
                        Msg.Msg = "验证码不能为空";
                        return;
                    }
                    if (Code != Chckcode)
                    {
                        Msg.Msg = "图文验证码错误！";
                        return;
                    }
                    //用户名查询
                    // TODO: 选择list出来判断
                    ListUsers = new Hi.BLL.SYS_Users().GetListUser("top 1 *", "Username", Username, "");
                    //手机查询
                    if (ListUsers.Count <= 0)
                        ListUsers = new Hi.BLL.SYS_Users().GetListUser("top 1 *", "Phone", Username, "");

                    if (ListUsers.Count > 0)
                    {
                        if (ListUsers.Where(T => T.IsEnabled == 1).ToList().Count == 0)
                        {
                            Msg.Msg = "用户已被禁用！";
                            return;
                        }
                        if (Password != Util.SHA1Encrypt(Util.SHA1Encrypt(ListUsers[0].UserPwd)))
                        {
                            //登录录日志
                            Utils.EditLog("安全日志", Username, "用户" + Username + "登录管理系统失败，密码错误。", "系统安全模块", loginUrl, 0, 0, ListUsers[0].Type);
                            Msg.Msg = "用户名或密码错误！";
                            return;
                        }
                        //用户明细表
                        ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and  Userid=" + ListUsers[0].ID + "", " createdate ");
                        Msg.Result = true;
                    }
                    else
                    {
                        Msg.Msg = "用户名或密码错误！";
                        return;
                    }
                    ; break;

                //手机登录
                case "PhoneLogin":
                    ischklgoin = Convert.ToBoolean((context.Request["chklogin"] + "").Trim());
                    Phone =Common.NoHTML( context.Request["Phone"]);
                    PhoneCode = context.Request["PhoneCode"];
                    Code = context.Request["Code"];
                    if (string.IsNullOrWhiteSpace(Phone))
                    {
                        Msg.Msg = "登录手机号码不能为空。";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(PhoneCode))
                    {
                        Msg.Msg = "手机验证码不能为空";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Code))
                    {
                        Msg.Msg = "验证码不能为空";
                        return;
                    }
                    Regex Phonereg = new Regex("^0?1[0-9]{10}$");
                    if (!Phonereg.IsMatch(Phone))
                    {
                        Msg.Msg = "手机号码格式错误。";
                        return;
                    }
                    ListUsers = new Hi.BLL.SYS_Users().GetListUser("*", "Phone", Phone, "");
                    if (ListUsers.Count == 0)
                    {
                        Msg.Msg = "该手机未绑定用户！";
                        return;
                    }
                    if (Code != Chckcode)
                    {
                        Msg.Msg = "图文验证码错误！";
                        return;
                    }
                    List<Hi.Model.SYS_PhoneCode> ListCode = new Hi.BLL.SYS_PhoneCode().GetListModel("手机登录", Phone);
                    if (ListCode.Count == 0)
                    {
                        Msg.Msg = "请发送手机验证码！";
                        return;
                    }
                    ListCode = ListCode.Where(T => T.PhoneCode == PhoneCode).ToList();
                    if (ListCode.Count == 0)
                    {
                        Msg.Msg = "短信验证码错误！";
                        return;
                    }
                    if (ListCode[0].CreateDate.AddMinutes(30) < DateTime.Now)
                    {
                        Msg.Msg = "短信验证码已过期，请重新获取！";
                        return;
                    }
                    ListUsers = ListUsers.Where(T => T.IsEnabled == 1).ToList();
                    if (ListUsers.Count == 0)
                    {
                        Msg.Msg = "用户已被禁用！";
                        return;
                    }
                    string Userid = string.Join(",", ListUsers.Select(T => T.ID));
                    ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and  Userid in(" + Userid + ")", " createdate ");

                    Msg.Result = true;
                    IsphoneLogin = true;
                    PhoneModel = ListCode[0];
                    ; break;
                case "AccuntSwitch":
                    ischklgoin = Convert.ToBoolean((context.Request["chklogin"] + "").Trim());
                    Acheck = context.Session["UserModel"] as AccountCheck;
                    if (Acheck == null)
                    {
                        Msg.Msg = "请通过登录验证后，选择角色。";
                        return;
                    }
                    string Tip = context.Request["Tip"];
                    Tip = Common.DesDecrypt(Tip, Common.EncryptKey);
                    if (string.IsNullOrEmpty(Tip))
                    {
                        Msg.Msg = "请求参数错误。";
                        return;
                    }
                    ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and Userid in(" + Acheck.UsersID + ")", " createdate ");
                    if (ListCompUser.Count == 0)
                    {
                        Msg.Msg = "选择该角色失败，用户异常。";
                        return;
                    }
                    ListCompUser = ListCompUser.Where(T => T.ID == Tip.ToInt(0)).ToList();
                    if (ListCompUser.Count == 0)
                    {
                        Msg.Msg = "当前用户未注册该角色。";
                        return;
                    }
                    if ((ListCompUser = ListCompUser.Where(T => T.IsEnabled == 1).ToList()).Count == 0)
                    {
                        Msg.Msg = "选择角色失败，该角色已被禁用。";
                        return;
                    }
                    ListUsers = Acheck.ListUser.Where(T => T.ID == ListCompUser[0].UserID).ToList();
                    if (ListUsers.Count == 0)
                    {
                        Msg.Msg = "选择该角色失败，用户数据异常。";
                        return;
                    }
                    Msg.Result = true;
                    IsAccountCheck = true;
                    IsphoneLogin = Acheck.IsPhoneLogin;
                    PhoneModel = Acheck.PhoneModel;
                    ; break;
                case "CloseAccuntSwitch":
                    if (context.Session["UserModel"] is AccountCheck)
                    {
                        context.Session.Remove("UserModel");
                    }
                    ; break;
                case "CheckCode":
                    isReturnMsg = true;
                    Code = context.Request["Code"];
                    if (string.IsNullOrWhiteSpace(Code))
                    {
                        Msg.Msg = "验证码不能为空";
                        return;
                    }
                    if (Code != Chckcode)
                    {
                        Msg.Msg = "图文验证码错误！";
                        return;
                    }
                    Msg.Result = true;
                    ; break;
                default: Msg.Msg = "请求错误。"; break;
            }
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Msg.Msg = "登录异常，请稍候再试。";
            Utils.EditLog("安全日志", Username, "用户" + Username + "登录管理系统异常，" + ex.Message + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType); ;
        }
        finally
        {
            if (Msg.Result && !isReturnMsg)
            {
                UserLoginStateSave(IsphoneLogin, ListUsers, ListCompUser, Msg, loginUrl, context, PhoneModel, IsAccountCheck, ischklgoin);
            }
            else
            {
                context.Response.Write(js.Serialize(Msg));
                context.Response.End();
            }
        }
    }

    public void UserLoginStateSave(bool IsphoneLogin, List<Hi.Model.SYS_Users> ListUser, List<Hi.Model.SYS_CompUser> ListCompUser, Common.ReturnMessge Msg, string loginUrl, HttpContext context, Hi.Model.SYS_PhoneCode PhoneModel, bool IsAccountCheckLogin = false,bool ischklogin = false)
    {
        Hi.Model.SYS_Users User = ListUser[0];
        string TipName = IsAccountCheckLogin ? "选择该角色失败" : "登录";
        int CompUserID = 0;
        int TypeID = 0;
        int Ctype = 0;
        int CompID = 0;
        int DisID = 0;

        bool IsExistRole = false;
        try
        {
            Msg.Result = false;
            LoginModel Umodel = new LoginModel();
            string ParentGet = request("ParentGet", context);
            if (ParentGet != null)
            {
                Msg.Href = ParentGet;
            }

            if (ListCompUser.Count > 0)
            {
                IsExistRole = true;
                CompUserID = ListCompUser[0].ID;
                TypeID = ListCompUser[0].UType;
                Ctype = ListCompUser[0].CType;
                CompID = ListCompUser[0].CompID;
                DisID = ListCompUser[0].DisID;

                if (ListCompUser[0].CType == 1 && (ListCompUser[0].UType == 3 || ListCompUser[0].UType == 4 || ListCompUser[0].UType == 6))
                {
                    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("id,IsEnabled,AuditState,CompName,Erptype", " isnull(dr,0)=0 and id=" + ListCompUser[0].CompID + " ", "");
                    if (ListComp.Count > 0)
                    {
                        if (ListComp[0].IsEnabled == 0)
                        {
                            Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录厂商管理系统失败，用户绑定的厂商已被禁用CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                            Msg.Msg = "" + TipName + "失败，厂商已被禁用。";
                            return;
                        }
                        if (ListComp[0].AuditState != 2)
                        {
                            Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录厂商管理系统失败，用户绑定的厂商正在审核中CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                            Msg.Msg = "" + TipName + "失败，厂商正在审核中。";
                            return;
                        }
                        if (ListCompUser[0].IsAudit != 2)
                        {
                            Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录厂商管理系统失败，厂商管理员正在审核中CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                            Msg.Msg = "" + TipName + "失败，厂商管理员正在审核中。";
                            return;
                        }
                        Umodel.CompName = ListComp[0].CompName;
                        Umodel.Erptype = ListComp[0].Erptype;
                    }
                    else
                    {
                        Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录厂商管理系统异常，用户绑定的厂商已被删除CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                        Msg.Msg = "" + TipName + "失败，厂商不存在。";
                        return;
                    }
                }
                else if (ListCompUser[0].CType == 2 && (ListCompUser[0].UType == 1 || ListCompUser[0].UType == 5))
                {
                    List<Hi.Model.BD_Distributor> ListDis = new Hi.BLL.BD_Distributor().GetList("id,IsEnabled,AuditState,DisName,Compid", " isnull(dr,0)=0 and id=" + ListCompUser[0].DisID + " ", "");
                    if (ListDis == null || ListDis.Count > 1)
                    {
                        Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录" + (ListCompUser[0].CType == 1 ? "厂商" : "代理商") + "管理系统异常，关联多个代理商。CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                        Msg.Msg = "登录失败，该帐号数据异常，请联系客服。";
                        return;
                    }
                    if (ListDis.Count == 1)
                    {
                        if (ListCompUser[0].CompID > 0)
                        {
                            List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("IsEnabled", " isnull(dr,0)=0 and id=" + ListCompUser[0].CompID + " ", "");
                            if (ListComp.Count == 0)
                            {
                                Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录代理商管理系统失败，企业已被删除CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                                Msg.Msg = "" + TipName + "失败，代理商所属企业不存在。";
                                return;
                            }
                            else
                            {
                                if (ListComp[0].IsEnabled == 0)
                                {
                                    Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录代理商管理系统失败，企业已被删除CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                                    Msg.Msg = "" + TipName + "失败，代理商所属企业已被禁用。";
                                    return;
                                }
                            }
                            Umodel.DisName = ListDis[0].DisName;
                        }
                    }
                    else
                    {
                        Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录代理商管理系统异常，用户绑定的代理商已被删除CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                        Msg.Msg = "" + TipName + "失败，代理商不存在。";
                        return;
                    }
                }
                else
                {
                    Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录" + (ListCompUser[0].CType == 1 ? "厂商" : "代理商") + "管理系统异常，用户明细表数据异常Utype或Ctype类型异常CompUserId：" + ListCompUser[0].ID + "。", "系统安全模块", loginUrl, 0, 0, ListCompUser[0].UType);
                    Msg.Msg = "登录失败，用户异常。";
                    return;
                }
            }
            else if (ListCompUser.Count <= 0)
            {
                if (User.AuditState != 2)
                {
                    Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录厂商管理系统失败，用户未审核。", "系统安全模块", loginUrl, 0, 0, 0);
                    Msg.Msg = "" + User.UserName + "登录失败，用户未审核通过。";
                    return;
                }
                IsExistRole = false;
                TypeID = 5;
                DisID = User.DisID;
            }

            Umodel.Url = loginUrl;
            Umodel.UserName = User.UserName;
            Umodel.TrueName = User.TrueName;
            Umodel.UserID = User.ID;
            Umodel.TypeID = TypeID;
            Umodel.Ctype = Ctype;
            Umodel.CompID = CompID;
            Umodel.DisID = DisID;
            Umodel.Phone = User.Phone;
            Umodel.CUID = Common.DesEncrypt(CompUserID.ToString(), Common.EncryptKey);
            Umodel.IsPhoneLogin = IsphoneLogin;
            Umodel.IsExistRole = IsExistRole;
            context.Session.Remove("UserModel");
            context.Session["UserModel"] = Umodel;
            Msg.Result = true;
            Msg.Type = TypeID;
            Msg.Compid = Umodel.CompID;
            if (Acheck != null)
            {
                Msg.IsRegis = Acheck.IsWindowLogin;
            }
            if (IsphoneLogin)
            {
                if (PhoneModel != null)
                {
                    PhoneModel.ts = DateTime.Now;
                    PhoneModel.IsPast = 1;
                    new Hi.BLL.SYS_PhoneCode().Update(PhoneModel);
                }
            }
            Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录管理系统成功。", "系统安全模块", loginUrl, 0, 1,TypeID);
            if (ischklogin)
            {
                HttpCookie cookie = new HttpCookie("loginmodel", System.Web.HttpUtility.UrlEncode(User.UserName));
                cookie.Expires = DateTime.Now.AddDays(30);
                cookie.HttpOnly = true;
                context.Response.Cookies.Add(cookie);
                string token = Util.md5(User.UserName + "yibanmed.com");
                HttpCookie cookie2 = new HttpCookie("token", System.Web.HttpUtility.UrlEncode(token));
                cookie2.Expires = DateTime.Now.AddDays(30);
                cookie2.HttpOnly = true;
                context.Response.Cookies.Add(cookie2);
                HttpCookie cookie3 = new HttpCookie("login_type", System.Web.HttpUtility.UrlEncode(CompUserID.ToString()));
                cookie3.Expires = DateTime.Now.AddDays(30);
                cookie3.HttpOnly = true;
                context.Response.Cookies.Add(cookie3);
                HttpCookie cookie4 = new HttpCookie("login_state", "0");
                cookie4.Expires = DateTime.Now.AddDays(30);
                cookie4.HttpOnly = true;
                context.Response.Cookies.Add(cookie4);
            }
            else
            {
                if (context.Request.Cookies["loginmodel"] != null)
                {
                    HttpCookie cookie = new HttpCookie("loginmodel");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.HttpOnly = true;
                    context.Response.Cookies.Add(cookie);
                }
                if (context.Request.Cookies["token"] != null)
                {
                    HttpCookie cookie = new HttpCookie("token");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.HttpOnly = true;
                    context.Response.Cookies.Add(cookie);
                }
                if (context.Request.Cookies["login_type"] != null)
                {
                    HttpCookie cookie = new HttpCookie("login_type");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.HttpOnly = true;
                    context.Response.Cookies.Add(cookie);
                }
                if (context.Request.Cookies["login_state"] != null)
                {
                    HttpCookie cookie = new HttpCookie("login_state");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.HttpOnly = true;
                    context.Response.Cookies.Add(cookie);
                }
            }

            //添加token验证
            var myTokenValue = Guid.NewGuid().ToString("N");
            var responseCookie = new HttpCookie(MyTokenKey)
            {
                HttpOnly = true,
                Value = myTokenValue,
                Expires = DateTime.Now.AddDays(7)
            };
            context.Response.Cookies.Set(responseCookie);
            context.Session[MyUserNameKey] = Util.CreateToken(User.UserName);
            context.Session[MyTokenKey] = myTokenValue;
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            context.Session.Remove("UserModel");
            Msg.Msg = "" + TipName + "异常，请稍候再试。";
            Utils.EditLog("安全日志", User.UserName, "用户" + User.UserName + "登录管理系统异常，" + ex.Message + "。", "系统安全模块", loginUrl, 0, 0, TypeID);
        }
        finally
        {
            context.Response.Write(js.Serialize(Msg));
            context.Response.End();
        }
    }

    //申明变量
    private const string MyUserNameKey = "__MYUserName";
    private const string MyTokenKey = "__MYToken";

    public static string request(string paras, HttpContext context)
    {
        string url = context.Request.UrlReferrer.ToString();
        if (url.IndexOf("?") == -1)
        {
            return null;
        }
        string[] paraString = url.Substring(url.IndexOf("?") + 1, url.Length - url.IndexOf("?") - 1).Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        SortedDictionary<string, string> paraObj = new SortedDictionary<string, string>();
        string value = string.Empty;
        for (int i = 0; i < paraString.Length; i++)
        {
            value = paraString[i];
            paraObj.Add(value.Substring(0, value.IndexOf("=")).ToLower(), value.Substring(value.IndexOf("=") + 1, value.Length - value.IndexOf("=") - 1));
        }
        if (paraObj.ContainsKey(paras.ToLower()))
        {
            value = paraObj[paras.ToLower()];
            value = LoginModel.DesDecrypt(value, LoginModel.EncryptKey);
            return value;
        }
        return null;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}