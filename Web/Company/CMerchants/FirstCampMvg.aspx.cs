using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Company_CMerchants_FirstCampMvg : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
        DataBindLink();
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID);

            if (fcmodel != null)
            {
                this.txtDisCode.Value = Common.GetDis(fcmodel.DisID, "DisCode");
                this.txtDisName.Value = Common.GetDis(fcmodel.DisID, "DisName");
                this.txtInvalidDate.Value = fcmodel.InvalidDate == DateTime.MinValue ? "" : fcmodel.InvalidDate.ToString("yyyy-MM-dd");
                this.txtForceDate.Value = fcmodel.ForceDate == DateTime.MinValue ? "" : fcmodel.ForceDate.ToString("yyyy-MM-dd");
            }
        }
    }


    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        //查询需要提供的资料
        List<Hi.Model.YZT_Annex> annexlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + this.KeyID + " and fileAlias in (1) and dr=0", "");
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
                    if (item.fileAlias == "1" && item.type == 5)
                    {//营业执照
                        this.UpFile1.Visible = true;
                        this.validDate1.InnerText = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        UpFileText.Controls.Add(div);
                    }
                    else if (item.fileAlias == "1" && item.type == 7)
                    {//医疗器械经营许可证
                        this.UpFile2.Visible = true;
                        UpFileText2.Controls.Add(div);
                        this.validDate2.InnerText = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                    }
                    else if (item.fileAlias == "1" && item.type == 9)
                    {//开户许可证
                        this.UpFile3.Visible = true;
                        UpFileText3.Controls.Add(div);
                        this.validDate3.InnerText = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                    }
                    else if (item.fileAlias == "1" && item.type == 8)
                    {//医疗器械备案
                        this.UpFile4.Visible = true;
                        UpFileText4.Controls.Add(div);
                        this.validDate4.InnerText = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
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