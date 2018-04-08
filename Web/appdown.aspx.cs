using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;

public partial class AppDown : System.Web.UI.Page
{

    public bool Is_Weixin = false;
    public bool IsPhoneMobileBrower = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        SetPageNoCache();
        if (!IsPostBack)
        {
            DownFile();
        }
    }

    /// <summary>
    /// 设置页面不被缓存
    /// </summary>
    private void SetPageNoCache()
    {
        Response.Buffer = true;
        Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Response.AppendHeader("Pragma", "No-Cache");
    }

    public void DownFile()
    {
        string ver = Ver();
        if (string.IsNullOrWhiteSpace(ver)) return;
        string UserAgent = Request.Headers["User-Agent"];
        if (UserAgent.Contains("Windows"))
        {
            string filePath = Server.MapPath("APP/") + "yzt_" + ver + ".apk";
            if (File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                //解决中文文件名乱码    
                Response.AddHeader("Content-length", file.Length.ToString());
                Response.ContentType = "appliction/octet-stream";
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            if (UserAgent.Contains("Mobile"))
            {
                weixin_tip.Visible = true;
                if (UserAgent.Contains("MicroMessenger"))
                {
                    if (!UserAgent.Contains("Android"))
                    {
                        ImgMobile.Src = "/images/Phone_Weixin.png";
                    }
                    else
                    {
                        ImgMobile.Src = "/images/Android_Weixin.png";
                    }
                    Is_Weixin = true;
                }
                else
                {
                    if (UserAgent.Contains("Android"))
                    {
                        //string filePath = Server.MapPath("APP/") + "yzt_" + ver + ".apk";
                        //if (File.Exists(filePath))
                        //{
                        //    FileInfo file = new FileInfo(filePath);
                        //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
                        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                        //    //解决中文文件名乱码    
                        //    Response.AddHeader("Content-length", file.Length.ToString());
                        //    Response.ContentType = "appliction/octet-stream";
                        //    Response.WriteFile(file.FullName);
                        //    Response.Flush();
                        //    Response.End();
                        //}
                        Is_Weixin = true;
                    }
                    else if (UserAgent.Contains("iPhone"))
                    {
                        IsPhoneMobileBrower = true;
                    }
                    else
                    {
                        string filePath = Server.MapPath("APP/") + "yzt_" + ver + ".apk";
                        if (File.Exists(filePath))
                        {
                            FileInfo file = new FileInfo(filePath);
                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                            //解决中文文件名乱码    
                            Response.AddHeader("Content-length", file.Length.ToString());
                            Response.ContentType = "appliction/octet-stream";
                            Response.WriteFile(file.FullName);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            else
            {
                //string filePath = Server.MapPath("APP/") + "yzt_" + ver + ".apk";
                //if (File.Exists(filePath))
                //{
                //    FileInfo file = new FileInfo(filePath);
                //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
                //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
                //    //解决中文文件名乱码    
                //    Response.AddHeader("Content-length", file.Length.ToString());
                //    Response.ContentType = "appliction/octet-stream";
                //    Response.WriteFile(file.FullName);
                //    Response.Flush();
                //    Response.End();
                //}
            }
        }

    }

    public string Ver()
    {
        string version = string.Empty;
        string pathxml = HttpRuntime.AppDomainAppPath.ToString() + "APP\\VerControl.xml";
        if (File.Exists(pathxml))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathxml);
            
            XmlNode root = doc.SelectSingleNode("Version");
            if (root != null)
            {
                if (root.SelectSingleNode("CompControl") != null)
                    version = (root.SelectSingleNode("CompControl")).InnerText;
            }
        }
        return version;
    }
}