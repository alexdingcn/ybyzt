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

public partial class Financing_SettlementAdd :DisPageBase
{
    //Hi.Model.SYS_Users user;
    public string CityCode;
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
            string action = Request.Form["action"];
            string Pcode = Request.Form["Pcode"];
            if (action == "GetCity" && !string.IsNullOrEmpty(Pcode))
            {
                GetCityValue(Pcode);
            }
            Common.BindPro(selePrcCd, "请选择");
            List<Hi.Model.PAY_Withdrawals> withdrawals = new Hi.BLL.PAY_Withdrawals().GetList("", "dr=0  and DisID=" + this.DisID, "");
            if (withdrawals.Count > 0)
            {
                btnupdate.InnerText = "确定修改";
                txtAccNo.Value = withdrawals[0].AccNo;
                lblAccNm.InnerText = withdrawals[0].AccNm;
                seleAccTp.Value = withdrawals[0].AccTp.ToString();
                seleCrsMk.Value = withdrawals[0].CrsMk.ToString();
                if (selePrcCd.Items.FindByValue(withdrawals[0].PrcCd.ToString()) != null)
                {
                    selePrcCd.Value = withdrawals[0].PrcCd.ToString();
                    hdCityCd.Value = withdrawals[0].CityCd.ToString();
                    CityCode = hdCityCd.Value;
                }
                Boxbank.BankId = withdrawals[0].OpenBkCd;
                txtBkAddr.Value = withdrawals[0].OpenBkAddr;
                txtBkNm.Value = withdrawals[0].OpenBkNm;
            }
            else
            {
                List<Hi.Model.PAY_OpenAccount> openAcc = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and state=1 and DisID=" + this.DisID, "");
                if (openAcc.Count > 0)
                {
                    lblAccNm.InnerText = openAcc[0].AccName;
                }
            }
        }
    }

    public void GetCityValue(string Pcode)
    {
        if (!string.IsNullOrEmpty(Pcode))
        {
            string OPHtml = string.Empty;
            List<Hi.Model.BF_ZD_PROVINCE> Bpro = new Hi.BLL.BF_ZD_PROVINCE().GetList("", " PROVCODE='" + Pcode + "'", "");
            if (Bpro.Count > 0)
            {
                List<Hi.Model.BF_ZD_CITY> BCity = new Hi.BLL.BF_ZD_CITY().GetList("", " PROVID='" + Bpro[0].PROVID + "'", "");
                foreach (Hi.Model.BF_ZD_CITY model in BCity)
                {
                    OPHtml += "<option value=\"" + model.CITYCODE + "\">" + model.CITYNAME + "</option>";
                }
            }
            Response.Write(OPHtml);
            Response.End();
        }
    }

    protected void Btn_Save(object sender, EventArgs e)
    {
        List<Hi.Model.PAY_OpenAccount> openAcc = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and state=1 and DisID=" + this.DisID, "");
        if (openAcc.Count == 0)
        {
            lblMsg.InnerText = "请先添加开销户信息";
            return;
        }
        if (openAcc[0].State != 1) {
            lblMsg.InnerText = "请成功开销户后再绑定结算账户";
            return;
        }
        List<Hi.Model.PAY_Withdrawals> Lwithdrawals = new Hi.BLL.PAY_Withdrawals().GetList("", " isnull(dr,0)=0  and DisID=" + this.DisID, "");
        BLL.Service.WithdrawalsParame WWP = new BLL.Service.WithdrawalsParame();
        IPubnetwk IPT = new IPubnetwk();
        IPT.Timeout = 2500;
        if (Lwithdrawals.Count > 0)
        {
            try
            {
                Lwithdrawals[0].AccNo = txtAccNo.Value.Trim();
                Lwithdrawals[0].AccTp = seleAccTp.Value.ToInt(0);
                Lwithdrawals[0].CrsMk = seleCrsMk.Value.ToInt(0);
                Lwithdrawals[0].OpenBkCd = Boxbank.BankId;
                Lwithdrawals[0].PrcCd = selePrcCd.Value.ToInt(0);
                Lwithdrawals[0].CityCd = hdCityCd.Value.ToInt(0);
                Lwithdrawals[0].OpenBkNm = txtBkNm.Value.Trim();
                Lwithdrawals[0].OpenBkAddr = txtBkAddr.Value.Trim();
                Lwithdrawals[0].ts = DateTime.Now;
                Lwithdrawals[0].modifyuser = this.UserID;
                //结算账户接口参数赋值
                WWP.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");
                WWP.srl_ptnsrl = Common.Number_repeat("1");
                Lwithdrawals[0].vdef1 = WWP.srl_ptnsrl;
                WWP.cltacc_cltno = openAcc[0].AccNumver;
                WWP.cltacc_cltnm = openAcc[0].AccName;
                WWP.bkacc_accno = Lwithdrawals[0].AccNo;
                WWP.bkacc_accnm = Lwithdrawals[0].AccNm;
                WWP.bkacc_acctp = Lwithdrawals[0].AccTp.ToString();
                WWP.bkacc_crsmk = Lwithdrawals[0].CrsMk.ToString();
                WWP.bkacc_openbkcd = Lwithdrawals[0].OpenBkCd;
                WWP.bkacc_openbknm = Lwithdrawals[0].OpenBkNm;
                WWP.bkacc_prccd = Lwithdrawals[0].PrcCd.ToString();
                WWP.bkacc_citycd = Lwithdrawals[0].CityCd.ToString();
                WWP.bkacc_openbkaddr = Lwithdrawals[0].OpenBkAddr;
                if (Lwithdrawals[0].State != 1)
                    WWP.fcflg = "1";
                else
                    WWP.fcflg = "2";
                string Result = IPT.trd12000(new JavaScriptSerializer().Serialize(WWP));
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
                        Lwithdrawals[0].State = 1;
                        if (new Hi.BLL.PAY_Withdrawals().Update(Lwithdrawals[0]))
                        {
                            Response.Redirect("SettlementInfo.aspx", true);
                        }
                    }
                    else
                    {
                        lblMsg.InnerText = "结算账户修改失败," + jData["msghd_rspmsg"].ToString() + "";
                        lblMsg.Visible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMsg.InnerText = "结算账户修改失败," + ex.Message + "";
                lblMsg.Visible = true;
            }
        }
        else
        {
            try
            {
                Hi.Model.PAY_Withdrawals withdrawals = new Hi.Model.PAY_Withdrawals();
                withdrawals.DisID = this.DisID;
                withdrawals.CompID = this.CompID;
                withdrawals.State = 2;
                withdrawals.OpenAccID = openAcc[0].ID;
                withdrawals.AccNo = txtAccNo.Value.Trim();
                withdrawals.AccNm = openAcc[0].AccName;
                withdrawals.AccTp = seleAccTp.Value.ToInt(0);
                withdrawals.CrsMk = seleCrsMk.Value.ToInt(0);
                withdrawals.OpenBkCd = Boxbank.BankId;
                withdrawals.PrcCd = selePrcCd.Value.ToInt(0);
                withdrawals.CityCd = hdCityCd.Value.ToInt(0);
                withdrawals.OpenBkNm = txtBkNm.Value.Trim();
                withdrawals.OpenBkAddr = txtBkAddr.Value.Trim();
                withdrawals.ts = DateTime.Now;
                withdrawals.modifyuser = this.UserID;
                //结算账户接口参数赋值
                WWP.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");
                WWP.srl_ptnsrl = Common.Number_repeat("1");
                withdrawals.vdef1 = WWP.srl_ptnsrl;
                WWP.cltacc_cltno = openAcc[0].AccNumver;
                WWP.cltacc_cltnm = openAcc[0].AccName;
                WWP.bkacc_accno = withdrawals.AccNo;
                WWP.bkacc_accnm = withdrawals.AccNm;
                WWP.bkacc_acctp = withdrawals.AccTp.ToString();
                WWP.bkacc_crsmk = withdrawals.CrsMk.ToString();
                WWP.bkacc_openbkcd = withdrawals.OpenBkCd;
                WWP.bkacc_openbknm = withdrawals.OpenBkNm;
                WWP.bkacc_prccd = withdrawals.PrcCd.ToString();
                WWP.bkacc_citycd = withdrawals.CityCd.ToString();
                WWP.bkacc_openbkaddr = withdrawals.OpenBkAddr;
                WWP.fcflg = "1";
                string Result = IPT.trd12000(new JavaScriptSerializer().Serialize(WWP));
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
                        withdrawals.State = 1;
                        if (new Hi.BLL.PAY_Withdrawals().Add(withdrawals) != 0)
                        {
                            Response.Redirect("SettlementInfo.aspx", true);
                        }
                    }
                    else
                    {
                        lblMsg.InnerText = "结算账户新增失败," + jData["msghd_rspmsg"].ToString() + "";
                        lblMsg.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.InnerText = "结算账户新增失败," + ex.Message + "";
                lblMsg.Visible = true;
            }
        }
    }
}