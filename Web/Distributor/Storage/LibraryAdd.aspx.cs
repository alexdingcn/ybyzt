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

public partial class Distributor_Storage_StorageAdd : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBindLink();
        object obj = Request["action"];
        if (obj != null)
        {
            //选中的商品
            if (obj.ToString() == "goodsInfo")
            {
                string OrderOutDetailID = Request["OrderOutDetailID"] + "";
                Response.Write(disBings(OrderOutDetailID));
                Response.End();
            }
        }

        if (!IsPostBack)
        {
           
            Bind();
        }

    }

    public string disBings(string OrderOutDetailID = "")
    {

        string strwhere = "  DisID="+DisID+" ";
        if (!Util.IsEmpty(OrderOutDetailID))
        {
            strwhere+=" and id in(" + OrderOutDetailID + ")";
        }
        List<Hi.Model.YZT_GoodsStock> storageDetailList = new Hi.BLL.YZT_GoodsStock().GetList("", strwhere, "");
        DataTable dt = Common.FillDataTable(storageDetailList);
        return ConvertJson.ToJson(dt);
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
                this.LibraryNO.Value = libraryModel.LibraryNO;
                this.LibraryDate.Value = libraryModel.LibraryDate.ToString("yyyy-MM-dd");
                this.Salesman.Value = libraryModel.Salesman;
                this.PaymentDays.Value = libraryModel.PaymentDays.ToString();
                this.MoneyDate.Value = libraryModel.MoneyDate.ToString("yyyy-MM-dd");
                this.OrderNote.Value = libraryModel.Remark;
                this.HtDrop.Value = libraryModel.hospital.ToString();
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
                string HidFfileNamestr = "";
                foreach (Hi.Model.YZT_Annex item in AnnexDelList)
                {
                    file = item.fileName;
                    if (!string.IsNullOrEmpty(file))
                    {
                        HidFfileNamestr += file + ',';
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
                        HtmlImage img = new HtmlImage();
                        img.Src = "../../images/icon_del.png";
                        img.Attributes.Add("title", "删除附件");
                        img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
                        div.Controls.Add(img);
                        DFile.Controls.Add(div);
                    }
                }
                this.HidFfileName.Value = HidFfileNamestr;
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


    public string getStockNum(string StockID, string OutNum)
    {
        if (string.IsNullOrWhiteSpace(StockID))
        return "0";
        else{
            Hi.Model.YZT_GoodsStock model = new Hi.BLL.YZT_GoodsStock().GetModel(Convert.ToInt32(StockID));
            if (model == null) return "0";
            else if (model.StockNum < (model.StockUseNum -Convert.ToDecimal(OutNum))) return "0";
            else return (model.StockNum - (model.StockUseNum - Convert.ToDecimal(OutNum))).ToString();
            //else return model.StockNum.ToString();
        }
         
    }


}