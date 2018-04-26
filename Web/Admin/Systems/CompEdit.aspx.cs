
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
using System.Text.RegularExpressions;
using DBUtility;
using System.Data;

public partial class Admin_Systems_CompEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Action = Request["Action"] + "";
            string Name = Request["Value"] + "";
            if (Action == "GetPhone")
            {
                Response.Write(ExistsUserPhone(Name));
                Response.End();
            }

            Common.BindIndDDL(txtIndusName);
            DataBinds();
        }
        DataBindLink();
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
                        img.Attributes.Add("onclick", "ConFirmDelteAnnex(this,'Comp'," + KeyID + ",'" + file + "')");
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
    public void DataBinds()
    {
        if (Request["show"] == "0")
        {
            divshow.Visible = false;
        }
        if (KeyID > 0)
        {
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
            if (comp != null)
            {
                Common.BindErptype(ddlErptype, comp.Erptype);
                if (comp.AuditState == 0)
                {
                    if (Request["S"] == "1")
                    {
                        if (Request["type"] == "2")
                        {
                            Atitle.InnerText = "企业查询";
                        }
                        else
                        {
                            Atitle.InnerText = "企业审核";
                        }
                    }
                    else
                    {
                        if (Request["type"] == "2")
                        {
                            btnAdd.Text = "确定并审核通过";
                            Atitle.InnerText = "企业查询";
                        }
                        else
                        {
                            btnAdd.Text = "确定并审核通过";
                            Atitle.InnerText = "企业审核";
                        }
                    }
                }
                else
                {
                    if (Request["S"] == "1")
                    {
                        Atitle.InnerText = "企业查询";
                    }
                }
                //edit by hgh
                //if (comp.AuditState == 2)
                //{
                //    Response.Write("企业已通过审核不允许编辑");
                //    Response.End();
                //}
                txtPhone.Attributes.Remove("onblur");
                UpwsTitle.Visible = false;
                UpwTitle.Visible = false;
                txtLegalTel.Value = comp.LegalTel;
                txtOrcode.Value = comp.OrganizationCode;
                txtCompName.Value = comp.CompName;
                Capital.Value = comp.Capital;
                CompType.SelectedValue = comp.CompType.ToString();

                lblCompCode.InnerText = comp.CompCode;
                txtTel.Value = comp.Tel;
                txtIdentitys.Value = comp.Identitys;
                txtPrincipal.Value = comp.Principal;
                txtLegal.Value = comp.Legal;
                txtPhone.Value = comp.Phone;
                txtZip.Value = comp.Zip;
                txtInfo.Value = comp.ManageInfo;
                txtLicence.Value = comp.creditCode;
                txtFax.Value = comp.Fax;
                txtAccount.Value = comp.Account;
                txtAddress.Value = comp.Address;
                txtIndusName.SelectedValue = comp.IndID.ToString();
                txtRemark.Value = comp.Remark;
                if (ddlErptype.Items.FindByValue(comp.Erptype.ToString()) != null)
                {
                    ddlErptype.SelectedValue = comp.Erptype.ToString();
                }
                if (!string.IsNullOrWhiteSpace(comp.CompAddr))
                {
                    string[] CompAddr = comp.CompAddr.Split('-');
                    if (CompAddr.Length >0)
                    {
                        this.hidProvince.Value = CompAddr[0].ToString();
                        this.ddlProvince.Value = CompAddr[0].ToString();
                        if (CompAddr.Length > 1)
                        {
                            this.ddlCity.Value = CompAddr[1].ToString();
                            this.hidCity.Value = CompAddr[1].ToString();
                            if (CompAddr.Length > 2)
                            {
                                this.ddlArea.Value = CompAddr[2].ToString();
                                this.hidArea.Value = CompAddr[2].ToString();
                            }
                        }
                    }
                }
                if (comp.HotShow == 0)
                {
                    rdHotShowNo.Checked = true;
                    rdHotShowYes.Checked = false;
                }
                txtShotName.Value = comp.ShortName;
                if (comp.CompName.Length <= 12 && comp.ShortName == "")
                {
                    txtShotName.Value = comp.CompName;
                }
                if (comp.IsEnabled == 0)
                {
                    rdEbleYes.Checked = false;
                    rdEbleNo.Checked = true;
                }
                //if (comp.FirstShow == 0)
                //{
                //    rdFirstShowNo.Checked = true;
                //    rdFirstShowYes.Checked = false;
                //}
                this.ddlChkShow.SelectedValue = comp.FirstShow.ToString();
                List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("UserID", " dr=0 and Ctype=1 and Utype=4 and CompID=" + KeyID + "", "");
                if (ListComp.Count > 0)
                {
                    List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("UserName,Phone,TrueName,UserPwd", "  isnull(dr,0)=0 and id=" + ListComp[0].UserID + " ", "");
                    if (user.Count > 0)
                    {
                        txtUsername.Disabled = true;
                        txtUserPhone.Disabled = true;
                        txtUsername.Value = user[0].UserName;
                        txtUserPhone.Value = user[0].Phone;
                        txtUserTrueName.Value = user[0].TrueName;
                        txtUpwd.Attributes.Add("value", "123456123456");
                        txtUpwd.Enabled = false;
                        txtUpwds.Attributes.Add("value", "123456123456");
                        txtUpwds.Enabled = false;
                    }
                    else
                    {
                        if (Request.UrlReferrer != null)
                        {
                            JScript.AlertMsgMo(this, "用户数据有误", "function (){ history.go(-1); }");
                            return;
                        }
                        else
                        {
                            Response.Write("用户数据有误。");
                            Response.End();
                        }
                    }
                    if (UserType == 3 || UserType == 4)
                    {
                        if (comp.OrgID != OrgID)
                        {
                            Response.Write("数据不存在");
                            Response.End();
                        }
                    }
                }
                else
                {
                    if (Request.UrlReferrer != null)
                    {
                        JScript.AlertMsgMo(this, "用户明细数据有误", "function (){ history.go(-1); }");
                        return;
                    }
                    else
                    {
                        Response.Write("用户明细数据有误。");
                        Response.End();
                    }
                }
            }
            else
            {
                if (Request.UrlReferrer != null)
                {
                    JScript.AlertMsgMo(this, "数据错误", "function (){ history.go(-1); }");
                    return;
                }
                else
                {
                    Response.Write("数据错误。");
                    Response.End();
                }
            }
        }
        else
        {
            Common.BindErptype(ddlErptype, 0);
            txtUpwd.Attributes.Add("value", "123456");
            txtUpwds.Attributes.Add("value", "123456");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (KeyID > 0)
        {
            bool Audit = false;
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
            if (comp != null)
            {
                if (comp.dr == 1)
                {
                    JScript.AlertMsg(this, "厂商不存在！。");
                    return;
                }
                string str = string.Empty;
                SqlTransaction Tran = null;
                try
                {
                    if (comp.AuditState == 2)
                    {
                        Audit = true;
                    }
                    if (Common.CompExistsAttribute("CompName", txtCompName.Value.Trim(), KeyID.ToString()))
                    {
                        JScript.AlertMsg(this, "该厂商名称已存在。");
                        return;
                    }
                    string CompAddr = hidProvince.Value.Trim();
                    if (!string.IsNullOrWhiteSpace(hidCity.Value.Trim()) && hidCity.Value.Trim() != "选择市")
                        CompAddr += "-" + hidCity.Value.Trim();
                    if (!string.IsNullOrWhiteSpace(hidArea.Value.Trim()) && hidArea.Value.Trim() != "选择区")
                        CompAddr += "-" + hidArea.Value.Trim();
                    comp.CompAddr = CompAddr;
                    comp.CompName = Common.NoHTML(txtCompName.Value.Trim());
                    comp.Capital = Capital.Value.Trim();
                    comp.CompType =Convert.ToInt32(CompType.SelectedValue);
                    comp.Tel = Common.NoHTML(txtTel.Value.Trim());
                    if (txtPrincipal.Value.Trim() != "")
                        comp.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
                    else
                        comp.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
                    comp.Legal = Common.NoHTML(txtLegal.Value.Trim());
                    if (txtPhone.Value.Trim() != "")
                        comp.Phone = Common.NoHTML(txtPhone.Value.Trim());
                    else
                        comp.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                    comp.ShortName = Common.NoHTML(txtShotName.Value.Trim());
                    comp.Zip = Common.NoHTML(txtZip.Value.Trim());
                    comp.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
                    comp.Licence = Common.NoHTML(txtLicence.Value.Trim());
                    comp.LegalTel = Common.NoHTML(txtLegalTel.Value.Trim());
                    comp.ManageInfo = Common.NoHTML(txtInfo.Value.Trim());
                    comp.Fax = Common.NoHTML(txtFax.Value.Trim());
                    comp.Account = Common.NoHTML(txtAccount.Value.Trim());
                    comp.Address = Common.NoHTML(txtAddress.Value.Trim());
                    comp.IndID = txtIndusName.SelectedValue.ToInt(0);
                    comp.Trade = txtIndusName.Items[txtIndusName.SelectedIndex].Text;
                    comp.AuditState = 2;
                    comp.AuditDate = DateTime.Now;//add by hgh 审核日期
                    comp.AuditUser = UserID.ToString();

                    comp.ts = DateTime.Now;
                    comp.modifyuser = UserID;
                    comp.OrganizationCode = Common.NoHTML(txtOrcode.Value.Trim());
                    comp.IsEnabled = rdEbleYes.Checked ? 1 : 0;
                    comp.HotShow = rdHotShowYes.Checked ? 1 : 0;
                    comp.FirstShow = Convert.ToInt32(ddlChkShow.SelectedValue);// rdFirstShowYes.Checked ? 1 : 0;
                    comp.Remark = Common.NoHTML(txtRemark.Value.Trim());
                    comp.Erptype = ddlErptype.SelectedValue.ToInt(0);
                    //企业编号  add by hgh
                    comp.CompCode = Common.CreateCode(KeyID);
                    if (HidFfileName.Value != "")
                    {
                        if (string.IsNullOrEmpty(comp.Attachment))
                        {
                            comp.Attachment = HidFfileName.Value;
                        }
                        else
                        {
                            comp.Attachment += "," + HidFfileName.Value;
                        }
                    }
                    List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and Ctype=1 and Utype=4 and CompID=" + KeyID + "", "");
                    if (ListComp.Count > 0)
                    {
                        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                        Audit = ListComp[0].IsAudit == 2;
                        if (Audit)
                        {
                            List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList("", " isnull(dr,0)=0 and compid=" + KeyID + "", "");
                            foreach (Hi.Model.BD_Distributor model2 in dis)
                            {
                                model2.IsEnabled = comp.IsEnabled;
                                model2.ts = DateTime.Now;
                                model2.modifyuser = UserID;
                                new Hi.BLL.BD_Distributor().Update(model2, Tran);
                            }
                            new Hi.BLL.BD_Company().Update(comp, Tran);
                            List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and id=" + ListComp[0].UserID + " ", "");
                            if (user.Count > 0)
                            {
                                user[0].TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                                user[0].ts = DateTime.Now;
                                user[0].modifyuser = UserID;
                                new Hi.BLL.SYS_Users().Update(user[0], Tran);
                            }
                            Tran.Commit();
                            Response.Redirect("CompInfo.aspx?go=1&KeyID=" + KeyID + "&type=5", false);
                        }
                        else
                        {
                            if (UserType == 3 || UserType == 4)
                            {
                                comp.OrgID = OrgID;
                                comp.SalesManID = SalesManID;
                            }
                            if ((new Hi.BLL.BD_Company().Update(comp, Tran)))
                            {
                                //新增数据字典
                                Hi.Model.BD_DefDoc doc = new Hi.Model.BD_DefDoc();
                                doc.CompID = KeyID;
                                doc.AtCode = "";
                                doc.AtName = "计量单位";
                                doc.ts = DateTime.Now;
                                doc.modifyuser = UserID;
                                doc.dr = 0;
                                List<Hi.Model.BD_DefDoc> ll = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + KeyID + " and atname='计量单位'", "", Tran);
                                if (ll.Count == 0)
                                {
                                    new Hi.BLL.BD_DefDoc().Add(doc, Tran);
                                }

                                //代理商加盟是否需要审核
                                Hi.Model.SYS_SysName sysname = new Hi.Model.SYS_SysName();
                                sysname.CompID = KeyID;
                                sysname.Code = "";
                                sysname.Name = "代理商加盟是否需要审核";
                                sysname.Value = "0";
                                sysname.ts = DateTime.Now;
                                sysname.modifyuser = UserID;
                                List<Hi.Model.SYS_SysName> sysl = new Hi.BLL.SYS_SysName().GetList("", "Name='代理商加盟是否需要审核' and CompID=" + KeyID, "", Tran);
                                if (sysl != null && sysl.Count == 0)
                                {
                                    new Hi.BLL.SYS_SysName().Add(sysname, Tran);
                                }

                                //费用科目
                                Hi.Model.BD_DefDoc doc1 = new Hi.Model.BD_DefDoc();
                                doc1.CompID = KeyID;
                                doc1.AtCode = "";
                                doc1.AtName = "费用科目";
                                doc1.ts = DateTime.Now;
                                doc1.modifyuser = UserID;
                                doc1.dr = 0;
                                List<Hi.Model.BD_DefDoc> ll1 = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + KeyID + " and atname='费用科目'", "", Tran);
                                if (ll1.Count == 0)
                                {
                                    new Hi.BLL.BD_DefDoc().Add(doc1, Tran);
                                }


                                doc.CompID = KeyID;
                                doc.AtCode = "";
                                doc.AtName = "代理商等级";
                                doc.ts = DateTime.Now;
                                doc.modifyuser = UserID;
                                doc.dr = 0;
                                List<Hi.Model.BD_DefDoc> lll = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + KeyID + " and atname='代理商等级'", "", Tran);
                                if (lll.Count == 0)
                                {
                                    new Hi.BLL.BD_DefDoc().Add(doc, Tran);
                                }
                                List<Hi.Model.SYS_Role> l = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + KeyID + " and RoleName='企业管理员'", "");
                                if (l.Count == 0)
                                {

                                    //新增角色（企业管理员）
                                    Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                                    role.CompID = KeyID;
                                    role.RoleName = "企业管理员";
                                    role.IsEnabled = 1;
                                    role.SortIndex = "1";
                                    role.CreateDate = DateTime.Now;
                                    role.CreateUserID = UserID;
                                    role.ts = DateTime.Now;
                                    role.modifyuser = UserID;
                                    role.dr = 0;
                                    int Roid = new Hi.BLL.SYS_Role().Add(role, Tran);

                                    //新增角色用户
                                    Hi.Model.SYS_RoleUser RoleUser = new Hi.Model.SYS_RoleUser();
                                    RoleUser.FunType = 1;
                                    RoleUser.UserID = ListComp[0].UserID;
                                    RoleUser.RoleID = Roid;
                                    RoleUser.IsEnabled = true;
                                    RoleUser.CreateUser = this.UserID.ToString();
                                    RoleUser.CreateDate = DateTime.Now;
                                    RoleUser.ts = DateTime.Now;
                                    RoleUser.dr = 0;
                                    new Hi.BLL.SYS_RoleUser().Add(RoleUser, Tran);

                                    //修改用户对应的角色
                                    List<Hi.Model.SYS_Users> ListUser = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and id=" + ListComp[0].UserID + " ", "");
                                    if (ListUser.Count > 0)
                                    {
                                        ListUser[0].TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                                        ListUser[0].ts = DateTime.Now;
                                        ListUser[0].modifyuser = UserID;
                                        ListUser[0].AuditState = 2;
                                        new Hi.BLL.SYS_Users().Update(ListUser[0], Tran);
                                    }
                                    ListComp[0].IsAudit = 2;
                                    ListComp[0].modifyuser = UserID;
                                    ListComp[0].ts = DateTime.Now;
                                    ListComp[0].RoleID = Roid;
                                    new Hi.BLL.SYS_CompUser().Update(ListComp[0], Tran);

                                    Hi.Model.BD_CompNews CNew = new Hi.Model.BD_CompNews();
                                    CNew.NewsTitle = "欢迎登录" + ConfigurationManager.AppSettings["PhoneSendName"].ToString() + "平台";
                                    CNew.NewsContents = "”" + ConfigurationManager.AppSettings["PhoneSendName"].ToString() + "”平台为企业和代理商之间搭建了电子商务平台，通过企业入驻及代理商的加盟，形成覆盖全行业的线上销售网络。同时，充分利用平台信息化整合优势，帮助入驻企业销售模式电商化改造，以及为代理商信息化建设提供有力支持。另外，还将为供应链上的相关各方提供全渠道的结算服务以及授信融资等多项互联网金融服务。";
                                    CNew.IsEnabled = 1;
                                    CNew.IsTop = 0;
                                    CNew.NewsType = 2;
                                    CNew.ShowType = "1,2";
                                    CNew.CompID = KeyID;
                                    CNew.CreateDate = DateTime.Now;
                                    CNew.CreateUserID = UserID;
                                    CNew.ts = DateTime.Now;
                                    CNew.modifyuser = UserID;
                                    new Hi.BLL.BD_CompNews().Add(CNew, Tran);

                                    //新增角色权限表
                                    Hi.Model.SYS_RoleSysFun rolesys = null;
                                    //add by hgh   增加了：and funcode<>'1030'
                                    List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=1 and funcode<>'1030'", ""); 
                                    foreach (Hi.Model.SYS_SysFun sys in funList)
                                    {
                                        rolesys = new Hi.Model.SYS_RoleSysFun();
                                        rolesys.CompID = KeyID;
                                        rolesys.RoleID = Roid;
                                        rolesys.FunCode = sys.FunCode;
                                        rolesys.FunName = sys.FunName;
                                        rolesys.IsEnabled = 1;
                                        rolesys.CreateUserID = UserID;
                                        rolesys.CreateDate = DateTime.Now;
                                        rolesys.ts = DateTime.Now;
                                        rolesys.modifyuser = UserID;
                                        new Hi.BLL.SYS_RoleSysFun().Add(rolesys, Tran);
                                    }

                                    //新增代理商分类
                                    Hi.Model.BD_DisType distype = new Hi.Model.BD_DisType();
                                    distype.CompID = KeyID;
                                    distype.TypeName = "全部";
                                    distype.ParentId = 0;
                                    distype.TypeCode = "1001";
                                    distype.SortIndex = "1000";
                                    distype.IsEnabled = 0;
                                    distype.CreateUserID = UserID;
                                    distype.CreateDate = DateTime.Now;
                                    distype.ts = DateTime.Now;
                                    distype.modifyuser = UserID;
                                    distype.dr = 0;
                                    new Hi.BLL.BD_DisType().Add(distype, Tran);
                                    Tran.Commit();

                                    //审核成功   添加一条默认的商品分类 -开始

                                    string Typecode ="";//商品大类
                                    //Hi.Model.SYS_GType gtype = new Hi.BLL.SYS_GType().GetList(" top 1 *", " Deep=3  and FullCode like '"+ Typecode + "-%' and IsEnabled=1 and dr=0 ", " parentid,ID")[0];
                                    //Hi.Model.BD_GoodsCategory DisType = new Hi.Model.BD_GoodsCategory();
                                    
                                    //DisType.Code = NewCategoryCode("1");
                                    //DisType.Deep = 1;
                                    //DisType.ParCode = "";
                                    //DisType.ParentId = 0;
                                    //DisType.CompID = KeyID;
                                    //DisType.CategoryName = "默认";
                                    //DisType.GoodsTypeID = gtype.ID;
                                    //DisType.SortIndex = "1000";
                                    //DisType.CreateDate = DateTime.Now;
                                    //DisType.CreateUserID = 0;
                                    //DisType.IsEnabled = 1;
                                    //DisType.ts = DateTime.Now;
                                    //DisType.modifyuser = 0;
                                    //SqlTransaction trans = SqlHelper.CreateStoreTranSaction();
                                    //try
                                    //{
                                    //    int countID = 0;
                                    //    if ((countID = new Hi.BLL.BD_GoodsCategory().Add(DisType, trans)) > 0)
                                    //    {
                                    //        List<Hi.Model.BD_Goods> gList = new Hi.BLL.BD_Goods().GetList("", " CategoryID=''", "");
                                    //        if (gList != null && gList.Count > 0)
                                    //        {
                                    //            foreach (var bdGoodse in gList)
                                    //            {
                                    //                bdGoodse.CategoryID = countID;
                                    //                new Hi.BLL.BD_Goods().Update(bdGoodse, trans);
                                    //            }
                                    //        }
                                    //        trans.Commit();

                                    //    }
                                        
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    Tiannuo.LogHelper.LogHelper.Error("Error", ex);
                                    //    if (trans != null)
                                    //    {
                                    //        if (trans.Connection != null)
                                    //        {
                                    //            trans.Rollback();
                                    //        }
                                    //    }
                                    //    return;
                                    //}
                                    //finally
                                    //{
                                    //    SqlHelper.ConnectionClose();
                                    //    if (trans != null)
                                    //    {
                                    //        if (trans.Connection != null)
                                    //        {
                                    //            trans.Connection.Close();
                                    //        }
                                    //    }
                                    //}
                                    //审核成功   添加一条默认的商品分类 -结束



                                    DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
                                    str = getphonecode.ReturnSTRS(ListUser[0].Phone, comp.CompName, ListUser[0].UserName);
                                    if (str != "Success")
                                    {
                                        JScript.AlertMsgMo(this, "审核通过的通知短信发送失败！请自行发送短信通知企业。", "function(){ window.location.href='CompInfo.aspx?go=1&KeyID=" + KeyID + "&type=" + Request.QueryString["type"] + "" + "'; }");
                                    }
                                    else
                                    {
                                        JScript.AlertMsgMo(this, "审核成功", "function(){ window.location.href='CompInfo.aspx?go=1&KeyID=" + KeyID + "&type=5'; }");
                                    }
                                }
                                else
                                {
                                    ListComp[0].IsAudit = 2;
                                    ListComp[0].modifyuser = UserID;
                                    ListComp[0].ts = DateTime.Now;
                                    new Hi.BLL.SYS_CompUser().Update(ListComp[0], Tran);
                                    Hi.Model.SYS_Users user = null;
                                    List<Hi.Model.SYS_CompUser> User2 = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and utype=4 and CompID=" + KeyID + "", "");
                                    if (User2.Count > 0)
                                    {
                                        user = new Hi.BLL.SYS_Users().GetModel(User2[0].UserID);
                                    }
                                    Tran.Commit();

                                    // 发短信通知
                                    DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
                                    getphonecode.GetUser(ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(), ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
                                    str = getphonecode.ReturnSTRS(user.Phone, comp.CompName, user.UserName);
                                    if (str != "Success")
                                    {
                                        JScript.AlertMsgMo(this, "审核通过的通知短信发送失败！请自行发送短信通知企业。", "function(){ window.location.href='CompInfo.aspx?go=1&KeyID=" + KeyID + "&type=" + Request.QueryString["type"] + "" + "'; }");
                                    }
                                    else
                                    {
                                        JScript.AlertMsgMo(this, "审核成功", "function(){ window.location.href='CompInfo.aspx?go=1&KeyID=" + KeyID + "&type=5'; }");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        JScript.AlertMsg(this, "用户明细数据异常！。");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Tiannuo.LogHelper.LogHelper.Error("Error", ex);
                    if (Tran != null)
                    {
                        if (Tran.Connection != null)
                            Tran.Rollback();
                    }
                    JScript.AlertMsgMo(this, "审核失败", "function(){ window.location.href=window.location.href; }");
                }
                finally
                {
                    DBUtility.SqlHelper.ConnectionClose();
                }
            }
            else
            {
                JScript.AlertMsg(this, "厂商不存在！。");
                return;
            }


            //cust by ggh 20180327 begin  审核通过时，默认给核心企业设置手续费  

            Settings(KeyID);

            //cust by ggh 20180327  end  审核通过时，默认给核心企业设置手续费  
        }
        else
        {
            if (Common.GetUserExists(txtUsername.Value.Trim()))
            {
                JScript.AlertMsg(this, "该登录帐号已存在。");
                return;
            }
            if (Common.CompExistsAttribute("CompName", txtCompName.Value.Trim()))
            {
                JScript.AlertMsg(this, "该厂商名称已存在。");
                return;
            }
            Regex Phonereg = new Regex("^0?1[0-9]{10}$");
            if (!Phonereg.IsMatch(txtUserPhone.Value.Trim()))
            {
                JScript.AlertMsg(this, "手机号码格式错误！");
                return;
            }
            if (Common.GetUserExists("Phone", txtUserPhone.Value.Trim()))
            {
                JScript.AlertMsg(this, "手机号码已被注册！");
                return;
            }
            Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
            if (UserType == 3 || UserType == 4)
            {
                comp.OrgID = OrgID;
                comp.SalesManID = SalesManID;
            }
            comp.CompName = Common.NoHTML(txtCompName.Value.Trim());
            comp.Capital = Capital.Value.Trim();
            comp.CompType = Convert.ToInt32(CompType.SelectedValue);
            comp.Tel = Common.NoHTML(txtTel.Value.Trim());
            if (txtPrincipal.Value.Trim() != "")
                comp.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
            else
                comp.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
            comp.Legal = Common.NoHTML(txtLegal.Value.Trim());
            comp.LegalTel = Common.NoHTML(txtLegalTel.Value.Trim());
            if (txtPhone.Value.Trim() != "")
                comp.Phone = Common.NoHTML(txtPhone.Value.Trim());
            else
                comp.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
            comp.ShortName = Common.NoHTML(txtShotName.Value.Trim());
            comp.Zip = Common.NoHTML(txtZip.Value.Trim());
            comp.Identitys = Common.NoHTML(txtIdentitys.Value.Trim());
            comp.Licence = Common.NoHTML(txtLicence.Value.Trim());
            comp.ManageInfo = Common.NoHTML(txtInfo.Value.Trim());
            comp.Fax = Common.NoHTML(txtFax.Value.Trim());
            comp.OrganizationCode = Common.NoHTML(txtOrcode.Value.Trim());
            comp.Trade = txtIndusName.Items[txtIndusName.SelectedIndex].Text;
            comp.Account = Common.NoHTML(txtAccount.Value.Trim());
            comp.Attachment = HidFfileName.Value;
            comp.Address = Common.NoHTML(txtAddress.Value.Trim());
            comp.CustomCompinfo = "本公司产品种类丰富、质量优良、价格公道、服务周到。感谢您长期的支持与厚爱，您的满意是我们最高的追求，我们将竭诚为您提供优质、贴心的服务！";

            string CompAddr = hidProvince.Value.Trim();
            if (!string.IsNullOrWhiteSpace(hidCity.Value.Trim()) && hidCity.Value.Trim() != "选择市")
                CompAddr += "-" + hidCity.Value.Trim();
            if (!string.IsNullOrWhiteSpace(hidArea.Value.Trim()) && hidArea.Value.Trim() != "选择区")
                CompAddr += "-" + hidArea.Value.Trim();
            comp.CompAddr = CompAddr;

            comp.IndID = txtIndusName.SelectedValue.ToInt(0);
            comp.CreateDate = DateTime.Now;
            comp.CreateUserID = UserID;
            comp.ts = DateTime.Now;
            comp.modifyuser = UserID;
            comp.IsEnabled = rdEbleYes.Checked ? 1 : 0;
            comp.HotShow = rdHotShowYes.Checked ? 1 : 0;
            comp.FirstShow = Convert.ToInt32(ddlChkShow.SelectedValue);// rdFirstShowYes.Checked ? 1 : 0;
            comp.SortIndex = "001";
            comp.Remark = Common.NoHTML(txtRemark.Value.Trim());
            comp.Erptype = ddlErptype.SelectedValue.ToInt(0);
            comp.AuditState = 0;
            int comid = 0;
            SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            comid = new Hi.BLL.BD_Company().Add(comp, Tran);
            comp.CompCode = Common.CreateCode(comid);
            comp.ID = comid;
            new Hi.BLL.BD_Company().Update(comp, Tran);
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            user.UserName = Common.NoHTML(txtUsername.Value.Trim());
            user.TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
            user.UserPwd = Util.md5(txtUpwd.Text.Trim());
            user.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
            user.AuditState = 2;
            user.IsEnabled = 1;
            user.AuditUser = UserID.ToString();
            user.CreateUserID = UserID;
            user.CreateDate = DateTime.Now;
            user.ts = DateTime.Now;
            user.modifyuser = UserID;
            int userid = 0;
            userid = new Hi.BLL.SYS_Users().Add(user, Tran);
            Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
            CompUser.CompID = comid;
            CompUser.DisID = 0;
            CompUser.CreateDate = DateTime.Now;
            CompUser.CreateUserID = UserID;
            CompUser.modifyuser = UserID;
            CompUser.CType = 1;
            CompUser.UType = 4;
            CompUser.IsEnabled = 1;
            CompUser.IsAudit = 0;
            CompUser.ts = DateTime.Now;
            CompUser.dr = 0;
            CompUser.UserID = userid;
            new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
            Tran.Commit();
            Response.Redirect("CompInfo.aspx?go=1&KeyID=" + comid,false);
        }
    }


    /// <summary>
    /// 获取最大Code
    /// </summary>
    /// <param name="deep"></param>
    /// <returns></returns>
    public string NewCategoryCode(string deep)
    {
        string NewCode = string.Empty;
        List<Hi.Model.BD_GoodsCategory> oneList = new Hi.BLL.BD_GoodsCategory().GetList("", "deep = '" + deep + "'", "ID desc");
        if (oneList != null && oneList.Count > 0)
        {
            if (deep == "1")
                NewCode = oneList[0].Code.Substring(0, 4);
            else if (deep == "2")
            {
                NewCode = oneList[0].Code.Substring(5, 4);
            }
            else
            {
                NewCode = oneList[0].Code.Substring(10, 4);
            }
        }
        else
        {
            NewCode = "999";
        }
        return (Convert.ToInt32(NewCode) + 1).ToString();
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

    /// <summary>
    /// 保存核心企业默认手续费设置
    /// </summary>
    /// <param name="CompId">核心企业id</param>
    public void Settings(int CompId)
    {
        string bind = string.Empty;
        string strSql = string.Empty;

        SqlTransaction TranSaction = null;
        SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
        Connection.Open();
        TranSaction = Connection.BeginTransaction();

        try
        {
            //企业ID
            //int CompId = Convert.ToInt32(Request["hid_compID"]);

            //手续费收取
            //手续费收取
            int pay_sxfsq = 1;//手续费收取(0,平台 1，经销商 2，核心企业)

            //int pay_sxfsq = 2;//厂商收费 Convert.ToInt32(Request["pay_sxfsq"]);

            //支付方式
            int pay_zffs = 0;//线上支付 Convert.ToInt32(Request["pay_zffs"]);

            //手续费比例
            decimal pay_kjzfbl = 5;// Convert.ToDecimal(Request["pay_kjzfbl"] + "" == "" ? "0" : Request["pay_kjzfbl"]);
            decimal pay_kjzfstart = 0;// Convert.ToDecimal(Request["pay_kjzfstart"] + "" == "" ? "0" : Request["pay_kjzfstart"]);
            decimal pay_kjzfend = 0;// Convert.ToDecimal(Request["pay_kjzfend"] + "" == "" ? "0" : Request["pay_kjzfend"]);

            decimal pay_ylzfbl = 0;// Convert.ToDecimal(Request["pay_ylzfbl"] + "" == "" ? "0" : Request["pay_ylzfbl"]);
            decimal pay_ylzfstart = 0;// Convert.ToDecimal(Request["pay_ylzfstart"] + "" == "" ? "0" : Request["pay_ylzfstart"]);
            decimal pay_ylzfend = 0;// Convert.ToDecimal(Request["pay_ylzfend"] + "" == "" ? "0" : Request["pay_ylzfend"]);

            decimal pay_b2cwyzfbl = 2;// Convert.ToDecimal(Request["pay_b2cwyzfbl"] + "" == "" ? "0" : Request["pay_b2cwyzfbl"]);
            decimal pay_b2cwyzfstart = 0;// Convert.ToDecimal(Request["pay_b2cwyzfstart"] + "" == "" ? "0" : Request["pay_b2cwyzfstart"]);

            decimal pay_b2bwyzf = 10;// Convert.ToDecimal(Request["pay_b2bwyzf"] + "" == "" ? "0" : Request["pay_b2bwyzf"]);

            //免手续费支付次数
            decimal Pay_mfcs = 0;// Convert.ToDecimal(Request["Pay_mfcs"] + "" == "" ? "0" : Request["Pay_mfcs"]);




            //查询该企业的设置
            List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompId, "");

            //判断企业的是否有设置
            if (Sysl.Count > 0)
            {
                strSql = string.Format(@"UPDATE [Pay_PaymentSettings]
                   SET [pay_sxfsq] = {0}
                      ,[pay_zffs] = {1}
                      ,[pay_kjzfbl] = {2}
                      ,[pay_kjzfstart] = {3}
                      ,[pay_kjzfend] ={4}
                      ,[pay_ylzfbl] = {5}
                      ,[pay_ylzfstart] ={6}
                      ,[pay_ylzfend] = {7}
                      ,[pay_b2cwyzfbl] = {8}
                      ,[pay_b2bwyzf] = {9}
                      ,[Pay_mfcs] = {10}       
                      ,[modifyuser] = {11}
                      ,vdef1={13}
                 WHERE [CompID] = {12}", pay_sxfsq, pay_zffs, pay_kjzfbl, pay_kjzfstart, pay_kjzfend
                        , pay_ylzfbl, pay_ylzfstart, pay_ylzfend, pay_b2cwyzfbl, pay_b2bwyzf, Pay_mfcs, UserID, CompId, pay_b2cwyzfstart);

            }
            else
            {
                strSql = string.Format(@"INSERT INTO [Pay_PaymentSettings]
                               ([CompID]
                               ,[pay_sxfsq]
                               ,[pay_zffs]
                               ,[pay_kjzfbl]
                               ,[pay_kjzfstart]
                               ,[pay_kjzfend]
                               ,[pay_ylzfbl]
                               ,[pay_ylzfstart]
                               ,[pay_ylzfend]
                               ,[pay_b2cwyzfbl]
                               ,[pay_b2bwyzf]
                               ,[Pay_mfcs]
                               ,[createUser]
                               ,[createDate]
                               ,[Start]
                               ,[remark]          
                               ,[ts]
                               ,[dr]
                               ,[modifyuser]
                               ,vdef1)
                         VALUES
                               ({0}
                               ,{1}
                               ,{2}
                               ,{3}
                               ,{4}
                               ,{5}
                               ,{6}
                               ,{7}
                               ,{8}
                               ,{9}
                               ,{10}
                               ,{11}
                               ,{12}
                               ,'{13}'
                               ,{14}
                               ,'{15}'         
                               ,'{16}'
                               ,{17}
                               ,{18}
                               ,{19})", CompId, pay_sxfsq, pay_zffs, pay_kjzfbl, pay_kjzfstart, pay_kjzfend, pay_ylzfbl
                            , pay_ylzfstart, pay_ylzfend, pay_b2cwyzfbl, pay_b2bwyzf, Pay_mfcs, UserID, DateTime.Now, 0
                            , "", DateTime.Now, 0, UserID, pay_b2cwyzfstart);
            }

            SqlCommand cmd = new SqlCommand(strSql.ToString(), Connection, TranSaction);
            cmd.CommandType = CommandType.Text;
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            if (rowsAffected > 0)
            {
                TranSaction.Commit();
                // bind = "{\"ds\":\"0\",\"prompt\":\"提交成功！\"}";
            }
            else
            {
                TranSaction.Rollback();
                // bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
            }
        }
        catch (Exception ex)
        {
            TranSaction.Rollback();
            //bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
        }
        finally
        {
            Connection.Dispose();

            //Response.Write(bind);
            // Response.End();
        }

    }

}