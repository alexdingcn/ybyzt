using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class newsInfo : LoginPageBase
{
    public string title = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sy();
            xy();
            DataBindInfo();
        }
    }


    #region  *********** 新闻公告详情*************
    public void DataBindInfo()
    {
        List<Hi.Model.SYS_NewsNotice> ListNews = new Hi.BLL.SYS_NewsNotice().GetList("KeyWords,NewsTitle,NewsContents,NewsType,CreateDate", " isnull(dr,0)=0 and id=" + Request["Newsid"] + " ", "");
        if (ListNews.Count > 0)
        {
            HNewsTitle.InnerText = ListNews[0].NewsTitle;
            title = ListNews[0].NewsTitle;
            DivNewsContens.InnerHtml = ListNews[0].NewsContents;
            mKeyword.Content = ListNews[0].KeyWords;
            DivDateTime.InnerHtml = "发布日期：" + Convert.ToDateTime(ListNews[0].CreateDate).ToString("yyyy-MM-dd");
            switch (ListNews[0].NewsType.ToString())
            {
                case "1": LiNews_1.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "行业新闻"; break;
                case "3": LiNews_3.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "资讯"; break;
                case "4": LiNews_4.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "生意经"; break;
                case "2": LiNews_2.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "公告"; break;
            }
        }
    }


    #endregion
    //上一篇
    protected void s_ServerClick(object sender, EventArgs e)
    {
        int page=Convert.ToInt32( Request["Newsid"]);
        int[] intarr = Count(page, "s");
        if (intarr[0] > 0)
        {
            Response.Redirect("newsinfo_" + intarr[1] + ".html");
        }
        else
        {
            Response.Redirect("newsinfo_" + page + ".html");
        }
    }
    //下一篇
    protected void x_ServerClick(object sender, EventArgs e)
    {
        int page = Convert.ToInt32(Request["Newsid"]);
        int[] intarr = Count(page, "x");
        if (intarr[0] > 0)
        {
            Response.Redirect("newsinfo_" + intarr[1] + ".html");
        }
        else
        {
            Response.Redirect("newsinfo_" + page + ".html");
        }
    }
    public string sy()
    {
        int page = Convert.ToInt32(Request["Newsid"]);
      int[] intarr=  Count(page,"s");
        if (intarr[0] > 0)
        {
            Panel1.Visible = true;
            return "上一篇";
        }
        else
        {
            Panel1.Visible = false;
            return "已是首篇";
        }
    }
    public string xy()
    {
        int page = Convert.ToInt32(Request["Newsid"]);
        int[] intarr = Count(page, "x");
        if (intarr[0] > 0)
        {
            Panel2.Visible = true;
            return "下一篇";
        }
        else
        {
            Panel2.Visible = false;
            return "已是末篇";
        }
    }
    public int[] Count(int page, string type)
    {
        if (type == "s")
        {
            List<Hi.Model.SYS_NewsNotice> List = new Hi.BLL.SYS_NewsNotice().GetList("NewsTitle,NewsContents,NewsType", " isnull(dr,0)=0 and id=" + page + " ", "");
            string preSql = " ID < " + page + " and NewsType='" + List[0].NewsType + "'";
            List<Hi.Model.SYS_NewsNotice> ListNews = new Hi.BLL.SYS_NewsNotice().GetList("top 1 *", preSql, " ID desc");
            int[] intarr = new int[2];
            intarr[0] = ListNews.Count;
            if (intarr[0] > 0)
            {
                intarr[1] = ListNews[0].ID;
            }
            else
            {
                intarr[1] = page;
            }

            return intarr;
        }
        else
        {
            List<Hi.Model.SYS_NewsNotice> List = new Hi.BLL.SYS_NewsNotice().GetList("NewsTitle,NewsContents,NewsType", " isnull(dr,0)=0 and id=" + page + " ", "");
            string nextSql = " ID > " + page + " and NewsType='" + List[0].NewsType + "'";
            List<Hi.Model.SYS_NewsNotice> ListNews = new Hi.BLL.SYS_NewsNotice().GetList("top 1 *", nextSql, " ID ASC");
            int[] intarr = new int[2];
            intarr[0] = ListNews.Count;
            if (intarr[0] > 0)
            {
                intarr[1] = ListNews[0].ID;
            }
            else
            {
                intarr[1] = page;
            }
            return intarr;
        }
    }
}