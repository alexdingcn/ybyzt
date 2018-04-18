using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Financing_SettlementInfo : DisPageBase
{

    //public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
          //  user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            if (!IsPostBack)
            {
                DataBindS();
            }
        }
    }

    public void DataBindS()
    {
        List<Hi.Model.PAY_Withdrawals> Lwithdrawals = new Hi.BLL.PAY_Withdrawals().GetList("", " isnull(dr,0)=0  and DisID=" + this.DisID, "");
        if (Lwithdrawals.Count > 0)
        {
            lblState.InnerHtml = Lwithdrawals[0].State == (int)Enums.Pay_OpenState.成功 ? Enum.GetName(typeof(Enums.Pay_OpenState), Lwithdrawals[0].State) : "<i style='color:red;'>失败</i>";
            object Accname;
            if ((Accname = Common.GetOpenAccountValue(Lwithdrawals[0].OpenAccID, "AccNumver")) != null)
            {
                lblAccNumver.InnerText = Accname.ToString();
            }
            string ProName = string.Empty;
            string CityName = string.Empty;
            List<Hi.Model.BF_ZD_PROVINCE> BPro = new Hi.BLL.BF_ZD_PROVINCE().GetList("", " PROVCODE='" + Lwithdrawals[0].PrcCd + "'", "");
            List<Hi.Model.BF_ZD_CITY> BCity = new Hi.BLL.BF_ZD_CITY().GetList("", " CITYCODE='" + Lwithdrawals[0].CityCd + "'", "");
            lblAccNo.InnerText = Lwithdrawals[0].AccNo;
            lblAccNm.InnerText = Lwithdrawals[0].AccNm;
            lblAccTp.InnerText = Enum.GetName(typeof(Enums.Pay_AccTp), Lwithdrawals[0].AccTp);
            lblCrsMk.InnerText = Enum.GetName(typeof(Enums.Pay_CrsMk), Lwithdrawals[0].CrsMk);
            lblOpenBkCd.InnerText = Lwithdrawals[0].OpenBkCd;
            lblPrcCd.InnerText = BPro.Count > 0 ? BPro[0].PROVNAME : "";
            lblCityCd.InnerText = BCity.Count > 0 ? BCity[0].CITYNAME : "";
            lblOpenBkNm.InnerText = Lwithdrawals[0].OpenBkNm;
            lblOpenBkAddr.InnerText = Lwithdrawals[0].OpenBkAddr;
            lblCcyCd.InnerText = Lwithdrawals[0].CcyCd;
        }
        else
        {
            Response.Redirect("SettlementAdd.aspx", true);
        }
    }
}