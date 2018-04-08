using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///LoginPageBase 的摘要说明
/// </summary>
public class LoginPageBase : System.Web.UI.Page
{
    public LoginPageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    #region  重新System.Web.UI.Page基类方法
    //重写基类中的抽象方法init
    override protected void OnInit(EventArgs e)
    {
        try
        {
            if (!(HttpContext.Current.Session["UserModel"] is LoginModel))
            {
                HttpCookie cookie = Request.Cookies["loginmodel"];
                if (cookie != null)
                {
                    string token = Util.md5(cookie.Value + "yibanmed.com");
                    HttpCookie cookie2 = Request.Cookies["token"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Value == token)
                        {
                            HttpCookie cookie4 = new HttpCookie("login_state", "1");
                            cookie4.Expires = DateTime.Now.AddDays(30);
                            cookie4.HttpOnly = true;
                            Response.Cookies.Add(cookie4);
                            HttpCookie cookie3 = Request.Cookies["login_type"];
                            if (cookie3 == null)
                                cookie3 = Request.Cookies["type"];
                            List<Hi.Model.SYS_Users> l = new Hi.BLL.SYS_Users().GetList("", "isnull(dr,0)=0 and isenabled=1 and username='" + cookie.Value + "'", "");
                            if (cookie3.Value == "")
                                return;
                            Hi.Model.SYS_CompUser model = new Hi.BLL.SYS_CompUser().GetModel(cookie3.Value.ToInt(0));
                            LoginModel umodel = new LoginModel();

                            if (model.CType == 1)
                            {
                                Hi.Model.BD_Company model2 = new Hi.BLL.BD_Company().GetModel(model.CompID);
                                umodel.CompName = model2.CompName;
                                umodel.Erptype = model2.Erptype;
                            }
                            else
                            {
                                Hi.Model.BD_Distributor model3 = new Hi.BLL.BD_Distributor().GetModel(model.DisID);
                                umodel.DisName = model3.DisName;
                            }
                            string loginUrl = string.Empty;
                            if (Request.UrlReferrer == null)
                            {
                                loginUrl = "index.html";
                            }
                            else
                            {
                                loginUrl = Request.UrlReferrer.PathAndQuery;
                                if (loginUrl == "/")
                                {
                                    loginUrl = "index.html";
                                }
                                else
                                {
                                    loginUrl = loginUrl.Replace("/", "");
                                }
                            }
                            umodel.Url = loginUrl;
                            umodel.UserName = l[0].UserName;
                            umodel.TrueName = l[0].TrueName;
                            umodel.UserID = l[0].ID;
                            umodel.TypeID = model.UType;
                            umodel.Ctype = model.CType;
                            umodel.CompID = model.CompID;
                            umodel.DisID = model.DisID;
                            umodel.Phone = l[0].Phone;
                            umodel.CUID = Common.DesEncrypt(model.ID.ToString(), Common.EncryptKey);
                            umodel.IsPhoneLogin = false;
                            Session.Remove("UserModel");
                            Session["UserModel"] = umodel;
                            Utils.EditLog("安全日志", l[0].UserName, "用户" + l[0].UserName + "自动登录管理系统成功。", "系统安全模块", loginUrl, 0, 1, model.UType);
                        }
                    }
                }
            }
        }
        catch { }
    }
    #endregion
}