<%@ WebHandler Language="C#" Class="BehindNoticePage" %>

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;
using System.Data;
using CFCA.Payment.Api;
using System.Web.Configuration;
using BehindUp;

public class BehindNoticePage : IHttpHandler
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public void ProcessRequest(HttpContext context)
    {
        Console.WriteLine("---------- Begin [ReceiveNotice] process......");

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        //1 获得参数message和signature
        String message = request.Form["message"];
        String signature = request.Form["signature"];


        Console.WriteLine("[message]=[" + message + "]");
        Console.WriteLine("[signature]=[" + signature + "]");



        LogManager.LogFielPrefix = "123";
        LogManager.LogPath = "D:/测试/";

        LogManager.WriteLog(LogFile.Trace.ToString(), DateTime.Now.ToString() + "-进入后台启动>>>>>>>>>>>...\r\n");

      
       
           
            //广州农商行流水处理
            
                LogManager.WriteLog(LogFile.Trace.ToString(), "进入广州农商行回调程序\r\n");

                //调用后台通知接口
               BehindService behindservice = new BehindService();

               string str = behindservice.BehindUpdate("MYKJ2016111019040633511562", 20, Convert.ToDecimal(0.50));
                
                LogManager.WriteLog(LogFile.Trace.ToString(),"执行结果："+str+"\r\n");                
            
         

      

        //4 响应支付平台
        

        response.Clear();
        response.Write(str);
        //response.Flush();
        response.End();
        response.Close();

        Console.WriteLine("---------- End [ReceiveNotice] process.");

    }





    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}