using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Admin_Systems_CompFixture : AdminPageBase
{
    public string contents = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBinds();
    }
    public void DataBinds()
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            lblPhone.InnerText = comp.Phone;
            lblFax.InnerText = comp.Fax;
            lblTel.InnerText = comp.Tel;
            lblZip.InnerText = comp.Zip;
            lblPrincipal.InnerText = comp.Principal;
            lblAddress.InnerText = comp.Address;
            ImgShopLogo.Src = comp.ShopLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.ShopLogo : "";
            ImgCompLogo.Src = comp.CompLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.CompLogo : "";
            ImgNewShopLogo.Src = comp.CompNewLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.CompNewLogo : "";
            string[] PathArry = comp.FirstBanerImg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string OuterHtml = "";
            foreach (string PathV in PathArry)
            {
                OuterHtml += "<li><img src=\"" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/"+PathV + "\" style=\"margin: 5px 0px 5px 5px; border:1px solid ;  width: 628px; height: 148px;float:left; \"></li>";
            }
            contents = comp.CustomCompinfo;
            contents = contents.Replace("<span", "<p").Replace("</span", "</p");
            if (UserType == 3 || UserType == 4)
            {
                if (comp.OrgID != OrgID)
                {
                    Response.Write("数据不存在");
                    Response.End();
                }
            }
        }
    }
}