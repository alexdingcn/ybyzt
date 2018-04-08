using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using System.Configuration;
using System.IO;

/// <summary>
///UpLoadPic 的摘要说明
/// </summary>
public class UpLoadPic
{
	public UpLoadPic()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public Dis_Order_Version3.ResultEdit UpGoodsPic(String JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string StringImage = string.Empty;
        string FileName = string.Empty;
        string GoodID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" ||
                JInfo["StringImage"].ToString().Trim() == "" || JInfo["FileName"].ToString().Trim() == "" || JInfo["GoodID"].ToString().Trim() == "")
            {
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "参数异常" };
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            StringImage = JInfo["StringImage"].ToString();
            FileName = Common.NoHTML(JInfo["FileName"].ToString());
            GoodID = JInfo["GoodID"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            #endregion

            string ext = FileName.Substring(FileName.LastIndexOf("."));
            if (!(ext.ToUpper() == ".JPG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".JPEG"))
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "图片格式不正确" };
            string ImgFolder = "GoodsImg/";
            string name = Guid.NewGuid().ToString() + ext;
            string path = GetWebConfigKey("ImgPath") + ImgFolder;
            string viewPath = GetWebConfigKey("ImgViewPath") + ImgFolder;
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
            DirectoryInfo di2 = new DirectoryInfo(GetWebConfigKey("ImgPath") + "PicSpace/" + CompID);
            if (!di2.Exists)
            {
                di2.Create();
            }
            string toFileName = name;
            string saveFile = path + "D" + toFileName;
            byte[] b = Convert.FromBase64String(StringImage);
            FileStream fs = new FileStream(saveFile, FileMode.Create, FileAccess.Write);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();

            ////单独保存图片
            string ThumbPath = GetWebConfigKey("ImgPath") + "PicSpace/" + CompID + "/" + FileName;
            //HttpFile.SaveAs(ThumbPath);
            //大缩略图
            string bigThumbPath = path + "X" + toFileName;
            MakeThumbnail(saveFile, bigThumbPath, 400, 400, "Cut");
            //小缩略图
            string smallThumbPath = path + toFileName;
            MakeThumbnail(saveFile, smallThumbPath, 200, 200, "Cut");

            FileStream fs1 = new FileStream(ThumbPath, FileMode.Create, FileAccess.Write);
            fs1.Write(b, 0, b.Length);
            fs1.Flush();
            fs1.Close();
            //新增一条BD_ImageList数据
            Hi.Model.BD_ImageList modelImg = new Hi.Model.BD_ImageList();
            modelImg.CompID = comp.ID;
            modelImg.GoodsID = Int32.Parse(GoodID);
            modelImg.Pic = toFileName;
            modelImg.Pic2 = "X" + toFileName;
            modelImg.Pic3 = "D" + toFileName;
            modelImg.IsIndex = 0;
            modelImg.CreateDate = DateTime.Now;
            modelImg.CreateUserID = Int32.Parse(UserID);
            modelImg.ts = DateTime.Now;
            modelImg.modifyuser = Int32.Parse(UserID);
            if(new Hi.BLL.BD_ImageList().Add(modelImg)<=0)
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "图片上传失败" };
            
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "UpGoodsPic:" + JSon);
            return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "图片上传失败" };
        }

        return new Dis_Order_Version3.ResultEdit() { Result = "T", Description = "图片上传成功" };
        
    }

    public Dis_Order_Version3.ResultEdit CompFileUp(String JSon)
    {
        string CompID = string.Empty;
        string FileName = string.Empty;
        string StringImage = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["CompID"].ToString().Trim() == "" || JInfo["StringImage"].ToString().Trim() == "" || JInfo["FileName"].ToString().Trim() == "")
            {
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "参数异常" };
            }
            CompID = JInfo["CompID"].ToString();
            StringImage = JInfo["StringImage"].ToString();
            FileName = Common.NoHTML(JInfo["FileName"].ToString()).Replace(",", "").Replace("_", "").Replace(" ", ""); ;
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0)
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            string path = "UploadFile/";
            int k = FileName.LastIndexOf(".");
            int j = FileName.LastIndexOf("\\");
            string type = FileName.Substring(k + 1);
            FileName = FileName.Substring(j + 1);
            string ext = "." + type.ToLower();
            if (!(ext.ToUpper() == ".JPG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".JPEG"))
                return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "附件格式不正确" };
            DateTime datetime1 = System.DateTime.Now;

            FileName = FileName.Replace("." + type, "") + "_" + datetime1.ToString("yyyyMMddHHmmssffff") + "." + type;
            //string saveFile = System.Web.HttpContext.Current.Server.MapPath("../" + path + "" + FileName);
            string saveFile = GetWebConfigKey("UpFile") + FileName;

            byte[] b = Convert.FromBase64String(StringImage);
            FileStream fs = new FileStream(saveFile, FileMode.Create, FileAccess.Write);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();

            if (comp.Attachment == null || comp.Attachment == "")
                comp.Attachment = FileName;
            else
                comp.Attachment += "," + FileName;

           if(!new Hi.BLL.BD_Company().Update(comp))
               return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "附件上传失败" };

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompFileUp:" + JSon);
            return new Dis_Order_Version3.ResultEdit() { Result = "F", Description = "附件上传失败" };
        }
        return new Dis_Order_Version3.ResultEdit() { Result = "T", Description = "附件上传成功" };

    }


    public static string GetWebConfigKey(string Name)
    {
        string keyvalue = "";
        if (Name != "")
        {
            keyvalue = ConfigurationManager.AppSettings["" + Name + ""].ToString().Trim();
        }
        return keyvalue;
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