using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Diagnostics;
public partial class Company_GoodsNew_PicSpaceList : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object obj = Request["action"];
            if (obj != null)
            {
                if (obj.ToString() == "mohu")
                {
                    string name =Common.NoHTML( Request["name"].ToString());//模糊搜索值
                    string soft =Common.NoHTML( Request["soft2"].ToString());//下拉排序值
                    string soft2 =Common.NoHTML( Request["soft"].ToString());//下拉排序
                    Response.Write(ConvertJson.ToJson(Bind(name, soft.Trim(), soft2.Trim())));
                    Response.End();
                }
                else if (obj.ToString() == "delImg")
                {
                    string filepath = Request["filepath"];//图片名称
                    Response.Write(DelImg(filepath));
                    Response.End();
                }
            }
            else
            {
                DataTable dt = Bind("", "sabc down", "时间");//默认时间降序
                rptImg.DataSource = dt;
                rptImg.DataBind();
            }
        }
    }
    public DataTable Bind(string name = "", string soft = "", string soft2 = "")
    {
        DirectoryInfo imagesfile = new DirectoryInfo(Common.GetWebConfigKey("ImgPath") + "PicSpace/" + this.CompID + "/");//Server.MapPath("./images"));
        if (!imagesfile.Exists)
        {
            imagesfile.Create();
        }
        FileInfo[] str = imagesfile.GetFiles("*");
        DataTable dt = new DataTable();
        dt.Columns.Add("Name", typeof(string));     //名称
        dt.Columns.Add("Pixel", typeof(string));     //像素
        dt.Columns.Add("Time", typeof(DateTime));     //时间
        dt.Columns.Add("Size", typeof(decimal));     //大小
        dt.Columns.Add("Type", typeof(string));     //类型
        for (int i = 0; i < str.Length; i++)
        {
            DataRow dr1 = dt.NewRow();
            if ((str[i].Extension == ".jpg" || str[i].Extension == ".jpeg" || str[i].Extension == ".png") && (str[i].Length < 3 * 1024 * 1024 && (name == "" ? 1 == 1 : str[i].Name.ToUpper().IndexOf(name.ToUpper()) != -1)))
            {
                //单图片不能大于3m
                dr1["Name"] = str[i].Name;
                System.Drawing.Image pic = System.Drawing.Image.FromFile(str[i].FullName);//strFilePath是该图片的绝对路径
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值 
                pic.Dispose();
                dr1["Pixel"] = intWidth + "*" + intHeight;
                dr1["Time"] = str[i].CreationTime;
                dr1["Size"] = str[i].Length;
                dr1["Type"] = str[i].Extension;
            }
            else
            {
                continue;
            }
            dt.Rows.Add(dr1);
        }
        DataView dv = dt.DefaultView;
        if (soft2 == "时间")
        {
            if (soft.Split(' ')[1] == "down")
            {
                //按修改时间从晚到早
                dv.Sort = "Time Desc";
            }
            else
            {
                //按修改时间时间从早到晚
                dv.Sort = "Time Asc";
            }
        }
        else if (soft2 == "大小")
        {
            if (soft.Split(' ')[1] == "down")
            {
                //按图片从大到小序
                dv.Sort = "Size Desc";
            }
            else
            {
                //按图片从小到大序
                dv.Sort = "Size Asc";
            }
        }
        else if (soft2 == "名称")
        {
            if (soft.Split(' ')[1] == "down")
            {
                //按图片名降序
                dv.Sort = "Name Desc";
            }
            else
            {
                //按图片名升序
                dv.Sort = "Name Asc";
            }
        }
        return dv.ToTable();
    }
    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public string DelImg(string filepath)
    {
        try
        {
            string path = Common.GetWebConfigKey("ImgPath");
            if (!string.IsNullOrEmpty(filepath))
            {
                int z = 0;
                string[] list = filepath.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (!string.IsNullOrEmpty(list[i]))
                    {
                        string path2 = path + "PicSpace/" + this.CompID + "/" + list[i];
                        if (File.Exists(path2))
                        {
                            //if ((File.GetAttributes(path2) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            //{
                            //    // 如果是将文件的属性设置为Normal
                            //    File.SetAttributes(path2, FileAttributes.Normal);
                            //}
                            System.GC.Collect();
                            File.Delete(path2);
                            z++;
                        }
                    }
                }
                if (z != 0)
                {
                    return "cg";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }
        catch (Exception)
        {
            return "";
        }
    }
}