using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using DBUtility;

public partial class Company_SysManager_DisInfo : CompPageBase
{
    public string page = "1";
    public string DisID="";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack) {
            if (Request.Form["action"] == "GetSaleMan")
            {
                SelectDisSM(Request.Form["Value"]);
            } else if (Request.Form["action"] == "insertContract")
            {
                insertContract(Request.Form["FileName"].ToString(), Request.Form["txtForceDate"].ToString(), Request.Form["txtInvalidDate"].ToString());
            }
            if (HttpContext.Current.Session["UserModel"] == null && HttpContext.Current.Session["AdminUser"] == null)
            {
                HttpContext.Current.Response.Redirect("~/login.aspx", true);
            }
            try
            {
                if (Request["KeyID"] != null && Request["KeyID"].ToString() != "")
                    KeyID = Convert.ToInt32(Common.DesDecrypt(Request["KeyID"], Common.EncryptKey));
                else
                    KeyID = 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
            UserDataBind();
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                btitle.InnerText = "我要开通";
                libtnAudit.Visible = false;
                Atitle.InnerText = "新增代理商";
                libtnNext.Style.Add("display", "block;");
            }
            getContract();
        }
        DisID = Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey);
        //DataBindLink();
        //代理商审核权限
        if (!Common.HasRight(this.CompID, this.UserID, "1312"))
            this.libtnAudit.Visible = false;
        //代理商禁用/删除权限
        if (!Common.HasRight(this.CompID, this.UserID, "1313"))
        {
            this.libtnNUse.Visible = false;
            this.libtnUse.Visible = false;
            this.libtnDel.Visible = false;
        }
        if (!Common.HasRight(this.CompID, this.UserID, "1311"))
            this.libtnEdit.Visible = false;
    }

    public void SelectDisSM(string value)
    {
        List<Hi.Model.BD_DisSalesMan> DisM = new Hi.BLL.BD_DisSalesMan().GetList("", " isnull(dr,0)=0 and IsEnabled=1 and Compid=" + CompID + "" + (value == "" ? "" : " and SalesName like '%" + value + "%'") + "", "");
        if (DisM.Count>0)
        {
            string json = "[";
            foreach (Hi.Model.BD_DisSalesMan model in DisM) {
                json += "{\"name\":\"" + model.SalesName + "\",\"id\":" + model.ID + ",\"type\":\"" + Enum.GetName(typeof(Enums.DisSMType), model.SalesType) + "\"},";
            }
            if (json[json.Length - 1] == ',') {
                json = json.Substring(0, json.Length - 1);
            }
            json += "]";
            Response.Write(json);
            Response.End();
        }
        else
        {
            Response.Write("");
            Response.End();
        }
    }

    /// <summary>
    /// 上传合同
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="vDate"></param>
    public void insertContract(string Name,string txtForceDate,string txtInvalidDate)
    {
        string json = "";
        SqlTransaction tran = null;
        if (!string.IsNullOrWhiteSpace(Name))
        {
            try
            {
                tran = SqlHelper.CreateStoreTranSaction();
                DateTime time = DateTime.Now;
                Hi.Model.YZT_Contract contractModel = new Hi.Model.YZT_Contract();
                contractModel.contractNO = "";
                contractModel.contractDate = time;
                contractModel.DisID = KeyID;
                contractModel.CState = 3;//线下合同
                contractModel.ForceDate = Convert.ToDateTime(txtForceDate);
                contractModel.InvalidDate = Convert.ToDateTime(txtInvalidDate);
                contractModel.Remark = "线下合同";
                contractModel.CreateDate = DateTime.Now;
                contractModel.CreateUserID = UserID;
                contractModel.dr = 0;
                contractModel.ts = DateTime.Now;
                contractModel.modifyuser = UserID;
                contractModel.CompID = CompID;
                int Cid = new Hi.BLL.YZT_Contract().Add(contractModel, tran);
                Hi.Model.YZT_Annex annexModel = new Hi.Model.YZT_Annex();
                annexModel.fcID = Convert.ToInt32(Cid);
                annexModel.type = 10;
                annexModel.fileName = Name;
                annexModel.fileAlias = "3";
                annexModel.validDate = Convert.ToDateTime(txtInvalidDate);
                annexModel.CreateDate = time;
                annexModel.dr = 0;
                annexModel.ts = time;
                annexModel.modifyuser = UserID;
                annexModel.CreateUserID = UserID;
                int count = new Hi.BLL.YZT_Annex().Add(annexModel, tran);
                tran.Commit();
                if (count > 0)
                    json = "{\"ret\":\"ok\"}";
                else
                    json = "{\"ret\":\"上传失败\"}";
            }
            catch (Exception e)
            {
                tran.Rollback();
                json = "{\"ret\":\""+ e .Message+ "\"}";
            }
            
        }
        else
             json = "{\"ret\":\"附件不能为空!\"}";
        Response.Write(json);
        Response.End();
    }


    //public void DataBindLink()
    //{
    //    Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
    //    if (Dis != null)
    //    {
    //        if (!string.IsNullOrEmpty(Dis.pic))
    //        {
    //            string[] files = Dis.pic.Split(new char[] { ',' });
    //            foreach (string file in files)
    //            {
    //                if (!string.IsNullOrEmpty(file))
    //                {
    //                    LinkButton linkFile = new LinkButton();
    //                    linkFile.Click += new EventHandler(Download_Click);
    //                    if (file.LastIndexOf("_") != -1)
    //                    {
    //                        string text = file.Substring(0, file.LastIndexOf("_")) + Path.GetExtension(file);
    //                        if (text.Length < 15)
    //                            linkFile.Text = text;
    //                        else
    //                        {
    //                            linkFile.Text = text.Substring(0, 15) + "...";
    //                        }
    //                        linkFile.Attributes.Add("title", text);
    //                    }
    //                    else
    //                    {
    //                        string text = file.Substring(0, file.LastIndexOf("-")) + Path.GetExtension(file);
    //                        if (text.Length < 15)
    //                            linkFile.Text = text;
    //                        else
    //                        {
    //                            linkFile.Text = text.Substring(0, 15) + "...";
    //                        }
    //                        linkFile.Attributes.Add("title", text);
    //                    }
    //                    linkFile.Style.Add("margin-right", "5px");
    //                    linkFile.Style.Add("text-decoration", "underline");
    //                    linkFile.Attributes.Add("fileName", file);
    //                    HtmlGenericControl div = new HtmlGenericControl("div");
    //                    div.Controls.Add(linkFile);
    //                    //HtmlImage img = new HtmlImage();
    //                    //img.Src = "../../images/icon_del.png";
    //                    //img.Attributes.Add("title", "删除附件");
    //                    //img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
    //                    //div.Controls.Add(img);
    //                    DFile.Controls.Add(div);
    //                }
    //            }
    //        }
    //    }
    //}

    public void DataBinds()
    {
        //Common.BindMan(this.rptUnit, this.CompID);//绑定单位下拉
        if (Request["type"] == "0")
        {
            btitle.InnerText = "代理商审核";
            lblbtnback.Attributes["onclick"] = "javascript:window.location.href='DisAuditList.aspx';";
            this.btitle.Attributes.Add("href", "../SysManager/DisAuditList.aspx");
        }
        else if (Request["type"] == "3")
        {
            Atitle.InnerText = "代理商管理员查询";
            lblbtnback.Attributes["onclick"] = "javascript:window.location.href='DisUserList.aspx';";
            this.btitle.Attributes.Add("href", "../SysManager/DisUserList.aspx");
        }
        else if (Request["type"] == "2")
        {
            Atitle.InnerText = "代理商信息维护";
            if (Request["nextstep"] != null && Request["nextstep"] == "1")
            {
                lblbtnback.Attributes["onclick"] = "javascript:window.location.href='DisList.aspx?nextstep=1';";
                this.btitle.Attributes.Add("href", "../SysManager/DisList.aspx?nextstep=1");
            }
            else
            {
                lblbtnback.Attributes["onclick"] = "javascript:window.location.href='DisList.aspx';";
                this.btitle.Attributes.Add("href", "../SysManager/DisList.aspx");
            }
            
        }
        else if (Request["type"] == "4")
        {
            Atitle.InnerText = "代理商信息维护";
            lblbtnback.Attributes["onclick"] = "javascript:window.location.href='DisEdit.aspx?KeyID="+KeyID+"';";
        }
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
        if (Dis != null)
        {
            if (Dis.dr == 1)
            {
                if (Request.UrlReferrer != null)
                {
                    JScript.AlertMethod(this, "代理商不存在！", JScript.IconOption.错误, "function (){ history.go(-1); }");
                    Response.End();
                    return;
                }
                else
                {
                    Response.Write("数据错误。");
                    Response.End();
                }
            }
            LoginModel Lmodel = HttpContext.Current.Session["UserModel"] as LoginModel;
            //暂时屏蔽 edit by hgh  180125
            //if (Dis.CompID != Lmodel.CompID)
            //{
            //    if (Request.UrlReferrer != null)
            //    {
            //        JScript.AlertMethod(this, "代理商不存在！", JScript.IconOption.错误, "function (){ history.go(-1); }");
            //        Response.End();
            //        return;
            //    }
            //    else
            //    {
            //        Response.Write("代理商不存在！");
            //        Response.End();
            //    }
            //}

            if (Request["type"] == "3")
            {
                libtnAudit.Visible = false;
                libtnDel.Visible = false;
                libtnEdit.Visible = false;
                libtnUse.Visible = false;
                libtnNUse.Visible = false;
            }
            if (Request["type"] == "0")
            {
                libtnEdit.Visible = false;
                libtnUse.Visible = false;
                libtnNUse.Visible = false;
            }
            
            lblDisName.Value = Dis.DisName;
            DisCode.Value = Dis.DisCode;
            if (Dis.SMID > 0)
            {
                txtSaleMan.Value = Common.BindManName(Dis.SMID, CompID);
                Hi.Model.BD_DisSalesMan SMmodel = new Hi.BLL.BD_DisSalesMan().GetModel(Dis.SMID);
                if (SMmodel != null)
                {
                    //ddlSMType.SelectedValue = SMmodel.SalesType.ToString();
                    //lblSMName.InnerText = SMmodel.SalesName;
                }
            }
            //lblTyoeName.Value = Common.GetDisTypeNameById(Dis.DisTypeID);
            //lblAreaName.Value = Common.GetDisAreaNameById(Dis.AreaID);
            //lblLeading.InnerText = Dis.Leading;
            //lblLeadingPhone.InnerText = Dis.LeadingPhone;
            //lblDisLevel.Value = Dis.DisLevel;
            lblFax.Value = Dis.Fax;
            //lblTel.InnerText = Dis.Tel;
            //lblLicence.InnerText = Dis.Licence;
            lblPerson.Value = Dis.Principal;
            lblPhone.Value = Dis.Phone;
            lblAddress.Value = Dis.Province + Dis.City + Dis.Area + Dis.Address;
            lblZip.Value = Dis.Zip;
            //lblFinancingRatio.InnerText = Dis.FinancingRatio.ToString("0.00") + " %";
            lblRemark.Value = Dis.Remark;
            //lblIsEnabled.InnerHtml = Dis.IsEnabled == 1 ? "启用" : "<i style='color:red;'>禁用</i>";
            //rdAuditYes.InnerHtml = Dis.IsCheck == 1 ? "是" : "<i style='color:red;'>否</i>";
            //rdCreditYes.InnerHtml = Dis.CreditType == 0 ? "<i style='color:red;'>否</i>" : "是";
            //lblCreditAmount.Value = Dis.CreditAmount.ToString("0.00") == "0.00" ? "没有限额" : Dis.CreditAmount.ToString("0.00");
            //if (Dis.CreditType == 0)
            //{
            //    this.spanCreditAmount.Style.Add("display", "none");
            //    this.lblCreditAmount.Style.Add("display", "none");
            //}
            //this.Integral.InnerHtml = Dis.Integral.ToString("0.00");

        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                Response.Write("代理商不存在！");
                Response.End();
                return;
            }
            else
            {
                Response.Write("代理商不存在！");
                Response.End();
            }
            return;
        }
    }

    public void UserDataBind()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and DisID=" + KeyID + " and CompID="+this.CompID, "");
        if (ListComp.Count > 0)
        {
            lblTyoeName.Value = Common.GetDisTypeNameById(ListComp[0].DisTypeID);
            lblAreaName.Value = Common.GetDisAreaNameById(ListComp[0].AreaID);
            lblIsEnabled.InnerHtml = ListComp[0].IsEnabled == 1 ? "启用" : "<i style='color:red;'>禁用</i>";
            rdAuditYes.InnerHtml = ListComp[0].IsCheck == 1 ? "是" : "<i style='color:red;'>否</i>";
            rdCreditYes.InnerHtml = ListComp[0].CreditType == 0 ? "<i style='color:red;'>否</i>" : "是";
            lblCreditAmount.Value = ListComp[0].CreditAmount.ToString("0.00") == "0.00" ? "没有限额" : ListComp[0].CreditAmount.ToString("0.00");
            if (ListComp[0].CreditType == 0)
            {
                this.spanCreditAmount.Style.Add("display", "none");
                this.lblCreditAmount.Style.Add("display", "none");
            }

            if (ListComp[0].IsAudit == 2)
            {
                if (ListComp[0].IsEnabled == 1)
                {
                    libtnNUse.Visible = true;
                    libtnUse.Visible = false;
                }
                else
                {
                    libtnUse.Visible = true;
                    libtnNUse.Visible = false;
                }
                libtnAudit.Visible = false;
                libtnDel.Visible = false;
            }
            else
            {
                libtnEdit.Visible = false;
                libtnUse.Visible = false;
                libtnNUse.Visible = false;
            }
            //禁用状态，可以删除
            if (ListComp[0].IsEnabled == 0)
            {
                libtnDel.Visible = true;
            }


            string users = string.Join(",", ListComp.Select(t => t.UserID));
            List<Hi.Model.SYS_Users> LDis = new Hi.BLL.SYS_Users().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, " and isnull(dr,0)=0 and id in(" + users + ") ", out pageCount, out Counts);
            this.Rpt_User.DataSource = LDis;
            this.Rpt_User.DataBind();
            Pager.RecordCount = Counts;
            page = Pager.CurrentPageIndex.ToString();
        }
        else
        {
            if (Request.UrlReferrer != null)
            {
                JScript.AlertMethod(this, "用户数据有误", JScript.IconOption.错误, "function (){ history.go(-1); }");
                return;
            }
            else
            {
                Response.Write("用户数据有误！");
                Response.End();
            }
        }
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        UserDataBind();
    }
    protected void btnSMBind_Click(object sender, EventArgs e)
    {
        if (HidSM.Value.ToInt(0) > 0)
        {
            List<Hi.Model.BD_DisSalesMan> models = new Hi.BLL.BD_DisSalesMan().GetList("id", " isnull(dr,0)=0 and IsEnabled=1 and Compid=" + CompID + "  and  ID=" + HidSM.Value.ToInt(0) + " ", "");
            if (models.Count == 0)
            {
                JScript.AlertMethod(this, "绑定的业务员不是合法有效的!", JScript.IconOption.错误, "function (){ location.href=location.href; }");
                return;
            }
            Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
            if (Dis != null)
            {
                Dis.SMID = HidSM.Value.ToInt(0);
                Dis.modifyuser = UserID;
                Dis.ts = DateTime.Now;
                if (new Hi.BLL.BD_Distributor().Update(Dis))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "this", "<script>location.href=location.href;</script>");
                }
            }
        }
    }

    //public void btn_AuditClick(object sender, EventArgs e)
    //{
    //    Hi.Model.BD_Distributor model = new Hi.BLL.BD_Distributor().GetModel(KeyID);
    //    string str = string.Empty;
    //    // if (model.DisTypeID == 0) {
    //    //    str = "代理商分类不能为空。";
    //    //}
    //    //else if (model.AreaID == 0) {
    //    //    str = "代理商区域不能为空。";
    //    //}
    //    if (!string.IsNullOrEmpty(str))
    //    {
    //        JScript.AlertMsgMo(this, str, "function(){ window.location.href=window.location.href; }");
    //        return;
    //    }
    //    model.AuditState = 2;
    //    model.AuditDate = DateTime.Now;
    //    model.AuditUser = UserName;
    //    if (new Hi.BLL.BD_Distributor().Update(model))
    //    {
    //        List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " type=5 and  DisID='" + model.ID + "'", "");
    //        if (user.Count > 0)
    //        {
    //            user[0].AuditState = 2;
    //            user[0].AuditUser = UserName;
    //            new Hi.BLL.SYS_Users().Update(user[0]);
    //            DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
    //            getphonecode.GetUser("jksc068", "jksc06833");
    //            str = getphonecode.ReturnSTRS(user[0].Phone, model.DisName, user[0].UserName);
    //            if (str != "Success")
    //            {
    //                JScript.AlertMsgMo(this, "通知短信发送失败！请自行发送短信通知企业。", "function(){ window.location.href='CompAuditList.aspx'; }");
    //            }
    //            else
    //            {
    //                JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='DisList.aspx';$(window.parent[1].frameElement.contentDocument).find('.jxszlgl').trigger('click'); }");
    //            }
    //        }
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

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
        if (Dis != null)
        {
            //if (Dis.AuditState == 2 && Dis.IsEnabled == 1)
            //{
            //    JScript.AlertMsgOne(this, "已审核未被禁用的代理商不允许删除！", JScript.IconOption.错误);
            //    return;
            //}
            //Dis.dr = 1;
            //Dis.ts = DateTime.Now;
            //Dis.modifyuser = Common.UserID();

            List<int> ListUserid = new List<int>();
            List<int> ListDelUserid = new List<int>();
            List<Hi.Model.SYS_CompUser> luser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and DisID=" + KeyID + " and Utype in (1,5) and Ctype=2 and CompID=" + this.CompID, "");

            foreach (Hi.Model.SYS_CompUser model in luser)
            {
                if (model.IsAudit == 2 && model.IsEnabled == 1)
                {
                    JScript.AlertMsgOne(this, "已审核未被禁用的代理商不允许删除！", JScript.IconOption.错误);
                    return;
                }

                if (!ListUserid.Contains(model.UserID))
                {
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id", " dr=0 and Userid=" + model.UserID + " and CompID=" + this.CompID, "");
                    if (ListCompUser.Count == 1)
                    {
                        ListDelUserid.Add(model.UserID);
                    }
                    ListUserid.Add(model.UserID);
                }
                model.dr = 1;
                model.ts = DateTime.Now;
                model.modifyuser = Common.UserID();
                new Hi.BLL.SYS_CompUser().Update(model);
            }
            if (ListDelUserid.Count > 0)
            {
                List<Hi.Model.SYS_Users> ListUsers = new Hi.BLL.SYS_Users().GetList("", " dr=0 and id in(" + string.Join(",", ListDelUserid) + ")", "");
                foreach (Hi.Model.SYS_Users model in ListUsers)
                {
                    model.dr = 1;
                    model.ts = DateTime.Now;
                    model.modifyuser = Common.UserID();
                    new Hi.BLL.SYS_Users().Update(model);
                }
            }
            //string Phone = Common.GetDis(Dis.ID, "Phone");
            //string msg = "您所注册的代理商：" + Dis.DisName + "已注销！[ " + Common.GetCompValue(CompID, "CompName") + " ]";
            //Common.GetPhone(Phone, msg);
            JScript.AlertMethod(this, "删除成功！", JScript.IconOption.错误, "function(){ window.location.href='DisList.aspx'; }");

        }
    }

    protected void btn_Use(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
        if (Dis != null)
        {

            List<Hi.Model.SYS_CompUser> luser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and DisID=" + KeyID + " and Utype in (1,5) and Ctype=2 and CompID=" + this.CompID, "");

            foreach (Hi.Model.SYS_CompUser model in luser)
            {
                model.IsEnabled = 1;
                model.ts = DateTime.Now;
                model.modifyuser = Common.UserID();
                new Hi.BLL.SYS_CompUser().Update(model);
            }

            if (Request["nextstep"] + "" == "1")
            {
                Response.Redirect("DisList.aspx?nextstep=1");
            }
            else
            {
                Response.Redirect("DisInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=2");
            }

        }
    }


    protected void btn_NUse(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);

        if (Dis != null)
        {
            List<Hi.Model.SYS_CompUser> luser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and DisID=" + KeyID + " and Utype in (1,5) and Ctype=2 and CompID=" + this.CompID, "");
            foreach (Hi.Model.SYS_CompUser model in luser)
            {
                model.IsEnabled = 0;
                model.ts = DateTime.Now;
                model.modifyuser = Common.UserID();
                new Hi.BLL.SYS_CompUser().Update(model);
            }
            if (Request["nextstep"] + "" == "1")
            {
                Response.Redirect("DisList.aspx?nextstep=1");
            }
            else
            {
                Response.Redirect("DisInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=2");
            }

        }
    }


    /// <summary>
    /// 绑定合同列表
    /// </summary>
    public void getContract()
    {
        int Counts = 0;
        int pageCount = 0; 
        List<Hi.Model.YZT_Contract> LDis = new Hi.BLL.YZT_Contract().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", true, " and isnull(dr,0)=0 and CompID="+CompID+ " and DisID="+KeyID+"", out pageCount, out Counts);
        if (LDis.Count > 0)
        {
            if (LDis[0].InvalidDate > DateTime.Now)
                liContract.Visible = false;
        }
        this.ContractRep.DataSource = LDis;
        this.ContractRep.DataBind();
        Page1.RecordCount = Counts;
        page = Page1.CurrentPageIndex.ToString();
    }


}