using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Company_SysManager_CompInfo : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBinds();
    }

    public void DataBinds() {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null) {
            //lblCompName.InnerText = comp.CompName;
            //lblIndusName.Id = comp.IndID.ToString();
            //lblCompCode.InnerText = comp.CompCode;
            //lblInfo.InnerText = comp.ManageInfo;
            lblFax.InnerText = comp.Fax;
            lblTel.InnerText = comp.Tel;
            this.BrandInfo.InnerText = comp.BrandInfo;
            //lblPhone.InnerText = comp.Phone;
            lblPhone.InnerText = comp.Phone;
            lblPrincipal.InnerText = comp.Principal;
            lblAddress.InnerText = comp.Address;
            //lblCompInfo.InnerText = comp.CompInfo;
            ImgCompLogo.Src = comp.CompLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "../CompImage/" + comp.CompLogo : "";
            div_CustomInfo.InnerHtml = comp.CustomCompinfo;
        }
    }
}