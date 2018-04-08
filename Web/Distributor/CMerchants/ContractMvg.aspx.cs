using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Distributor_CMerchants_ContractMvg : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBindLink();
    }


    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        //查询需要提供的资料
        List<Hi.Model.YZT_Annex> annexlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + this.KeyID + " and fileAlias in (1,2) and dr=0", "");
        if (annexlist != null && annexlist.Count > 0)
        {
            foreach (Hi.Model.YZT_Annex item in annexlist)
            {
                if (!string.IsNullOrEmpty(item.fileName))
                {
                    LinkButton linkFile = new LinkButton();
                    linkFile.Click += new EventHandler(Download_Click);

                    if (item.fileName.LastIndexOf("_") != -1)
                    {
                        string text = item.fileName.Substring(0, item.fileName.LastIndexOf("_")) + Path.GetExtension(item.fileName);
                        if (text.Length < 15)
                            linkFile.Text = text;
                        else
                        {
                            linkFile.Text = text.Substring(0, 15) + "...";
                        }
                        linkFile.Attributes.Add("title", text);
                    }
                    else
                    {
                        string text = item.fileName.Substring(0, item.fileName.LastIndexOf("-")) + Path.GetExtension(item.fileName);
                        if (text.Length < 15)
                            linkFile.Text = text;
                        else
                        {
                            linkFile.Text = text.Substring(0, 15) + "...";
                        }
                        linkFile.Attributes.Add("title", text);
                    }
                    linkFile.Style.Add("text-decoration", "underline");
                    linkFile.Attributes.Add("fileName", item.fileName);
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Controls.Add(linkFile);

                    if (item.fileAlias == "3")
                    {//合同
                        this.UpFile1.Visible = true;
                        UpFileText.Controls.Add(div);
                    }
                }
            }
        }
    }

    public void Download_Click(object sender, EventArgs e)
    {
        LinkButton bt = sender as LinkButton;
        string fileName = bt.Attributes["fileName"];
        string filePath = Server.MapPath("../../UploadFile/") + fileName;
        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "appliction/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误);
        }
    }

}