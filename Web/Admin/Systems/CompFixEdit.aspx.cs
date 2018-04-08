using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;

public partial class Admin_Systems_CompFixEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            comp.Tel = Common.NoHTML(txtTel.Value.Trim());
            comp.Zip = Common.NoHTML(txtZip.Value.Trim());
            comp.Fax = Common.NoHTML(txtFax.Value.Trim());
            comp.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
            comp.Address = Common.NoHTML(txtAddress.Value.Trim());
            comp.CompLogo = Common.NoHTML(HDCompPath.Value);
            comp.ShopLogo = Common.NoHTML(HdShopLogoPath.Value);
            comp.CompNewLogo = Common.NoHTML(HdNewShopLogoPath.Value);
            comp.CustomCompinfo = txtCustomInfo.Text;
            if (txtPhone.Value.Trim() == "")
            {
                List<Hi.Model.SYS_CompUser> User2 = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and utype=4 and CompID=" + KeyID + "", "");
                if (User2.Count > 0)
                {
                    Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(User2[0].UserID);
                    if (User != null)
                    {
                        comp.Phone = User.Phone;
                    }
                }
                //List<Hi.Model.SYS_Users> User = new Hi.BLL.SYS_Users().GetList("", " dr=0 and type=4 and CompID=" + KeyID + "", "");
                //if (User.Count > 0)
                //{
                //    comp.Phone = User[0].Phone;
                //}
            }
            else
            {
                comp.Phone = Common.NoHTML(txtPhone.Value.Trim());
            }
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                Response.Redirect("CompFixture.aspx?KeyID=" + KeyID);
            }

        }
    }


    public void DataBinds()
    {

        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp != null)
        {
            //if (Request["back"] == "0")
            //{
            //    btnback.Visible = false;
            //}
            txtTel.Value = comp.Tel;
            txtZip.Value = comp.Zip;
            txtFax.Value = comp.Fax;
            txtPrincipal.Value = comp.Principal;
            txtPhone.Value = comp.Phone;
            if (string.IsNullOrWhiteSpace(comp.CustomCompinfo))
            {
                txtCustomInfo.Text = "本公司产品种类丰富、质量优良、价格公道、服务周到。感谢您长期的支持与厚爱，您的满意是我们最高的追求，我们将竭诚为您提供优质、贴心的服务！";
            }
            else
                txtCustomInfo.Text = comp.CustomCompinfo;
            txtAddress.Value = comp.Address;
            ImgCompLOGO.Src = comp.CompLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.CompLogo : "";
            ImgShopLogo.Src = comp.ShopLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.ShopLogo : "";
            ImgNewShopLogo.Src = comp.CompNewLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + comp.CompNewLogo : "";
            if (comp.CompLogo != "")
            {
                DivCompLogo.Visible = false;
            }
            else
            {
                CompLogoI.Visible = true;
                ImgCompLOGO.Style.Add("display", "none"); 
            }
            if (comp.ShopLogo != "")
            {
                DivShopLogo.Visible = false;
            }
            else
            {
                ImgShopLogo.Style.Add("display", "none");
            }

            if (comp.CompNewLogo != "")
            {
                DivNewShopLogo.Visible = false;
            }
            else
            {
                ImgNewShopLogo.Style.Add("display", "none");
            }

            spImgName.InnerText = comp.CompName;
            spShoplogoName.InnerText = comp.CompName;
            spNewShoplogoName.InnerText = comp.CompName;
            HDCompPath.Value = comp.CompLogo;
            HdShopLogoPath.Value = comp.ShopLogo;
            HdNewShopLogoPath.Value = comp.CompNewLogo;
            string[] PathArry = comp.FirstBanerImg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string OuterHtml = "";
            foreach (string PathV in PathArry)
            {
                OuterHtml += "<li><img src=\"" + ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + PathV + "\" style=\"margin: 5px 0px 5px 5px; border:1px solid ;  width: 628px; height: 148px;float:left; \"><a href=\"JavaScript:;\" class=\"delImg2\" tip=\"" + PathV + "\">删除</a></li>";
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
    }
}