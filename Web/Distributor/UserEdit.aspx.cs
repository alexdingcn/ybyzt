

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

public partial class Distributor_UserEdit : DisPageBase
{
    //public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            Databind();
        }
        DataBindLink();
    }

    public void DataBindLink()
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (Dis != null)
        {
            if (!string.IsNullOrEmpty(Dis.pic))
            {
                string[] files = Dis.pic.Split(new char[] { ',' });
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
                        //img.Src = "../images/icon_del.png";
                        //img.Attributes.Add("title", "删除附件");
                        //img.Attributes.Add("onclick", "ConFirmDelteAnnex(this,'Dis'," + this.DisID + ",'" + file + "')");
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
        string filePath = Server.MapPath("../UploadFile/") + fileName;
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
            JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误,2500);
        }
    }

    public void Databind()
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (Dis != null)
        {
            hidProvince.Value = Dis.Province;
            hidCity.Value = Dis.City;
            hidArea.Value = Dis.Area;
            txtDisName.Value = Dis.DisName;
            txtLicence.Value = Dis.Licence;
            txtTel.Value = Dis.Tel;
            txtname.Value = Dis.Principal;
            txtnamephone.Value = Dis.Phone;
            txtZip.Value = Dis.Zip;
            txtFax.Value = Dis.Fax;
            txtAddress.Value = Dis.Address;
            txtLeading.Value = Dis.Leading;
            txtLeadingPhone.Value = Dis.LeadingPhone;
             List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("", "  isnull(dr,0)=0 and  disId='" + this.DisID + "' and utype=5", "");
            if (user2.Count > 0)
            {
                Hi.Model.SYS_Users u = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                if (u != null)
                {
                    //List<Hi.Model.SYS_Users> u = new Hi.BLL.SYS_Users().GetList("", "  isnull(dr,0)=0 and  DisID='" + this.DisID + "' and type=5", "");
                    //if (u.Count > 0)
                    //{
                    txtUsername.Disabled = true;
                    txtUserPhone.Disabled = true;
                    txtUsername.Value = u.UserName;
                    txtUserPhone.Value = u.Phone;
                    txtUserTrueName.Value = u.TrueName;
                }
            }
            else {
               List< Hi.Model.SYS_Users> users = new Hi.BLL.SYS_Users().GetList("", " DisID='"+ this.DisID + "'", "");

                if (users.Count >0)
                {
                    //List<Hi.Model.SYS_Users> u = new Hi.BLL.SYS_Users().GetList("", "  isnull(dr,0)=0 and  DisID='" + this.DisID + "' and type=5", "");
                    //if (u.Count > 0)
                    //{
                    txtUsername.Disabled = true;
                    txtUserPhone.Disabled = true;
                    txtUsername.Value = users[0].UserName;
                    txtUserPhone.Value = users[0].Phone;
                    txtUserTrueName.Value = users[0].TrueName;
                }

            }


        }
    }

    protected void btn_Save(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (Dis != null)
        {
            if (Common.DisExistsAttribute("DisName", txtDisName.Value.Trim(), this.CompID.ToString(), this.DisID.ToString()))
            {
                JScript.AlertMsgOne(this, "代理商名称已存在！", JScript.IconOption.错误, 2500);
                return;
            }
            Dis.Province =Common.NoHTML( hidProvince.Value.Trim());
            Dis.City = Common.NoHTML(hidCity.Value.Trim());
            Dis.Area = Common.NoHTML(hidArea.Value.Trim());
            Dis.DisName = Common.NoHTML(txtDisName.Value.Trim());
            Dis.Licence = Common.NoHTML(txtLicence.Value.Trim());
            Dis.Tel = Common.NoHTML(txtTel.Value.Trim());
            Dis.Principal = Common.NoHTML(txtname.Value);
            Dis.Phone = Common.NoHTML(txtnamephone.Value);
            Dis.Zip = Common.NoHTML(txtZip.Value.Trim());
            Dis.Fax = Common.NoHTML(txtFax.Value.Trim());
            Dis.Address = Common.NoHTML(txtAddress.Value.Trim());
            Dis.Leading = Common.NoHTML(txtLeading.Value.Trim());
            Dis.LeadingPhone = Common.NoHTML(txtLeadingPhone.Value.Trim());
            if (!string.IsNullOrEmpty(HidFfileName.Value))
            {
                if (Dis.pic == "")
                {
                    Dis.pic = Common.NoHTML(HidFfileName.Value);
                }
                else
                {
                    Dis.pic += "," + Common.NoHTML(HidFfileName.Value);
                }
            }
            Dis.ts = DateTime.Now;
            Dis.modifyuser = this.UserID;
            if (new Hi.BLL.BD_Distributor().Update(Dis))
            {
                List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("UserID", "  isnull(dr,0)=0 and  disId='" + this.DisID + "' and utype=5", "");
                if (user2.Count > 0)
                {
                    Hi.Model.SYS_Users u = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                    if (u != null)
                    {
                        u.ts = DateTime.Now;
                        u.modifyuser = this.UserID;
                        u.TrueName = Common.NoHTML(txtUserTrueName.Value);
                        new Hi.BLL.SYS_Users().Update(u);
                    }
                }
                else
                {
                    Hi.Model.SYS_Users u = new Hi.BLL.SYS_Users().GetModel(this.UserID);
                    if (u != null)
                    {
                        u.ts = DateTime.Now;
                        u.modifyuser = this.UserID;
                        u.TrueName = Common.NoHTML(txtUserTrueName.Value);
                        new Hi.BLL.SYS_Users().Update(u);
                    }
                }
                List<Hi.Model.BD_DisAddr> LDaddr = new Hi.BLL.BD_DisAddr().GetList(" top 1 *", " dr=0 and  disid=" + this.DisID + "", " createdate asc");
                if (LDaddr.Count > 0)
                {
                    LDaddr[0].Address = Common.NoHTML(txtAddress.Value.Trim());
                    LDaddr[0].Province = Common.NoHTML(hidProvince.Value.Trim());
                    LDaddr[0].City = Common.NoHTML(hidCity.Value.Trim());
                    LDaddr[0].Area = Common.NoHTML(hidArea.Value.Trim());
                    LDaddr[0].modifyuser = this.UserID;
                    LDaddr[0].ts = DateTime.Now;
                    new Hi.BLL.BD_DisAddr().Update(LDaddr[0]);
                }
                Response.Redirect("UserIndex.aspx");
            }

        }
    }

    protected void btn_Save1(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (Dis != null)
        {
            if (Common.DisExistsAttribute("DisName", txtDisName.Value.Trim(), this.CompID.ToString(), this.DisID.ToString()))
            {
                JScript.AlertMsgOne(this, "代理商已存在！", JScript.IconOption.错误, 2500);
                return;
            }
            Dis.Province = Common.NoHTML(hidProvince.Value.Trim());
            Dis.City = Common.NoHTML(hidCity.Value.Trim());
            Dis.Area = Common.NoHTML(hidArea.Value.Trim());
            Dis.DisName = Common.NoHTML(txtDisName.Value.Trim());
            Dis.Licence = Common.NoHTML(txtLicence.Value.Trim());
            Dis.Tel = Common.NoHTML(txtTel.Value.Trim());
            Dis.Principal = Common.NoHTML(txtname.Value);
            Dis.Phone = Common.NoHTML(txtnamephone.Value);
            Dis.Zip = Common.NoHTML(txtZip.Value.Trim());
            Dis.Fax = Common.NoHTML(txtFax.Value.Trim());
            Dis.Address = Common.NoHTML(txtAddress.Value.Trim());
            Dis.Leading = Common.NoHTML(txtLeading.Value.Trim());
            Dis.LeadingPhone = Common.NoHTML(txtLeadingPhone.Value.Trim());
            if (!string.IsNullOrEmpty(HidFfileName.Value))
            {
                if (Dis.pic == "")
                {
                    Dis.pic = Common.NoHTML(HidFfileName.Value);
                }
                else
                {
                    Dis.pic += "," + Common.NoHTML(HidFfileName.Value);
                }
            }
            Dis.ts = DateTime.Now;
            Dis.modifyuser = this.UserID;
            if (new Hi.BLL.BD_Distributor().Update(Dis))
            {
                List<Hi.Model.SYS_CompUser> user2 = new Hi.BLL.SYS_CompUser().GetList("UserID", "  isnull(dr,0)=0 and  disId='" + this.DisID + "' and utype=5", "");
                if (user2.Count > 0)
                {
                    Hi.Model.SYS_Users u = new Hi.BLL.SYS_Users().GetModel(user2[0].UserID);

                    if (u != null)
                    {
                        u.ts = DateTime.Now;
                        u.modifyuser = this.UserID;
                        u.TrueName = Common.NoHTML(txtUserTrueName.Value);
                        new Hi.BLL.SYS_Users().Update(u);
                    }
                }
                Response.Redirect("PhoneEdit.aspx?type=hf");
            }

        }
    }
}