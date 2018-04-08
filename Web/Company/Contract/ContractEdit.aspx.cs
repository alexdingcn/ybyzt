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

public partial class Company_Contract_ContractEdit : CompPageBase
{
    public string TitleType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            //默认绑定
            if (obj.ToString() == "dislist") 
            {
                Response.Write(disBing());
                Response.End();
            }
            if (obj.ToString() == "disBings")
            {
                Response.Write(disBings());
                Response.End();
            }
            //选中的商品
            if (obj.ToString() == "goodsInfo")
            {
                string goodsinfoid = Request["goodsInfoId"] + "";
                string disid = Request["disid"] + "";
                string str = Request["str"] + "";
                Response.Write(disBing(goodsinfoid, disid, str));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            var date=DateTime.Now.ToString("yyyy-MM-dd");
            this.txtcontractDate.Value = date;
            this.txtForceDate.Value = date;

            string  type = Request["type"];
            string no = Common.DesDecrypt(Request["no"], Common.EncryptKey);//有值是修改 
            DataTable dt = Common.BindDisList(this.CompID.ToString());//代理商绑定
            if (dt != null)
            {
                this.DropDis.DataSource = dt;
                this.DropDis.DataTextField = "disname";
                this.DropDis.DataValueField = "id";
                this.DropDis.DataBind();

                if (Request["disid"] + "" != "")
                {
                    this.DropDis.SelectedValue = Request["disid"] + "";
                }
            }
            this.hidCompId.Value = this.CompID.ToString();
            Bind();
            DataBindLink();
        }
    }
    /// <summary>
    /// 选择商品
    /// </summary>
    /// <returns></returns>
    public string disBing(string goodsinfoid = "", string disid = "", string str="")
    {

        StringBuilder strwhere = new StringBuilder();
        StringBuilder strwhere1 = new StringBuilder();

        //启用库存
       // strwhere.AppendFormat(" and a.compid=" + this.CompID );
        if (!Util.IsEmpty(goodsinfoid))
        {
            strwhere.AppendFormat(" and a.id in(" + goodsinfoid + ")");
            strwhere1.AppendFormat(" and c.GoodsID in(" + goodsinfoid + ")");
        }
        if (!Util.IsEmpty(disid))
        {
            //if (str == "CM")
            //{
            strwhere1.AppendFormat(" and f.DisID=" + disid);
            //}
        }

        DataTable dt = new Hi.BLL.BD_GoodsInfo().getGoodsModels(strwhere.ToString()).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            DataTable fcdt = new Hi.BLL.BD_GoodsInfo().getGoodsCMerchants(strwhere1.ToString()).Tables[0];

            if (fcdt != null && fcdt.Rows.Count > 0)
            {
                foreach (DataRow item in fcdt.Rows)
                {
                    DataRow[] dr = dt.Select(" Id=" + item["GoodsID"]);
                    if (dr.Length > 0)
                    {
                        dr[0]["FirstCampID"] = item["FirstCampID"];
                        dr[0]["htid"] = item["htid"];
                        dr[0]["HospitalName"] = item["HospitalName"];
                        dr[0]["AreaID"] = item["AreaID"];
                        dr[0]["AreaName"] = item["AreaName"];
                    }
                }
            }
        }
        return ConvertJson.ToJson(dt);
    }

    public string disBings(string goodsinfoid = "")
    {

        StringBuilder strwhere = new StringBuilder();
        //启用库存
        // strwhere.AppendFormat(" and a.compid=" + this.CompID );
        if (!Util.IsEmpty(goodsinfoid))
        {
            strwhere.AppendFormat(" and a.id in(" + goodsinfoid + ")");
        }
        DataTable dt = new Hi.BLL.BD_GoodsInfo().GetGoodsModel(strwhere.ToString()).Tables[0];
        return ConvertJson.ToJson(dt);
    }

    /// <summary>
    /// 修改绑定
    /// </summary>
    /// <param name="type">入库or出库</param>
    /// <param name="no">单据ID</param>
    public void Bind()
    {
        string ContID = Request.QueryString["cid"];
        this.Cid.Value = Request.QueryString["cid"];
        if (!string.IsNullOrWhiteSpace(ContID))
        {
            Hi.Model.YZT_Contract contractModel = new Hi.BLL.YZT_Contract().GetModel(Convert.ToInt32(ContID));
            if (contractModel != null)
            {
                this.txtcontractNO.Value = contractModel.contractNO;
                this.txtcontractDate.Value = contractModel.contractDate.ToString("yyyy-MM-dd");
                this.txtForceDate.Value = contractModel.ForceDate.ToString("yyyy-MM-dd");
                this.txtInvalidDate.Value = contractModel.InvalidDate.ToString("yyyy-MM-dd");
                this.txtRemark.Value = contractModel.Remark;
                this.DropDis.SelectedValue = contractModel.DisID.ToString();
                this.CState.Value = contractModel.CState.ToString();
            }
            DataTable dt = new Hi.BLL.YZT_ContractDetail().getDataTable(ContID);
            if (dt.Rows.Count > 0)
            {
                tbodyTR.Visible = false;
                this.RepContractDetail.DataSource = dt;
                this.RepContractDetail.DataBind();
            }

            List<Hi.Model.YZT_Annex> AnnexDelList = new Hi.BLL.YZT_Annex().GetList("", " dr=0 and fcID=" + ContID + " and fileAlias=3", "");
            if (AnnexDelList.Count > 0)
            {
                string fileName = "";
                foreach (Hi.Model.YZT_Annex item in AnnexDelList) {
                    fileName += item.fileName + ",";
                }
                HidFfileName.Value = fileName;
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
                        HtmlImage img = new HtmlImage();
                        img.Src = "../../images/icon_del.png";
                        img.Attributes.Add("title", "删除附件");
                        img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
                        div.Controls.Add(img);
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