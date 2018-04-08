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

public partial class Distributor_Payment_PaymentAdd : DisPageBase
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
                string inindex = Request["inindex"] + "";
                Response.Write(disBings(OrderOutDetailID, inindex));
                Response.End();
            }
        }

        if (!IsPostBack)
        {
           
            Bind();
        }

    }

    public string disBings(string OrderOutDetailID = "",string inindex="")
    {

        string strwhere = "  DisID="+DisID+" ";
        if (!Util.IsEmpty(OrderOutDetailID))
        {
            strwhere+=" and id in(" + OrderOutDetailID + ")";
        }
        if (inindex == "-1") {
            List<Hi.Model.YZT_LibraryDetail> DetailList1 = new Hi.BLL.YZT_LibraryDetail().GetList("", strwhere, "");
            DataTable dt2 = Common.FillDataTable(DetailList1);
            return ConvertJson.ToJson(dt2);
        }
        List<Hi.Model.YZT_GoodsStock> DetailList2 = new Hi.BLL.YZT_GoodsStock().GetList("", strwhere, "");
        DataTable dt = Common.FillDataTable(DetailList2);
        return ConvertJson.ToJson(dt);
    }


    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        DataTable ht = new Hi.BLL.YZT_Library().getHtDrop(DisID.ToString());
        this.HtDrop.DataSource = ht;
        this.HtDrop.DataTextField = "hospital";
        this.HtDrop.DataValueField = "hospital";
        this.HtDrop.DataBind();

        string PaymentID = Request.QueryString["KeyID"];
        this.PaymentID.Value = PaymentID;
        if (!string.IsNullOrWhiteSpace(PaymentID))
        {

            Hi.Model.YZT_Payment paymentModel = new Hi.BLL.YZT_Payment().GetModel(Convert.ToInt32(PaymentID));
            if (paymentModel != null)
            {
                this.PaymentNO.Value = paymentModel.PaymentNO;
                this.PaymentDate.Value = paymentModel.PaymentDate.ToString("yyyy-MM-dd");
                this.IState.Value = paymentModel.IState.ToString();
                this.PaymentAmount.Value = paymentModel.PaymentAmount.ToString("#0.00");
                this.Remark.Value = paymentModel.Remark.ToString();
                this.HtDrop.SelectedValue = paymentModel.hospital.ToString();
                this.HtDrop.Enabled = false;
            }

            List<Hi.Model.YZT_PaymentDetail> libraryDetailList = new Hi.BLL.YZT_PaymentDetail().GetList("", " dr=0 and PaymentID=" + PaymentID + "", "");
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
            List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + LibraryID + " and fileAlias=6 and type=11", "");
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


}