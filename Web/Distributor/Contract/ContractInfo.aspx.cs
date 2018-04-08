using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DBUtility;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Company_Contract_ContractEdit : DisPageBase
{
    public string cid = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        cid = Request.QueryString["cid"];
        if (!IsPostBack)
        {
            
            this.hidCompId.Value = this.CompID.ToString();

            Bind();
        }
        DataBindLink();
    }


    /// <summary>
    /// 修改绑定
    /// </summary>
    public void Bind()
    {
        string ContID = Request.QueryString["cid"];
        if (!string.IsNullOrWhiteSpace(ContID)) {

            Hi.Model.YZT_Contract contractModel = new Hi.BLL.YZT_Contract().GetModel(Convert.ToInt32(ContID));
            if (contractModel != null) {

                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(contractModel.CompID));
                if(comp!=null)
                this.txtCompName.Value = comp.CompName;
                this.txtcontractNO.Value = contractModel.contractNO;
                this.txtcontractDate.Value = contractModel.contractDate.ToString("yyyy-MM-dd");
                this.txtForceDate.Value = contractModel.ForceDate.ToString("yyyy-MM-dd");
                this.txtInvalidDate.Value = contractModel.InvalidDate.ToString("yyyy-MM-dd");
                this.txtRemark.Value = contractModel.Remark;
                this.txtState.Value = contractModel.CState.ToString() == "1" ? "启用" : "停用";
            }

            DataTable dt = new Hi.BLL.YZT_ContractDetail().getDataTable(ContID);
            if (dt.Rows.Count > 0)
            {
                this.RepContractDetail.DataSource = dt;
                this.RepContractDetail.DataBind();
            }

        }
    }


    public string getHtName(string htid)
    {
        Hi.Model.SYS_Hospital hospital = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(htid));
        if (hospital != null)
            return hospital.HospitalName;
        else
            return "选择医院";
    }



    public void DataBindLink()
    {
        string id = Request.QueryString["cid"];
        if (!string.IsNullOrWhiteSpace(id))
        {
            List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + id + " and fileAlias=3", "");
            if (AnnexDelList .Count>0)
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