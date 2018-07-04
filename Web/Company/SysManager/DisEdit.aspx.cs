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
using System.Net;
using System.Configuration;
using System.Text;

public partial class Company_SysManager_DisEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename,.txt_txtAreaname\").css(\"width\", \"170px\");</script>");
        if (!IsPostBack)
        {

            if (Request["type"] == "0")
            {
                atitle.InnerText = "代理商审核";
                this.atitle.Attributes.Add("href", "../SysManager/DisAuditList.aspx");
            }
            else if (Request["type"] == "2")
            {
                this.atitle.Attributes.Add("href", "../SysManager/DisList.aspx");
            }

            string Action = Request["Action"] + "";
            string Name = Request["Value"] + "";
            if (Action == "GetPhone")
            {
                Response.Write(ExistsUserPhone(Name));
                Response.End();
            }
            if (Action == "GetDis")
            {
                Response.Write(ExistsDisLevel(Name));
                Response.End();
            }
            if (Action == "DisExists")
            {
                Response.Write(DisExists(Name));
                Response.End();
            }
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                btitle.InnerText = "新增代理商";

            }

            //txtDisAreaBox.CompID = CompID.ToString();
            //txtDisTypeBox.CompID = CompID.ToString();
            //txtDisArea.CompID = CompID.ToString();
            //txtDisType.CompID = CompID.ToString();
            Databind();
            //Common.BindUnit(rptUnit, "代理商等级", CompID);
        }
        //DataBindLink();
        bind();
    }

    #region 三证
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
    //                    HtmlImage img = new HtmlImage();
    //                    img.Src = "../../images/icon_del.png";
    //                    img.Attributes.Add("title", "删除附件");
    //                    img.Attributes.Add("onclick", "AnnexDel(this,'Dis'," + KeyID + ",'" + file + "')");
    //                    div.Controls.Add(img);
    //                    DFile.Controls.Add(div);
    //                }
    //            }
    //        }
    //    }
    //}

    //public void Download_Click(object sender, EventArgs e)
    //{
    //    LinkButton bt = sender as LinkButton;
    //    string fileName = bt.Attributes["fileName"];
    //    string filePath = Server.MapPath("../../UploadFile/") + fileName;
    //    if (File.Exists(filePath))
    //    {
    //        FileInfo file = new FileInfo(filePath);
    //        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
    //        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
    //        Response.AddHeader("Content-length", file.Length.ToString());
    //        Response.ContentType = "appliction/octet-stream";
    //        Response.WriteFile(file.FullName);
    //        Response.Flush();
    //        Response.End();
    //    }
    //    else
    //    {
    //        JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误);
    //    }
    //}
    #endregion


    public string DisExists(string DisName)
    {
        int Disid = 0;
        List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id", " DisName='" + DisName + "' and isnull(dr,0)=0 ", "");

        if (Dis != null && Dis.Count > 0)
        {
            Disid = Dis[0].ID;
            List<Hi.Model.SYS_CompUser> comUser = new Hi.BLL.SYS_CompUser().GetList("", "CompID=" + CompID.ToString() + " and DisID=" + Disid + " and isnull(dr,0)=0 ", "");

            if (comUser != null && comUser.Count > 0)
            {
                return "{ \"result\":false,\"msg\":\"该代理商已是您的渠道商\"}";
            }
        }

        if (Common.DisExistsAttribute("DisName", DisName, CompID.ToString(), KeyID.ToString()))
        {
            return "{ \"result\":true}";
        }
        return "{ \"result\":false,\"msg\":\"\"}";
    }

    public string ExistsUserPhone(string Phone)
    {
        if (Common.GetUserExists("Phone", Phone))
        {
            List<Hi.Model.SYS_Users> user = null; user = new Hi.BLL.SYS_Users().GetList("id,UserName,ID", " Phone='" + Phone + "' and isnull(dr,0)=0", "");
            return "{ \"result\":true,\"userNO\":\"" + user[0].UserName + "\",\"userid\":\"" + user[0].ID + "\"}";
        }
        else
        {
            return "{ \"result\":false}"; ;
        }
    }
    public string ExistsDisLevel(string DisLevel)
    {
        List<Hi.Model.BD_DefDoc_B> lll = new Hi.BLL.BD_DefDoc_B().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='代理商等级' and atval='" + DisLevel + "'", "");
        if (lll.Count > 0)
        {
            return "{ \"result\":true}";
        }
        else
        {
            return "{ \"result\":false}"; ;
        }
    }
    public void Databind()
    {
        if (KeyID > 0)
        {

            Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
            if (Dis != null)
            {
                if (Dis.dr == 1)
                {
                    if (Request.UrlReferrer != null)
                    {
                        JScript.AlertMethod(this, "数据错误", JScript.IconOption.错误, "function (){ history.go(-1); }");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write("数据错误。");
                        Response.End();
                    }
                }
                //暂时屏蔽 edit by hgh  180125
                //if (Dis.CompID != CompID)
                //{
                //    if (Request.UrlReferrer != null)
                //    {
                //        JScript.AlertMethod(this, "数据错误！", JScript.IconOption.错误, "function (){ history.go(-1); }");
                //        Response.End();
                //        return;
                //    }
                //    else
                //    {
                //        Response.Write("数据错误。");
                //        Response.End();
                //    }
                //}
                try
                {
                    if (Request["type"] == "0")
                    {
                        btnAdd.Text = "确定并审核通过";
                        btitle.InnerText = "代理商审核";
                    }
                    else
                    {
                        if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
                        {
                            btitle.InnerText = "新增代理商";
                        }
                        else
                        {
                            if (Request["type"] == "2")
                            {
                                btitle.InnerText = "代理商信息维护";
                            }
                        }
                    }
                    txtDisName.Value = Dis.DisName;
                    txtPhone.Attributes.Remove("onblur");
                    //txtpassword.Attributes.Add("value", Dis.Paypwd); //edit by hgh
                    //txtDisTypeBox.Id = Dis.DisTypeID.ToString();
                    // txtDisAreaBox.Id = Dis.AreaID.ToString();

                    hidProvince.Value = Dis.Province;
                    hidCity.Value = Dis.City;
                    hidArea.Value = Dis.Area;
                    //txtLeading.Value = Dis.Leading;
                    //txtLeadingPhone.Value = Dis.LeadingPhone;
                    //txtunit.Value = Dis.DisLevel;
                    txtFax.Value = Dis.Fax;
                    //txtLicence.Value = Dis.Licence;
                    //txtTel.Value = Dis.Tel;
                    txtPerson.Value = Dis.Principal;
                    //txtFinancingRatio.Value = Dis.FinancingRatio.ToString("0");
                    txtPhone.Value = Dis.Phone;
                    txtZip.Value = Dis.Zip;
                    txtAddress.Value = Dis.Address;
                    txtRemark.Value = Dis.Remark;
                    DisCode.Value = Dis.DisCode;
                    txtUsername.Disabled = true;
                    txtUserPhone.Disabled = true;
                    txtUpwd.Enabled = false;
                    txtUpwds.Enabled = false;
                    List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and CompID=" + this.CompID + " and DisID=" + Dis.ID + "", "");
                    if (ListComp.Count > 0)
                    {

                        //代理商等级
                        hid_txtDisType.Value = ListComp[0].DisTypeID.ToString();
                        hid_txtDisType.Attributes.Add("code", ListComp[0].DisTypeID.ToString());
                        Hi.Model.BD_DisType type = new Hi.BLL.BD_DisType().GetModel(ListComp[0].DisTypeID);
                        if (type != null)
                        {
                            txtDisType.Value = type.TypeName;

                        }
                        //代理商地区
                        hid_txtDisArea.Value = ListComp[0].AreaID.ToString();
                        hid_txtDisArea.Attributes.Add("code", ListComp[0].AreaID.ToString());
                        Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(ListComp[0].AreaID);
                        if (area != null)
                        {
                            txtDisArea.Value = area.AreaName;
                        }
                        //代理商订单是否审核
                        if (ListComp[0].IsCheck != 0)
                        {
                            rdAuditNo.Checked = false;
                            rdAuditYes.Checked = true;
                        }
                        //代理商启用授信
                        if (ListComp[0].CreditType == 1)
                        {
                            this.txtCreditAmount.Value = ListComp[0].CreditAmount.ToString("0.00") == "0.00" ? "" : ListComp[0].CreditAmount.ToString("0.00");
                            rdCreditYes.Checked = true;
                            rdCreditNo.Checked = false;
                        }
                        //代理商启用帐号
                        if (ListComp[0].IsEnabled != 1)
                        {
                            rdEnabledNo.Checked = true;
                            rdEnabledYes.Checked = false;
                        }

                        List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("UserPwd,UserName,TrueName,Phone", " isnull(dr,0)=0 and id=" + ListComp[0].UserID + "", "");
                        if (user.Count > 0)
                        {
                            txtUsername.Value = user[0].UserName;
                            txtUpwd.Attributes.Add("value", "123456123456");
                            txtUpwds.Attributes.Add("value", "123456123456");
                            txtUserPhone.Value = user[0].Phone;
                            txtUserTrueName.Value = user[0].TrueName;
                        }
                        else
                        {
                            txtUsername.Disabled = false;
                            txtUserPhone.Disabled = false;
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                if (Request.UrlReferrer != null)
                {
                    JScript.AlertMethod(this, "数据错误", JScript.IconOption.错误, "function (){ history.go(-1); }");
                    Response.End();
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
            txtUpwd.Attributes.Add("value", "123456");
            txtUpwds.Attributes.Add("value", "123456");
            DisCode.Value = DateTime.Now.ToString("yyyyMMddHHmm");
        }
    }

    /// <summary>
    /// 确认按钮单机事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        Hi.Model.BD_Distributor Dis = null;
        if (KeyID > 0)
        {
            SqlTransaction Tran = null;
            try
            {
                bool IsAudit = false;
                Dis = new Hi.BLL.BD_Distributor().GetModel(KeyID);
                if (Dis != null)
                {
                    Dis.DisName = txtDisName.Value.Trim();
                    //if (Common.DisExistsAttribute("DisName", Dis.DisName, CompID.ToString(), KeyID.ToString()))
                    //{
                    //    JScript.AlertMsgOne(this, "该代理商名称已存在！", JScript.IconOption.错误);
                    //    return;
                    //}

                    IsAudit = Dis.AuditState == 2;

                    //  Dis.DisTypeID = txtDisTypeBox.Id.ToInt(0);
                    // Dis.AreaID = txtDisAreaBox.Id.ToInt(0);
                    Dis.Province = Common.NoHTML(hidProvince.Value.Trim());
                    Dis.City = Common.NoHTML(hidCity.Value.Trim());
                    Dis.Area = Common.NoHTML(hidArea.Value.Trim());
                    //Dis.DisLevel = Common.NoHTML(txtunit.Value.Trim());
                    Dis.DisCode = Common.NoHTML(DisCode.Value.Trim());
                    //Dis.FinancingRatio = txtFinancingRatio.Value.Trim().ToInt(0);
                    Dis.Address = Common.NoHTML(txtAddress.Value.Trim());
                    //Dis.Leading = Common.NoHTML(txtLeading.Value.Trim());
                    //Dis.LeadingPhone = Common.NoHTML(txtLeadingPhone.Value.Trim());
                    if (txtPhone.Value.Trim() != "")
                        Dis.Phone = Common.NoHTML(txtPhone.Value.Trim());
                    else
                        Dis.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                    if (txtPerson.Value.Trim() != "")
                        Dis.Principal = Common.NoHTML(txtPerson.Value.Trim());
                    else
                        Dis.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
                    Dis.Zip = Common.NoHTML(txtZip.Value.Trim());
                    Dis.Fax = Common.NoHTML(txtFax.Value.Trim());
                    //Dis.Tel = Common.NoHTML(txtTel.Value.Trim());
                    //Dis.Licence = Common.NoHTML(txtLicence.Value.Trim());
                    Dis.Remark = Common.NoHTML(txtRemark.Value.Trim());
                    Dis.DisAccount = 0;
                    Dis.AuditState = 2;

                    Dis.DisTypeID = hid_txtDisType.Value.ToInt(0);
                    Dis.AreaID = hid_txtDisArea.Value.ToInt(0);
                    Dis.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                    Dis.IsCheck = rdAuditYes.Checked ? 1 : 0;
                    Dis.CreditType = rdCreditYes.Checked ? 1 : 0;
                    if (Dis.CreditType == 1)
                    {
                        Dis.CreditAmount = this.txtCreditAmount.Value.Trim().ToString().ToDecimal(0);
                        //if (this.txtCreditAmount.Value.Trim().ToString() != "")
                        //{
                        //    //未支付的订单企业授信
                        // decimal GetSumAmount = OrderInfoType.GetSumAmount(KeyID);
                        //    if (Dis.CreditAmount < GetSumAmount)
                        //    {
                        //        JScript.AlertMsgOne(this, "正在赊销的订单金额大于企业授信额度，订单赊销金额：" + GetSumAmount.ToString("N"), JScript.IconOption.错误);
                        //        return;
                        //    }
                        //}
                    }

                    //if (!string.IsNullOrEmpty(HidFfileName.Value))
                    //{
                    //    if (Dis.pic == "")
                    //    {
                    //        Dis.pic = HidFfileName.Value;
                    //    }
                    //    else
                    //    {
                    //        Dis.pic += "," + HidFfileName.Value;
                    //    }
                    //}
                    Dis.ts = DateTime.Now;
                    Dis.modifyuser = UserID;
                    Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                    List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and CompID=" + this.CompID + " and DisID=" + Dis.ID + "", "");
                    if (new Hi.BLL.BD_Distributor().Update(Dis, Tran))
                    {
                        if (ListComp.Count > 0)
                        {
                            List<Hi.Model.SYS_Users> users = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and id=" + ListComp[0].UserID + "", "");
                            if (users.Count > 0)
                            {
                                IsAudit = ListComp[0].IsAudit == 2;
                                if (!IsAudit)
                                {
                                    List<Hi.Model.SYS_Role> l = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and isenabled=1 and DisID=" + KeyID + " and RoleName='企业管理员'", "");
                                    if (l.Count == 0)
                                    {
                                        //新增角色（企业管理员）
                                        Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                                        role.CompID = CompID;
                                        role.DisID = KeyID;
                                        role.RoleName = "企业管理员";
                                        role.IsEnabled = 1;
                                        role.SortIndex = "1";
                                        role.CreateDate = DateTime.Now;
                                        role.CreateUserID = UserID;
                                        role.ts = DateTime.Now;
                                        role.modifyuser = UserID;
                                        role.dr = 0;
                                        int Roid = new Hi.BLL.SYS_Role().Add(role, Tran);
                                        //修改用户明细对应的角色
                                        //ListComp[0].IsEnabled = 1;
                                        ListComp[0].IsAudit = 2;
                                        ListComp[0].RoleID = 0;

                                        ///修改用户
                                        users[0].IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                                        users[0].TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                                        users[0].ts = DateTime.Now;
                                        users[0].modifyuser = UserID;
                                        users[0].AuditState = 2;
                                        new Hi.BLL.SYS_Users().Update(users[0], Tran);
                                        //新增角色权限表
                                        Hi.Model.SYS_RoleSysFun rolesys = null;
                                        List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=2", "");
                                        foreach (Hi.Model.SYS_SysFun sys in funList)
                                        {
                                            rolesys = new Hi.Model.SYS_RoleSysFun();
                                            rolesys.CompID = CompID;
                                            rolesys.DisID = KeyID;
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
                                    }
                                    Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                                    addr.Province = Dis.Province;
                                    addr.City = Dis.City;
                                    addr.Area = Dis.Area;
                                    addr.DisID = KeyID;
                                    addr.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
                                    addr.Phone = users[0].Phone;
                                    addr.Address = Dis.Province + Dis.City + Dis.Area + Common.NoHTML(txtAddress.Value.Trim());
                                    addr.IsDefault = 1;
                                    addr.ts = DateTime.Now;
                                    addr.CreateDate = DateTime.Now;
                                    addr.CreateUserID = UserID;
                                    addr.modifyuser = UserID;
                                    new Hi.BLL.BD_DisAddr().Add(addr, Tran);
                                    Tran.Commit();
                                    DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
                                    getphonecode.GetUser(ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(), ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
                                    str = getphonecode.ReturnDisRe(users[0].Phone, new Hi.BLL.BD_Company().GetModel(CompID).CompName, users[0].UserName);
                                    if (str != "Success")
                                    {
                                        JScript.AlertMethod(this, "告知代理商审核通过的短信发送失败！请自行发送短信告知代理商！", JScript.IconOption.哭脸, "function (){ window.location.href='DisInfo.aspx?KeyID=" + KeyID + "'; }");
                                    }
                                    else
                                    {
                                        if (Request["nextstep"] + "" == "1")
                                        {
                                            Response.Redirect("DisInfo.aspx?nextstep=1&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"]);
                                        }
                                        else
                                        {
                                            Response.Redirect("DisInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"]);
                                        }
                                    }

                                    return;
                                }
                                users[0].TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                                users[0].ts = DateTime.Now;
                                users[0].modifyuser = UserID;
                                new Hi.BLL.SYS_Users().Update(users[0], Tran);

                                ListComp[0].DisTypeID = hid_txtDisType.Value.ToInt(0);
                                ListComp[0].AreaID = hid_txtDisArea.Value.ToInt(0);
                                ListComp[0].IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                                ListComp[0].IsCheck = rdAuditYes.Checked ? 1 : 0;
                                ListComp[0].CreditType = rdCreditYes.Checked ? 1 : 0;
                                if (ListComp[0].CreditType == 1)
                                {
                                    ListComp[0].CreditAmount = this.txtCreditAmount.Value.Trim().ToString().ToDecimal(0);
                                }
                                new Hi.BLL.SYS_CompUser().Update(ListComp[0], Tran);


                                List<Hi.Model.BD_DisAddr> LDaddr = new Hi.BLL.BD_DisAddr().GetList(" top 1 *", " dr=0 and  disid=" + KeyID + "", " createdate asc");
                                if (LDaddr.Count > 0)
                                {
                                    LDaddr[0].Province = Dis.Province;
                                    LDaddr[0].City = Dis.City;
                                    LDaddr[0].Area = Dis.Area;
                                    LDaddr[0].Address = Dis.Province + Dis.City + Dis.Area + Common.NoHTML(txtAddress.Value.Trim());
                                    LDaddr[0].modifyuser = UserID;
                                    LDaddr[0].ts = DateTime.Now;
                                    new Hi.BLL.BD_DisAddr().Update(LDaddr[0], Tran);
                                }
                                Tran.Commit();
                                if (Request["nextstep"] + "" == "1")
                                {
                                    Response.Redirect("DisInfo.aspx?nextstep=1&KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"]);
                                }
                                else
                                {
                                    Response.Redirect("DisInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"]);
                                }
                            }
                            else
                            {
                                Tran.Rollback();
                                JScript.AlertMsgOne(this, "用户异常，请联系网站客服。", JScript.IconOption.错误);
                                return;
                            }
                        }
                        else
                        {
                            Tran.Rollback();
                            JScript.AlertMsgOne(this, "用户异常，请联系网站客服。", JScript.IconOption.错误);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                }
                JScript.AlertMethod(this, "操作失败", JScript.IconOption.错误, "function (){ window.location.href=window.location.href; }");
            }
            finally
            {
                DBUtility.SqlHelper.ConnectionClose();
            }
        }
        else
        {
            Regex Phonereg = new Regex("^0?1[0-9]{10}$");
            Dis = new Hi.Model.BD_Distributor();
            Dis.CompID = CompID;
            Dis.DisName = txtDisName.Value.Trim();
            if (Common.DisExistsAttribute("DisName", Dis.DisName, CompID.ToString()))
            {
                JScript.AlertMsgOne(this, "该代理商名称已存在", JScript.IconOption.错误);
                return;
            }
            if (Common.GetUserExists(txtUsername.Value.Trim()))
            {
                JScript.AlertMsgOne(this, "该登录帐号已存在, 请更换！", JScript.IconOption.错误);
                return;
            }
            if (!Phonereg.IsMatch(txtUserPhone.Value.Trim()))
            {
                JScript.AlertMsgOne(this, "手机号码格式错误！", JScript.IconOption.错误);
                return;
            }
            if (Common.GetUserExists("Phone", txtUserPhone.Value.Trim()) && this.userid.Value == "0")
            {
                JScript.AlertMsgOne(this, "已被注册的手机号码，请通知代理商参与招商和加盟！", JScript.IconOption.错误);
                return;
            }
            if (this.userid.Value != "0")
            {
                List<Hi.Model.SYS_CompUser> compuser = new Hi.BLL.SYS_CompUser().GetList("id", " UserID='" + this.userid.Value + "' and CompID='" + CompID + "' and isnull(dr,0)=0", "");
                if (compuser.Count > 0)
                {
                    this.userid.Value = "0";
                    JScript.AlertMsgOne(this, "该手机号已经是您的代理商，无法重复注册两个帐号", JScript.IconOption.错误);
                    return;
                }
            }

            //Dis.DisTypeID = txtDisTypeBox.Id.ToInt(0);
            //Dis.AreaID = txtDisAreaBox.Id.ToInt(0);
            Dis.Province = Common.NoHTML(hidProvince.Value.Trim());
            Dis.City = Common.NoHTML(hidCity.Value.Trim());
            Dis.Area = Common.NoHTML(hidArea.Value.Trim());
            //Dis.DisLevel = Common.NoHTML(txtunit.Value.Trim());
            //Dis.FinancingRatio = txtFinancingRatio.Value.Trim().ToInt(0);
            Dis.Address = Common.NoHTML(txtAddress.Value.Trim());
            if (string.IsNullOrWhiteSpace(DisCode.Value))
            {
                Dis.DisCode = DateTime.Now.ToString("yyyyMMddHHmm");
            }
            else
            {
                Dis.DisCode = Common.NoHTML(DisCode.Value.Trim());
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Value))
            {
                Dis.Phone = Common.NoHTML(txtPhone.Value.Trim());
            }
            else
            {
                Dis.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
            }

            if (!string.IsNullOrWhiteSpace(txtPerson.Value))
            {
                Dis.Principal = Common.NoHTML(txtPerson.Value.Trim());
            }
            else
            {
                Dis.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
            }
            //Dis.Leading = Common.NoHTML(txtLeading.Value.Trim());
            //Dis.LeadingPhone = Common.NoHTML(txtLeadingPhone.Value.Trim());
            Dis.Zip = Common.NoHTML(txtZip.Value.Trim());
            Dis.Fax = Common.NoHTML(txtFax.Value.Trim());
            Dis.Remark = Common.NoHTML(txtRemark.Value.Trim());
            //Dis.Tel = Common.NoHTML(txtTel.Value.Trim());
            //Dis.Licence = Common.NoHTML(txtLicence.Value.Trim());
            Dis.Paypwd = Util.md5("123456");
            Dis.CreateDate = DateTime.Now;
            Dis.CreateUserID = UserID;
            Dis.ts = DateTime.Now;
            Dis.modifyuser = UserID;
            //Dis.pic = HidFfileName.Value;
            Dis.DisAccount = 0;
            Dis.AuditState = 2;

            Dis.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
            Dis.DisTypeID = hid_txtDisType.Value.ToInt(0);
            Dis.AreaID = hid_txtDisArea.Value.ToInt(0);
            Dis.IsCheck = rdAuditYes.Checked ? 1 : 0;
            //Dis.CreditType = rdCreditYes.Checked ? 1 : 0;
            if (Dis.CreditType == 1)
            {
                Dis.CreditAmount = this.txtCreditAmount.Value.Trim().ToString().ToDecimal(0);
            }

            int disId = 0;
            SqlTransaction Tran = null;
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();

            try
            {
                disId = new Hi.BLL.BD_Distributor().Add(Dis, Tran);
                if (disId > 0)
                {
                    //新增角色（企业管理员）
                    Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                    role.CompID = CompID;
                    role.DisID = disId;
                    role.RoleName = "企业管理员";
                    role.IsEnabled = 1;
                    role.SortIndex = "1";
                    role.CreateDate = DateTime.Now;
                    role.CreateUserID = UserID;
                    role.ts = DateTime.Now;
                    role.modifyuser = UserID;
                    role.dr = 0;
                    int Roid = new Hi.BLL.SYS_Role().Add(role, Tran);
                    //新增管理员用户和角色
                    int AddUserid = 0;
                    if (this.userid.Value == "0")
                    {
                        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                        user.UserName = Common.NoHTML(txtUsername.Value.Trim());
                        // user.CompID = CompID;
                        // user.Type = 5;
                        // user.RoleID = Roid;
                        user.TrueName = Common.NoHTML(txtUserTrueName.Value.Trim());
                        user.UserPwd = Util.md5(txtUpwd.Text.Trim());
                        user.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                        user.AuditState = 2;
                        user.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                        user.CreateUserID = UserID;
                        user.CreateDate = DateTime.Now;
                        user.ts = DateTime.Now;
                        user.modifyuser = UserID;
                        AddUserid = new Hi.BLL.SYS_Users().Add(user, Tran);
                        //新增角色用户
                        Hi.Model.SYS_RoleUser RoleUser = new Hi.Model.SYS_RoleUser();
                        RoleUser.FunType = 1;
                        RoleUser.UserID = AddUserid;
                        RoleUser.RoleID = Roid;
                        RoleUser.IsEnabled = true;
                        RoleUser.CreateUser = this.UserID.ToString();
                        RoleUser.CreateDate = DateTime.Now;
                        RoleUser.ts = DateTime.Now;
                        RoleUser.dr = 0;
                        new Hi.BLL.SYS_RoleUser().Add(RoleUser, Tran);
                        //新增角色权限表
                        Hi.Model.SYS_RoleSysFun rolesys = null;
                        List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=2", "");
                        foreach (Hi.Model.SYS_SysFun sys in funList)
                        {
                            rolesys = new Hi.Model.SYS_RoleSysFun();
                            rolesys.CompID = CompID;
                            rolesys.DisID = disId;
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

                    }
                    else
                    {
                        AddUserid = Convert.ToInt32(this.userid.Value);
                    }

                    ///用户明细表
                    Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                    CompUser.CompID = CompID;
                    CompUser.DisID = disId;
                    CompUser.CreateDate = DateTime.Now;
                    CompUser.CreateUserID = UserID;
                    CompUser.modifyuser = UserID;
                    CompUser.CType = 2;
                    CompUser.UType = 5;
                    //CompUser.IsEnabled = 1;
                    CompUser.IsAudit = 2;
                    CompUser.RoleID = 0;
                    CompUser.ts = DateTime.Now;
                    CompUser.dr = 0;
                    CompUser.UserID = AddUserid;

                    //by szj 代理商信息
                    CompUser.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                    CompUser.DisTypeID = hid_txtDisType.Value.ToInt(0);
                    CompUser.AreaID = hid_txtDisArea.Value.ToInt(0);
                    CompUser.IsCheck = rdAuditYes.Checked ? 1 : 0;
                    CompUser.CreditType = rdCreditYes.Checked ? 1 : 0;
                    if (CompUser.CreditType == 1)
                    {
                        CompUser.CreditAmount = this.txtCreditAmount.Value.Trim().ToString().ToDecimal(0);
                    }

                    new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);

                    Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                    addr.Province = hidProvince.Value;
                    addr.City = hidCity.Value;
                    addr.Area = hidArea.Value;
                    addr.DisID = disId;
                    addr.Principal = Common.NoHTML(txtUserTrueName.Value.Trim());
                    addr.Phone = Common.NoHTML(txtUserPhone.Value.Trim());
                    addr.Address = Dis.Province + Dis.City + Dis.Area + Common.NoHTML(txtAddress.Value.Trim());
                    addr.IsDefault = 1;
                    addr.ts = DateTime.Now;
                    addr.CreateDate = DateTime.Now;
                    addr.CreateUserID = UserID;
                    addr.modifyuser = UserID;
                    new Hi.BLL.BD_DisAddr().Add(addr, Tran);
                    Tran.Commit();
                    string nextstep = Request["nextstep"];
                    string type = Request["type"];
                    Response.Redirect("DisInfo.aspx?nextstep=" + nextstep + "&KeyID=" + Common.DesEncrypt(disId.ToString(), Common.EncryptKey) + "&type=" + type, true);
                }
                else
                {
                    JScript.AlertMethod(this, "操作失败", JScript.IconOption.错误, "function (){ window.location.href=window.location.href; }");
                }
            }
            catch
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                }
                JScript.AlertMethod(this, "新增失败", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
            }
            finally
            {
                DBUtility.SqlHelper.ConnectionClose();
            }
        }
    }

    protected void btnAddDis_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        string disName = this.txtDisName.Value.Trim();
        try
        {
            int disId = 0;
            // Find same name and not deleted
            List<Hi.Model.BD_Distributor> disList = new Hi.BLL.BD_Distributor().GetList("id", " DisName='" + disName + "' and isnull(dr,0)=0 ", "");

            // TODO: only one is acceptable
            if (disList != null)
            {
                if (disList.Count <= 0)
                {
                    JScript.AlertMethod(this, "代理商" + disName + "不存在", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
                    return;
                }
                else if (disList.Count > 1)
                {
                    JScript.AlertMethod(this, "代理商" + disName + "存在同名，请联系客服", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
                    return;
                }

                disId = disList[0].ID;

                int AddUserid = 0;
                // TODO： 如何选到管理员帐号
                List<Hi.Model.SYS_CompUser> comUser = new Hi.BLL.SYS_CompUser().GetList("", "DisID=" + disId + " and UType=5 and isnull(dr,0)=0 ", "createdate");
                if (comUser != null && comUser.Count > 0)
                {
                    AddUserid = comUser[0].UserID;
                }

                if (AddUserid != 0)
                {
                    List<Hi.Model.SYS_Role> l = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and CompID=" + this.CompID + " and isenabled=1 and DisID=" + disId + " and RoleName='企业管理员'", "");

                    if (l.Count == 0)
                    {
                        //新增角色（企业管理员）
                        Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                        role.CompID = CompID;
                        role.DisID = disId;
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
                        RoleUser.UserID = AddUserid;
                        RoleUser.RoleID = Roid;
                        RoleUser.IsEnabled = true;
                        RoleUser.CreateUser = this.UserID.ToString();
                        RoleUser.CreateDate = DateTime.Now;
                        RoleUser.ts = DateTime.Now;
                        RoleUser.dr = 0;
                        new Hi.BLL.SYS_RoleUser().Add(RoleUser, Tran);
                        //新增角色权限表
                        Hi.Model.SYS_RoleSysFun rolesys = null;
                        List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=2", "");
                        foreach (Hi.Model.SYS_SysFun sys in funList)
                        {
                            rolesys = new Hi.Model.SYS_RoleSysFun();
                            rolesys.CompID = CompID;
                            rolesys.DisID = disId;
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

                        ///用户明细表
                        Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                        CompUser.CompID = CompID;
                        CompUser.DisID = disId;
                        CompUser.CreateDate = DateTime.Now;
                        CompUser.CreateUserID = UserID;
                        CompUser.modifyuser = UserID;
                        CompUser.CType = 2;
                        CompUser.UType = 5;
                        //CompUser.IsEnabled = 1;
                        CompUser.IsAudit = 2;
                        CompUser.RoleID = 0;
                        CompUser.ts = DateTime.Now;
                        CompUser.dr = 0;
                        CompUser.UserID = AddUserid;

                        //by szj 代理商信息
                        CompUser.IsEnabled = rdEnabledYes.Checked ? 1 : 0;
                        CompUser.DisTypeID = hid_txtDisType.Value.ToInt(0);
                        CompUser.AreaID = hid_txtDisArea.Value.ToInt(0);
                        CompUser.IsCheck = rdAuditYes.Checked ? 1 : 0;
                        CompUser.CreditType = rdCreditYes.Checked ? 1 : 0;
                        if (CompUser.CreditType == 1)
                        {
                            CompUser.CreditAmount = this.txtCreditAmount.Value.Trim().ToString().ToDecimal(0);
                        }

                        new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);

                        Tran.Commit();
                        string nextstep = Request["nextstep"];
                        string type = Request["type"];
                        Response.Redirect("DisInfo.aspx?nextstep=" + nextstep + "&KeyID=" + Common.DesEncrypt(disId.ToString(), Common.EncryptKey) + "&type=" + type, true);

                    }
                }
                else
                {
                    JScript.AlertMethod(this, "代理商用户有误,新增失败", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
                }
            }
        }
        catch
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, "新增失败", JScript.IconOption.错误, "function(){ window.location.href=window.location.href; }");
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }

    //导入Eecel返回Dataset的方法
    public static DataSet ReaderExcelToDataset(string filePath)
    {
        string connStr = "";
        string fileType = System.IO.Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(fileType)) return null;
        //根据文件的类型定义链接映射字段
        //if (fileType == ".xls")
        //    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
        //else
        connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
        string sql_F = "Select * FROM [{0}]";

        OleDbConnection conn = null;
        OleDbDataAdapter da = null;
        DataTable dtSheetName = null;

        DataSet ds = new DataSet();
        try
        {
            // 初始化连接，并打开 
            conn = new OleDbConnection(connStr);
            conn.Open();

            // 获取数据源的表定义元数据                        
            string SheetName = "";
            dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            // 初始化适配器
            da = new OleDbDataAdapter();
            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
                SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

                if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                {
                    continue;
                }

                da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                DataSet dsItem = new DataSet();
                da.Fill(dsItem, SheetName.Replace("$", ""));
                ds.Tables.Add(dsItem.Tables[0].Copy());
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            // 关闭连接
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                da.Dispose();
                conn.Dispose();
            }
        }

        return ds;
    }
    /// <summary>
    /// 添加代理商等级
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddUnit_Click(object sender, EventArgs e)
    {
        //string unit = Common.NoHTML(txtunits.Value.Trim());
        //if (Util.IsEmpty(unit))
        //{
        //    JScript.AlertMsgOne(this, "代理商等级不能为空！", JScript.IconOption.错误);
        //    return;
        //}
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            //新增数据字典
            Hi.Model.BD_DefDoc doc = new Hi.Model.BD_DefDoc();
            doc.CompID = this.CompID;
            doc.AtCode = "";
            doc.AtName = "代理商等级";
            doc.ts = DateTime.Now;
            doc.modifyuser = UserID;
            doc.dr = 0;
            List<Hi.Model.BD_DefDoc> ll = new Hi.BLL.BD_DefDoc().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='代理商等级'", "");
            int defid = 0;
            if (ll.Count == 0)
            {
                defid = new Hi.BLL.BD_DefDoc().Add(doc, Tran);
            }
            else
            {
                defid = ll[0].ID;
            }
            if (defid != 0)
            {
                Hi.Model.BD_DefDoc_B doc2 = new Hi.Model.BD_DefDoc_B();
                doc2.CompID = this.CompID;
                doc2.DefID = defid;
                doc2.AtName = "代理商等级";
                //doc2.AtVal = txtunits.Value.Trim();
                doc2.ts = DateTime.Now;
                doc2.dr = 0;
                doc2.modifyuser = this.UserID;
                //List<Hi.Model.BD_DefDoc_B> lll = new Hi.BLL.BD_DefDoc_B().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='代理商等级' and defid=" + defid + " and atval='" + txtunits.Value.Trim() + "'", "");
                //if (lll.Count == 0)
                //{
                //    new Hi.BLL.BD_DefDoc_B().Add(doc2, Tran);
                //}
                //else
                //{
                //    JScript.AlertMsgOne(this, "代理商等级已存在！", JScript.IconOption.错误);
                //    return;
                //}
            }
            Tran.Commit();
            //this.txtunit.Value =Common.NoHTML( txtunits.Value.Trim());
            //Common.BindUnit(this.rptUnit, "代理商等级", this.CompID);//绑定单位下拉
        }
        catch (Exception)
        {

            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }

    /// <summary>
    /// 绑定  代理商分类 区域 
    /// </summary>
    public string DisType = string.Empty;//代理商分类数据源
    public string DisArea = string.Empty;//代理商区域数据源
    public string DisDj = string.Empty;//代理商等级
    public void bind()
    {
        //绑定分类
        StringBuilder sbdis = new StringBuilder();
        List<Hi.Model.BD_DisType> distype1 = new Hi.BLL.BD_DisType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0 and  ParentId=0  and CompID=" + this.CompID, "");
        if (distype1.Count > 0)
        {
            sbdis.Append("[");
            int count = 0;
            foreach (var item in distype1)
            {
                count++;
                sbdis.Append("{code:'" + item.ID + "',value: '" + item.ID + "',label: '" + item.TypeName + "'");
                List<Hi.Model.BD_DisType> distype2 = new Hi.BLL.BD_DisType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0  and ParentId=" + item.ID, "");
                if (distype2.Count > 0)
                {
                    sbdis.Append(",children: [");
                    int count2 = 0;
                    foreach (var item2 in distype2)
                    {
                        count2++;
                        sbdis.Append("{code:'" + item2.ID + "',value: '" + item2.ID + "',label: '" + item2.TypeName + "'");
                        List<Hi.Model.BD_DisType> distype3 = new Hi.BLL.BD_DisType().GetList("TypeCode,ID,TypeName", "isnull(dr,0)=0  and ParentId=" + item2.ID, "");
                        if (distype3.Count > 0)
                        {
                            sbdis.Append(",children: [");
                            int count3 = 0;
                            foreach (var item3 in distype3)
                            {
                                count3++;
                                if (count3 == distype3.Count)
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.TypeName + "'}");
                                else
                                    sbdis.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.TypeName + "'},");

                            }
                            sbdis.Append("]");

                        }

                        if (count2 == distype2.Count)
                            sbdis.Append("}");
                        else
                            sbdis.Append("},");
                    }
                    sbdis.Append("]");

                }
                if (count == distype1.Count)
                    sbdis.Append("}");
                else
                    sbdis.Append("},");
            }
            sbdis.Append("]");
            DisType = sbdis.ToString();
        }

        //绑定区域
        StringBuilder sbare = new StringBuilder();
        List<Hi.Model.BD_DisArea> are = new Hi.BLL.BD_DisArea().GetList("top 12 * ", "isnull(dr,0)=0 and  ParentId=0  and CompanyID=" + this.CompID, " SortIndex");
        if (are.Count > 0)
        {
            sbare.Append("[");
            int num = 0;
            foreach (var model in are)
            {
                num++;
                sbare.Append("{code:'" + model.ID + "',value: '" + model.ID + "',label: '" + model.AreaName + "'");
                List<Hi.Model.BD_DisArea> aret1 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model.ID, "");
                if (aret1.Count > 0)
                {
                    sbare.Append(",children: [");
                    int num2 = 0;
                    foreach (var model2 in aret1)
                    {
                        num2++;
                        sbare.Append("{code:'" + model2.ID + "',value: '" + model2.ID + "',label: '" + model2.AreaName + "'");
                        List<Hi.Model.BD_DisArea> are3 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model2.ID, "");
                        if (are3.Count > 0)
                        {
                            sbare.Append(",children: [");
                            int num3 = 0;
                            foreach (var item3 in are3)
                            {
                                num3++;
                                if (num3 == are3.Count)
                                    sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'}");
                                else
                                    sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'},");

                            }
                            sbare.Append("]");

                        }

                        if (num2 == aret1.Count)
                            sbare.Append("}");
                        else
                            sbare.Append("},");
                    }
                    sbare.Append("]");

                }
                if (num == are.Count)
                    sbare.Append("}");
                else
                    sbare.Append("},");
            }
            sbare.Append("]");
            DisArea = sbare.ToString();
        }


        //代理商等级

        StringBuilder disdj = new StringBuilder();
        List<Hi.Model.BD_DefDoc_B> lll = new Hi.BLL.BD_DefDoc_B().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and atname='代理商等级'", "");
        if (lll.Count > 0)
        {
            disdj.Append("[");
            int sum = 0;
            foreach (var models in lll)
            {
                sum++;
                disdj.Append("{code:'" + models.ID + "',value: '" + models.ID + "',label: '" + models.AtVal + "'");
                if (sum == lll.Count)
                    disdj.Append("}");
                else
                    disdj.Append("},");
            }
            disdj.Append("]");
            DisDj = disdj.ToString();
        }

    }
}