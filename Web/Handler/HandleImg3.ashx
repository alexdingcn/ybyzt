<%@ WebHandler Language="C#" Class="HandleImg3" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Configuration;


public class HandleImg3 : loginInfoMation, IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        context.Response.ContentType = "text/html";
        if (context.Request.HttpMethod == "POST")
        {
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    string FlileID = context.Request.QueryString["FlileID"];
                    if (string.IsNullOrEmpty(FlileID))
                    {
                        context.Response.Write("0");
                        return;
                    }
                    System.IO.Stream stream = context.Request.Files[FlileID].InputStream;
                    byte[] b = new byte[context.Request.Files[FlileID].ContentLength];
                    new loginInfoMation().LoadData();
                    string Json = "";
                    if (UserModel == null && !(context.Session["AdminUser"]  is Hi.Model.SYS_AdminUser))
                    {
                        Json = "{ \"msg\":\"用户未登陆无法上传图片\",\"result\":false }";
                        context.Response.Write(Json);
                        return;
                    }
                    stream.Read(b, 0, b.Length);
                    string filename = context.Request.Files[FlileID].FileName;
                    string ext = Path.GetExtension(filename);

                    string path = ConfigurationManager.AppSettings["ImgPath"] + "CompFiveImg" + "/";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    //文件名带后缀，根据时间产生唯一名称
                    string Compid = "";
                    if (CompID > 0)
                    {
                        Compid = CompID.ToString();
                    }
                    else
                    {
                        Compid = context.Request["Compid"];
                    }
                    string name = Guid.NewGuid().ToString() + "_" + Compid + ext;
                    string avatar_path = HandleHeadImg(path, name, b);
                    Json = " \"msg\":\"{0}\",result:true ";
                    context.Response.Write("{"+string.Format(Json, avatar_path)+"}");

                }
                catch
                {
                    context.Response.Write("0");
                }
            }
            else
            {
                context.Response.Write("0");
                return;
            }
        }

    }

    private string HandleHeadImg(string file_path, string filename, byte[] b)
    {
        string original = filename;
        if (FileLib.WriteFile(file_path, original, b))
        {
            string avatar_path = filename;
            return avatar_path;
        }
        return "";
    }

    public static void Create_Small_Jpg(string Original_ImageFilePathandName, string Small_ImageFilePathandName, int small_width, int small_height)
    {
        System.Drawing.Image image1 = System.Drawing.Image.FromFile(Original_ImageFilePathandName);  //@"d:\\irene.jpg"

        if ((image1.Width > small_width) && (image1.Height > small_height))
        {

            //small_width = 80;
            small_height = (small_width * image1.Height / image1.Width);
            //if (image1.Width < small_width)
            //{
            //    small_width = (image1.Width*small_width);
            //}
            //if (image1.Height < small_height)
            //{
            //    small_height = image1.Height;
            //}

            System.Drawing.Bitmap bmp1 = ResizeImage(image1, small_width, small_height);

            bmp1.Save(Small_ImageFilePathandName, System.Drawing.Imaging.ImageFormat.Jpeg);  //@"d:\test.png"

            bmp1.Dispose();

            image1.Dispose();

        }
        else
        {
            image1.Save(Small_ImageFilePathandName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        image1.Dispose();
    }
    public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
    {
        //a holder for the result             
        Bitmap result = new Bitmap(width, height);

        //use a graphics object to draw the resized image into the bitmap            
        using (Graphics graphics = Graphics.FromImage(result))
        {
            //set the resize quality modes to high quality                 \
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //draw the image into the target bitmap                 
            graphics.DrawImage(image, 0, 0, result.Width, result.Height);
        }
        return result;
    }

    public static class FileLib
    {
        #region 将文件保存到相应文件目录

        /// <summary>
        /// WeiPanPath
        /// </summary>
        //public static string WeiPanPath
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["docbank_path"]; ;
        //    }
        //}

        /// <summary>
        /// 创建文档文件夹
        /// </summary>
        public static void CreateDocDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 写入上传文件
        /// </summary>
        public static bool WriteFile(string directory, string filename, byte[] b)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                //CreateDocDirectory(HttpContext.Current.Server.MapPath(Directory + "/" + childDirectory));

                if (!File.Exists(directory + "/" + filename))
                {
                    Stream s = File.Create(directory + "/" + filename);
                    s.Close();
                }
                FileStream outStream = File.OpenWrite(directory + "/" + filename);
                for (int i = 0; i < b.Length; i++)
                {
                    outStream.WriteByte(b[i]);
                }
                outStream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 修改上传文件
        /// </summary>
        public static bool ModifyFile(string parentPath, string childDirectory, string filename, string type, byte[] b)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(parentPath + "/" + childDirectory + "/"));

                FileInfo[] f_info = info.GetFiles();

                foreach (FileInfo i in f_info)
                {
                    if (i.Name.Substring(0, i.Name.IndexOf('.')).ToUpper() == filename.ToUpper())
                    {
                        i.Delete();
                    }
                }
                if (!File.Exists(HttpContext.Current.Server.MapPath(parentPath + "/" + childDirectory + "/" + filename + type)))
                {
                    Stream s = File.Create(HttpContext.Current.Server.MapPath(parentPath + "/" + childDirectory + "/" + filename + type));
                    s.Close();
                }
                FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(parentPath + "/" + childDirectory + "/" + filename + type));
                for (int i = 0; i < b.Length; i++)
                {
                    outStream.WriteByte(b[i]);
                }
                outStream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除上传文件
        /// </summary>
        public static bool DeleteFile(string parentPath, string childDirectory, string filename)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(parentPath + "/" + childDirectory + "/"));

                FileInfo[] f_info = info.GetFiles();

                foreach (FileInfo i in f_info)
                {
                    string name = i.Name.Substring(0, i.Name.IndexOf('.'));
                    if (name.ToUpper() == filename.ToUpper())
                    {
                        i.Delete();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        #endregion

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}