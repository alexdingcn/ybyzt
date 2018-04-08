using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

public partial class Company_SysManager_CompServiceEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.BindIndDDL(txtIndusName);
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
                        //img.Attributes.Add("onclick", "ConFirmDelteAnnex(this,'Comp'," + CompID + ",'" + file + "')");
                        //div.Controls.Add(img);
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
    public void DataBinds()
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            if (comp.ShortName == "" && comp.CompName.Length <= 12)
            {
                txtShortName.Value = comp.CompName;
            }
            else
            {
                txtShortName.Value = comp.ShortName;
            }
            txtOrcode.Value = comp.OrganizationCode;
            txtCompName.Value = comp.CompName;
            lblCompCode.InnerText = comp.CompCode;
            txtTel.Value = comp.Tel;
            txtZip.Value = comp.Zip;
            txtFax.Value = comp.Fax;
            txtLicence.Value = comp.creditCode;
            txtAccount.Value = comp.Account;
            txtAddress.Value = comp.Address;
            txtLegal.Value = comp.Legal;
            txtIdentitys.Value = comp.Identitys;
            txtPrincipal.Value = comp.Principal;
            txtPhone.Value = comp.Phone;
            txtInfo.Value = comp.ManageInfo;
            txtLegalTel.Value = comp.LegalTel;
            txtIndusName.SelectedValue = comp.IndID.ToString();
            QQ.Value = comp.QQ;
            if (comp.HotShow == 0)
            {
                rdHotShowNo.Checked = true;
                rdHotShowYes.Checked = false;
            }

            //txtFinanceCode.Value = comp.FinanceCode.ToString();
            //txtFinanceName.Value = comp.FinanceName.ToString();
            List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("", "  isnull(dr,0)=0 and  compid='" + CompID + "' and utype=4", "");
            if (user2.Count > 0)
            {
               Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                if (user!=null)
                {
                    txtUsername.Disabled = true;
                    txtUserPhone.Disabled = true;
                    txtUsername.Value = user.UserName;
                    txtUserPhone.Value = user.Phone;
                    txtUserTrueName.Value = user.TrueName;
                }
            }
        }
    }
    protected void btn_Save(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            if (Common.CompExistsAttribute("CompName", txtCompName.Value.Trim(), CompID.ToString()))
            {
                JScript.AlertMsgOne(this, "该厂商名称已存在！", JScript.IconOption.错误);
                return;
            }
            comp.ShortName =Common.NoHTML( txtShortName.Value.Trim());
            comp.CompName = Common.NoHTML(txtCompName.Value.Trim());
            comp.creditCode = Common.NoHTML(txtLicence.Value.Trim());
            comp.Tel = Common.NoHTML(txtTel.Value.Trim());
            comp.Fax = Common.NoHTML(txtFax.Value.Trim());
            comp.Zip = Common.NoHTML(txtZip.Value.Trim());
            comp.Account = Common.NoHTML(txtAccount.Value.Trim());
            comp.OrganizationCode = Common.NoHTML(txtOrcode.Value.Trim());
            comp.Legal = Common.NoHTML(txtLegal.Value.Trim());
            comp.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
            comp.LegalTel = Common.NoHTML(txtLegalTel.Value.Trim());
            comp.HotShow = rdHotShowYes.Checked ? 1 : 0;
            comp.QQ = Common.NoHTML(QQ.Value.Trim());
            //comp.FinanceCode = txtFinanceCode.Value.Trim();
            //comp.FinanceName = txtFinanceName.Value.Trim();

            if (txtPrincipal.Value.Trim() == "")
            {
                comp.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
            }
            else
            {
                comp.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
            }
            if (txtPhone.Value.Trim() == "")
            {
                comp.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
            }
            else
            {
                comp.Phone = Common.NoHTML(txtPhone.Value.Trim());
            }
            comp.ManageInfo = Common.NoHTML(txtInfo.Value.Trim());
            comp.IndID = txtIndusName.SelectedValue.ToInt(0);
            if (HidFfileName.Value != "")
            {
                if (string.IsNullOrEmpty(comp.Attachment))
                {
                    comp.Attachment = Common.NoHTML(HidFfileName.Value);
                }
                else
                {
                    comp.Attachment += "," + Common.NoHTML(HidFfileName.Value);
                }
            }
            comp.Address = Common.NoHTML(txtAddress.Value.Trim());
            comp.ts = DateTime.Now;
            comp.modifyuser = UserID;
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("", "  isnull(dr,0)=0 and  compid='" + CompID + "' and utype=4", "");
                if (user2.Count > 0)
                {
                    Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                    if (user != null)
                    {
                        // List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", "  isnull(dr,0)=0 and  compid='" + CompID + "' and type=4", "");
                        // if (user.Count > 0)
                        //{
                        user.ts = DateTime.Now;
                        user.modifyuser = UserID;
                        user.TrueName = txtUserTrueName.Value;
                        new Hi.BLL.SYS_Users().Update(user);
                    }
                    Response.Redirect("CompServiceInfo.aspx");
                }
            }

        }
    }
}