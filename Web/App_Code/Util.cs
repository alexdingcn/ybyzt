using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
///Util 的摘要说明
/// </summary>
public class Util
{
    public Util()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    ///   <summary>   
    ///   判断字符串是否为空
    ///   </summary>   
    ///   <param name="s"></param>   
    ///   <returns></returns>  
    public static bool IsEmpty(string s)
    {
        if ((s == null) || (s.Length == 0))
            return true;
        else
            return false;
    }
    public static string md5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
    }
    /// <summary>
    /// 按字符数取出字符串
    /// </summary>
    /// <param name="Old">需要截取的字符</param>
    /// <param name="len">截取长度</param>
    /// <returns></returns>
    public static string GetSubString(object Old, int len)
    {
        string strOld = Old.ToString();
        if (strOld.Length > len)
        {
            strOld = strOld.Substring(0, len) + "...";
        }
        return strOld;
    }

    /// <summary>
    /// sha1加密方法
    /// </summary>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public static string SHA1Encrypt(string pwd)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "SHA1").ToLower();
    }


    /////Admin后台加密方式//////////////
    //定义加密字符串
    public static string _salt = "1qaz@,.;#sQWER2017";
    //加密Token
    public static string CreateToken()
    {
        return md5(HttpContext.Current.Session.SessionID + _salt);
        //return md5(DateTime.Now.ToString() + _salt);
    }
    public static string CreateToken(string value)
    {
        return md5(value + _salt);
    }
    //保存Token
    public static string GetToken()
    {
        string token = CreateToken();
        //一般保存在session中
        HttpContext.Current.Session["CRSFToken"] = token;
        return token;
    }
    //获取Token
    public static void TokenIsOK(object token)
    {
        try
        {
            if (token == null || HttpContext.Current.Session["CRSFToken"] == null)
            {
                HttpContext.Current.Response.Redirect("~/error.aspx?errortype=4");
            }
            else
            {
                string tokenInServer = HttpContext.Current.Session["CRSFToken"].ToString();
                if (!tokenInServer.Equals(token.ToString()))
                {
                    HttpContext.Current.Response.Redirect("~/error.aspx?errortype=4");
                }
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Redirect("~/error.aspx?errortype=4");
        }
    }
    /////////////////////////end///////////////////////////

    //分析用户请求是否正常
    public void StartProcessRequest()
    {
        try
        {
            string getkeys = "";
            if (System.Web.HttpContext.Current.Request.QueryString != null)
            {
                for (int i = 0; i < System.Web.HttpContext.Current.Request.QueryString.Count; i++)
                {
                    getkeys = System.Web.HttpContext.Current.Request.QueryString.Keys[i];
                    if (ProcessSqlStr(System.Web.HttpContext.Current.Request.QueryString[getkeys].ToLower()))
                    {
                        System.Web.HttpContext.Current.Response.Redirect("~/error.aspx?errortype=3");
                        System.Web.HttpContext.Current.Response.End();
                    }
                }
            }
        }
        catch
        {
            System.Web.HttpContext.Current.Response.Redirect("~/error.aspx?errortype=3");
            System.Web.HttpContext.Current.Response.End();
        }
    }
    /**/
    /// <summary>   
    /// 分析用户请求是否正常   
    /// </summary>   
    /// <param name="Str">传入用户提交数据</param>   
    /// <returns>返回是否含有SQL注入式攻击代码</returns>   
    private bool ProcessSqlStr(string Str)
    {
        bool ReturnValue = false;
        try
        {
            if (Str != "" && Str != null)
            {
                string SqlStr = "";
                //exec|insert|delete|drop|truncate|update|declare|frame|or|style|expression|and|select|create|script|img|body|meta|object|alert|href
                SqlStr = @"('|insert|select|delete|update|master|truncate|declare)";
                ReturnValue = Regex.IsMatch(Str, SqlStr);
                return ReturnValue;
            }
        }
        catch (Exception ex)
        {
            ReturnValue = false;
        }
        return ReturnValue;
    }

}