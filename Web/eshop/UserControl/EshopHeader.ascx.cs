using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;

public partial class EShop_UserControl_EshopHeader : System.Web.UI.UserControl
{
    public string txt_GoodsSearchId
    {
        get { return txt_GoosName.ClientID; }
    }

    public bool NoGetUserList
    {
        get { return HeaderTop.NoGetUserList; }
        set { HeaderTop.NoGetUserList = value; }
    }

    public List<Hi.Model.BD_Company> ComList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IsLogin();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        int value;
        if (!int.TryParse(Request["Comid"], out value))
        {
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
        ComList = new Hi.BLL.BD_Company().GetList("id,FirstBanerImg,CompName,CompLogo,Principal,Phone,Address,ManageInfo,BrandInfo,QQ ", " id='" + Request["Comid"] + "' and dr=0 and AuditState=2 and IsEnabled=1  ", "");
        if (ComList.Count == 0)
        {
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
        txt_GoosName.Attributes["placeholder"] = "欢迎进入" + ComList[0].CompName;
    }

    public void IsLogin()
    {
        LoginModel loginuser = HttpContext.Current.Session["UserModel"] as LoginModel;
        ImgBigCompQRCode.Src = ConfigurationManager.AppSettings["ImgViewPath"] + "CompImg/qr_" + Request["Comid"].Trim() + ".jpg";
        ImgSmCompQRCode.Src = ConfigurationManager.AppSettings["ImgViewPath"] + "CompImg/qr_" + Request["Comid"].Trim() + ".jpg";
        if (loginuser != null)
        {
            switch (loginuser.TypeID)
            {
                case 1:
                case 5:
                    RidrctCart.Visible = true;
                    RidrctCart.Attributes.Add("onclick", "location.href='" + ResolveUrl("~/Distributor/UserIndex.aspx") + "'");
                    ; break; //代理商
                case 3:
                case 4:
                case 6:
                    RidrctCart.Visible = true;
                    RidrctCart.Attributes.Add("onclick", "location.href='" + ResolveUrl("~/Company/jsc.aspx") + "'");
                    break;   //企业
                default: ; break;
            }
        }
        if (Request.Url.ToString().IndexOf("index.aspx") == -1)
        {
            Nav_AllGoods.Attributes["href"] = ResolveUrl("/" + Request["Comid"]+".html");
        }
        if (!string.IsNullOrEmpty(Request["GoodsName"]))
        {
            txt_GoosName.Value = Request["GoodsName"].Trim();
        }
        if (Request.Url.ToString().IndexOf("index.aspx") > -1)
        {
            HoverGoods.Attributes.Add("class", "hover");
        }
        //else if (Request.Url.ToString().IndexOf("productsview.aspx") > -1)
        //{
        //    HoverAllGoods.Attributes.Add("class", "hover");
        //}
        else if (Request.Url.ToString().IndexOf("productsview.aspx") == -1)
        {
            HoverCompNew.Attributes.Add("class", "hover");
        }
    }

    protected void Click_DownUrl(object sender, EventArgs e)
    {
        string BrowerName = Request.Browser.Browser;
        string filePath = Server.MapPath("~/UserControl/") + "平台.url";
        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            if (BrowerName == "Firefox")
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
            }
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
    }

}