<%@ WebHandler Language="C#" Class="HandleImg" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
public class HandleImg : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {

        context.Response.Clear();
        context.Response.ContentType = "text/html";
        string PageAction = context.Request["action"];
        string ReturnMsg = "";
        switch (PageAction)
        {
            case "delfile": ReturnMsg = DelFile(context); break;
            default: ReturnMsg = HandleFile(context); break;
        }
        context.Response.Write(ReturnMsg);
        context.Response.End();
    }
    public string DelFile(HttpContext context)
    {
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        if (logUser != null)
        {
            try
            {
                string filepath = context.Request["files"];
                string orderid = context.Request["orderid"];
                string path = Common.GetWebConfigKey("ImgPath");

                //  string url = context.Request.UrlReferrer.LocalPath;
                // var str = url.Substring(url.LastIndexOf("/") + 1);
                // if (str == "orderRefer.aspx")
                // {
                if (!Util.IsEmpty(orderid))
                {
                    bool bol = Update(orderid, filepath, "del", logUser);
                    if (bol)
                    {
                        if (!string.IsNullOrEmpty(filepath))
                        {
                            string path2 = path + "OrderFJ/" + filepath;
                            if (File.Exists(path2))
                            {
                                File.Delete(path2);
                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filepath))
                    {
                        string path2 = path + "OrderFJ/" + filepath;
                        if (File.Exists(path2))
                        {
                            File.Delete(path2);
                        }
                    }
                }
                return "cg";
            }
            catch (Exception)
            {

                return "";
            }
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 修改附件
    /// </summary>
    /// <param name="orderid"></param>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public bool Update(string orderid, string filepath, string type, LoginModel logUser)
    {
        bool bol = false;
        try
        {
            Hi.Model.DIS_Order model = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));
            if (model != null)
            {
                string fj = model.Atta;//附件
                string[] fjlist = fj.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                string strlist = string.Empty;
                if (type == "del")
                {
                    for (int i = 0; i < fjlist.Length; i++)
                    {
                        if (filepath != fjlist[i])
                        {
                            strlist += fjlist[i] + "@@";
                        }
                    }
                }
                else
                {
                    strlist = fj + filepath + "@@";
                }
                model.modifyuser = logUser.UserID;
                //model.ts = DateTime.Now;
                model.Atta = strlist;
                bol = new Hi.BLL.DIS_Order().Update(model);
            }
        }
        catch (Exception)
        {
            bol = false;
        }
        return bol;
    }
    public string HandleFile(HttpContext context)
    {
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        if (logUser != null)
        {
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    string orderid = context.Request["orderid"];

                    System.IO.Stream stream = context.Request.Files["AddBanner"].InputStream;
                    byte[] b = new byte[context.Request.Files["AddBanner"].ContentLength];
                    if (context.Request.Files["AddBanner"].ContentLength > 20480 * 1024)
                    {
                        return "1";
                    }
                    stream.Read(b, 0, b.Length);
                    string filename = context.Request.Files["AddBanner"].FileName;
                    if (filename.IndexOf("\\") != -1)
                    {
                        filename = filename.Substring(filename.LastIndexOf("\\") + 1);
                    }
                    string ext = filename.Substring(filename.LastIndexOf("."));

                    if (!(ext.ToUpper() == ".PDF" || ext.ToUpper() == ".DOC" || ext.ToUpper() == ".XLS" || ext.ToUpper() == ".DOCX" || ext.ToUpper() == ".XLSX" || ext.ToUpper() == ".TXT" || ext.ToUpper() == ".JPG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".BMP" || ext.ToUpper() == ".GIF" || ext.ToUpper() == ".RAR" || ext.ToUpper() == ".ZIP"))
                    {
                        return "2";
                    }
                    
                    var ImgFolder = "OrderFJ/";
                    var name = Guid.NewGuid().ToString() + ext;
                    var path = Common.GetWebConfigKey("ImgPath") + ImgFolder;
                    var viewPath = Common.GetWebConfigKey("ImgViewPath") + ImgFolder;
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    var toFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + ext;
                    string saveFile = path + filename.Substring(0, filename.LastIndexOf('.')) + "^^" + toFileName;
                    var HttpFile = context.Request.Files["AddBanner"];
                    HttpFile.SaveAs(saveFile);
                    if (!Util.IsEmpty(orderid))
                    {
                        bool bol = Update(orderid, filename.Substring(0, filename.LastIndexOf('.')) + "^^" + toFileName, "edit", logUser);
                    }
                    return filename.Substring(0, filename.LastIndexOf('.')) + "^^" + toFileName + "@#$" + context.Request.Files["AddBanner"].ContentLength;
                }
                catch (Exception)
                {
                    return "0";
                }


            }
            else
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    private string HandleHeadImg(string file_path, string filename, byte[] b)
    {
        string original = "original_" + filename;
        string Original_ImageFilePathandName = HttpContext.Current.Server.MapPath(file_path) + "/" + original;
        string Small_ImageFilePathandName = HttpContext.Current.Server.MapPath(file_path) + "/" + filename;

        if (FileLib.WriteFile(file_path, original, b))
        {
            Create_Small_Jpg(Original_ImageFilePathandName, Small_ImageFilePathandName, 10000000, 1000000);
        }


        string avatar_path = file_path + filename;
        return avatar_path;
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
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));

                //CreateDocDirectory(HttpContext.Current.Server.MapPath(Directory + "/" + childDirectory));

                if (!File.Exists(HttpContext.Current.Server.MapPath(directory + "/" + filename)))
                {
                    Stream s = File.Create(HttpContext.Current.Server.MapPath(directory + "/" + filename));
                    s.Close();
                }
                FileStream outStream = File.OpenWrite(HttpContext.Current.Server.MapPath(directory + "/" + filename));
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

    /// <summary> 
    /// 生成缩略图 
    /// </summary> 
    /// <param   name= "originalImagePath ">源图路径（物理路径） </param> 
    /// <param   name= "thumbnailPath "> 缩略图路径（物理路径） </param> 
    /// <param   name= "width "> 缩略图宽度 </param> 
    /// <param   name= "height "> 缩略图高度 </param> 
    /// <param   name= "mode "> 生成缩略图的方式 </param>         
    public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

        int towidth = width;
        int toheight = height;

        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;

        switch (mode)
        {
            case "HW "://指定高宽缩放（可能变形）                                 
                break;
            case "W "://指定宽，高按比例                                         
                toheight = originalImage.Height * width / originalImage.Width;
                break;
            case "H "://指定高，宽按比例 
                towidth = originalImage.Width * height / originalImage.Height;
                break;
            case "Cut "://指定高宽裁减（不变形）                                 
                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * height / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }
                break;
            default:
                break;
        }

        //新建一个bmp图片 
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //新建一个画板 
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //设置高质量插值法 
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //设置高质量,低速度呈现平滑程度 
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //清空画布并以透明背景色填充 
        g.Clear(System.Drawing.Color.Transparent);

        //在指定位置并且按指定大小绘制原图片的指定部分 
        g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

        try
        {
            //以jpg格式保存缩略图 
            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
        }
    }
}