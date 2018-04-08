using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

public partial class news : LoginPageBase
{

    public string page = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindNewsList();
                switch (Request["NewsType"]) {
                    case "1": LiNews_1.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "行业新闻"; break;
                    case "3": LiNews_3.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "资讯"; break;
                    case "4": LiNews_4.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "生意经"; break;
                    case "2": LiNews_2.Attributes["class"] = "flbt hover"; Inewstext.InnerText = "公告"; break;
                }

               //操作日志统计开始
               Utils.WritePageLog(Request, "新闻中心");
              //操作日志统计结束

            }
        }
        catch (Exception ex)
        {
            Response.Redirect( ConfigurationManager.AppSettings["WebDomainName"].ToString());
        }
    }
    public string GetNewsInfo(string NewsInfo)
    {
        if (NewsInfo!=null)
        {

            if (NewsInfo.Length < 40)
            {
                return NewsInfo;
            }
            else
            {
                return NewsInfo.Substring(0, 40) + "....";
            }
        }
        else
        {
            return "";
        }
       
    }
    #region  *********** 新闻公告*************
    public void BindNewsList()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.SYS_NewsNotice> NewLsit = new Hi.BLL.SYS_NewsNotice().GetList(Pager_List.PageSize, Pager_List.CurrentPageIndex, "CreateDate", true, " and  isnull(dr,0)=0 " + SqlWhrere() + " ", out pageCount, out Counts);
        Rpt_News.DataSource = NewLsit;
        Rpt_News.DataBind();
        Pager_List.RecordCount = Counts;
        Pager_List.TextBeforePageIndexBox = "<i class='tf2'>共" + Pager_List.PageCount + "页</i> <span class='tf2'>到第:</span>";
        page = Pager_List.CurrentPageIndex.ToString();
    }

    protected void PagerList_PageChanged(object sender, EventArgs e)
    {
        page = Pager_List.CurrentPageIndex.ToString();
        BindNewsList();
    }

    public string SqlWhrere()
    {
        string Wherer = "";
        if (!string.IsNullOrEmpty(Request["NewsType"]))
        {
            Wherer += " and NewsType =" + Request["NewsType"].ToInt(0) + "";
        }
        return Wherer;
    }


    public static string NoHTML(string Htmlstring,object info)
    {
        if (info != null && info.ToString() != "")
        {
            return info.ToString().Length > 140 ? info.ToString().Substring(0, 140) + "...." : info.ToString();
        }
        else
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            Htmlstring = Htmlstring.Length > 140 ? Htmlstring.Substring(0, 140) + "...." : Htmlstring;
            return Htmlstring;
        }
    }  

    #endregion

    protected override void Render(HtmlTextWriter writer)
    {
        StringWriter strWriter = new StringWriter();
        base.Render(new HtmlTextWriter(strWriter));
        writer.Write(
            strWriter.ToString().
            Replace("alert(\"页索引超出范围！\")","layerCommon.msg(\"页码超出范围\",IconOption.错误)").
            Replace("alert(\"页索引不是有效的数值！\")", "layerCommon.msg(\"输入的页码不是数值类型\",IconOption.错误)")
            );
    }

    public string GetTitle()
    {
        int NewsType = Convert.ToInt16(Request["NewsType"]);
        switch (NewsType)
        {
            case 1:
                return "行业新闻-医站通新闻资讯";
            case 3:
                return "资讯-医站通新闻资讯";
            case 4:
                return "生意经-医站通新闻资讯";
            case 2:
                return "公告-医站通新闻资讯";
            default:
                return "新闻资讯-行业新闻、资讯、生意经、公告-医站通";
             
        }

    }

}