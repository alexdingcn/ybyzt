using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Company_Financing_SpendingUserInfo : CompPageBase{
    
    public List<Hi.Model.PAY_OpenAccount> LPayOpen;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["IsFinancing"] != "1")
        {
            Response.Redirect("../NoOperable.aspx", true);
            Response.End();
        }
        if (!IsPostBack)
        {
            DataBindS();
        }
    }

    public void DataBindS()
    {
        LPayOpen = new Hi.BLL.PAY_OpenAccount().GetList("", " isnull(dr,0)=0 and CompID=" + CompID + " and DisID=0", "");
        if (LPayOpen.Count > 0)
        {
            lblAccNumver.InnerText = LPayOpen[0].AccNumver;
            lblAccName.InnerText = LPayOpen[0].AccName;
            lblAccountName.InnerText = LPayOpen[0].AccountName;
            lblAccountNature.InnerText = Enum.GetName(typeof(Enums.AccountNature), LPayOpen[0].AccountNature);
            lblDocumentType.InnerText = Enum.GetName(typeof(Enums.CertificatesNature), LPayOpen[0].DocumentType.ToInt(0));
            lblDocumentCode.InnerText = LPayOpen[0].DocumentCode;
            lblOrgCode.InnerText = LPayOpen[0].OrgCode;
            lblBusinessLicense.InnerText = LPayOpen[0].BusinessLicense;
            lblSex.InnerText = Enum.GetName(typeof(Enums.Sex), LPayOpen[0].Sex);
            lblNationality.InnerText = LPayOpen[0].Nationality == "CHN" ? "中国" : LPayOpen[0].Nationality;
            lblPhoneNumbe.InnerText = LPayOpen[0].PhoneNumbe;
            lblPhone.InnerText = LPayOpen[0].Phone;
            lblFax.InnerText = LPayOpen[0].Fax;
            lblPostCode.InnerText = LPayOpen[0].Postcode;
            lblAccAddress.InnerText = LPayOpen[0].AccAddress;
            lblState.InnerHtml = LPayOpen[0].State == (int)Enums.Pay_OpenState.成功 ? Enum.GetName(typeof(Enums.Pay_OpenState), LPayOpen[0].State) : "<i style='color:red;'>失败</i>";
        }
        else
        {
            Response.Redirect("SpendingUserAdd.aspx");
        }
    }
}