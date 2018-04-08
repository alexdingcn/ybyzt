<%@ Application Language="C#" %>
<%@ import Namespace="System.Threading" %>  
<%@ import Namespace="System.Net" %>  
<%@ import Namespace="System.IO" %>  
<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        try
        {
            //在应用程序启动时运行的代码
            string configFileName = Server.MapPath("~\\log.log4net");
            System.IO.FileInfo f = new System.IO.FileInfo(configFileName);
            Tiannuo.LogHelper.LogHelper.SetConfig(f, System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            Tiannuo.LogHelper.LogHelper.Info("系统开始运行");
        }
        catch (Exception exp)
        {
            Tiannuo.LogHelper.LogHelper.Error("Application Start 异常 系统不能正常运行", exp);
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码
        //Utils.AddSysBusinessLog(00, "Test", "123", "Test定时执行", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application End!");

        //下面的代码是关键，可解决IIS应用程序池自动回收的问题  

        //Thread.Sleep(1000);

        //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  

        Tiannuo.LogHelper.LogHelper.Info("applicatin_end事件触发" + this.ToString());
        Tiannuo.LogHelper.LogHelper.Info("系统结束运行");
    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码
        Exception ex = Server.GetLastError().GetBaseException();
        Tiannuo.LogHelper.LogHelper.Error("Error", ex);
    }


    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码


    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        //SQL注入式攻击代码验证
        Util util = new Util();
        util.StartProcessRequest();
    }

    void Application_EndRequest(Object sender, EventArgs e)
    {
        //if (Context.Response.StatusCode == 404 || Context.Response.StatusCode == 403 || Context.Response.StatusCode == 500)
        //{
        //    System.Web.HttpContext.Current.Response.Redirect("/error.html", true);
        //}
    }


    private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    {

        //间隔时间执行某动作  

    }

</script>
