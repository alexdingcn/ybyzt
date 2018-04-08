using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for LoginModel
/// </summary>
[Serializable]
public class LoginModel
{
    public static string EncryptKey = "HiEShop@.2015M";
    private string url;
    /// <summary>
    /// 登录来源
    /// </summary>
    public string Url
    {
        get { return url; }
        set { url = value; }
    }
    private int Userid;
    /// <summary>
    /// 用户主ID
    /// </summary>
    public int UserID
    {
        get { return Userid; }
        set { Userid = value; }
    }
    private string Username;
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName
    {
        get { return Username; }
        set { Username = value; }
    }


    private string Truename;
    /// <summary>
    /// 姓名
    /// </summary>
    public string TrueName
    {
        get { return Truename; }
        set { Truename = value; }
    }


    private int Compid;
    /// <summary>
    ///企业ID
    /// </summary>
    public int CompID
    {
        get { return Compid; }
        set { Compid = value; }
    }
    private int TypeiD;
    /// <summary>
    ///用户类别ID
    /// </summary>
    public int TypeID
    {
        get { return TypeiD; }
        set { TypeiD = value; }
    }

    private int Disid;
    /// <summary>
    ///代理商ID
    /// </summary>
    public int DisID
    {
        get { return Disid; }
        set { Disid = value; }
    }

    private int enrptype;
    /// <summary>
    /// 企业来源
    /// </summary>
    public int Erptype
    {
        get { return enrptype; }
        set { enrptype = value; }
    }

    /// <summary>
    /// 厂商名称
    /// </summary>
    public string CompName
    {
        get;
        set;
    }
    /// <summary>
    /// 代理商名称
    /// </summary>
    public string DisName
    {
        get;
        set;
    }

    /// <summary>
    /// 角色类型  1：厂商  2：:代理商
    /// </summary>
    public int Ctype
    {
        get;
        set;
    }

    /// <summary>
    /// 用户明细表ID
    /// </summary>
    public string CUID
    {
        get;
        set;
    }

    /// <summary>
    /// 用户是否审核
    /// </summary>
    public int IsAudit
    {
        get;
        set;
    }

    /// <summary>
    /// 用户手机号
    /// </summary>
    public string Phone
    {
        get;
        set;
    }

    /// <summary>
    /// 用户是否通过手机登陆
    /// </summary>
    public bool IsPhoneLogin
    {
        get;
        set;
    }

    /// <summary>
    /// 用户是否存在用户明细
    /// </summary>
    public bool IsExistRole
    {
        get;
        set;
    }

    /// <summary>
    /// 用户是否存在用户明细
    /// </summary>
    public static bool ExistRoleComp()
    {
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                LoginModel UserModel = HttpContext.Current.Session["UserModel"] as LoginModel;

                List<Hi.Model.SYS_CompUser> copmuserlist = new Hi.BLL.SYS_CompUser().GetList("", " isnull(dr,0)=0 and UserID=" + UserModel.UserID + " and CType=2 and IsAudit=2 and IsEnabled=1 and UType in(1,5)", "");

                if (copmuserlist != null && copmuserlist.Count > 0) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 判断是否登录
    /// </summary>
    /// <returns></returns>
    public static bool IsLogin()
    {
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                LoginModel UserModel = HttpContext.Current.Session["UserModel"] as LoginModel;
                if (UserModel.Ctype == 2 && (UserModel.TypeID == 1 || UserModel.TypeID == 5))
                    return true;
            }
        }
        return false; ;
    }

    /// <summary>
    /// 判断是否登录
    /// </summary>
    /// <returns></returns>
    public static Hi.Model.SYS_Users IsLoginUser()
    {
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel((HttpContext.Current.Session["UserModel"] as LoginModel).UserID);
                if (user != null)
                {
                    if (user.dr == 0 && user.IsEnabled != 0 && (user.Type == 1 || user.Type == 5))
                        return user;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 判断是否登录
    /// </summary>
    /// <returns></returns>
    public static bool IsLoginAll()
    {
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] != null)
            {
                if (HttpContext.Current.Session["UserModel"] is LoginModel)
                {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 判断是否登录
    /// </summary>
    /// <returns></returns>
    public static Hi.Model.SYS_Users IsLoginAllUser()
    {
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel((HttpContext.Current.Session["UserModel"] as LoginModel).UserID);
                if (user != null)
                {
                    if (user.dr == 0 && user.IsEnabled != 0)
                        return user;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 判断是否登录，并获取登录信息
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static Hi.Model.SYS_Users IsLogined(System.Web.UI.Page page)
    {
        string OpenUrl = page.Request.AppRelativeCurrentExecutionFilePath;
        foreach (string key in page.Request.QueryString.Keys)
        {
            if (OpenUrl.Contains("?"))
            {
                OpenUrl += key + "=" + page.Request.QueryString[key] + "&";
            }
            else
            {
                OpenUrl += "?" + key + "=" + page.Request.QueryString[key] + "&";
            }
        }
        if (OpenUrl[OpenUrl.Length - 1] == '&')
        {
            OpenUrl = OpenUrl.Substring(0, OpenUrl.Length - 1);
        }
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel((HttpContext.Current.Session["UserModel"] as LoginModel).UserID);
                if (User != null)
                {
                    if (User.dr == 0 && User.IsEnabled != 0 && (User.Type == 1 || User.Type == 5))
                    {
                        return User;
                    }
                    else
                    {
                        page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
                    }
                }
                else
                {
                    page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
                }
            }
            else
            {
                page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
            }
        }
        else
        {
            if (page != null)
            {
                page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
            }
        }
        return null;
    }


    public static Hi.Model.SYS_Users IsLoginedAll(System.Web.UI.Page page)
    {
        string OpenUrl = page.Request.AppRelativeCurrentExecutionFilePath;
        foreach (string key in page.Request.QueryString.Keys)
        {
            if (OpenUrl.Contains("?"))
            {
                OpenUrl += key + "=" + page.Request.QueryString[key] + "&";
            }
            else
            {
                OpenUrl += "?" + key + "=" + page.Request.QueryString[key] + "&";
            }
        }
        if (OpenUrl[OpenUrl.Length - 1] == '&')
        {
            OpenUrl = OpenUrl.Substring(0, OpenUrl.Length - 1);
        }
        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel((HttpContext.Current.Session["UserModel"] as LoginModel).UserID);
                if (User != null)
                {
                    if (User.dr == 0 && User.IsEnabled != 0)
                    {
                        return User;
                    }
                    else
                    {
                        page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
                    }
                }
                else
                {
                    page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
                }
            }
            else
            {
                page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
            }
        }
        else
        {
            if (page != null)
            {
                page.Response.Redirect("~/login.aspx?GetType=" + page.Server.UrlEncode(DesEncrypt(OpenUrl, EncryptKey)), true);
            }
        }
        return null;
    }

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="inputString">要进行加密的字符串</param>
    /// <param name="encryptKey">需要进行解密操作的密钥</param>
    /// <param name="DesEncrypt">返回加密后的字符串</param>
    public static string DesEncrypt(string inputString, string encryptKey)
    {
        byte[] byKey = null;
        try
        {
            byte[] IV = UTF8Encoding.UTF8.GetBytes("SYJ2015SORFTJIAMI");
            if (encryptKey.Length > 16)
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
            }
            else
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey);
            }
            RijndaelManaged rij = new RijndaelManaged();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, rij.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string ToBase = Convert.ToBase64String(ms.ToArray());
            return HttpContext.Current.Server.UrlEncode(ToBase);
        }
        catch
        {
            //return error.Message;
            return "";
        }
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="inputString">加了密的字符串</param>
    /// <param name="decryptKey">解密的密钥</param>
    /// <param name="DesDecrypt">返回解密后的字符串</param>
    public static string DesDecrypt(string inputString, string decryptKey)
    {
        byte[] byKey = null;
        try
        {
            if (!(inputString.IndexOf("+") >= 0) && !(inputString.IndexOf("/") >= 0))
            {
                inputString = HttpContext.Current.Server.UrlDecode(inputString);
            }
            byte[] IV = UTF8Encoding.UTF8.GetBytes("SYJ2015SORFTJIAMI");
            byte[] inputByteArray = new Byte[inputString.Length];
            if (decryptKey.Length > 16)
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
            }
            else
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey);
            }
            RijndaelManaged rij = new RijndaelManaged();
            inputByteArray = Convert.FromBase64String(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, rij.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        catch
        {
            //return error.Message;
            return "";
        }
    }

    /// <summary>
    /// 获取登录用户厂商ID
    /// </summary>
    /// <param name="UserID"></param>
    /// <returns></returns>
    public static int GetUserCompID(string UserID)
    {
        List<Hi.Model.SYS_CompUser> culist = new Hi.BLL.SYS_CompUser().GetList("", " UserID=" + UserID + " and Ctype=1 and UType=4", "");
        if (culist != null && culist.Count > 0) {
            return culist[0].CompID;
        }
        return 0;
    }

}