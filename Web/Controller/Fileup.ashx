<%@ WebHandler Language="C#" Class="Fileup" Debug="true"%>

using System;
using System.Web;
using System.IO;
using Aliyun.OSS;

public class Fileup : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        string str1 = "";
        string str2 = "";
        string rvalue = "";
        try
        {
            HttpFileCollection fileList = context.Request.Files;
            string UpSrc = context.Request["UploadFiles"];
            if (string.IsNullOrEmpty(UpSrc))
            {
                UpSrc = "";
            }
            else if (!UpSrc.EndsWith("/"))
            {
                UpSrc += "/";
            }
            string MaxLenth = context.Request["maxLenth"];
            for (int i = 0; i < fileList.Count; i++)
            {
                HttpPostedFile hPostedFile = fileList[i];
                string filename = "";
                string filepath = "";
                if (hPostedFile.ContentLength > 0 && hPostedFile.FileName.Length > 0)
                {
                    if (!string.IsNullOrEmpty(MaxLenth))
                    {
                        if (hPostedFile.ContentLength > MaxLenth.ToInt(0))
                        {
                            context.Response.Expires = -1;
                            context.Response.Clear();
                            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                            context.Response.ContentType = "text/plain";
                            context.Response.Write("@returnstart@" + "{\"result\":false,\"Code\":\"上传的附件超过指定大小\"}" + "@returnend@");
                            context.Response.End();
                            return;
                        }
                    }
                    //float zldx = hPostedFile.ContentLength / 1024;
                    filename = hPostedFile.FileName.Replace("、", "").Replace(",", "").Replace("_", "").Replace(" ", "");

                    int k = filename.LastIndexOf(".");
                    int j = filename.LastIndexOf("\\");
                    string type = filename.Substring(k + 1);
                    filename = filename.Substring(j + 1);
                    DateTime datetime1 = System.DateTime.Now;
                    string ext = "." + type.ToLower();
                    if (!(ext.ToUpper() == ".PDF" || ext.ToUpper() == ".DOC" || ext.ToUpper() == ".XLS" || ext.ToUpper() == ".DOCX" || ext.ToUpper() == ".XLSX" || ext.ToUpper() == ".TXT" || ext.ToUpper() == ".JPG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".BMP" || ext.ToUpper() == ".GIF" || ext.ToUpper() == ".RAR" || ext.ToUpper() == ".ZIP"))
                    {
                        context.Response.Expires = -1;
                        context.Response.Clear();
                        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("@returnstart@" + "{\"result\":false,\"Code\":\"附件格式错误\"}" + "@returnend@");
                        context.Response.End();
                        return;
                    }
                    filepath = filename.Replace("." + type, "") + "_" + datetime1.ToString("yyyyMMddHHmmssffff") + "." + type;

                    //hPostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("../" + UpSrc + "" + filepath));
                    string accessKeyId = Common.GetWebConfigKey("OssAccessKeyId");
                    string accessKeySecret = Common.GetWebConfigKey("OssAccessKeySecret");
                    string bucketName = Common.GetWebConfigKey("OssBucketName");

                    var client = new OssClient("oss-cn-shanghai.aliyuncs.com", accessKeyId, accessKeySecret);
                    if (client != null)
                    {
                        client.PutObject(bucketName, UpSrc + filepath, hPostedFile.InputStream);
                    }

                    if (str1 == "")
                        str1 = filepath;
                    else
                        str1 += "," + filepath;
                    if (str2 == "")
                    {
                        str2 += filename;
                    }
                    else
                        str2 += "," + filename;
                }
            }
            if (str1.Length > 0)
            {
                //context.Response.Write(str1 + "&" + str2);
                rvalue = "{\"result\":true,\"name\":\"" + str1 + "\"}";
            }
            else
            {
                rvalue = "{\"result\":false}";
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            //捕捉线程终止异常   不处理
        }
        catch (Exception ex)
        {
            rvalue = "{\"result\":false}";
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
        }
        finally
        {
            ;
        }

        context.Response.Expires = -1;
        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        context.Response.ContentType = "text/plain";
        context.Response.Write("@returnstart@" + rvalue + "@returnend@");
        context.Response.End();

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}