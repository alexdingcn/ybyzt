using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class Company_SysManager_CompServiceInfo : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();

        }
        DataBindLink();
    }
    public void DataBindLink()
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            if (!string.IsNullOrEmpty(comp.Attachment))
            {
                string[] files = comp.Attachment.Split(new char[] { ',' });
                foreach (string file in files)
                {
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
                        //HtmlImage img = new HtmlImage();
                        //img.Src = "../../images/icon_del.png";
                        //img.Attributes.Add("title", "删除附件");
                        //img.Attributes.Add("onclick", "AnnexDel(this,'Comp'," + CompID + ",'" + file + "')");
                        //div.Controls.Add(img);
                        DFile.Controls.Add(div);
                    }
                }
            }
        }
    }


    public void DataBinds()
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            lblShortName.InnerText = comp.ShortName;
            lblCompName.InnerText = comp.CompName;
            lblCompCode.InnerText = comp.CompCode;
            lblTel.InnerText = comp.Tel;
            lblLicence.InnerText = comp.creditCode;
            lblOrCode.InnerText = comp.OrganizationCode;
            lblZip.InnerText = comp.Zip;
            lblFax.InnerText = comp.Fax;
            lblAccount.InnerText = comp.Account;
            lblAddress.InnerText = comp.Address;
            lblIndusName.Id = comp.IndID.ToString();
            lblLegal.InnerText = comp.Legal;
            lblIdentitys.InnerText = comp.Identitys;
            lblPrincipal.InnerText = comp.Principal;
            lblPhone.InnerText = comp.Phone;
            lblInfo.InnerText = comp.ManageInfo;
            lblLegalTel.InnerText = comp.LegalTel;
            lblIsHot.InnerHtml = comp.HotShow == 1 ? "是" : "<i style='color:red;'>否</i>";
            QQ.InnerHtml = comp.QQ;
            //lblFinanceCode.InnerText = comp.FinanceCode;
            //lblFinanceName.InnerText = comp.FinanceName;
            List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("", "  isnull(dr,0)=0 and  compid='" + CompID + "' and utype=4", "");
            if (user2.Count > 0)
            {
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                if (user != null)
                {
                    //List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", "  isnull(dr,0)=0 and  compid='" + CompID + "' and type=4", "");
                    // if (user.Count > 0)
                    // {
                    lblUsername.InnerText = user.UserName;
                    lblUserPhone.InnerText = user.Phone;
                    lblUserTrueName.InnerText = user.TrueName;
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
            JScript.AlertMethod(this, "附件不存在", JScript.IconOption.错误, "function (){ location.href=location.href; }");
            return;
        }
    }
}