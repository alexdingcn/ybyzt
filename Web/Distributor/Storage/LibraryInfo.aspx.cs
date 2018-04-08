using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Distributor_Storage_StorageInfo : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            Bind();
        }
        DataBindLink();
    }

 

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string LibraryID = Request.QueryString["KeyID"];
        this.LibraryID.Value = LibraryID;
        if (!string.IsNullOrWhiteSpace(LibraryID))
        {

            Hi.Model.YZT_Library libraryModel = new Hi.BLL.YZT_Library().GetModel(Convert.ToInt32(LibraryID));
            if (libraryModel != null)
            {
                if (libraryModel.IState == 1)
                {
                    btnUpdate.Visible = false;
                    auditBtn.Visible = false;
                }
                this.LibraryNO.Value = libraryModel.LibraryNO;
                this.LibraryDate.Value = libraryModel.LibraryDate.ToString("yyyy-MM-dd");
                this.Salesman.Value = libraryModel.Salesman;
                this.PaymentDays.Value = libraryModel.PaymentDays.ToString();
                this.MoneyDate.Value = libraryModel.MoneyDate.ToString("yyyy-MM-dd");
                this.OrderNote.Value = libraryModel.Remark;
                this.Htname.Value = libraryModel.hospital;
            }

            List<Hi.Model.YZT_LibraryDetail> libraryDetailList = new Hi.BLL.YZT_LibraryDetail().GetList("", " dr=0 and LibraryID=" + LibraryID + "", "");
            Rep_StorageDetail.DataSource = libraryDetailList;
            Rep_StorageDetail.DataBind();
            if (libraryDetailList.Count > 0) oneTR.Visible = false;
        }


    }
    public void DataBindLink()
    {
        string LibraryID = Request.QueryString["KeyID"];
        if (!string.IsNullOrWhiteSpace(LibraryID))
        {
            List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + LibraryID + " and fileAlias=5 and type=11", "");
            if (AnnexDelList.Count > 0)
            {
                string file = "";
                foreach (Hi.Model.YZT_Annex item in AnnexDelList)
                {
                    file = item.fileName;
                    if (!string.IsNullOrEmpty(file))
                    {
                        LinkButton linkFile = new LinkButton();
                        linkFile.Click += new EventHandler(Download_Click);
                        if (file.LastIndexOf("_") != -1)
                        {
                            string text = file.Substring(0, file.LastIndexOf("_")) + Path.GetExtension(file);
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
                            string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        linkFile.Style.Add("margin-right", "5px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", file);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Controls.Add(linkFile);
                        DFile.Controls.Add(div);
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