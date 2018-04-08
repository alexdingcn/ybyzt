//using System.Linq;
using System;
using System.IO;
using System.Text;

/// <summary>
/// LogHelper 的摘要说明
/// </summary>
public class LogHelper
{
    public bool Result = false;
    private string content = "";
    private string logFile = "";

    /// <summary>
    /// 不带参数的构造函数
    /// </summary>
    public LogHelper()
    {
    }

    /// <summary>
    /// 带参数的构造函数
    /// </summary>
    /// <param name="logFile"></param>
    public LogHelper(string logFile ,string text)
    {
        content = text;

        this.logFile = logFile;
        if (!File.Exists(logFile))
        {
            FileStream fs = File.Create(logFile);
            fs.Close();
        }
    }

    /// <summary>
    /// 追加一条信息
    /// </summary>
    /// <param name="text"></param>
    public void Write()
    {
        using (StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8))
        {
            sw.Write("\r\n=========================" + DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") +
                     "记录开始==============================\r\n");
            sw.Write(content);
            sw.Write("\r\n=================================记录结束======================================\r\n");
            Result = true;
        }
    }
}
