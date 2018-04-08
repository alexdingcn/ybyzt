
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data;

public partial class Admin_Systems_CompzxInfo : AdminPageBase
{
    public string page = "1";
    public string page1 = "1";
    public string type = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Action = Request["Action"] + "";
            string OrgID = Request["OrgID"] + "";
            if (Action == "Action")
            {
                Response.Write(Common.getsaleman(OrgID,true));
                Response.End();
            }
            string Actiom = Request["Action"] + "";
            string value = Request["value"] + "";
            if (Actiom == "GetPhone")
            {
                Response.Write(ExistsUserPhone(value));
                Response.End();
            }
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request.QueryString["page1"] != null)
            {
                page1 = Request.QueryString["page1"].ToString();
                Pager1.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request["type"] != null)
            {
                type = Request["type"] + "";
            }

            Common.BindOrgSale(Org, SaleMan,"全部");
            DataBinds();
            DisDataBind();
            UserDataBind();
        }
        DataBindLink();
    }

    public string ExistsUserPhone(string Phone)
    {
        if (Common.GetUserExists("Phone", Phone))
        {
            return "{ \"result\":true}";
        }
        else
        {
            return "{ \"result\":false}"; ;
        }
    }

    public void DataBindLink()
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
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
                                linkFile.Text = text.Substring(0, 15)+"...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        else {
                            string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        linkFile.Style.Add("margin-right","5px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", file);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Controls.Add(linkFile);
                        //HtmlImage img = new HtmlImage();
                        //img.Src = "../../images/icon_del.png";
                        //img.Attributes.Add("title", "删除附件");
                        //img.Attributes.Add("onclick", "AnnexDel(this,'Comp'," + KeyID + ",'" + file + "')");
                        //div.Controls.Add(img);
                        DFile.Controls.Add(div);
                    }
                }
            }
        }
    }

    public void DataBinds()
    {
        if (Request["type"] == "1")
        {
            Atitle.InnerText = "厂商装修";
            lblbtnback.Attributes["onclick"] = "javascript:window.location.href='CompzxList.aspx?type=" + type + "&page=" + Request["page"] + "';";
        }
        else if (Request["type"] == "2") {
            Atitle.InnerText = "厂商装修审核";
            lblbtnback.Attributes["onclick"] = "javascript:window.location.href='CompzxList.aspx?type=" + type + "&page=" + Request["page"] + "';"; ;
        }

         if (Request["type"] == "4")
        {
            Btitle.InnerText = Common.NoHTML(Request["atitle"].ToString());
            Atitle.InnerText = Common.NoHTML(Request["btitle"].ToString());
            lblbtnback.Attributes["onclick"] = "javascript:history.go(-1);";
        }

        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            if (comp.IsEnabled == 0)
            {
               
            }
            if (comp.AuditState == 2)
            {
                //Common.BindRoleDDL(ddlRoleId, KeyID.ToString());
                //libtnUserAdd.Visible = true;
                
                lizx.Visible = true;
                
                //if (comp.FirstShow == 1)
                //{
                    Erptype.ColSpan = 0;
                    Sort.Visible = true;
                    tSort.Visible = true;
                //}
                
            }
            else
            {
                //liOrgBind.Visible = false;
                lizx.Visible = false;
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                
                //lizxiu.Visible = false;
               
                
                Erptype.ColSpan = 3;
                Sort.Visible = false;
                tSort.Visible = false;
              
            }
            if (Request["type"] == "3")
            {
                //lizxiu.Visible = false;
               
                Erptype.ColSpan = 3;
                Sort.Visible = false;
                tSort.Visible = false;
            }
            if (Request["type"] == "4")
            {
                //lizxiu.Visible = false;
                //add.Visible = false;
                
                Erptype.ColSpan = 3;
                Sort.Visible = false;
                tSort.Visible = false;
            }
            if (Request["type"] == "1")
            {
                //lizxiu.Visible = false;
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
               
                Erptype.ColSpan = 3;
                Sort.Visible = false;
                tSort.Visible = false;
            }
            if (comp.OrgID > 0) {
                Org.SelectedValue = comp.OrgID.ToString();
            }
            if (comp.SalesManID > 0) {
                salemanid.Value = comp.SalesManID.ToString();
                //add by hgh  0228 不允许修改
                if (this.UserName != "admin")
                {
                    this.SaleMan.Enabled = false;
                }
            }
            lblOrg.InnerText = Common.GetOrgValue(comp.OrgID, "OrgName") == null ? "" : Common.GetOrgValue(comp.OrgID, "OrgName").ToString();
            lblSaleMan.InnerText = Common.GetSaleManValue(comp.SalesManID, "SalesName") == null ? "" : Common.GetSaleManValue(comp.SalesManID, "SalesName").ToString();
            lblShotName.InnerText = comp.ShortName;
            lblCompName.InnerText = comp.CompName;
            lblCapital.InnerHtml = comp.Capital+"万";
            lblCompType.InnerHtml =comp.CompType == 1 ? "个人" : "股份";
            lblIndusName.Id = comp.IndID.ToString();
            lblCompCode.InnerText = comp.CompCode;
            lblTel.InnerText = comp.Tel;
            lblLegal.InnerText = comp.Legal;
            lblLegalTel.InnerText = comp.LegalTel;
            lblIsHot.InnerHtml = comp.HotShow == 1 ? "是" : "<i style='color:red;'>否</i>";
            lblIsFirst.InnerHtml = comp.FirstShow == 1 ? "首页显示" : comp.FirstShow == 2 ? "搜索页显示" : "<i style='color:red;'>否</i>";
            txtSort.Value = comp.SortIndex;
            lblIsEbled.InnerHtml = comp.IsEnabled == 1 ? "启用" : "<i style='color:red;'>禁用</i>";
            lblPhone.InnerText = comp.Phone;
            lblIdentitys.InnerText = comp.Identitys;
            lblLicence.InnerText = comp.Licence;
            lblRemark.InnerText = comp.Remark;
            lblOrCode.InnerText = comp.OrganizationCode;
            lblErptype.InnerText = Enum.GetName(typeof(Enums.Erptype), comp.Erptype);
            //if (!string.IsNullOrEmpty(comp.Attachment))
            //{
            //    linkFile.Text = comp.Attachment.Substring(0, comp.Attachment.LastIndexOf("-")) + Path.GetExtension(comp.Attachment);
            //    linkFile.Attributes.Add("fileName", comp.Attachment);
            //}
            //else
            //{
            //    linkFile.Visible = false;
            //}
            lblZip.InnerText = comp.Zip;
            lblInfo.InnerText = comp.ManageInfo;
            lblFax.InnerText = comp.Fax;
            lblPrincipal.InnerText = comp.Principal;
            lblAccount.InnerText = comp.Account;
            lblAddress.InnerText = comp.Address;
            lblCompAddr.InnerText = comp.CompAddr;
            string IsZXAuditText = string.Empty;

            //装修确认
            if (comp.Isorgzx == 1)
            {
                if (comp.IsZXAudit == 0 &&"1".Equals(type))
                {
                    this.lizx.Visible = false;
                    this.liqxzx.Visible = true;
                }
                else
                {
                    this.lizx.Visible = false;
                    this.liqxzx.Visible = false;
                }
                this.lblzx.InnerHtml = "是";
            }
            else
            {
                if (comp.IsZXAudit == 0 && "1".Equals(type))
                {
                    this.lizx.Visible = true;
                    this.liqxzx.Visible = false;
                }
                else
                {
                    this.lizx.Visible = false;
                    this.liqxzx.Visible = false;
                }
                this.lblzx.InnerHtml = "<i style='color:red;'>否</i>";
            }

            //装修审核
            if (comp.IsZXAudit == 1)
            {
                if (comp.Isorgzx == 1 && "2".Equals(type))
                {
                    this.liqxzxaudit.Visible = true;
                    this.lizxaudit.Visible = false;
                }
                else
                {
                    this.liqxzxaudit.Visible = false;
                    this.lizxaudit.Visible = false;
                }
                this.lblzxaudit.InnerHtml = "是 (" + comp.ZXAuditDate + ")";
            }
            else
            {
                if (comp.Isorgzx == 1 && "2".Equals(type))
                {
                    this.liqxzxaudit.Visible = false;
                    this.lizxaudit.Visible = true;
                }
                else
                {
                    this.liqxzxaudit.Visible = false;
                    this.lizxaudit.Visible = false;
                }
                this.lblzxaudit.InnerHtml = "<i style='color:red;'>否</i>";
            }

            lblAudit.InnerHtml = comp.AuditState == 2 ? "是" : "<i style='color:red;'>否</i>";

            //绑定服务日期
            //List<Hi.Model.Pay_Service> serviceord = new Hi.BLL.Pay_Service().GetList("*", " compid=" + KeyID + " and isaudit=1 and outofdata=0 ", " createdate desc");
            //if (serviceord.Count > 0)
            //{
            EnabledStartDate.InnerHtml = comp.EnabledStartDate.ToString("yyyy-MM-dd")== "0001-01-01"?"": comp.EnabledStartDate.ToString("yyyy-MM-dd");//存在有效服务
                EnabledEndDate.InnerHtml = comp.EnabledEndDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : comp.EnabledEndDate.ToString("yyyy-MM-dd");
            //}

              this.lblOrgzxdate.InnerHtml = comp.Orgzxdate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : comp.Orgzxdate.ToString("yyyy-MM-dd");
              this.lblZXAuditDate.InnerHtml = comp.ZXAuditDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : comp.ZXAuditDate.ToString("yyyy-MM-dd");


            if (ConfigurationManager.AppSettings["IsAdminUser"] != "1")
            {
                TRORG.Visible = false;
                
            };
            if (UserType == 3 || UserType == 4) {
                if (comp.OrgID != OrgID) {
                    Response.Write("数据不存在");
                    Response.End();
                }
            }
        }
        else
        {
            Response.Write("数据不存在");
            Response.End();
        }
    }

    //保存首页显示排序
    //protected void btnSaveSort_click(object sender, EventArgs e)
    //{
    //    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
    //    comp.SortIndex = Common.NoHTML(txtSort.Value.Trim());
    //    if (new Hi.BLL.BD_Company().Update(comp))
    //    {
    //        JScript.AlertMsgMo(this, "保存成功！", "function(){}");
    //    }
    //}

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
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            JScript.AlertMsgMo(this, "附件不存在", "function(){ location.href=location.href;}");
        }
    }

    public void DisDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, " and isnull(dr,0)=0 and CompID=" + KeyID + "", out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = LDis;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void UserDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = " SYS_CompUser inner join Sys_Users Users on  SYS_CompUser.userid=Users.id and isnull(SYS_CompUser.dr,0)=0 and isnull(Users.dr,0)=0 ";
        DataTable LUser = new Hi.BLL.SYS_CompUser().GetList(Pager.PageSize, Pager.CurrentPageIndex, "SYS_CompUser.createdate", false, " SYS_CompUser.id,UserName,SYS_CompUser.createdate,TrueName,Phone,Identitys,Email,SYS_CompUser.IsEnabled,utype Type,isaudit ", JoinTableStr, " and utype in(3,4) and ctype=1  and SYS_CompUser.compid=" + KeyID + " ", out pageCount, out Counts);
        this.Rpt_User.DataSource = LUser;
        this.Rpt_User.DataBind();
        Pager1.RecordCount = Counts;
        page1 = Pager1.CurrentPageIndex.ToString();
    }



    /// <summary>
    /// 业务员装修确认
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_zx(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            //comp.IsZXAudit = 1;
            //comp.ZXAuditDate = DateTime.Now;
            comp.Isorgzx = 1;
            comp.Orgzxdate = DateTime.Now;
            comp.ts = DateTime.Now;
            comp.modifyuser = Common.UserID();
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                JScript.AlertMsgMo(this, "确认成功", "function(){ window.location.href=window.location.href; }");
            }
            else
            {
                JScript.AlertMsgMo(this, "确认失败", "function(){ window.location.href=window.location.href; }");
            }
        }
        else {
            JScript.AlertMsgMo(this, "厂商不存在", "function(){ window.location.href=window.location.href; }");
        }
    }
    /// <summary>
    /// 业务员取消装修确认
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_qxzx(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            //comp.IsZXAudit = 1;
            //comp.ZXAuditDate = DateTime.Now;

            //业务员已确认装修且管理员装修未审核
            if (comp.Isorgzx == 1 && comp.IsZXAudit == 0)
            {
                comp.Isorgzx = 2;
                comp.Orgzxdate = DateTime.Now;
                comp.ts = DateTime.Now;
                comp.modifyuser = Common.UserID();
                if (new Hi.BLL.BD_Company().Update(comp))
                {
                    JScript.AlertMsgMo(this, "取消装修成功", "function(){ window.location.href=window.location.href; }");
                }
                else
                {
                    JScript.AlertMsgMo(this, "取消装修失败", "function(){ window.location.href=window.location.href; }");
                }
            }
            else
            {
                JScript.AlertMsgMo(this, "厂商未装修，取消装修失败", "function(){ window.location.href=window.location.href; }");
            }
        }
        else
        {
            JScript.AlertMsgMo(this, "厂商不存在", "function(){ window.location.href=window.location.href; }");
        }
    }

     /// <summary>
    /// 装修审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_zxaudit(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            comp.IsZXAudit = 1;
            comp.ZXAuditDate = DateTime.Now;
            comp.Zxaudituser = this.UserID;
            comp.ts = DateTime.Now;
            comp.modifyuser = Common.UserID();
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                JScript.AlertMsgMo(this, "装修审核成功", "function(){ window.location.href=window.location.href; }");
            }
            else
            {
                JScript.AlertMsgMo(this, "装修审核失败", "function(){ window.location.href=window.location.href; }");
            }
        }
        else
        {
            JScript.AlertMsgMo(this, "厂商不存在", "function(){ window.location.href=window.location.href; }");
        }
    }

     /// <summary>
    /// 装修取消审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_qxzxaudit(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            comp.IsZXAudit = 0;
            comp.ZXAuditDate = DateTime.Now;
            comp.Zxaudituser = this.UserID;
            comp.ts = DateTime.Now;
            comp.modifyuser = Common.UserID();
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                JScript.AlertMsgMo(this, "装修取消审核成功", "function(){ window.location.href=window.location.href; }");
            }
            else
            {
                JScript.AlertMsgMo(this, "装修取消审核失败", "function(){ window.location.href=window.location.href; }");
            }
        }
        else
        {
            JScript.AlertMsgMo(this, "厂商不存在", "function(){ window.location.href=window.location.href; }");
        }
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        DisDataBind();
    }


    protected void Pager_PageChanged1(object sender, EventArgs e)
    {
        page1 = Pager1.CurrentPageIndex.ToString();
        UserDataBind();
    }

    protected void btn_Use(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        comp.IsEnabled = 1;
        comp.ts = DateTime.Now;
        comp.modifyuser = UserID;
        if (new Hi.BLL.BD_Company().Update(comp))
        {
            //List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and compid=" + KeyID + "", "");
            //foreach (Hi.Model.SYS_Users model1 in user)
            //{
            //    model1.IsEnabled = 1;
            //    model1.ts = DateTime.Now;
            //    model1.modifyuser = UserID;
            //    new Hi.BLL.SYS_Users().Update(model1);
            //}
            List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList("", " isnull(dr,0)=0 and compid=" + KeyID + "", "");
            foreach (Hi.Model.BD_Distributor model2 in dis)
            {
                model2.IsEnabled = 1;
                model2.ts = DateTime.Now;
                model2.modifyuser = UserID;
                new Hi.BLL.BD_Distributor().Update(model2);
            }
            JScript.AlertMsgMo(this, "用户启用成功", "function(){ window.location.href='CompList.aspx'; }");
        }
    }
    protected void btnBind_Click(object sender, EventArgs e)
    {
        try
        {
            if (Org.SelectedValue != "-1" || salemanid.Value == "" || salemanid.Value == "-1")
            {
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
                if (comp != null)
                {
                    comp.OrgID = int.Parse(Org.SelectedValue);
                    if (UserType == 3 || UserType == 4)
                    {
                        comp.OrgID = OrgID;
                    }
                    comp.SalesManID = int.Parse(salemanid.Value);
                    new Hi.BLL.BD_Company().Update(comp);
                    ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>window.location.href='CompInfo.aspx?KeyID="+KeyID+"';</script>");
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>alert('请选择业务员或机构');</script>");
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>alert('绑定失败');</script>");
        }
    }

    protected void btn_NUse(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        comp.IsEnabled = 0;
        comp.ts = DateTime.Now;
        comp.modifyuser = UserID;
        if (new Hi.BLL.BD_Company().Update(comp))
        {
            //List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and compid=" + KeyID + "", "");
            //foreach (Hi.Model.SYS_Users model1 in user)
            //{
            //    model1.IsEnabled = 0;
            //    model1.ts = DateTime.Now;
            //    model1.modifyuser = UserID;
            //    new Hi.BLL.SYS_Users().Update(model1);
            //}
            List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList("", " isnull(dr,0)=0 and compid=" + KeyID + "", "");
            foreach (Hi.Model.BD_Distributor model2 in dis)
            {
                model2.IsEnabled = 0;
                model2.ts = DateTime.Now;
                model2.modifyuser = UserID;
                new Hi.BLL.BD_Distributor().Update(model2);
            }
            JScript.AlertMsgMo(this, "用户禁用成功", "function(){ window.location.href='CompList.aspx'; }");
        }
    }

}