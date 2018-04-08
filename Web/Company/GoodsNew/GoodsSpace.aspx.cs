using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Company_GoodsNew_GoodsSpace : CompPageBase
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
                    string name = Request["name"].ToString();//模糊搜索值
                    string soft = Request["soft"].ToString();//下拉排序值
                    Response.Write(ConvertJson.ToJson(Bind(name, soft)));
                    Response.End();
                }
            }
            else
            {
                DataTable dt = Bind("", "1");
                rptImg.DataSource = dt;
                rptImg.DataBind();
            }
        }
    }
    public DataTable Bind(string name = "", string soft = "")
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
            }
            else
            {
                continue;
            }
            dt.Rows.Add(dr1);
        }
        DataView dv = dt.DefaultView;
        if (soft == "1")
        {//按修改时间从晚到早
            dv.Sort = "Time Desc";
        }
        else if (soft == "2")
        { //按修改时间时间从早到晚
            dv.Sort = "Time Asc";
        }
        else if (soft == "7")
        {//按图片从大到小序
            dv.Sort = "Size Desc";
        }
        else if (soft == "8")
        {//按图片从小到大序
            dv.Sort = "Size Asc";
        }
        else if (soft == "6")
        {//按图片名升序
            dv.Sort = "Name Asc";
        }
        else if (soft == "5")
        {//按图片名降序
            dv.Sort = "Name Desc";
        }
        //  DataTable dt2 = dv.ToTable();
        return dv.ToTable();
    }
}