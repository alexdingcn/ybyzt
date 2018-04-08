using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

public partial class Admin_Systems_DisInfo : AdminPageBase
{
    public string page = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request["type"] == "0")
            {
                atitle.InnerText = "代理商管理员查询";
            }
            if (Request["type"] == "3")
            {
                divTitle.Style.Add("display", "none;");
                divTitle.Visible = false;
                lblbtnback.Visible = false;
            }
            DataBinds();
            UserDataBind();
        }
        DataBindLink();
    }
    public void DataBindLink()
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
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
                            linkFile.Text = text;
                        }
                        else
                        {
                            string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
                            linkFile.Text = text;
                        }
                        linkFile.Style.Add("margin-right", "15px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", file);
                        DFile.Controls.Add(linkFile);
                    }
                }
            }
        }
    }
    public void DataBinds()
    {
        if (KeyID!=0)
        {
            Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
            if (Dis != null)
            {
                lblDisName.InnerText = Dis.DisName;
                //lblTyoeName.InnerText = Common.GetDisTypeNameById(Dis.DisTypeID);
                //lblAreaName.InnerText = Common.GetDisAreaNameById(Dis.AreaID);
                //lblDisLevel.InnerText = Dis.DisLevel;
                lblLeading.InnerText = Dis.Leading;
                lblLeadingPhone.InnerText = Dis.LeadingPhone;
                lblFax.InnerText = Dis.Fax;
                lblTel.InnerText = Dis.Tel;
                lblLicence.InnerText = Dis.Licence;
                lblPerson.InnerText = Dis.Principal;
                lblPhone.InnerText = Dis.Phone;
                lblAddress.InnerText = Dis.Address;
                lblZip.InnerText = Dis.Zip;
                lblRemark.InnerText = Dis.Remark;
                lblIsEnabled.InnerHtml = Dis.IsEnabled == 1 ? "启用" : "<i style='color:red;'>禁用</i>";

                if (Dis.IsEnabled == 1)
                {
                    libtnNUse.Visible = true;
                    libtnUse.Visible = false;
                }
                else
                {
                    libtnUse.Visible = true;
                    libtnNUse.Visible = false;
                }

                //rdAuditYes.InnerHtml = Dis.IsCheck == 1 ? "是" : "<i style='color:red;'>否</i>";
                //rdCreditYes.InnerHtml = Dis.CreditType == 0 ? "<i style='color:red;'>否</i>" : "是";
            }
            else
            {
                JScript.AlertMsg(this, "代理商不存在！");
                return;
            }
        }
        else
        {
            JScript.AlertMsg(this, "数据错误！");
            return;
        }
    }
    public void UserDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, " SYS_CompUser.id,UserName,SYS_CompUser.CompID,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,utype Type,isaudit ", JoinTableStr, " and utype in(1,5) and ctype=2  and SYS_CompUser.Disid=" + KeyID + " ", out pageCount, out Counts);
        if (LUser.Rows.Count > 0)
        {
            this.Rpt_User.DataSource = LUser;
            this.Rpt_User.DataBind();
            Pager.RecordCount = Counts;
            page = Pager.CurrentPageIndex.ToString();
        }
        else
        {
            List<Hi.Model.SYS_Users> LUser2 = new Hi.BLL.SYS_Users().GetList(" UserName,TrueName,auditstate,5 Type,Phone ", " isnull(dr,0)=0 and disid=" + KeyID + "", "");
            this.Rpt_User.DataSource = LUser2;
            this.Rpt_User.DataBind();
            Pager.RecordCount = Counts;
            page = Pager.CurrentPageIndex.ToString();
        }
    }
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        UserDataBind();
    }

    public void Download_Click(object sender, EventArgs e)
    {
        LinkButton bt = sender as LinkButton;
        string fileName = bt.Attributes["fileName"];
        string filePath = Server.MapPath("../../UploadFile/") + Common.NoHTML(fileName);
        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            JScript.AlertMsg(this, "附件不存在");
        }
    }

    protected void btn_Use(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
        string sql = "select * from SYS_CompUser where ISNULL(dr,0)=0 and DisID=" + KeyID;
        DataTable userDT = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (userDT.Rows.Count > 0)
        {
            SqlTransaction Tran = null;
            try
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                for (int i = 0; i < userDT.Rows.Count; i++)
                {
                    Hi.Model.SYS_CompUser user = new Hi.BLL.SYS_CompUser().GetModel(Convert.ToInt32(userDT.Rows[i]["ID"].ToString()));
                    user.IsEnabled = 1;
                    user.ts = DateTime.Now;
                    user.modifyuser = UserID;
                    new Hi.BLL.SYS_CompUser().Update(user, Tran);
                }
                Tran.Commit();
            }
            catch
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                    JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
                }
            }
        }
        if (Dis != null)
        {
            Dis.IsEnabled = 1;
            Dis.ts = DateTime.Now;
            Dis.modifyuser = UserID;
            if (new Hi.BLL.BD_Distributor().Update(Dis))
            {
                if (Request["nextstep"] + "" == "1")
                {
                    Response.Redirect("DisList.aspx?nextstep=1");
                }
                else
                {
                    Response.Redirect("DisInfo.aspx?KeyID=" + KeyID.ToString() + "&type=2");
                }
            }
        }
    }


    protected void btn_NUse(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
        string sql = "select * from SYS_CompUser where ISNULL(dr,0)=0 and DisID=" + KeyID;
        DataTable userDT = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (userDT.Rows.Count > 0)
        {
            SqlTransaction Tran = null;
            try
            {
                Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                for (int i = 0; i < userDT.Rows.Count; i++)
                {
                    Hi.Model.SYS_CompUser user = new Hi.BLL.SYS_CompUser().GetModel(Convert.ToInt32(userDT.Rows[i]["ID"].ToString()));
                    user.IsEnabled = 0;
                    user.ts = DateTime.Now;
                    user.modifyuser = UserID;
                    new Hi.BLL.SYS_CompUser().Update(user, Tran);
                }
                Tran.Commit();
            }
            catch
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                    JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
                }
            }
        }
        if (Dis != null)
        {
            Dis.IsEnabled = 0;
            Dis.ts = DateTime.Now;
            Dis.modifyuser = UserID;
            if (new Hi.BLL.BD_Distributor().Update(Dis))
            {
                if (Request["nextstep"] + "" == "1")
                {
                    Response.Redirect("DisList.aspx?nextstep=1");
                }
                else
                {
                    Response.Redirect("DisInfo.aspx?KeyID=" + KeyID.ToString() + "&type=2");
                }
            }
        }
    }

}