using System;
using System.Data;
using System.Configuration;
using System.Web;


/// <summary>
/// LogManager 的摘要说明
/// </summary>
public class LogManager
{
    private static string logPath = string.Empty;
    private static string logPath2 = string.Empty;
    /// <summary>
    /// 保存日志的文件夹
    /// </summary>
    public static string LogPath
    {
        get
        {
            if (logPath == string.Empty)
            {
                logPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            return logPath;
        }
        set { logPath = value; }
    }

    /// <summary>
    /// 保存日志的文件夹
    /// </summary>
    public static string LogPath2
    {
        get
        {
            if (logPath2 == string.Empty)
            {
                logPath2 = AppDomain.CurrentDomain.BaseDirectory;
            }
            return logPath2;
        }
        set { logPath2 = value; }
    }

    private static string logFielPrefix = string.Empty;
    /// <summary>
    /// 日志文件前缀
    /// </summary>
    public static string LogFielPrefix
    {
        get { return logFielPrefix; }
        set { logFielPrefix = value; }
    }

    /// <summary>
    /// 日志文件前缀
    /// </summary>
    public static string LogFielPrefix2
    {
        get { return logFielPrefix2; }
        set { logFielPrefix2 = value; }
    }
    private static string logFielPrefix2 = string.Empty;
    /// <summary>
    /// 写日志
    /// </summary>
    public static void WriteLog(string logFile, string msg)
    {
        try
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                LogPath + LogFielPrefix + logFile + " " +
                DateTime.Now.ToString("yyyyMMdd") + ".Log"
                );
            sw.WriteLine(msg);
            sw.Close();
        }
        catch
        { }
    }

    /// <summary>
    /// 写日志2
    /// </summary>
    public static void WriteLog2(string logFile, string msg)
    {
        try
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                LogPath2 + LogFielPrefix2 + logFile + " " +
                DateTime.Now.ToString("yyyyMMdd") + ".Log"
                );
            sw.WriteLine(msg);
            sw.Close();
        }
        catch
        { }
    }


}

/// <summary>
/// 日志类型
/// </summary>
public enum LogFiles
{
    Trace,
    Warning,
    Error,
    SQL
}
