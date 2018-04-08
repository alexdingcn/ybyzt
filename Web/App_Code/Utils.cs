using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using DBUtility;

/// <summary>
///Utils 的摘要说明
/// </summary>
public class Utils
{
    public static string Connection = ConfigurationManager.AppSettings["ConnectionString_Log"];
    public Utils()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region 获取web代理商端ip

    /// <summary>
    /// 获得当前页面代理商端的IP
    /// </summary>
    /// <returns>当前页面代理商端的IP</returns>
    public static string GetIP()
    {
        string result = String.Empty;
        //edit by hgh 直接获取代理服务器 HTTP_X_FORWARDED_FOR 参数
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            //获取代理的服务器Ip地址
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null) //判断发出请求的远程主机的ip地址是否为空
        {
            //获取发出请求的远程主机的Ip地址
            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        else
        {
            //result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            //获取代理商端IP
            result = HttpContext.Current.Request.UserHostAddress;
        }
        if (string.IsNullOrEmpty(result))
        {
            return "127.0.0.1";
        }
        return result;


        //if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  //得到穿过代理服务器的ip地址
        //{
        //    //result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        //    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //    {
        //        //获取代理的服务器Ip地址
        //        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        //    }
        //    else
        //    {
        //        //获取代理商端IP
        //        result = HttpContext.Current.Request.UserHostAddress;
        //    }
        //}
        //else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null) //判断发出请求的远程主机的ip地址是否为空
        //{
        //    //获取发出请求的远程主机的Ip地址
        //    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        //}
        //else
        //{
        //    //result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        //    //获取代理商端IP
        //    result = HttpContext.Current.Request.UserHostAddress;
        //}
        //if (string.IsNullOrEmpty(result))
        //{
        //    return "127.0.0.1";
        //}
        //return result;
    }

    
    /// <summary>
    /// 获取web代理商端ip
    /// </summary>
    /// <returns></returns>
    public static string GetWebClientIp()
    {

        string userIP = "::1";

        try
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Request.ServerVariables == null)
                return "";

            string CustomerIP = "";

            //CDN加速后取到的IP simone 090805
            CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            if (!string.IsNullOrEmpty(CustomerIP))
            {
                return CustomerIP;
            }

            CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


            if (!String.IsNullOrEmpty(CustomerIP))
                return CustomerIP;

            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (CustomerIP == null)
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            }

            if (string.Compare(CustomerIP, "unknown", true) == 0)
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            return CustomerIP;
        }
        catch { }

        return userIP;

    }
    #endregion

    /// <summary>
    /// 登录操作日志
    /// </summary>
    /// <param name="EditLog">日志文本</param>
    /// <param name="LName">模块页面</param>
    /// <param name="Module">系统模块</param>
    /// <param name="LoginPage">模块页面</param>
    /// <param name="login">登录是否成功： 0： 不成功  1： 成功</param>
    /// <param name="LoginType">类型0：登录跟踪 1：表示操作跟踪</param>
    /// <param name="Remark"></param>
    /// <param name="Type">用户登录类型 0：平台总后台登录 1：代理商用户  2：公共用户  3：企业用户 4：企业管理员  5：代理商管理员 </param>
    public static void EditLog(string EditLog, string LName, string Remark, string Module, string LoginPage, int LoginType, int login, int type)
    {

        try
        {
            int LoginId = 0;   //登录人Id
            string LoginName = LName; //登录人名称
            string LoginIp = string.Empty;  //登录Ip
            DateTime LoginStartDate = DateTime.Now; //登录时间
            int LoginUserType = type;  //用户登录类型

            //代理商端ip
            //LoginIp = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
            //if (LoginIp == "::1" || LoginIp == "")
            //{
            //    //代理商端主机名ip
            //    LoginIp = HttpContext.Current.Request.ServerVariables.Get("Remote_Host").ToString();
            //}
            LoginIp = Utils.GetIP();
            
            if (type != 0)
            {
                LoginModel AdminUserModel = null;
                if (HttpContext.Current.Session["UserModel"] is LoginModel)
                {
                    AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel; //得到登录LoginId    

                    if (AdminUserModel != null)
                    {
                        LoginId = AdminUserModel.UserID;
                        //LoginName = AdminUserModel.TrueName == "" ? AdminUserModel.UserName : AdminUserModel.TrueName;
                        LoginName = AdminUserModel.UserName;
                    }
                    else
                    {
                        LoginName = LName;
                    }
                }
            }
            else
            {
                Hi.Model.SYS_AdminUser UModel= null;
                if (HttpContext.Current.Session["AdminUser"] != null)
                {
                    UModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser; //得到登录LoginId    

                    if (UModel != null)
                    {
                        LoginId = UModel.ID;
                        //LoginName = UModel.TrueName == "" ? UModel.LoginName : UModel.TrueName;
                        LoginName = UModel.LoginName;
                    }
                    else
                    {
                        LoginName = LName;
                    }
                }
            }

            string LoginNote = string.Empty;
            if (LoginType == 1)
            {
                LoginNote = LoginName + ":" + EditLog;
            }
            else
            {
                LoginNote = EditLog;
            }

            string sql = "insert into [A_LoginLog] (LoginStartDate,LoginNote,Module,LoginPage,LoginType,Remark,LoginId,LoginName,LoginIp,[LoginUserType],[Login]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";
            SqlHelper.ExecuteSql(Connection, string.Format(sql, LoginStartDate, LoginNote, Module, LoginPage, LoginType, Remark, LoginId, LoginName, LoginIp, LoginUserType, login));
        }
        catch { }

        #region
        //LSY.Model.A_AdminLog LogModel = new LSY.Model.A_AdminLog();
        //LogModel.LoginStartDate = DateTime.Now;
        //if (LoginType == 1)//操作日志
        //{
        //    LogModel.LoginNote = UModel.Name + ":" + EditLog;
        //}
        //else
        //{
        //    LogModel.LoginNote = EditLog;
        //}
        //LogModel.Module = Module;
        //LogModel.LoginPage = LoginPage;
        //LogModel.LoginType = LoginType;
        //LogModel.Remark = Remark;
        //if (UModel != null)
        //{
        //    LogModel.LoginId = UModel.id;
        //    LogModel.LoginName = UModel.LoginId;
        //    LogModel.LoginIp = UModel.LastLoginIP;
        //}
        //else
        //{
        //    LogModel.LoginIp = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
        //}
        //new LSY.BLL.A_AdminLog().Add(LogModel);
        #endregion
    }


    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="LogUID">登录帐号对应的ID</param>
    /// <param name="LogName">登录帐号</param>
    /// <param name="CompName">厂商名称（厂商、代理商）</param>
    /// <param name="UserType">用户登录类型身份（1厂商、2代理商、3平台工作人员、4游客）</param>
    /// <param name="LoginPage">被访问的URL</param>
    /// <param name="Module">模块（首页，平台介绍，帮助中心，新闻资讯，注册页，登录页，8月精选店铺，入市指南，企业店铺，商品，店铺搜索，商品搜索....）</param>
    /// <param name="Remark">备注（）</param>
    /// <param name="url"></param>
    public static void Operation(int LogUID, string LogName,string CompName, int UserType,string LoginPage, string Module, string Remark)
    {
        string LoginIp = string.Empty;  //登录Ip
        string LName = LogName;
        string CName = CompName;
        int LoginUserType = 0;
        DateTime LoginStartDate = DateTime.Now; //访问时间
        //判断登录用户名是否为空，为空表示游客访问
        if (LName=="")
        {
            LoginUserType = 4;  //游客
        }
        else 
        {
            LoginUserType = UserType;  //用户登录类型身份（1厂商、2代理商、3平台工作人员、4游客）
            if (LoginUserType == 1 || LoginUserType == 5)
            {
                LoginUserType = 2;   //代理商
            }
            else if (LoginUserType == 3 || LoginUserType == 4)
            {
                LoginUserType = 1;     //厂商
            }
            else if (LoginUserType == 0)
            {
                LoginUserType = 3;    //平台工作人员
            }
            else
            {
                LoginUserType = 4;    //游客
            }
        }
        if (LogUID==0&&LName=="")
        {
           LoginIp = Utils.GetIP();
           string sql = "INSERT INTO dbo.A_SystemLog(LogUIp,LogPage,UserType,Module,Remark,LogTime,CreateDate,dr) VALUES  ( '{0}','{1}',{2},'{3}','{4}','{5}','{6}',{7}) ";
           SqlHelper.ExecuteSql(Connection, string.Format(sql, LoginIp, LoginPage, LoginUserType, Module, Remark,LoginStartDate, LoginStartDate, 0));
        }
        else
        {
            LoginIp = Utils.GetIP();
            string sql = "INSERT INTO dbo.A_SystemLog(LogUID,LogUName,LogUIp,LogPage,UserType,CompName,ViewType,LogType,Module,Remark,LogTime,CreateDate,dr) VALUES  ( {0},'{1}','{2}','{3}' ,{4} ,'{5}' ,{6} ,{7} ,'{8}' ,'{9}' ,'{10}' ,'{11}',{12}) ";
            SqlHelper.ExecuteSql(Connection, string.Format(sql, LogUID, LName, LoginIp, LoginPage, LoginUserType, CName, 0, 0, Module, Remark, LoginStartDate, LoginStartDate, 0));
        }
    }

    /// <summary>
    /// 新增销售单据日志
    /// </summary>
    /// <param name="ReceiptID">单据ID</param>
    /// <param name="ReceiptNo">单据号</param>
    /// <param name="ReceiptType">单据类型 1：进货单 2：出库单 3：到货单</param>
    /// <param name="ReceiptState">单据操作状态</param>
    public static void AddOrderLog(int ReceiptID, string ReceiptNo, int ReceiptType, int ReceiptState)
    {
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel; //得到登录LoginId    
            }

            Hi.Model.DIS_OrderLog OrderLogModel = new Hi.Model.DIS_OrderLog();

            OrderLogModel.ReceiptID = ReceiptID;
            OrderLogModel.ReceiptNo = ReceiptNo;
            OrderLogModel.ReceiptType = ReceiptType;
            OrderLogModel.ReceiptState = ReceiptState;
            OrderLogModel.CreateUserID = AdminUserModel.UserID;
            OrderLogModel.CreateDate = DateTime.Now;
            OrderLogModel.ts = DateTime.Now;

            new Hi.BLL.DIS_OrderLog().Add(OrderLogModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// 新增库存日志记录
    /// </summary>
    public static void AddStoreStockLog()
    {

    }

    /// <summary>
    /// 新增业务日志
    /// </summary>
    /// <param name="LogClass">单据名称</param>
    /// <param name="ApplicationId">业务ID</param>
    /// <param name="LogType">业务名称 新增、审核、退回、提交...</param>
    /// <param name="LogRemark">备注</param>
    public static void AddSysBusinessLog(int CompId,string LogClass, string ApplicationId, string LogType, string LogRemark)
    {
        try
        {
            LoginModel AdminUserModel = null;
            if (HttpContext.Current.Session != null)
            {
                AdminUserModel = HttpContext.Current.Session["UserModel"] as LoginModel;//得到登录LoginId    
            }

            if (LogType.Equals("订单物流"))
            {
                string strwhere = "CompID=" + CompId + " and ApplicationId=" + ApplicationId + " and LogType='" + LogType + "'";
                List<Hi.Model.SYS_SysBusinessLog> l = new Hi.BLL.SYS_SysBusinessLog().GetList("", strwhere, "");

                if (l != null)
                {
                    if (l.Count > 0)
                    {
                        foreach (Hi.Model.SYS_SysBusinessLog item in l)
                        {
                            item.LogRemark = LogRemark;
                            item.LogTime = DateTime.Now;
                            item.OperatePersonId = AdminUserModel.UserID;
                            item.OperatePerson = AdminUserModel.TrueName == "" ? AdminUserModel.UserName : AdminUserModel.TrueName;
                            item.modifyuser = AdminUserModel.UserID;
                            new Hi.BLL.SYS_SysBusinessLog().Update(item);
                        }
                    }
                    else
                    {
                        Hi.Model.SYS_SysBusinessLog SysBusinessLogModel = new Hi.Model.SYS_SysBusinessLog();
                        SysBusinessLogModel.CompID = CompId;
                        SysBusinessLogModel.LogClass = LogClass;
                        SysBusinessLogModel.ApplicationId = Convert.ToInt32(ApplicationId);
                        SysBusinessLogModel.LogType = LogType;
                        SysBusinessLogModel.OperatePersonId = AdminUserModel.UserID;
                        SysBusinessLogModel.OperatePerson = AdminUserModel.TrueName == "" ? AdminUserModel.UserName : AdminUserModel.TrueName;
                        SysBusinessLogModel.LogRemark = LogRemark;
                        SysBusinessLogModel.LogTime = DateTime.Now;
                        SysBusinessLogModel.ts = DateTime.Now;

                        new Hi.BLL.SYS_SysBusinessLog().Add(SysBusinessLogModel);
                    }
                }
            }
            else
            {

                Hi.Model.SYS_SysBusinessLog SysBusinessLogModel = new Hi.Model.SYS_SysBusinessLog();
                SysBusinessLogModel.CompID = CompId;
                SysBusinessLogModel.LogClass = LogClass;
                SysBusinessLogModel.ApplicationId = Convert.ToInt32(ApplicationId);
                SysBusinessLogModel.LogType = LogType;
                SysBusinessLogModel.OperatePersonId = AdminUserModel.UserID;
                SysBusinessLogModel.OperatePerson = AdminUserModel.TrueName == "" ? AdminUserModel.UserName : AdminUserModel.TrueName;
                SysBusinessLogModel.LogRemark = LogRemark;
                SysBusinessLogModel.LogTime = DateTime.Now;
                SysBusinessLogModel.ts = DateTime.Now;

                new Hi.BLL.SYS_SysBusinessLog().Add(SysBusinessLogModel);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// 新增业务日志
    /// </summary>
    /// <param name="LogClass">单据名称</param>
    /// <param name="ApplicationId">业务ID</param>
    /// <param name="LogType">业务名称 新增、审核、退回、提交...</param>
    /// <param name="LogRemark">备注</param>
    /// <param name="UserID">操作人Id</param>
    public static void AddSysBusinessLog(int CompId, string LogClass, string ApplicationId, string LogType, string LogRemark, string UserID)
    {
        try
        {

            Hi.Model.SYS_Users userModel = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(UserID));
            if (userModel != null)
            {

                Hi.Model.SYS_SysBusinessLog SysBusinessLogModel = new Hi.Model.SYS_SysBusinessLog();
                SysBusinessLogModel.CompID = CompId;
                SysBusinessLogModel.LogClass = LogClass;
                SysBusinessLogModel.ApplicationId = Convert.ToInt32(ApplicationId);
                SysBusinessLogModel.LogType = LogType;
                SysBusinessLogModel.OperatePersonId = Convert.ToInt32(UserID);
                SysBusinessLogModel.OperatePerson = userModel.TrueName == "" ? userModel.UserName : userModel.TrueName;
                SysBusinessLogModel.LogRemark = LogRemark;
                SysBusinessLogModel.LogTime = DateTime.Now;
                SysBusinessLogModel.ts = DateTime.Now;

                new Hi.BLL.SYS_SysBusinessLog().Add(SysBusinessLogModel);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }



    /// <summary>
    /// 操作日志调用的方法
    /// </summary>
    /// <param name="Request"></param>
    /// <param name="Title"></param>
    public static void WritePageLog(HttpRequest Request,string Title)
    {
        string LoginPage =Request.Url.LocalPath;
        LoginModel user = HttpContext.Current.Session["UserModel"] as LoginModel; 
        if (user != null)
        {
            int LogUID = user.UserID;      //登录的id
            string LName = user.UserName;  //登录用户名
            int Type = user.Ctype;       //区分是代理商还是厂商
            int UserType = user.TypeID;  //用户登录类型 0：平台总后台登录 1：代理商用户  2：公共用户  3：企业用户 4：企业管理员  5：代理商管理员
            LoginPage = Request.Url.LocalPath;       //访问的页面url
            string CompName = string.Empty;
            if (Type == 1)
            {
                CompName = user.CompName;     //厂商名称
            }
            else if (Type == 2)
            {
                CompName = user.DisName;     //代理商名称
            }

            Utils.Operation(LogUID, LName, CompName, UserType, LoginPage, Title, "用户" + LName + "访问" + LoginPage + "");
        }
        else
        {
            string url = Request.Url.LocalPath;
            Utils.Operation(0,"", "", 0, LoginPage, Title, "游客访问" + LoginPage + "");
        }
    }

}