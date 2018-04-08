using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_NewsInfo : AdminPageBase
{
    public string title = "";
    public string content = "";
    public int newstype = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_NewsNotice NewsNotice = new Hi.BLL.SYS_NewsNotice().GetModel(KeyID);

            //lblnewstitle.InnerText = NewsNotice.NewsTitle;
            title = NewsNotice.NewsTitle;
            //lblcontent.InnerText = NewsNotice.NewsContents;
            if (NewsNotice.KeyWords != null)
                this.lblKeywords.Text = NewsNotice.KeyWords;
            if (NewsNotice.NewsInfo.Length < 100)
            {
                lblNewsInfo.Text = NewsNotice.NewsInfo;
            }
            else
            {
                lblNewsInfo.Text=  NewsNotice.NewsInfo.Substring(0, 100)+"....";
            }
            
            string pat = "<[^<>]+>";
            Regex regex = new Regex(pat, RegexOptions.None);
            content = regex.Replace(NewsNotice.NewsContents, "");
            lblstate.InnerText = NewsNotice.IsEnabled == 1 ? "发布" : "非发布";
            lblistop.InnerText = NewsNotice.IsTop == 1 ? "置顶" : "非置顶";
            newstype = NewsNotice.NewsType;
            if (newstype == 1)
            {
                lblnewtype.InnerHtml = "新闻";
            }
            else if (newstype == 2)
            {
                lblnewtype.InnerHtml = "公告";
            }
            else if (newstype == 2)
            {
                lblnewtype.InnerHtml = "资讯";
            }
            else
            {
                lblnewtype.InnerHtml = "生意经";
            }

            content7.Text = NewsNotice.NewsContents;
               
        }
        else
        {
            Response.Write("信息不存在。");
            Response.End();
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_NewsNotice NewsNotice = new Hi.BLL.SYS_NewsNotice().GetModel(KeyID);
        if (NewsNotice != null)
        {
            NewsNotice.dr = 1;
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            if (new Hi.BLL.SYS_NewsNotice().Update(NewsNotice))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='NewsList.aspx'; }");
                Response.Redirect("NewsList.aspx");
            }
        }
    }
}