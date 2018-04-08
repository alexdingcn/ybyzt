<%@ WebHandler Language="C#" Class="UploadHandler2" %>

using System;
using System.Web;
using System.IO;
public class UploadHandler2 : System.Web.SessionState.IRequiresSessionState, IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        if (context.Request["REQUEST_METHOD"] == "OPTIONS")
        {
            context.Response.End();
        }
        SaveFile(context);
    }
    private void SaveFile(HttpContext context)
    {

        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            string name = string.Empty;
            System.Data.SqlClient.SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            try
            {
                string basePath = Common.GetWebConfigKey("ImgPath") + "GoodsImg/";
                //basePath = System.Web.HttpContext.Current.Server.MapPath(basePath);
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (!System.IO.Directory.Exists(basePath))
                {
                    System.IO.Directory.CreateDirectory(basePath);
                }
                var suffix = files[0].ContentType.Split('/');
                //获取文件格式
                //var _suffix = suffix[1].Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) ? "" : suffix[1];
                var _suffix = suffix[1];
                var _temp = System.Web.HttpContext.Current.Request["name"];
                //如果不修改文件名，则创建随机文件名
                if (!string.IsNullOrEmpty(_temp))
                {
                    name = _temp;
                }
                else
                {
                    Random rand = new Random(24 * (int)DateTime.Now.Ticks);
                    name = rand.Next() + "." + _suffix;
                }
                //文件保存
                var full = basePath + "D" + name;
                files[0].SaveAs(full);
                //大缩略图
                string bigThumbPath = basePath + "X" + name;
                MakeThumbnail(full, bigThumbPath, 400, 400, "Cut");
                //小缩略图
                string smallThumbPath = basePath + name;
                MakeThumbnail(full, smallThumbPath, 200, 200, "Cut");

                string barcode = name.Substring(0, name.IndexOf('-'));//商品编码
                System.Collections.Generic.List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and barcode='" + barcode + "'", "");
                if (l.Count > 0)
                {
                    //System.Collections.Generic.List<Hi.Model.BD_ImageList> ll = new System.Collections.Generic.List<Hi.Model.BD_ImageList>();
                    foreach (Hi.Model.BD_GoodsInfo item in l)
                    {
                        if (name.Substring(name.LastIndexOf('-') + 1, 3) == "001")
                        {
                            Hi.Model.BD_Goods gsmodel = new Hi.BLL.BD_Goods().GetModel(item.GoodsID);
                            gsmodel.Pic = name;
                            gsmodel.Pic2 = "X" + name;
                            gsmodel.Pic3 = "D" + name;
                            gsmodel.ts = DateTime.Now;
                            gsmodel.modifyuser = logUser.UserID;
                            new Hi.BLL.BD_Goods().Update(gsmodel, Tran);
                        }
                        System.Collections.Generic.List<Hi.Model.BD_ImageList> ll = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + item.GoodsID + " and pic  like '%" + barcode + "%'", "");
                        if (ll.Count == 0)
                        {
                            new Hi.BLL.BD_ImageList().Delete(item.GoodsID, logUser.CompID, Tran);
                        }
                        System.Collections.Generic.List<Hi.Model.BD_ImageList> lll = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + logUser.CompID + " and goodsid=" + item.GoodsID + " and pic  = '" + name + "'", "");
                        if (lll.Count == 0)
                        {
                            Hi.Model.BD_ImageList modelImg = new Hi.Model.BD_ImageList();
                            modelImg.CompID = logUser.CompID;
                            modelImg.GoodsID = item.GoodsID;
                            modelImg.Pic = name;
                            modelImg.Pic2 = "X" + name;
                            modelImg.Pic3 = "D" + name;
                            modelImg.IsIndex = 0;
                            modelImg.CreateDate = DateTime.Now;
                            modelImg.CreateUserID = logUser.UserID;
                            modelImg.ts = DateTime.Now;
                            modelImg.modifyuser = logUser.UserID;
                            new Hi.BLL.BD_ImageList().Add(modelImg, Tran);
                        }
                    }
                }
                Tran.Commit();
                var _result = "{\"jsonrpc\" : \"2.0\", \"result\" : null, \"id\" : \"" + name + "\"}";
                System.Web.HttpContext.Current.Response.Write(_result);
            }
            catch (Exception)
            {
                string path = Common.GetWebConfigKey("ImgPath");
                string path2 = path + "GoodsImg/D" + name;
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
                string path3 = path + "GoodsImg/X" + name;
                if (File.Exists(path3))
                {
                    File.Delete(path3);
                }
                string path4 = path + "GoodsImg/" + name;
                if (File.Exists(path4))
                {
                    File.Delete(path4);
                }
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                }
            }
            finally
            {
                DBUtility.SqlHelper.ConnectionClose();
            }
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
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}