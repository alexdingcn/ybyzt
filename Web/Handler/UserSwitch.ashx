<%@ WebHandler Language="C#" Class="UserSwitch" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data;
using System.Web.SessionState;
using System.Web.Script.Serialization;

/***
 * 用户切换
 * ***/
public class UserSwitch : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        try
        {
            string action = context.Request["action"] + "";
            switch (action)
            {
                case "UserList":
                    //用户列表
                    context.Response.Write(UserList(context));
                    break;
                case "switch":
                    //切换登录用户
                    USwitch(context);
                    break;
            }
        }
        catch (Exception)
        {
            throw;
        }
        context.Response.End();
    }

    /// <summary>
    /// 用户切换
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public void USwitch(HttpContext context)
    {
        //登录信息
        LoginModel user = context.Session["UserModel"] as LoginModel;
        if (user != null)
        {
            try
            {
                //获取切换角色的路径
                string loginUrl = "";
               if (context.Request.UrlReferrer != null)
                {
                    loginUrl = context.Request.UrlReferrer.PathAndQuery;
                    if (loginUrl == "/")
                        loginUrl = "index.aspx";
                    else
                        loginUrl = loginUrl.Replace("/", "");
                }

                string CUID = Common.DesDecrypt(context.Server.UrlDecode(context.Request["ID"].ToString()), Common.EncryptKey);

                string str = " UserID=" + user.UserID + "and cu.ID=" + CUID + " and isnull(cu.dr,0)=0 and cu.IsEnabled=1";
                DataRow dr = new Hi.BLL.SYS_CompUser().GetComUser(str).Rows[0];

                if (dr != null)
                {
                    //if (dr["IsAudit"].ToString().ToInt(0) != 2)
                    //{
                        //登录成功记录日志
                        //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换失败,账户未审核。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));

                    //    context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-账户未审核,切换失败！\"}");
                    //    return;
                    //}

                    //代理商
                    //if (dr["CType"].ToString().ToInt(0) == 2)
                    //{
                    //    if (dr["disAuditState"].ToString().ToInt(0) != 2)
                    //    {
                    //        //登录成功记录日志
                    //        //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换失败,代理商未审核。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));
                    //        context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-代理商未审核！\"}");
                    //        return;
                    //    }
                    //    if (dr["disIsEnabled"].ToString().ToInt(0) != 1)
                    //    {
                    //        //登录成功记录日志
                    //        //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换失败,代理商已禁用。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));
                    //        context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-代理商已禁用！\"}");
                    //        return;
                    //    }
                    //}
                    //else 
                    if (dr["CType"].ToString().ToInt(0) == 1)
                    {
                        //厂商
                        if (dr["comAuditState"].ToString().ToInt(0) != 2)
                        {
                            //登录成功记录日志
                            //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换失败,厂商未审核。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));
                            context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-厂商未审核！\"}");
                            return;
                        }
                        if (dr["comIsEnabled"].ToString().ToInt(0) != 1)
                        {
                            //登录成功记录日志
                            //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换失败,厂商已禁用。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));

                            context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-厂商已禁用！\"}");
                            return;
                        }
                    }

                    //清空登录的用户信息
                    context.Session["UserModel"] = null;
                    context.Session.Clear();
                    //context.Session.Abandon();

                    LoginModel lmodel = new LoginModel();
                    lmodel.CUID = Common.DesEncrypt(dr["ID"].ToString(), Common.EncryptKey);
                    lmodel.CompID = dr["CompID"].ToString().ToInt(0);
                    lmodel.CompName = dr["CompName"].ToString();
                    if (dr["CType"].ToString().ToInt() == 1 && (dr["UType"].ToString().ToInt() == 3 || dr["UType"].ToString().ToInt() == 4))
                        lmodel.Erptype = dr["Erptype"].ToString().ToInt(0);
                    else if (dr["CType"].ToString().ToInt() == 2 && (dr["UType"].ToString().ToInt() == 1 || dr["UType"].ToString().ToInt() == 5))
                    {
                        lmodel.DisID = dr["DisID"].ToString().ToInt(0);
                        lmodel.DisName = dr["DisName"].ToString();
                    }
                    lmodel.UserID = dr["UserID"].ToString().ToInt(0);
                    lmodel.Phone = dr["Phone"].ToString();
                    lmodel.TypeID = dr["UType"].ToString().ToInt(0);
                    lmodel.Ctype = dr["CType"].ToString().ToInt(0);
                    lmodel.TrueName = dr["TrueName"].ToString();
                    lmodel.UserName = dr["UserName"].ToString();
                    context.Session["UserModel"] = lmodel;

                    //登录成功记录日志
                    //Utils.EditLog("安全日志", dr["UserName"].ToString(), "用户" + dr["UserName"] + "切换账户成功。", "系统安全模块", loginUrl, 0, 1, dr["UType"].ToString().ToInt(0));

                    //添加token验证
                    string MyTokenKey = "__MYToken";
                    string MyUserNameKey = "__MYUserName";
                    var myTokenValue = Guid.NewGuid().ToString("N");
                    var responseCookie = new HttpCookie(MyTokenKey)
                    {
                        HttpOnly = true,
                        Value = myTokenValue,
                        Expires = DateTime.Now.AddDays(7)
                    };
                    context.Response.Cookies.Set(responseCookie);
                    context.Session[MyUserNameKey] = Util.md5(lmodel.UserName + Util._salt);
                    context.Session[MyTokenKey] = myTokenValue;
                    //////////////////////end//////////////////////////
                    
                    context.Response.Write("{\"type\":true,\"typeid\":\"" + dr["UType"].ToString() + "\",\"ctype\":\"" + dr["CType"].ToString() + "\",\"disID\":\"" + dr["DisID"].ToString() + "\",\"CompID\":\"" + dr["CompID"].ToString() + "\",\"str\":\"-切换账户成功！\"}");
                    return;
                }
                else
                {
                    //Utils.EditLog("安全日志", user.UserName, "用户" + user.UserName + "切换失败。", "系统安全模块", loginUrl, 0, 1, user.TypeID);
                    context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-账户异常，请联系客服！\"}");
                    return;
                }
            }
            catch (Exception)
            {
                //Utils.EditLog("安全日志", user.UserName, "用户" + user.UserName + "切换失败。", "系统安全模块", "index.aspx", 0, 1, user.TypeID);
                context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-账户异常，请联系客服！\"}");
                return;
            }
        }
        else
        {
            //Utils.EditLog("安全日志", user.UserName, "用户" + user.UserName + "切换失败。", "系统安全模块", "index.aspx", 0, 1, user.TypeID);
            context.Response.Write("{\"type\":false,\"typeid\":0,\"str\":\"-请先登录！\"}");
            return;
        }
    }

    /// <summary>
    /// 用户列表
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public string UserList(HttpContext context)
    {
        List<LoginModel> lmlist = new List<LoginModel>();
        LoginModel lmodel = null;

        //登录信息
        LoginModel user = context.Session["UserModel"] as LoginModel;
        if (user != null)
        {
            try
            {
                string str = " cu.UserId=" + user.UserID + "and isnull(cu.dr,0)=0 and cu.IsEnabled=1"; //
                DataTable dt = new Hi.BLL.SYS_CompUser().GetComUser(str);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        string CUID = Common.DesDecrypt(context.Server.UrlDecode(user.CUID), Common.EncryptKey);
                        if (CUID == item["ID"].ToString())
                            continue;
                        //代理商
                        //if (item["CType"].ToString().ToInt(0) == 2)
                        //{
                        //    if (item["disAuditState"].ToString().ToInt(0) != 2)
                        //    {
                        //        continue;
                        //    }
                        //    if (item["disIsEnabled"].ToString().ToInt(0) != 1)
                        //    {
                        //        continue;
                        //    }
                        //}
                        //else 
                        if (item["CType"].ToString().ToInt(0) == 1)
                        {
                            //厂商
                            if (item["comAuditState"].ToString().ToInt(0) != 2)
                            {
                                continue;
                            }
                            if (item["comIsEnabled"].ToString().ToInt(0) != 1)
                            {
                                continue;
                            }
                        }

                        lmodel = new LoginModel();
                        lmodel.CUID = Common.DesEncrypt(item["ID"].ToString(), Common.EncryptKey);
                        lmodel.IsAudit = item["IsAudit"].ToString().ToInt(0);
                        lmodel.CompID = item["CompID"].ToString().ToInt(0);
                        lmodel.CompName = item["CompName"].ToString();
                        lmodel.UserID = item["UserID"].ToString().ToInt(0);
                        lmodel.DisID = item["DisID"].ToString().ToInt(0);
                        lmodel.DisName = item["DisName"].ToString();
                        lmodel.TypeID = item["UType"].ToString().ToInt(0);
                        lmodel.Ctype = item["CType"].ToString().ToInt(0);
                        lmodel.TrueName = item["TrueName"].ToString();
                        lmodel.UserName = item["UserName"].ToString();

                        lmlist.Add(lmodel);
                    }
                    context.Session["UserList"] = lmlist;
                }
            }
            catch (Exception)
            { }
        }

        return new JavaScriptSerializer().Serialize(lmlist);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}