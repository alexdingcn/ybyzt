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

public partial class Financing_AcrossTradeInMoney :DisPageBase
{
    List<Hi.Model.PAY_OpenAccount> LPayOpen;
    List<Hi.Model.PAY_Withdrawals> Lwithdrawals;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
       
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
                decimal money = Jdata["amt_balamt"].ToString().ToDecimal(0)/100;
                SPAcBalance.InnerText = "￥"+money.ToString("0.00");
            }
        }
        catch (Exception ex)
        {
            SPAcBalance.InnerText = "￥"+"0.00";
        }
    }
}