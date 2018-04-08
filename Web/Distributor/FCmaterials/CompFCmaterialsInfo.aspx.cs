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

    public partial class Company_SysManager_DisEdit :DisPageBase
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
        string fid = Request.QueryString["fid"];
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
                        UpFileText.Controls.Add(div);
                    }
                    else
                    {
                        UpFileText2.Controls.Add(div);
                    }
                }
            
        }

        }
    }


    public string getFileName(string name)
    {
        if (name.LastIndexOf("_") != -1)
        {
            string text = name.Substring(0, name.LastIndexOf("_")) + Path.GetExtension(name);
            if (text.Length < 15)
                return text;
            else
            {
                return  text.Substring(0, 15) + "...";
            }
        }
        else
        {
            string text = name.Substring(0, name.LastIndexOf("-")) + Path.GetExtension(name);
            if (text.Length < 15)
                return text;
            else
            {
                return text.Substring(0, 15) + "...";
            }
        }
    }

    /// <summary>
    /// 页面数据绑定
    /// </summary>
    public void bind()
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

        string fid = Request.QueryString["fid"];
        if (!string.IsNullOrWhiteSpace(fid))
        {
            string CompID = Request.QueryString["CompID"];
            Hi.Model.YZT_FCmaterials fCmaterialsModel =new Hi.BLL.YZT_FCmaterials().GetModel(Convert.ToInt32(fid));
            if (fCmaterialsModel!=null) {

                DataTable table = new Hi.BLL.YZT_FCmaterials().getDataModel(fCmaterialsModel.DisID.ToString(), "and f.ID=" + fid + " and d.ID=" + CompID);
                if (table != null && table.Rows.Count>0)
                {
                    this.txtComCode.Value = table.Rows[0]["CompCode"].ToString();
                    this.txtComName.Value = table.Rows[0]["CompName"].ToString();

                    dataBindGoods(table.Rows[0]["CompID"].ToString());
                }

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
                            this.li1.Visible = true;

                            //营业执照绑定
                            this.HidFfileName.Value = item.fileName;
                            this.validDate.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 6)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li2.Visible = true;

                            //生产许可证绑定
                            this.HidFfileName2.Value = item.fileName;
                            this.validDate2.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 12)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li3.Visible = true;

                            this.HidFfileName3.Value = item.fileName;
                            this.validDate3.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 13)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li4.Visible = true;

                            this.HidFfileName4.Value = item.fileName;
                            this.validDate4.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 14)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li5.Visible = true;

                            this.HidFfileName5.Value = item.fileName;
                            this.validDate5.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 15)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li6.Visible = true;

                            this.HidFfileName6.Value = item.fileName;
                            this.validDate6.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 16)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li7.Visible = true;

                            this.HidFfileName7.Value = item.fileName;
                            this.validDate7.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 17)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li8.Visible = true;

                            this.HidFfileName8.Value = item.fileName;
                            this.validDate8.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 18)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li9.Visible = true;

                            this.HidFfileName9.Value = item.fileName;
                            this.validDate9.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                    else if (item.type == 19)
                    {
                        if (!string.IsNullOrWhiteSpace(item.fileName))
                        {
                            this.li10.Visible = true;

                            this.HidFfileName10.Value = item.fileName;
                            this.validDate10.Value = item.validDate.ToString("yyyy/MM/dd");
                        }
                    }
                }
            }

            
        }
    }


    public void dataBindGoods(string CompID)
    {
        string goodsid = string.Empty;
        string strWhere = " CompID=" + CompID + " and isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and isnull(registeredCertificate,'')<>''";
        string ss = "select distinct cd.GoodsID from (select i.GoodsID  from  (select cd.GoodsID from  YZT_Contract con left join  YZT_ContractDetail cd on con.ID=cd.ContID where con.DisID=" + this.DisID + " and con.CState=1 union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp fc on cm.ID=fc.CMID left join YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.CompID in (" + CompID + ") and fc.DisID=" + this.DisID + ") cd left join BD_GoodsInfo i on i.ID=cd.GoodsID) cd";

        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, ss).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                goodsid += goodsid == "" ? item["GoodsID"].ToString() : "," + item["GoodsID"].ToString();
            }
        }

        List<Hi.Model.BD_Goods> glist = new Hi.BLL.BD_Goods().GetList("", strWhere + " and ID in(" + goodsid + ")", "");

        this.rptGoods.DataSource = glist;
        this.rptGoods.DataBind();

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

    /// <summary>
    /// 商品资格证查看
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FileOut_Click(object sender, EventArgs e)
    {
        string fileName = this.FileName.Value.Trim(); ;
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