using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Distributor_CMerchants_FirstCampInfo : DisPageBase
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
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        if (this.KeyID > 0)
        {
            string sql = @"select fc.ID,fc.State,fc.CompID,fc.ForceDate,fc.InvalidDate,fc.Remark,fc.ApplyRemark,
 cm.CMCode,cm.CMName,g.CategoryID,cm.GoodsID,g.GoodsCode,g.GoodsName,cm.ProvideData,info.ValueInfo,ht.HospitalName from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID 
 left join BD_GoodsInfo info  on info.ID=cm.GoodsID
left join BD_Goods g on info.GoodsID=g.ID
 left join SYS_Hospital ht on fc.HtID=ht.ID where fc.ID=" + this.KeyID;
            DataTable FirstCampDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            if (FirstCampDt != null && FirstCampDt.Rows.Count > 0)
            {
                this.hidid.Value = FirstCampDt.Rows[0]["ID"].ToString();
                this.txtCMCode.Value = FirstCampDt.Rows[0]["CMCode"].ToString();
                this.txtCMName.Value = FirstCampDt.Rows[0]["CMName"].ToString();
                this.txtGoodsCode.Value = FirstCampDt.Rows[0]["GoodsCode"].ToString();
                this.txtGoodsName.Value = FirstCampDt.Rows[0]["GoodsName"].ToString();
                //this.txtGoodsCode.Value = Common.GetGoodsName(FirstCampDt.Rows[0]["GoodsID"].ToString(), "GoodsCode");
                //this.txtGoodsName.Value = Common.GetGoodsName(FirstCampDt.Rows[0]["GoodsID"].ToString(), "GoodsName");

                this.txtCompName.Value = Common.GetCompValue(FirstCampDt.Rows[0]["CompID"].ToString().ToInt(0), "CompName").ToString();
                this.txtValueInfo.Value = FirstCampDt.Rows[0]["ValueInfo"].ToString();

                this.txtForceDate.Value = (FirstCampDt.Rows[0]["ForceDate"] == null || FirstCampDt.Rows[0]["ForceDate"].ToString()=="") ? "" : Convert.ToDateTime(FirstCampDt.Rows[0]["ForceDate"]).ToString("yyyy-MM-dd");
                this.txtInvalidDate.Value = (FirstCampDt.Rows[0]["InvalidDate"] == null || FirstCampDt.Rows[0]["InvalidDate"] .ToString()=="")? "" : Convert.ToDateTime(FirstCampDt.Rows[0]["InvalidDate"]).ToString("yyyy-MM-dd");
                this.txtHtName.Value = FirstCampDt.Rows[0]["HospitalName"].ToString();

                this.txtState.Value = FirstCampDt.Rows[0]["State"].ToString() == "0" ? "申请" : FirstCampDt.Rows[0]["State"].ToString() == "1" ? "驳回" : "通过";

                this.txtRemark.Value = FirstCampDt.Rows[0]["Remark"].ToString();

                this.txtApplyRemark.Value = FirstCampDt.Rows[0]["ApplyRemark"].ToString();

                if (FirstCampDt.Rows[0]["State"].ToString() == "1")
                {
                    this.libtnSubmit.Visible = true;
                    this.libtnEdit.Visible = true;
                }
            }

            string contsql = @"select * from YZT_ContractDetail cd left join YZT_Contract c on cd.ContID=c.ID where cd.FCID=" + this.KeyID;
            DataTable contDt = SqlHelper.Query(SqlHelper.LocalSqlServer, contsql).Tables[0];

             if (contDt != null && contDt.Rows.Count > 0)
             {
                 this.rptCon.DataSource = contDt;
                 this.rptCon.DataBind();
             }

        }
    }

    /// <summary>
    /// 首营信息提交
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Edit(object sender, EventArgs e)
    {
        if (this.KeyID > 0)
        {
            Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID);

            if (fcmodel != null)
            {
                fcmodel.State = 0;
                fcmodel.ts = DateTime.Now;
                fcmodel.modifyuser = this.UserID;
                if (new Hi.BLL.YZT_FirstCamp().Update(fcmodel)) {
                    Response.Redirect("FirstCampInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                }
            }
        }
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
                        if (item.fileAlias=="1"&&item.type == 5)
                        {//营业执照
                            this.UpFile1.Visible = true;
                            this.validDate1.Value =item.validDate==DateTime.MinValue?"": item.validDate.ToString("yyyy-MM-dd");
                            UpFileText.Controls.Add(div);
                        }
                        else if (item.fileAlias == "1" && item.type == 7)
                        {//医疗器械经营许可证
                            this.UpFile2.Visible = true;
                            UpFileText2.Controls.Add(div);
                            this.validDate2.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        }
                        else if (item.fileAlias == "1" && item.type == 9)
                        {//开户许可证
                            this.UpFile3.Visible = true;
                            UpFileText3.Controls.Add(div);
                            this.validDate3.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        }
                        else if (item.fileAlias == "1" && item.type == 8)
                        {//医疗器械备案
                            this.UpFile4.Visible = true;
                            UpFileText4.Controls.Add(div);
                            this.validDate4.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        }
                        else if (item.fileAlias == "2" && item.type == 2)
                        {   //授权书
                            this.UpFile5.Visible = true;
                            UpFileText5.Controls.Add(div);
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