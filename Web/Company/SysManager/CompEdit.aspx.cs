using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;

public partial class Company_SysManager_CompEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();

            if (Request["nextstep"] == "1")
            {
                //返回按钮不可用
                btnback.Visible = false;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            comp.Tel =Common.NoHTML( txtTel.Value.Trim());
            comp.Fax = Common.NoHTML(txtFax.Value.Trim());
            comp.Principal = Common.NoHTML(txtPrincipal.Value.Trim());
            comp.Address = Common.NoHTML(txtAddress.Value.Trim());
            if (txtPhone.Value.Trim() != "")
                comp.Phone = Common.NoHTML(txtPhone.Value.Trim());
            else
            {
                List<Hi.Model.SYS_CompUser> User2 = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and utype=4 and CompID=" + CompID + "", "");
                if (User2.Count > 0)
                {
                    Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(User2[0].UserID);
                    if (User != null)
                    {
                        comp.Phone = Common.NoHTML(User.Phone);
                    }
                }
            }
            comp.BrandInfo = Common.NoHTML(this.BrandIfon.Value.Trim());
            comp.CompLogo = Common.NoHTML(HDCompPath.Value);
            comp.CustomCompinfo = txtCustomInfo.Text;
            if (new Hi.BLL.BD_Company().Update(comp))
            {
                if (Request["nextstep"] + "" == "1")
                {
                    JScript.AlertMethod(this, "恭喜您成功入驻医站通，您的商城您做主，尽情享受您的电商之旅吧！", JScript.IconOption.笑脸, "function () { onlinkOrder('../newOrder/orderBuy.aspx', 'dkxd') }");
                }
                else
                {
                    Response.Redirect("CompInfo.aspx");
                }                
            }

        }
    }

    public void DataBinds()
    {

        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            if (Request["back"] == "0")
            {
                btnback.Visible = false;
            }
            txtTel.Value = comp.Tel;
            txtFax.Value = comp.Fax;
            txtPrincipal.Value = comp.Principal;
            this.BrandIfon.Value = comp.BrandInfo;
            if (comp.CustomCompinfo==null || comp.CustomCompinfo == "")
            {
                txtCustomInfo.Text = "本公司产品种类丰富、质量优良、价格公道、服务周到。感谢您长期的支持与厚爱，您的满意是我们最高的追求，我们将竭诚为您提供优质、贴心的服务！";
            }
            else
                txtCustomInfo.Text = comp.CustomCompinfo;
            txtAddress.Value = comp.Address;
            ImgCompLOGO.Src = comp.CompLogo != "" ? ConfigurationManager.AppSettings["ImgViewPath"] + "../CompImage/" + comp.CompLogo : "";
            if (comp.CompLogo != "")
            {
                ImgCompName.Visible = false;
            }
            else
            {
                CompLogoI.Visible = true;
                ImgCompLOGO.Style.Add("display", "none");
            }
            txtPhone.Value = comp.Phone;
            spImgName.InnerText = comp.CompName;
            HDCompPath.Value = comp.CompLogo;
        }
        else {
            Response.Write("未找到数据");
            Response.End();
        }
    }

}