using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinancingReferences;
using System.Web.Script.Serialization;
using LitJson;
using System.Configuration;

public partial class Financing_SpendingUserAdd : DisPageBase
{
    //public Hi.Model.SYS_Users user = null;
    public List<Hi.Model.PAY_OpenAccount> LPayOpen;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
        //user = LoginModel.IsLogined(this);
        lblMsg.Visible = false;
        if (!IsPostBack)
        {
            Common.BindDoCmentType(ddlDocumentType);
            DataBindS();
        }
    }

    public void DataBindS()
    {
        LPayOpen = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and DisID=" + this.DisID + "", "");
        if (LPayOpen.Count > 0)
        {
            btnupdate.InnerText = "确定修改";
            if (LPayOpen[0].State == 1)
            {
                txtAccNumver.Disabled = true;
            }
            txtOgCode.Visible = false;
            txtAccNumver.Attributes["style"] = "";
            txtAccNumver.Value = LPayOpen[0].AccNumver;
            txtAccName.Value = LPayOpen[0].AccName;
            txtAccountName.Value = LPayOpen[0].AccountName;
            ddlAccountNature.SelectedValue = LPayOpen[0].AccountNature.ToString();
            txtDocumentCode.Value = LPayOpen[0].DocumentCode;
            ddlDocumentType.SelectedValue = LPayOpen[0].DocumentType;
            txtOrgCode.Value = LPayOpen[0].OrgCode;
            txtBusinessLicense.Value = LPayOpen[0].BusinessLicense;
            txtAccAddress.Value = LPayOpen[0].AccAddress;
            ddlSex.SelectedValue = LPayOpen[0].Sex.ToString();
            txtNationality.Value = LPayOpen[0].Nationality;
            txtPhoneNumbe.Value = LPayOpen[0].PhoneNumbe;
            txtPhone.Value = LPayOpen[0].Phone;
            txtEmail.Value = LPayOpen[0].Email;
            txtFax.Value = LPayOpen[0].Fax;
            txtPostcode.Value = LPayOpen[0].Postcode;
        }
        else
        {
            txtOgCode.Value = ConfigurationManager.AppSettings["OrgCode"];
            txtAccNumver.Style.Add("width", "" + (150 - ConfigurationManager.AppSettings["OrgCode"].Length * 8 + 1) + "px");
            txtOgCode.Style.Add("width", "" + (ConfigurationManager.AppSettings["OrgCode"].Length * 8) + "px");
            txtOgCode.Attributes["readonly"] = "readonly";
        }
    }

    protected void Btn_Save(object sender, EventArgs e)
    {
        LPayOpen = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and DisID=" + this.DisID + "", "");
        BLL.Service.OpenAccountParame OPB = new BLL.Service.OpenAccountParame();
        IPubnetwk IPT = new IPubnetwk();
        IPT.Timeout = 2500;
        if (LPayOpen.Count > 0)
        {
            LPayOpen[0].AccName = txtAccName.Value.Trim();
            LPayOpen[0].AccountName = txtAccountName.Value.Trim();
            LPayOpen[0].AccountNature = ddlAccountNature.SelectedValue.ToInt(0);
            LPayOpen[0].DocumentType = ddlDocumentType.SelectedValue;
            LPayOpen[0].DocumentCode = txtDocumentCode.Value.Trim();
            LPayOpen[0].OrgCode = txtOrgCode.Value.Trim();
            LPayOpen[0].BusinessLicense = txtBusinessLicense.Value.Trim();
            LPayOpen[0].AccAddress = txtAccAddress.Value.Trim();
            LPayOpen[0].Sex = ddlSex.SelectedValue.ToInt(0);
            if (txtNationality.Value.Trim() != "")
                LPayOpen[0].Nationality = txtNationality.Value.Trim();
            LPayOpen[0].PhoneNumbe = txtPhoneNumbe.Value.Trim();
            LPayOpen[0].Phone = txtPhone.Value.Trim();
            LPayOpen[0].Email = txtEmail.Value.Trim();
            LPayOpen[0].Fax = txtFax.Value.Trim();
            LPayOpen[0].Postcode = txtPostcode.Value.Trim();
            LPayOpen[0].ts = DateTime.Now;
            LPayOpen[0].modifyuser = this.UserID;
            //开销户接口信息
            OPB.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");
            OPB.srl_ptnsrl = Common.Number_repeat("1");
            LPayOpen[0].vdef1 = OPB.srl_ptnsrl;
            OPB.cltacc_cltno = LPayOpen[0].AccNumver;
            OPB.cltacc_subno = "";
            OPB.cltacc_cltnm = LPayOpen[0].AccName;
            OPB.cltacc_pwd = "";
            OPB.clt_nm = LPayOpen[0].AccountName;
            OPB.clt_kd = LPayOpen[0].AccountNature.ToString();
            OPB.clt_cdtp = LPayOpen[0].DocumentType;
            OPB.clt_cdno = LPayOpen[0].DocumentCode;
            OPB.clt_orgcd = LPayOpen[0].OrgCode;
            OPB.clt_bslic = LPayOpen[0].BusinessLicense;
            OPB.clt_gender = LPayOpen[0].Sex.ToString();
            OPB.clt_nationality = "CHN";
            OPB.clt_telno = LPayOpen[0].PhoneNumbe;
            OPB.clt_faxno = LPayOpen[0].Fax;
            OPB.clt_mobno = LPayOpen[0].Phone;
            OPB.clt_email = LPayOpen[0].Email;
            OPB.clt_postno = LPayOpen[0].Postcode;
            OPB.clt_addr = LPayOpen[0].AccAddress;
            if (LPayOpen[0].State != 1)
                OPB.fcflg = "1";
            else
                OPB.fcflg = "2";
            OPB.acctp = "1";
            try
            {
                string Result = IPT.trd11000(new JavaScriptSerializer().Serialize(OPB));
                JsonData jData = null;
                try
                {
                    jData = JsonMapper.ToObject(Result);
                }
                catch
                {
                    throw new Exception(Result);
                }
                if (jData != null)
                {
                    if (jData["msghd_rspcode"].ToString() == "000000")
                    {
                        LPayOpen[0].State = 1;
                        if (new Hi.BLL.PAY_OpenAccount().Update(LPayOpen[0]))
                        {
                            Response.Redirect("SpendingUserInfo.aspx");
                        }
                    }
                    else
                    {
                        lblMsg.InnerText = "开销户修改失败," + jData["msghd_rspmsg"].ToString() + "";
                        lblMsg.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerText = "开销户修改失败," + ex.Message + "";
                lblMsg.Visible = true;
            }
        }
        else
        {
            if (Common.OenAExistsAttribute("AccNumver", txtAccNumver.Value.Trim()))
            {
                lblMsg.InnerText = "开销户账户号已存在，请修改。";
                lblMsg.Visible = true;
                return;
            }
            Hi.Model.PAY_OpenAccount model = new Hi.Model.PAY_OpenAccount();
            model.DisID = this.DisID;
            model.CompID = this.CompID;
            model.State = (int)Enums.Pay_OpenState.失败;
            model.AccNumver = ConfigurationManager.AppSettings["OrgCode"] + txtAccNumver.Value.Trim();
            model.AccName = txtAccName.Value.Trim();
            model.AccountName = txtAccountName.Value.Trim();
            model.AccountNature = ddlAccountNature.SelectedValue.ToInt(0);
            model.DocumentType = ddlDocumentType.SelectedValue;
            model.DocumentCode = txtDocumentCode.Value.Trim();
            model.OrgCode = txtOrgCode.Value.Trim();
            model.BusinessLicense = txtBusinessLicense.Value.Trim();
            model.AccAddress = txtAccAddress.Value.Trim();
            model.Sex = ddlSex.SelectedValue.ToInt(0);
            if (txtNationality.Value.Trim() != "")
                model.Nationality = txtNationality.Value.Trim();
            model.PhoneNumbe = txtPhoneNumbe.Value.Trim();
            model.Phone = txtPhone.Value.Trim();
            model.Email = txtEmail.Value.Trim();
            model.Fax = txtFax.Value.Trim();
            model.Postcode = txtPostcode.Value.Trim();
            model.ts = DateTime.Now;
            model.modifyuser = this.UserID;
            //开销户接口信息
            OPB.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");
            OPB.srl_ptnsrl = Common.Number_repeat("1");
            model.vdef1 = OPB.srl_ptnsrl;
            OPB.cltacc_cltno = model.AccNumver;
            OPB.cltacc_subno = "";
            OPB.cltacc_cltnm = model.AccName;
            OPB.cltacc_pwd = "";
            OPB.clt_nm = model.AccountName;
            OPB.clt_kd = model.AccountNature.ToString();
            OPB.clt_cdtp = model.DocumentType;
            OPB.clt_cdno = model.DocumentCode;
            OPB.clt_orgcd = model.OrgCode;
            OPB.clt_bslic = model.BusinessLicense;
            OPB.clt_gender = model.Sex.ToString();
            OPB.clt_nationality = "CHN";
            OPB.clt_telno = model.PhoneNumbe;
            OPB.clt_faxno = model.Fax;
            OPB.clt_mobno = model.Phone;
            OPB.clt_email = model.Email;
            OPB.clt_postno = model.Postcode;
            OPB.clt_addr = model.AccAddress;
            OPB.fcflg = "1";
            OPB.acctp = "1";
            try
            {
                int POid = 0;
                if ((POid= new Hi.BLL.PAY_OpenAccount().Add(model)) > 0)
                {
                    string Result = IPT.trd11000(new JavaScriptSerializer().Serialize(OPB));
                    JsonData jData = null;
                    try
                    {
                        jData = JsonMapper.ToObject(Result);
                    }
                    catch
                    {
                        throw new Exception(Result);
                    }
                    if (jData != null)
                    {
                        model = new Hi.BLL.PAY_OpenAccount().GetModel(POid);
                        if (jData["msghd_rspcode"].ToString() == "000000")
                        {
                            model.State = (int)Enums.Pay_OpenState.成功;
                            if (new Hi.BLL.PAY_OpenAccount().Update(model))
                            {
                                Response.Redirect("SpendingUserInfo.aspx");
                            }
                        }
                        else
                        {
                            model.dr = 1;
                            new Hi.BLL.PAY_OpenAccount().Update(model);
                            lblMsg.InnerText = "开户失败," + jData["msghd_rspmsg"].ToString() + "";
                            lblMsg.Visible = true;
                        }
                    }
                }
                else
                {
                    lblMsg.InnerText = "开户失败,服务器异常请重试";
                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerText = "开户失败," + ex.Message;
                lblMsg.Visible = true;
            }
        }
    }
}