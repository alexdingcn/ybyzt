using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DBUtility;
using System.Configuration;

public partial class Distributor_DealerHead : System.Web.UI.UserControl
{
    //登录信息
    public LoginModel logUser = null;
    public bool bol = false;
    public Hi.Model.BD_Distributor dis = null;
    //public Hi.Model.BD_Company com = null;
    public string ShowName = "";
    public string logo = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session != null)
        {
            logUser = Session["UserModel"] as LoginModel;
            dis = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);
            //com = new Hi.BLL.BD_Company().GetModel(logUser.CompID);
            //if (com != null)
            //{
            //    string lg = com.CompLogo;
            //    logo = lg == "" ? "../Config/image/logo8.jpg" : System.Configuration.ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" + lg;
            //}
            if (logUser.Url != null)
            {
                //罗汉
                if (logUser.Url.IndexOf("lhhome") != -1)
                {
                    bol = true;
                }
                //酒隆仓
                if (logUser.Url.IndexOf("jlc") != -1)
                {
                    bol = true;
                }
            }
            Hi.Model.SYS_Users sysuser = new Hi.BLL.SYS_Users().GetModel(logUser.UserID);
            if (sysuser.TrueName != null && sysuser.TrueName.ToString() != "")
                ShowName = sysuser.TrueName.ToString();
            else
                ShowName = sysuser.UserName.ToString();

            if (ShowName.Length > 6)
                ShowName = ShowName.Substring(0, 6) + "...";
        }

        DataTable dt = new Hi.BLL.DIS_ShopCart().GetGoodsCart(" sc.[CompID]=" + logUser.CompID + " and sc.[DisID]=" + logUser.DisID + "and sc.dr=0", "sc.[CreateDate] desc ");
        string cart = "";
        int i = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            //查询购物车商品数量、总价
            DataTable dtsum = new Hi.BLL.DIS_ShopCart().SumCartNum(logUser.CompID.ToString(), logUser.DisID.ToString());
            if (dtsum != null && dtsum.Rows.Count > 0)
                this.Top_CartNum.InnerText = dtsum.Rows[0]["cart"].ToString().ToDecimal(0).ToString("0");
            foreach (DataRow item in dt.Rows)
            {
                i++;
                if (i > 4)
                {
                    cart += "<div class=\"border\"><span>购物车还有<span id=\"num\">" + (dtsum.Rows[0]["cart"].ToString().ToDecimal(0) - 4) + "</span>个商品</span><a class=\"cklink\" href=\"" + ResolveUrl("../Distributor/Shop.aspx") + "\">去购物车</a></div>";
                    break;
                }
                cart += "<i class=\"GoGoodsInfo\" goods_tip=\"" + item["GoodsID"] + "\" tip=\"" + item["GoodsinfoID"] + "\">" +Common.MySubstring(item["GoodsName"].ToString(),15,"...") + "<span class=\"goodsnum\"> " + item["GoodsNum"].ToString().ToDecimal(0).ToString("0") + "</span><span>x</span></i>";
            }
        }
        cart = !string.IsNullOrEmpty(cart) ? cart : "<i tip=\"title\">购物车中还没有商品，赶紧选购吧！</i>";
        this.tgnCar.InnerHtml = cart;
    }

    protected void Click_DownUrl(object sender, EventArgs e)
    {
        string BrowerName = Request.Browser.Browser;
        string filePath = Server.MapPath("../UserControl/") + "平台.url";
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
            //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
    }

    //判断是否为管理员登录
    public bool IsDisAdmin(int ID)
    {
        string sql = "select id from SYS_Users where isnull(dr,0)=0 and type=5 and ID=" + ID;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            return true;
        else
            return false;
    }

    protected void QuitLogin_Click(object sender, EventArgs e)
    {
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
        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
        if (bol)
        {
            //酒隆仓
            //Response.Redirect("http://jlc.my1818.com", true);
        }
        else
        {
            Response.Redirect( ConfigurationManager.AppSettings["WebDomainName"].ToString(), true);
        }
    }
}