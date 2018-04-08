    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.OleDb;
    using System.Web.UI.HtmlControls;
    using System.Net;
    using System.Configuration;
    using System.Text;

    public partial class Company_SysManager_DisEdit : CompPageBase
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
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        this.li1.Visible = false;
        this.li2.Visible = false;
        this.li3.Visible = false;
        this.li4.Visible = false;
        this.li5.Visible = false;
        this.li6.Visible = false;
        this.li7.Visible = false;
        this.li8.Visible = false;
        this.li9.Visible = false;
        this.li10.Visible = false;
        this.li11.Visible = false;
        this.li12.Visible = false;

        string fid = Request.QueryString["id"];
        if (!string.IsNullOrWhiteSpace(fid))
        {
            List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + fid + " and dr=0 and fileAlias='4'", "");
            foreach (Hi.Model.YZT_Annex item in annexList)
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
                    if (item.type == 5)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li1.Visible = true;
                            UpFileText.Controls.Add(div);
                            validDate.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if(item.type == 7)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li2.Visible = true;
                       
                            UpFileText2.Controls.Add(div);
                            validDate2.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 8)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li3.Visible = true;
                       
                            UpFileText3.Controls.Add(div);
                            validDate3.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 9)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li4.Visible = true;
                        
                            UpFileText4.Controls.Add(div);
                            validDate4.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 12)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li5.Visible = true;
                       
                            this.HidFfileName3.Value = item.fileName;
                            this.validDate3.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 13)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li6.Visible = true;

                            this.HidFfileName4.Value = item.fileName;
                            this.validDate4.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 14)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li7.Visible = true;
                        
                            this.HidFfileName5.Value = item.fileName;
                            this.validDate5.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 15)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li8.Visible = true;
                      
                            this.HidFfileName6.Value = item.fileName;
                            this.validDate6.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 16)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li9.Visible = true;
                        
                            this.HidFfileName7.Value = item.fileName;
                            this.validDate7.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 17)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li10.Visible = true;
                       
                            this.HidFfileName8.Value = item.fileName;
                            this.validDate8.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 18)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li11.Visible = true;
                       
                            this.HidFfileName9.Value = item.fileName;
                            this.validDate9.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 19)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li12.Visible = true;

                            this.HidFfileName10.Value = item.fileName;
                            this.validDate10.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                }
            
        }

        }
    }




    /// <summary>
    /// 页面数据绑定
    /// </summary>
    public void bind()
    {
        string fid = Request.QueryString["id"];
        if (!string.IsNullOrWhiteSpace(fid))
        {
            DataTable fCmaterialsModel = new Hi.BLL.YZT_FCmaterials().getDataModel(fid);
            if (fCmaterialsModel.Rows.Count>0) {
                this.txtDisCode.Value = fCmaterialsModel.Rows[0]["DisCode"].ToString();
                this.txtDisName.Value = fCmaterialsModel.Rows[0]["DisName"].ToString();
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