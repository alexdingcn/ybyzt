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
    public int fid=0;
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
        List<Hi.Model.YZT_FCmaterials> fCmaterialsList = new Hi.BLL.YZT_FCmaterials().GetList("", " CompID =" + CompID + " and dr=0 ", "");
        if (fCmaterialsList.Count > 0)
        {
            List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + fCmaterialsList[0].ID + " and dr=0 and fileAlias='4'", "");
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

                    }
                    else
                    {

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
        List<Hi.Model.YZT_FCmaterials> fCmaterialsList = new Hi.BLL.YZT_FCmaterials().GetList("", " CompID ="+CompID+" and dr=0 ", "");

        if (fCmaterialsList.Count > 0)
        {
            //修改
            Hi.Model.YZT_FCmaterials fCmaterialsModel = fCmaterialsList[0];
            fid = fCmaterialsModel.ID;
            this.txtRise.Value = fCmaterialsModel.Rise;
            this.txtContent.Value = fCmaterialsModel.Content;
            this.txtOBank.Value = fCmaterialsModel.OBank;
            this.txtOAccount.Value = fCmaterialsModel.OAccount;
            this.txtTRNumber.Value = fCmaterialsModel.TRNumber;
            List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + fCmaterialsModel.ID + " and dr=0 and fileAlias='4'", "");
            foreach (Hi.Model.YZT_Annex item in annexList)
            {
                if (item.type == 5)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        //营业执照绑定
                        this.HidFfileName.InnerHtml = item.fileName;
                        this.validDate.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 6)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        //生产许可证绑定
                        this.HidFfileName2.InnerHtml = item.fileName;
                        this.validDate2.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 12)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName3.InnerHtml = item.fileName;
                        this.validDate3.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 13)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName4.InnerHtml = item.fileName;
                        this.validDate4.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 14)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName5.InnerHtml = item.fileName;
                        this.validDate5.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 15)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName6.InnerHtml = item.fileName;
                        this.validDate6.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 16)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName7.InnerHtml = item.fileName;
                        this.validDate7.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 17)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName8.InnerHtml = item.fileName;
                        this.validDate8.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 18)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName9.InnerHtml = item.fileName;
                        this.validDate9.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
                else if (item.type == 19)
                {
                    if (!string.IsNullOrWhiteSpace(item.fileName))
                    {
                        this.HidFfileName10.InnerHtml = item.fileName;
                        this.validDate10.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }
            }
        }
        else
        {
            fid = 0;
        }
    }


    /// <summary>
    /// 获取页面数据Model
    /// </summary>
    /// <returns></returns>
    private Hi.Model.YZT_FCmaterials getfCmaterialsModel(Hi.Model.YZT_FCmaterials fCmaterialsModel) {
        fCmaterialsModel.CompID = CompID;
        fCmaterialsModel.DisID = 0;
        fCmaterialsModel.type = 1;
        fCmaterialsModel.Rise = this.txtRise.Value.Trim();
        fCmaterialsModel.Content = this.txtContent.Value.Trim();
        fCmaterialsModel.OBank = this.txtOBank.Value.Trim();
        fCmaterialsModel.OAccount = this.txtOAccount.Value.Trim();
        fCmaterialsModel.TRNumber = this.txtTRNumber.Value.Trim();
        fCmaterialsModel.ts = DateTime.Now;
        fCmaterialsModel.modifyuser = UserID;

        return fCmaterialsModel;
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