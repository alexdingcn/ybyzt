using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class UserControl_TopControl : System.Web.UI.UserControl
{
    public LoginModel user = null;
    public int Comp = 0;
    public bool NoGetUserList { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!NoGetUserList)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Key", "<script>GeAccountUserList(undefined);</script>");
        }
        GtypeBind();
    }

    protected override void OnInit(EventArgs e)
    {
        try
        {
            if (!(HttpContext.Current.Session["UserModel"] is LoginModel))
            {
                HttpCookie cookie = Request.Cookies["loginmodel"];
                if (cookie != null)
                {
                    string token = Util.md5(cookie.Value + "yibanmed.com");
                    HttpCookie cookie2 = Request.Cookies["token"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Value == token)
                        {
                            HttpCookie cookie4 = new HttpCookie("login_state", "1");
                            cookie4.Expires = DateTime.Now.AddDays(30);
                            cookie4.HttpOnly = true;
                            Response.Cookies.Add(cookie4);
                            HttpCookie cookie3 = Request.Cookies["login_type"];
                            if (cookie3 == null)
                                cookie3 = Request.Cookies["type"];
                            List<Hi.Model.SYS_Users> l = new Hi.BLL.SYS_Users().GetList("", "isnull(dr,0)=0 and isenabled=1 and username='" + cookie.Value + "'", "");
                            if (cookie3.Value == "")
                            {
                                return;
                            }
                            Hi.Model.SYS_CompUser model = new Hi.BLL.SYS_CompUser().GetModel(cookie3.Value.ToInt(0));
                            LoginModel umodel = new LoginModel();

                            if (model.CType == 1)
                            {
                                Hi.Model.BD_Company model2 = new Hi.BLL.BD_Company().GetModel(model.CompID);
                                umodel.CompName = model2.CompName;
                                umodel.Erptype = model2.Erptype;
                            }
                            else
                            {
                                Hi.Model.BD_Distributor model3 = new Hi.BLL.BD_Distributor().GetModel(model.DisID);
                                umodel.DisName = model3.DisName;
                            }
                            string loginUrl = string.Empty;
                            if (Request.UrlReferrer == null)
                            {
                                loginUrl = "index.html";
                            }
                            else
                            {
                                loginUrl = Request.UrlReferrer.PathAndQuery;
                                if (loginUrl == "/")
                                {
                                    loginUrl = "index.html";
                                }
                                else
                                {
                                    loginUrl = loginUrl.Replace("/", "");
                                }
                            }
                            umodel.Url = loginUrl;
                            umodel.UserName = l[0].UserName;
                            umodel.TrueName = l[0].TrueName;
                            umodel.UserID = l[0].ID;
                            umodel.TypeID = model.UType;
                            umodel.Ctype = model.CType;
                            umodel.CompID = model.CompID;
                            umodel.DisID = model.DisID;
                            umodel.Phone = l[0].Phone;
                            umodel.CUID = Common.DesEncrypt(model.ID.ToString(), Common.EncryptKey);
                            umodel.IsPhoneLogin = false;
                            Session.Remove("UserModel");
                            Session["UserModel"] = umodel;
                            Utils.EditLog("安全日志", l[0].UserName, "用户" + l[0].UserName + "自动登录管理系统成功。", "系统安全模块", loginUrl, 0, 1, model.UType);
                        }
                    }
                }
            }
            this.mlogin.Visible = false;
            this.A_QuitLogin.Visible = false;
            this.userSw.Visible = false;
            this.disTop.Visible = false;
            this.compTop.Visible = false;
            user = Session["UserModel"] as LoginModel;
            if (user != null)
            {
                Comp = user.CompID;
                this.A_QuitLogin.Visible = true;
                this.userSw.Visible = true;
                this.username.InnerText = user.TrueName != "" ? user.TrueName : user.UserName;
                switch (user.TypeID)
                {
                    case 1:
                    case 5:
                        //代理商
                        this.disTop.Visible = true;
                        this.compname.InnerText = user.DisName == null ? new Hi.BLL.BD_Distributor().GetModel(user.DisID).DisName : user.DisName;
                        BindStoreCart(user.DisID, user.CompID);
                        break;
                    case 3:
                    case 4:
                    case 6:
                        //厂商
                        this.compTop.Visible = true;
                        this.Headjx.Attributes.Add("class", "hxGray-i");
                        this.compname.InnerText = user.CompName;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.mlogin.Visible = true;
                this.disTop.Visible = true;
                TopE_StoreCart.InnerHtml = "<a href=\"javascript:void(0);\"><i class=\"name\" tip=\"title\">购物车没有商品，赶紧选购吧！</i></a>";
            }
        }
        catch { }
    }

    public void BindStoreCart(int DisID, int Compid)
    {
        DataTable ListCart = new Hi.BLL.DIS_ShopCart().GetList("GoodsName,bg.id,GoodsinfoID,GoodsNum ", "DIS_ShopCart cart join BD_Goods bg on cart.GoodsID=bg.ID join BD_GoodsInfo bginfo on cart.GoodsinfoID=bginfo.ID", " cart.dr=0  and cart.DisID=" + DisID + " ", " cart.createdate desc");
        decimal goodsNum = 0;
        DataTable dtsum = new Hi.BLL.DIS_ShopCart().SumCartNum(Compid.ToString(), DisID.ToString());
        if (dtsum != null && dtsum.Rows.Count > 0)
            goodsNum = dtsum.Rows[0]["cart"].ToString().ToDecimal(0);
        this.TopE_CartNum.InnerText = goodsNum.ToString();
        if (ListCart.Rows.Count == 0)
        {
            TopE_StoreCart.InnerHtml = "<a href=\"javascript:void(0);\"><i class=\"name\" tip=\"title\">购物车中还没有商品，赶紧选购吧！</i></a>";
        }
        else
        {
            string CartHTML = "";
            int index = 0;
            foreach (DataRow row in ListCart.Rows)
            {
                index++;

                if (index > 4)
                {
                    CartHTML += "<div class=\"border\"><span>&nbsp;购物车还有<span class='red' id=\"num\">" + (ListCart.Rows.Count - 4) + "</span></span>个商品<a class\"cklink\" style='float:right' href=\"../Distributor/Shop.aspx\">去购物车</a></div>";
                    break;
                }
                CartHTML += "<a target='_blank' Infoid='" + row["GoodsinfoID"] + "' href=\"/e" + row["GoodsinfoID"] + "_" + Compid + "_.html\"><i class=\"GoGoodsInfo name\" title='" + row["GoodsName"] + "'>" + (row["GoodsName"].ToString().Length > 15 ? row["GoodsName"].ToString().Substring(0, 15) + "..." : row["GoodsName"].ToString()) + "</i><span class=\"goodsnum num\">x" + row["GoodsNum"].ToString().ToDecimal(0).ToString("#") + "</span></a>";
            }
            TopE_StoreCart.InnerHtml = CartHTML;
        }
    }

    protected void QuitLogin_Click(object sender, EventArgs e)
    {
        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
        if (Context.Request.Cookies["loginmodel"] != null)
        {
            HttpCookie cookie = new HttpCookie("loginmodel");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["token"] != null)
        {
            HttpCookie cookie = new HttpCookie("token");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_type"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_type");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        if (Context.Request.Cookies["login_state"] != null)
        {
            HttpCookie cookie = new HttpCookie("login_state");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
        }
        Session.Remove("UserModel");
        Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
    }

    public void GtypeBind()
    {
        List<Hi.Model.SYS_GType> list = new Hi.BLL.SYS_GType().GetList("", " dr=0 and IsEnabled=1 and Deep=1", "");
        this.Gtype.DataSource = list;
        this.Gtype.DataBind();

    }

}