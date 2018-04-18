using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using FinancingReferences;
using System.Configuration;
using System.Text;

public partial class Financing_AcrossTradeOutMoney :DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    List<Hi.Model.PAY_OpenAccount> LPayOpen;
    List<Hi.Model.PAY_Withdrawals> Lwithdrawals;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
        lblErr.InnerHtml = "";
        //user = LoginModel.IsLogined(this);
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack) {
            DataMoney();
        }
    }

    public void DataMoney() {
        try
        {
            LPayOpen = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and DisID=" + this.DisID + "", "");
            Lwithdrawals = new Hi.BLL.PAY_Withdrawals().GetList("", " isnull(dr,0)=0  and DisID=" + this.DisID, "");
            if (LPayOpen.Count > 0 && Lwithdrawals.Count > 0)
            {
                IPubnetwk IPT = new IPubnetwk();
                StringBuilder Sjson = new StringBuilder();
                Sjson.Append("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + Common.Number_repeat("1") + "\",");
                Sjson.Append("\"cltacc_cltno\":\"" + LPayOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LPayOpen[0].AccName + "\",");
                Sjson.Append("\"bkacc_accno\":\"" + Lwithdrawals[0].AccNo + "\",\"bkacc_accnm\":\"" + LPayOpen[0].AccName + "\"}");
                string json = IPT.trd13010(Sjson.ToString());
                JsonData Jdata = JsonMapper.ToObject(json);
                decimal money = Jdata["amt_balamt"].ToString().ToDecimal(0) / 100;
                SPAcBalance.InnerText = "￥" + money.ToString("0.00");
            }
        }
        catch (Exception ex)
        {
            SPAcBalance.InnerText = "￥" + "0.00";
        }
    }

    protected void btn_save(object sender, EventArgs e)
    {
        string guid = Common.Number_repeat("1");
        Hi.Model.PAY_Financing Fin = new Hi.Model.PAY_Financing();
        Fin.DisID = dis.ID;
        Fin.CompID = dis.CompID;
        Fin.State = 2;
        List<Hi.Model.PAY_OpenAccount> LOpen = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + Fin.DisID + " and State=1 and dr=0", "");
        if (LOpen.Count > 0)
        {
            Fin.OpenAccID = LOpen[0].ID;
        }
        else
        {
            this.lblErr.InnerHtml = "请先添加开销账户！";
            return;
        }
        Fin.OrderID = 0;
        Fin.AclAmt = Convert.ToDecimal(txtPrice.Value);
        Fin.Guid = guid;
        Fin.Type = 2;
        Fin.CcyCd = "CNY";
        Fin.vdef1 = txtRemark.Value;
        Fin.ts = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        Fin.modifyuser = this.UserID;
        List<Hi.Model.PAY_Withdrawals> Lwith = new Hi.BLL.PAY_Withdrawals().GetList("", "DisID=" + Fin.DisID + " and state=1 and isnull(dr,0)=0", "");
        if (Lwith.Count == 0)
        {
            this.lblErr.InnerHtml = "请先维护结算账户！";
            return;
        }
        if (Util.md5(txtPayPwd.Value.Trim()) != dis.Paypwd)
        {
            this.lblErr.InnerHtml = "支付密码错误！";
            return;
        }
        int result = 0;
        result = new Hi.BLL.PAY_Financing().Add(Fin);
        if (result > 0)
        {
            String msghd_rspcode = "";
            String msghd_rspmsg = "";
            String Json = "";
            try
            {
                IPubnetwk ipu = new IPubnetwk();
                Json = ipu.trd16000("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + guid + "\",\"cltacc_cltno\":\"" + LOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LOpen[0].AccName + "\",\"amt_aclamt\":\"" + (int)(Convert.ToDecimal(txtPrice.Value) * 100) + "\"}");
            }
            catch
            {
                this.lblErr.InnerHtml = "接口连接失败！";
                return;
            }
            try
            {
                JsonData Params = JsonMapper.ToObject(Json);
                msghd_rspcode = Params["msghd_rspcode"].ToString();
                msghd_rspmsg = Params["msghd_rspmsg"].ToString();
            }
            catch { }
            if ("000000".Equals(msghd_rspcode))
            {
                try
                {
                    Hi.Model.PAY_Financing finaM = new Hi.BLL.PAY_Financing().GetModel(result);
                    finaM.State = 1;
                    finaM.ts = DateTime.Now;
                    new Hi.BLL.PAY_Financing().Update(finaM);
                }
                catch { }
                Response.Redirect("FinancingDetailList.aspx");
            }
            else
            {
                this.lblErr.InnerHtml = msghd_rspmsg;
                return;
            }
        }
    }
}