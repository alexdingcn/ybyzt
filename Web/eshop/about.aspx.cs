using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EShop_about : LoginPageBase
{
    public string shopname = "";//名称
    List<Hi.Model.BD_Company> ComList = new List<Hi.Model.BD_Company>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindComp();
        }
    }

    public void DataBindComp()
    {
        if (!string.IsNullOrWhiteSpace(ComList[0].CustomCompinfo))
        {
            DivContent.InnerHtml = ComList[0].CustomCompinfo;
        }
        else
        {
            DivContent.InnerHtml = ComList[0].CompInfo;
        }
         LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;

         if (logUser != null)
         {
             lblAddress.InnerHtml = "地　址：" + ComList[0].Address;
             if (!string.IsNullOrWhiteSpace(ComList[0].Principal))
             {
                 lblPrincipal.InnerHtml = "联系人：" + ComList[0].Principal;
             }
             if (!string.IsNullOrWhiteSpace(ComList[0].Phone))
             {
                 lblPhone.InnerHtml = "电　话：" + ComList[0].Phone;
             }
             lbllogin.Visible = false;

             List<Hi.Model.SYS_CompUser> User2 = new Hi.BLL.SYS_CompUser().GetList("", "isnull(dr,0)=0 and IsEnabled=1 and utype=4  and CompID=" + ViewState["Compid"], "");
             if (User2.Count > 0)
             {
                 Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(User2[0].UserID);
                 if (User != null)
                 {
                     if (!string.IsNullOrWhiteSpace(ComList[0].Principal))
                     {
                         lblPrincipal.InnerHtml = "联系人：" + ComList[0].Principal;
                     }
                     else
                     {
                         lblPrincipal.InnerHtml = "联系人：" + User.TrueName;
                     }
                     if (!string.IsNullOrWhiteSpace(ComList[0].Phone))
                     {
                         lblPhone.InnerHtml = "电　话：" + ComList[0].Phone;
                     }
                     else
                     {
                         lblPhone.InnerHtml = "电　话：" + User.Phone;
                     }
                 }
             }
         }
         else
         {
             lblPrincipal.InnerHtml = "<i>联系人：</i>" + "***";
             lblPhone.InnerHtml = "<i>电　话：</i>" + "***";
             lblAddress.InnerHtml = "<i>地　址：</i>" + "***";
             lbllogin.InnerHtml = "<a href=\"/login.html\"><i  style=\" color:Red;\">请先登录>></i></a>";
         }
    }

    protected override void OnInit(EventArgs e)
    {
        int value;
        if (!int.TryParse(Request["Comid"], out value))
        {
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
        ComList = new Hi.BLL.BD_Company().GetList("id,CompName,CustomCompinfo,CompInfo,Principal,Phone,Fax,Zip,address", " id='" + Request["Comid"] + "' and dr=0 and IsEnabled=1 and AuditState=2  ", "");
        if (ComList.Count == 0)
        {
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
        shopname = ComList[0].CompName;
        mKeyword.Content = ComList[0].CompInfo;
        ViewState["Compid"] = ComList[0].ID;
    }

}