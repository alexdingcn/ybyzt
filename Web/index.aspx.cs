using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Hi.BLL;
using System.Configuration;

public partial class index : LoginPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "No-Cache");
            TopNav1.IsIndex = true;
            if (true) //!IsPostBack  每次加载执行
            {
                IsLogin();
            }
            if (!IsPostBack)
            {
                //操作日志统计开始
                Utils.WritePageLog(Request, "首页");
                //操作日志统计结束
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            //捕捉线程终止异常   不处理
        }
        catch (Exception ex)
        {
            Session.Clear();
            Session.Abandon();
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString());
        }
    }

    #region  *********** 登录*************
    /// <summary>
    /// 判断是否登录，显示登录框
    /// </summary>
    public void IsLogin()
    {
        try
        {
            //判断是否登录
            if (LoginModel.IsLoginAll())
            {
                //隐藏登录注册框
                index_Dvlogin.Visible = false;
                index_mNotice.Style.Add("height", "240px");
                index_mNotice.Style.Add("padding-top", "10px;");
            }
            else
            {
                index_mNotice.Attributes.Remove("style");
            }


            //公告
            List<Hi.Model.SYS_NewsNotice> ListNes = new Hi.BLL.SYS_NewsNotice().GetList(" top 6 ID,NewsTitle,CreateDate ", " dr=0 and NewsType=2 and IsEnabled=1 ", " createdate desc ");
            Rpt_News.DataSource = ListNes;
            Rpt_News.DataBind();

            //行业新闻
            List<Hi.Model.SYS_NewsNotice> ListNes1 = new Hi.BLL.SYS_NewsNotice().GetList(" top 6 ID,NewsTitle,CreateDate ", " dr=0 and NewsType in(1,3) and IsEnabled=1 ", " createdate desc ");
            Rpt_News1.DataSource = ListNes1;
            Rpt_News1.DataBind();
        }
        catch (Exception)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString());
        }
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        Response.Redirect(ConfigurationManager.AppSettings["WebDomainName"].ToString());
    }
    #endregion



    //    #region  *********** 新闻公告*************
    //    public void GetNewsList()
    //    {
    //        List<Hi.Model.SYS_NewsNotice> AllNewNotice = null;
    //        if (HttpRuntime.Cache.Get("AllNewNotice") == null)
    //        {
    //            AllNewNotice = new Hi.BLL.SYS_NewsNotice().QueryGroupNew("top 3 id,NewsType,NewsTitle,createdate");
    //            HttpRuntime.Cache.Insert("AllNewNotice", AllNewNotice, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
    //        }
    //        else
    //        {
    //            AllNewNotice = HttpRuntime.Cache["AllNewNotice"] as List<Hi.Model.SYS_NewsNotice>;
    //        }

    //        //行业新闻   
    //        List<Hi.Model.SYS_NewsNotice> LANewsNotice = AllNewNotice.Where(T => T.NewsType == 1).ToList();
    //        this.Rpt_News_1.DataSource = LANewsNotice;
    //        this.Rpt_News_1.DataBind();
    //        // 小陌咨询
    //        List<Hi.Model.SYS_NewsNotice> NewsList2 = AllNewNotice.Where(T => T.NewsType == 3).ToList();
    //        this.Rpt_News_2.DataSource = NewsList2;
    //        this.Rpt_News_2.DataBind();
    //        // 
    //        List<Hi.Model.SYS_NewsNotice> NewsList4 = AllNewNotice.Where(T => T.NewsType == 4).ToList();
    //        this.Rpt_News_4.DataSource = NewsList4;
    //        this.Rpt_News_4.DataBind();
    //    }
    //    #endregion

    //    //精品品牌 加‘招商’推广 
    //    public string GetClass(int id)
    //    {
    //        //暂时屏蔽 add by hgh  1202
    //        return "";
    //        //List<int> listID = new List<int>() { 1213, 1060, 1173, 1151, 1127, 1119, 1123 };
    //        //if (listID.Contains(id))
    //        //{
    //        //    return "<i class='adhot'></i>";
    //        //}
    //        //else
    //        //{
    //        //    return "";
    //        //}
    //    }


    //    /// <summary>
    //    /// 截取地区
    //    /// </summary>
    //    /// <param name="Add"></param>
    //    /// <returns></returns>
    //    public string GetAdd(string Add)
    //    {
    //        if (Add.Contains("省") && Add.Contains("市"))
    //        {
    //            string[] Shen = Add.Split('省');
    //            string[] Shi = Shen[1].Split('市');
    //            if (Shi[0].ToString().Length > 6)
    //            {
    //                Shi[0] = Shi[0].ToString().Substring(0, 6) + "..";
    //            }
    //            return Shen[0].ToString() + "-" + Shi[0].ToString();
    //        }
    //        else if (Add.Contains("省") && !Add.Contains("市"))
    //        {
    //            string[] Shen = Add.Split('省');
    //            return Shen[0].ToString();
    //        }
    //        else if (Add.Contains("市") && Add.Contains("区") && !Add.Contains("省"))
    //        {
    //            string[] Shen = Add.Split('市');
    //            string[] Shi = Shen[1].Split('区');
    //            if (Shi[0].ToString().Length > 6)
    //            {
    //                Shi[0] = Shi[0].ToString().Substring(0, 6) + "..";
    //            }
    //            return Shen[0].ToString() + "-" + Shi[0].ToString();
    //        }
    //        else if (Add.Contains("市") && !Add.Contains("区") && !Add.Contains("省"))
    //        {
    //            string[] Shen = Add.Split('市');
    //            return Shen[0].ToString();
    //        }
    //        else
    //        {
    //            if (Add.Length < 6)
    //            {
    //                return Add.ToString();
    //            }
    //            else
    //            {
    //                return Add.Substring(0, 6) + "..";
    //            }
    //        }
    //    }

}

